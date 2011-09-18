using System.Linq;
using D3Sharp.Core.Accounts;
using D3Sharp.Net;
using D3Sharp.Net.Packets;

namespace D3Sharp.Core.Services
{
    [Service(serviceID: 0x1, serviceName: "bnet.protocol.authentication.AuthenticationServer")]
    public class AuthenticationService:Service
    {
        [ServiceMethod(0x1)]
        public void Logon(IClient client, Packet packetIn)
        {
            var request = bnet.protocol.authentication.LogonRequest.ParseFrom(packetIn.Payload.ToArray());

            Logger.Trace("RPC:Authentication:Logon(): " + request.Email);
            client.Account = AccountsManager.GetAccount(request.Email);

            var response = bnet.protocol.authentication.LogonResponse.CreateBuilder()
                .SetAccount(client.Account.BnetAccountID)
                .SetGameAccount(client.Account.BnetGameAccountID)
                .Build();

            var packet = new Packet(
                new Header(0xfe, 0x0, packetIn.Header.RequestID, (uint)response.SerializedSize),
                response.ToByteArray());

            client.Send(packet);
        }       
    }
}
