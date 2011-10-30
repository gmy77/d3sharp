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
using System.Collections.Generic;
using System.Linq;
using Mooege.Core.Common.Toons;
using Mooege.Core.MooNet.Channels;
using Mooege.Net.MooNet;
using bnet.protocol.game_master;

namespace Mooege.Core.MooNet.Games
{
    public static class GameCreatorManager
    {
        /// <summary>
        /// List of games.
        /// </summary>
        private static readonly Dictionary<ulong, GameCreator> GameCreators =
            new Dictionary<ulong, GameCreator>();

        /// <summary>
        /// Request id counter for find-game requests.
        /// </summary>
        public static ulong RequestIdCounter = 0; // request Id counter for find game responses.

        public static GameCreator CreateGame(Channel channel)
        {
            var game = new GameCreator(channel);
            GameCreators.Add(game.DynamicId, game);
            return game;
        }

        public static void FindGame(MooNetClient client, ulong requestId, FindGameRequest request)
        {
            var clients = new List<MooNetClient>();
            foreach(var player in request.PlayerList)
            {
                var toon = ToonManager.GetToonByLowID(player.ToonId.Low);
                if(toon.Owner.LoggedInClient==null) continue;
                clients.Add(toon.Owner.LoggedInClient);
            }

            string version = null;
            D3.OnlineService.GameCreateParams gameCreateParams = null;
            foreach(bnet.protocol.attribute.Attribute attribute in request.Properties.Filter.AttributeList)
            {
                if(attribute.Name == "GameCreateParams")
                {
                    gameCreateParams = D3.OnlineService.GameCreateParams.ParseFrom(attribute.Value.MessageValue);
                }
                else if(attribute.Name == "version")
                {
                    version = attribute.Value.StringValue;
                }
            }

            List<GameCreator> matchingGames;
            if (!request.Properties.Create && (matchingGames = FindMatchingGames(request)).Count > 0)
            {
                Random rand = new Random();
                var game = matchingGames[rand.Next(matchingGames.Count)];
                game.JoinGame(clients, request.ObjectId);
            }
            else
            {
                client.CurrentChannel.Game.RequestId = requestId;
                client.CurrentChannel.Game.StartGame(clients, request.ObjectId, gameCreateParams, version);
            }
        }

        private static List<GameCreator> FindMatchingGames(FindGameRequest request)
        {
            String version = String.Empty;
            int difficulty = 0;
            int currentQuest = 0;
            foreach (bnet.protocol.attribute.Attribute attribute in request.Properties.Filter.AttributeList)
            {
                switch (attribute.Name)
                {
                    case "version":
                        version = attribute.Value.StringValue;
                        break;
                    case "Game.Difficulty":
                        difficulty = (int)attribute.Value.IntValue;
                        break;
                    case "Game.CurrentQuest":
                        currentQuest = (int)attribute.Value.IntValue;
                        break;
                }
            }

            Func<bool, bool, bool, bool> matchOp;
            switch (request.Properties.Filter.Op)
            {
                case bnet.protocol.attribute.AttributeFilter.Types.Operation.MATCH_ANY:
                    matchOp = (bool b1, bool b2, bool b3) => b1 || b2 || b3;
                    break;
                case bnet.protocol.attribute.AttributeFilter.Types.Operation.MATCH_NONE:
                    matchOp = (bool b1, bool b2, bool b3) => !b1 && !b2 && !b3;
                    break;
                case bnet.protocol.attribute.AttributeFilter.Types.Operation.MATCH_ALL:
                default://default to match all, fall through is on purpose
                    matchOp = (bool b1, bool b2, bool b3) => b1 && b2 && b3;
                    break;
            }

            List<GameCreator> matches = new List<GameCreator>();
            foreach (GameCreator game in GameCreators.Values)
            {   //FIXME: don't currently track max players allowed in a game, hardcoded 4 /dustinconrad
                if (game.InGame != null && !game.GameCreateParams.IsPrivate && game.InGame.Players.Count < 4)
                {
                    if (matchOp(version == game.Version, difficulty == game.GameCreateParams.Coop.DifficultyLevel, currentQuest == game.GameCreateParams.Coop.SnoQuest))
                    {
                        matches.Add(game);
                    }
                }
            }
            return matches;
        }

        //FIXME: MATCH_ALL_MOST_SPECIFIC not implemented /dustinconrad
        public static GameStatsBucket.Builder GetGameStats(GetGameStatsRequest request)
        {
            String version = String.Empty;
            int difficulty = 0;
            int currentQuest = 0;
            foreach (bnet.protocol.attribute.Attribute attribute in request.Filter.AttributeList)
            {
                switch (attribute.Name)
                {
                    case "version":
                        version = attribute.Value.StringValue;
                        break;
                    case "Game.Difficulty":
                        difficulty = (int)attribute.Value.IntValue;
                        break;
                    case "Game.CurrentQuest":
                        currentQuest = (int)attribute.Value.IntValue;
                        break;
                }
            }

            Func<bool, bool, bool, bool> matchOp;
            switch (request.Filter.Op)
            {
                case bnet.protocol.attribute.AttributeFilter.Types.Operation.MATCH_ANY:
                    matchOp = (bool b1, bool b2, bool b3) => b1 || b2 || b3;
                    break;
                case bnet.protocol.attribute.AttributeFilter.Types.Operation.MATCH_NONE:
                    matchOp = (bool b1, bool b2, bool b3) => !b1 && !b2 && !b3;
                    break;
                case bnet.protocol.attribute.AttributeFilter.Types.Operation.MATCH_ALL:
                default://default to match all, fall through is on purpose
                    matchOp = (bool b1, bool b2, bool b3) => b1 && b2 && b3;
                    break;
            }

            uint games = 0;
            int players = 0;
            foreach(GameCreator game in GameCreators.Values)
            {
                if (game.InGame != null && !game.GameCreateParams.IsPrivate)
                {
                    if (matchOp(version == game.Version, difficulty == game.GameCreateParams.Coop.DifficultyLevel, currentQuest == game.GameCreateParams.Coop.SnoQuest))
                    {
                        games++;
                        players += game.InGame.Players.Count;
                    }
                }
            }

            var bucket = GameStatsBucket.CreateBuilder()
                .SetWaitMilliseconds(200)
                .SetActiveGames(games)
                .SetActivePlayers((uint)players)
                .SetFormingGames(0)
                .SetWaitingPlayers(0);
                
            return bucket;
        }
    }
}
