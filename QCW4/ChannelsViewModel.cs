using QCW4.Properties;
using System;
using System.ComponentModel;
using System.Windows.Input;

namespace QCW4
{
    class ChannelsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler Close;

        public ChannelsViewModel()
        {
            CloseCommand = new RelayCommand(() => Close?.Invoke(this, new EventArgs()));
        }

        public ICommand CloseCommand { get; private set; }

        public string Channel1Name
        {
            get => Settings.Default.Channel1Name;
            set => Settings.Default.Channel1Name = value;
        }
        public string Channel2Name
        {
            get => Settings.Default.Channel2Name;
            set => Settings.Default.Channel2Name = value;
        }
        public string Channel3Name
        {
            get => Settings.Default.Channel3Name;
            set => Settings.Default.Channel3Name = value;
        }
        public string Channel4Name
        {
            get => Settings.Default.Channel4Name;
            set => Settings.Default.Channel4Name = value;
        }

        public bool IsChannel1Landscape
        {
            get => Settings.Default.IsChannel1Landscape;
            set => Settings.Default.IsChannel1Landscape = value;
        }
        public bool IsChannel2Landscape
        {
            get => Settings.Default.IsChannel2Landscape;
            set => Settings.Default.IsChannel2Landscape = value;
        }
        public bool IsChannel3Landscape
        {
            get => Settings.Default.IsChannel3Landscape;
            set => Settings.Default.IsChannel3Landscape = value;
        }
        public bool IsChannel4Landscape
        {
            get => Settings.Default.IsChannel4Landscape;
            set => Settings.Default.IsChannel4Landscape = value;
        }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
