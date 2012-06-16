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
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using Mooege.Common.Storage;
using Mooege.Core.MooNet.Accounts;
using Mooege.Core.MooNet.Helpers;
using Mooege.Core.MooNet.Objects;
using Mooege.Net.MooNet;
using Wintellect.PowerCollections;

namespace Mooege.Core.MooNet.Friends
{
    public class FriendManager : RPCObject
    {
        private static readonly FriendManager _instance = new FriendManager();
        public static FriendManager Instance { get { return _instance; } }

        /*public static readonly MultiDictionary<ulong, bnet.protocol.friends.Friend> Friends =
            new MultiDictionary<ulong, bnet.protocol.friends.Friend>(true);*/

        public static readonly Dictionary<ulong, bnet.protocol.invitation.Invitation> OnGoingInvitations =
            new Dictionary<ulong, bnet.protocol.invitation.Invitation>();

        public static ulong InvitationIdCounter = 1;

        /*static FriendManager()
        {
            // TODO: Find correct bnet EntityId
            _instance.BnetEntityId = bnet.protocol.EntityId.CreateBuilder().SetHigh((ulong)EntityIdHelper.HighIdType.Unknown).SetLow(1).Build();
            LoadFriendships();
        }*/

        public static bool AreFriends(Account account1, Account account2)
        {
            return account1.DBAccount.Friends.Contains(account2.DBAccount);
        }

        public static bool InvitationExists(Account inviter, Account invitee)
        {
            foreach (var invitation in OnGoingInvitations.Values)
            {
                if ((invitation.InviterIdentity.AccountId == inviter.BnetEntityId) && (invitation.InviteeIdentity.AccountId == invitee.BnetEntityId))
                    return true;
            }
            return false;
        }
        public static void HandleInvitation(MooNetClient client, bnet.protocol.invitation.Invitation invitation)
        {
            var invitee = AccountManager.GetAccountByPersistentID(invitation.InviteeIdentity.AccountId.Low);
            //var invitee = Instance.Subscribers.FirstOrDefault(subscriber => subscriber.Account.BnetEntityId.Low == invitation.InviteeIdentity.AccountId.Low);
            if (invitee == null) return; // if we can't find invite just return - though we should actually check for it until expiration time and store this in database.

            //Check for duplicate invites
            if (OnGoingInvitations.Values.Any(oldInvite => (oldInvite.InviteeIdentity.AccountId == invitation.InviteeIdentity.AccountId) && (oldInvite.InviterIdentity.AccountId == invitation.InviterIdentity.AccountId)))
                return;

            OnGoingInvitations.Add(invitation.Id, invitation); // track ongoing invitations so we can tranport it forth and back.

            if (invitee.IsOnline)
            {
                var inviter = AccountManager.GetAccountByPersistentID(invitation.InviterIdentity.AccountId.Low);

                var notification = bnet.protocol.friends.InvitationNotification.CreateBuilder().SetInvitation(invitation).SetGameAccountId(inviter.CurrentGameAccount.BnetEntityId);

                invitee.CurrentGameAccount.LoggedInClient.MakeTargetedRPC(FriendManager.Instance, () =>
                    bnet.protocol.friends.FriendsNotify.CreateStub(invitee.CurrentGameAccount.LoggedInClient).NotifyReceivedInvitationAdded(null, notification.Build(), callback => { }));
            }
        }

