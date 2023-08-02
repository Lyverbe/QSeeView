using QSeeView.Tools.Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace QSeeView.Tools
{
    [DataContract]
    public partial class Settings
    {
        private IList<ChannelInfoModel> _channelsInfo;

        public Settings(int channelsCount)
        {
            ChannelsInfo = new List<ChannelInfoModel>();
            InitializeChannelsInfo(channelsCount);

            DownloadFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            IsConvertingToAvi = true;
            NightFilesStartHour = 23;
            NightFilesEndHour = 6;
        }

        [DataMember]
        public IList<ChannelInfoModel> ChannelsInfo
        {
            get => _channelsInfo;
            set => _channelsInfo = new List<ChannelInfoModel>(value);
        }

        [DataMember]
        public string DownloadFolder { get; set; }
        [DataMember]
        public string DeviceIp { get; set; }
        [DataMember]
        public ushort DevicePort { get; set; }
        [DataMember]
        public string Username { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public bool IsAutomaticLogin { get; set; }
        [DataMember]
        public bool IsConvertingToAvi { get; set; }
        [DataMember]
        public bool IsQueryIgnoringNightFiles { get; set; }
        [DataMember]
        public bool IsAutoQueryAtStartup { get; set; }
        [DataMember]
        public bool IsResettingPlaybackSpeed { get; set; }
        [DataMember]
        public int NightFilesStartHour { get; set; }
        [DataMember]
        public int NightFilesEndHour { get; set; }
        [DataMember]
        public string FfmpegPath { get; set; }
        [DataMember]
        public int StartDatesOffset { get; set; }

        private void InitializeChannelsInfo(int channelsCount)
        {
            ChannelsInfo.Clear();
            for (var channelId = 0; channelId < channelsCount; channelId++)
                ChannelsInfo.Add(new ChannelInfoModel(channelId, string.Empty, true));
        }
    }
}
