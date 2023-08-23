using QSeeView.Tools.Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace QSeeView.Tools
{
    [DataContract]
    public partial class Settings
    {
        public Settings(int channelsCount)
        {
            ChannelsInfo = new List<ChannelInfoModel>();
            InitializeChannelsInfo(channelsCount);

            DownloadFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            IsConvertingToAvi = true;
            NightFilesStartHour = 23;
            NightFilesEndHour = 6;
            FileNamesPattern = "%Y-%M-%D_%Hh%Nm%S_ch%c";
            LiveViewSize = 2;
        }

        [DataMember]
        public IList<ChannelInfoModel> ChannelsInfo { get; set; }
        [DataMember]
        public string DownloadFolder { get; set; }
        [DataMember]
        public string DeviceIp { get; set; }
        [DataMember]
        public ushort DevicePort { get; set; }
        [DataMember]
        public string Username { get; set; }
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
        [DataMember]
        public string FileNamesPattern { get; set; }
        [DataMember]
        public string EncodedPassword { get; set; }
        [DataMember]
        public int LiveViewSize { get; set; }
        [DataMember]
        public bool IsAutoOpenDownloads { get; set; }
        [DataMember]
        public bool DoPlayDownloadsCompleteSound { get; set; }
        [DataMember]
        public int LastPlaybackCaptureFormat { get; set; }

        public string Password { get; set; }

        private void InitializeChannelsInfo(int channelsCount)
        {
            ChannelsInfo.Clear();
            for (var channelId = 0; channelId < channelsCount; channelId++)
                ChannelsInfo.Add(new ChannelInfoModel(channelId, string.Empty, true));
        }

        [OnSerializing]
        private void OnSerializing(StreamingContext context)
        {
            var bytes = Encoding.UTF8.GetBytes(Password);
            EncodedPassword = Convert.ToBase64String(bytes);
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            DecodePassword();
            if (string.IsNullOrEmpty(DownloadFolder))
                DownloadFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (NightFilesStartHour == 0 && NightFilesEndHour == 0)
            {
                NightFilesStartHour = 23;
                NightFilesEndHour = 6;
            }
            if (string.IsNullOrEmpty(FileNamesPattern))
                FileNamesPattern = "%Y-%M-%D_%Hh%Nm%S_ch%c";
            if (LiveViewSize == 0)
                LiveViewSize = 2;
        }

        private void DecodePassword()
        {
            if (!string.IsNullOrEmpty(EncodedPassword))
            {
                var encodedPassword = Convert.FromBase64String(EncodedPassword);
                var decoded_char = Encoding.UTF8.GetChars(encodedPassword, 0, encodedPassword.Length);
                Password = new string(decoded_char);
            }
            else
                Password = string.Empty;
        }
    }
}
