using QSeeView.ViewModels;
using System.Linq;
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

            _viewModel.Close += (s, e) => CloseIfAllowed();
        }

        public void CloseIfAllowed()
        {
            if (_viewModel.Channels.Any(channel => channel.IsVisibleInList))
                Close();
            else
                MessageBox.Show("You must have at least 1 channel active.", "Channels filter");
        }
    }
}
