using QSeeView.Models;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace QSeeView.ViewModels
{
    public class HardDiskInfoViewModel
    {
        public event EventHandler Close;

        public HardDiskInfoViewModel()
        {
            CloseCommand = new RelayCommand(() => Close?.Invoke(this, EventArgs.Empty));
        }

        public ICommand CloseCommand { get; }

        public ObservableCollection<HardDiskInfoModel> HardDiskInfo { get; set; }
    }
}
