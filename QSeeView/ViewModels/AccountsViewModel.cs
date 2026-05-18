using QSeeView.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace QSeeView.ViewModels
{
    public class AccountsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler Close;
        public event EventHandler AddAccount;
        public event EventHandler DeleteAccount;
        public event EventHandler UpdateAccount;

        private AccountModel _selectedAccount;
        private string _name;
        private AccountGroupModel _selectedGroup;
        private string _memo;
        private bool _isPasswordChanged;
        private string _oldPassword;
        private string _newPassword;

        public AccountsViewModel()
        {
            CloseCommand = new RelayCommand(() => Close?.Invoke(this, EventArgs.Empty));
            AddAccountCommand = new RelayCommand(() => AddAccount?.Invoke(this, EventArgs.Empty));
            DeleteAccountCommand = new RelayCommand(() => DeleteAccount?.Invoke(this, EventArgs.Empty), CanDeleteAccount);
            UpdateCommand = new RelayCommand(() => UpdateAccount?.Invoke(this, EventArgs.Empty), CanUpdateAccount);
        }

        public ICommand CloseCommand { get; }
        public ICommand AddAccountCommand { get; }
        public ICommand DeleteAccountCommand { get; }
        public ICommand UpdateCommand { get; }

        public Func<AccountModel, MessageBoxResult> ConfirmDiscardChangesFunc { get; set; }

        public ObservableCollection<AccountModel> Accounts { get; set; }
        public ObservableCollection<RightModel> Rights { get; set; }
        public ObservableCollection<AccountGroupModel> Groups { get; set; }

        public AccountModel SelectedAccount
        {
            get => _selectedAccount;
            set
            {
                if (_selectedAccount == value)
                    return;

                if (CanUpdateAccount() && ConfirmDiscardChangesFunc != null)
                {
                    var answer = ConfirmDiscardChangesFunc(value);
                    if (answer == MessageBoxResult.Cancel)
                    {
                        var currentAccount = _selectedAccount;
                        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            _selectedAccount = null;
                            OnPropertyChanged(nameof(SelectedAccount));
                            _selectedAccount = currentAccount;
                            OnPropertyChanged(nameof(SelectedAccount));
                        }));
                        return;
                    }
                }

                _selectedAccount = value;
                OnPropertyChanged(nameof(SelectedAccount));
                UpdateAccountInfo();
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public AccountGroupModel SelectedGroup
        {
            get => _selectedGroup;
            set
            {
                _selectedGroup = value;
                OnPropertyChanged(nameof(SelectedGroup));
            }
        }

        public string Memo
        {
            get => _memo;
            set
            {
                _memo = value;
                OnPropertyChanged(nameof(Memo));
            }
        }

        public bool IsPasswordChanged
        {
            get => _isPasswordChanged;
            set
            {
                _isPasswordChanged = value;
                OnPropertyChanged(nameof(IsPasswordChanged));
            }
        }

        public string OldPassword
        {
            get => _oldPassword;
            set
            {
                _oldPassword = value;
                OnPropertyChanged(nameof(OldPassword));
            }
        }

        public string NewPassword
        {
            get => _newPassword;
            set
            {
                _newPassword = value;
                OnPropertyChanged(nameof(NewPassword));
            }
        }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void UpdateAccountInfo()
        {
            if (SelectedAccount == null)
            {
                Name = string.Empty;
                SelectedGroup = Groups.First();
                Memo = string.Empty;

                foreach (var right in Rights)
                    right.IsOwned = false;
            }
            else
            {
                Name = SelectedAccount.Name;
                SelectedGroup = Groups.First(group => group.ID == SelectedAccount.GroupId);
                Memo = SelectedAccount.Memo;

                foreach (var right in Rights)
                    right.IsOwned = SelectedAccount.Rights.Any(accountRight => right.ID == accountRight);
            }

            IsPasswordChanged = false;
            OldPassword = string.Empty;
            NewPassword = string.Empty;
        }

        private bool CanDeleteAccount()
        {
            if (SelectedAccount == null)
                return false;
            if (SelectedAccount.Name == "admin" || SelectedAccount.Name == "user")
                return false;
            return true;
        }

        private bool CanUpdateAccount() =>
            SelectedAccount != null && (AreDetailsDirty() || IsPasswordDirty());

        public bool AreDetailsDirty()
        {
            var selectedRights = Rights.Where(right => right.IsOwned).Select(right => right.ID).OrderBy(right => right);
            return (Name != SelectedAccount.Name || SelectedGroup.ID != SelectedAccount.GroupId || Memo != SelectedAccount.Memo ||
                    !selectedRights.SequenceEqual(SelectedAccount.Rights.Where(rightId => rightId != 0).OrderBy(right => right)));
        }

        public bool IsPasswordDirty() =>
            IsPasswordChanged && !string.IsNullOrEmpty(OldPassword) && !string.IsNullOrEmpty(NewPassword) && OldPassword != NewPassword;
    }
}