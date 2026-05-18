using NetSDKCS;
using System.Collections.Generic;

namespace QSeeView.Models
{
    public class AccountGroupModel
    {
        public AccountGroupModel(NET_USER_GROUP_INFO_EX2 groupInfo)
        {
            ID = groupInfo.dwID;
            Name = groupInfo.name;
            Rights = new List<uint>(groupInfo.rights);
            Memo = groupInfo.memo;
        }

        public uint ID { get; }
        public string Name { get; }
        public IEnumerable<uint> Rights { get; }
        public string Memo { get; }
    }
}
