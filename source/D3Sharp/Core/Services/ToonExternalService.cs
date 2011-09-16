using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using D3Sharp.Net;
using D3Sharp.Net.Packets;

namespace D3Sharp.Core.Services
{
    [Service(serviceID: 0x2, serverHash: 0x4124C31B, clientHash: 0x0)]
    public class ToonExternalService : Service
    {
        [ServiceMethod(0x1)]
        public void ToonListRequest(IClient client, Packet packetIn)
        {
            var response = bnet.protocol.toon.external.ToonListResponse.CreateBuilder().Build();

            var packet = new Packet(
                new Header(0xfe, 0x0, packetIn.Header.RequestID, (uint)response.SerializedSize),
                response.ToByteArray());

            Logger.Debug("RPC:Toon:ToonListRequest()");
            client.Send(packet);
        }

        [ServiceMethod(0x3)]
        public void CreateToonRequest(IClient client, Packet packetIn)
        {
            var request = bnet.protocol.toon.external.CreateToonRequest.CreateBuilder().MergeFrom(packetIn.Payload.ToArray()).Build();
            Console.WriteLine(request);

            var response = bnet.protocol.toon.external.CreateToonResponse.CreateBuilder()
                .SetToon(bnet.protocol.EntityId.CreateBuilder().SetHigh(0x300016200004433).SetLow(1))                
                .Build();

            var packet = new Packet(
                new Header(0xfe, 0x0, packetIn.Header.RequestID, (uint)response.SerializedSize),
                response.ToByteArray());

            Logger.Debug("RPC:Toon:CreateToonRequest()");
            client.Send(packet);
        }
    }
}
