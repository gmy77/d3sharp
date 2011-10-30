﻿/*
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
using Mooege.Core.GS.Common.Types.Math;
using Mooege.Core.GS.Map;
using Mooege.Net.GS.Message;
using Mooege.Net.GS.Message.Definitions.Animation;
using Mooege.Net.GS.Message.Definitions.Map;
using Mooege.Net.GS.Message.Definitions.Misc;
using Mooege.Net.GS.Message.Fields;

namespace Mooege.Core.GS.Actors.Implementations
{
    [HandledSNO(6442 /* Waypoint.acr */, 192164 /* Waypoint_OldTristram.acr */)]
    public sealed class Waypoint : Gizmo
    {
        public static Dictionary<int, Waypoint> Waypoints = new Dictionary<int, Waypoint>();

        public Waypoint(World world, int actorSNO, Vector3D position) : base(world, actorSNO, position)
        {
            this.Attributes[GameAttribute.MinimapActive] = true;
        }

        public override void OnTargeted(Player.Player player, Net.GS.Message.Definitions.World.TargetMessage message)
        {
            var world = player.World;
            player.UpdateHeroState();

            world.BroadcastIfRevealed(new PlayAnimationMessage()
            {
                ActorID = this.DynamicID,
                Field1 = 5,
                Field2 = 0f,
                tAnim = new[]
                    {
                        new PlayAnimationMessageSpec()
                        {
                            Field0 = 4,
                            Field1 = 0x2f761,
                            Field2 = 0,
                            Field3 = 1f,
                        }
                    }
            }, this);

            player.InGameClient.SendMessage(new ANNDataMessage(Opcodes.OpenWaypointSelectionWindowMessage)
            {
                ActorID = this.DynamicID
            });
        }

         public override bool Reveal(Player.Player player)
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
                    WorldID = this._world.DynamicID
                },
                Field2 = 0x1FA21,  
                m_snoStringList = 0xF063,

                Field4 = unchecked((int)0x9799F57B),
                Field5 = 0,
                Field6 = 0,
                Field7 = 0,
                Field8 = 0,
                Field9 = true,
                Field10 = false,
                Field11 = false,
                Field12 = 0
            }); 

            return true;
        }   
    }
}
