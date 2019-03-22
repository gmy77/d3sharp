/*
 * Copyright (C) 2011 mooege project
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */

using System;
using Mooege.Common.Extensions;
using Mooege.Common.Logging;
using Mooege.Core.MooNet.Channels;
using Mooege.Core.MooNet.Toons;
using Mooege.Net.MooNet;

namespace Mooege.Core.MooNet.Services
{
    [Service(serviceID: 0x3, serviceName: "bnet.protocol.channel_invitation.ChannelInvitationService")]
    public class ChannelInvitationService: bnet.protocol.channel_invitation.ChannelInvitationService, IServerService
    {
        private static readonly Logger Logger = LogManager.CreateLogger();
        public MooNetClient Client { get; set; }
        public bnet.protocol.Header LastCallHeader { get; set; }

        private readonly ChannelInvitationManager _invitationManager = new ChannelInvitationManager();

        public override void Subscribe(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.channel_invitation.SubscribeRequest request, Action<bnet.protocol.channel_invitation.SubscribeResponse> done)
        {
            Logger.Trace("Subscribe() {0}",this.Client);

            this._invitationManager.AddSubscriber(this.Client, request.ObjectId);
            var builder = bnet.protocol.channel_invitation.SubscribeResponse.CreateBuilder();
            done(builder.Build());
        }

        public override void AcceptInvitation(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.channel_invitation.AcceptInvitationRequest request, Action<bnet.protocol.channel_invitation.AcceptInvitationResponse> done)
        {
            Logger.Trace("{0} accepted invitation.", this.Client.CurrentToon);

            var channel = this._invitationManager.HandleAccept(this.Client, request);

            var response = bnet.protocol.channel_invitation.AcceptInvitationResponse.CreateBuilder().SetObjectId(channel.DynamicId).Build();
            done(response);
        }

        public override void DeclineInvitation(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.invitation.GenericRequest request, Action<bnet.protocol.NoData> done)
        {
            Logger.Trace("{0} declined invitation.", this.Client.CurrentToon);

            var respone = bnet.protocol.NoData.CreateBuilder();
            done(respone.Build());

            this._invitationManager.HandleDecline(this.Client, request);
        }

        public override void RevokeInvitation(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.channel_invitation.RevokeInvitationRequest request, Action<bnet.protocol.NoData> done)
        {
            Logger.Trace("{0} revoked invitation.", this.Client.CurrentToon);

            var builder = bnet.protocol.NoData.CreateBuilder();
            done(builder.Build());

            this._invitationManager.Revoke(this.Client, request);
        }

