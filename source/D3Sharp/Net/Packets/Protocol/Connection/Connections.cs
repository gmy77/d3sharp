using System;
using System.Collections.Generic;
using System.Linq;
using D3Sharp.Utils.Extensions;
using bnet.protocol;

namespace D3Sharp.Net.Packets.Protocol.Connection
{
    [Service(serviceID: 0x0, serviceHash: 0x0, method: 0x1)]
    public class ConnectRequest : PacketIn
    {
        public ConnectRequest(Header header, IEnumerable<byte> payload) : base(header, payload) { }
    }

    public class ConnectResponse : PacketOut
    {
        public ConnectResponse(int requestID)
            : base(requestID)
        {
            this.Response = bnet.protocol.connection.ConnectResponse.CreateBuilder()
                .SetServerId(ProcessId.CreateBuilder().SetLabel(2).SetEpoch(DateTime.Now.ToUnixTime()))
                .SetClientId(ProcessId.CreateBuilder().SetLabel(1).SetEpoch(DateTime.Now.ToUnixTime()))
                .Build();

            this.Header = new Header(new byte[] { 0xfe, 0x0, (byte)requestID, 0x0, (byte)this.Response.SerializedSize });
            this.Payload = this.Response.ToByteArray();
        }
    }

    [Service(serviceID: 0x0, serviceHash: 0x0, method: 0x2)]
    public class BindRequest : PacketIn
    {
        public BindRequest(Header header, IEnumerable<byte> payload)
            : base(header, payload)
        {
            this.Request = bnet.protocol.connection.BindRequest.CreateBuilder().MergeFrom(this.Payload.ToArray()).Build();
        }
    }

    public class BindResponse : PacketOut
    {
        public BindResponse(int requestID, IEnumerable<uint> requestedServiceIDs)
            : base(requestID)
        {
            var builder = bnet.protocol.connection.BindResponse.CreateBuilder();           
            foreach (var serviceId in requestedServiceIDs)
                builder.AddImportedServiceId(serviceId);
            this.Response = builder.Build();

            this.Header = new Header(new byte[] { 0xfe, 0x0, (byte)requestID, 0x0, (byte)this.Response.SerializedSize });
            this.Payload = this.Response.ToByteArray();
        }
    }
}
