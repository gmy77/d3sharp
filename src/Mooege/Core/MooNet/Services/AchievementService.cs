/*
 * Copyright (C) 2011 - 2012 mooege project - http://www.mooege.org
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
using Mooege.Common.Versions;
using Mooege.Common.Logging;
using Mooege.Net.MooNet;
using Mooege.Common.Extensions;

namespace Mooege.Core.MooNet.Services
{
    [Service(serviceID: 0x20, serviceName: "bnet.protocol.achievements.AchievementsService")]
    public class AchievementService : bnet.protocol.achievements.AchievementsService, IServerService
    {
        private static readonly Logger Logger = LogManager.CreateLogger();
        public MooNetClient Client { get; set; }
        public bnet.protocol.Header LastCallHeader { get; set; }
        public uint Status { get; set; }

        public override void PostUpdate(IRpcController controller, bnet.protocol.achievements.PostUpdateRequest request, Action<bnet.protocol.achievements.PostUpdateResponse> done)
        {
            throw new NotImplementedException();
        }

        public override void RegisterWithService(IRpcController controller, bnet.protocol.achievements.RegisterWithServiceRequest request, Action<bnet.protocol.achievements.RegisterWithServiceResponse> done)
        {
            // This should register client with achievement notifier service. -Egris
            Logger.Trace("Register()");

            var snapshot = bnet.protocol.achievements.Snapshot.CreateBuilder();

            foreach (var achievement in this.Client.Account.CurrentGameAccount.Achievements)
                snapshot.AddAchievementSnapshot(achievement);

            foreach (var criteria in this.Client.Account.CurrentGameAccount.AchievementCriteria)
                snapshot.AddCriteriaSnapshot(criteria);

            var response = bnet.protocol.achievements.RegisterWithServiceResponse.CreateBuilder()
                .SetRegistrationFlags(3)
                .SetSnapshot(snapshot);

            done(response.Build());
        }

        public override void RequestSnapshot(IRpcController controller, bnet.protocol.achievements.RequestSnapshotRequest request, Action<bnet.protocol.achievements.RequestSnapshotResponse> done)
        {
            Logger.Trace("RequestSnapshot()");

            var snapshot = bnet.protocol.achievements.Snapshot.CreateBuilder();

            foreach (var achievement in this.Client.Account.CurrentGameAccount.Achievements)
                snapshot.AddAchievementSnapshot(achievement);

            foreach (var criteria in this.Client.Account.CurrentGameAccount.AchievementCriteria)
                snapshot.AddCriteriaSnapshot(criteria);

            var response = bnet.protocol.achievements.RequestSnapshotResponse.CreateBuilder().SetSnapshot(snapshot);
            done(response.Build());
        }

        public override void UnregisterFromService(IRpcController controller, bnet.protocol.achievements.UnregisterFromServiceRequest request, Action<bnet.protocol.NoData> done)
        {
            Logger.Trace("Unregister()");

            var builder = bnet.protocol.NoData.CreateBuilder();

            done(builder.Build());
        }

        public override void Initialize(IRpcController controller, bnet.protocol.achievements.InitializeRequest request, Action<bnet.protocol.achievements.InitializeResponse> done)
        {
            Logger.Trace("Initialize()");

            var contentHandle = bnet.protocol.ContentHandle.CreateBuilder()
                .SetRegion(VersionInfo.MooNet.Regions[VersionInfo.MooNet.Region])
                .SetUsage(0x61636875) //achu
                .SetHash(ByteString.CopyFrom(VersionInfo.MooNet.Achievements.AchievementFileHash.ToByteArray()));
            var reponse = bnet.protocol.achievements.InitializeResponse.CreateBuilder().SetContentHandle(contentHandle)
                .SetMaxRecordsPerUpdate(1)
                .SetMaxCriteriaPerRecord(2)
                .SetMaxAchievementsPerRecord(1)
                .SetMaxRegistrations(512)
                .SetFlushFrequency(1);

            done(reponse.Build());
        }

        public override void ValidateStaticData(IRpcController controller, bnet.protocol.achievements.ValidateStaticDataRequest request, Action<bnet.protocol.NoData> done)
        {
            throw new NotImplementedException();
        }
    }
}
