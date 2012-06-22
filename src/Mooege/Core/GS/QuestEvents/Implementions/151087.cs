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
using Mooege.Core.GS.Common.Types.TagMap;
using Mooege.Core.GS.Actors;


namespace Mooege.Core.GS.QuestEvents.Implementations
{
    class _151087 : QuestEvent
    {

        private static readonly Logger Logger = LogManager.CreateLogger();

        public _151087()
            : base(151087)
        {
        }

        List<uint> monstersAlive = new List<uint> { }; //We use this for the killeventlistener.
        public override void Execute(Map.World world)
        {
            //var WaitConversationTask = Task<bool>.Factory.StartNew(() => WaitConversation(world));
            ////Disable RumFord so he doesn't offer the quest. Somehow, hes supposed to mark it as readed and not offer it while theres no other quest available but he does,
            ////so you can trigger the event multiple times while the event is already running, therefor, we disable his interaction till the event is done.-Wesko

            //setActorOperable(world, 3739, false);
            //WaitConversationTask.ContinueWith(delegate
            //{
            //    //Start the conversation between RumFord & Guard.
            //    StartConversation(world, 198199);
            //    var WaitConversationTask2 = Task<bool>.Factory.StartNew(() => WaitConversation(world));
            //    //After Conversations ends!.
            //    WaitConversationTask2.ContinueWith(delegate
            //    {
            //        var wave1Actors = world.GetActorsInGroup("GizmoGroup1");

            //        foreach (var actor in wave1Actors)
            //        {
            //            actor.Spawn();
            //        }
            //    });
            //});
            ////Run Kill Event Listener
            //var ListenerFirstWaveTask = Task<bool>.Factory.StartNew(() => OnKillListener(world, "GizmoGroup1"));
            //ListenerFirstWaveTask.ContinueWith(delegate //Once killed:
            //{
            //    //Wave three: Skinnies + RumFord conversation #2 "They Keep Comming!".
            //    StartConversation(world, 80088);
            var wave2Actors = world.GetActorsInGroup("GizmoGroup2");
            foreach (var actor in wave2Actors)
            {
                if (actor is Spawner)
                {
                    (actor as Spawner).Spawn();
                }
            }

            //    var ListenerThirdWaveTask = Task<bool>.Factory.StartNew(() => OnKillListener(world, "GizmoGroup2"));
            //    ListenerThirdWaveTask.Wait();
            //    Task.WaitAll();

            //    //Event done we advance the quest and play last conversation #3.
            //    world.Game.Quests.Advance(87700);
            //    Logger.Debug("Event finished");
            //    StartConversation(world, 151102);
            //    setActorOperable(world, 3739, true);
            //});
        }

        //This is the way we Listen for mob killing events.
        private bool OnKillListener(Map.World world, string group)
        {
            while (world.HasActorsInGroup(group))
            {
            }
            return true;
        }

        //HACK!,This is the way we wait if we need to trigger something after a conversation ends.
        private bool _status = false;
        private bool WaitConversation(Map.World world)
        {
            var players = world.Players;
            while (!_status)
            {
                foreach (var player in players)
                {
                    if (player.Value.Conversations.ConversationRunning() == true)
                    {
                        Logger.Debug("Conversation Finished");
                        _status = false;
                        return true;
                    }
                    else
                    {
                        //Logger.Debug("Waiting");
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

        //Not Operable Rumford (To disable giving u the same quest while ur in the event)
        public static bool setActorOperable(Map.World world, Int32 snoId, bool status)
        {
            var actor = world.GetActorBySNO(snoId);
            foreach (var player in world.Players)
            {
                actor.Attributes[Net.GS.Message.GameAttribute.NPC_Is_Operatable] = status;
            }
            return true;
        }

    }
}
