using QSeeView.Tools.Models;
using QSeeView.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms;

namespace QSeeView.Views
{
    public partial class SettingsView : Window
    {
        private SettingsViewModel _viewModel;

        public SettingsView()
        {
            InitializeComponent();

            _viewModel = new SettingsViewModel();
            DataContext = _viewModel;

            _viewModel.Close += ViewModel_Close;
            _viewModel.BrowseDownloadFolder += ViewModel_BrowseDownloadFolder;
        }

        private void ViewModel_BrowseDownloadFolder(object sender, EventArgs e)
        {
            var view = new FolderBrowserDialog()
            {
                Description = "Select a directory",
                SelectedPath = _viewModel.DownloadFolder
            };
            var dialogResult = view.ShowDialog();
            if (dialogResult == System.Windows.Forms.DialogResult.OK)
            {
                _viewModel.DownloadFolder = view.SelectedPath;
            }
        }

        private void ViewModel_Close(object sender, bool isOkClicked)
        {
            if (isOkClicked)
            {
                App.Settings.IsAutomaticLogin = _viewModel.IsAutomaticLogin;
                App.Settings.IsConvertingToAvi = _viewModel.IsConvertingToAvi;
                App.Settings.IsAutoQueryAtStartup = _viewModel.IsAutoQueryAtStartup;
                App.Settings.ChannelsInfo = new List<ChannelInfoModel>(_viewModel.ChannelsInfo);
                App.Settings.DownloadFolder = _viewModel.DownloadFolder;
                App.Settings.NightFilesStartHour = _viewModel.NightFilesStartHour;
                App.Settings.NightFilesEndHour = _viewModel.NightFilesEndHour;
            }

            DialogResult = isOkClicked;
        }
    }
}
