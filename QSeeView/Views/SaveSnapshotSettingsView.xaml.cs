using NetSDKCS;
using QSeeView.Tools;
using QSeeView.ViewModels;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;

namespace QSeeView.Views
{
    public partial class SaveSnapshotSettingsView : Window
    {
        private SaveSnapshotSettingsViewModel _viewModel;

        public SaveSnapshotSettingsView(DateTime snapshotTimestamp, int channel)
        {
            InitializeComponent();

            _viewModel = new SaveSnapshotSettingsViewModel();
            DataContext = _viewModel;

            _viewModel.Close += ViewModel_Close;
            _viewModel.BrowseOutputFileName += ViewModel_BrowseOutputFileName;
            _viewModel.OutputTypeChanged += (s, e) => SetOutputFileNameExtension();

            SetDefaultOutputFileName(snapshotTimestamp, channel);
        }

        public string OutputFileName => _viewModel.OutputFileName;
        public EM_NET_CAPTURE_FORMATS CaptureFormat => _viewModel.OutputTypes.ElementAt(_viewModel.SelectedOutputTypeIndex).CaptureFormat;

        private void ViewModel_Close(object sender, bool isSaveClicked)
        {
            if (isSaveClicked)
                App.Settings.LastPlaybackCaptureFormat = _viewModel.SelectedOutputTypeIndex;

            DialogResult = isSaveClicked;
        }

        private void ViewModel_BrowseOutputFileName(object sender, System.EventArgs e)
        {
            var view = new SaveFileDialog()
            {
                Title = "Select output file name",
                Filter = "All files|*.*",
                OverwritePrompt = true
            };
            var dialogResult = view.ShowDialog();
            if (dialogResult == System.Windows.Forms.DialogResult.OK)
                _viewModel.OutputFileName = view.FileName;
        }

        private void SetDefaultOutputFileName(DateTime snapshotTimestamp, int channel)
        {
            var fileName = FileNameBuilder.Build(App.Settings.FileNamesPattern, snapshotTimestamp, channel);
            _viewModel.OutputFileName = Path.Combine(App.Settings.DownloadFolder, fileName);
            SetOutputFileNameExtension();
        }

        private void SetOutputFileNameExtension()
        {
            var outputFileName = _viewModel.OutputFileName;
            var extensionIndex = outputFileName.LastIndexOf(".");
            if (extensionIndex != -1)
                outputFileName = outputFileName.Substring(0, extensionIndex);

            var captureFormat = _viewModel.OutputTypes.ElementAt(_viewModel.SelectedOutputTypeIndex);
            if (captureFormat.CaptureFormat == EM_NET_CAPTURE_FORMATS.BMP)
                outputFileName += ".bmp";
            else
                outputFileName += ".jpg";
            _viewModel.OutputFileName = outputFileName;
        }
    }
}
