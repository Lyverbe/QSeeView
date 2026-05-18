using QSeeView.Models;
using QSeeView.Tools;
using QSeeView.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace QSeeView.Views
{
    public partial class AccountsView : Window
    {
        private IDeviceManager _deviceManager;
        private AccountsViewModel _viewModel;

        public AccountsView(IDeviceManager deviceManager)
        {
            InitializeComponent();

            _deviceManager = deviceManager;

            _viewModel = new AccountsViewModel();
            DataContext = _viewModel;

            _viewModel.Close += (s, e) => DialogResult = true;
            _viewModel.AddAccount += (s, e) => AddAccount();
            _viewModel.DeleteAccount += (s, e) => DeleteAccount();
            _viewModel.UpdateAccount += (s, e) => UpdateAccount();

            _viewModel.ConfirmDiscardChangesFunc = ConfirmDiscardChanges;

            IList<AccountModel> accounts = null;
            IList<RightModel> rights = null;
            IList<AccountGroupModel> groups = null;
            deviceManager.GetAccounts(ref accounts, ref rights, ref groups);
            _viewModel.Accounts = new ObservableCollection<AccountModel>(accounts.OrderBy(account => account.Name));
            _viewModel.Rights = new ObservableCollection<RightModel>(rights);
            _viewModel.Groups = new ObservableCollection<AccountGroupModel>(groups);
        }

        protected override void OnContentRendered(EventArgs e)
        {
            _viewModel.SelectedAccount = _viewModel.Accounts.FirstOrDefault();
            base.OnContentRendered(e);
        }

        private void AddAccount()
        {
            var view = new AddAccountView(_viewModel.Accounts)
            {
                Owner = this
            };
            if (view.ShowDialog() == true)
            {
                var userInfo = view.GetUserInfo();
                var success = _deviceManager.AddAccount(userInfo);
                if (success)
                {
                    var model = new AccountModel(userInfo);
                    _viewModel.Accounts.Add(model);
                    _viewModel.SelectedAccount = model;
                }
                else
                    MessageBox.Show("Operation failed.\n\n" + _deviceManager.GetLastError(), Title);
            }
        }

        private void DeleteAccount()
        {
            var answer = MessageBox.Show("Delete account \"" + _viewModel.SelectedAccount.Name + "\".\n\nAre you sure?",
                         Title, MessageBoxButton.YesNo);
            if (answer != MessageBoxResult.Yes)
                return;

            var success = _deviceManager.DeleteAccount(_viewModel.SelectedAccount.UserInfo);
            if (success)
            {
                _viewModel.Accounts.Remove(_viewModel.SelectedAccount);
                _viewModel.SelectedAccount = _viewModel.Accounts.First();
            }
            else
                MessageBox.Show("Operation failed.\n\n" + _deviceManager.GetLastError(), Title);
        }

        private bool UpdateAccount()
        {
            var originalUserInfo = _viewModel.SelectedAccount.UserInfo;
            var newUserInfo = originalUserInfo;
            var success = true;
            if (_viewModel.AreDetailsDirty())
            {
                newUserInfo.name = _viewModel.Name;
                newUserInfo.dwGroupID = _viewModel.SelectedGroup.ID;
                newUserInfo.memo = _viewModel.Memo;
                newUserInfo.rights = _viewModel.Rights.Where(right => right.IsOwned).Select(right => right.ID).Cast<uint>().ToArray();
                newUserInfo.dwRightNum = (uint)newUserInfo.rights.Length;
                Array.Resize(ref newUserInfo.rights, 1024);
                success = _deviceManager.UpdateAccount(originalUserInfo, newUserInfo);
            }

            if (success && _viewModel.IsPasswordDirty())
            {
                originalUserInfo.passWord = _viewModel.OldPassword;

                newUserInfo = originalUserInfo;
                newUserInfo.name = _viewModel.Name;
                newUserInfo.passWord = _viewModel.NewPassword;
                success = _deviceManager.UpdatePassword(originalUserInfo, newUserInfo);
            }

            if (success)
            {
                _viewModel.SelectedAccount.UserInfo = newUserInfo;
                _viewModel.IsPasswordChanged = false;
                _viewModel.OldPassword = string.Empty;
                _viewModel.NewPassword = string.Empty;
            }
            else
                MessageBox.Show("Operation failed.\n\n" + _deviceManager.GetLastError(), Title);

            return success;
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

        private MessageBoxResult ConfirmDiscardChanges(AccountModel model)
        {
            var answer = MessageBox.Show("Changes have not been saved.  Do you wish to save them now?", Title,
                                         MessageBoxButton.YesNoCancel);
            if (answer == MessageBoxResult.Yes)
                UpdateAccount();
            return answer;
        }
    }
}
