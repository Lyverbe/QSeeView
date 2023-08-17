using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Windows.Input;

namespace QSeeView.Tools.Models
{
    [DataContract]
    public sealed class ChannelInfoModel : INotifyPropertyChanged
    {
        public event EventHandler ListVisibilityChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        private bool _isLandscape;
        private string _name;
        private bool _isOnline;
        private bool _isVisibleInList;

        public ChannelInfoModel(int channelId, string name, bool isLandscape)
        {
            ChannelId = channelId;
            Name = name;
            IsLandscape = isLandscape;
            IsOnline = true;
            IsVisibleInList = true;

            Initialize();
        }
        public ChannelInfoModel()
        {
            ChannelId = -1;
            Name = "(offline)";

            Initialize();
        }

        public ICommand SetOrientationCommand { get; private set; }
        public ICommand SetOnlineCommand { get; private set; }

        [DataMember]
        public int ChannelId { get; set; }

        [DataMember]
        public string Name
        {
            get => string.IsNullOrEmpty(_name) ? ChannelId.ToString() : _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        [DataMember]
        public bool IsLandscape
        {
            get => _isLandscape;
            set
            {
                _isLandscape = value;
                OnPropertyChanged(nameof(IsLandscape));
            }
        }

        [DataMember]
        public bool IsOnline
        {
            get => _isOnline;
            set
            {
                _isOnline = value;
                OnPropertyChanged(nameof(IsOnline));
                OnPropertyChanged(nameof(Name));
            }
        }

        [DataMember]
        public bool IsVisibleInList
        {
            get => _isVisibleInList;
            set
            {
                _isVisibleInList = value;
                OnPropertyChanged(nameof(IsVisibleInList));
                ListVisibilityChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        [OnDeserialized]
        private void OnDeserialized(StreamingContext streamingContext) => Initialize();

        private void Initialize()
        {
            SetOrientationCommand = new RelayCommand(() => IsLandscape = !IsLandscape);
            SetOnlineCommand = new RelayCommand(() => IsOnline = !IsOnline);
        }
    }
}