        public static void HandleAccept(MooNetClient client, bnet.protocol.invitation.GenericRequest request)
        {
            if (!OnGoingInvitations.ContainsKey(request.InvitationId)) return;
            var invitation = OnGoingInvitations[request.InvitationId];

            var inviter = AccountManager.GetAccountByPersistentID(invitation.InviterIdentity.AccountId.Low);
            var invitee = AccountManager.GetAccountByPersistentID(invitation.InviteeIdentity.AccountId.Low);
            var inviteeAsFriend = bnet.protocol.friends.Friend.CreateBuilder().SetId(invitation.InviteeIdentity.AccountId).Build();
            var inviterAsFriend = bnet.protocol.friends.Friend.CreateBuilder().SetId(invitation.InviterIdentity.AccountId).Build();

            var notificationToInviter = bnet.protocol.friends.InvitationNotification.CreateBuilder()
                .SetGameAccountId(invitee.BnetEntityId)
                .SetInvitation(invitation)
                .SetReason((uint)InvitationRemoveReason.Accepted) // success?
                .Build();

            var notificationToInvitee = bnet.protocol.friends.InvitationNotification.CreateBuilder()
                .SetGameAccountId(inviter.BnetEntityId)
                .SetInvitation(invitation)
                .SetReason((uint)InvitationRemoveReason.Accepted) // success?
                .Build();

            AddFriendshipToDB(inviter, invitee);

            // send friend added notifications
            var friendAddedNotificationToInviter = bnet.protocol.friends.FriendNotification.CreateBuilder().SetTarget(inviteeAsFriend).SetGameAccountId(invitee.BnetEntityId).Build();
            var friendAddedNotificationToInvitee = bnet.protocol.friends.FriendNotification.CreateBuilder().SetTarget(inviterAsFriend).SetGameAccountId(inviter.BnetEntityId).Build();

            var inviterGameAccounts = GameAccountManager.GetGameAccountsForAccount(inviter);
            var inviteeGameAccounts = GameAccountManager.GetGameAccountsForAccount(invitee);

            foreach (var inviterGameAccount in inviterGameAccounts)
            {
                if (inviterGameAccount.IsOnline)
                {
                    inviterGameAccount.LoggedInClient.MakeTargetedRPC(FriendManager.Instance, () =>
                        bnet.protocol.friends.FriendsNotify.CreateStub(inviterGameAccount.LoggedInClient).NotifyReceivedInvitationRemoved(null, notificationToInviter, callback => { }));

                    inviterGameAccount.LoggedInClient.MakeTargetedRPC(FriendManager.Instance, () =>
                        bnet.protocol.friends.FriendsNotify.CreateStub(inviterGameAccount.LoggedInClient).NotifyFriendAdded(null, friendAddedNotificationToInviter, callback => { }));
                }
            }

            foreach (var inviteeGameAccount in inviteeGameAccounts)
            {
                if (inviteeGameAccount.IsOnline)
                {
                    inviteeGameAccount.LoggedInClient.MakeTargetedRPC(FriendManager.Instance, () =>
                        bnet.protocol.friends.FriendsNotify.CreateStub(inviteeGameAccount.LoggedInClient).NotifyFriendAdded(null, friendAddedNotificationToInvitee, callback => { }));

                    inviteeGameAccount.LoggedInClient.MakeTargetedRPC(FriendManager.Instance, () =>
                        bnet.protocol.friends.FriendsNotify.CreateStub(inviteeGameAccount.LoggedInClient).NotifyReceivedInvitationRemoved(null, notificationToInvitee, callback => { }));
                }
            }
        }

