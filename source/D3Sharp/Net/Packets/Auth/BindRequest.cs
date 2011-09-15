using System.Collections.Generic;
using System.Linq;

namespace D3Sharp.Net.Packets.Auth
{
    [Service(serviceID: 0x0, serviceHash: 0x0, method: 0x2)]
    public class BindRequest : PacketIn
    {
        public BindRequest(Header header, IEnumerable<byte> payload) : base(header, payload)
        {
            this.Request = bnet.protocol.connection.BindRequest.CreateBuilder().MergeFrom(this.Payload.ToArray()).Build();
        }
    }
}
