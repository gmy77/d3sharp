using System.Collections.Generic;
using System.Linq;

namespace D3Sharp.Net.Packets.Auth
{
    [Service(serviceID: -1, serviceHash: 0x71240e35, method: 0x1)]
    public class LogonRequest : PacketIn
    {
        public LogonRequest(Header header, IEnumerable<byte> payload) : base(header, payload)
        {
            this.Request = bnet.protocol.authentication.LogonRequest.CreateBuilder().MergeFrom(this.Payload.ToArray()).Build();
        }
    }
}
