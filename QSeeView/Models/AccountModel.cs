using NetSDKCS;
using System.Collections.Generic;
using System.ComponentModel;

namespace QSeeView.Models
{
    public class AccountModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private NET_USER_INFO_NEW _userInfo;

        private string _name;

        public AccountModel(NET_USER_INFO_NEW userInfo)
        {
            UserInfo = userInfo;
        }

        public NET_USER_INFO_NEW UserInfo
        {
            get => _userInfo;
            set
            {
                _userInfo = value;
                Name = _userInfo.name;
                Rights = new List<uint>(_userInfo.rights);
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
        public uint GroupId => UserInfo.dwGroupID;
        public string Memo => UserInfo.memo;
        public IEnumerable<uint> Rights { get; private set; }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}