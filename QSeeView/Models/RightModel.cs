using NetSDKCS;
using System;
using System.ComponentModel;

namespace QSeeView.Models
{
    public class RightModel : INotifyPropertyChanged, IComparable<RightModel>
    {
        private bool _isOwned;

        public event PropertyChangedEventHandler PropertyChanged;

        public RightModel(NET_OPR_RIGHT_NEW right)
        {
            ID = right.dwID;
            Name = right.name;
            Memo = right.memo;
        }

        public uint ID { get; }
        public string Name { get; }
        public string Memo { get; }

        public bool IsOwned
        {
            get => _isOwned;
            set
            {
                _isOwned = value;
                OnPropertyChanged(nameof(IsOwned));
            }
        }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public int CompareTo(RightModel other) => (int)(ID - other.ID);
    }
}
