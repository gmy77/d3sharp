using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using D3Sharp.Net;
using D3Sharp.Net.Packets;
using bnet.protocol;

namespace D3Sharp.Core.Services
{
    [Service(serviceID: 0x2, serviceHash: 0xDECFC01)]
    public class Authentication:Service
    {
        [ServiceMethod(0x1)]
        public void Logon(IClient client, Packet packetIn)
        {
            var request = bnet.protocol.authentication.LogonRequest.CreateBuilder().MergeFrom(packetIn.Payload.ToArray()).Build();

            var response = bnet.protocol.authentication.LogonResponse.CreateBuilder()
                .SetAccount(EntityId.CreateBuilder().SetHigh(12345).SetLow(67890))
                .SetGameAccount(EntityId.CreateBuilder().SetHigh(67890).SetLow(12345))
                .Build();

            var packet =
                new Packet(
                    new Header(new byte[] {0xfe, 0x0, (byte) packetIn.Header.RequestID, 0x0, (byte) response.SerializedSize})
                    , response.ToByteArray());

            client.Send(packet, response);
        }       
    }
}
