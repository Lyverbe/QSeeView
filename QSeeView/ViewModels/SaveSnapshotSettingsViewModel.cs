using NetSDKCS;
using QSeeView.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;

namespace QSeeView.ViewModels
{
    public class SaveSnapshotSettingsViewModel : INotifyPropertyChanged
    {
        public event EventHandler<bool> Close;
        public event EventHandler BrowseOutputFileName;
        public event EventHandler OutputTypeChanged;

        public event PropertyChangedEventHandler PropertyChanged;

        private string _outputFileName;
        private IEnumerable<CaptureFormatModel> _outputTypes;
        private int _selectedOuputTypeIndex;

        public SaveSnapshotSettingsViewModel()
        {
            SaveCommand = new RelayCommand(() => Close?.Invoke(this, true));
            CancelCommand = new RelayCommand(() => Close?.Invoke(this, false));
            BrowseOutputFileNameCommand = new RelayCommand(() => BrowseOutputFileName?.Invoke(this, EventArgs.Empty));

            SelectedOutputTypeIndex = App.Settings.LastPlaybackCaptureFormat;

            InitializeOutputTypes();
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand BrowseOutputFileNameCommand { get; }

        public string OutputFileName
        {
            get => _outputFileName;
            set
            {
                _outputFileName = value;
                OnPropertyChanged(nameof(OutputFileName));
            }
        }

        public IEnumerable<CaptureFormatModel> OutputTypes
        {
            get => _outputTypes;
            set
            {
                _outputTypes = value;
                OnPropertyChanged(nameof(OutputTypes));
            }
        }
        public int SelectedOutputTypeIndex
        {
            get => _selectedOuputTypeIndex;
            set
            {
                _selectedOuputTypeIndex = value;
                OnPropertyChanged(nameof(SelectedOutputTypeIndex));
                OutputTypeChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void InitializeOutputTypes()
        {
            var outputTypes = new List<CaptureFormatModel>()
            {
                new CaptureFormatModel() { Description = "Bitmap", CaptureFormat = EM_NET_CAPTURE_FORMATS.BMP },
                new CaptureFormatModel() { Description = "JPEG (100% quality)", CaptureFormat = EM_NET_CAPTURE_FORMATS.JPEG },
                new CaptureFormatModel() { Description = "JPEG (70% quality)", CaptureFormat = EM_NET_CAPTURE_FORMATS.JPEG_70 },
                new CaptureFormatModel() { Description = "JPEG (50% quality)", CaptureFormat = EM_NET_CAPTURE_FORMATS.JPEG_50 },
                new CaptureFormatModel() { Description = "JPEG (30% quality)", CaptureFormat = EM_NET_CAPTURE_FORMATS.JPEG_30 }
            };
            OutputTypes = outputTypes;
        }
    }
}
