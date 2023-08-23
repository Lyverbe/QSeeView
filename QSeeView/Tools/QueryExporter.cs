using QSeeView.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace QSeeView.Tools
{
    public class QueryExporter
    {
        public string LastError { get; private set; }

        public bool Export(string fileName, IList<RecordFileInfoModel> records)
        {
            if (!records.Any())
            {
                LastError = "No records to export";
                return false;
            }

            try
            {
                var csv = BuildCsv(records);
                File.WriteAllLines(fileName, csv);
            }
            catch (Exception exception)
            {
                LastError = exception.Message;
                return false;
            }

            return true;
        }

        private IEnumerable<string> BuildCsv(IList<RecordFileInfoModel> records)
        {
            var csv = new List<string>();
            csv.Add("#,Start time,End time,Channel,Channel name,Length");
            foreach (var record in records)
                csv.Add($"{record.ID},{record.StartTimeString},{record.EndTimeString},{record.Channel},{record.ChannelName},{record.Length}");
            return csv;
        }
    }
}
