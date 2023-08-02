using QSeeView.Tools;
using QSeeView.Views;
using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Input;

namespace QSeeView
{
    public partial class App : Application
    {
        public static readonly System.Drawing.Size HDSize = new System.Drawing.Size(1920, 1080);

        private IDeviceManager _deviceManager;

        public App()
        {
            _deviceManager = new QCW4DeviceManager();

            SettingsFileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "QSeeViewSettings.xml");

            if (File.Exists(SettingsFileName))
                LoadSettings();
            else
                Settings = new Settings(_deviceManager.ChannelsCount);
        }

        private string SettingsFileName { get; set; }
        public static Settings Settings { get; private set; }

        private void ApplicationStartup(object sender, StartupEventArgs e)
        {
            if (Settings.IsAutomaticLogin)
                _deviceManager.Login(Settings.DeviceIp, Settings.DevicePort, Settings.Username, Settings.Password);

            if (!_deviceManager.IsConnected)
            {
                var view = new LoginView();
                var isOkClicked = view.ShowDialog();
                if (isOkClicked == true)
                    _deviceManager.Login(Settings.DeviceIp, Settings.DevicePort, Settings.Username, Settings.Password);
            }

            if (_deviceManager.IsConnected)
                new MainWindow(_deviceManager).ShowDialog();
            else
                MessageBox.Show("Login failed.  Ensure you have the right username and password", "Login failed", MessageBoxButton.OK, MessageBoxImage.Error);

            SaveSettings();
            Shutdown();
        }

        private void LoadSettings()
        {
            try
            {
                using (var stream = new FileStream(SettingsFileName, FileMode.Open))
                {
                    var serializer = new DataContractSerializer(typeof(Settings));
                    Settings = (Settings)serializer.ReadObject(stream);
                }
            }
            catch { }
        }

        private void SaveSettings()
        {
            try
            {
                using (var stream = new FileStream(SettingsFileName, FileMode.Create))
                {
                    var serializer = new DataContractSerializer(typeof(Settings));
                    serializer.WriteObject(stream, Settings);
                }
            }
            catch { }
        }

        public static void IntTextBox_PreviewTextInput(TextCompositionEventArgs e)
        {
            if (e.Text.Length > 0)
            {
                var character = e.Text[0];
                if (!"0123456789".Contains(character))
                    e.Handled = true;
            }
        }
    }
}
