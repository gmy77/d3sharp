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

            var builder = bnet.protocol.channel.ChannelState.CreateBuilder()
                .SetPrivacyLevel(bnet.protocol.channel.ChannelState.Types.PrivacyLevel.PRIVACY_LEVEL_OPEN)
                .SetMaxMembers(8)
                .SetMinMembers(1)
                .SetMaxInvitations(12)
                .SetName("d3sharp test channel"); // Note: cap log doesn't set this optional field
            this.State = builder.Build();
        }

        public void AddUser(IClient client)
        {
            var identity = client.Identity;
            var user = bnet.protocol.channel.Member.CreateBuilder()
                .SetIdentity(identity)
                .SetState(bnet.protocol.channel.MemberState.CreateBuilder()
                    .AddRole(2)
                    .SetPrivileges(0xFBFF) // 64511
                    .Build())
                .Build();
            this.Members.Add(user);

            var builder = bnet.protocol.channel.AddNotification.CreateBuilder()
                .SetChannelState(this.State)
                .SetSelf(user);
            
            foreach (var m in this.Members/*.Where(m => m.Identity != user.Identity)*/)
            {
                builder.AddMember(m);
            }

            client.CurrentChannel = this;
            client.CallMethod(this.ID, bnet.protocol.channel.ChannelSubscriber.Descriptor.FindMethodByName("NotifyAdd"), builder.Build(), null, done =>{});
        }
        
        public bool HasUser(IClient client)
        {
            return this.Members.Any(m => m.Identity == client.Identity);
        }
        
        /*public void Close()
        {
            RemoveAllUsers();
        }
        
        public void RemoveAllUsers()
        {
            // Need a way to iterate clients on the server to send a NotifyRemove
            // and then call RemoveUser on them
            this.Members.Clear();
        }*/
        
        public void RemoveUser(IClient client)
        {
            var identity = client.Identity;
            var builder = bnet.protocol.channel.RemoveNotification.CreateBuilder()
                .SetMemberId(identity.ToonId);
            this.Members.RemoveAll(m => identity == m.Identity);
            client.CurrentChannel = null;
            ((IRpcChannel) client).CallMethod(bnet.protocol.channel.ChannelSubscriber.Descriptor.FindMethodByName("NotifyRemove"), null, builder.Build(), null, done =>{});
        }
    }
}
