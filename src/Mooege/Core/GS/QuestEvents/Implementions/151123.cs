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

namespace Mooege.Core.GS.QuestEvents.Implementations
{
    class _151123 : QuestEvent
    {
        //ActorID: 0x7A3100DD  
        //ZombieSkinny_A_LeahInn.acr (2050031837)
        //ActorSNOId: 0x00031971:ZombieSkinny_A_LeahInn.acr

        private static readonly Logger Logger = LogManager.CreateLogger();

        public _151123()
            : base(151123)
        {
        }

        List<Vector3D> ActorsVector3D = new List<Vector3D> { }; //We fill this with the vectors of the actors that transform, so we spwan zombies in the same location.
        List<uint> monstersAlive = new List<uint> { }; //We use this for the killeventlistener.

        public override void Execute(Map.World world)
        {
            StartConversation(world, 204113);

            var transformActors = Task<bool>.Factory.StartNew(() => HoudiniVsZombies(world, 204605));
            transformActors.Wait();
            var zombieWave = Task<bool>.Factory.StartNew(() => LaunchWave(ActorsVector3D, world, 203121));
            zombieWave.Wait();
            var ListenerZombie = Task<bool>.Factory.StartNew(() => OnKillListener(monstersAlive, world));
            ListenerZombie.ContinueWith(delegate //Once killed:
            {
                StartConversation(world, 151156);
                world.Game.Quests.Advance(87700);
            });
        }

        private bool HoudiniVsZombies(Map.World world, Int32 snoId)
        {
            var actorSourcePosition = world.GetActorBySNO(snoId);
            var around = actorSourcePosition.GetActorsInRange(10f);

            foreach (var player in world.Players)
            {
                foreach (var actors in around)
                {
                    ActorsVector3D.Add(new Vector3D(actors.Position.X, actors.Position.Y, actors.Position.Z));
                    actors.Destroy();
                }
            }
            return true;
        }

        private bool StartConversation(Map.World world, Int32 conversationId)
        {
            foreach (var player in world.Players)
            {
                player.Value.Conversations.StartConversation(conversationId);
            }
            return true;
        }

        private bool LaunchWave(List<Vector3D> Coordinates, Map.World world, Int32 SnoId)
        {
            var counter = 0;
            var monsterSNOHandle = new Common.Types.SNO.SNOHandle(SnoId);
            var monsterActor = monsterSNOHandle.Target as Actor;

            foreach (Vector3D coords in Coordinates)
            {
                Parallel.ForEach(world.Players, player => //Threading because many spawns at once with out Parallel freezes D3.
                {
                    var PRTransform = new PRTransform()
                    {
                        Quaternion = new Quaternion()
                        {
                            W = 0.590017f,
                            Vector3D = new Vector3D(0, 0, 0)
                        },
                        Vector3D = Coordinates[counter]
                    };

                    //Load the actor here.
                    var actor = WorldGenerator.loadActor(monsterSNOHandle, PRTransform, world, monsterActor.TagMap);
                    monstersAlive.Add(actor);
                    counter++;

                    //If Revealed play animation.
                    world.BroadcastIfRevealed(new PlayAnimationMessage
                    {
                        ActorID = actor,
                        Field1 = 9,
                        Field2 = 0,
                        tAnim = new Net.GS.Message.Fields.PlayAnimationMessageSpec[]
                        {
                            new Net.GS.Message.Fields.PlayAnimationMessageSpec()
                            {
                                Duration = 0x00000048,
                                AnimationSNO = 0x00029A08,
                                PermutationIndex = 0x00000000,
                                Speed = 1f
                            }
                        }
                    }, player.Value);
                });
            }
            return true;
        }

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
    }
}