        public static void HandleDecline(MooNetClient client, bnet.protocol.invitation.GenericRequest request)
        {
            if (!OnGoingInvitations.ContainsKey(request.InvitationId)) return;
            var invitation = OnGoingInvitations[request.InvitationId];

            var inviter = AccountManager.GetAccountByPersistentID(invitation.InviterIdentity.AccountId.Low);
            var invitee = AccountManager.GetAccountByPersistentID(invitation.InviteeIdentity.AccountId.Low);

            var declinedNotification = bnet.protocol.friends.InvitationNotification.CreateBuilder()
                .SetInvitation(invitation)
                .SetReason((uint)InvitationRemoveReason.Declined).Build();

            var inviterGameAccounts = GameAccountManager.GetGameAccountsForAccount(inviter);
            var inviteeGameAccounts = GameAccountManager.GetGameAccountsForAccount(invitee);

            foreach (var inviterGameAccount in inviterGameAccounts)
            {
                if (inviterGameAccount.IsOnline)
                {
                    inviterGameAccount.LoggedInClient.MakeTargetedRPC(FriendManager.Instance, () =>
                        bnet.protocol.friends.FriendsNotify.CreateStub(inviterGameAccount.LoggedInClient).NotifyReceivedInvitationRemoved(null, declinedNotification, callback => { }));
                }
            }

            foreach (var inviteeGameAccount in inviteeGameAccounts)
            {
                if (inviteeGameAccount.IsOnline)
                {
                    inviteeGameAccount.LoggedInClient.MakeTargetedRPC(FriendManager.Instance, () =>
                        bnet.protocol.friends.FriendsNotify.CreateStub(inviteeGameAccount.LoggedInClient).NotifyReceivedInvitationRemoved(null, declinedNotification, callback => { }));
                }
            }
        }

        public static void HandleRemove(MooNetClient client, bnet.protocol.friends.GenericFriendRequest request)
        {
            var removee = AccountManager.GetAccountByPersistentID(request.TargetId.Low);
            var remover = client.Account;

            var removeeAsFriend = bnet.protocol.friends.Friend.CreateBuilder().SetId(removee.BnetEntityId).Build();
            var removerAsFriend = bnet.protocol.friends.Friend.CreateBuilder().SetId(remover.BnetEntityId).Build();

            RemoveFriendshipFromDB(remover, removee);

            var notifyRemover = bnet.protocol.friends.FriendNotification.CreateBuilder().SetTarget(removeeAsFriend).Build();

            client.MakeTargetedRPC(FriendManager.Instance, () =>
                bnet.protocol.friends.FriendsNotify.CreateStub(client).NotifyFriendRemoved(null, notifyRemover, callback => { }));


            var removeeGameAccounts = GameAccountManager.GetGameAccountsForAccount(removee);
            foreach (var removeeGameAccount in removeeGameAccounts)
            {
                if (!removeeGameAccount.IsOnline)
                    continue;
                var notifyRemovee = bnet.protocol.friends.FriendNotification.CreateBuilder().SetTarget(removerAsFriend).Build();
                removeeGameAccount.LoggedInClient.MakeTargetedRPC(FriendManager.Instance, () =>
                                                                                          bnet.protocol.friends.FriendsNotify.CreateStub(removeeGameAccount.LoggedInClient).NotifyFriendRemoved(null, notifyRemovee, callback => { }));
            }
        }

        private static void AddFriendshipToDB(Account inviter, Account invitee)
        {
            try
            {
                inviter.DBAccount.Friends.Add(invitee.DBAccount);
                invitee.DBAccount.Friends.Add(inviter.DBAccount);
                DBSessions.AccountSession.SaveOrUpdate(inviter.DBAccount);
                DBSessions.AccountSession.SaveOrUpdate(invitee.DBAccount);
                DBSessions.AccountSession.Flush();

            }
            catch (Exception e)
            {
                Logger.ErrorException(e, "FriendManager.AddFriendshipToDB()");
            }
        }

        private static void RemoveFriendshipFromDB(Account remover, Account removee)
        {
            try
            {
                remover.DBAccount.Friends.Remove(removee.DBAccount);
                removee.DBAccount.Friends.Remove(remover.DBAccount);
                DBSessions.AccountSession.SaveOrUpdate(remover.DBAccount);
                DBSessions.AccountSession.SaveOrUpdate(removee.DBAccount);
                DBSessions.AccountSession.Flush();

            }
            catch (Exception e)
            {
                Logger.ErrorException(e, "FriendManager.RemoveFriendshipFromDB()");
            }
        }

    }

    public enum InvitationRemoveReason : uint //possibly more unknown values, such as one for revoked /dustinconrad
    {
        Accepted = 0x0,
        Declined = 0x1
    }
}
