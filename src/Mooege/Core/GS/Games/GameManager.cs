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
using System.Collections.Generic;
using Mooege.Common.Extensions;
using Mooege.Common.Logging;
using Mooege.Core.GS.Players;
using Mooege.Core.MooNet.Toons;
using Mooege.Net.GS.Message;

namespace Mooege.Core.GS.Games
{
    public static class GameManager
    {
        private static readonly Logger Logger = LogManager.CreateLogger();
        private static readonly Dictionary<int, Game> Games = new Dictionary<int, Game>();

        public static Game CreateGame(int gameId)
        {
            if (Games.ContainsKey(gameId))
                return Games[gameId];

            var game = new Game(gameId);
            Games.Add(gameId, game);
            return game;
        }

        public static Game GetGameById(int gameId)
        {
            return !Games.ContainsKey(gameId) ? null : Games[gameId];
        }

        public static void RemovePlayerFromGame(Net.GS.GameClient gameClient)
        {
            if (gameClient == null || gameClient.Game == null) return;

            var gameId = gameClient.Game.GameId;
            if (!Games.ContainsKey(gameId)) return;

            var game = Games[gameId];
            if (!game.Players.ContainsKey(gameClient)) return;

            Player p = null;
            if (!game.Players.TryRemove(gameClient, out p))
            {
                Logger.Error("Can't remove player ({0}) from game with id: {1}", gameClient.Player.Toon.Name, gameId);
            }

            if (p != null)
            {
                //TODO: Move this inside player OnLeave event
                var toon = p.Toon;
                toon.TimePlayed += DateTimeExtensions.ToUnixTime(DateTime.UtcNow) - toon.LoginTime;
                toon.ExperienceNext = p.Attributes[GameAttribute.Experience_Next];

                // Remove Player From World
                if (p.InGameClient != null)
                    p.World.Leave(p);

                // Generate Update for Client
                gameClient.BnetClient.Account.CurrentGameAccount.NotifyUpdate();
                //save hero to db after player data was updated in toon
                ToonManager.SaveToDB(toon);
            }

            if (game.Players.Count == 0)
            {
                Games.Remove(gameId); // we should be also disposing it /raist.
            }
        }
    }
}
