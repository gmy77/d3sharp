using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace D3Sharp.Net.Packets.Protocol.ChannelInvitation
{
    [Service(serviceID: -1, serviceHash: 0xf084fc20, method: 0x1)]
    public class SubscribeRequest : PacketIn
    {
        public SubscribeRequest(Header header, IEnumerable<byte> payload)
            : base(header, payload)
        {
            this.Request = bnet.protocol.channel_invitation.SubscribeRequest.CreateBuilder().MergeFrom(this.Payload.ToArray()).Build();
        }
    }
}
