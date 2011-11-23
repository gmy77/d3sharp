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
using Mooege.Core.MooNet.Online;
using Mooege.Net.MooNet;

namespace Mooege.Core.MooNet.Services
{
    [Service(serviceID: 0x5, serviceName: "bnet.protocol.user_manager.UserManagerService")]
    public class UserManagerService : bnet.protocol.user_manager.UserManagerService,IServerService
    {
        private static readonly Logger Logger = LogManager.CreateLogger();
        public MooNetClient Client { get; set; }
        public bnet.protocol.Header LastCallHeader { get; set; }

        public override void SubscribeToUserManager(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.user_manager.SubscribeToUserManagerRequest request, System.Action<bnet.protocol.user_manager.SubscribeToUserManagerResponse> done)
        {
            Logger.Trace("Subscribe() {0}", this.Client);

            // temp hack: send him all online players on server where he should be normally get list of player he met in his last few games /raist.

            var builder = bnet.protocol.user_manager.SubscribeToUserManagerResponse.CreateBuilder();
            foreach (var client in PlayerManager.OnlinePlayers)
            {
                if (client == this.Client) continue; // Don't add the requester to the list                
                if (client.CurrentToon == null) continue;

                Logger.Debug("RecentPlayer => " + client.CurrentToon);
                var recentPlayer = bnet.protocol.user_manager.RecentPlayer.CreateBuilder().SetPlayer(client.CurrentToon.BnetEntityID);
                builder.AddRecentPlayers(recentPlayer);
            }

            done(builder.Build());
        }

        public override void AddRecentPlayers(IRpcController controller, bnet.protocol.user_manager.AddRecentPlayersRequest request, Action<bnet.protocol.user_manager.AddRecentPlayersResponse> done)
        {
            throw new NotImplementedException();
        }

        public override void RemoveRecentPlayers(IRpcController controller, bnet.protocol.user_manager.RemoveRecentPlayersRequest request, Action<bnet.protocol.user_manager.RemoveRecentPlayersResponse> done)
        {
            throw new NotImplementedException();
        }

        public override void ReportToon(IRpcController controller, bnet.protocol.user_manager.ReportToonRequest request, Action<bnet.protocol.user_manager.ReportToonResponse> done)
        {
            throw new NotImplementedException();
        }

        public override void BlockToon(IRpcController controller, bnet.protocol.user_manager.BlockToonRequest request, Action<bnet.protocol.NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void UnblockToons(IRpcController controller, bnet.protocol.user_manager.UnblockToonsRequest request, Action<bnet.protocol.user_manager.UnblockToonsResponse> done)
        {
            throw new NotImplementedException();
        }

        public override void ReportAccount(IRpcController controller, bnet.protocol.user_manager.ReportAccountRequest request, Action<bnet.protocol.user_manager.ReportAccountResponse> done)
        {
            throw new NotImplementedException();
        }

        public override void IgnoreInviter(IRpcController controller, bnet.protocol.user_manager.IgnoreInviterRequest request, Action<bnet.protocol.NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void UnignoreInviters(IRpcController controller, bnet.protocol.user_manager.UnignoreInvitersRequest request, Action<bnet.protocol.user_manager.UnignoreInvitersResponse> done)
        {
            throw new NotImplementedException();
        }
    }
}
