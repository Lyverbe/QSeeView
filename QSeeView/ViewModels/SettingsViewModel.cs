using QSeeView.Tools;
using QSeeView.Tools.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;

namespace QSeeView.ViewModels
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        public event EventHandler<bool> Close;
        public event EventHandler BrowseDownloadFolder;
        public event EventHandler BrowseFfmegPath;

        public event PropertyChangedEventHandler PropertyChanged;

        private bool _isDarkTheme;
        private bool _isAutomaticLogin;
        private bool _isConvertingToAvi;
        private bool _isAutoQueryAtStartup;
        private (string name, bool isLanscape)[] _channels;
        private string _downloadFolder;
        private string _ffmpegPath;
        private bool _isResettingPlaybackSpeed;
        private string _fileNamesPattern;
        private string _fileNamesOutputExample;
        private int _liveViewRowsCount;
        private int _liveViewColumnsCount;
        private int? _hddPercentSpaceWarning;
        private int _queryYellowColorSeconds;
        private int _queryRedColorSeconds;
        private bool _isAutoQueryEnabled;
        private int _autoQuerySeconds;

        public SettingsViewModel()
        {
            _channels = new (string, bool)[4];

            OkCommand = new RelayCommand(() => Close?.Invoke(this, true));
            CancelCommand = new RelayCommand(() => Close?.Invoke(this, false));
            BrowseDownloadFolderCommand = new RelayCommand(() => BrowseDownloadFolder?.Invoke(this, EventArgs.Empty));
            BrowseFfmpegPathCommand = new RelayCommand(() => BrowseFfmegPath?.Invoke(this, EventArgs.Empty));

            IsDarkTheme = (App.Settings.ThemeId == Types.ThemeType.Dark);
            IsAutomaticLogin = App.Settings.IsAutomaticLogin;
            IsAutoQueryAtStartup = App.Settings.IsAutoQueryAtStartup;
            IsResettingPlaybackSpeed = App.Settings.IsResettingPlaybackSpeed;
            IsAutoSelectAtQuery = App.Settings.IsAutoSelectAtQuery;

            ChannelsInfo = new List<ChannelInfoModel>(App.Settings.ChannelsInfo);
            IsConvertingToAvi = App.Settings.IsConvertingToAvi;
            DownloadFolder = App.Settings.DownloadFolder;
            NightFilesStartHour = App.Settings.NightFilesStartHour;
            NightFilesEndHour = App.Settings.NightFilesEndHour;
            FfmpegPath = App.Settings.FfmpegPath;
            StartDatesOffset = App.Settings.StartDatesOffset;
            FileNamesPattern = App.Settings.FileNamesPattern;
            LiveViewSize = App.Settings.LiveViewSize;
            IsAutoOpenDownloads = App.Settings.IsAutoOpenDownloads;
            DoPlayDownloadsCompleteSound = App.Settings.DoPlayDownloadsCompleteSound;
            HddPercentSpaceWarning = App.Settings.HddPercentSpaceWarning;
            _queryYellowColorSeconds = App.Settings.QueryYellowColorSeconds;
            _queryRedColorSeconds = App.Settings.QueryRedColorSeconds;
            DoShowHddSpaceWarning = App.Settings.DoShowHddSpaceWarning;
            IsAutoQueryEnabled = App.Settings.AutoQuerySeconds.HasValue;
            AutoQuerySeconds = IsAutoQueryEnabled ? App.Settings.AutoQuerySeconds.Value : 1;
        }

        public ICommand OkCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand BrowseDownloadFolderCommand { get; }
        public ICommand BrowseFfmpegPathCommand { get; }

        public IList<ChannelInfoModel> ChannelsInfo { get; set; }

        public bool IsDarkTheme
        {
            get => _isDarkTheme;
            set
            {
                _isDarkTheme = value;
                OnPropertyChanged(nameof(IsDarkTheme));
            }
        }

        public bool IsAutomaticLogin
        {
            get => _isAutomaticLogin;
            set
            {
                _isAutomaticLogin = value;
                OnPropertyChanged(nameof(IsAutomaticLogin));
            }
        }

        public bool IsConvertingToAvi
        {
            get => _isConvertingToAvi;
            set
            {
                _isConvertingToAvi = value;
                OnPropertyChanged(nameof(IsConvertingToAvi));
            }
        }

        public bool IsAutoQueryAtStartup
        {
            get => _isAutoQueryAtStartup;
            set
            {
                _isAutoQueryAtStartup = value;
                OnPropertyChanged(nameof(IsAutoQueryAtStartup));
            }
        }

        public bool IsResettingPlaybackSpeed
        {
            get => _isResettingPlaybackSpeed;
            set
            {
                _isResettingPlaybackSpeed = value;
                OnPropertyChanged(nameof(IsResettingPlaybackSpeed));
            }
        }

        public string DownloadFolder
        {
            get => _downloadFolder;
            set
            {
                _downloadFolder = value;
                OnPropertyChanged(nameof(DownloadFolder));
            }
        }

        public string FfmpegPath
        {
            get => _ffmpegPath;
            set
            {
                _ffmpegPath = value;
                OnPropertyChanged(nameof(FfmpegPath));
            }
        }

        public string FileNamesPattern
        {
            get => _fileNamesPattern;
            set
            {
                _fileNamesPattern = value;
                OnPropertyChanged(nameof(FileNamesPattern));
                OnFileNamesPatternChanged();
            }
        }

        public string FileNamesOutputExample
        {
            get => _fileNamesOutputExample;
            set
            {
                _fileNamesOutputExample = value;
                OnPropertyChanged(nameof(FileNamesOutputExample));
            }
        }

        public int LiveViewSize
        {
            get => _liveViewRowsCount;
            set
            {
                _liveViewRowsCount = LimitLiveViewGridSize(value);
                OnPropertyChanged(nameof(LiveViewSize));
            }
        }

        public int LiveViewColumnsCount
        {
            get => _liveViewColumnsCount;
            set
            {
                _liveViewColumnsCount = LimitLiveViewGridSize(value);
                OnPropertyChanged(nameof(LiveViewColumnsCount));
            }
        }

        public int? HddPercentSpaceWarning
        {
            get => _hddPercentSpaceWarning;
            set
            {
                _hddPercentSpaceWarning = (value <= 100) ? value : 100;
                OnPropertyChanged(nameof(HddPercentSpaceWarning));
            }
        }

        public bool IsAutoOpenDownloads { get; set; }
        public bool DoPlayDownloadsCompleteSound { get; set; }
        public bool IsAutoSelectAtQuery { get; set; }

        public int NightFilesStartHour { get; set; }
        public int NightFilesEndHour { get; set; }
        public int QueryYellowColorSeconds
        {
            get => _queryYellowColorSeconds;
            set => _queryYellowColorSeconds = (value > QueryRedColorSeconds) ? QueryRedColorSeconds : value;
        }
        public int QueryRedColorSeconds
        {
            get => _queryRedColorSeconds;
            set => _queryRedColorSeconds = (value < QueryYellowColorSeconds) ? QueryYellowColorSeconds : value;
        }

        public bool IsAutoQueryEnabled
        {
            get => _isAutoQueryEnabled;
            set
            {
                _isAutoQueryEnabled = value;
                OnPropertyChanged(nameof(IsAutoQueryEnabled));
            }
        }
        public int AutoQuerySeconds
        {
            get => _autoQuerySeconds;
            set
            {
                _autoQuerySeconds = Math.Max(value, 1);
                OnPropertyChanged(nameof(AutoQuerySeconds));
            }
        }

        public int StartDatesOffset { get; set; }
        public bool DoShowHddSpaceWarning { get; set; }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void OnFileNamesPatternChanged()
        {
            if (string.IsNullOrEmpty(FileNamesPattern))
                FileNamesOutputExample = string.Empty;
            else
                FileNamesOutputExample = "Example output: " + FileNameBuilder.Build(FileNamesPattern, DateTime.Now, 0);
        }

        private int LimitLiveViewGridSize(int value)
        {
            if (value < 2)
                value = 2;
            else if (value > 8)
                value = 8;

            return value;
        }
    }
}
