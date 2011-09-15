using System.Collections.Generic;
using System.Linq;
using bnet.protocol;

namespace D3Sharp.Net.Packets.Protocol.Authentication
{
    [Service(serviceID: -1, serviceHash: 0x71240e35, method: 0x1)]
    public class LogonRequest : PacketIn
    {
        public LogonRequest(Header header, IEnumerable<byte> payload)
            : base(header, payload)
        {
            this.Request = bnet.protocol.authentication.LogonRequest.CreateBuilder().MergeFrom(this.Payload.ToArray()).Build();
        }
    }

    class LogonResponse : PacketOut
    {
        public LogonResponse(int requestID)
            : base(requestID)
        {
            this.Response = bnet.protocol.authentication.LogonResponse.CreateBuilder()
                .SetAccount(EntityId.CreateBuilder().SetHigh(12345).SetLow(67890))
                .SetGameAccount(EntityId.CreateBuilder().SetHigh(67890).SetLow(12345))
                .Build();

            this.Header = new Header(new byte[] { 0xfe, 0x0, (byte)requestID, 0x0, (byte)this.Response.SerializedSize });
            this.Payload = this.Response.ToByteArray();
        }
    }
}
