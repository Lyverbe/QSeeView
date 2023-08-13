using QSeeView.Models;
using System;
using System.Collections.Generic;

namespace QSeeView.Tools
{
    public class QueryManager
    {
        private IDeviceManager _deviceManager;

        public QueryManager(IDeviceManager deviceManager)
        {
            _deviceManager = deviceManager;
        }

        public IEnumerable<RecordFileInfoModel> Result { get; private set; }
        public IEnumerable<RecordFileInfoModel> FilteredResult { get; private set; }

        private DateTime LastQueryStart { get; set; }
        private DateTime LastQueryEnd { get; set; }
        private bool IsLastQueryIgnoringFile { get; set; }

        public void Run(DateTime start, DateTime end)
        {
            if (start == LastQueryStart && end == LastQueryEnd && App.Settings.IsQueryIgnoringNightFiles == IsLastQueryIgnoringFile)
                return;

            Result = _deviceManager.Query(start, end, App.Settings.IsQueryIgnoringNightFiles);

            LastQueryStart = start;
            LastQueryEnd = end;
            IsLastQueryIgnoringFile = App.Settings.IsQueryIgnoringNightFiles;
        }

        public void FilterResult()
        {
            var filteredRecords = new List<RecordFileInfoModel>();
            foreach (var record in Result)
            {
                var channelInfo = App.Settings.ChannelsInfo[record.Channel];
                if (channelInfo.IsVisibleInList)
                    filteredRecords.Add(record);
            }
            FilteredResult = filteredRecords;
        }
    }
}
