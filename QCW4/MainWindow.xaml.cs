using NetSDKCS;
using QCW4.Properties;
using System;
using System.Windows;

namespace QCW4
{
    public partial class MainWindow : Window
    {
        MainWindowViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new MainWindowViewModel();
            DataContext = _viewModel;

            if (!_viewModel.IsLoggedIn)
            {
                MessageBox.Show("Initialization failed", "Initialization", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
            _viewModel.ShowMessage += ViewModel_ShowMessage;
            _viewModel.PlayRecord += ViewModel_PlayRecord;
            _viewModel.SetChannels += ViewModel_SetChannels;

            ContentRendered += (s, e) => _viewModel.OnQuery();
        }

        protected override void OnClosed(EventArgs e)
        {
            OriginalSDK.CLIENT_Cleanup();
            Settings.Default.Save();
            base.OnClosed(e);
        }

        private void ViewModel_ShowMessage(object sender, string message)
        {
            MessageBox.Show(message);
        }

        private void ViewModel_PlayRecord(object sender, RecordFileInfo record)
        {
            var playIndex = _viewModel.Records.IndexOf(record);
            var view = new PlaybackView(_viewModel.Records, playIndex, _viewModel.LoginId)
            {
                Owner = this
            };
            view.ShowDialog();
        }

        private void ViewModel_SetChannels(object sender, EventArgs e)
        {
            var view = new ChannelsView()
            {
                Owner = this
            };
            view.ShowDialog();
        }
    }
}
