using D3Sharp.Net;
using D3Sharp.Net.Packets;

namespace D3Sharp.Core.Services
{
    [Service(serviceID: 0x6, serverHash: 0xA3DDB1BD, clientHash: 0x6F259A13)]
    public class FriendsService : Service
    {
        [ServiceMethod(0x1)]
        public void SubscribeToFriends(IClient client, Packet packetIn)
        {
            Logger.Trace("RPC:Friends:Subscribe()");
            var response = bnet.protocol.friends.SubscribeToFriendsResponse.CreateBuilder()
                .SetMaxFriends(127)
                .SetMaxReceivedInvitations(127)
                .SetMaxSentInvitations(127)
                .Build();

            var packet = new Packet(
                new Header(0xfe, 0x0, packetIn.Header.RequestID, (uint)response.SerializedSize),
                response.ToByteArray());

            client.Send(packet);
        }
    }
}
