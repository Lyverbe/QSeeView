using QSeeView.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace QSeeView.ViewModels
{
    public class LiveViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int _viewRowsCount;
        private int _viewColumnsCount;

        public LiveViewModel(int channelsCount)
        {
            LiveMonitors = new List<LiveMonitorModel>();
            foreach (var channelInfo in App.Settings.ChannelsInfo.Where(info => !string.IsNullOrEmpty(info.Name)))
                LiveMonitors.Add(new LiveMonitorModel(channelInfo.ChannelId));

            MaximizeRowsColumnsCount(channelsCount);
        }

        public int ViewRowsCount
        {
            get => _viewRowsCount;
            set
            {
                _viewRowsCount = value;
                OnPropertyChanged(nameof(ViewRowsCount));
            }
        }

        public int ViewColumnsCount
        {
            get => _viewColumnsCount;
            set
            {
                _viewColumnsCount = value;
                OnPropertyChanged(nameof(ViewColumnsCount));
            }
        }

        public IList<LiveMonitorModel> LiveMonitors { get; set; }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public void MaximizeRowsColumnsCount(int channelsCount)
        {
            ViewRowsCount = (int)Math.Floor(Math.Sqrt(channelsCount));
            ViewColumnsCount = channelsCount / ViewRowsCount;
        }
    }
}
