using NetSDKCS;
using System;
using System.Text;

namespace QSeeView.Models
{
    public class LogModel
    {
        public LogModel(uint logId, NET_DEVICE_LOG_ITEM_EX item)
        {
            Id = logId;
            Time = new DateTime((int)item.stuOperateTime.Year, (int)item.stuOperateTime.Month, (int)item.stuOperateTime.Day,
                (int)item.stuOperateTime.Hour, (int)item.stuOperateTime.Minute, (int)item.stuOperateTime.Second);
            Event = Encoding.UTF8.GetString(item.szLogContext);
            Detail = Encoding.UTF8.GetString(item.szDetailContext);
        }

        public uint Id { get; }
        public DateTime Time { get; }
        public string Event { get; }
        public string Detail { get; }
    }
}
