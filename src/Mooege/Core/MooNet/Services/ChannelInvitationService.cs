/*
 * Copyright (C) 2011 - 2012 mooege project - http://www.mooege.org
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
using Mooege.Core.MooNet.Accounts;
using Mooege.Net.MooNet;

namespace Mooege.Core.MooNet.Services
{
    [Service(serviceID: 0x3, serviceName: "bnet.protocol.channel_invitation.ChannelInvitationService")]
    public class ChannelInvitationService : bnet.protocol.channel_invitation.ChannelInvitationService, IServerService
    {
        private static readonly Logger Logger = LogManager.CreateLogger();
        public MooNetClient Client { get; set; }
        public bnet.protocol.Header LastCallHeader { get; set; }
        public uint Status { get; set; }

        private readonly ChannelInvitationManager _invitationManager = new ChannelInvitationManager();

        public override void Subscribe(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.channel_invitation.SubscribeRequest request, Action<bnet.protocol.channel_invitation.SubscribeResponse> done)
        {
            Logger.Trace("Subscribe() {0}", this.Client);

            this._invitationManager.AddSubscriber(this.Client, request.ObjectId);
            var builder = bnet.protocol.channel_invitation.SubscribeResponse.CreateBuilder();
            done(builder.Build());
        }

        public override void AcceptInvitation(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.channel_invitation.AcceptInvitationRequest request, Action<bnet.protocol.channel_invitation.AcceptInvitationResponse> done)
        {
            var channel = ChannelManager.GetChannelByEntityId(this._invitationManager.GetInvitationById(request.InvitationId).GetExtension(bnet.protocol.channel_invitation.ChannelInvitation.ChannelInvitationProp).ChannelDescription.ChannelId);

            Logger.Trace("{0} accepted invitation id {1} to channel {2}.", this.Client.Account.CurrentGameAccount.CurrentToon, request.InvitationId, channel);

            var response = bnet.protocol.channel_invitation.AcceptInvitationResponse.CreateBuilder().SetObjectId(channel.DynamicId).Build();
            done(response);

            this._invitationManager.HandleAccept(this.Client, request);
        }

        public override void DeclineInvitation(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.invitation.GenericRequest request, Action<bnet.protocol.NoData> done)
        {
            Logger.Trace("{0} declined invitation.", this.Client.Account.CurrentGameAccount.CurrentToon);

            var respone = bnet.protocol.NoData.CreateBuilder();
            done(respone.Build());

            this._invitationManager.HandleDecline(this.Client, request);
        }

        public override void RevokeInvitation(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.channel_invitation.RevokeInvitationRequest request, Action<bnet.protocol.NoData> done)
        {
            Logger.Trace("{0} revoked invitation.", this.Client.Account.CurrentGameAccount.CurrentToon);

            var builder = bnet.protocol.NoData.CreateBuilder();
            done(builder.Build());

            this._invitationManager.Revoke(this.Client, request);
        }

        public override void SendInvitation(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.invitation.SendInvitationRequest request, Action<bnet.protocol.invitation.SendInvitationResponse> done)
        {
            var invitee = GameAccountManager.GetAccountByPersistentID(request.TargetId.Low);
            //if (this.Client.CurrentChannel.HasMember(invitee)) return; // don't allow a second invitation if invitee is already a member of client's current channel.

            Logger.Debug("{0} invited {1} to his channel.", Client.Account.CurrentGameAccount.CurrentToon, invitee);

            // somehow protobuf lib doesnt handle this extension, so we're using a workaround to get that channelinfo.
            var extensionBytes = request.Params.UnknownFields.FieldDictionary[105].LengthDelimitedList[0].ToByteArray();
            var channelInvitationInfo = bnet.protocol.channel_invitation.ChannelInvitationParams.ParseFrom(extensionBytes);

            var channel = ChannelManager.GetChannelByEntityId(channelInvitationInfo.ChannelId);
            var channelDescription = bnet.protocol.channel.ChannelDescription.CreateBuilder()
                .SetChannelId(channelInvitationInfo.ChannelId)
                .SetCurrentMembers((uint)channel.Members.Count)
                .SetState(channel.State);

            var channelInvitation = bnet.protocol.channel_invitation.ChannelInvitation.CreateBuilder()
                .SetChannelDescription(channelDescription)
                .SetReserved(channelInvitationInfo.Reserved)
                .SetServiceType(channelInvitationInfo.ServiceType)
                .SetRejoin(false).Build();

            var invitation = bnet.protocol.invitation.Invitation.CreateBuilder();
            invitation.SetId(ChannelInvitationManager.InvitationIdCounter++)
                .SetInviterIdentity(bnet.protocol.Identity.CreateBuilder().SetGameAccountId(Client.Account.CurrentGameAccount.BnetEntityId).Build())
                .SetInviterName(Client.Account.CurrentGameAccount.Owner.BattleTag)
                .SetInviteeIdentity(bnet.protocol.Identity.CreateBuilder().SetGameAccountId(request.TargetId).Build())
                .SetInviteeName(invitee.Owner.BattleTag)
                .SetInvitationMessage(request.Params.InvitationMessage)
                .SetCreationTime(DateTime.Now.ToExtendedEpoch())
                .SetExpirationTime(DateTime.Now.ToUnixTime() + request.Params.ExpirationTime)
                .SetExtension(bnet.protocol.channel_invitation.ChannelInvitation.ChannelInvitationProp, channelInvitation);

            // oh blizz, cmon. your buggy client even doesn't care this message at all but waits the UpdateChannelStateNotification with embedded invitation proto to show "invitation sent message".
            // ADVICE TO POTENTIAL BLIZZ-WORKER READING THIS;
            // change rpc SendInvitation(.bnet.protocol.invitation.SendInvitationRequest) returns (.bnet.protocol.invitation.SendInvitationResponse); to rpc SendInvitation(.bnet.protocol.invitation.SendInvitationRequest) returns (.bnet.protocol.NoData);

            var builder = bnet.protocol.invitation.SendInvitationResponse.CreateBuilder();
            channel.AddInvitation(invitation.Build());

            if (!channel.HasMember(invitee))
                builder.SetInvitation(invitation.Clone()); // clone it because we need that invitation as un-builded below.

            done(builder.Build());

            // send bnet.protocol.channel.UpdateChannelStateNotification to inviter - update him on invitation is sent.          

            var notification = bnet.protocol.channel.UpdateChannelStateNotification.CreateBuilder()
                .SetAgentId(Client.Account.CurrentGameAccount.BnetEntityId)
                .SetStateChange(bnet.protocol.channel.ChannelState.CreateBuilder().AddInvitation(invitation.Clone()));

            this.Client.MakeTargetedRPC(channel, () =>
                bnet.protocol.channel.ChannelSubscriber.CreateStub(Client).NotifyUpdateChannelState(controller, notification.Build(), callback => { }));

            // notify the invitee on invitation.
            this._invitationManager.HandleInvitation(this.Client, invitation.Build());
        }

        public override void SuggestInvitation(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.channel_invitation.SuggestInvitationRequest request, Action<bnet.protocol.NoData> done)
        {
            var suggester = GameAccountManager.GetAccountByPersistentID(request.TargetId.Low); //wants invite
            var suggestee = GameAccountManager.GetAccountByPersistentID(request.ApprovalId.Low); //approves invite
            if (suggestee == null) return;

            Logger.Debug("{0} suggested {1} to invite him.", suggester, suggestee);
            var respone = bnet.protocol.NoData.CreateBuilder();
            done(respone.Build());

            // Even though it makes no sense, the suggester is used for all fields in the caps and is what works with the client. /dustinconrad
            var suggestion = bnet.protocol.invitation.Suggestion.CreateBuilder()
                .SetChannelId(request.ChannelId)
                .SetSuggesterId(suggester.BnetEntityId)
                .SetSuggesterName(suggester.Owner.BattleTag)
                .SetSuggesteeId(suggester.BnetEntityId)
                .SetSuggesteeName(suggester.Owner.BattleTag)
                .Build();

            var notification = bnet.protocol.channel_invitation.SuggestionAddedNotification.CreateBuilder().SetSuggestion(suggestion);

            suggestee.LoggedInClient.MakeTargetedRPC(this._invitationManager, () =>
                bnet.protocol.channel_invitation.ChannelInvitationNotify.CreateStub(suggestee.LoggedInClient).NotifyReceivedSuggestionAdded(null, notification.Build(), callback => { }));
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
