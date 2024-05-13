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
        private bool _areControlsVisible;

        public LiveViewModel(IDeviceManager deviceManager)
        {
            LiveMonitors = new List<LiveMonitorModel>();
            for (var channelId = 0; channelId < Math.Pow(App.Settings.LiveViewSize, 2); channelId++)
                LiveMonitors.Add(new LiveMonitorModel(deviceManager, channelId));
            MaximizeRowsColumnsCount();

            _areControlsVisible = true;
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

        public bool AreControlsVisible
        {
            get => _areControlsVisible;
            set
            {
                _areControlsVisible = value;
                OnPropertyChanged(nameof(AreControlsVisible));
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
