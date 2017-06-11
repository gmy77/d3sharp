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

using System.Collections.Generic;
using System.Linq;
using Mooege.Core.MooNet.Objects;
using Mooege.Core.MooNet.Accounts;
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

        public bnet.protocol.invitation.Invitation GetInvitationById(ulong Id)
        {
            if (!this._onGoingInvitations.ContainsKey(Id))
                return null;
            else
                return this._onGoingInvitations[Id];
        }

        public void HandleInvitation(MooNetClient client, bnet.protocol.invitation.Invitation invitation)
        {
            var invitee = this.Subscribers.FirstOrDefault(subscriber => subscriber.Account.CurrentGameAccount.BnetEntityId.Low == invitation.InviteeIdentity.GameAccountId.Low);
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

            var notification = bnet.protocol.channel_invitation.InvitationRemovedNotification.CreateBuilder().SetInvitation(invitation.ToBuilder()).SetReason((uint)InvitationRemoveReason.Accepted);
            this._onGoingInvitations.Remove(invitation.Id);

            // notify invitee and let him remove the handled invitation.
            client.MakeTargetedRPC(this, () =>
                bnet.protocol.channel_invitation.ChannelInvitationNotify.CreateStub(client).NotifyReceivedInvitationRemoved(null, notification.Build(), callback => { }));

            channel.Join(client, request.ObjectId); // add invitee to channel -- so inviter and other members will also be notified too.

            var inviter = GameAccountManager.GetAccountByPersistentID(invitation.InviterIdentity.AccountId.Low);

            var stateNotification = bnet.protocol.channel.UpdateChannelStateNotification.CreateBuilder()
                .SetAgentId(bnet.protocol.EntityId.CreateBuilder().SetHigh(0).SetLow(0).Build())
                .SetStateChange(bnet.protocol.channel.ChannelState.CreateBuilder().AddRangeInvitation(channel.Invitations.Values).SetReason(0).Build())
                .Build();

            foreach (var member in channel.Members.Keys)
            {
                member.MakeTargetedRPC(channel, () =>
                    bnet.protocol.channel.ChannelSubscriber.CreateStub(member).NotifyUpdateChannelState(null, stateNotification, callback => { }));
            }

            return channel;
        }

        public void HandleDecline(MooNetClient client, bnet.protocol.invitation.GenericRequest request)
        {
            if (!this._onGoingInvitations.ContainsKey(request.InvitationId)) return;
            var invitation = this._onGoingInvitations[request.InvitationId];

            var inviter = GameAccountManager.GetAccountByPersistentID(invitation.InviterIdentity.AccountId.Low);
            if (inviter == null || inviter.LoggedInClient == null) return;

            var notification =
                bnet.protocol.channel.UpdateChannelStateNotification.CreateBuilder()
                .SetAgentId(bnet.protocol.EntityId.CreateBuilder().SetHigh(0).SetLow(0)) // caps have this set to high: 0 low: 0 /raist.
                .SetStateChange(bnet.protocol.channel.ChannelState.CreateBuilder().AddInvitation(invitation)
                .SetReason((uint)InvitationRemoveReason.Declined));

            this._onGoingInvitations.Remove(invitation.Id);

            // notify invoker about the decline.
            inviter.LoggedInClient.MakeTargetedRPC(inviter.LoggedInClient.PartyChannel, () =>
                bnet.protocol.channel.ChannelSubscriber.CreateStub(inviter.LoggedInClient).NotifyUpdateChannelState(null, notification.Build(), callback => { }));

            //inviter.LoggedInClient.MakeTargetedRPC(inviter.LoggedInClient.CurrentChannel, () =>
            //    bnet.protocol.channel.ChannelSubscriber.CreateStub(inviter.LoggedInClient).NotifyUpdateChannelState(null, notification.Build(), callback => { }));
        }

        public void Revoke(MooNetClient client, bnet.protocol.channel_invitation.RevokeInvitationRequest request)
        {
            if (!this._onGoingInvitations.ContainsKey(request.InvitationId)) return;
            var invitation = this._onGoingInvitations[request.InvitationId];

            var channel = ChannelManager.GetChannelByEntityId(request.ChannelId);

            //notify inviter about revoke
            var updateChannelNotification =
                bnet.protocol.channel.UpdateChannelStateNotification.CreateBuilder()
                .SetAgentId(bnet.protocol.EntityId.CreateBuilder().SetHigh(0).SetLow(0)) // caps have this set to high: 0 low: 0 /dustin
                .SetStateChange(bnet.protocol.channel.ChannelState.CreateBuilder()
                    .AddInvitation(invitation)
                    .SetReason((uint)InvitationRemoveReason.Revoked));

            this._onGoingInvitations.Remove(request.InvitationId);

            client.MakeTargetedRPC(channel, () =>
                bnet.protocol.channel.ChannelSubscriber.CreateStub(client).NotifyUpdateChannelState(null, updateChannelNotification.Build(), callback => { }));

            //notify invitee about revoke
            var invitationRemoved =
                bnet.protocol.channel_invitation.InvitationRemovedNotification.CreateBuilder()
                .SetInvitation(invitation)
                .SetReason((uint)InvitationRemoveReason.Revoked);

            var invitee = GameAccountManager.GetAccountByPersistentID(invitation.InviteeIdentity.AccountId.Low);
            invitee.LoggedInClient.MakeTargetedRPC(this, () =>
                bnet.protocol.channel_invitation.ChannelInvitationNotify.CreateStub(invitee.LoggedInClient).NotifyReceivedInvitationRemoved(null, invitationRemoved.Build(), callback => { }));
        }

        public enum InvitationRemoveReason : uint // not sure -- and don't have all the values yet /raist.
        {
            Accepted = 0x0,
            Declined = 0x1,
            Revoked = 0x2
        }
    }
}
