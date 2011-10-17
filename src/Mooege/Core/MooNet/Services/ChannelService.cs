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

            if (!request.HasMessage) return; // only continue if the request actually contains a message.

            if (!(request.Message.AttributeCount > 0 && request.Message.AttributeList[0].HasValue &&
                CommandManager.TryParse(request.Message.AttributeList[0].Value.StringValue, this.Client, CommandManager.ResponseMediums.Channel))) // try parsing the message as a command   
            {
                this.Client.CurrentChannel.SendMessage(this.Client, request.Message); // if it's not - let channel itself to broadcast message to it's members.  
            }
        }

        public override void SetRoles(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.channel.SetRolesRequest request, System.Action<bnet.protocol.NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void UpdateChannelState(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.channel.UpdateChannelStateRequest request, System.Action<bnet.protocol.NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void UpdateMemberState(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.channel.UpdateMemberStateRequest request, System.Action<bnet.protocol.NoData> done)
        {
            throw new NotImplementedException();
        }
    }
}
