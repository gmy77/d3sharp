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
            this.BnetEntityID = bnet.protocol.EntityId.CreateBuilder().SetHigh(433661094618860925).SetLow(11233645142038554527).Build();

            var builder = bnet.protocol.channel.ChannelState.CreateBuilder()
                .SetPrivacyLevel(bnet.protocol.channel.ChannelState.Types.PrivacyLevel.PRIVACY_LEVEL_OPEN_INVITATION)
                .SetMaxMembers(8)
                .SetMinMembers(1)
                .SetMaxInvitations(12);
                //.SetName("d3sharp test channel"); // Note: cap log doesn't set this optional field
            this.State = builder.Build();
        }

        public void NotifyChannelState(Client client)
        {
            var field1 =
                bnet.protocol.presence.Field.CreateBuilder().SetKey(
                    bnet.protocol.presence.FieldKey.CreateBuilder().SetProgram(16974).SetGroup(3).SetField(3).SetIndex(0)
                        .Build()).SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetBoolValue(true).Build()).Build();

            var field2 =
                bnet.protocol.presence.Field.CreateBuilder().SetKey(
                    bnet.protocol.presence.FieldKey.CreateBuilder().SetProgram(16974).SetGroup(3).SetField(10).SetIndex(0)
                        .Build()).SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetIntValue(1315530390868296).Build()).Build();

            var field3 =
                bnet.protocol.presence.Field.CreateBuilder().SetKey(
                    bnet.protocol.presence.FieldKey.CreateBuilder().SetProgram(16974).SetGroup(3).SetField(11).SetIndex(0)
                        .Build()).SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetMessageValue(
                                ByteString.CopyFrom(new byte[]
                                                        {
                                                            0x9, 0x46, 0xee, 0x00, 0x00, 0x00, 0x00, 0x00, 0x4,
                                                            0x11, 0xdd, 0xb4, 0x63, 0xe7, 0x82, 0x44, 0x68, 0x4e
                                                        })).Build()).Build();


            var fieldOperation1 = bnet.protocol.presence.FieldOperation.CreateBuilder().SetField(field1).Build();
            var fieldOperation2 = bnet.protocol.presence.FieldOperation.CreateBuilder().SetField(field2).Build();
            var fieldOperation3 = bnet.protocol.presence.FieldOperation.CreateBuilder().SetField(field3).Build();

            var state =
                bnet.protocol.presence.ChannelState.CreateBuilder().SetEntityId(this.BnetEntityID).AddFieldOperation(
                    fieldOperation1).AddFieldOperation(fieldOperation2).AddFieldOperation(fieldOperation3).Build();


            var channelState = bnet.protocol.channel.ChannelState.CreateBuilder().SetExtension(bnet.protocol.presence.ChannelState.Presence, state);
            var builder = bnet.protocol.channel.UpdateChannelStateNotification.CreateBuilder().SetStateChange(channelState);

            client.CallMethod(bnet.protocol.channel.ChannelSubscriber.Descriptor.FindMethodByName("NotifyUpdateChannelState"), builder.Build());
        }

        public void Add(Client client, ulong objectId)
        {
            var builder = bnet.protocol.channel.AddNotification.CreateBuilder();
            var identity = bnet.protocol.Identity.CreateBuilder();
            identity.SetAccountId(client.Account.BnetAccountID);
            identity.SetGameAccountId(client.Account.BnetGameAccountID);
            if (client.Account.Toons.Count > 0) identity.SetToonId(client.Account.Toons.First().Value.BnetEntityID);

            var state = bnet.protocol.channel.MemberState.CreateBuilder()
                .AddRole(2)
                .SetPrivileges(64511);
            var selfBuilder = bnet.protocol.channel.Member.CreateBuilder().SetIdentity(identity.Build()).SetState(state.Build());
            var self = selfBuilder.Build();
            builder.SetSelf(self);

            if (client.Account.Toons.Count > 0)
            {
                var memberIdentity = bnet.protocol.Identity.CreateBuilder();
                memberIdentity.SetToonId(client.Account.Toons.First().Value.BnetEntityID);

                var memberState = bnet.protocol.channel.MemberState.CreateBuilder().AddRole(2).SetPrivileges(64511);

                var member = bnet.protocol.channel.Member.CreateBuilder().SetIdentity(memberIdentity).SetState(memberState.Build());
                builder.AddMember(self);
            }

            builder.SetChannelState(this.State);
            client.CallMethod(bnet.protocol.channel.ChannelSubscriber.Descriptor.FindMethodByName("NotifyAdd"), builder.Build(), objectId);
        }

        public bool HasUser(Client client)
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
        
        public void RemoveUser(Client client)
        {
            var identity = client.Identity;
            var builder = bnet.protocol.channel.RemoveNotification.CreateBuilder()
                .SetMemberId(identity.ToonId);
            this.Members.RemoveAll(m => identity == m.Identity);
            client.CurrentChannel = null;
            client.CallMethod(bnet.protocol.channel.ChannelSubscriber.Descriptor.FindMethodByName("NotifyRemove"), builder.Build(), this.ID);
        }
    }
}
