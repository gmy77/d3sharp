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

using System.Collections.Generic;
using System.Linq;
using Mooege.Core.MooNet.Objects;
using Mooege.Core.MooNet.Toons;
using Mooege.Core.MooNet.Helpers;
using Mooege.Net.MooNet;

namespace Mooege.Core.MooNet.Channels
{
    public class ChannelInvitationManager : RPCObject
    {
        private readonly Dictionary<ulong, bnet.protocol.invitation.Invitation> _onGoingInvitations = new Dictionary<ulong, bnet.protocol.invitation.Invitation>();

        public static ulong InvitationIdCounter = 1;

        public ChannelInvitationManager()
        {
            // TODO: Hardcoded 1 as channel persistent id in this case...
            this.BnetEntityId = bnet.protocol.EntityId.CreateBuilder().SetHigh((ulong)EntityIdHelper.HighIdType.ChannelId).SetLow(1).Build();
        }

        public void HandleInvitation(MooNetClient client, bnet.protocol.invitation.Invitation invitation)
        {
            var invitee = this.Subscribers.FirstOrDefault(subscriber => subscriber.Account.CurrentGameAccount.BnetEntityId.Low == invitation.InviteeIdentity.AccountId.Low);
            if (invitee == null) return; // if we can't find invite just return - though we should actually check for it until expiration time.

            this._onGoingInvitations.Add(invitation.Id, invitation); // track ongoing invitations so we can tranport it forth and back.

            var notification = bnet.protocol.channel_invitation.InvitationAddedNotification.CreateBuilder().SetInvitation(invitation);

            invitee.MakeTargetedRPC(this, () =>
                bnet.protocol.channel_invitation.ChannelInvitationNotify.CreateStub(invitee).NotifyReceivedInvitationAdded(null, notification.Build(), callback => { }));
        }

        public Channel HandleAccept(MooNetClient client, bnet.protocol.channel_invitation.AcceptInvitationRequest request)
        {
            if (!this._onGoingInvitations.ContainsKey(request.InvitationId)) return null;

            var invitation = this._onGoingInvitations[request.InvitationId];
            var channel = ChannelManager.GetChannelByEntityId(invitation.GetExtension(bnet.protocol.channel_invitation.ChannelInvitation.ChannelInvitationProp).ChannelDescription.ChannelId);

            channel.Join(client, request.ObjectId); // add invitee to channel -- so inviter and other members will also be notified too.

            var notification = bnet.protocol.channel_invitation.InvitationRemovedNotification.CreateBuilder().SetInvitation(invitation).SetReason((uint)InvitationRemoveReason.Accepted);
            this._onGoingInvitations.Remove(invitation.Id);

            // notify invitee and let him remove the handled invitation.
            client.MakeTargetedRPC(this, () =>
                bnet.protocol.channel_invitation.ChannelInvitationNotify.CreateStub(client).NotifyReceivedInvitationRemoved(null, notification.Build(), callback => { }));

            return channel;
        }

        public void HandleDecline(MooNetClient client, bnet.protocol.invitation.GenericRequest request)
        {
            if (!this._onGoingInvitations.ContainsKey(request.InvitationId)) return;
            var invitation = this._onGoingInvitations[request.InvitationId];

            var inviter = ToonManager.GetToonByLowID(invitation.InviterIdentity.AccountId.Low);
            if (inviter == null || inviter.GameAccount.LoggedInClient == null) return;

            var notification =
                bnet.protocol.channel.UpdateChannelStateNotification.CreateBuilder()
                .SetAgentId(bnet.protocol.EntityId.CreateBuilder().SetHigh(0).SetLow(0)) // caps have this set to high: 0 low: 0 /raist.
                .SetStateChange(bnet.protocol.channel.ChannelState.CreateBuilder().AddInvitation(invitation)
                .SetReason((uint)InvitationRemoveReason.Declined));

            this._onGoingInvitations.Remove(invitation.Id);

            // notify invoker about the decline.
            inviter.GameAccount.LoggedInClient.MakeTargetedRPC(inviter.GameAccount.LoggedInClient.CurrentChannel, () =>
                bnet.protocol.channel.ChannelSubscriber.CreateStub(inviter.GameAccount.LoggedInClient).NotifyUpdateChannelState(null, notification.Build(), callback => { }));
        }

        public void Revoke(MooNetClient client, bnet.protocol.channel_invitation.RevokeInvitationRequest request)
        {
            if (!this._onGoingInvitations.ContainsKey(request.InvitationId)) return;
            var invitation = this._onGoingInvitations[request.InvitationId];

            //notify inviter about revoke
            var updateChannelNotification =
                bnet.protocol.channel.UpdateChannelStateNotification.CreateBuilder()
                .SetAgentId(bnet.protocol.EntityId.CreateBuilder().SetHigh(0).SetLow(0)) // caps have this set to high: 0 low: 0 /dustin
                .SetStateChange(bnet.protocol.channel.ChannelState.CreateBuilder()
                    .AddInvitation(invitation)
                    .SetReason((uint)InvitationRemoveReason.Revoked));

            this._onGoingInvitations.Remove(request.InvitationId);

            client.MakeTargetedRPC(client.CurrentChannel, () =>
                bnet.protocol.channel.ChannelSubscriber.CreateStub(client).NotifyUpdateChannelState(null, updateChannelNotification.Build(), callback => { }));

            //notify invitee about revoke
            var invitationRemoved =
                bnet.protocol.channel_invitation.InvitationRemovedNotification.CreateBuilder()
                .SetInvitation(invitation)
                .SetReason((uint)InvitationRemoveReason.Revoked);

            var invitee = ToonManager.GetToonByLowID(invitation.InviteeIdentity.AccountId.Low);
            invitee.GameAccount.LoggedInClient.MakeTargetedRPC(this, () =>
                bnet.protocol.channel_invitation.ChannelInvitationNotify.CreateStub(invitee.GameAccount.LoggedInClient).NotifyReceivedInvitationRemoved(null, invitationRemoved.Build(), callback => { }));
        }

        public enum InvitationRemoveReason : uint // not sure -- and don't have all the values yet /raist.
        {
            Accepted = 0x0,
            Declined = 0x1,
            Revoked = 0x2
        }
    }
}
