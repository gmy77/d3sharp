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
                new Header(0xfe, 0x0, packetIn.Header.RequestID, (uint)response.SerializedSize),
                response.ToByteArray());

            Logger.Trace("RPC:Followers:Subscribe()");
            client.Send(packet);
        }
    }
}
