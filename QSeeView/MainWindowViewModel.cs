using QSeeView.Models;
using QSeeView.Tools;
using QSeeView.Types;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;

namespace QSeeView
{
    partial class MainWindowViewModel : INotifyPropertyChanged
    {
        public event EventHandler<RecordFileInfoModel> PlayRecord;
        public event EventHandler ShowSettings;
        public event EventHandler Query;
        public event EventHandler ShowLiveView;

        public event PropertyChangedEventHandler PropertyChanged;

        private IDeviceManager _deviceManager;
        private Downloader _downloader;

        private bool _isRecordsListEnabled;
        private bool _isQueryEnabled;
        private string _downloadCommandString;
        private bool _isDownloadFolderEnabled;
        private bool _checkAll;
        private string _statusBarInfo;
        private StateType _state;
        private bool _isErrorSectionVisible;
        private double _taskbarProgressValue;
        private int _totalDownloadCount;
        private IList<RecordFileInfoModel> _records;

        public MainWindowViewModel(IDeviceManager deviceManager)
        {
            _deviceManager = deviceManager;
            _downloader = new Downloader(deviceManager);

            DownloadErrors = new ObservableCollection<string>();
            QueryCommand = new RelayCommand(() => Query?.Invoke(this, EventArgs.Empty));
            DownloadStopCommand = new RelayCommand(OnDownloadOrStop, IsDownloadCommandAvailable);
            ToggleSelectCommand = new RelayCommand<ObservableCollection<object>>(OnToggleSelect, IsToggleSelectAvailable);
            OpenDownloadFolderCommand = new RelayCommand(() => Process.Start(App.Settings.DownloadFolder));
            PlayCommand = new RelayCommand<RecordFileInfoModel>((record) => PlayRecord?.Invoke(this, record));
            SettingsCommand = new RelayCommand(() => ShowSettings?.Invoke(this, EventArgs.Empty));
            LiveViewCommand = new RelayCommand(() => ShowLiveView?.Invoke(this, EventArgs.Empty));

            State = StateType.Idle;

            _deviceManager.DownloadCompleted += Downloader_DownloadCompleted;
            _downloader.DownloadStarted += Downloader_DownloadStarted;
            _downloader.DownloadError += Downloader_DownloadError;
            _downloader.DownloadsCompleted += Downloader_DownloadsCompleted;

            EndDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            StartDateTime = EndDateTime.AddDays(-1);
            StatusBarInfo = "Ready";
        }

        public ICommand QueryCommand { get; }
        public ICommand DownloadStopCommand { get; }
        public ICommand ToggleSelectCommand { get; }
        public ICommand OpenDownloadFolderCommand { get; }
        public ICommand PlayCommand { get; }
        public ICommand SettingsCommand { get; }
        public ICommand LiveViewCommand { get; }

        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public ObservableCollection<string> DownloadErrors { get; private set; }

        public IList<RecordFileInfoModel> Records
        {
            get => _records;
            set
            {
                _records = value;
                OnPropertyChanged(nameof(Records));
            }
        }

        public string StatusBarInfo
        {
            get => _statusBarInfo;
            set
            {
                _statusBarInfo = value;
                OnPropertyChanged(nameof(StatusBarInfo));
            }
        }

        public string DownloadCommandString
        {
            get => _downloadCommandString;
            set
            {
                _downloadCommandString = value;
                OnPropertyChanged(nameof(DownloadCommandString));
            }
        }

        public bool IsRecordsListEnabled
        {
            get => _isRecordsListEnabled;
            set
            {
                _isRecordsListEnabled = value;
                OnPropertyChanged(nameof(IsRecordsListEnabled));
            }
        }

        public bool IsQueryEnabled
        {
            get => _isQueryEnabled;
            set
            {
                _isQueryEnabled = value;
                OnPropertyChanged(nameof(IsQueryEnabled));
            }
        }

        public bool IsDownloadFolderEnabled
        {
            get => _isDownloadFolderEnabled;
            set
            {
                _isDownloadFolderEnabled = value;
                OnPropertyChanged(nameof(IsDownloadFolderEnabled));
            }
        }

        public bool CheckAll
        {
            get => _checkAll;
            set
            {
                _checkAll = value;
                OnPropertyChanged(nameof(CheckAll));
                if (Records != null)
                    Records.ToList().ForEach(record => record.IsSelected = value);
            }
        }

        public bool IsIdle => (State == StateType.Idle);

        public StateType State
        {
            get => _state;
            set
            {
                _state = value;
                OnPropertyChanged(nameof(State));
                OnPropertyChanged(nameof(IsIdle));
                DownloadCommandString = IsIdle ? "Download selected" : "Stop downloads";
            }
        }

        public bool IsErrorSectionVisible
        {
            get => _isErrorSectionVisible;
            set
            {
                _isErrorSectionVisible = value;
                OnPropertyChanged(nameof(IsErrorSectionVisible));
            }
        }

        public double TaskbarProgressValue
        {
            get => _taskbarProgressValue;
            set
            {
                _taskbarProgressValue = value;
                OnPropertyChanged(nameof(TaskbarProgressValue));
            }
        }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        /// <summary>
        /// Callback when the Download/Stop button has been clicked
        /// </summary>
        private void OnDownloadOrStop()
        {
            if (_downloader.IsDownloading)
            {
                _downloader.StopDownload();
                State = StateType.Idle;
            }
            else
            {
                Records.ToList().ForEach(record => record.ResetProgress());

                _downloader.PendingDownloads = Records.Where(record => record.IsSelected).ToList();
                if (_downloader.PendingDownloads.Any())
                {
                    State = StateType.Running;
                    _totalDownloadCount = _downloader.PendingDownloads.Count();
                }
            }
        }
        private bool IsDownloadCommandAvailable()
        {
            return !string.IsNullOrEmpty(App.Settings.DownloadFolder) && Records != null && Records.Any(record => record.IsSelected);
        }

        /// <summary>
        /// Callback when the "[Un]Select highlighted" button has been clicked
        /// </summary>
        private void OnToggleSelect(ObservableCollection<object> highlightedItems)
        {
            highlightedItems.ToList().ForEach(record => (record as RecordFileInfoModel).IsSelected = !(record as RecordFileInfoModel).IsSelected);
        }
        private bool IsToggleSelectAvailable(ObservableCollection<object> highlightedItems)
        {
            return (Records != null && Records.Any());
        }

        /// <summary>
        /// Callback when a single download is completed
        /// </summary>
        private void Downloader_DownloadCompleted(object sender, string errorMessage)
        {
            TaskbarProgressValue = 1.0 - (_downloader.PendingDownloads.Count() / (double)_totalDownloadCount);

            if (!string.IsNullOrEmpty(errorMessage))
            {
                DownloadErrors.Add($"{_deviceManager.DownloadRecord} - {errorMessage}");
                IsErrorSectionVisible = true;
            }
        }

        /// <summary>
        /// Callback when all downloads are completed
        /// </summary>
        private void Downloader_DownloadsCompleted(object sender, EventArgs e)
        {
            State = StateType.Idle;
            StatusBarInfo = "Ready";
            TaskbarProgressValue = 0;
        }

        private void Downloader_DownloadStarted(object sender, RecordFileInfoModel record)
        {
            StatusBarInfo = $"Downloading {record} ({_downloader.PendingDownloads.Count} remaining)";
        }

        private void Downloader_DownloadError(object sender, string message)
        {
            DownloadErrors.Add(message);
            IsErrorSectionVisible = true;
        }
    }
}
