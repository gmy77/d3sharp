using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace D3Sharp.Net.Packets.Protocol.Friends
{
    [Service(serviceID: -1, serviceHash: 0x6f259a13, method: 0x1)]
    public class SubscribeToFriendsRequest : PacketIn
    {
        public SubscribeToFriendsRequest(Header header, IEnumerable<byte> payload)
            : base(header, payload)
        {
            this.Request = bnet.protocol.friends.SubscribeToFriendsRequest.CreateBuilder().MergeFrom(this.Payload.ToArray()).Build();
        }
    }
}
