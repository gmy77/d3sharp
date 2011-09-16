using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using D3Sharp.Net;
using D3Sharp.Net.Packets;
using bnet.protocol;

namespace D3Sharp.Core.Services
{
    [Service(serviceID: 0x1, serverHash: 0xDECFC01, clientHash: 0x71240E35)]
    public class AuthenticationService:Service
    {
        [ServiceMethod(0x1)]
        public void Logon(IClient client, Packet packetIn)
        {
            var response = bnet.protocol.authentication.LogonResponse.CreateBuilder()
                .SetAccount(EntityId.CreateBuilder().SetHigh(0x100000000000000).SetLow(0))
                .SetGameAccount(EntityId.CreateBuilder().SetHigh(0x200006200004433).SetLow(0))
                .Build();

            var packet = new Packet(
                new Header(new byte[] {0xfe, 0x0, (byte) packetIn.Header.RequestID, 0x0, (byte) response.SerializedSize}), 
                response.ToByteArray());

            Logger.Debug("RPC:Logon()");
            client.Send(packet);
        }       
    }
}
