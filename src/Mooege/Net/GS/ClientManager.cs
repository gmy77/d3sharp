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
using Mooege.Common;
using Mooege.Core.GS.Game;

namespace Mooege.Net.GS
{
    public class ClientManager
    {
        protected static readonly Logger Logger = LogManager.CreateLogger();

        public static List<Game> Games = new List<Game>();

        public static void OnConnect(object sender, ConnectionEventArgs e)
        {
            Logger.Trace("Game-Client connected: {0}", e.Connection.ToString());

            // atm, just creating a game - though clients should be able to join existing ones.
            var game = new Game();
            Games.Add(game);

            var gameClient = new GameClient(e.Connection, game);
            gameClient.Game = game;
            e.Connection.Client = gameClient;
        }

        public static void OnDisconnect(object sender, ConnectionEventArgs e)
        {
            Logger.Trace("Client disconnected: {0}", e.Connection.ToString());
            Games.Remove(((GameClient)e.Connection.Client).Game);
        }
    }
}
