using QSeeView.Models;
using QSeeView.Tools;
using QSeeView.ViewModels;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace QSeeView.Views
{
    public partial class HardDisksInfoView : Window
    {
        private HardDisksInfoViewModel _viewModel;

        public HardDisksInfoView(IDeviceManager deviceManager)
        {
            InitializeComponent();

            _viewModel = new HardDisksInfoViewModel();
            DataContext = _viewModel;

            _viewModel.Close += (s, e) => DialogResult = true;

            _viewModel.HardDiskInfo = new ObservableCollection<HardDiskInfoModel>(deviceManager.GetHardDisksInfo().Where(info => info.Capacity > 0));
        }
    }
}
