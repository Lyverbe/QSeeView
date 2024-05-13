using QSeeView.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace QSeeView.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        public event EventHandler<bool> Close;

        public event PropertyChangedEventHandler PropertyChanged;

        private string _deviceIp;
        private ushort _devicePort;
        private string _username;
        private string _password;
        private IEnumerable<DeviceModelType> _deviceModels;
        private DeviceModelType _selectedModel;

        public LoginViewModel()
        {
            _deviceModels = Enum.GetValues(typeof(DeviceModelType)).OfType<DeviceModelType>().ToList();

            OkCommand = new RelayCommand(() => Close?.Invoke(this, true), IsOkEnabled);
            CancelCommand = new RelayCommand(() => Close?.Invoke(this, false));
            ClearCommand = new RelayCommand(OnClear);

            DeviceIp = App.Settings.DeviceIp;
            DevicePort = App.Settings.DevicePort;
            Username = App.Settings.Username;
            Password = App.Settings.Password;
            DeviceModel = App.Settings.DeviceModel;
        }

        public ICommand OkCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand ClearCommand { get; }

        public string DeviceIp
        {
            get => _deviceIp;
            set
            {
                _deviceIp = value;
                OnPropertyChanged(nameof(DeviceIp));
            }
        }

        public ushort DevicePort
        {
            get => _devicePort;
            set
            {
                _devicePort = value;
                OnPropertyChanged(nameof(DevicePort));
            }
        }

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public IEnumerable<DeviceModelType> DeviceModels
        {
            get => _deviceModels;
            set
            {
                _deviceModels = value;
                OnPropertyChanged(nameof(DeviceModels));
            }
        }

        public DeviceModelType DeviceModel
        {
            get => _selectedModel;
            set
            {
                _selectedModel = value;
                OnPropertyChanged(nameof(DeviceModel));
            }
        }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private bool IsOkEnabled() => !string.IsNullOrEmpty(DeviceIp) && DevicePort > 0 && !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password);

        private void OnClear()
        {
            DeviceIp = string.Empty;
            DevicePort = App.Settings.DefaultPort;
            Username = string.Empty;
            Password = string.Empty;
        }
    }
}
