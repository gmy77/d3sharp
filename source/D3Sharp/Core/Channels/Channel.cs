using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace D3Sharp.Core.Channels
{
    public class Channel
    {
        public ulong ID { get; private set; }
        public bnet.protocol.EntityId BnetEntityID { get; private set; }
        public bnet.protocol.channel.ChannelState State { get; private set; }

        public Channel(ulong id, bnet.protocol.channel.ChannelState state=null)
        {
            this.ID = id;
            this.BnetEntityID = bnet.protocol.EntityId.CreateBuilder().SetLow(id).SetHigh(0x0).Build();

            var builder = bnet.protocol.channel.ChannelState.CreateBuilder();
            builder.SetPrivacyLevel(bnet.protocol.channel.ChannelState.Types.PrivacyLevel.PRIVACY_LEVEL_OPEN);
            builder.SetMaxMembers(8);
            builder.SetMinMembers(1);
            builder.SetMaxInvitations(12);
            builder.SetName("d3sharp test channel");
        }
    }
}
