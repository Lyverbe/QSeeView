using QSeeView.Models;
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
        public event EventHandler StartDownload;
        public event EventHandler StopDownload;

        public event PropertyChangedEventHandler PropertyChanged;

        private DateTime _startDateTime;
        private DateTime _endDateTime;
        private bool _isRecordsListEnabled;
        private bool _isQueryEnabled;
        private string _downloadCommandString;
        private bool _isDownloadFolderEnabled;
        private bool _checkAll;
        private string _statusBarInfo;
        private StateType _state;
        private bool _isErrorSectionVisible;
        private double _taskbarProgressValue;
        private IList<RecordFileInfoModel> _records;
        private string _datesOffsetString;

        public MainWindowViewModel()
        {
            DownloadErrors = new ObservableCollection<string>();
            QueryCommand = new RelayCommand(() => Query?.Invoke(this, EventArgs.Empty));
            DownloadStopCommand = new RelayCommand(OnDownloadOrStop, IsDownloadCommandAvailable);
            ToggleSelectCommand = new RelayCommand<ObservableCollection<object>>(OnToggleSelect, IsToggleSelectAvailable);
            OpenDownloadFolderCommand = new RelayCommand(() => Process.Start(App.Settings.DownloadFolder));
            PlayCommand = new RelayCommand<RecordFileInfoModel>((record) => PlayRecord?.Invoke(this, record));
            SettingsCommand = new RelayCommand(() => ShowSettings?.Invoke(this, EventArgs.Empty));
            LiveViewCommand = new RelayCommand(() => ShowLiveView?.Invoke(this, EventArgs.Empty));
            DecreaseDatesOffsetCommand = new RelayCommand(() => ChangeDatesOffset(-1));
            IncreaseDatesOffsetCommand = new RelayCommand(() => ChangeDatesOffset(1));

            State = StateType.Idle;

            DatesOffsetString = App.Settings.StartDatesOffset.ToString();
            StartDateTime = DateTime.Now.Date.AddDays(-DatesOffset);
            EndDateTime = DateTime.Now.Date.AddDays(-DatesOffset + 1);
            StatusBarInfo = "Ready";
        }

        public ICommand QueryCommand { get; }
        public ICommand DownloadStopCommand { get; }
        public ICommand ToggleSelectCommand { get; }
        public ICommand OpenDownloadFolderCommand { get; }
        public ICommand PlayCommand { get; }
        public ICommand SettingsCommand { get; }
        public ICommand LiveViewCommand { get; }
        public ICommand DecreaseDatesOffsetCommand { get; }
        public ICommand IncreaseDatesOffsetCommand { get; }

        public ObservableCollection<string> DownloadErrors { get; private set; }
        public int TotalDownloadCount { get; set; }

        public DateTime StartDateTime
        {
            get => _startDateTime;
            set
            {
                _startDateTime = value;
                OnPropertyChanged(nameof(StartDateTime));
            }
        }

        public DateTime EndDateTime
        {
            get => _endDateTime;
            set
            {
                _endDateTime = value;
                OnPropertyChanged(nameof(EndDateTime));
            }
        }

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

        public string DatesOffsetString
        {
            get => _datesOffsetString;
            set
            {
                var isValid = int.TryParse(value, out var datesOffset);
                if (isValid && datesOffset >= 0)
                {
                    _datesOffsetString = value;
                    OnPropertyChanged(nameof(DatesOffsetString));
                    DatesOffset = datesOffset;
                    OnDatesOffsetChanged();
                }
            }
        }
        private int DatesOffset { get; set; }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        /// <summary>
        /// Callback when the Download/Stop button has been clicked
        /// </summary>
        private void OnDownloadOrStop()
        {
            if (IsIdle)
                StartDownload?.Invoke(this, EventArgs.Empty);
            else
                StopDownload?.Invoke(this, EventArgs.Empty);
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
        /// Called when the dates offset has changed
        /// </summary>
        private void OnDatesOffsetChanged()
        {
            StartDateTime = DateTime.Now.AddDays(-DatesOffset).Date;
            EndDateTime = DateTime.Now.AddDays(-DatesOffset+ 1).Date;
        }

        private void ChangeDatesOffset(int delta)
        {
            if (DatesOffset + delta >= 0)
                DatesOffsetString = (DatesOffset + delta).ToString();
        }
    }
}
