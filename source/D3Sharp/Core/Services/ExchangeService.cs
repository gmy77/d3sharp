using D3Sharp.Net;
using D3Sharp.Net.Packets;

namespace D3Sharp.Core.Services
{
    [Service(serviceID: 0x0a, serverHash: 0xd750148b, clientHash: 0x0)]
    public class ExchangeService : Service
    {
        [ServiceMethod(0x1b)]
        public void GetConfiguration(IClient client, Packet packetIn)
        {
            Logger.Trace("RPC:Exchange:GetConfiguration()");
            var response = bnet.protocol.exchange.GetConfigurationResponse.CreateBuilder().Build();

            var packet = new Packet(
                new Header(0xfe, 0x0, packetIn.Header.RequestID, (uint)response.SerializedSize),
                response.ToByteArray());

            client.Send(packet);
        }
    }
}
