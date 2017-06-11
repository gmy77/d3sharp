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
using Mooege.Common.Logging;
using Mooege.Core.MooNet.Online;
using Mooege.Net.MooNet;

namespace Mooege.Core.MooNet.Services
{
    [Service(serviceID: 0x5, serviceName: "bnet.protocol.user_manager.UserManagerService")]
    public class UserManagerService : bnet.protocol.user_manager.UserManagerService, IServerService
    {
        private static readonly Logger Logger = LogManager.CreateLogger();
        public MooNetClient Client { get; set; }
        public bnet.protocol.Header LastCallHeader { get; set; }
        public uint Status { get; set; }

        public override void SubscribeToUserManager(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.user_manager.SubscribeToUserManagerRequest request, System.Action<bnet.protocol.user_manager.SubscribeToUserManagerResponse> done)
        {
            Logger.Trace("Subscribe() {0}", this.Client);

            // temp hack: send him all online players on server where he should be normally get list of player he met in his last few games /raist.

            var builder = bnet.protocol.user_manager.SubscribeToUserManagerResponse.CreateBuilder();
            uint i = 0;
            foreach (var client in PlayerManager.OnlinePlayers)
            {
                if (client == this.Client) continue; // Don't add the requester to the list                
                if (client.Account.CurrentGameAccount.CurrentToon == null) continue;

                Logger.Debug("RecentPlayer => " + client.Account.CurrentGameAccount.CurrentToon);
                var recentPlayer = bnet.protocol.user_manager.RecentPlayer.CreateBuilder()
                    .SetEntity(client.Account.BnetEntityId)
                    .SetProgramId("D3")
                    .AddAttributes(bnet.protocol.attribute.Attribute.CreateBuilder()
                        .SetName("GameAccountEntityId")
                        .SetValue(bnet.protocol.attribute.Variant.CreateBuilder()
                            .SetMessageValue(client.Account.CurrentGameAccount.D3GameAccountId.ToByteString())
                            .Build())
                        .Build())
                    .SetId(i++)
                    .Build();
                builder.AddRecentPlayers(recentPlayer);
            }

            done(builder.Build());
        }

        public override void AddRecentPlayers(IRpcController controller, bnet.protocol.user_manager.AddRecentPlayersRequest request, Action<bnet.protocol.user_manager.AddRecentPlayersResponse> done)
        {
            throw new NotImplementedException();
        }

        public override void ClearRecentPlayers(IRpcController controller, bnet.protocol.NoData request, Action<bnet.protocol.user_manager.ClearRecentPlayersResponse> done)
        {
            throw new NotImplementedException();
        }

        public override void BlockEntity(IRpcController controller, bnet.protocol.user_manager.BlockEntityRequest request, Action<bnet.protocol.NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void UnblockEntity(IRpcController controller, bnet.protocol.user_manager.UnblockEntityRequest request, Action<bnet.protocol.NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void BlockEntityForSession(IRpcController controller, bnet.protocol.user_manager.BlockEntityRequest request, Action<bnet.protocol.NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void LoadBlockList(IRpcController controller, bnet.protocol.EntityId request, Action<bnet.protocol.NoData> done)
        {
            throw new NotImplementedException();
        }
    }
}
