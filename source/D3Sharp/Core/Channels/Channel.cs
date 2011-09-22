/*
 * Copyright (C) 2011 D3Sharp Project
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
using D3Sharp.Core.Helpers;
using D3Sharp.Core.Objects;
using D3Sharp.Net.BNet;
using Google.ProtocolBuffers;

namespace D3Sharp.Core.Channels
{
    public class Channel : RPCObject
    {
        public bnet.protocol.EntityId BnetEntityId { get; private set; }
        public D3.OnlineService.EntityId D3EntityId { get; private set; }
        public bnet.protocol.channel.ChannelState State { get; private set; }

        public readonly Dictionary<BNetClient, bnet.protocol.channel.Member> Members = new Dictionary<BNetClient, bnet.protocol.channel.Member>();

        public Channel(BNetClient client, ulong remoteObjectId)
        {
            this.BnetEntityId = bnet.protocol.EntityId.CreateBuilder().SetHigh((ulong)EntityIdHelper.HighIdType.ChannelId).SetLow(this.DynamicId).Build();
            this.D3EntityId = D3.OnlineService.EntityId.CreateBuilder().SetIdHigh((ulong) EntityIdHelper.HighIdType.ChannelId).SetIdLow(this.DynamicId).Build();

            // This is an object creator, so we have to map the remote object ID
            client.MapLocalObjectID(this.DynamicId, remoteObjectId);
            
            var builder = bnet.protocol.channel.ChannelState.CreateBuilder()
                .SetPrivacyLevel(bnet.protocol.channel.ChannelState.Types.PrivacyLevel.PRIVACY_LEVEL_OPEN)
                .SetMaxMembers(8)
                .SetMinMembers(1)
                .SetMaxInvitations(12);
            //.SetName("d3sharp test channel"); // NOTE: cap log doesn't set this optional field
            this.State = builder.Build();

            // Add the client that requested the creation of channel as the owner
            this.AddOwner(client);
            client.CurrentChannel = this;
        }

        public void AddOwner(BNetClient client)
        {
            var identity = client.GetIdentity(false, false, true);
            var member = bnet.protocol.channel.Member.CreateBuilder()
                .SetIdentity(identity)
                .SetState(bnet.protocol.channel.MemberState.CreateBuilder()
                    .AddRole(2)
                    .SetPrivileges(0xFBFF) // 64511
                    .Build())
                .Build();

            // Be careful when editing the below RPC call, you may break in-game to error!! /raist
            var builder = bnet.protocol.channel.AddNotification.CreateBuilder()
                .SetChannelState(this.State)
                .SetSelf(member);
            builder.AddMember(member);
            client.CallMethod(bnet.protocol.channel.ChannelSubscriber.Descriptor.FindMethodByName("NotifyAdd"), builder.Build(), this.DynamicId);

            this.Members.Add(client, member);
        }

        public bool HasUser(BNetClient client)
        {
            return this.Members.Any(pair => pair.Key == client);
        } 
    }
}
