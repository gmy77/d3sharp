using System.Collections.Generic;

namespace D3Sharp.Net.Packets.Auth
{
    [Service(serviceID: 0x0, serviceHash: 0x0, method: 0x1)]
    public class ConnectRequest : PacketIn
    {
        public ConnectRequest(Header header, IEnumerable<byte> payload) : base(header, payload) { }
    }
}
