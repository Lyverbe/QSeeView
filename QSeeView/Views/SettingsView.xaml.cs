using QSeeView.Tools.Models;
using QSeeView.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

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
            _viewModel.BrowseFfmegPath += ViewModel_BrowseFfmegPath;
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
                App.Settings.FfmpegPath = _viewModel.FfmpegPath;
                App.Settings.StartDatesOffset = _viewModel.StartDatesOffset;
                App.Settings.IsResettingPlaybackSpeed = _viewModel.IsResettingPlaybackSpeed;
                App.Settings.FileNamesPattern = _viewModel.FileNamesPattern;
                App.Settings.LiveViewSize = _viewModel.LiveViewSize;
                App.Settings.IsAutoOpenDownloads = _viewModel.IsAutoOpenDownloads;
                App.Settings.DoPlayDownloadsCompleteSound = _viewModel.DoPlayDownloadsCompleteSound;
            }

            DialogResult = isOkClicked;
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
                _viewModel.DownloadFolder = view.SelectedPath;
        }

        private void ViewModel_BrowseFfmegPath(object sender, EventArgs e)
        {
            var view = new OpenFileDialog()
            {
                Title = "Select FFMPEG executable location",
                Filter = "Executable|ffmpeg.exe|All files|*.*",
                CheckFileExists = true
            };
            var dialogResult = view.ShowDialog();
            if (dialogResult == System.Windows.Forms.DialogResult.OK)
                _viewModel.FfmpegPath = view.FileName;
        }

        private void IntTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e) => App.IntTextBox_PreviewTextInput(e);
    }
}
