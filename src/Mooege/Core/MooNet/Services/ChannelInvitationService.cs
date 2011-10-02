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
using Mooege.Common;
using Mooege.Common.Extensions;
using Mooege.Core.Common.Toons;
using Mooege.Net.MooNet;
using bnet.protocol.channel_invitation;

namespace Mooege.Core.MooNet.Services
{
    [Service(serviceID: 0x3, serviceName: "bnet.protocol.channel_invitation.ChannelInvitationService")]
    public class ChannelInvitationService: bnet.protocol.channel_invitation.ChannelInvitationService, IServerService
    {
        private static readonly Logger Logger = LogManager.CreateLogger();
        public IMooNetClient Client { get; set; }

        public override void Subscribe(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.channel_invitation.SubscribeRequest request, System.Action<bnet.protocol.channel_invitation.SubscribeResponse> done)
        {
            Logger.Trace("Subscribe()");
            
            // NOTE: SubscribeRequest gives us an object_id
            // Client should be added as a subscriber to.. something
            
            /*
            // TODO: Set these to the correct values.
            const ulong accountHandle = 0x0000000000000000;
            const ulong gameAccountHandle = 0x0000000000000000;

            var invitation = bnet.protocol.invitation.Invitation.CreateBuilder()
                .SetId(0)
                .SetInviterIdentity(bnet.protocol.Identity.CreateBuilder()
                    .SetAccountId(Client.Account.BnetAccountID)
                    .SetGameAccountId(Client.Account.BnetGameAccountID)
                    .Build())
                .SetInviteeIdentity(bnet.protocol.Identity.CreateBuilder()
                    .SetAccountId(bnet.protocol.EntityId.CreateBuilder().SetHigh(accountHandle).SetLow(0x0).Build()) //TODO: Change SetLow to an actual index in the database.
                    .SetGameAccountId(bnet.protocol.EntityId.CreateBuilder().SetHigh(gameAccountHandle).SetLow(0x0).Build()) //TODO: Change SetLow to an actual index in the database.
                    .Build())
                .SetInviterName("YourName")
                .SetInviteeName("FriendName") // lookup this from agentid.toon_id?
                .SetInvitationMessage("Invite Message")
                .SetCreationTime(DateTime.Now.ToUnixTime())
                .SetExpirationTime(DateTime.Now.AddDays(2).ToUnixTime())
                .Build();

            var invite_collection = bnet.protocol.channel_invitation.InvitationCollection.CreateBuilder()
                .SetServiceType(0)
                .SetMaxReceivedInvitations(127)
                .SetObjectId(request.ObjectId)
                .AddReceivedInvitation(invitation)
                .Build();
            */

            var builder = bnet.protocol.channel_invitation.SubscribeResponse.CreateBuilder()
                //.AddCollection(invite_collection)
                /*.AddReceivedInvitation(invitation)*/;

            done(builder.Build());
        }

        public override void AcceptInvitation(Google.ProtocolBuffers.IRpcController controller, AcceptInvitationRequest request, System.Action<AcceptInvitationResponse> done)
        {
            throw new NotImplementedException();
        }

        public override void DeclineInvitation(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.invitation.GenericRequest request, System.Action<bnet.protocol.NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void RevokeInvitation(Google.ProtocolBuffers.IRpcController controller, RevokeInvitationRequest request, System.Action<bnet.protocol.NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void SendInvitation(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.invitation.SendInvitationRequest request, System.Action<bnet.protocol.invitation.SendInvitationResponse> done)
        {            
            var invitee = ToonManager.GetToonByLowID(request.TargetId.Low);
            Logger.Debug("{0} invited {1} to his channel", Client.CurrentToon.Name, invitee.Name);
            
            // somehow protobuf lib doesnt handle this extension, so we're using a workaround to get that channelinfo.
            var extensionBytes = request.UnknownFields.FieldDictionary[105].LengthDelimitedList[0].ToByteArray();
            var channelInvitationInfo = bnet.protocol.channel_invitation.SendInvitationRequest.ParseFrom(extensionBytes);

            var channelInvitation = bnet.protocol.channel_invitation.Invitation.CreateBuilder()
                .SetChannelDescription(bnet.protocol.channel.ChannelDescription.CreateBuilder().SetChannelId(channelInvitationInfo.ChannelId).Build())
                .SetReserved(channelInvitationInfo.Reserved)
                .SetServiceType(channelInvitationInfo.ServiceType)
                .SetRejoin(false).Build();
                
            var invitation = bnet.protocol.invitation.Invitation.CreateBuilder(); // also need to add creation_time, expiration_time.
            invitation.SetId(1) // TODO: fix the id
                .SetInviterIdentity(bnet.protocol.Identity.CreateBuilder().SetToonId(Client.CurrentToon.BnetEntityID).Build())
                .SetInviterName(Client.CurrentToon.Name)
                .SetInviteeIdentity(bnet.protocol.Identity.CreateBuilder().SetToonId(request.TargetId).Build())
                .SetInviteeName(invitee.Name)
                .SetInvitationMessage(request.InvitationMessage)
                .SetCreationTime(DateTime.Now.ToUnixTime())
                .SetExpirationTime(DateTime.Now.ToUnixTime() + request.ExpirationTime)
                .SetExtension(bnet.protocol.channel_invitation.Invitation.ChannelInvitation, channelInvitation);

            var builder = bnet.protocol.invitation.SendInvitationResponse.CreateBuilder()
                .SetInvitation(invitation);

            done(builder.Build());
        }

        public override void SuggestInvitation(Google.ProtocolBuffers.IRpcController controller, SuggestInvitationRequest request, System.Action<bnet.protocol.NoData> done)
        {
            // "request to join party"
            throw new NotImplementedException();
        }

        public override void Unsubscribe(Google.ProtocolBuffers.IRpcController controller, UnsubscribeRequest request, System.Action<bnet.protocol.NoData> done)
        {
            throw new NotImplementedException();
        }
    }
}
