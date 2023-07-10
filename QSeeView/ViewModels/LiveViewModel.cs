using QSeeView.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QSeeView.ViewModels
{
    public class LiveViewModel
    {
        public LiveViewModel(int channelsCount)
        {
            LiveMonitors = new List<LiveMonitorModel>();
            foreach (var channelInfo in App.Settings.ChannelsInfo.Where(info => !string.IsNullOrEmpty(info.Name)))
                LiveMonitors.Add(new LiveMonitorModel(channelInfo.ChannelId));

            ViewRowsCount = (int)Math.Floor(Math.Sqrt(channelsCount));
            ViewColumnsCount = channelsCount / ViewRowsCount;
        }

        public int ViewRowsCount { get; }
        public int ViewColumnsCount { get; }

        public IList<LiveMonitorModel> LiveMonitors { get; set; }
    }
}
