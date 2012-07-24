﻿/*
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

using System.Windows;
using Mooege.Common.MPQ;
using Mooege.Core.GS.Common.Types.SNO;
using Mooege.Core.GS.Map;
using Mooege.Core.GS.Players;
using Mooege.Net.GS.Message;
using Mooege.Net.GS.Message.Definitions.Animation;
using Mooege.Net.GS.Message.Definitions.Map;
using Mooege.Net.GS.Message.Definitions.Misc;
using Mooege.Net.GS.Message.Fields;
using Mooege.Core.GS.Common.Types.TagMap;

namespace Mooege.Core.GS.Actors.Implementations
{
    public sealed class Waypoint : Gizmo
    {
        public int WaypointId { get; private set; }

        public Waypoint(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
            this.Attributes[GameAttribute.MinimapActive] = true;
        }

        public override void OnEnter(World world)
        {
            this.ReadWaypointId();
        }

        private void ReadWaypointId()
        {
            var actData = (Mooege.Common.MPQ.FileFormats.Act)MPQStorage.Data.Assets[SNOGroup.Act][70015].Data;
            var wayPointInfo = actData.WayPointInfo;

            var proximity = new Rect(this.Position.X - 1.0, this.Position.Y - 1.0, 2.0, 2.0);
            var scenes = this.World.QuadTree.Query<Scene>(proximity);
            if (scenes.Count == 0) return; // TODO: fixme! /raist

            var scene = scenes[0]; // Parent scene /fasbat

            if (scenes.Count == 2) // What if it's a subscene? /fasbat
            {
                if (scenes[1].ParentChunkID != 0xFFFFFFFF)
                    scene = scenes[1];
            }

            for (int i = 0; i < wayPointInfo.Length; i++)
            {
                if (wayPointInfo[i].SNOLevelArea == -1)
                    continue;

                if (scene.Specification == null) continue;
                foreach (var area in scene.Specification.SNOLevelAreas)
                {
                    if (wayPointInfo[i].SNOWorld != this.World.WorldSNO.Id || wayPointInfo[i].SNOLevelArea != area)
                        continue;

                    this.WaypointId = i;
                    break;
                }
            }
        }

        public override void OnTargeted(Player player, Net.GS.Message.Definitions.World.TargetMessage message)
        {
            var world = player.World;

            world.BroadcastIfRevealed(new PlayAnimationMessage()
            {
                ActorID = this.DynamicID,
                Field1 = 5,
                Field2 = 0f,
                tAnim = new[]
                    {
                        new PlayAnimationMessageSpec()
                        {
                            Duration = 4,
                            AnimationSNO = 0x2f761,
                            PermutationIndex = 0,
                            Speed = 1f,
                        }
                    }
            }, this);

            player.InGameClient.SendMessage(new ANNDataMessage(Opcodes.OpenWaypointSelectionWindowMessage)
            {
                ActorID = this.DynamicID
            });
        }

        public override bool Reveal(Player player)
        {
            if (!base.Reveal(player))
                return false;

            // Show a minimap icon
            player.InGameClient.SendMessage(new MapMarkerInfoMessage()
            {
                Field0 = (int)World.NewActorID,    // TODO What is the correct id space for mapmarkers? /fasbat
                Field1 = new WorldPlace()
                {
                    Position = this.Position,
                    WorldID = this.World.DynamicID
                },
                Field2 = 0x1FA21,
                m_snoStringList = 0xF063,

                Field3 = unchecked((int)0x9799F57B),
                Field9 = 0,
                Field10 = 0,
                Field11 = 0,
                Field5 = 0,
                Field6 = true,
                Field7 = false,
                Field8 = false,
                Field12 = 0
            });

            return true;
        }
    }
}
