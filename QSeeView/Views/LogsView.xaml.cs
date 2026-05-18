using NetSDKCS;
using QSeeView.Tools;
using QSeeView.ViewModels;
using System;
using System.Windows;

namespace QSeeView.Views
{
    public partial class LogsView : Window
    {
        private IDeviceManager _deviceManager;
        private LogsViewModel _viewModel;

        public LogsView(IDeviceManager deviceManager)
        {
            InitializeComponent();

            _viewModel = new LogsViewModel(deviceManager);
            DataContext = _viewModel;

            _deviceManager = deviceManager;

            _viewModel.Close += (s, e) => DialogResult = true;
            _viewModel.Clear += ClearLogs;
        }

        private void ClearLogs(object sender, EventArgs e)
        {
            var answer = MessageBox.Show("Are you sure you wish to clear the logs?\n\nRemember that this operation deletes ALL logs, not only the current range.",
                "Clear logs", MessageBoxButton.YesNo);
            if (answer  == MessageBoxResult.Yes)
            {
                var success = _deviceManager.ClearLogs();
                if (success)
                    _viewModel.Logs.Clear();
                else
                    MessageBox.Show("Failed to clear the logs.", "Clear logs");
            }
        }
    }
}
