﻿/*
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
using System.Linq;
using Mooege.Common.Logging;
using Mooege.Core.MooNet.Channels;
using Mooege.Core.MooNet.Commands;
using Mooege.Net.MooNet;

namespace Mooege.Core.MooNet.Services
{
    [Service(serviceID: 0x10, serviceName: "bnet.protocol.channel.Channel")]
    public class ChannelService : bnet.protocol.channel.Channel, IServerService
    {
        private static readonly Logger Logger = LogManager.CreateLogger();
        public MooNetClient Client { get; set; }
        public bnet.protocol.Header LastCallHeader { get; set; }

        public override void AddMember(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.channel.AddMemberRequest request, System.Action<bnet.protocol.NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void Dissolve(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.channel.DissolveRequest request, System.Action<bnet.protocol.NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void RemoveMember(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.channel.RemoveMemberRequest request, System.Action<bnet.protocol.NoData> done)
        {
            Logger.Trace("RemoveMember()");

            // TODO: we should be actually checking for which member has to be removed. /raist.            
            var builder = bnet.protocol.NoData.CreateBuilder();
            done(builder.Build());
            this.Client.CurrentChannel.RemoveMember(this.Client, Channel.GetRemoveReasonForRequest((Channel.RemoveRequestReason)request.Reason));
        }

        public override void SendMessage(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.channel.SendMessageRequest request, System.Action<bnet.protocol.NoData> done)
        {
            Logger.Trace("{0} sent a message to channel {1}.", this.Client.CurrentToon, this.Client.CurrentChannel);

            var builder = bnet.protocol.NoData.CreateBuilder();
            done(builder.Build());

            if (!request.HasMessage) 
                return; // only continue if the request actually contains a message.
            
            if (request.Message.AttributeCount == 0 || !request.Message.AttributeList.First().HasValue)
                return; // check if it has attributes.

            var channel = ChannelManager.GetChannelByDynamicId(this.LastCallHeader.ObjectId);
            var parsedAsCommand = CommandManager.TryParse(request.Message.AttributeList[0].Value.StringValue, this.Client); // try parsing the message as a command

            if(!parsedAsCommand)
                channel.SendMessage(this.Client, request.Message); // if it's not parsed as an command - let channel itself to broadcast message to it's members.              
        }

        public override void SetRoles(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.channel.SetRolesRequest request, System.Action<bnet.protocol.NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void UpdateChannelState(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.channel.UpdateChannelStateRequest request, System.Action<bnet.protocol.NoData> done)
        {
            Logger.Trace("UpdateChannelState()");

            // TODO: Should be actually applying changes on channel. /raist.
            var channelState = bnet.protocol.channel.ChannelState.CreateBuilder();

            foreach (bnet.protocol.attribute.Attribute attribute in request.StateChange.AttributeList)
            {
                if (attribute.Name == "D3.Party.GameCreateParams")
                {
                    if (attribute.HasValue && !attribute.Value.MessageValue.IsEmpty) //Sometimes not present -Egris
                    {
                        var gameCreateParams = D3.OnlineService.GameCreateParams.ParseFrom(attribute.Value.MessageValue);

                        var attr = bnet.protocol.attribute.Attribute.CreateBuilder()
                            .SetName("D3.Party.GameCreateParams")
                            .SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetMessageValue(gameCreateParams.ToByteString()).Build());
                        channelState.AddAttribute(attr);
                    }
                }
                else if (attribute.Name == "D3.Party.SearchForPublicGame.Params")
                {
                    // TODO: Find a game that fits the clients params and join /raist.
                    var publicGameParams = D3.PartyMessage.SearchForPublicGameParams.ParseFrom(attribute.Value.MessageValue);
                    Logger.Warn("SearchForPublicGameParams: {0}", publicGameParams.ToString());
                }
                else if (attribute.Name == "D3.Party.ScreenStatus")
                {
                    if (!this.Client.MOTDSent)
                        this.Client.SendMOTD(); // send the MOTD to client if we haven't yet so.

                    if (!attribute.HasValue || attribute.Value.MessageValue.IsEmpty) //Sometimes not present -Egris
                    {
                        var newScreen = this.Client.Account.ScreenStatus;

                        var attr = bnet.protocol.attribute.Attribute.CreateBuilder()
                            .SetName("D3.Party.ScreenStatus")
                            .SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetMessageValue(newScreen.ToByteString()));
                        channelState.AddAttribute(attr);
                    }
                    else
                    {
                        var oldScreen = D3.PartyMessage.ScreenStatus.ParseFrom(attribute.Value.MessageValue);
                        this.Client.Account.ScreenStatus = oldScreen;

                        // TODO: save screen status for use with friends -Egris
                        var attr = bnet.protocol.attribute.Attribute.CreateBuilder()
                            .SetName("D3.Party.ScreenStatus")
                            .SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetMessageValue(oldScreen.ToByteString()));
                        channelState.AddAttribute(attr);
                        Logger.Debug("Client moving to Screen: {0}, with Status: {1}", oldScreen.Screen, oldScreen.Status);
                    }
                }
                else if (attribute.Name == "D3.Party.JoinPermissionPreviousToLock")
                {
                    // 0 - CLOSED
                    // 1 - ASK_TO_JOIN

                    var joinPermission = attribute.Value;
                    var attr = bnet.protocol.attribute.Attribute.CreateBuilder()
                        .SetName("D3.Party.JoinPermissionPreviousToLock")
                        .SetValue(joinPermission);
                    channelState.AddAttribute(attr);
                }
                else if (attribute.Name == "D3.Party.LockReasons")
                {
                    // 0 - CREATING_GAME
                    // 2 - MATCHMAKER_SEARCHING

                    var lockReason = attribute.Value;
                    var attr = bnet.protocol.attribute.Attribute.CreateBuilder()
                        .SetName("D3.Party.LockReasons")
                        .SetValue(lockReason);
                    channelState.AddAttribute(attr);
                }
                else if (attribute.Name == "D3.Party.GameId")
                {
                    if (attribute.HasValue && !attribute.Value.MessageValue.IsEmpty) //Sometimes not present -Egris
                    {
                        var gameId = D3.OnlineService.GameId.ParseFrom(attribute.Value.MessageValue);
                        var attr = bnet.protocol.attribute.Attribute.CreateBuilder()
                            .SetName("D3.Party.GameId")
                            .SetValue(bnet.protocol.attribute.Variant.CreateBuilder().SetMessageValue(gameId.ToByteString()).Build());
                        channelState.AddAttribute(attr);
                    }

                }
                else
                {
                    Logger.Warn("UpdateChannelState(): Unknown attribute: {0}", attribute.Name);
                }
            }

            var builder = bnet.protocol.NoData.CreateBuilder();
            done(builder.Build());

            if (request.StateChange.HasPrivacyLevel)
                channelState.PrivacyLevel = request.StateChange.PrivacyLevel;

            var notification = bnet.protocol.channel.UpdateChannelStateNotification.CreateBuilder()
                .SetAgentId(this.Client.CurrentToon.BnetEntityID)
                .SetStateChange(channelState)
                .Build();

            // Send UpdateChannelStateNotification RPC call.
            this.Client.MakeTargetedRPC(this.Client.CurrentChannel, () =>
                bnet.protocol.channel.ChannelSubscriber.CreateStub(this.Client).NotifyUpdateChannelState(null, notification, callback => { }));
        }

        public override void UpdateMemberState(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.channel.UpdateMemberStateRequest request, System.Action<bnet.protocol.NoData> done)
        {
            throw new NotImplementedException();
        }
    }
}
