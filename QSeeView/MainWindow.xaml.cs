using QSeeView.Models;
using QSeeView.Tools;
using QSeeView.Types;
using QSeeView.Views;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Media;
using System.Reflection;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
        private GridViewColumnHeader _lastColumnClicked;
        private Timer _autoQueryTimer;

        public MainWindow(IDeviceManager deviceManager)
        {
            InitializeComponent();

            _deviceManager = deviceManager;
            _downloader = new Downloader(deviceManager);
            _queryManager = new QueryManager(deviceManager);

            _viewModel = new MainWindowViewModel();
            DataContext = _viewModel;

            _viewModel.PlayRecord += (s, record) => PlaybackRecord(record);
            _viewModel.ShowSettings += ViewModel_ShowSettings;
            _viewModel.Query += (s, e) => Query();
            _viewModel.ShowLiveView += (s, e) => ShowLiveView();
            _viewModel.StartDownload += ViewModel_StartDownload;
            _viewModel.StopDownload += ViewModel_StopDownload;
            _viewModel.FilterChannels += ViewModel_FilterChannels;
            _viewModel.Close += (s, e) => Close();
            _viewModel.ExportQuery += ViewModel_ExportQuery;
            _viewModel.HardDisksInfo += ViewModel_HardDisksInfo;
            _viewModel.ApplyDateOffset += ViewModel_ApplyDateOffset;
            _viewModel.Logout += ViewModel_Logout;

            _deviceManager.DownloadCompleted += DeviceManager_DownloadCompleted;

            _downloader.DownloadStarted += Downloader_DownloadStarted;
            _downloader.DownloadError += Downloader_DownloadError;
            _downloader.DownloadsCompleted += Downloader_DownloadsCompleted;

            _autoQueryTimer = new Timer();
            _autoQueryTimer.Elapsed += (s, e) => Application.Current.Dispatcher.BeginInvoke(new Action(() => Query()));

            ContentRendered += MainWindow_ContentRendered;

            var assemblyName = Assembly.GetExecutingAssembly().GetName();
            var name = assemblyName.Name;
            var version = assemblyName.Version.ToString();
            Title = $"{name} v{version}";

            SubscribeListVisibilityChanged();
            ClearStatusBar();
            UpdateAutoQueryParameters();
        }

        private void MainWindow_ContentRendered(object sender, EventArgs e)
        {
            var commandLineArgs = Environment.GetCommandLineArgs();
            var lowercaseCommandLineArgs = commandLineArgs.ToList().ConvertAll(arg => arg.ToLower());
            if (lowercaseCommandLineArgs.Any(arg => arg.StartsWith("-live")))
            {
                Hide();
                ShowLiveView();
                Close();
            }
            else if (App.Settings.IsAutoQueryAtStartup)
                Query();
        }

        protected override void OnClosed(EventArgs e)
        {
            App.Settings.WindowRect = new Rect(Left, Top, Width, Height);
            _deviceManager.Shutdown();
            base.OnClosed(e);
        }

        protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            if (_filterChannelsView != null && _filterChannelsView.IsVisible)
                _filterChannelsView.CloseIfAllowed();
            base.OnPreviewMouseLeftButtonUp(e);
        }

        protected override void OnPreviewMouseDoubleClick(MouseButtonEventArgs e)
        {
            var listView = e.Source as ListView;
            if (listView != null && !IsGridViewColumnHeader(e.OriginalSource as DependencyObject))
            {
                var record = listView.SelectedValue as RecordFileInfoModel;
                if (record != null)
                    PlaybackRecord(record);
            }

            base.OnPreviewMouseDoubleClick(e);
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (_viewModel.SelectedRecord != null && (e.Key == Key.Enter || e.Key == Key.Space))
                PlaybackRecord(_viewModel.SelectedRecord);
            base.OnPreviewKeyDown(e);
        }

        private void PlaybackRecord(RecordFileInfoModel record)
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

        private void UpdateAutoQueryParameters()
        {
            if (App.Settings.AutoQuerySeconds.HasValue)
            {
                _autoQueryTimer.Interval = App.Settings.AutoQuerySeconds.Value * 1000;
                _autoQueryTimer.Start();
            }
            else
                _autoQueryTimer.Stop();
        }

        private void Query()
        {
            if (_viewModel.EndDateTime <= _viewModel.StartDateTime)
            {
                MessageBox.Show("End date must be greater than start date", "Query");
                return;
            }

            Mouse.OverrideCursor = Cursors.Wait;
            _queryManager.Run(_viewModel.StartDateTime, _viewModel.EndDateTime);
            _viewModel.Records = _queryManager.FilterResult();
            Mouse.OverrideCursor = null;

            if (_viewModel.Records.Count >= _deviceManager.MaxQueryRecords)
                MessageBox.Show($"Limit of {_deviceManager.MaxQueryRecords} records has been reached. Some files may be missing.", "Query");

            _viewModel.CheckAll = App.Settings.IsAutoSelectAtQuery;
        }

        private void ViewModel_ShowSettings(object sender, EventArgs e)
        {
            var view = new SettingsView()
            {
                Owner = this
            };
            var isOkClicked = view.ShowDialog();
            if (isOkClicked == true)
            {
                _viewModel.Records?.ToList().ForEach(record => record.RefreshChannelNames());
                ClearStatusBar();   // Update for HDD space warning
                if (view.RefreshQuery)
                    Query();
                UpdateAutoQueryParameters();
            }
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

            var recordsToDownload = _viewModel.Records.Where(record => record.IsSelected).ToList();
            if (recordsToDownload.Any())
            {
                _downloader.StartDownloads(recordsToDownload);
                _viewModel.State = StateType.Downloading;
                _viewModel.TotalDownloadCount = _downloader.PendingDownloads.Count();
                _viewModel.UpdateDownloadButtonTooltip();
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
            _viewModel.TaskbarProgressValue = 0;
            _viewModel.UpdateDownloadButtonTooltip();
        }

        /// <summary>
        /// Callback when a single download of a batch is completed
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
            ClearStatusBar();
            _viewModel.TaskbarProgressValue = 0;
            _viewModel.UpdateDownloadButtonTooltip();

            if (App.Settings.IsAutoOpenDownloads)
                Process.Start(App.Settings.DownloadFolder);
            if (App.Settings.DoPlayDownloadsCompleteSound)
                new SoundPlayer(Properties.Resources.DownloadsComplete).Play();
        }

        private void Downloader_DownloadStarted(object sender, RecordFileInfoModel record)
        {
            _viewModel.StatusBarInfo = $"Downloading {record.FileName} ({_downloader.PendingDownloads.Count} remaining)";
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

        private void RecordsListHeader_Click(object sender, RoutedEventArgs e)
        {
            var columnHeader = e.OriginalSource as GridViewColumnHeader;
            if (columnHeader != null)
            {
                var gridView = (sender as ListView)?.View as GridView;
                if (gridView != null)
                {
                    var isAscendingOrder = (columnHeader != _lastColumnClicked);

                    if (columnHeader.Column == DownloadColumn)
                        _viewModel.Records = Sort(record => record.ID, isAscendingOrder).ToList();
                    else if (columnHeader.Column == StartColumn)
                        _viewModel.Records = Sort(record => record.StartTime, isAscendingOrder).ToList();
                    else if (columnHeader.Column == ChannelColumn)
                        _viewModel.Records = Sort(record => record.Channel, isAscendingOrder).ToList();
                    else if (columnHeader.Column == LengthColumn)
                        _viewModel.Records = Sort(record => record.Length, isAscendingOrder).ToList();

                    _lastColumnClicked = isAscendingOrder ? columnHeader : null;
                }
            }
        }

        private IOrderedQueryable<RecordFileInfoModel> Sort<T>(Expression<Func<RecordFileInfoModel, T>> predicate, bool isAscendingOrder)
        {
            var query = _viewModel.Records.AsQueryable();
            return isAscendingOrder ? query.OrderBy(predicate) : query.OrderByDescending(predicate);
        }

        private void ViewModel_ExportQuery(object sender, EventArgs e)
        {
            var dialog = new System.Windows.Forms.SaveFileDialog()
            {
                Title = "Export query",
                Filter = "CSV files|*.csv|All files|*.*",
                OverwritePrompt = true
            };
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var exporter = new QueryExporter();
                var success = exporter.Export(dialog.FileName, _viewModel.Records);
                if (success)
                    MessageBox.Show("Export successful", "Export query");
                else
                    MessageBox.Show("Export failed:\n" + exporter.LastError, "Export query", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ViewModel_HardDisksInfo(object sender, EventArgs e)
        {
            new HardDisksInfoView(_deviceManager)
            {
                Owner = this
            }.ShowDialog();
        }

        private void ClearStatusBar()
        {
            var text = "Ready";
            if (App.Settings.DoShowHddSpaceWarning)
            {
                var hddsInfo = _deviceManager.GetHardDisksInfo();
                if (hddsInfo.Any(hddInfo => (hddInfo.FreeSpace / (double)hddInfo.Capacity) * 100 < App.Settings.HddPercentSpaceWarning))
                    text = $"WARNING: At least one of the hard disk drive is below {App.Settings.HddPercentSpaceWarning}%";
            }

            _viewModel.StatusBarInfo = text;
        }

        private bool IsGridViewColumnHeader(DependencyObject dependencyObject)
        {
            while (dependencyObject != null && !(dependencyObject is GridViewColumnHeader))
                dependencyObject = System.Windows.Media.VisualTreeHelper.GetParent(dependencyObject);
            return (dependencyObject is GridViewColumnHeader);
        }

        private void ViewModel_ApplyDateOffset(object sender, EventArgs e)
        {
            _viewModel.EndDateTime = _viewModel.StartDateTime.AddDays(1);
        }

        private void ViewModel_Logout(object sender, EventArgs e)
        {
            App.Settings.Password = string.Empty;
            Close();
        }
    }
}
