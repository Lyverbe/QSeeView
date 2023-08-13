using QSeeView.Models;
using QSeeView.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace QSeeView.ViewModels
{
    public class LiveViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int _viewRowsCount;
        private int _viewColumnsCount;

        public LiveViewModel(IDeviceManager deviceManager)
        {
            LiveMonitors = new List<LiveMonitorModel>();
            //foreach (var channelInfo in App.Settings.ChannelsInfo)
            //    LiveMonitors.Add(new LiveMonitorModel(deviceManager, channelInfo.ChannelId));
            for (var channelId = 0; channelId < Math.Pow(App.Settings.LiveViewSize, 2); channelId++)
            {
                LiveMonitors.Add(new LiveMonitorModel(deviceManager, channelId));
            }

            MaximizeRowsColumnsCount();
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

        public void MaximizeRowsColumnsCount()
        {
            ViewRowsCount = (int)Math.Floor(Math.Sqrt(LiveMonitors.Count));
            ViewColumnsCount = LiveMonitors.Count / ViewRowsCount;
        }
    }
}
