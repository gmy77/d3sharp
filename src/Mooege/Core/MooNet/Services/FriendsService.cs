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
using Google.ProtocolBuffers;
using Mooege.Common;
using Mooege.Common.Extensions;
using Mooege.Net.MooNet;
using bnet.protocol;
using bnet.protocol.friends;
using bnet.protocol.invitation;

namespace Mooege.Core.MooNet.Services
{
    [Service(serviceID: 0x6, serviceName: "bnet.protocol.friends.FriendsService")]
    public class FriendsService : bnet.protocol.friends.FriendsService,IServerService
    {
        private static readonly Logger Logger = LogManager.CreateLogger();
        public IMooNetClient Client { get; set; }

        public override void SubscribeToFriends(IRpcController controller, SubscribeToFriendsRequest request, Action<SubscribeToFriendsResponse> done)
        {
            Logger.Trace("SubscribeToFriends()");
            var builder = SubscribeToFriendsResponse.CreateBuilder()
                .SetMaxFriends(127)
                .SetMaxReceivedInvitations(127)
                .SetMaxSentInvitations(127);
            done(builder.Build());
        }

        public override void SendInvitation(IRpcController controller, bnet.protocol.invitation.SendInvitationRequest request, Action<SendInvitationResponse> done)
        {
            Logger.Trace("SendInvitation() Stub");

            //TODO: Set these to the corect values.
            const ulong accountHandle = 0x0000000000000000;
            const ulong gameAccountHandle = 0x0000000000000000;

            var invitation = bnet.protocol.invitation.Invitation.CreateBuilder()
                .SetCreationTime(DateTime.Now.ToUnixTime())
                .SetExpirationTime(request.ExpirationTime)
                .SetId(0)
                .SetInvitationMessage(request.InvitationMessage)
                .SetInviteeIdentity(bnet.protocol.Identity.CreateBuilder()
                                        .SetAccountId(bnet.protocol.EntityId.CreateBuilder().SetHigh(accountHandle).SetLow(0x1).Build()) //TODO: Change SetLow to an actual index in the database.
                                        .SetGameAccountId(bnet.protocol.EntityId.CreateBuilder().SetHigh(gameAccountHandle).SetLow(0x1).Build()) //TODO: Change SetLow to an actual index in the database.
                                        .Build())
                .SetInviteeName("FriendName") //TODO: Set this to the name retrieved from the database.
                .SetInviterIdentity(bnet.protocol.Identity.CreateBuilder()
                                        .SetAccountId(bnet.protocol.EntityId.CreateBuilder().SetHigh(accountHandle).SetLow(0x0).Build()) //TODO: Change SetLow to an actual index in the database.
                                        .SetGameAccountId(bnet.protocol.EntityId.CreateBuilder().SetHigh(gameAccountHandle).SetLow(0x0).Build()) //TODO: Change SetLow to an actual index in the database.
                                        .Build())
                .SetInviterName("YourName") //TODO: Set this to the name retrieved from the database.
                .Build();

            var builder = bnet.protocol.invitation.SendInvitationResponse.CreateBuilder().SetInvitation(invitation);
            done(builder.Build());
        }

        public override void AcceptInvitation(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.invitation.GenericRequest request, Action<bnet.protocol.NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void RevokeInvitation(IRpcController controller, GenericRequest request, Action<NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void DeclineInvitation(IRpcController controller, GenericRequest request, Action<NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void IgnoreInvitation(IRpcController controller, GenericRequest request, Action<NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void RemoveFriend(IRpcController controller, GenericFriendRequest request, Action<GenericFriendResponse> done)
        {
            throw new NotImplementedException();
        }

        public override void ViewFriends(IRpcController controller, ViewFriendsRequest request, Action<ViewFriendsResponse> done)
        {
            throw new NotImplementedException();
        }

        public override void UpdateFriendState(IRpcController controller, UpdateFriendStateRequest request, Action<UpdateFriendStateResponse> done)
        {
            throw new NotImplementedException();
        }

        public override void UnsubscribeToFriends(IRpcController controller, UnsubscribeToFriendsRequest request, Action<NoData> done)
        {
            throw new NotImplementedException();
        }
    }
}
