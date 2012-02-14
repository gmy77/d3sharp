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
using Google.ProtocolBuffers;
using Mooege.Common.Extensions;
using Mooege.Common.Logging;
using Mooege.Core.MooNet.Accounts;
using Mooege.Core.MooNet.Friends;
using Mooege.Net.MooNet;

namespace Mooege.Core.MooNet.Services
{
    [Service(serviceID: 0x6, serviceName: "bnet.protocol.friends.FriendsService")]
    public class FriendsService : bnet.protocol.friends.FriendsService,IServerService
    {
        private static readonly Logger Logger = LogManager.CreateLogger();
        public MooNetClient Client { get; set; }
        public bnet.protocol.Header LastCallHeader { get; set; }

        public override void SubscribeToFriends(IRpcController controller, bnet.protocol.friends.SubscribeToFriendsRequest request, Action<bnet.protocol.friends.SubscribeToFriendsResponse> done)
        {
            Logger.Trace("Subscribe() {0}", this.Client);

            FriendManager.Instance.AddSubscriber(this.Client, request.ObjectId);

            var builder = bnet.protocol.friends.SubscribeToFriendsResponse.CreateBuilder()
                .SetMaxFriends(127)
                .SetMaxReceivedInvitations(127)
                .SetMaxSentInvitations(127)
                .AddRole(bnet.protocol.Role.CreateBuilder().SetId(1).SetName("battle_tag_friend").Build())
                .AddRole(bnet.protocol.Role.CreateBuilder().SetId(2).SetName("real_id_friend").Build());

            foreach (var friend in FriendManager.Friends[this.Client.Account.BnetEntityId.Low]) // send friends list.
            {
                builder.AddFriends(friend);
            }

            var invitations = new List<bnet.protocol.invitation.Invitation>();

            foreach (var invitation in FriendManager.OnGoingInvitations.Values)
            {
                if (invitation.InviteeIdentity.AccountId == this.Client.Account.BnetEntityId)
                {
                    invitations.Add(invitation);
                }
            }

            if (invitations.Count > 0)
                builder.AddRangeReceivedInvitations(invitations);

            done(builder.Build());
        }

        public override void SendInvitation(IRpcController controller, bnet.protocol.invitation.SendInvitationRequest request, Action<bnet.protocol.NoData> done)
        {
            //TODO: Add battletag invitation -Egris

            // somehow protobuf lib doesnt handle this extension, so we're using a workaround to get that channelinfo.
            var extensionBytes = request.Params.UnknownFields.FieldDictionary[103].LengthDelimitedList[0].ToByteArray();
            var friendRequest = bnet.protocol.friends.FriendInvitationParams.ParseFrom(extensionBytes);

            if (friendRequest.TargetEmail.ToLower() == this.Client.Account.Email.ToLower()) return; // don't allow him to invite himself - and we should actually return an error!
                                                                                                    // also he shouldn't be allowed to invite his current friends - put that check too!. /raist
            var inviteee = AccountManager.GetAccountByEmail(friendRequest.TargetEmail);
            if (inviteee == null) return; // we need send an error response here /raist.
            //Header.Status(4) = account does not exist

            Logger.Trace("{0} sent {1} friend invitation.", this.Client.Account, inviteee);

            var invitation = bnet.protocol.invitation.Invitation.CreateBuilder()
                .SetId(FriendManager.InvitationIdCounter++) // we may actually need to store invitation ids in database with the actual invitation there. /raist.                
                .SetInviterIdentity(this.Client.GetIdentity(true, false, false))
                .SetInviterName(this.Client.Account.Email) // we shoulde be instead using account owner's name here.
                .SetInviteeIdentity(bnet.protocol.Identity.CreateBuilder().SetAccountId(inviteee.BnetEntityId))
                .SetInviteeName(inviteee.Email) // again we should be instead using invitee's name.
                .SetInvitationMessage(request.Params.InvitationMessage)
                .SetCreationTime(DateTime.Now.ToUnixTime())
                .SetExpirationTime(86400);

            // Response is bnet.protocol.NoData as of 7728. /raist.
            //var response = bnet.protocol.invitation.SendInvitationResponse.CreateBuilder()
            //    .SetInvitation(invitation.Clone());

            var response = bnet.protocol.NoData.CreateBuilder();
            done(response.Build());

            // notify the invitee on invitation.
            FriendManager.HandleInvitation(this.Client, invitation.Build());
        }

        public override void UpdateInvitation(IRpcController controller, bnet.protocol.invitation.UpdateInvitationRequest request, Action<bnet.protocol.NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void AcceptInvitation(IRpcController controller, bnet.protocol.invitation.GenericRequest request, Action<bnet.protocol.NoData> done)
        {
            Logger.Trace("{0} accepted friend invitation.", this.Client.Account);

            var response = bnet.protocol.NoData.CreateBuilder();
            done(response.Build());

            FriendManager.HandleAccept(this.Client, request);
        }

        public override void RevokeInvitation(IRpcController controller, bnet.protocol.invitation.GenericRequest request, Action<bnet.protocol.NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void DeclineInvitation(IRpcController controller, bnet.protocol.invitation.GenericRequest request, Action<bnet.protocol.NoData> done)
        {
            Logger.Trace("{0} declined friend invitation.", this.Client.Account);

            var response = bnet.protocol.NoData.CreateBuilder();
            done(response.Build());

            FriendManager.HandleDecline(this.Client, request);
        }

        public override void IgnoreInvitation(IRpcController controller, bnet.protocol.invitation.GenericRequest request, Action<bnet.protocol.NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void AssignRole(IRpcController controller, bnet.protocol.friends.AssignRoleRequest request, Action<bnet.protocol.NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void RemoveFriend(IRpcController controller, bnet.protocol.friends.GenericFriendRequest request, Action<bnet.protocol.friends.GenericFriendResponse> done)
        {
            Logger.Trace("{0} removed friend with id {1}.", this.Client.Account, request.TargetId);

            var response = bnet.protocol.friends.GenericFriendResponse.CreateBuilder()
                .SetTargetFriend(bnet.protocol.friends.Friend.CreateBuilder().SetId(request.TargetId).Build());

            done(response.Build());

            FriendManager.HandleRemove(this.Client, request);
        }

        public override void ViewFriends(IRpcController controller, bnet.protocol.friends.ViewFriendsRequest request, Action<bnet.protocol.friends.ViewFriendsResponse> done)
        {
            throw new NotImplementedException();
        }

        public override void UpdateFriendState(IRpcController controller, bnet.protocol.friends.UpdateFriendStateRequest request, Action<bnet.protocol.NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void UnsubscribeToFriends(IRpcController controller, bnet.protocol.friends.UnsubscribeToFriendsRequest request, Action<bnet.protocol.NoData> done)
        {
            throw new NotImplementedException();
        }
    }
}
