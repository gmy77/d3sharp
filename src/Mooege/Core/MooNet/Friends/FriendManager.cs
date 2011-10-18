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
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using Mooege.Core.MooNet.Accounts;
using Mooege.Core.MooNet.Helpers;
using Mooege.Core.MooNet.Objects;
using Mooege.Net.MooNet;
using Mooege.Core.Common.Storage;
using Wintellect.PowerCollections;

namespace Mooege.Core.MooNet.Friends
{
    public class FriendManager : RPCObject
    {
        private static readonly FriendManager _instance = new FriendManager();
        public static FriendManager Instance { get { return _instance; } }

        public static readonly MultiDictionary<ulong, bnet.protocol.friends.Friend> Friends =
            new MultiDictionary<ulong, bnet.protocol.friends.Friend>(true);

        public static readonly Dictionary<ulong, bnet.protocol.invitation.Invitation> OnGoingInvitations =
            new Dictionary<ulong, bnet.protocol.invitation.Invitation>();

        public static ulong InvitationIdCounter = 1;

        static FriendManager()
        {
            LoadFriendships();
        }

        public static void HandleInvitation(MooNetClient client, bnet.protocol.invitation.Invitation invitation)
        {
            var invitee = Instance.Subscribers.FirstOrDefault(subscriber => subscriber.Account.BnetAccountID.Low == invitation.InviteeIdentity.AccountId.Low);
            if (invitee == null) return; // if we can't find invite just return - though we should actually check for it until expiration time and store this in database.

            OnGoingInvitations.Add(invitation.Id, invitation); // track ongoing invitations so we can tranport it forth and back.

            var notification = bnet.protocol.friends.InvitationAddedNotification.CreateBuilder().SetInvitation(invitation);

            invitee.MakeTargetedRPC(FriendManager.Instance, () =>
                bnet.protocol.friends.FriendsNotify.CreateStub(invitee).NotifyReceivedInvitationAdded(null, notification.Build(), callback => { }));
        }

        public static void HandleAccept(MooNetClient client, bnet.protocol.invitation.GenericRequest request)
        {
            if (!OnGoingInvitations.ContainsKey(request.InvitationId)) return;
            var invitation = OnGoingInvitations[request.InvitationId];

            var inviter = AccountManager.GetAccountByPersistentID(invitation.InviterIdentity.AccountId.Low);
            var invitee = AccountManager.GetAccountByPersistentID(invitation.InviteeIdentity.AccountId.Low);
            var inviteeAsFriend = bnet.protocol.friends.Friend.CreateBuilder().SetId(invitation.InviteeIdentity.AccountId).Build();
            var inviterAsFriend = bnet.protocol.friends.Friend.CreateBuilder().SetId(invitation.InviterIdentity.AccountId).Build();

            var notificationToInviter = bnet.protocol.friends.InvitationRemovedNotification.CreateBuilder()
                .SetInvitation(invitation)
                .SetReason(0) // success?
                .SetAddedFriend(inviteeAsFriend).Build();

            inviter.LoggedInClient.MakeTargetedRPC(FriendManager.Instance, () =>
                bnet.protocol.friends.FriendsNotify.CreateStub(inviter.LoggedInClient).NotifyReceivedInvitationRemoved(null, notificationToInviter,callback => { }));
            
            var notificationToInvitee = bnet.protocol.friends.InvitationRemovedNotification.CreateBuilder()
                .SetInvitation(invitation)
                .SetReason(0) // success?
                .SetAddedFriend(inviterAsFriend).Build();

            invitee.LoggedInClient.MakeTargetedRPC(FriendManager.Instance, () =>
                bnet.protocol.friends.FriendsNotify.CreateStub(invitee.LoggedInClient).NotifyReceivedInvitationRemoved(null, notificationToInvitee,callback => { }));

            Friends.Add(inviter.BnetAccountID.Low, inviteeAsFriend);
            AddFriendshipToDB(inviter,invitee);

            // send friend added notification to inviter
            var friendAddedNotificationToInviter = bnet.protocol.friends.FriendNotification.CreateBuilder().SetTarget(inviteeAsFriend).Build();

            inviter.LoggedInClient.MakeTargetedRPC(FriendManager.Instance, () =>
                bnet.protocol.friends.FriendsNotify.CreateStub(inviter.LoggedInClient).NotifyFriendAdded(null, friendAddedNotificationToInviter, callback => { }));

            // send friend added notification to invitee 
            var friendAddedNotificationToInvitee = bnet.protocol.friends.FriendNotification.CreateBuilder().SetTarget(inviterAsFriend).Build();

            invitee.LoggedInClient.MakeTargetedRPC(FriendManager.Instance, () =>
                bnet.protocol.friends.FriendsNotify.CreateStub(invitee.LoggedInClient).NotifyFriendAdded(null, friendAddedNotificationToInvitee, callback => { }));
        }

        private static void AddFriendshipToDB(Account inviter, Account invitee)
        {
            try
            {
                var query = string.Format("INSERT INTO friends (accountId, friendId) VALUES({0},{1}); INSERT INTO friends (accountId, friendId) VALUES({2},{3});", inviter.BnetAccountID.Low, invitee.BnetAccountID.Low, invitee.BnetAccountID.Low, inviter.BnetAccountID.Low);

                var cmd = new SQLiteCommand(query, DBManager.Connection);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Logger.ErrorException(e, "FriendManager.AddFriendshipToDB()");
            }
        }

        private static void LoadFriendships() // load friends from database.
        {
            const string query = "SELECT * from friends";
            var cmd = new SQLiteCommand(query, DBManager.Connection);
            var reader = cmd.ExecuteReader();

            if (!reader.HasRows) return;

            while (reader.Read())
            {
                var friend =
                    bnet.protocol.friends.Friend.CreateBuilder().SetId(
                        bnet.protocol.EntityId.CreateBuilder().SetHigh((ulong)EntityIdHelper.HighIdType.AccountId).
                            SetLow((ulong)reader.GetInt64(1))).Build();

                Friends.Add((ulong)reader.GetInt64(0), friend);
            }
        }
    }
}
