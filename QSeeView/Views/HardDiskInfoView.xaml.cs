using QSeeView.Models;
using QSeeView.Tools;
using QSeeView.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace QSeeView.Views
{
    public partial class HardDiskInfoView : Window
    {
        private HardDiskInfoViewModel _viewModel;

        public HardDiskInfoView(IDeviceManager deviceManager)
        {
            InitializeComponent();

            _viewModel = new HardDiskInfoViewModel();
            DataContext = _viewModel;

            _viewModel.Close += (s, e) => DialogResult = true;

            FillHardDiskInfo(deviceManager);
        }

        private void FillHardDiskInfo(IDeviceManager deviceManager)
        {
            _viewModel.HardDiskInfo = new ObservableCollection<HardDiskInfoModel>(deviceManager.GetHardDiskInfo());
        }
    }
}
