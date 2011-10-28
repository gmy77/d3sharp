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
using Mooege.Net.GS.Message.Definitions.Misc;
using Mooege.Net.GS.Message.Fields;

namespace Mooege.Core.GS.Actors.Implementations
{
    [HandledSNO(6442 /* Waypoint.acr */, 192164 /* Waypoint_OldTristram.acr */)]
    public sealed class Waypoint : Gizmo
    {
        public static Dictionary<int, Vector3D> Waypoints = new Dictionary<int, Vector3D>()
        {
            { 0, new Vector3D { X = 2981.73f, Y = 2835.009f, Z = 24.66344f } }, // New Tristram
            { 1, new Vector3D { X = 1976.71f, Y = 2788.136f, Z = 41.22956f } }, // Old Tristram
            { 2, new Vector3D { X = 1478.028f, Y = 2849.783f, Z = 57.44714f } }, // Cathedral Garden
            { 6, new Vector3D { X = 2161.802f, Y = 1826.882f, Z = 1.864148f } }, // Cementary of the Forsaken 
            { 8, new Vector3D { X = 1263.054f, Y = 827.8673f, Z = 63.05397f } }, // Drowned Temple
            { 11, new Vector3D { X = 2310.5f, Y = 4770f, Z = 1.010971f } } // Highlands Crossing
        };

        public Waypoint(World world, int actorSNO, Vector3D position) : base(world, actorSNO, position)
        { }

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
    }
}
