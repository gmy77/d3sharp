using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using D3Sharp.Net;
using D3Sharp.Net.Packets;
using D3Sharp.Utils.Extensions;

namespace D3Sharp.Core.Services
{
    [Service(serviceID: 0x2, serverHash: 0x4124C31B, clientHash: 0x0)]
    public class ToonExternalService : Service
    {
        [ServiceMethod(0x1)]
        public void ToonListRequest(IClient client, Packet packetIn)
        {
            Logger.Trace("RPC:ToonExternal:ToonListRequest()");
            var response = bnet.protocol.toon.external.ToonListResponse.CreateBuilder().Build();

            var packet = new Packet(
                new Header(0xfe, 0x0, packetIn.Header.RequestID, (uint)response.SerializedSize),
                response.ToByteArray());

            client.Send(packet);
        }

        [ServiceMethod(0x2)]
        public void SelectToonRequest(IClient client, Packet packetIn)
        {
            Logger.Trace("RPC:ToonExternal:SelectToonRequest()");
            var request = bnet.protocol.toon.external.SelectToonRequest.ParseFrom(packetIn.Payload.ToArray());
            var response = bnet.protocol.toon.external.SelectToonResponse.CreateBuilder().Build();

            var packet = new Packet(
                new Header(0xfe, 0x0, packetIn.Header.RequestID, (uint)response.SerializedSize),
                response.ToByteArray());

            client.Send(packet);
        }

        [ServiceMethod(0x3)]
        public void CreateToonRequest(IClient client, Packet packetIn)
        {
            Logger.Trace("RPC:ToonExternal:CreateToonRequest()");
            var request = bnet.protocol.toon.external.CreateToonRequest.ParseFrom(packetIn.Payload.ToArray());

            Logger.Debug("request:\n{0}", request.ToString());
            Logger.Debug("request dump:\n{0}", request.ToByteArray().Dump());
            foreach (var attr in request.AttributeList) {
                Logger.Debug("blob: {0} message: {1}", attr.Value.HasBlobValue, attr.Value.HasMessageValue);
                if (attr.Value.HasMessageValue) {
                    var hcp=D3.OnlineService.HeroCreateParams.ParseFrom(attr.Value.MessageValue.ToByteArray());
                    Logger.Debug("HeroCreateParams:\n{0}", hcp.ToString());
                    Logger.Debug("message:\n{0}", attr.Value.MessageValue.ToByteArray().Dump());
                }
            }

            var response = bnet.protocol.toon.external.CreateToonResponse.CreateBuilder()
                .SetToon(bnet.protocol.EntityId.CreateBuilder().SetHigh(0x300016200004433).SetLow(0xFFFFFFFFFFFFFFFF))
                .Build();

            var packet = new Packet(
                new Header(0xfe, 0x0, packetIn.Header.RequestID, (uint)response.SerializedSize),
                response.ToByteArray());

            client.Send(packet);
        }
    }
}
