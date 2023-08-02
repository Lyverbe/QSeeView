using System.Runtime.Serialization;

namespace QSeeView.Tools.Models
{
    [DataContract]
    public sealed class ChannelInfoModel
    {
        public ChannelInfoModel(int channelId, string name, bool isLandscape)
        {
            ChannelId = channelId;
            Name = name;
            IsLandscape = isLandscape;
        }

        [DataMember]
        public int ChannelId { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public bool IsLandscape { get; set; }
    }
}
