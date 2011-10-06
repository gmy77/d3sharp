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
using Mooege.Core.MooNet.Accounts;
using Mooege.Core.MooNet.Objects;
using Mooege.Net.MooNet;
using Wintellect.PowerCollections;

namespace Mooege.Core.MooNet.Friends
{
    public class FriendManager : RPCObject
    {
        private readonly MultiDictionary<bnet.protocol.EntityId, bnet.protocol.friends.Friend> _friends =
            new MultiDictionary<bnet.protocol.EntityId, bnet.protocol.friends.Friend>(true);

        private readonly Dictionary<ulong, bnet.protocol.invitation.Invitation> _onGoingInvitations =
            new Dictionary<ulong, bnet.protocol.invitation.Invitation>();

        public static ulong InvitationIdCounter = 1;

        private void NotifyFriends(Account account)
        {

        }

        private void SendFriends(Account account)
        {
            if (!_friends.ContainsKey(account.BnetAccountID)) return;
            var friends = _friends[account.BnetAccountID];

            foreach (var friend in friends)
            {

            }
        }

        public void HandleInvitation(MooNetClient client, bnet.protocol.invitation.Invitation invitation)
        {
            var invitee = this.Subscribers.FirstOrDefault(subscriber => subscriber.Account.BnetAccountID.Low == invitation.InviteeIdentity.AccountId.Low);
            if (invitee == null) return; // if we can't find invite just return - though we should actually check for it until expiration time and store this in database.

            this._onGoingInvitations.Add(invitation.Id, invitation); // track ongoing invitations so we can tranport it forth and back.

            var notification = bnet.protocol.friends.InvitationAddedNotification.CreateBuilder().SetInvitation(invitation);
            invitee.CallMethod(bnet.protocol.friends.FriendsNotify.Descriptor.FindMethodByName("NotifyReceivedInvitationAdded"), notification.Build(), this.DynamicId);
        }

        public void HandleAccept(MooNetClient client, bnet.protocol.invitation.GenericRequest request)
        {
            if (!this._onGoingInvitations.ContainsKey(request.InvitationId)) return;
            var invitation = this._onGoingInvitations[request.InvitationId];

            var inviter = AccountManager.GetAccountByPersistantID(invitation.InviterIdentity.AccountId.Low);
            var invitee = AccountManager.GetAccountByPersistantID(invitation.InviteeIdentity.AccountId.Low);
            var inviteeAsFriend = bnet.protocol.friends.Friend.CreateBuilder().SetId(invitation.InviteeIdentity.AccountId).Build();
            var inviterAsFriend = bnet.protocol.friends.Friend.CreateBuilder().SetId(invitation.InviterIdentity.AccountId).Build();

            var notificationToInviter = bnet.protocol.friends.InvitationRemovedNotification.CreateBuilder()
                .SetInvitation(invitation)
                .SetReason(0) // success?
                .SetAddedFriend(inviteeAsFriend).Build();

            inviter.LoggedInClient.CallMethod(bnet.protocol.friends.FriendsNotify.Descriptor.FindMethodByName("NotifyReceivedInvitationRemoved"), notificationToInviter, this.DynamicId);
            
            var notificationToInvitee = bnet.protocol.friends.InvitationRemovedNotification.CreateBuilder()
                .SetInvitation(invitation)
                .SetReason(0) // success?
                .SetAddedFriend(inviterAsFriend).Build();

            invitee.LoggedInClient.CallMethod(bnet.protocol.friends.FriendsNotify.Descriptor.FindMethodByName("NotifyReceivedInvitationRemoved"), notificationToInvitee, this.DynamicId);

            _friends.Add(inviter.BnetAccountID, inviteeAsFriend);

            // send friend added notification to inviter
            var friendAddedNotificationToInviter = bnet.protocol.friends.FriendNotification.CreateBuilder().SetTarget(inviteeAsFriend).Build();
            inviter.LoggedInClient.CallMethod(
                bnet.protocol.friends.FriendsNotify.Descriptor.FindMethodByName("NotifyFriendAdded"), friendAddedNotificationToInviter,
                this.DynamicId);

            // send friend added notification to invitee 
            var friendAddedNotificationToInvitee = bnet.protocol.friends.FriendNotification.CreateBuilder().SetTarget(inviterAsFriend).Build();
            invitee.LoggedInClient.CallMethod(
                bnet.protocol.friends.FriendsNotify.Descriptor.FindMethodByName("NotifyFriendAdded"), friendAddedNotificationToInvitee,
                this.DynamicId);

            //this.SendFriends(account); // sends notification about its friends to account.
            //this.NotifyFriends(account); // send notifications to account's friends too.
        }
    }
}
