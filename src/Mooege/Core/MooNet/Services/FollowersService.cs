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
using Mooege.Common.Logging;
using Mooege.Net.MooNet;

namespace Mooege.Core.MooNet.Services
{
    [Service(serviceID: 0x4, serviceName: "bnet.protocol.followers.FollowersService")]
    public class FollowersService : bnet.protocol.followers.FollowersService,IServerService
    {
        private static readonly Logger Logger = LogManager.CreateLogger();
        public MooNetClient Client { get; set; }
        public bnet.protocol.Header LastCallHeader { get; set; }

        public override void SubscribeToFollowers(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.followers.SubscribeToFollowersRequest request, System.Action<bnet.protocol.followers.SubscribeToFollowersResponse> done)
        {
            Logger.Trace("Subscribe() {0}", this.Client);

            var builder = bnet.protocol.followers.SubscribeToFollowersResponse.CreateBuilder();
            done(builder.Build());
        }

        public override void StartFollowing(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.followers.StartFollowingRequest request, System.Action<bnet.protocol.followers.StartFollowingResponse> done)
        {
            throw new NotImplementedException();
        }

        public override void StopFollowing(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.followers.StopFollowingRequest request, System.Action<bnet.protocol.followers.StopFollowingResponse> done)
        {
            throw new NotImplementedException();
        }

        public override void UpdateFollowerState(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.followers.UpdateFollowerStateRequest request, System.Action<bnet.protocol.followers.UpdateFollowerStateResponse> done)
        {
            throw new NotImplementedException();
        }
    }
}
