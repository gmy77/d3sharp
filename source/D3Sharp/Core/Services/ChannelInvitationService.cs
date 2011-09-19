using D3Sharp.Net;
using D3Sharp.Net.Packets;
using System.Linq;
using System;
using D3Sharp.Utils;
using D3Sharp.Utils.Extensions;
using Google.ProtocolBuffers;
using bnet.protocol;
using bnet.protocol.channel_invitation;
using bnet.protocol.invitation;

namespace D3Sharp.Core.Services
{
    [Service(serviceID: 0x3, serviceName: "bnet.protocol.channel_invitation.ChannelInvitationService")]
    public class ChannelInvitationService: bnet.protocol.channel_invitation.ChannelInvitationService, IServerService
    {
        protected static readonly Logger Logger = LogManager.CreateLogger();
        public IClient Client { get; set; }

        public override void Subscribe(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.channel_invitation.SubscribeRequest request, System.Action<bnet.protocol.channel_invitation.SubscribeResponse> done)
        {
            Logger.Trace("Subscribe()");

            //TODO: Set these to the corect values.
            const ulong accountHandle = 0x0000000000000000;
            const ulong gameAccountHandle = 0x0000000000000000;

            var invitation = bnet.protocol.invitation.Invitation.CreateBuilder()
                .SetId(0)
                .SetInviterIdentity(bnet.protocol.Identity.CreateBuilder()
                    .SetAccountId(bnet.protocol.EntityId.CreateBuilder().SetHigh(accountHandle).SetLow(0x0).Build()) //TODO: Change SetLow to an actual index in the database.
                    .SetGameAccountId(bnet.protocol.EntityId.CreateBuilder().SetHigh(gameAccountHandle).SetLow(0x0).Build()) //TODO: Change SetLow to an actual index in the database.
                    .Build())
                .SetInviteeIdentity(bnet.protocol.Identity.CreateBuilder()
                    .SetAccountId(bnet.protocol.EntityId.CreateBuilder().SetHigh(accountHandle).SetLow(0x0).Build()) //TODO: Change SetLow to an actual index in the database.
                    .SetGameAccountId(bnet.protocol.EntityId.CreateBuilder().SetHigh(gameAccountHandle).SetLow(0x0).Build()) //TODO: Change SetLow to an actual index in the database.
                    .Build())
                .SetInviterName("YourName")
                .SetInviteeName("FriendName") // lookup this from agentid.toon_id?
                .SetInvitationMessage("Invite Message")
                .SetCreationTime(DateTime.Now.ToUnixTime())
                .SetExpirationTime(DateTime.Now.AddDays(2).ToUnixTime())
                .Build();

            var invite_collection = bnet.protocol.channel_invitation.InvitationCollection.CreateBuilder()
                .SetServiceType(0)
                .SetMaxReceivedInvitations(127)
                .SetObjectId(request.ObjectId)
                .AddReceivedInvitation(invitation)
                .Build();

            var builder = bnet.protocol.channel_invitation.SubscribeResponse.CreateBuilder()
                .AddCollection(invite_collection)
                .AddReceivedInvitation(invitation);

            done(builder.Build());
        }

        public override void AcceptInvitation(Google.ProtocolBuffers.IRpcController controller, AcceptInvitationRequest request, System.Action<AcceptInvitationResponse> done)
        {
            throw new System.NotImplementedException();
        }

        public override void DeclineInvitation(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.invitation.GenericRequest request, System.Action<bnet.protocol.NoData> done)
        {
            throw new System.NotImplementedException();
        }

        public override void RevokeInvitation(Google.ProtocolBuffers.IRpcController controller, RevokeInvitationRequest request, System.Action<bnet.protocol.NoData> done)
        {
            throw new System.NotImplementedException();
        }

        public override void SendInvitation(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.invitation.SendInvitationRequest request, System.Action<bnet.protocol.invitation.SendInvitationResponse> done)
        {
            throw new System.NotImplementedException();
        }

        public override void SuggestInvitation(Google.ProtocolBuffers.IRpcController controller, SuggestInvitationRequest request, System.Action<bnet.protocol.NoData> done)
        {
            throw new System.NotImplementedException();
        }

        public override void Unsubscribe(Google.ProtocolBuffers.IRpcController controller, UnsubscribeRequest request, System.Action<bnet.protocol.NoData> done)
        {
            throw new System.NotImplementedException();
        }
    }
}
