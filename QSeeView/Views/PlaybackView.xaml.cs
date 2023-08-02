using NetSDKCS;
using QSeeView.Models;
using QSeeView.Tools;
using QSeeView.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Media;

namespace QSeeView.Views
{
    public partial class PlaybackView : Window
    {
        private IDeviceManager _deviceManager;
        private PictureBox _pictureBox;
        private PlaybackViewModel _viewModel;
        private IList<RecordFileInfoModel> _records;
        private bool _reinitializePlayer;
        private int _playIndex;
        private double _currentSpeed;

        public PlaybackView(IDeviceManager deviceManager, IList<RecordFileInfoModel> records, int playIndex)
        {
            InitializeComponent();

            _deviceManager = deviceManager;
            _records = records;
            _playIndex = playIndex;

            _currentSpeed = 1.0;

            _viewModel = new PlaybackViewModel();
            DataContext = _viewModel;

            _viewModel.Close += (s, e) => Close();
            _viewModel.PlaybackTimeChanged += ViewModel_PlaybackTimeChanged;
            _viewModel.PreviousVideo += (s, e) => PlayPreviousVideo();
            _viewModel.NextVideo += (s, e) => PlayNextVideo();
            _viewModel.ReduceSpeed += (s, e) => ReduceSpeed();
            _viewModel.IncreaseSpeed += (s, e) => IncreaseSpeed();
            _viewModel.SetPlaybackControl += (s, command) => _deviceManager.PlaybackControl(_viewModel.PlaybackID, command);
            _viewModel.UpdateSlider += ViewModel_UpdateSlider;

            Loaded += PlaybackView_Loaded;
            ContentRendered += (s, e) => InitializePlayback();
        }

        private void PlaybackView_Loaded(object sender, EventArgs e)
        {
            _pictureBox = FindPictureBox(this);
            _pictureBox.BorderStyle = BorderStyle.FixedSingle;
            _pictureBox.Refresh();
        }

        protected override void OnClosed(EventArgs e)
        {
            _viewModel.Stop();
            base.OnClosed(e);
        }

        public static PictureBox FindPictureBox(DependencyObject dependencyObj)
        {
            if (dependencyObj != null)
            {
                for (var childId = 0; childId < VisualTreeHelper.GetChildrenCount(dependencyObj); childId++)
                {
                    var child = VisualTreeHelper.GetChild(dependencyObj, childId);
                    if (child != null && child is WindowsFormsHost)
                        return ((WindowsFormsHost)child).Child as PictureBox;

                    var pictureBox = FindPictureBox(child);
                    if (pictureBox != null)
                        return pictureBox;
                }
            }
            return null;
        }

        private void InitializePlayback()
        {
            _viewModel.IsLandscape = App.Settings.ChannelsInfo[(int)_records[_playIndex].Channel].IsLandscape;
            _viewModel.RefreshPlaybackImageSize();

            InitializeSlider();
            StartPlayback(_records[_playIndex].Source.starttime);

            if (App.Settings.IsResettingPlaybackSpeed)
                _currentSpeed = 1.0;
            else
                ResumeSpeed();
            UpdateTitle();
        }

        private void StartPlayback(NET_TIME startTime)
        {
            _viewModel.PlaybackID = _deviceManager.StartPlayback(startTime, _records[_playIndex].Source.endtime, _records[_playIndex].Channel, _pictureBox.Handle);
            _viewModel.Start();
        }

        private void ViewModel_PlaybackTimeChanged(object sender, EventArgs e)
        {
            if (_viewModel.IsPaused)
                _reinitializePlayer = true;
        }

        protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            if (_reinitializePlayer)
            {
                _viewModel.Stop();
                System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    var time = new DateTime((long)_viewModel.PlaybackSliderValue);
                    var deviceTime = NET_TIME.FromDateTime(time);
                    StartPlayback(deviceTime);
                }));
                _reinitializePlayer = false;
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
                            _viewModel.PlayCommand.Execute(null);
                        else
                            _viewModel.PauseCommand.Execute(null);
                        break;
                    }
            }

            base.OnPreviewKeyDown(e);
        }

        private void ReduceSpeed()
        {
            if (_currentSpeed > 0.125)
            {
                _deviceManager.PlaybackControl(_viewModel.PlaybackID, PlayBackType.Slow);
                _currentSpeed /= 2;
                UpdateTitle();
            }
        }

        private void IncreaseSpeed()
        {
            if (_currentSpeed < 8.0)
            {
                _deviceManager.PlaybackControl(_viewModel.PlaybackID, PlayBackType.Fast);
                _currentSpeed *= 2;
                UpdateTitle();
            }
        }

        private void PlayPreviousVideo()
        {
            if (_playIndex > 0)
            {
                _viewModel.Stop();
                _playIndex--;
                InitializePlayback();
            }
        }

        private void PlayNextVideo()
        {
            if (_playIndex + 1 < _records.Count)
            {
                _viewModel.Stop();
                _playIndex++;
                InitializePlayback();
            }
        }

        private void InitializeSlider()
        {
            _viewModel.PlaybackSliderMinimum = _records[_playIndex].StartTime.Ticks;
            _viewModel.PlaybackSliderMaximum = _records[_playIndex].EndTime.Ticks;
            _viewModel.SliderLargeChange = (_viewModel.PlaybackSliderMaximum - _viewModel.PlaybackSliderMinimum) / 10;
        }

        private void ViewModel_UpdateSlider(object sender, EventArgs e)
        {
            var currentTick = _deviceManager.GetPlayBackOsdTick(_viewModel.PlaybackID);
            if (currentTick.HasValue)
            {
                if (currentTick < _viewModel.PlaybackSliderMinimum)
                    _viewModel.PlaybackSliderMinimum = currentTick.Value;

                _viewModel.PlaybackSliderValue = currentTick.Value;
            }
        }

        private void ResumeSpeed()
        {
            var catchupSpeed = 1.0;
            while (_currentSpeed > catchupSpeed)
            {
                _deviceManager.PlaybackControl(_viewModel.PlaybackID, PlayBackType.Fast);
                catchupSpeed *= 2;
            }
            while (_currentSpeed < catchupSpeed)
            {
                _deviceManager.PlaybackControl(_viewModel.PlaybackID, PlayBackType.Slow);
                catchupSpeed /= 2;
            }
        }
    }
}
