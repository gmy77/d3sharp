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
using Mooege.Net.GS.Message.Definitions.Animation;
using Mooege.Net.GS.Message;
using Mooege.Core.GS.Ticker;

namespace Mooege.Core.GS.Actors.Implementations
{
    class ProximityTriggeredGizmo : Gizmo
    {
        private bool _collapsed = false;

        public ProximityTriggeredGizmo(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
        }

        public override void OnPlayerApproaching(Players.Player player)
        {
            if (player.Position.DistanceSquared(ref _position) < ActorData.Sphere.Radius * ActorData.Sphere.Radius * this.Scale * this.Scale && !_collapsed)
            {
                _collapsed = true;

                // TODO most of the fields here are unknown, find out about animation playing duration
                int duration = 500; // ticks
                World.BroadcastIfRevealed(new PlayAnimationMessage
                {
                    ActorID = this.DynamicID,
                    Field1 = 11,
                    Field2 = 0,
                    tAnim = new Net.GS.Message.Fields.PlayAnimationMessageSpec[]
                    {
                        new Net.GS.Message.Fields.PlayAnimationMessageSpec()
                        {
                            Duration = duration,
                            AnimationSNO = ActorData.TagMap.ContainsKey(ActorKeys.DeathAnimationTag) ? AnimationSet.TagMapAnimDefault[ActorData.TagMap[ActorKeys.DeathAnimationTag]].Int : AnimationSet.TagMapAnimDefault[AnimationSetKeys.DeathDefault] ,
                            PermutationIndex = 0,
                            Speed = 1
                        }
                    }

                }, this);

                World.BroadcastIfRevealed(new SetIdleAnimationMessage
                {
                    ActorID = this.DynamicID,
                    AnimationSNO = AnimationSetKeys.DeadDefault.ID
                }, this);

                this.Attributes[GameAttribute.Deleted_On_Server] = true;
                Attributes.BroadcastChangedIfRevealed();

                RelativeTickTimer destroy = new RelativeTickTimer(World.Game, duration, x => this.Destroy());
            }
        }

    }
}
