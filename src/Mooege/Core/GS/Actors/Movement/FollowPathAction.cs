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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mooege.Core.GS.Actors.Actions;
using Mooege.Core.GS.Common.Types.Math;
using Mooege.Core.GS.Ticker;

namespace Mooege.Core.GS.Actors.Movement
{
    public class FollowPathAction : ActorAction
    {
        public Vector3D Heading { get; private set; }

        public SteppedRelativeTickTimer Timer;
        List<Vector3D> Path;
        public FollowPathAction(Actor owner, List<Vector3D> Path)
            : base(owner)
        {
            this.Path = Path;
            this.Heading = Path.Last();
        }

        public override void Start(int tickCounter)
        {
            // Each path step will be 2.5f apart roughly, not sure on the math to get correct walk speed for the timer.
            // mobs sometimes skip a bit, not sure why :( - DarkLotus
            var distance = MovementHelpers.GetDistance(this.Owner.Position, this.Heading);
            var facingAngle = MovementHelpers.GetFacingAngle(this.Owner, this.Heading);
            
            if (distance < 1f)
            { Path.Clear(); this.Done = true; return; }
            this.Timer = new SteppedRelativeTickTimer(this.Owner.World.Game, 6, (int)(distance / this.Owner.WalkSpeed),
            (tick) =>
            {
                //this.Owner.Position = MovementHelpers.GetMovementPosition(this.Owner.Position, this.Owner.WalkSpeed, facingAngle, 6);
                if (Path.Count >= 1)
                {
                    this.Owner.Move(this.Path.First(), MovementHelpers.GetFacingAngle(this.Owner, this.Path.First()));
                    this.Owner.Position = Path.First();
                    Path.RemoveAt(0);
                    //Logger.Trace("Step left in Queue: " + Path.Count);
                }
                else { //Logger.Trace("Ticking with no path steps left"); 
                        this.Done = true; }

            },
            (tick) =>
            {
                this.Owner.Position = Heading;
                //Logger.Trace("Completed! Path contains :" + this.Path.Count);
                this.Done = true;
            });

            this.Started = true;
        }

        public override void Update(int tickCounter)
        {
            this.Timer.Update(tickCounter);
        }

        public override void Cancel(int tickCounter)
        {
            this.Done = true;
        }
    }
}
