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
        public bnet.protocol.EntityId BnetEntityID { get; private set; }
        public bnet.protocol.channel.ChannelState State { get; private set; }

        public readonly Dictionary<BNetClient, bnet.protocol.channel.Member> Members = new Dictionary<BNetClient, bnet.protocol.channel.Member>();

        public Channel(BNetClient client)
        {
            this.BnetEntityID = bnet.protocol.EntityId.CreateBuilder().SetHigh((ulong)EntityIdHelper.HighIdType.ChannelId).SetLow(this.DynamicId).Build();

            var builder = bnet.protocol.channel.ChannelState.CreateBuilder()
                .SetPrivacyLevel(bnet.protocol.channel.ChannelState.Types.PrivacyLevel.PRIVACY_LEVEL_OPEN)
                .SetMaxMembers(8)
                .SetMinMembers(1)
                .SetMaxInvitations(12);
            //.SetName("d3sharp test channel"); // NOTE: cap log doesn't set this optional field
            this.State = builder.Build();

            // add the client that requested the creation of channel to channel
            this.AddMember(client);
        }

        public void AddMember(BNetClient client)
        {
            var identity = client.GetIdentity(false, false, true);
            var member = bnet.protocol.channel.Member.CreateBuilder()
                .SetIdentity(identity)
                .SetState(bnet.protocol.channel.MemberState.CreateBuilder()
                    .AddRole(2)
                    .SetPrivileges(0xFBFF) // 64511
                    .Build())
                .Build();

            // be carefult when editing the below rpc call, you may broke in game to error!! /raist.
            var builder = bnet.protocol.channel.AddNotification.CreateBuilder()
                .SetChannelState(this.State)
                .SetSelf(member);
            client.CallMethod(bnet.protocol.channel.ChannelSubscriber.Descriptor.FindMethodByName("NotifyAdd"), builder.Build(), this.DynamicId);

            this.Members.Add(client, member);
        }

        /*public void NotifyJoin(BNetClient client)
        {
            var notification = bnet.protocol.channel.JoinNotification.CreateBuilder().SetMember(this.Members[client]).Build();

            foreach(var pair in Members)
            {
                if (pair.Key == client) continue;

                pair.Key.CallMethod(bnet.protocol.channel.ChannelSubscriber.Descriptor.FindMethodByName("NotifyJoin"), notification, this.DynamicId);
            }
        }    */

        public bool HasUser(BNetClient client)
        {
            return this.Members.Any(pair => pair.Key == client);
        }

        public void RemoveUser(BNetClient client)
        {
            //// send notification remove to client itself.
            //var identity = client.GetIdentity(false, false, true);
            //var builder = bnet.protocol.channel.RemoveNotification.CreateBuilder()
            //    .SetMemberId(identity.ToonId);

            //this.Members.Remove(client);
            //client.CallMethod(bnet.protocol.channel.ChannelSubscriber.Descriptor.FindMethodByName("NotifyRemove"), builder.Build(), this.DynamicId);

            //// notify all subscribers.
            //this.NotifySubscribers();
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

        //public void NotifyChannelState(BNetClient client)
        //{
        //    var field1 =
        //        bnet.protocol.presence.Field.CreateBuilder().SetKey(
        //            bnet.protocol.presence.FieldKey.CreateBuilder().SetProgram(16974).SetGroup(3).SetField(3).SetIndex(0)
        //                .Build()).SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetBoolValue(true).Build()).Build();

        //    var field2 =
        //        bnet.protocol.presence.Field.CreateBuilder().SetKey(
        //            bnet.protocol.presence.FieldKey.CreateBuilder().SetProgram(16974).SetGroup(3).SetField(10).SetIndex(0)
        //                .Build()).SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetIntValue(1315530390868296).Build()).Build();

        //    var field3 =
        //        bnet.protocol.presence.Field.CreateBuilder().SetKey(
        //            bnet.protocol.presence.FieldKey.CreateBuilder().SetProgram(16974).SetGroup(3).SetField(11).SetIndex(0)
        //                .Build()).SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetMessageValue(
        //                        ByteString.CopyFrom(new byte[]
        //                                                {
        //                                                    0x9, 0x46, 0xee, 0x00, 0x00, 0x00, 0x00, 0x00, 0x4,
        //                                                    0x11, 0xdd, 0xb4, 0x63, 0xe7, 0x82, 0x44, 0x68, 0x4e
        //                                                })).Build()).Build();


        //    var fieldOperation1 = bnet.protocol.presence.FieldOperation.CreateBuilder().SetField(field1).Build();
        //    var fieldOperation2 = bnet.protocol.presence.FieldOperation.CreateBuilder().SetField(field2).Build();
        //    var fieldOperation3 = bnet.protocol.presence.FieldOperation.CreateBuilder().SetField(field3).Build();

        //    var state =
        //        bnet.protocol.presence.ChannelState.CreateBuilder().SetEntityId(this.BnetEntityID).AddFieldOperation(
        //            fieldOperation1).AddFieldOperation(fieldOperation2).AddFieldOperation(fieldOperation3).Build();


        //    var channelState = bnet.protocol.channel.ChannelState.CreateBuilder().SetExtension(bnet.protocol.presence.ChannelState.Presence, state);
        //    var builder = bnet.protocol.channel.UpdateChannelStateNotification.CreateBuilder().SetStateChange(channelState);

        //    client.CallMethod(bnet.protocol.channel.ChannelSubscriber.Descriptor.FindMethodByName("NotifyUpdateChannelState"), builder.Build(), this.DynamicId);
        //}
    }
}
