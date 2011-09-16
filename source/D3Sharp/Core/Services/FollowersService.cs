using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using D3Sharp.Net;
using D3Sharp.Net.Packets;

namespace D3Sharp.Core.Services
{
    [Service(serviceID: 0x4, serverHash: 0xE5A11099, clientHash: 0x905CDF9F)]
    public class FollowersService : Service
    {
        [ServiceMethod(0x1)]
        public void SubscribeToFollowers(IClient client, Packet packetIn)
        {
            var response = bnet.protocol.followers.SubscribeToFollowersResponse.CreateBuilder().Build();

            var packet = new Packet(
                new Header(new byte[] { 0xfe, 0x0, (byte)packetIn.Header.RequestID, 0x0, (byte)response.SerializedSize }),
                response.ToByteArray());

            Logger.Debug("RPC:Followers:Subscribe()");
            client.Send(packet);
        }
    }
}
