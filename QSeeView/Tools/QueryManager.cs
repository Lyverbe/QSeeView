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

        private DateTime LastQueryStart { get; set; }
        private DateTime LastQueryEnd { get; set; }
        private bool IsLastQueryIgnoringFile { get; set; }

        public void Run(DateTime start, DateTime end)
        {
            if (start == LastQueryStart && end == LastQueryEnd && App.Settings.IsQueryIgnoringNightFiles == IsLastQueryIgnoringFile)
                return;

            Result = _deviceManager.Query(start, end);

            LastQueryStart = start;
            LastQueryEnd = end;
            IsLastQueryIgnoringFile = App.Settings.IsQueryIgnoringNightFiles;
        }

        public IList<RecordFileInfoModel> FilterResult()
        {
            var filteredRecords = new List<RecordFileInfoModel>();
            foreach (var record in Result)
            {
                var channelInfo = App.Settings.ChannelsInfo[record.Channel];
                var isValid = channelInfo.IsVisibleInList;

                if (isValid && App.Settings.IsQueryIgnoringNightFiles)
                {
                    if (App.Settings.NightFilesStartHour < App.Settings.NightFilesEndHour)  // Ex. Between 1h00 and 6h00
                    {
                        if (record.StartTime.Hour >= App.Settings.NightFilesStartHour && record.StartTime.Hour < App.Settings.NightFilesEndHour)
                            isValid = false;
                    }
                    else // Ex. Between 23h00 and 6h00
                    {
                        if (record.StartTime.Hour >= App.Settings.NightFilesStartHour || record.StartTime.Hour < App.Settings.NightFilesEndHour)
                            isValid = false;
                    }
                }

                if (isValid)
                    filteredRecords.Add(record);
            }

            return filteredRecords;
        }
    }
}
