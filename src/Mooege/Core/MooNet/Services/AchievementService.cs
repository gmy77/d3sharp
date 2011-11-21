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
using Mooege.Common.Logging;
using Mooege.Net.MooNet;

namespace Mooege.Core.MooNet.Services
{
    [Service(serviceID: 0x20, serviceName: "bnet.protocol.achievements.AchievementsService")]
    public class AchievementService : bnet.protocol.achievements.AchievementsService, IServerService
    {
        private static readonly Logger Logger = LogManager.CreateLogger();
        public MooNetClient Client { get; set; }
        public bnet.protocol.Header LastCallHeader { get; set; }

        public override void PostUpdate(IRpcController controller, bnet.protocol.achievements.PostUpdateRequest request, Action<bnet.protocol.achievements.PostUpdateResponse> done)
        {
            throw new NotImplementedException();
        }

        public override void RegisterWithService(IRpcController controller, bnet.protocol.achievements.RegisterWithServiceRequest request, Action<bnet.protocol.achievements.RegisterWithServiceResponse> done)
        {
            throw new NotImplementedException();
        }

        public override void RequestSnapshot(IRpcController controller, bnet.protocol.achievements.RequestSnapshotRequest request, Action<bnet.protocol.achievements.RequestSnapshotResponse> done)
        {
            throw new NotImplementedException();
        }

        public override void UnregisterFromService(IRpcController controller, bnet.protocol.achievements.UnregisterFromServiceRequest request, Action<bnet.protocol.NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void Initialize(IRpcController controller, bnet.protocol.achievements.InitializeRequest request, Action<bnet.protocol.achievements.InitializeResponse> done)
        {
            throw new NotImplementedException();
        }
    }
}
