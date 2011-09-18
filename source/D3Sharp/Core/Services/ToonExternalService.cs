using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using D3Sharp.Core;
using D3Sharp.Net;
using D3Sharp.Net.Packets;
using D3Sharp.Utils.Extensions;
using D3Sharp.Core.Storage;
using Google.ProtocolBuffers;

namespace D3Sharp.Core.Services
{
    [Service(serviceID: 0x2, serviceName: "bnet.protocol.toon.external.ToonServiceExternal", clientHash: 0x0)]
    public class ToonExternalService : Service
    {
        [ServiceMethod(0x1)]
        public void ToonList(IClient client, Packet packetIn)
        {            
            Logger.Trace("RPC:ToonExternal:ToonList()");

            var builder = bnet.protocol.toon.external.ToonListResponse.CreateBuilder();

            if(client.Toons.Count>0)
            {
                foreach (var pair in client.Toons)
                {
                    builder.AddToons(pair.Value.BnetEntityID);
                }
            }

            var response = builder.Build();

            var packet = new Packet(
                new Header(0xfe, 0x0, packetIn.Header.RequestID, (uint)response.SerializedSize),
                response.ToByteArray());

            client.Send(packet);
        }

        [ServiceMethod(0x2)]
        public void SelectToon(IClient client, Packet packetIn)
        {
            Logger.Trace("RPC:ToonExternal:SelectToon()");
            var response = bnet.protocol.toon.external.SelectToonResponse.CreateBuilder().Build();

            var packet = new Packet(
                new Header(0xfe, 0x0, packetIn.Header.RequestID, (uint)response.SerializedSize),
                response.ToByteArray());

            client.Send(packet);
        }

        [ServiceMethod(0x3)]
        public void CreateToon(IClient client, Packet packetIn)
        {
            Logger.Trace("RPC:ToonExternal:CreateToon()");
            
            var request = bnet.protocol.toon.external.CreateToonRequest.ParseFrom(packetIn.Payload.ToArray());
            var heroCreateParams = D3.OnlineService.HeroCreateParams.ParseFrom(request.AttributeList[0].Value.MessageValue);
            var builder = bnet.protocol.toon.external.CreateToonResponse.CreateBuilder();

            var toon = new Toons.Toon(request.Name, (uint)heroCreateParams.GbidClass, heroCreateParams.IsFemale ? Toons.ToonGender.Female : Toons.ToonGender.Male, 1, client.Email);
            if (Toons.ToonManager.SaveToon(toon)) builder.SetToon(toon.BnetEntityID);

            var response = builder.Build();

            var packet = new Packet(
                new Header(0xfe, 0x0, packetIn.Header.RequestID, (uint)response.SerializedSize),
                response.ToByteArray());

            client.Send(packet);
        }

        [ServiceMethod(0x4)]
        public void DeleteToon(IClient client, Packet packetIn)
        {
            Logger.Trace("RPC:ToonExternal:DeleteToon()");

            var request = bnet.protocol.toon.external.DeleteToonRequest.ParseFrom(packetIn.Payload.ToArray());
            var id = request.Toon.Low;

            var toon = Toons.ToonManager.GetToon(id);
            Toons.ToonManager.DeleteToon(toon);

            var response = bnet.protocol.toon.external.DeleteToonResponse.CreateBuilder().Build();

            var packet = new Packet(
                new Header(0xfe, 0x0, packetIn.Header.RequestID, (uint)response.SerializedSize),
                response.ToByteArray());

            client.Send(packet);
        }
    }
}
