using QSeeView.ViewModels;
using System.Windows;

namespace QSeeView.Views
{
    public partial class FilterChannelsView : Window
    {
        private FilterChannelsViewModel _viewModel;

        public FilterChannelsView()
        {
            InitializeComponent();

            _viewModel = new FilterChannelsViewModel();
            DataContext = _viewModel;

            _viewModel.Close += (s, e) => Close();
        }
    }
}
