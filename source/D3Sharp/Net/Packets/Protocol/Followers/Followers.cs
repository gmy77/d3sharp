using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace D3Sharp.Net.Packets.Protocol.Followers
{
    [Service(serviceID: -1, serviceHash: 0x905cdf9f, method: 0x1)]
    public class SubscribeToFollowersRequest : PacketIn
    {
        public SubscribeToFollowersRequest(Header header, IEnumerable<byte> payload)
            : base(header, payload)
        {
            this.Request = bnet.protocol.followers.SubscribeToFollowersRequest.CreateBuilder().MergeFrom(this.Payload.ToArray()).Build();
        }
    }
}
