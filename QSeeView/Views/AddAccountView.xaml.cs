using NetSDKCS;
using QSeeView.Models;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace QSeeView.Views
{
    public partial class AddAccountView : Window
    {
        private AddAccountViewModel _viewModel;

        public AddAccountView(IEnumerable<AccountModel> existingAccounts)
        {
            InitializeComponent();

            _viewModel = new AddAccountViewModel(existingAccounts);
            DataContext = _viewModel;

            _viewModel.Close += (s, isOkClicked) => DialogResult = isOkClicked;
        }

        public NET_USER_INFO_NEW GetUserInfo()
        {
            var modifiedUserInfo = _viewModel.SelectedAccount.UserInfo;
            modifiedUserInfo.name = _viewModel.Name;
            modifiedUserInfo.passWord = _viewModel.Password;
            return modifiedUserInfo;
        }

        private void TextBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            bool shiftPressed = (Keyboard.Modifiers & ModifierKeys.Shift) != 0;
            if ((e.Key >= Key.A && e.Key <= Key.Z) ||
                (!shiftPressed && e.Key >= Key.D0 && e.Key <= Key.D9) ||
                (e.Key == Key.Left || e.Key == Key.Right || e.Key == Key.Back || e.Key == Key.Tab))
            {
                e.Handled = false;
                return;
            }

            e.Handled = true;
        }
    }
}
