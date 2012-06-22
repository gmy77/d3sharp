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
using System.Linq;
using System.Text;
using Mooege.Common.MPQ.FileFormats;
using Mooege.Net.GS.Message.Definitions.ACD;
using Mooege.Net.GS.Message.Definitions.Animation;
using Mooege.Core.GS.Common.Types.Math;
using Mooege.Core.GS.Generators;
using Mooege.Common.Logging;
using System.Threading.Tasks;
using System.Threading;

namespace Mooege.Core.GS.QuestEvents.Implementations
{
    class _198503 : QuestEvent
    {

        private static readonly Logger Logger = LogManager.CreateLogger();

        public _198503()
            : base(198503)
        {
        }

        List<uint> monstersAlive = new List<uint> { }; //We use this for the killeventlistener.
        public override void Execute(Map.World world)
        {
            //The spawning positions for each monster in its wave. Basically, you add here the "number" of mobs, accoring to each vector LaunchWave() will spawn every mob in its position.
            //Vector3D[] WretchedMotherSpawn = { new Vector3D(2766.513f, 2913.982f, 24.04533f) };

            //Somehow shes already spawned when the Inn event finishes.. so we search for the ID and add it to the kill event listener.
            var actor = world.GetActorBySNO(219725);
            monstersAlive.Add(actor.DynamicID);

            //Run Kill Event Listener
            var ListenerFirstWaveTask = Task<bool>.Factory.StartNew(() => OnKillListener(monstersAlive, world));
            //Wait for wtretchedmother to be killed.
            ListenerFirstWaveTask.ContinueWith(delegate //Once killed:
            {
                    world.Game.Quests.Advance(87700);
                    Logger.Debug("Event finished");
                    StartConversation(world, 156223);
            });
        }

        //This is the way we Listen for mob killing events.
        private bool OnKillListener(List<uint> monstersAlive, Map.World world)
        {
            Int32 monstersKilled = 0;
            var monsterCount = monstersAlive.Count; //Since we are removing values while iterating, this is set at the first real read of the mob counting.
            while (monstersKilled != monsterCount)
            {
                //Iterate through monstersAlive List, if found dead we start to remove em till all of em are dead and removed.
                for (int i = monstersAlive.Count - 1; i >= 0; i--)
                {
                    if (world.HasMonster(monstersAlive[i]))
                    {
                        //Alive: Nothing.
                    }
                    else
                    {
                        //If dead we remove it from the list and keep iterating.
                        Logger.Debug(monstersAlive[i] + " has been killed");
                        monstersAlive.RemoveAt(i);
                        monstersKilled++;
                    }
                }
            }
            return true;
        }     

        //Launch Conversations.
        private bool StartConversation(Map.World world, Int32 conversationId)
        {
            foreach (var player in world.Players)
            {
                player.Value.Conversations.StartConversation(conversationId);
            }
            return true;
        }
    }
}
