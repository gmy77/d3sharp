using D3Sharp.Net;
using D3Sharp.Net.Packets;

namespace D3Sharp.Core.Services
{
    [Service(serviceID: 0x4, serviceName: "bnet.protocol.followers.FollowersService")]
    public class FollowersService : Service
    {
        [ServiceMethod(0x1)]
        public void SubscribeToFollowers(IClient client, Packet packetIn)
        {
            Logger.Trace("RPC:Followers:SubscribeToFollowers()");
            var response = bnet.protocol.followers.SubscribeToFollowersResponse.CreateBuilder().Build();

            var packet = new Packet(
                new Header(0xfe, 0x0, packetIn.Header.RequestID, (uint)response.SerializedSize),
                response.ToByteArray());

            client.Send(packet);
        }
    }
}
