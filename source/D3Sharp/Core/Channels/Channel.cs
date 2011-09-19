using System.Collections.Generic;
using System.Linq;
using D3Sharp.Net;
using Google.ProtocolBuffers;

namespace D3Sharp.Core.Channels
{
    public class Channel
    {
        public ulong ID { get; private set; }
        public bnet.protocol.EntityId BnetEntityID { get; private set; }
        public bnet.protocol.channel.ChannelState State { get; private set; }

        public List<bnet.protocol.channel.Member> Members = new List<bnet.protocol.channel.Member>();

        public Channel(ulong id)
        {
            this.ID = id;
            this.BnetEntityID = bnet.protocol.EntityId.CreateBuilder().SetLow(id).SetHigh(0x0).Build();

            var builder = bnet.protocol.channel.ChannelState.CreateBuilder();
            builder.SetPrivacyLevel(bnet.protocol.channel.ChannelState.Types.PrivacyLevel.PRIVACY_LEVEL_OPEN);
            builder.SetMaxMembers(8);
            builder.SetMinMembers(1);
            builder.SetMaxInvitations(12);
            builder.SetName("d3sharp test channel");
            this.State = builder.Build();
        }

        public void AddUser(IClient client)
        {
            var identityBuilder = bnet.protocol.Identity.CreateBuilder();
            identityBuilder.SetAccountId(client.Account.BnetAccountID);
            identityBuilder.SetGameAccountId(client.Account.BnetGameAccountID);
            if (client.Account.Toons.Count > 0) identityBuilder.SetToonId(client.Account.Toons.First().Value.BnetEntityID);
            var identity = identityBuilder.Build();

            var user = bnet.protocol.channel.Member.CreateBuilder().SetIdentity(identity).SetState(bnet.protocol.channel.MemberState.CreateBuilder().AddRole(2).Build()).Build();
            this.Members.Add(user);

            var builder = bnet.protocol.channel.AddNotification.CreateBuilder().SetChannelState(this.State).SetSelf(user);
            
            foreach (var m in this.Members.Where(m => m.Identity != user.Identity))
            {
                builder.AddMember(m);
            }

            ((IRpcChannel) client).CallMethod(bnet.protocol.channel.ChannelSubscriber.Descriptor.FindMethodByName("NotifyAdd"), null, builder.Build(), null, done =>{});
        }
    }
}
