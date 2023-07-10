using QSeeView.Models;
using QSeeView.Tools;
using QSeeView.Views;
using System;
using System.Linq;
using System.Windows;

namespace QSeeView
{
    public partial class MainWindow : Window
    {
        private MainWindowViewModel _viewModel;

        private IDeviceManager _deviceManager;

        public MainWindow(IDeviceManager deviceManager)
        {
            InitializeComponent();

            _deviceManager = deviceManager;

            _viewModel = new MainWindowViewModel(deviceManager);
            DataContext = _viewModel;

            _viewModel.PlayRecord += ViewModel_PlayRecord;
            _viewModel.ShowSettings += ViewModel_ShowSettings;
            _viewModel.Query += (s, e) => Query();
            _viewModel.ShowLiveView += ViewModel_ShowLiveView;

            if (App.Settings.IsAutoQueryAtStartup)
                ContentRendered += (s, e) => Query();
        }

        protected override void OnClosed(EventArgs e)
        {
            _deviceManager.Shutdown();
            base.OnClosed(e);
        }

        private void ViewModel_PlayRecord(object sender, RecordFileInfoModel record)
        {
            var playIndex = _viewModel.Records.IndexOf(record);
            var view = new PlaybackView(_deviceManager, _viewModel.Records, playIndex)
            {
                Owner = this
            };
            view.ShowDialog();
        }

        private void Query()
        {
            _viewModel.Records = _deviceManager.Query(_viewModel.StartDateTime, _viewModel.EndDateTime, App.Settings.IsQueryIgnoringNightFiles);
            if (_viewModel.Records.Count >= _deviceManager.MaxQueryRecords)
                MessageBox.Show($"Limit of {_deviceManager.MaxQueryRecords} records has been reached. Some files may be missing.", "Query");

            _viewModel.CheckAll = true;
        }

        private void ViewModel_ShowSettings(object sender, EventArgs e)
        {
            var view = new SettingsView()
            {
                Owner = this
            };
            var isOkClicked = view.ShowDialog();
            if (isOkClicked == true)
                _viewModel.Records?.ToList().ForEach(record => record.RefreshChannelNames());
        }

        private void ViewModel_ShowLiveView(object sender, EventArgs e)
        {
            new LiveView(_deviceManager)
            {
                Owner = this
            }.ShowDialog();
        }
    }
}
