using QSeeView.Models;
using QSeeView.Tools;
using QSeeView.Types;
using QSeeView.Views;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

/*
To-do
- Settings-Channels: Online button
- Download curreny playback video
- Start with live view: Don't show main UI and shutdown app when live view closed
- Open download folder after downloads (settings)
- Play sound when downloads done (settings)
- Sort query content
*/

namespace QSeeView
{
    public partial class MainWindow : Window
    {
        private MainWindowViewModel _viewModel;

        private IDeviceManager _deviceManager;
        private Downloader _downloader;
        private FilterChannelsView _filterChannelsView;
        private Point _filterChannelsViewOffset;
        private QueryManager _queryManager;

        public MainWindow(IDeviceManager deviceManager)
        {
            InitializeComponent();

            _deviceManager = deviceManager;
            _downloader = new Downloader(deviceManager);
            _queryManager = new QueryManager(deviceManager);

            _viewModel = new MainWindowViewModel();
            DataContext = _viewModel;

            _viewModel.PlayRecord += ViewModel_PlayRecord;
            _viewModel.ShowSettings += ViewModel_ShowSettings;
            _viewModel.Query += (s, e) => Query();
            _viewModel.ShowLiveView += (s, e) => ShowLiveView();
            _viewModel.StartDownload += ViewModel_StartDownload;
            _viewModel.StopDownload += ViewModel_StopDownload;
            _viewModel.FilterChannels += ViewModel_FilterChannels;

            _deviceManager.DownloadCompleted += DeviceManager_DownloadCompleted;

            _downloader.DownloadStarted += Downloader_DownloadStarted;
            _downloader.DownloadError += Downloader_DownloadError;
            _downloader.DownloadsCompleted += Downloader_DownloadsCompleted;

            if (App.Settings.IsAutoQueryAtStartup)
                ContentRendered += MainWindow_ContentRendered;
            SubscribeListVisibilityChanged();
        }

        private void MainWindow_ContentRendered(object sender, EventArgs e)
        {
            var commandLineArgs = Environment.GetCommandLineArgs();
            var lowercaseCommandLineArgs = commandLineArgs.ToList().ConvertAll(arg => arg.ToLower());
            if (lowercaseCommandLineArgs.Any(arg => arg.StartsWith("-live")))
                ShowLiveView();

            Query();
        }

        protected override void OnClosed(EventArgs e)
        {
            _deviceManager.Shutdown();
            base.OnClosed(e);
        }

        private void ViewModel_PlayRecord(object sender, RecordFileInfoModel record)
        {
            var playIndex = _viewModel.Records.IndexOf(record);
            var view = new PlaybackView(_deviceManager, _viewModel.Records, playIndex)
            {
                Owner = this
            };
            view.ShowDialog();
        }

        private void SubscribeListVisibilityChanged()
        {
            foreach (var channelInfo in App.Settings.ChannelsInfo)
                channelInfo.ListVisibilityChanged += (s, e) => Query();
        }

        private void Query()
        {
            _queryManager.Run(_viewModel.StartDateTime, _viewModel.EndDateTime);
            _queryManager.FilterResult();
            _viewModel.Records = _queryManager.FilteredResult.ToList();

            if (_viewModel.Records.Count >= _deviceManager.MaxQueryRecords)
                MessageBox.Show($"Limit of {_deviceManager.MaxQueryRecords} records has been reached. Some files may be missing.", "Query");

            _viewModel.CheckAll = true;
        }

        private void ViewModel_ShowSettings(object sender, EventArgs e)
        {
            var view = new SettingsView()
            {
                Owner = this
            };
            var isOkClicked = view.ShowDialog();
            if (isOkClicked == true)
                _viewModel.Records?.ToList().ForEach(record => record.RefreshChannelNames());
        }

        private void ShowLiveView()
        {
            new LiveView(_deviceManager)
            {
                Owner = this
            }.ShowDialog();
        }

