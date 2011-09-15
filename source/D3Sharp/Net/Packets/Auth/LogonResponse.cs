using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using bnet.protocol;

namespace D3Sharp.Net.Packets.Auth
{
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
