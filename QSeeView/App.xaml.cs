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
            SettingsFileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "QSeeViewSettings.xml");
        }

        private string SettingsFileName { get; set; }
        public static Settings Settings { get; private set; }

        private void ApplicationStartup(object sender, StartupEventArgs e)
        {
            LoadSettings(SettingsFileName);

            var retryLogin = true;
            bool? isOkClickedOnLogin = false;
            if (Settings != null)
            {
                InitializeTheme();

                do
                {
                    if (Settings.IsAutomaticLogin)
                    {
                        _deviceManager = GetDeviceManager();
                        _deviceManager.Login(Settings.DeviceIp, Settings.DevicePort, Settings.Username, Settings.Password);
                    }
                    else
                    {
                        var view = new LoginView();
                        isOkClickedOnLogin = view.ShowDialog();
                        if (isOkClickedOnLogin == true)
                        {
                            _deviceManager = GetDeviceManager();
                            _deviceManager.Login(Settings.DeviceIp, Settings.DevicePort, Settings.Username, Settings.Password);
                        }
                        else
                            retryLogin = false;
                    }

                    if (_deviceManager != null && _deviceManager.IsConnected)
                    {
                        var window = new MainWindow(_deviceManager);
                        if (Settings.WindowRect.Size.Width != 0 && Settings.WindowRect.Size.Height != 0)
                        {
                            window.Left = Settings.WindowRect.Left;
                            window.Top = Settings.WindowRect.Top;
                            window.Width = Settings.WindowRect.Width;
                            window.Height = Settings.WindowRect.Height;
                        }
                        window.ShowDialog();

                        CreateSettingsBackup();
                        SaveSettings();
                    }
                    else if (isOkClickedOnLogin == true)
                    {
                        MessageBox.Show("Login failed.  Ensure you have the right username and password.", "Login failed", MessageBoxButton.OK, MessageBoxImage.Error);
                        retryLogin = true;
                    }
                } while (retryLogin && !_deviceManager.IsConnected);
            }
            Shutdown();
        }

        private IDeviceManager GetDeviceManager()
        {
            switch (Settings.DeviceModel)
            {
                case DeviceModelType.QCW4:
                    return new QCW4DeviceManager();
                default:
                    throw new Exception("Unhandled device model");
            }
        }

        private void InitializeTheme()
        {
            var dictionary = Resources.MergedDictionaries.FirstOrDefault(dict => dict.Source.OriginalString.ToLower().StartsWith("themes/"));
            if (dictionary != null)
            {
                var theme = (Settings.ThemeId == Types.ThemeType.Light) ? "Light" : "Dark";
                dictionary.Source = new Uri($"Themes/{theme}.xaml", UriKind.RelativeOrAbsolute);
            }
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
            //else
            //    Settings = new Settings();

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
                Settings = new Settings();
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