        public override void SendInvitation(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.invitation.SendInvitationRequest request, Action<bnet.protocol.invitation.SendInvitationResponse> done)
        {
            var invitee = ToonManager.GetToonByLowID(request.TargetId.Low);
            if (this.Client.CurrentChannel.HasToon(invitee)) return; // don't allow a second invitation if invitee is already a member of client's current channel.

            Logger.Debug("{0} invited {1} to his channel.", Client.CurrentToon, invitee);

            // somehow protobuf lib doesnt handle this extension, so we're using a workaround to get that channelinfo.
            var extensionBytes = request.UnknownFields.FieldDictionary[105].LengthDelimitedList[0].ToByteArray();
            var channelInvitationInfo = bnet.protocol.channel_invitation.SendInvitationRequest.ParseFrom(extensionBytes);

            var channelInvitation = bnet.protocol.channel_invitation.Invitation.CreateBuilder()
                .SetChannelDescription(bnet.protocol.channel.ChannelDescription.CreateBuilder().SetChannelId(channelInvitationInfo.ChannelId).Build())
                .SetReserved(channelInvitationInfo.Reserved)
                .SetServiceType(channelInvitationInfo.ServiceType)
                .SetRejoin(false).Build();

            var invitation = bnet.protocol.invitation.Invitation.CreateBuilder();
            invitation.SetId(ChannelInvitationManager.InvitationIdCounter++)
                .SetInviterIdentity(bnet.protocol.Identity.CreateBuilder().SetToonId(Client.CurrentToon.BnetEntityID).Build())
                .SetInviterName(Client.CurrentToon.Name)
                .SetInviteeIdentity(bnet.protocol.Identity.CreateBuilder().SetToonId(request.TargetId).Build())
                .SetInviteeName(invitee.Name)
                .SetInvitationMessage(request.InvitationMessage)
                .SetCreationTime(DateTime.Now.ToExtendedEpoch())
                .SetExpirationTime(DateTime.Now.ToUnixTime() + request.ExpirationTime)
                .SetExtension(bnet.protocol.channel_invitation.Invitation.ChannelInvitation, channelInvitation);            

            // oh blizz, cmon. your buggy client even doesn't care this message at all but waits the UpdateChannelStateNotification with embedded invitation proto to show "invitation sent message".
            // ADVICE TO POTENTIAL BLIZZ-WORKER READING THIS;
            // change rpc SendInvitation(.bnet.protocol.invitation.SendInvitationRequest) returns (.bnet.protocol.invitation.SendInvitationResponse); to rpc SendInvitation(.bnet.protocol.invitation.SendInvitationRequest) returns (.bnet.protocol.NoData);

            var builder = bnet.protocol.invitation.SendInvitationResponse.CreateBuilder()
                .SetInvitation(invitation.Clone()); // clone it because we need that invitation as un-builded below.

            done(builder.Build());

            // send bnet.protocol.channel.UpdateChannelStateNotification to inviter - update him on invitation is sent.          

            var notification = bnet.protocol.channel.UpdateChannelStateNotification.CreateBuilder()
                .SetAgentId(Client.CurrentToon.BnetEntityID)
                .SetStateChange(bnet.protocol.channel.ChannelState.CreateBuilder().AddInvitation(invitation.Clone()));

            this.Client.MakeTargetedRPC(this.Client.CurrentChannel, () =>
                bnet.protocol.channel.ChannelSubscriber.CreateStub(Client).NotifyUpdateChannelState(controller,notification.Build(),callback => { }));

            // notify the invitee on invitation.
            this._invitationManager.HandleInvitation(this.Client, invitation.Build());
        }

        public override void SuggestInvitation(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.channel_invitation.SuggestInvitationRequest request, Action<bnet.protocol.NoData> done)
        {
            var suggester = ToonManager.GetToonByLowID(request.TargetId.Low); //wants invite
            var suggestee = ToonManager.GetToonByLowID(request.ApprovalId.Low); //approves invite
            if(suggestee==null) return;

            Logger.Debug("{0} suggested {1} to invite him.", suggester, suggestee);
            var respone = bnet.protocol.NoData.CreateBuilder();
            done(respone.Build());

            // Even though it makes no sense, the suggester is used for all fields in the caps and is what works with the client. /dustinconrad
            var suggestion = bnet.protocol.invitation.Suggestion.CreateBuilder()
                .SetChannelId(request.ChannelId)
                .SetSuggesterId(suggester.BnetEntityID)
                .SetSuggesterName(suggester.Name)
                .SetSuggesteeId(suggester.BnetEntityID)
                .SetSuggesteeName(suggester.Name)
                .Build();

            var notification = bnet.protocol.channel_invitation.SuggestionAddedNotification.CreateBuilder().SetSuggestion(suggestion);

            suggestee.Owner.LoggedInClient.MakeTargetedRPC(this._invitationManager, () => 
                bnet.protocol.channel_invitation.ChannelInvitationNotify.CreateStub(suggestee.Owner.LoggedInClient).NotifyReceivedSuggestionAdded(null, notification.Build(), callback => { }));
        }

        public override void Unsubscribe(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.channel_invitation.UnsubscribeRequest request, Action<bnet.protocol.NoData> done)
        {
            Logger.Trace("Unsubscribe() {0}", this.Client);

            this._invitationManager.RemoveSubscriber(Client);
            var builder = bnet.protocol.NoData.CreateBuilder();
            done(builder.Build());
        }
    }
}
