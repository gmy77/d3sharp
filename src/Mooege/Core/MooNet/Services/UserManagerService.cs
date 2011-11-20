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
using Mooege.Common;
using Mooege.Core.MooNet.Online;
using Mooege.Net.MooNet;
using bnet.protocol;
using bnet.protocol.user_manager;

namespace Mooege.Core.MooNet.Services
{
    [Service(serviceID: 0x5, serviceName: "bnet.protocol.user_manager.UserManagerService")]
    public class UserManagerService : bnet.protocol.user_manager.UserManagerService,IServerService
    {
        private static readonly Logger Logger = LogManager.CreateLogger();
        public MooNetClient Client { get; set; }

        public override void SubscribeToUserManager(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.user_manager.SubscribeToUserManagerRequest request, System.Action<bnet.protocol.user_manager.SubscribeToUserManagerResponse> done)
        {
            Logger.Trace("Subscribe() {0}", this.Client);

            // temp hack: send him all online players on server where he should be normally get list of player he met in his last few games /raist.

            var builder = SubscribeToUserManagerResponse.CreateBuilder();
            foreach (var client in PlayerManager.OnlinePlayers)
            {
                if (client == this.Client) continue; // Don't add the requester to the list                
                if (client.CurrentToon == null) continue;

                Logger.Debug("RecentPlayer => " + client.CurrentToon);
                var recentPlayer = RecentPlayer.CreateBuilder().SetPlayer(client.CurrentToon.BnetEntityID);
                builder.AddRecentPlayers(recentPlayer);
            }

            done(builder.Build());
        }

        public override void AddRecentPlayers(IRpcController controller, AddRecentPlayersRequest request, Action<AddRecentPlayersResponse> done)
        {
            throw new NotImplementedException();
        }

        public override void RemoveRecentPlayers(IRpcController controller, RemoveRecentPlayersRequest request, Action<RemoveRecentPlayersResponse> done)
        {
            throw new NotImplementedException();
        }

        public override void ReportToon(IRpcController controller, ReportToonRequest request, Action<ReportToonResponse> done)
        {
            throw new NotImplementedException();
        }

        public override void BlockToon(IRpcController controller, BlockToonRequest request, Action<NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void UnblockToons(IRpcController controller, UnblockToonsRequest request, Action<UnblockToonsResponse> done)
        {
            throw new NotImplementedException();
        }

        public override void ReportAccount(IRpcController controller, ReportAccountRequest request, Action<ReportAccountResponse> done)
        {
            throw new NotImplementedException();
        }

        public override void IgnoreInviter(IRpcController controller, IgnoreInviterRequest request, Action<NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void UnignoreInviters(IRpcController controller, UnignoreInvitersRequest request, Action<UnignoreInvitersResponse> done)
        {
            throw new NotImplementedException();
        }
    }
}
