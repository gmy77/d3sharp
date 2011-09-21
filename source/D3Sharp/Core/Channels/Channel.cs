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
using D3Sharp.Core.Objects;
using D3Sharp.Net.BNet;
using Google.ProtocolBuffers;

namespace D3Sharp.Core.Channels
{
    public class Channel : RPCObject
    {
        public ulong Id { get; private set; }
        public static ulong IdCounter = 0;

        public bnet.protocol.EntityId BnetEntityID { get; private set; }
        public bnet.protocol.channel.ChannelState State { get; private set; }

        public List<bnet.protocol.channel.Member> Members = new List<bnet.protocol.channel.Member>();

        public Channel()
        {
            this.Id = ++IdCounter;
            this.BnetEntityID = bnet.protocol.EntityId.CreateBuilder().SetHigh(this.Id).SetLow(0).Build();

            var builder = bnet.protocol.channel.ChannelState.CreateBuilder()
                .SetPrivacyLevel(bnet.protocol.channel.ChannelState.Types.PrivacyLevel.PRIVACY_LEVEL_OPEN)
                .SetMaxMembers(8)
                .SetMinMembers(1)
                .SetMaxInvitations(12);
                //.SetName("d3sharp test channel"); // Note: cap log doesn't set this optional field
            this.State = builder.Build();
        }

        public void NotifyChannelState(BNetClient client)
        {
            var field1 =
                bnet.protocol.presence.Field.CreateBuilder().SetKey(
                    bnet.protocol.presence.FieldKey.CreateBuilder().SetProgram(16974).SetGroup(3).SetField(3).SetIndex(0)
                        .Build()).SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetBoolValue(true).Build()).Build();

            var field2 =
                bnet.protocol.presence.Field.CreateBuilder().SetKey(
                    bnet.protocol.presence.FieldKey.CreateBuilder().SetProgram(16974).SetGroup(3).SetField(10).SetIndex(0)
                        .Build()).SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetIntValue(1315530390868296).Build()).Build();

            var field3 =
                bnet.protocol.presence.Field.CreateBuilder().SetKey(
                    bnet.protocol.presence.FieldKey.CreateBuilder().SetProgram(16974).SetGroup(3).SetField(11).SetIndex(0)
                        .Build()).SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetMessageValue(
                                ByteString.CopyFrom(new byte[]
                                                        {
                                                            0x9, 0x46, 0xee, 0x00, 0x00, 0x00, 0x00, 0x00, 0x4,
                                                            0x11, 0xdd, 0xb4, 0x63, 0xe7, 0x82, 0x44, 0x68, 0x4e
                                                        })).Build()).Build();


            var fieldOperation1 = bnet.protocol.presence.FieldOperation.CreateBuilder().SetField(field1).Build();
            var fieldOperation2 = bnet.protocol.presence.FieldOperation.CreateBuilder().SetField(field2).Build();
            var fieldOperation3 = bnet.protocol.presence.FieldOperation.CreateBuilder().SetField(field3).Build();

            var state =
                bnet.protocol.presence.ChannelState.CreateBuilder().SetEntityId(this.BnetEntityID).AddFieldOperation(
                    fieldOperation1).AddFieldOperation(fieldOperation2).AddFieldOperation(fieldOperation3).Build();


            var channelState = bnet.protocol.channel.ChannelState.CreateBuilder().SetExtension(bnet.protocol.presence.ChannelState.Presence, state);
            var builder = bnet.protocol.channel.UpdateChannelStateNotification.CreateBuilder().SetStateChange(channelState);

            client.CallMethod(bnet.protocol.channel.ChannelSubscriber.Descriptor.FindMethodByName("NotifyUpdateChannelState"), builder.Build());
        }

        public void Add(BNetClient client)
        {
            var identity = client.GetIdentity(false, false, true);
            var user = bnet.protocol.channel.Member.CreateBuilder()
                .SetIdentity(identity)
                .SetState(bnet.protocol.channel.MemberState.CreateBuilder()
                    .AddRole(2)
                    .SetPrivileges(0xFBFF) // 64511
                    .Build())
                .Build();
            this.Members.Add(user);

            var builder = bnet.protocol.channel.AddNotification.CreateBuilder()
                .SetChannelState(this.State)
                .SetSelf(user);
            
            // Cap includes the user that was added
            foreach (var m in this.Members)
            {
                builder.AddMember(m);
            }
            client.CallMethod(bnet.protocol.channel.ChannelSubscriber.Descriptor.FindMethodByName("NotifyAdd"), builder.Build(), this.LocalObjectId);
        }

        public bool HasUser(BNetClient client)
        {
            return this.Members.Any(m => m.Identity == client.GetIdentity(false, false, true));
        }
        
        /*public void Close()
        {
            RemoveAllUsers();
        }
        
        public void RemoveAllUsers()
        {
            // Need a way to iterate clients on the server to send a NotifyRemove
            // and then call RemoveUser on them
            this.Members.Clear();
        }*/
        
        public void RemoveUser(BNetClient client)
        {
            var identity = client.GetIdentity(false, false, true);
            var builder = bnet.protocol.channel.RemoveNotification.CreateBuilder()
                .SetMemberId(identity.ToonId);
            this.Members.RemoveAll(m => identity == m.Identity);
            client.CurrentChannel = null;
            client.CallMethod(bnet.protocol.channel.ChannelSubscriber.Descriptor.FindMethodByName("NotifyRemove"), builder.Build(), this.LocalObjectId);
        }
    }
}
