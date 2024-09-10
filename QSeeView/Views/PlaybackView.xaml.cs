﻿using NetSDKCS;
using QSeeView.Models;
using QSeeView.Tools;
using QSeeView.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
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
        private double _currentSpeed;

        public PlaybackView(IDeviceManager deviceManager, IList<RecordFileInfoModel> records, int playIndex)
        {
            InitializeComponent();

            _deviceManager = deviceManager;
            _records = records;

            _currentSpeed = 1.0;

            _viewModel = new PlaybackViewModel(playIndex, records.Count);
            DataContext = _viewModel;

            _viewModel.Close += (s, e) => Close();
            _viewModel.PlaybackTimeChanged += ViewModel_PlaybackTimeChanged;
            _viewModel.PreviousVideo += (s, e) => PlayPreviousVideo();
            _viewModel.NextVideo += (s, e) => PlayNextVideo();
            _viewModel.ReduceSpeed += (s, e) => ReduceSpeed();
            _viewModel.IncreaseSpeed += (s, e) => IncreaseSpeed();
            _viewModel.Replay += (s, e) => Replay();
            _viewModel.SetPlaybackControl += (s, command) => _deviceManager.PlaybackControl(_viewModel.PlaybackID, command);
            _viewModel.UpdateSlider += ViewModel_UpdateSlider;
            _viewModel.SaveSnapshot += ViewModel_SaveSnapshot;
            _viewModel.SelectedInQueryChanged += (s, e) => _records[_viewModel.PlayIndex].IsSelected = _viewModel.IsSelectedInQuery;

            Loaded += PlaybackView_Loaded;
            ContentRendered += (s, e) => InitializePlayback();
            _deviceManager.PlaybackCompleted += DeviceManager_PlaybackCompleted;
        }

        private void PlaybackView_Loaded(object sender, EventArgs e)
        {
            _pictureBox = FindPictureBox(this);
            _pictureBox.Refresh();

            _pictureBox.Click += (s, ee) => _viewModel.IsPaused = !_viewModel.IsPaused;
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
            _viewModel.IsLandscape = App.Settings.ChannelsInfo[(int)_records[_viewModel.PlayIndex].Channel].IsLandscape;
            _viewModel.RefreshPlaybackImageSize();

            InitializeSlider();
            StartPlayback(_records[_viewModel.PlayIndex].Source.starttime);

            if (App.Settings.IsResettingPlaybackSpeed)
                _currentSpeed = 1.0;
            else
                ResumeSpeed();
            UpdateTitle();
            _viewModel.IsSelectedInQuery = _records[_viewModel.PlayIndex].IsSelected;
            _viewModel.RecordLength = _records[_viewModel.PlayIndex].Length.ToString("T");
        }

        private void StartPlayback(NET_TIME startTime)
        {
            _viewModel.PlaybackID = _deviceManager.StartPlayback(startTime, _records[_viewModel.PlayIndex].Source.endtime, (uint)_records[_viewModel.PlayIndex].Channel, _pictureBox.Handle);
            _viewModel.Start();
        }

        private void DeviceManager_PlaybackCompleted(object sender, IntPtr e)
        {
            _viewModel.Stop();
            _viewModel.PlaybackSliderValue = _viewModel.PlaybackSliderMaximum;

            if (_viewModel.IsAutoNextEnabled)
            {
                _viewModel.PlayIndex++;
                if (_viewModel.PlayIndex < _records.Count)
                    System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() => InitializePlayback()));
            }
        }

        private void ViewModel_PlaybackTimeChanged(object sender, EventArgs e)
        {
            if (_viewModel.IsPaused)
                _reinitializePlayer = true;
        }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (e.Source is Slider)
            {
                _viewModel.IsPaused = true;
                _reinitializePlayer = true;
            }
            base.OnPreviewMouseLeftButtonDown(e);
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

        protected override void OnPreviewMouseMove(System.Windows.Input.MouseEventArgs e)
        {
            var sliderControl = e.Source as Slider;
            if (sliderControl != null)
            {
                var mouseSliderPosPercent = e.GetPosition(sliderControl).X / sliderControl.RenderSize.Width;
                var timePos = _viewModel.PlaybackSliderMinimum + ((_viewModel.PlaybackSliderMaximum - _viewModel.PlaybackSliderMinimum) * mouseSliderPosPercent);
                _viewModel.SliderToolTip = new DateTime((long)timePos).ToString("T");
            }

            base.OnPreviewMouseMove(e);
        }

        protected override void OnPreviewKeyDown(System.Windows.Input.KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    Close();
                    break;
                case Key.Left:
                    PlayPreviousVideo();
                    e.Handled = true;
                    break;
                case Key.Right:
                    PlayNextVideo();
                    e.Handled = true;
                    break;
                case Key.Up:
                    IncreaseSpeed();
                    e.Handled = true;
                    break;
                case Key.Down:
                    ReduceSpeed();
                    e.Handled = true;
                    break;
                case Key.Space:
                    if (_viewModel.IsPaused)
                        _viewModel.PlayCommand.Execute(null);
                    else
                        _viewModel.PauseCommand.Execute(null);
                    break;
                case Key.S:
                    _viewModel.IsSelectedInQuery = !_viewModel.IsSelectedInQuery;
                    break;
                case Key.R:
                    Replay();
                    break;
            }

            base.OnPreviewKeyDown(e);
        }

        private void UpdateTitle()
        {
            var newTitle = "Playback - " + _records[_viewModel.PlayIndex].FileName;
            if (_currentSpeed != 1.0)
            {
                newTitle += $" - {_currentSpeed}x";
            }

            Title = newTitle;
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
            if (_viewModel.PlayIndex > 0)
            {
                _viewModel.Stop();
                _viewModel.PlayIndex--;
                InitializePlayback();
            }
        }

        private void PlayNextVideo()
        {
            if (_viewModel.PlayIndex + 1 < _records.Count)
            {
                _viewModel.Stop();
                _viewModel.PlayIndex++;
                InitializePlayback();
            }
        }

        private void InitializeSlider()
        {
            _viewModel.PlaybackSliderMinimum = _records[_viewModel.PlayIndex].StartTime.Ticks;
            _viewModel.PlaybackSliderMaximum = _records[_viewModel.PlayIndex].EndTime.Ticks;
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

        private void Replay()
        {
            _viewModel.Stop();
            InitializePlayback();
        }

        private void ViewModel_SaveSnapshot(object sender, EventArgs e)
        {
            var snapshotTimestamp = new DateTime((long)_viewModel.PlaybackSliderValue);
            var view = new SaveSnapshotSettingsView(snapshotTimestamp, _records[_viewModel.PlayIndex].Channel)
            {
                Owner = this
            };
            var saveRequested = view.ShowDialog();
            if (saveRequested == true)
                _deviceManager.CapturePlaybackPicture(_viewModel.PlaybackID, view.OutputFileName, view.CaptureFormat);
        }
    }
}
