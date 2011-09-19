using System.Linq;
using D3Sharp.Core.Channels;
using D3Sharp.Net;
using D3Sharp.Net.Packets;

namespace D3Sharp.Core.Services
{
    // bnet.protocol.party.PartyService
    [Service(serviceID: 0x0D, serviceName: "bnet.protocol.party.PartyService")]
    public class PartyService : Service
    {
        [ServiceMethod(0x01)]
        public void CreateChannel(IClient client, Packet packetIn)
        {
            Logger.Trace("RPC:Party:CreateChannel()");
            var request = bnet.protocol.channel.CreateChannelRequest.ParseFrom(packetIn.Payload.ToArray());
            //Logger.Debug("request:\n{0}", request.ToString());

            var newChannel = ChannelsManager.CreateNewChannel(client);

            var response = bnet.protocol.channel.CreateChannelResponse.CreateBuilder()
                .SetObjectId(request.ObjectId)
                .SetChannelId(newChannel.BnetEntityID)
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
