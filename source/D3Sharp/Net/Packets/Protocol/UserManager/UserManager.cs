using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace D3Sharp.Net.Packets.Protocol.UserManager
{
    [Service(serviceID: -1, serviceHash: 0xbc872c22, method: 0x1)]
    public class SubscribeToUserManagerRequest : PacketIn
    {
        public SubscribeToUserManagerRequest(Header header, IEnumerable<byte> payload)
            : base(header, payload)
        {
            this.Request = bnet.protocol.user_manager.SubscribeToUserManagerRequest.CreateBuilder().MergeFrom(this.Payload.ToArray()).Build();
        }
    }
}
