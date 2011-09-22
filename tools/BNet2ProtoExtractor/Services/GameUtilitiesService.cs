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
using Google.ProtocolBuffers;
using bnet.protocol;
using bnet.protocol.game_utilities;
using bnet.protocol.server_pool;

namespace BNet2ProtoExtractor.Services
{
    [Service(serviceID: 0x8, serviceName: "bnet.protocol.game_utilities.GameUtilities")]
    public class GameUtilitiesService : GameUtilities
    {
        public override void ProcessClientRequest(IRpcController controller, ClientRequest request, Action<ClientResponse> done)
        {
            ProtoOutputBuffer.Write(request.GetType(), request.ToString());
        }

        public override void CreateToon(IRpcController controller, CreateToonRequest request, Action<CreateToonResponse> done)
        {
            ProtoOutputBuffer.Write(request.GetType(), request.ToString());
        }

        public override void DeleteToon(IRpcController controller, DeleteToonRequest request, Action<NoData> done)
        {
            ProtoOutputBuffer.Write(request.GetType(), request.ToString());
        }

        public override void TransferToon(IRpcController controller, TransferToonRequest request, Action<NoData> done)
        {
            ProtoOutputBuffer.Write(request.GetType(), request.ToString());
        }

        public override void SelectToon(IRpcController controller, SelectToonRequest request, Action<NoData> done)
        {
            ProtoOutputBuffer.Write(request.GetType(), request.ToString());
        }

        public override void PresenceChannelCreated(IRpcController controller, PresenceChannelCreatedRequest request, Action<NoData> done)
        {
            ProtoOutputBuffer.Write(request.GetType(), request.ToString());
        }

        public override void GetPlayerVariables(IRpcController controller, PlayerVariablesRequest request, Action<VariablesResponse> done)
        {
            ProtoOutputBuffer.Write(request.GetType(), request.ToString());
        }

        public override void GetGameVariables(IRpcController controller, GameVariablesRequest request, Action<VariablesResponse> done)
        {
            ProtoOutputBuffer.Write(request.GetType(), request.ToString());
        }

        public override void GetLoad(IRpcController controller, GetLoadRequest request, Action<ServerState> done)
        {
            ProtoOutputBuffer.Write(request.GetType(), request.ToString());
        }
    }
}
