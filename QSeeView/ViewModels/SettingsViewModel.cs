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

        public event PropertyChangedEventHandler PropertyChanged;

        private bool _isAutomaticLogin;
        private bool _isConvertingToAvi;
        private bool _isAutoQueryAtStartup;
        private (string name, bool isLanscape)[] _channels;
        private string _downloadFolder;

        public SettingsViewModel()
        {
            _channels = new (string, bool)[4];

            OkCommand = new RelayCommand(() => Close?.Invoke(this, true));
            CancelCommand = new RelayCommand(() => Close?.Invoke(this, false));
            BrowseDownloadFolderCommand = new RelayCommand(() => BrowseDownloadFolder?.Invoke(this, EventArgs.Empty));

            IsAutomaticLogin = App.Settings.IsAutomaticLogin;
            IsAutoQueryAtStartup = App.Settings.IsAutoQueryAtStartup;

            ChannelsInfo = new List<ChannelInfoModel>(App.Settings.ChannelsInfo);
            IsConvertingToAvi = App.Settings.IsConvertingToAvi;
            DownloadFolder = App.Settings.DownloadFolder;
            NightFilesStartHour = App.Settings.NightFilesStartHour;
            NightFilesEndHour = App.Settings.NightFilesEndHour;
        }

        public ICommand OkCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand BrowseDownloadFolderCommand { get; }

        public IList<ChannelInfoModel> ChannelsInfo { get; set; }

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

        public string DownloadFolder
        {
            get => _downloadFolder;
            set
            {
                _downloadFolder = value;
                OnPropertyChanged(nameof(DownloadFolder));
            }
        }

        public int NightFilesStartHour { get; set; }
        public int NightFilesEndHour { get; set; }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
