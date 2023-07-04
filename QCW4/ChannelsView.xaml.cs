using System.Windows;

namespace QCW4
{
    public partial class ChannelsView : Window
    {
        ChannelsViewModel _viewModel;

        public ChannelsView()
        {
            InitializeComponent();

            _viewModel = new ChannelsViewModel();
            DataContext = _viewModel;

            _viewModel.Close += (s, e) => Close();
        }
    }
}
