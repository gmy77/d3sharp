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

using System.Collections.Generic;
using Mooege.Core.Common.Toons;
using Mooege.Core.MooNet.Channels;
using Mooege.Net.MooNet;
using bnet.protocol.game_master;

namespace Mooege.Core.MooNet.Games
{
    public static class GameManager
    {
        /// <summary>
        /// List of games.
        /// </summary>
        public static readonly Dictionary<ulong, Game> AvailableGames =
            new Dictionary<ulong, Game>();

        /// <summary>
        /// Request id counter for find-game requests.
        /// </summary>
        public static ulong RequestIdCounter = 0; // request Id counter for find game responses.

        public static Game CreateGame(Channel channel)
        {
            var game = new Game(channel);
            AvailableGames.Add(game.DynamicId, game);
            return game;
        }

        public static void FindGame(MooNetClient client, ulong requestId, FindGameRequest request)
        {
            // We actually need to check request here and see if client wants to join a public game or create his own.

            var clients = new List<MooNetClient>();
            foreach(var player in request.PlayerList)
            {
                var toon = ToonManager.GetToonByLowID(player.ToonId.Low);
                if(toon.Owner.LoggedInClient==null) continue;
                clients.Add(toon.Owner.LoggedInClient);
            }

            client.CurrentChannel.Game.RequestId = requestId;
            client.CurrentChannel.Game.StartGame(clients, request.ObjectId);            
        }
    }
}
