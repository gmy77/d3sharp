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

using System;
using System.Collections.Generic;
using System.Linq;
using D3Sharp.Utils;
using D3Sharp.Core.Helpers;
using D3Sharp.Core.Objects;
using D3Sharp.Net.BNet;
using Google.ProtocolBuffers;

namespace D3Sharp.Core.Channels
{
    public class Channel : RPCObject
    {
        // Reasons the client tries to remove a member
        // TODO: Need more data to complete this
        public enum RemoveRequestReason : uint
        {
            RequestedBySelf = 0x00   // Default; generally when the client quits or leaves a channel (for example, when switching toons)
            // Kick is probably 0x01 or somesuch
        }
        
        // Reasons a member was removed (sent in NotifyRemove)
        public enum RemoveReason : uint
        {
            Kicked = 0x00,           // The member was kicked
            Left = 0x01              // The member left
        }
        
        public static RemoveReason GetRemoveReasonForRequest(RemoveRequestReason reqreason)
        {
            switch (reqreason)
            {
                case RemoveRequestReason.RequestedBySelf:
                    return RemoveReason.Left;
                default:
                    Logger.Warn("No RemoveReason for given RemoveRequestReason: {0}", Enum.GetName(typeof(RemoveRequestReason), reqreason));
                    break;
            }
            return RemoveReason.Left;
        }
        
        public bnet.protocol.EntityId BnetEntityId { get; private set; }
        public D3.OnlineService.EntityId D3EntityId { get; private set; }
        public bnet.protocol.channel.ChannelState State { get; private set; }

        public readonly Dictionary<BNetClient, bnet.protocol.channel.Member> Members = new Dictionary<BNetClient, bnet.protocol.channel.Member>();
        public BNetClient Owner { get; private set; }

        public Channel(BNetClient client, ulong remoteObjectId)
        {
            this.BnetEntityId = bnet.protocol.EntityId.CreateBuilder().SetHigh((ulong)EntityIdHelper.HighIdType.ChannelId).SetLow(this.DynamicId).Build();
            this.D3EntityId = D3.OnlineService.EntityId.CreateBuilder().SetIdHigh((ulong)EntityIdHelper.HighIdType.ChannelId).SetIdLow(this.DynamicId).Build();

            // This is an object creator, so we have to map the remote object ID
            client.MapLocalObjectID(this.DynamicId, remoteObjectId);
            
            var builder = bnet.protocol.channel.ChannelState.CreateBuilder()
                .SetPrivacyLevel(bnet.protocol.channel.ChannelState.Types.PrivacyLevel.PRIVACY_LEVEL_OPEN_INVITATION)
                .SetMaxMembers(8)
                .SetMinMembers(1)
                .SetMaxInvitations(12);
            //.SetName("d3sharp test channel"); // NOTE: cap log doesn't set this optional field
            this.State = builder.Build();
        }

        public void SetOwner(BNetClient client)
        {
            if (client == this.Owner)
            {
                Logger.Warn("Tried to set client {0} as owner of channel when it was already the owner", client.Connection.RemoteEndPoint.ToString());
                return;
            }
            RemoveOwner(RemoveReason.Left);
            AddMember(client);
            this.Owner = client;
        }

        public void RemoveOwner(RemoveReason reason)
        {
            if (this.Owner != null)
            {
                RemoveMember(this.Owner, reason);
            }
        }

        public void AddMember(BNetClient client)
        {
            if (HasUser(client))
            {
                Logger.Warn("Attempted to add client {0} to channel when it was already a member of the channel", client.Connection.RemoteEndPoint.ToString());
                return;
            }
            
            var identity = client.GetIdentity(false, false, true);
            var addedMember = bnet.protocol.channel.Member.CreateBuilder()
                .SetIdentity(identity)
                .SetState(bnet.protocol.channel.MemberState.CreateBuilder()
                    .AddRole(2)
                    .SetPrivileges(0xFBFF) // 64511
                    .Build())
                .Build();

            // This needs to be here so that the foreach below will also send to the client that was just added
            this.Members.Add(client, addedMember);
            
            var method = bnet.protocol.channel.ChannelSubscriber.Descriptor.FindMethodByName("NotifyAdd");
            foreach (var pair in this.Members)
            {
                var message = bnet.protocol.channel.AddNotification.CreateBuilder()
                    .SetChannelState(this.State)
                    // Here we have to set the self property for each call on each client
                    // TODO: This may not be necessary here (this field is optional); check the caps
                    .SetSelf(pair.Value)
                    .AddMember(addedMember)
                    .Build();
                pair.Key.CallMethod(method, message, this.DynamicId);
            }
            client.CurrentChannel = this;
        }
        
        public void RemoveAllMembers()
        {
            foreach (var pair in this.Members)
            {
                // TODO: There should probably be a RemoveReason for "channel disbanded"; find it!
                RemoveMember(pair.Key, RemoveReason.Left);
            }
        }
        
        public void RemoveMemberByID(bnet.protocol.EntityId memberId, RemoveReason reason)
        {
            var client = this.Members.FirstOrDefault(pair => pair.Value.Identity.ToonId == memberId).Key;
            RemoveMember(client, reason);
        }
        
        public void RemoveMember(BNetClient client, RemoveReason reason)
        {
            if (client.CurrentToon == null)
            {
                Logger.Warn("Could not remove toon-less client {0}", client.Connection.RemoteEndPoint.ToString());
                return;
            }
            else if (!HasUser(client))
            {
                Logger.Warn("Attempted to remove non-member client {0} from channel", client.Connection.RemoteEndPoint.ToString());
                return;
            }
            else if (client.CurrentChannel != this)
            {
                Logger.Warn("Client {0} is being removed from a channel that is not its current one..", client.Connection.RemoteEndPoint.ToString());
            }
            var memberId = this.Members[client].Identity.ToonId;
            var message = bnet.protocol.channel.RemoveNotification.CreateBuilder()
                .SetMemberId(memberId)
                .SetReason((uint)reason)
                .Build();
            //Logger.Debug("NotifyRemove message:\n{0}", message.ToString());
            var method = bnet.protocol.channel.ChannelSubscriber.Descriptor.FindMethodByName("NotifyRemove");
            foreach (var pair in this.Members)
            {
                pair.Key.CallMethod(method, message, this.DynamicId);
            }
            this.Members.Remove(client);
            client.CurrentChannel = null;
        }
        
        public bool HasUser(BNetClient client)
        {
            return this.Members.Any(pair => pair.Key == client);
        } 
    }
}
