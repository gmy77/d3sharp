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
using bnet.protocol;
using bnet.protocol.channel;

namespace Extractor.Services
{
    [Service(serviceID: 0x10, serviceName: "bnet.protocol.channel.Channel")]
    public class ChannelService : bnet.protocol.channel.Channel
    {
        public override void AddMember(IRpcController controller, AddMemberRequest request, Action<NoData> done)
        {
            ProtoOutputBuffer.Write(request.GetType(), request.ToString());
        }

        public override void RemoveMember(IRpcController controller, RemoveMemberRequest request, Action<NoData> done)
        {
            ProtoOutputBuffer.Write(request.GetType(), request.ToString());
        }

        public override void SendMessage(IRpcController controller, SendMessageRequest request, Action<NoData> done)
        {
            ProtoOutputBuffer.Write(request.GetType(), request.ToString());
        }

        public override void UpdateChannelState(IRpcController controller, UpdateChannelStateRequest request, Action<NoData> done)
        {
            ProtoOutputBuffer.Write(request.GetType(), request.ToString());
        }

        public override void UpdateMemberState(IRpcController controller, UpdateMemberStateRequest request, Action<NoData> done)
        {
            ProtoOutputBuffer.Write(request.GetType(), request.ToString());
        }

        public override void Dissolve(IRpcController controller, DissolveRequest request, Action<NoData> done)
        {
            ProtoOutputBuffer.Write(request.GetType(), request.ToString());
        }

        public override void SetRoles(IRpcController controller, SetRolesRequest request, Action<NoData> done)
        {
            ProtoOutputBuffer.Write(request.GetType(), request.ToString());
        }
    }
}
