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
    [Service(serviceID: 0x8, serviceName: "bnet.protocol.game_utilities.GameUtilities")]
    public class GameUtilitiesService : bnet.protocol.game_utilities.GameUtilities,IServerService
    {
        private static readonly Logger Logger = LogManager.CreateLogger();
        public MooNetClient Client { get; set; }
        public bnet.protocol.Header LastCallHeader { get; set; }

        public override void ProcessClientRequest(IRpcController controller, bnet.protocol.game_utilities.ClientRequest request, Action<bnet.protocol.game_utilities.ClientResponse> done)
        {
            Logger.Trace("ProcessClientRequest()");

            // TODO: handle the request. this is where banner changing happens (CustomMessageId 4)
            // CustomMessage for banner change is a D3.GameMessages.SaveBannerConfiguration
            //Logger.Debug("request:\n{0}", request.ToString());

            var builder = bnet.protocol.game_utilities.ClientResponse.CreateBuilder();
            done(builder.Build());
        }

        public override void CreateToon(IRpcController controller, bnet.protocol.game_utilities.CreateToonRequest request, Action<bnet.protocol.NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void DeleteToon(IRpcController controller, bnet.protocol.game_utilities.DeleteToonRequest request, Action<bnet.protocol.NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void TransferToon(IRpcController controller, bnet.protocol.game_utilities.TransferToonRequest request, Action<bnet.protocol.NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void SelectToon(IRpcController controller, bnet.protocol.game_utilities.SelectToonRequest request, Action<bnet.protocol.NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void PresenceChannelCreated(IRpcController controller, bnet.protocol.game_utilities.PresenceChannelCreatedRequest request, Action<bnet.protocol.NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void GetPlayerVariables(IRpcController controller, bnet.protocol.game_utilities.PlayerVariablesRequest request, Action<bnet.protocol.game_utilities.VariablesResponse> done)
        {
            throw new NotImplementedException();
        }

        public override void GetGameVariables(IRpcController controller, bnet.protocol.game_utilities.GameVariablesRequest request, Action<bnet.protocol.game_utilities.VariablesResponse> done)
        {
            throw new NotImplementedException();
        }

        public override void GetLoad(IRpcController controller, bnet.protocol.server_pool.GetLoadRequest request, Action<bnet.protocol.server_pool.ServerState> done)
        {
            throw new NotImplementedException();
        }

        public override void CreateToonEntity(IRpcController controller, bnet.protocol.toon.CreateToonEntityRequest request, Action<bnet.protocol.toon.CreateToonEntityResponse> done)
        {
            throw new NotImplementedException();
        }

        public override void FinalizeToonCreation(IRpcController controller, bnet.protocol.toon.FinalizeToonCreationRequest request, Action<bnet.protocol.NoData> done)
        {
            throw new NotImplementedException();
        }

        public override void ProcessServerRequest(IRpcController controller, bnet.protocol.game_utilities.ServerRequest request, Action<bnet.protocol.game_utilities.ServerResponse> done)
        {
            throw new NotImplementedException();
        }
    }
}
