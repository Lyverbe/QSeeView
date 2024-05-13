using QSeeView.Tools;
using QSeeView.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace QSeeView.Views
{
    public partial class LoginView : Window
    {
        private LoginViewModel _viewModel;

        public LoginView()
        {
            InitializeComponent();

            _viewModel = new LoginViewModel();
            DataContext = _viewModel;

            _viewModel.Close += ViewModel_Close;
        }

        private void ViewModel_Close(object sender, bool isOkClicked)
        {
            if (isOkClicked)
            {
                App.Settings.DeviceIp = _viewModel.DeviceIp;
                App.Settings.DevicePort = _viewModel.DevicePort;
                App.Settings.Username = _viewModel.Username;
                App.Settings.Password = _viewModel.Password;
                App.Settings.DeviceModel = _viewModel.DeviceModel;
            }

            DialogResult = isOkClicked;
        }

        private void TextBox_PreviewTextInput_NumbersOnly(object sender, TextCompositionEventArgs e)
        {
            if (e.Text[0] < '0' || e.Text[0] > '9')
                e.Handled = true;
        }
    }
}
