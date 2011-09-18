using D3Sharp.Net;
using D3Sharp.Net.Packets;
using System.Linq;
using System;
using D3Sharp.Utils.Extensions;

namespace D3Sharp.Core.Services
{
    [Service(serviceID: 0x6, serviceName: "bnet.protocol.friends.FriendsService", clientHash: 0x6F259A13)]
    public class FriendsService : Service
    {
        [ServiceMethod(0x1)]
        public void SubscribeToFriends(IClient client, Packet packetIn)
        {
            Logger.Trace("RPC:Friends:SubscribeToFriends()");
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

        [ServiceMethod(0x2)]
        public void SendInvitationRequest(IClient client, Packet packetIn)
        {
            //TODO: Set these to the corect values.
            const ulong accountHandle = 0x0000000000000000;
            const ulong gameAccountHandle = 0x0000000000000000;

            Logger.Trace("RPC:Friends:SendInvitationRequest() Stub");
            var request = bnet.protocol.invitation.SendInvitationRequest.ParseFrom(packetIn.Payload.ToArray());

            var invitation = bnet.protocol.invitation.Invitation.CreateBuilder()
                .SetCreationTime(DateTime.Now.ToUnixTime())
                .SetExpirationTime(request.ExpirationTime)
                .SetId(0)
                .SetInvitationMessage(request.InvitationMessage)
                .SetInviteeIdentity(bnet.protocol.Identity.CreateBuilder()
                    .SetAccountId(bnet.protocol.EntityId.CreateBuilder().SetHigh(accountHandle).SetLow(0x1).Build())            //TODO: Change SetLow to an actual index in the database.
                    .SetGameAccountId(bnet.protocol.EntityId.CreateBuilder().SetHigh(gameAccountHandle).SetLow(0x1).Build())    //TODO: Change SetLow to an actual index in the database.
                    .Build())
                .SetInviteeName("FriendName")                                                                                   //TODO: Set this to the name retrieved from the database.
                .SetInviterIdentity(bnet.protocol.Identity.CreateBuilder()
                    .SetAccountId(bnet.protocol.EntityId.CreateBuilder().SetHigh(accountHandle).SetLow(0x0).Build())            //TODO: Change SetLow to an actual index in the database.
                    .SetGameAccountId(bnet.protocol.EntityId.CreateBuilder().SetHigh(gameAccountHandle).SetLow(0x0).Build())    //TODO: Change SetLow to an actual index in the database.
                    .Build())
                .SetInviterName("YourName")                                                                                     //TODO: Set this to the name retrieved from the database.
                .Build();

            var response = bnet.protocol.invitation.SendInvitationResponse.CreateBuilder()
                .SetInvitation(invitation)
                .Build();

            var packet = new Packet(
                new Header(0xfe, 0x0, packetIn.Header.RequestID, (uint)response.SerializedSize),
                response.ToByteArray());

            client.Send(packet);
        }
    }
}