        private void ViewModel_StartDownload(object sender, EventArgs e)
        {
            var canStartDownload = CanStartDownload();
            if (!canStartDownload)
                return;

            _viewModel.Records.ToList().ForEach(record => record.ResetProgress());

            _downloader.PendingDownloads = _viewModel.Records.Where(record => record.IsSelected).ToList();
            if (_downloader.PendingDownloads.Any())
            {
                _viewModel.State = StateType.Running;
                _viewModel.TotalDownloadCount = _downloader.PendingDownloads.Count();
            }
        }

        private bool CanStartDownload()
        {
            if (App.Settings.IsConvertingToAvi && !File.Exists(App.Settings.FfmpegPath))
            {
                MessageBox.Show("Path to FFMPEG is invalid.  Check the settings.", Title);
                return false;
            }

            return true;
        }

        private void ViewModel_StopDownload(object sender, EventArgs e)
        {
            _downloader.StopDownload();
            _viewModel.State = StateType.Idle;
        }

        /// <summary>
        /// Callback when a single download is completed
        /// </summary>
        private void DeviceManager_DownloadCompleted(object sender, string errorMessage)
        {
            _viewModel.TaskbarProgressValue = 1.0 - (_downloader.PendingDownloads.Count() / (double)_viewModel.TotalDownloadCount);

            if (!string.IsNullOrEmpty(errorMessage))
            {
                _viewModel.DownloadErrors.Add($"{_deviceManager.DownloadRecord} - {errorMessage}");
                _viewModel.IsErrorSectionVisible = true;
            }
        }

        /// <summary>
        /// Callback when all downloads are completed
        /// </summary>
        private void Downloader_DownloadsCompleted(object sender, EventArgs e)
        {
            _viewModel.State = StateType.Idle;
            _viewModel.StatusBarInfo = "Ready";
            _viewModel.TaskbarProgressValue = 0;
        }

        private void Downloader_DownloadStarted(object sender, RecordFileInfoModel record)
        {
            _viewModel.StatusBarInfo = $"Downloading {record} ({_downloader.PendingDownloads.Count} remaining)";
        }

        private void Downloader_DownloadError(object sender, string message)
        {
            _viewModel.DownloadErrors.Add(message);
            _viewModel.IsErrorSectionVisible = true;
        }

        private void ViewModel_FilterChannels(object sender, Button filterChannelButton)
        {
            if (_filterChannelsView != null && _filterChannelsView.IsVisible)
            {
                _filterChannelsView.Close();
                return;
            }

            var mainWindow = Application.Current.MainWindow;
            var mainWindowLocation = mainWindow.PointToScreen(new Point(0, 0));
            var buttonScreenLocation = filterChannelButton.PointToScreen(new Point(0, 0));
            _filterChannelsViewOffset = new Point()
            {
                X = buttonScreenLocation.X - mainWindowLocation.X + SystemParameters.ResizeFrameVerticalBorderWidth + 4,
                Y = buttonScreenLocation.Y - mainWindowLocation.Y + SystemParameters.ResizeFrameHorizontalBorderHeight + SystemParameters.CaptionHeight + filterChannelButton.ActualHeight + 5
            };

            _filterChannelsView = new FilterChannelsView();
            _filterChannelsView.Owner = Application.Current.MainWindow;
            _filterChannelsView.Left = buttonScreenLocation.X;
            _filterChannelsView.Top = buttonScreenLocation.Y + filterChannelButton.ActualHeight;
            _filterChannelsView.Show();
        }

        protected override void OnLocationChanged(EventArgs e)
        {
            if (_filterChannelsView != null && _filterChannelsView.IsVisible)
            {
                _filterChannelsView.Left = Left + _filterChannelsViewOffset.X;
                _filterChannelsView.Top = Top + _filterChannelsViewOffset.Y;
            }

            base.OnLocationChanged(e);
        }
    }
}
