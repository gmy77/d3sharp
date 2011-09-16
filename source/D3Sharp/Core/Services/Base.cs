using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using D3Sharp.Net;
using D3Sharp.Net.Packets;
using D3Sharp.Utils.Extensions;
using bnet.protocol;

namespace D3Sharp.Core.Services
{
    [Service(serviceID: 0x0, serviceHash: 0x0)]
    public class Base : Service
    {
        private static uint _importedServiceCounter = 99;

        [ServiceMethod(0x1)]
        public void Connect(IClient client, Packet packetIn)
        {
            var response = bnet.protocol.connection.ConnectResponse.CreateBuilder()
                .SetServerId(ProcessId.CreateBuilder().SetLabel(2).SetEpoch(DateTime.Now.ToUnixTime()))
                .SetClientId(ProcessId.CreateBuilder().SetLabel(1).SetEpoch(DateTime.Now.ToUnixTime()))
                .Build();

            var packet =
                new Packet(
                    new Header(new byte[] {0xfe, 0x0, (byte) packetIn.Header.RequestID, 0x0, (byte) response.SerializedSize}),
                    response.ToByteArray());

            client.Send(packet, response);
        }

        [ServiceMethod(0x2)]
        public void Bind(IClient client, Packet packetIn)
        {
            var request = bnet.protocol.connection.BindRequest.CreateBuilder().MergeFrom(packetIn.Payload.ToArray()).Build();

            // supply service id's requested by client using service-hashes.
            var requestedServiceIDs = new List<uint>(); 
            foreach (var serviceHash in request.ImportedServiceHashList)
            {
                requestedServiceIDs.Add(ServiceManager.GetServiceIDByHash(serviceHash));
            }

            // read services supplied by client..
            foreach (var service in request.ExportedServiceList)
            {
                if (!client.Services.ContainsKey(service.Id))
                    client.Services.Add(service.Id, service.Hash);
            }

            var builder = bnet.protocol.connection.BindResponse.CreateBuilder();
            foreach (var serviceId in requestedServiceIDs)
                builder.AddImportedServiceId(serviceId);
            var response = builder.Build();

            var packet =
                new Packet(
                    new Header(new byte[] {0xfe, 0x0, (byte) packetIn.Header.RequestID, 0x0, (byte) response.SerializedSize}),
                    response.ToByteArray());

            client.Send(packet, response);
        }
    }
}
