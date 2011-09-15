using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using D3Sharp.Utils.Extensions;
using bnet.protocol;

namespace D3Sharp.Net.Packets.Auth
{    
    public class ConnectResponse:PacketOut
    {
        public ConnectResponse(int requestID):base(requestID)
        {            
            this.Response = bnet.protocol.connection.ConnectResponse.CreateBuilder()
                .SetServerId(ProcessId.CreateBuilder().SetLabel(2).SetEpoch(DateTime.Now.ToUnixTime()))
                .SetClientId(ProcessId.CreateBuilder().SetLabel(1).SetEpoch(DateTime.Now.ToUnixTime()))
                .Build();

            this.Header = new Header(new byte[] { 0xfe, 0x0, (byte)requestID, 0x0, (byte)this.Response.SerializedSize });
            this.Payload = this.Response.ToByteArray();
        }
    }
}
