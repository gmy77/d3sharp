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
using System.Linq;
using Mooege.Core.Common.Toons;
using Mooege.Core.MooNet.Games;
using Mooege.Core.MooNet.Helpers;
using Mooege.Core.MooNet.Objects;
using Mooege.Net.MooNet;

namespace Mooege.Core.MooNet.Channels
{
    public class Channel : RPCObject
    {
        /// <summary>
        /// bnet.protocol.EntityId encoded channel Id.
        /// </summary>
        public bnet.protocol.EntityId BnetEntityId { get; private set; }

        /// <summary>
        /// D3.OnlineService.EntityId encoded channel Id.
        /// </summary>
        public D3.OnlineService.EntityId D3EntityId { get; private set; }

        /// <summary>
        /// Channel PrivacyLevel.
        /// </summary>
        public bnet.protocol.channel.ChannelState.Types.PrivacyLevel PrivacyLevel { get; private set; }

        /// <summary>
        /// The bound game for channel.
        /// </summary>
        public GameCreator Game { get; private set; }

        /// <summary>
        /// Max number of members.
        /// </summary>
        public uint MaxMembers { get; set; }

        /// <summary>
        /// Minimum number of members.
        /// </summary>
        public uint MinMembers { get; set; }

        /// <summary>
        /// Maximum invitations.
        /// </summary>
        public uint MaxInvitations { get; set; }

        /// <summary>
        /// List of channel members.
        /// </summary>
        public readonly Dictionary<MooNetClient, Member> Members = new Dictionary<MooNetClient, Member>();

        /// <summary>
        /// Channel owner.
        /// </summary>
        public MooNetClient Owner { get; private set; }

        /// <summary>
        /// Creates a new channel for given client with supplied remote object-id.
        /// </summary>
        /// <param name="client">The client channels is created for</param>
        /// <param name="remoteObjectId">The remove object-id of the client.</param>
        public Channel(MooNetClient client, ulong remoteObjectId)
        {
            this.BnetEntityId = bnet.protocol.EntityId.CreateBuilder().SetHigh((ulong)EntityIdHelper.HighIdType.ChannelId).SetLow(this.DynamicId).Build();
            this.D3EntityId = D3.OnlineService.EntityId.CreateBuilder().SetIdHigh((ulong)EntityIdHelper.HighIdType.ChannelId).SetIdLow(this.DynamicId).Build();
            this.PrivacyLevel = bnet.protocol.channel.ChannelState.Types.PrivacyLevel.PRIVACY_LEVEL_OPEN_INVITATION;
            this.MinMembers = 1;
            this.MaxMembers = 8;
            this.MaxInvitations = 12;

            // This is an object creator, so we have to map the remote object ID
            client.MapLocalObjectID(this.DynamicId, remoteObjectId);

            // The client can't be set as the owner (or added as a member) here because the server must first make a response
            // to the client before using a mapped ID (presuming that this was called from a service).
            // We'll just let the caller do that for us.

            this.Game = GameCreatorManager.CreateGame(this); // attach a game to channel.
        }

        #region common methods 

        public bool HasUser(MooNetClient client)
        {
            return this.Members.Any(pair => pair.Key == client);
        }

        public bool HasToon(Toon toon) // checks if given toon is already channels member
        {
            return this.Members.Any(pair => pair.Value.Identity.ToonId.Low == toon.BnetEntityID.Low);
        }

        public Member GetMember(MooNetClient client)
        {
            return this.Members[client];
        }

        public void Dissolve()
        {
            ChannelManager.DissolveChannel(this.DynamicId);
        }

        #endregion

        #region owner functionality

        public void SetOwner(MooNetClient client)
        {
            if (client == this.Owner)
            {
                Logger.Warn("Tried to set client {0} as owner of channel when it was already the owner", client.Connection.RemoteEndPoint.ToString());
                return;
            }
            RemoveOwner(RemoveReason.Left); // TODO: Should send state update to current owner instead of removing it
            this.Owner = client;
            AddMember(client);
        }

        public void RemoveOwner(RemoveReason reason)
        {
            if (this.Owner == null) return;

            RemoveMember(this.Owner, reason, false);
            this.Owner = null;
        }

        #endregion

        #region member functinality

        public void Join(MooNetClient client, ulong remoteObjectId)
        {
            client.MapLocalObjectID(this.DynamicId, remoteObjectId);
            this.AddMember(client);
        }

