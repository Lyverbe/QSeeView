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
        public const string SettingsBackupExtension = ".backup";

        private IDeviceManager _deviceManager;

        public App()
        {
            _deviceManager = new QCW4DeviceManager();

            SettingsFileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "QSeeViewSettings.xml");
        }

        private string SettingsFileName { get; set; }
        public static Settings Settings { get; private set; }

        private void ApplicationStartup(object sender, StartupEventArgs e)
        {
            LoadSettings(SettingsFileName);

            if (Settings != null)
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

                CreateSettingsBackup();
                SaveSettings();
            }
            Shutdown();
        }

        private void LoadSettings(string fileName)
        {
            if (File.Exists(fileName))
            {
                try
                {
                    using (var stream = new FileStream(fileName, FileMode.Open))
                    {
                        var serializer = new DataContractSerializer(typeof(Settings));
                        Settings = (Settings)serializer.ReadObject(stream);
                    }
                }
                catch { }
            }

            if (Settings == null)
                HandleLoadSettingsFailure();
        }

        private void HandleLoadSettingsFailure()
        {
            var loadFromBackup = false;
            var backupFileName = SettingsFileName + SettingsBackupExtension;
            if (File.Exists(backupFileName))
            {
                var reply = MessageBox.Show("Failed to load settings.  Do you wish to attempt loading from the backup file?",
                    "Load settings", MessageBoxButton.YesNoCancel, MessageBoxImage.Error);
                if (reply == MessageBoxResult.Cancel)
                    return;
                loadFromBackup = (reply == MessageBoxResult.Yes);
            }
            if (loadFromBackup)
                LoadSettings(backupFileName);
            if (Settings == null)
            {
                MessageBox.Show("Failed to load settings.  Application will launch with initial settings.", "Load settings");
                Settings = new Settings(_deviceManager.ChannelsCount);
            }
        }

        private void CreateSettingsBackup()
        {
            if (File.Exists(SettingsFileName))
                File.Copy(SettingsFileName, SettingsFileName + SettingsBackupExtension, true);
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
