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

            var builder = bnet.protocol.channel.ChannelState.CreateBuilder();
            builder.SetPrivacyLevel(bnet.protocol.channel.ChannelState.Types.PrivacyLevel.PRIVACY_LEVEL_OPEN_INVITATION);
            builder.SetMaxMembers(8);
            builder.SetMinMembers(1);
            builder.SetMaxInvitations(12);
            //builder.SetName("d3sharp test channel");
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

            var state = bnet.protocol.presence.ChannelState.CreateBuilder().SetEntityId(this.BnetEntityID).AddFieldOperation(fieldOperation1).AddFieldOperation(fieldOperation2).AddFieldOperation(fieldOperation3).Build();
            var builder = bnet.protocol.channel.UpdateChannelStateNotification.CreateBuilder().SetStateChange((bnet.protocol.channel.ChannelState)state).Build();

            client.CallMethod(bnet.protocol.channel.ChannelSubscriber.Descriptor.FindMethodByName("NotifyUpdateChannelState"), null, builder, null, r => { });
        }

        public void Add(Client client)
        {
            var builder = bnet.protocol.channel.AddNotification.CreateBuilder();
            var identity = bnet.protocol.Identity.CreateBuilder();
            identity.SetAccountId(client.Account.BnetAccountID);
            identity.SetGameAccountId(client.Account.BnetGameAccountID);
            if (client.Account.Toons.Count > 0) identity.SetToonId(client.Account.Toons.First().Value.BnetEntityID);

            var state = bnet.protocol.channel.MemberState.CreateBuilder().AddRole(2).SetPrivileges(64511);
            var selfBuilder = bnet.protocol.channel.Member.CreateBuilder().SetIdentity(identity.Build()).SetState(state.Build());
            var self = selfBuilder.Build();
            builder.SetSelf(self);
            builder.AddMember(self);
            builder.SetChannelState(this.State);

            client.CallMethod(bnet.protocol.channel.ChannelSubscriber.Descriptor.FindMethodByName("NotifyAdd"), null, builder.Build(), null, r => { });
        }
    }
}