        public void AddMember(MooNetClient client)
        {
            if (HasUser(client))
            {
                Logger.Warn("Attempted to add client {0} to channel when it was already a member of the channel", client.Connection.RemoteEndPoint.ToString());
                return;
            }

            var identity = client.GetIdentity(false, false, true);

            bool isOwner = client == this.Owner;
            var addedMember = new Member(identity, (isOwner) ? Member.Privilege.UnkCreator : Member.Privilege.UnkMember);

            if (this.Members.Count > 0)
            {
                addedMember.AddRoles((isOwner) ? Member.Role.PartyLeader : Member.Role.PartyMember, Member.Role.ChannelMember);
            }
            else
            {
                addedMember.AddRole((isOwner) ? Member.Role.ChannelCreator : Member.Role.ChannelMember);
            }

            // This needs to be here so that the foreach below will also send to the client that was just added
            this.Members.Add(client, addedMember);

            // Cache the built state and member
            var channelState = this.State;

            // added member should recieve a NotifyAdd.
            var addNotification = bnet.protocol.channel.AddNotification.CreateBuilder()
                .SetChannelState(channelState)
                .SetSelf(addedMember.BnetMember)
                .AddRangeMember(this.Members.Values.ToList().Select(member => member.BnetMember).ToList()).Build();

            client.MakeTargetedRPC(this, () =>
                bnet.protocol.channel.ChannelSubscriber.CreateStub(client).NotifyAdd(null, addNotification, callback => { }));

            client.CurrentChannel = this; // set clients current channel to one he just joined.

            if (this.Members.Count < 2) return;

            // other members should recieve a NotifyJoin.
            var joinNotification = bnet.protocol.channel.JoinNotification.CreateBuilder()
                .SetMember(addedMember.BnetMember).Build();

            foreach (var pair in this.Members.Where(pair => pair.Value != addedMember)) // only send this to previous members of the channel.
            {
                pair.Key.MakeTargetedRPC(this, () =>
                    bnet.protocol.channel.ChannelSubscriber.CreateStub(pair.Key).NotifyJoin(null, joinNotification, callback => { }));
            }
        }

        public void RemoveAllMembers(bool dissolving)
        {
            if (!dissolving)
            {
                Dissolve();
                return;
            }
            foreach (var pair in this.Members)
            {
                // TODO: There should probably be a RemoveReason for "channel dissolved"; find it!
                RemoveMember(pair.Key, RemoveReason.Left, true);
            }
        }

        public void RemoveMemberByID(bnet.protocol.EntityId memberId, RemoveReason reason)
        {
            var client = this.Members.FirstOrDefault(pair => pair.Value.Identity.ToonId == memberId).Key;
            RemoveMember(client, reason, false);
        }

        public void RemoveMember(MooNetClient client, RemoveReason reason)
        {
            RemoveMember(client, reason, false);
        }

        public void RemoveMember(MooNetClient client, RemoveReason reason, bool dissolving)
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

            foreach (var pair in this.Members)
            {
                pair.Key.MakeTargetedRPC(this, () =>
                    bnet.protocol.channel.ChannelSubscriber.CreateStub(pair.Key).NotifyRemove(null, message, callback => { }));
            }

            this.Members.Remove(client);
            client.CurrentChannel = null;

            if (client == this.Owner)
                this.Owner = null;

            if (this.Members.Count == 0 && !dissolving)
                Dissolve();
        }

        #endregion

        #region channel-messaging

        public void SendMessage(MooNetClient client, bnet.protocol.channel.Message message)
        {
            var notification =
                bnet.protocol.channel.SendMessageNotification.CreateBuilder().SetAgentId(client.CurrentToon.BnetEntityID)
                    .SetMessage(message).SetRequiredPrivileges(0).Build();
                    

            foreach (var pair in this.Members) // send to all members of channel even to the actual one that sent the message else he'll not see his own message.
            {
                pair.Key.MakeTargetedRPC(this, () =>
                    bnet.protocol.channel.ChannelSubscriber.CreateStub(pair.Key).NotifySendMessage(null, notification, callback => { }));
            }
        }

        #endregion
        
        #region channel state messages

        /// <summary>
        /// bnet.protocol.channel.ChannelState message.
        /// </summary>
        public bnet.protocol.channel.ChannelState State
        {
            get
            {
                return bnet.protocol.channel.ChannelState.CreateBuilder()
                    .SetMinMembers(this.MinMembers)
                    .SetMaxMembers(this.MaxMembers)
                    .SetMaxInvitations(this.MaxInvitations)
                    .SetPrivacyLevel(this.PrivacyLevel)
                    .Build();
            }
        }

        /// <summary>
        /// bnet.protocol.channel.ChannelDescription message.
        /// </summary>
        public bnet.protocol.channel.ChannelDescription Description
        {
            get
            {
                var builder = bnet.protocol.channel.ChannelDescription.CreateBuilder() // NOTE: Can have extensions
                    .SetChannelId(this.BnetEntityId)
                    .SetState(this.State);

                if (this.Members.Count > 0) // No reason to set a value that defaults to 0
                    builder.SetCurrentMembers((uint)this.Members.Count);
                return builder.Build();
            }
        }

        /// <summary>
        /// bnet.protocol.channel.ChannelInfo message.
        /// </summary>
        public bnet.protocol.channel.ChannelInfo Info
        {
            get
            {
                var builder = bnet.protocol.channel.ChannelInfo.CreateBuilder() // NOTE: Can have extensions
                    .SetDescription(this.Description);

                foreach (var pair in this.Members)
                {
                    builder.AddMember(pair.Value.BnetMember);
                }

                return builder.Build();
            }
        }     

        #endregion

        #region remove-reason helpers 

        // Reasons the client tries to remove a member - // TODO: Need more data to complete this        
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

        #endregion
    }
}
