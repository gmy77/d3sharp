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
    [Service(serviceID: 0x0, serverHash: 0x0, clientHash: 0x0)]
    public class BaseService : Service
    {
        [ServiceMethod(0x1)]
        public void Connect(IClient client, Packet packetIn)
        {
            var response = bnet.protocol.connection.ConnectResponse.CreateBuilder()
                .SetServerId(ProcessId.CreateBuilder().SetLabel(2).SetEpoch(DateTime.Now.ToUnixTime()))
                .SetClientId(ProcessId.CreateBuilder().SetLabel(1).SetEpoch(DateTime.Now.ToUnixTime()))
                .Build();

            var packet = new Packet(
                    new Header(0xfe, 0x0, packetIn.Header.RequestID, (uint)response.SerializedSize),
                    response.ToByteArray());

            Logger.Debug("RPC:Connect()");
            client.Send(packet);
        }

        [ServiceMethod(0x2)]
        public void Bind(IClient client, Packet packetIn)
        {
            var request = bnet.protocol.connection.BindRequest.CreateBuilder().MergeFrom(packetIn.Payload.ToArray()).Build();

            // supply service id's requested by client using service-hashes.
            var requestedServiceIDs = new List<uint>(); 
            foreach (var serviceHash in request.ImportedServiceHashList)
            {
                var serviceID = ServiceManager.GetServerServiceIDByHash(serviceHash);
                Logger.Trace("RPC:Bind() - Hash: 0x{0} ID: {1} Service: {2} ", serviceHash.ToString("X"), serviceID, ServiceManager.GetServerServiceByID(serviceID) != null ? ServiceManager.GetServerServiceByID(serviceID).GetType().Name : "N/A");
                requestedServiceIDs.Add(serviceID);
            }

            // read services supplied by client..
            foreach (var service in request.ExportedServiceList)
            {
                if (!client.Services.ContainsKey(service.Id))
                    client.Services.Add(service.Id, service.Hash);
            }

            var builder = bnet.protocol.connection.BindResponse.CreateBuilder();
            foreach (var serviceId in requestedServiceIDs) builder.AddImportedServiceId(serviceId);
            var response = builder.Build();

            var packet =
                new Packet(
                    new Header(0xfe, 0x0, packetIn.Header.RequestID, (uint) response.SerializedSize),
                    response.ToByteArray());

            client.Send(packet);
        }
    }
}
