namespace QSeeView.Models
{
    public class LiveMonitorModel
    {
        public LiveMonitorModel(int channelId)
        {
            ChannelId = channelId;

            var channelInfo = App.Settings.ChannelsInfo[ChannelId];
            Width = channelInfo.IsLandscape ? App.HDSize.Width : App.HDSize.Height;
            Height = channelInfo.IsLandscape ? App.HDSize.Height : App.HDSize.Width;
        }

        public int ChannelId { get; }

        public double Width { get; }
        public double Height { get; }
    }
}
