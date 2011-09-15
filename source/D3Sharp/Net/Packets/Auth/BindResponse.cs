using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace D3Sharp.Net.Packets.Auth
{
    public class BindResponse:PacketOut
    {
        public BindResponse(int requestID, IEnumerable<uint> importedServices):base(requestID)
        {
            var builder = bnet.protocol.connection.BindResponse.CreateBuilder();
            foreach(var serviceId in importedServices) { builder.AddImportedServiceId(serviceId); }
            this.Response = builder.Build();

            this.Header = new Header(new byte[] { 0xfe, 0x0, (byte)requestID, 0x0, (byte)this.Response.SerializedSize });
            this.Payload = this.Response.ToByteArray();
        }
    }
}
