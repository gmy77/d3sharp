using System.Linq;
using D3Sharp.Net;
using D3Sharp.Net.Packets;
using D3Sharp.Utils.Extensions;
using Google.ProtocolBuffers;

﻿namespace D3Sharp.Core.Services
{
    // bnet.protocol.party.PartyService
    [Service(serviceID: 0x0D, serverHash: 0xF4E7FA35, clientHash: 0x0)]
    public class PartyService : Service
    {
        [ServiceMethod(0x01)]
        public void CreateChannel(IClient client, Packet packetIn)
        {
            Logger.Trace("RPC:Party:CreateChannel()");
            var request = bnet.protocol.channel.CreateChannelRequest.ParseFrom(packetIn.Payload.ToArray());
            //Logger.Debug("request:\n{0}", request.ToString());

            var response = bnet.protocol.channel.CreateChannelResponse.CreateBuilder()
                .SetObjectId(request.ObjectId)
                .SetChannelId(bnet.protocol.EntityId.CreateBuilder().SetHigh(0xCCDD).SetLow(0xAABB))
                .Build();

            var packet = new Packet(
                new Header(0xfe, 0x0, packetIn.Header.RequestID, (uint)response.SerializedSize),
                response.ToByteArray());

            client.Send(packet);
        }
        
        [ServiceMethod(0x02)]
        public void JoinChannel(IClient client, Packet packetIn)
        {
            Logger.Trace("RPC:Party:JoinChannel() Stub");
        }

        [ServiceMethod(0x03)]
        public void GetChannelInfo(IClient client, Packet packetIn)
        {
            Logger.Trace("RPC:Party:GetChannelInfo() Stub");
        }
    }
}
