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
using Mooege.Core.MooNet.Channels;
using Mooege.Core.MooNet.Objects;
using Mooege.Net.MooNet;

namespace Mooege.Core.MooNet.Invitation
{
    public class ChannelInvitationManager : RPCObject
    {
        private readonly Dictionary<ulong, bnet.protocol.invitation.Invitation> _onGoingInvitations = new Dictionary<ulong, bnet.protocol.invitation.Invitation>();

        public static ulong InvitationIdCounter = 1;

        public void HandleInvitation(MooNetClient client, bnet.protocol.invitation.Invitation invitation)
        {
            var invitee = this.Subscribers.FirstOrDefault(subscriber => subscriber.CurrentToon.BnetEntityID.Low == invitation.InviteeIdentity.ToonId.Low);
            if (invitee == null) return; // if we can't find invite just return - though we should actually check for it until expiration time.

            this._onGoingInvitations.Add(invitation.Id, invitation); // track ongoing invitations so we can tranport it forth and back.

            var notification = bnet.protocol.channel_invitation.InvitationAddedNotification.CreateBuilder().SetInvitation(invitation);
            invitee.CallMethod(bnet.protocol.channel_invitation.ChannelInvitationNotify.Descriptor.FindMethodByName("NotifyReceivedInvitationAdded"), notification.Build(), this.DynamicId);
        }

        public void HandleAccept(MooNetClient client, bnet.protocol.channel_invitation.AcceptInvitationRequest request)
        {
            var invitation = this._onGoingInvitations[request.InvitationId];

            var channel =
                ChannelManager.GetChannelByEntityId(
                    invitation.GetExtension(bnet.protocol.channel_invitation.Invitation.ChannelInvitation).
                        ChannelDescription.ChannelId);

            channel.Join(client, request.ObjectId);
            channel.AddMember(client);

            var notification = bnet.protocol.channel_invitation.InvitationRemovedNotification.CreateBuilder().SetInvitation(invitation).SetReason(0); // 0 - means success?
            client.CallMethod(bnet.protocol.channel_invitation.ChannelInvitationNotify.Descriptor.FindMethodByName("NotifyReceivedInvitationRemoved"), notification.Build(), this.DynamicId);
        }

        public void HandleDecline(MooNetClient client, bnet.protocol.invitation.GenericRequest request)
        {
            
        }
    }
}
