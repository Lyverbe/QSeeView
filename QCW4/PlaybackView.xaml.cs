using NetSDKCS;
using QCW4.Properties;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace QCW4
{
    public partial class PlaybackView : Window
    {
        private PlaybackViewModel _viewModel;
        private IList<RecordFileInfo> _records;
        private IntPtr _loginId;
        private bool _reinitializePlayer;
        private int _playIndex;
        private double _currentSpeed;

        public PlaybackView(IList<RecordFileInfo> records, int playIndex, IntPtr loginId)
        {
            InitializeComponent();

            _viewModel = new PlaybackViewModel();
            DataContext = _viewModel;

            PlaybackPictureBox.Refresh();
            PlaybackPictureBox.BorderStyle = BorderStyle.FixedSingle;
            PlaybackPictureBox.Size = PlaybackViewModel.HDSize;
            PlaybackPictureBox.Refresh();

            _records = records;
            _playIndex = playIndex;
            _loginId = loginId;

            _viewModel.Close += (s, e) => Close();
            _viewModel.PlaybackTimeChanged += ViewModel_PlaybackTimeChanged;
            _viewModel.PreviousVideo += (s, e) => PlayPreviousVideo();
            _viewModel.NextVideo += (s, e) => PlayNextVideo();
            _viewModel.ReduceSpeed += (s, e) => ReduceSpeed();
            _viewModel.IncreaseSpeed += (s, e) => IncreaseSpeed();
        }

        private void OnLoaded(object sender, RoutedEventArgs e) => InitializePlayback();

        protected override void OnClosed(EventArgs e)
        {
            _viewModel.Stop();
            base.OnClosed(e);
        }

        private void InitializePlayback()
        {
            _currentSpeed = 1.0;

            SetLandscapeMode();
            _viewModel.RefreshPlaybackImageSize();
            SetResolution();
            UpdateTitle();
            StartPlayback(_records[_playIndex].Source.starttime);
            _viewModel.Start();

            Left = 0;
            Top = 0;
        }

        private void StartPlayback(NET_TIME startTime)
        {
            var stream = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(int)));
            Marshal.StructureToPtr((int)EM_STREAM_TYPE.MAIN, stream, true);
            NETClient.SetDeviceMode(_loginId, EM_USEDEV_MODE.RECORD_STREAM_TYPE, stream);

            var inputInfo = new NET_IN_PLAY_BACK_BY_TIME_INFO();
            var outputInfo = new NET_OUT_PLAY_BACK_BY_TIME_INFO();
            inputInfo.stStartTime = startTime;
            inputInfo.stStopTime = _records[_playIndex].Source.endtime;
            inputInfo.hWnd = PlaybackPictureBox.Handle;
            inputInfo.cbDownLoadPos = null;
            inputInfo.dwPosUser = IntPtr.Zero;
            inputInfo.fDownLoadDataCallBack = null;
            inputInfo.dwDataUser = IntPtr.Zero;
            inputInfo.nPlayDirection = 0;
            inputInfo.nWaittime = 0;

            _viewModel.PlaybackID = NETClient.PlayBackByTime(_loginId, (int)_records[_playIndex].Channel, inputInfo, ref outputInfo);
            _viewModel.IsPaused = false;
        }

        private void SetLandscapeMode()
        {
            switch (_records[_playIndex].Channel)
            {
                case 0: _viewModel.IsLandscape = Settings.Default.IsChannel1Landscape; break;
                case 1: _viewModel.IsLandscape = Settings.Default.IsChannel2Landscape; break;
                case 2: _viewModel.IsLandscape = Settings.Default.IsChannel3Landscape; break;
                case 3: _viewModel.IsLandscape = Settings.Default.IsChannel4Landscape; break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void SetResolution()
        {
            double HDRatio = PlaybackViewModel.HDSize.Width / (double)PlaybackViewModel.HDSize.Height;
            if (_viewModel.IsLandscape)
            {
                Width = 1100;
                Height = (Width / HDRatio);
            }
            else
            {
                Width = 400;
                Height = (Width * HDRatio);
            }
        }

        private void ViewModel_PlaybackTimeChanged(object sender, EventArgs e)
        {
            if (_viewModel.IsMouseDown && !_reinitializePlayer)
            {
                _viewModel.Stop();
                _reinitializePlayer = true;
            }
        }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            _viewModel.IsMouseDown = true;

            _reinitializePlayer = false;
            base.OnPreviewMouseLeftButtonUp(e);
        }

        protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            _viewModel.IsMouseDown = false;

            if (_reinitializePlayer)
            {
                System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    var time = new DateTime((long)_viewModel.PlaybackSliderValue);
                    var deviceTime = NET_TIME.FromDateTime(time);
                    StartPlayback(deviceTime);
                }));
            }

            base.OnPreviewMouseLeftButtonUp(e);
        }

        private void UpdateTitle()
        {
            var newTitle = "Playback - " + _records[_playIndex].FileName;
            if (_currentSpeed != 1.0)
            {
                newTitle += $" - {_currentSpeed}x";
            }

            Title = newTitle;
        }

        protected override void OnPreviewKeyDown(System.Windows.Input.KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    {
                        PlayPreviousVideo();
                        e.Handled = true;
                        break;
                    }
                case Key.Right:
                    {
                        PlayNextVideo();
                        e.Handled = true;
                        break;
                    }
                case Key.Up:
                    {
                        IncreaseSpeed();
                        e.Handled = true;
                        break;
                    }
                case Key.Down:
                    {
                        ReduceSpeed();
                        e.Handled = true;
                        break;
                    }
                case Key.Space:
                    {
                        if (_viewModel.IsPaused)
                            _viewModel.Play();
                        else
                            _viewModel.Pause();
                        break;
                    }
            }

            base.OnPreviewKeyDown(e);
        }

        private void ReduceSpeed()
        {
            if (_currentSpeed > 0.125)
            {
                NETClient.PlayBackControl(_viewModel.PlaybackID, PlayBackType.Slow);
                _currentSpeed /= 2;
                UpdateTitle();
            }
        }

        private void IncreaseSpeed()
        {
            if (_currentSpeed < 8.0)
            {
                NETClient.PlayBackControl(_viewModel.PlaybackID, PlayBackType.Fast);
                _currentSpeed *= 2;
                UpdateTitle();
            }
        }

        private void PlayPreviousVideo()
        {
            if (_playIndex > 0)
            {
                _playIndex--;
                _viewModel.Stop();
                InitializePlayback();
            }
        }

        private void PlayNextVideo()
        {
            if (_playIndex + 1 < _records.Count)
            {
                _playIndex++;
                _viewModel.Stop();
                InitializePlayback();
            }
        }
    }
}
