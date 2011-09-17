using D3Sharp.Net;
using D3Sharp.Net.Packets;

namespace D3Sharp.Core.Services
{
    [Service(serviceID: 0x5, serviceName: "bnet.protocol.user_manager.UserManagerService", clientHash: 0xBC872C22)]
    public class UserManagerService : Service
    {
        [ServiceMethod(0x1)]
        public void SubscribeToUserManager(IClient client, Packet packetIn)
        {
            Logger.Trace("RPC:UserManager:Subscribe()");
            var response = bnet.protocol.user_manager.SubscribeToUserManagerResponse.CreateBuilder().Build();

            var packet = new Packet(
                new Header(0xfe, 0x0, packetIn.Header.RequestID, (uint)response.SerializedSize),
                response.ToByteArray());

            //TODO: Sending this packet crashes my client. This may be a local issue as I haven't heard anyone else mention it.     -Ethos
            //client.Send(packet);
        }
    }
}
