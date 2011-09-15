using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace D3Sharp.Net.Packets.Protocol.Toon
{
    [Service(serviceID: -1, serviceHash: 0x83040608, method: 0x1)]
    public class ToonListRequest : PacketIn
    {
        public ToonListRequest(Header header, IEnumerable<byte> payload)
            : base(header, payload)
        {
            this.Request = bnet.protocol.toon.external.ToonListRequest.CreateBuilder().MergeFrom(this.Payload.ToArray()).Build();
        }
    }
}
