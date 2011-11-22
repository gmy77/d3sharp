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
using Mooege.Core.GS.AI;
using System.Threading.Tasks;

namespace Mooege.Core.GS.Actors.Movement
{
    public class PathfindToPoint : ActorAction
    {
        public Vector3D Heading { get; private set; }

        public SteppedRelativeTickTimer Timer;
        private List<Vector3D> Path = new List<Vector3D>();
        //private Task<List<Vector3D>> task;

        public PathfindToPoint(Actor owner, Vector3D heading)
            : base(owner)
        {
                  //task = Task.Factory.StartNew(() => pather.FindPath(ref Path,owner,owner.Position,heading));
                  //task = Task<List<Vector3D>>.Factory.StartNew(() => pather.FindPath(owner, owner.Position, heading)); 
            owner.World.Game.Pathfinder.GetPath(owner, owner.Position, heading, ref Path);
            this.Heading = heading;

        }

        public override void Start(int tickCounter)
        {
            if (Path.Count == 0)
                return;
            if (Path.Count < 2)
            {
                this.Started = true;
                this.Done = true;
                return;
            }
            // Each path step will be 2.5f apart roughly, not sure on the math to get correct walk speed for the timer.
            // mobs sometimes skip a bit, pretty sure this is because timing isnt correct.  :( - DarkLotus
            
            //var facingAngle = MovementHelpers.GetFacingAngle(this.Owner, this.Heading);
            

            this.Timer = new SteppedRelativeTickTimer(this.Owner.World.Game, 18, (int)(Path.Count *2 / this.Owner.WalkSpeed),
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
                else
                {
                    this.Owner.Position = Heading; //Logger.Trace("Ticking with no path steps left"); 
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
