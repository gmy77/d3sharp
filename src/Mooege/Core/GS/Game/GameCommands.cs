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
using System.Linq;
using Mooege.Common.Helpers;
using Mooege.Core.MooNet.Commands;
using Mooege.Net.GS.Message.Fields;
using Mooege.Net.MooNet;

namespace Mooege.Core.GS.Game
{
    [Command("Spawn")]
    public class SpawnCommand : Command
    {
        public override string Help()
        {
            return "usage: spawn <amount> <actorSNO>";
        }

        public override string Invoke(string parameters, MooNetClient invokerClient = null)
        {
            if (invokerClient == null)
                return "You can not invoked this command from console.";
            
            if (invokerClient.InGameClient == null) 
                return "You can only invoke this command while ingame.";

            var player = invokerClient.InGameClient.Player;
            var actorSNO = 6652; /* zombie */
            var amount = 1;

            var @params = parameters.Split(' ');
            if(@params.Count()<1)
                return "Invalid arguments. Type 'help spawn' to get help.";

            if (!Int32.TryParse(@params[0], out amount))
                amount = 1;

            if (amount > 100) amount = 100;
            
            if(@params.Count()>1)
                if (!Int32.TryParse(@params[1], out actorSNO))
                    actorSNO = 6652;

            for(int i=0;i<amount;i++)
            {
                var position = new Vector3D(player.Position.X + (float) RandomHelper.NextDouble()*20f,
                                            player.Position.Y + (float) RandomHelper.NextDouble()*20f, 
                                            player.Position.Z);

                player.World.SpawnMob(player, actorSNO, position);
            }

            return string.Format("Spawned {0} mobs with ActorSNO: {1}", amount, actorSNO);
        }
    }
}

