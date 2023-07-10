using System;
using System.ComponentModel;
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

        public LoginViewModel()
        {
            OkCommand = new RelayCommand(() => Close?.Invoke(this, true), IsOkEnabled);
            CancelCommand = new RelayCommand(() => Close?.Invoke(this, false));

            DeviceIp = App.Settings.DeviceIp;
            DevicePort = App.Settings.DevicePort;
            Username = App.Settings.Username;
            Password = App.Settings.Password;
        }

        public ICommand OkCommand { get; }
        public ICommand CancelCommand { get; }

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

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private bool IsOkEnabled() => !string.IsNullOrEmpty(DeviceIp) && DevicePort > 0 && !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password);
    }
}
