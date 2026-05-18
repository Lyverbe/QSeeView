using QSeeView.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace QSeeView.Views
{
    public class AddAccountViewModel : INotifyPropertyChanged
    {
        public event EventHandler<bool> Close;

        public event PropertyChangedEventHandler PropertyChanged;

        private string _name;
        private AccountModel _selectedAccount;
        private string _password;

        public AddAccountViewModel(IEnumerable<AccountModel> existingAccounts)
        {
            OkCommand = new RelayCommand(() => Close?.Invoke(this, true), IsOkEnabled);
            CancelCommand = new RelayCommand(() => Close?.Invoke(this, false));

            ExistingAccounts = existingAccounts;
            SelectedAccount = ExistingAccounts.First();
        }

        public ICommand OkCommand { get; }
        public ICommand CancelCommand { get; }

        public IEnumerable<AccountModel> ExistingAccounts { get; }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public AccountModel SelectedAccount
        {
            get => _selectedAccount;
            set
            {
                _selectedAccount = value;
                OnPropertyChanged(nameof(SelectedAccount));
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private bool IsOkEnabled() => !string.IsNullOrEmpty(Name) && !ExistingAccounts.Any(account => account.Name == Name) && !string.IsNullOrEmpty(Password);
   }
}