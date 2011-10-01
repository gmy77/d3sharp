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
using bnet.protocol.user_manager;

namespace Extractor.Services
{
    [Service(serviceID: 0x5, serviceName: "bnet.protocol.user_manager.UserManagerService")]
    public class UserManagerService : bnet.protocol.user_manager.UserManagerService
    {
        public override void SubscribeToUserManager(IRpcController controller, SubscribeToUserManagerRequest request, Action<SubscribeToUserManagerResponse> done)
        {
            ProtoOutputBuffer.Write(request.GetType(), request.ToString());
        }

        public override void ReportPlayer(IRpcController controller, ReportPlayerRequest request, Action<ReportPlayerResponse> done)
        {
            ProtoOutputBuffer.Write(request.GetType(), request.ToString());
        }

        public override void BlockPlayer(IRpcController controller, BlockPlayerRequest request, Action<BlockPlayerResponse> done)
        {
            ProtoOutputBuffer.Write(request.GetType(), request.ToString());
        }

        public override void RemovePlayerBlock(IRpcController controller, RemovePlayerBlockRequest request, Action<RemovePlayerBlockResponse> done)
        {
            ProtoOutputBuffer.Write(request.GetType(), request.ToString());
        }

        public override void AddRecentPlayers(IRpcController controller, AddRecentPlayersRequest request, Action<AddRecentPlayersResponse> done)
        {
            ProtoOutputBuffer.Write(request.GetType(), request.ToString());
        }

        public override void RemoveRecentPlayers(IRpcController controller, RemoveRecentPlayersRequest request, Action<RemoveRecentPlayersResponse> done)
        {
            ProtoOutputBuffer.Write(request.GetType(), request.ToString());
        }
    }
}
