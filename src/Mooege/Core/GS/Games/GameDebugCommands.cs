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
using System.Threading;
using System.Windows.Forms;
using Mooege.Core.GS.Map;
using Mooege.Core.GS.Map.Debug;
using Mooege.Core.GS.Players;
using Mooege.Core.MooNet.Commands;
using Mooege.Net.MooNet;

namespace Mooege.Core.GS.Games
{
    [CommandGroup("Debug", "Ingame debug commands.")]
    public class GameDebugCommands : CommandGroup
    {
        //    [Command("position", "Returns the position of the player\nUsage: debug position [player@email]")]
        //    public string Position(string[] @params, MooNetClient invokerClient)
        //    {
        //        Player player;

        //        if (invokerClient != null && invokerClient.InGameClient != null)
        //            player = invokerClient.InGameClient.Player;
        //        else
        //        {
        //            if (@params.Count() < 1)
        //                return "Invalid arguments. Type 'help debug position' to get help.";

        //            var account = AccountManager.GetAccountByEmail(@params[0]);
        //            if(account==null)
        //                return string.Format("No account with email: {0} exists.", @params[0]);

        //            if(account.LoggedInClient==null || account.LoggedInClient.InGameClient==null)
        //                return string.Format("Account {0} is not in-game.", @params[0]);

        //            player = account.LoggedInClient.InGameClient.Player;
        //        }

        //        return string.Format("Player position: {0}", player.Position);
        //    }

        //    [Command("revealed", "Lists revealed objects to player\nUsage: debug revealed [player@email]")]
        //    public string Revealed(string[] @params, MooNetClient invokerClient)
        //    {
        //        Player player;

        //        if (invokerClient != null && invokerClient.InGameClient != null)
        //            player = invokerClient.InGameClient.Player;
        //        else
        //        {
        //            if (@params.Count() < 1)
        //                return "Invalid arguments. Type 'help debug revealed' to get help.";

        //            var account = AccountManager.GetAccountByEmail(@params[0]);
        //            if(account==null)
        //                return string.Format("No account with email: {0} exists.", @params[0]);

        //            if(account.LoggedInClient==null || account.LoggedInClient.InGameClient==null)
        //                return string.Format("Account {0} is not in-game.", @params[0]);

        //            player = account.LoggedInClient.InGameClient.Player;
        //        }

        //        var output = new StringBuilder("Revealed objects:" + Environment.NewLine);

        //        foreach(var @object in player.RevealedObjects.Values)
        //        {
        //            output.Append(@object + Environment.NewLine);
        //        }

        //        return output.ToString();
        //    }

        //    [Command("inrange", "Lists objects in range of player\nUsage: debug inrange [player@email]")]
        //    public string InRange(string[] @params, MooNetClient invokerClient)
        //    {
        //        Player player;

        //        if (invokerClient != null && invokerClient.InGameClient != null)
        //            player = invokerClient.InGameClient.Player;
        //        else
        //        {
        //            if (@params.Count() < 1)
        //                return "Invalid arguments. Type 'help debug inrange' to get help.";

        //            var account = AccountManager.GetAccountByEmail(@params[0]);
        //            if (account == null)
        //                return string.Format("No account with email: {0} exists.", @params[0]);

        //            if (account.LoggedInClient == null || account.LoggedInClient.InGameClient == null)
        //                return string.Format("Account {0} is not in-game.", @params[0]);

        //            player = account.LoggedInClient.InGameClient.Player;
        //        }

        //        var output = new StringBuilder("Scenes In Range:" + Environment.NewLine);

        //        foreach (var scene in player.GetScenesInRange())
        //        {
        //            output.Append(scene + Environment.NewLine);
        //        }

        //        output.Append("Actors In Range:" + Environment.NewLine);

        //        foreach (var actor in player.GetActorsInRange())
        //        {
        //            output.Append(actor + Environment.NewLine);
        //        }

        //        return output.ToString();
        //    }

        [CommandGroup("draw", "Draws current world's visualization.\nUsage: draw [worldId]")]
        public class DrawCommand : CommandGroup
        {
            [DefaultCommand]
            public string DrawWorld(string[] @params, MooNetClient invokerClient)
            {
                if (invokerClient == null && @params == null)
                    return "Invalid arguments. Type 'help draw' to get help.";

                World world;
                Player player = null;
                int worldId;

                if (@params == null)
                {
                    if (invokerClient.InGameClient == null || invokerClient.InGameClient.Player == null)
                        return "Invalid arguments. Type 'help draw' to get help.";

                    player = invokerClient.InGameClient.Player;
                    world = invokerClient.InGameClient.Player.World;
                    worldId = world.WorldSNO.Id;
                }
                else
                {
                    if (!Int32.TryParse(@params[0], out worldId))
                        worldId = 71150;

                    var game = GameManager.CreateGame(worldId); // hack-hack /raist.
                    world = game.GetWorld(worldId);
                }

                if (world != null)
                {
                    new Thread(c => Application.Run(new WorldVisualizer(world, player))).Start();
                    return string.Format("Done visualizing world {0}.", worldId);
                }

                return string.Format("Invalid world id: {0}.", worldId);
            }
        }
    }
}
