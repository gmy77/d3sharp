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

using Mooege.Core.GS.Map;
using Mooege.Core.GS.Common.Types.TagMap;
using Mooege.Net.GS.Message.Definitions.Effect;

namespace Mooege.Core.GS.Actors.Implementations
{
    /// <summary>
    /// Implementation of checkpoints. Do they have any other purpose than sending the checkpoint visual?
    /// </summary>
    class Checkpoint : Gizmo
    {
        private bool _checkpointReached = false;

        public Checkpoint(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {

        }

        public override void OnPlayerApproaching(Players.Player player)
        {
            if (player.Position.DistanceSquared(ref _position) < ActorData.Sphere.Radius * ActorData.Sphere.Radius * this.Scale * this.Scale && !_checkpointReached)
            {
                _checkpointReached = true;

                this.World.BroadcastIfRevealed(new PlayEffectMessage
                {
                    ActorId = player.DynamicID,
                    Effect = Effect.Checkpoint
                }, this);
            }
        }

    }
}
