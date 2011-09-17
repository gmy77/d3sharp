using D3Sharp.Net;
using D3Sharp.Net.Packets;

namespace D3Sharp.Core.Services
{
    [Service(serviceID: 0x3, serverHash: 0x83040608, clientHash: 0xF084FC20)]
    public class ChannelInvitationService:Service
    {
        [ServiceMethod(0x1)]
        public void Subscribe(IClient client, Packet packetIn)
        {
            Logger.Trace("RPC:ChannelInvitation:Subscribe()");
            var response = bnet.protocol.channel_invitation.SubscribeResponse.CreateBuilder().Build();

            var packet = new Packet(
                new Header(0xfe, 0x0, packetIn.Header.RequestID, (uint)response.SerializedSize),
                response.ToByteArray());

            client.Send(packet);
        }  
    }
}
