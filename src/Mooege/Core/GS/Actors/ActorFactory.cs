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
using System.Reflection;
using Mooege.Common.MPQ;
using Mooege.Core.GS.Common.Types.SNO;
using Mooege.Core.GS.Map;
using Mooege.Core.GS.Common.Types.TagMap;
using Mooege.Core.GS.Actors.Implementations;
using Mooege.Common.Logging;

namespace Mooege.Core.GS.Actors
{
    public static class ActorFactory
    {
        private static readonly Dictionary<int, Type> SNOHandlers = new Dictionary<int, Type>();
        private static Logger Logger = new Logger("ActorFactory");

        static ActorFactory()
        {
            LoadSNOHandlers();
        }

        public static Actor Create(World world, int snoId, TagMap tags)
        {
            if (!MPQStorage.Data.Assets[SNOGroup.Actor].ContainsKey(snoId))
                return null;

            var actorAsset = MPQStorage.Data.Assets[SNOGroup.Actor][snoId];
            var actorData = actorAsset.Data as Mooege.Common.MPQ.FileFormats.Actor;
            if (actorData == null) return null;

            if (actorData.Type == ActorType.Invalid)
                return null;

            // see if we have an implementation for actor.
            if (SNOHandlers.ContainsKey(snoId))
                return (Actor)Activator.CreateInstance(SNOHandlers[snoId], new object[] { world, snoId, tags });

            switch (actorData.Type)
            {
                case ActorType.Monster:
                    if (tags.ContainsKey(MarkerKeys.ConversationList))
                        return new InteractiveNPC(world, snoId, tags);
                    else
                        if (!MPQStorage.Data.Assets[SNOGroup.Monster].ContainsKey(actorData.MonsterSNO))
                            return null;

                    var monsterAsset = MPQStorage.Data.Assets[SNOGroup.Monster][actorData.MonsterSNO];
                    var monsterData = monsterAsset.Data as Mooege.Common.MPQ.FileFormats.Monster;
                    if (monsterData.Type == Mooege.Common.MPQ.FileFormats.Monster.MonsterType.Ally ||
                        monsterData.Type == Mooege.Common.MPQ.FileFormats.Monster.MonsterType.Helper)
                        return new NPC(world, snoId, tags);
                    else
                        return new Monster(world, snoId, tags);
                case ActorType.Gizmo:
                    switch (actorData.TagMap[ActorKeys.GizmoGroup])
                    {
                        case GizmoGroup.LootContainer:
                            return new LootContainer(world, snoId, tags);
                        case GizmoGroup.Door:
                            return new Door(world, snoId, tags);
                        case GizmoGroup.DestructibleLootContainer:
                        case GizmoGroup.Barricade:
                            return new DesctructibleLootContainer(world, snoId, tags);
                        case GizmoGroup.Portal:
                            //Prevent Development Hell portal from showing
                            if (tags[MarkerKeys.DestinationWorld].Id != 222591)
                                return new Portal(world, snoId, tags);
                            else
                                return null;
                        case GizmoGroup.BossPortal:
                            Logger.Warn("Skipping loading of boss portals");
                            return null;
                        case GizmoGroup.CheckPoint:
                            return new Checkpoint(world, snoId, tags);
                        case GizmoGroup.Waypoint:
                            return new Waypoint(world, snoId, tags);
                        case GizmoGroup.Savepoint:
                            return new Savepoint(world, snoId, tags);
                        case GizmoGroup.ProximityTriggered:
                            return new ProximityTriggeredGizmo(world, snoId, tags);
                        case GizmoGroup.Shrine:
                            return new Shrine(world, snoId, tags);
                        case GizmoGroup.Healthwell:
                            return new Healthwell(world, snoId, tags);
                        case GizmoGroup.StartLocations:
                            return new StartingPoint(world, snoId, tags);

                        case GizmoGroup.ActChangeTempObject:
                        case GizmoGroup.Banner:
                        case GizmoGroup.CathedralIdol:
                        case GizmoGroup.Destructible:
                        case GizmoGroup.DungeonStonePortal:
                        case GizmoGroup.Headstone:
                        case GizmoGroup.HearthPortal:
                        //case GizmoGroup.NephalemAltar:
                        case GizmoGroup.Passive:
                        case GizmoGroup.PlayerSharedStash:
                        case GizmoGroup.QuestLoot:
                        case GizmoGroup.Readable:
                        case GizmoGroup.ServerProp:
                        case GizmoGroup.Sign:
                        case GizmoGroup.Spawner:
                        case GizmoGroup.TownPortal:
                        case GizmoGroup.Trigger:
                        case GizmoGroup.WeirdGroup57:
                            //Logger.Info("GizmoGroup {0} has no proper implementation, using default gizmo instead", actorData.TagMap[ActorKeys.GizmoGroup]);
                            return CreateGizmo(world, snoId, tags);
                        default:
                            Logger.Warn("Unknown gizmo group {0}", actorData.TagMap[ActorKeys.GizmoGroup]);
                            return CreateGizmo(world, snoId, tags);
                    }
                case ActorType.ServerProp:
                    return new ServerProp(world, snoId, tags);
            }
            return null;
        }

        private static Actor CreateGizmo(World world, int snoId, TagMap tags)
        {
            if (tags.ContainsKey(MarkerKeys.DestinationWorld))
            {
                if (tags[MarkerKeys.DestinationWorld].Id != 222591)
                    return new Portal(world, snoId, tags);
            }

            return new Gizmo(world, snoId, tags);
        }

        public static void LoadSNOHandlers()
        {
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (!type.IsSubclassOf(typeof(Actor))) continue;

                var attributes = (HandledSNOAttribute[])type.GetCustomAttributes(typeof(HandledSNOAttribute), true);
                if (attributes.Length == 0) continue;

                foreach (var sno in attributes.First().SNOIds)
                {
                    if (!SNOHandlers.ContainsKey(sno))
                        SNOHandlers.Add(sno, type);
                }
            }
        }
    }
}
