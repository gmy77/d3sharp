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
using D3Sharp.Net;
using D3Sharp.Utils;
using Google.ProtocolBuffers;
using bnet.protocol.user_manager;

namespace D3Sharp.Core.Services
{
    [Service(serviceID: 0x5, serviceName: "bnet.protocol.user_manager.UserManagerService")]
    public class UserManagerService : bnet.protocol.user_manager.UserManagerService,IServerService
    {
        protected static readonly Logger Logger = LogManager.CreateLogger();
        public IClient Client { get; set; }

        public override void SubscribeToUserManager(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.user_manager.SubscribeToUserManagerRequest request, System.Action<bnet.protocol.user_manager.SubscribeToUserManagerResponse> done)
        {
            // TODO: Sending this packet crashes my client. This may be a local issue as I haven't heard anyone else mention it.  -Ethos
            // Note that the request has an ObjectId, but it is never referenced later in the
            // capture.. suggesting that this doesn't create a specific object, or is maybe mapped
            // to a static identifier for handling future requsts by the client to this service
            var builder = bnet.protocol.user_manager.SubscribeToUserManagerResponse.CreateBuilder();
            done(builder.Build());
        }

        public override void ReportPlayer(IRpcController controller, ReportPlayerRequest request, Action<ReportPlayerResponse> done)
        {
            throw new NotImplementedException();
        }

        public override void BlockPlayer(IRpcController controller, BlockPlayerRequest request, Action<BlockPlayerResponse> done)
        {
            throw new NotImplementedException();
        }

        public override void RemovePlayerBlock(IRpcController controller, RemovePlayerBlockRequest request, Action<RemovePlayerBlockResponse> done)
        {
            throw new NotImplementedException();
        }

        public override void AddRecentPlayers(IRpcController controller, AddRecentPlayersRequest request, Action<AddRecentPlayersResponse> done)
        {
            throw new NotImplementedException();
        }

        public override void RemoveRecentPlayers(IRpcController controller, RemoveRecentPlayersRequest request, Action<RemoveRecentPlayersResponse> done)
        {
            throw new NotImplementedException();
        }
    }
}
