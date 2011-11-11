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

using System.Collections;
using System.Collections.Generic;
using Mooege.Common;
using Mooege.Core.GS.Actors;
using Mooege.Core.GS.Actors.Actions;
using Mooege.Core.GS.Actors.Movement;
using Mooege.Core.GS.Common.Types.Math;
using Mooege.Core.GS.Ticker;

namespace Mooege.Core.GS.AI
{
    public class Brain
    {
        protected static readonly Logger Logger = LogManager.CreateLogger();

        /// <summary>
        /// The body chained to brain.
        /// </summary>
        public Actor Body { get; private set; }

        /// <summary>
        /// The current brain state.
        /// </summary>
        public BrainState State { get; protected set; }

        /// <summary>
        /// Target.
        /// </summary>
        public Actor Target { get; protected set; }

        /// <summary>
        /// Actions to be taken.
        /// </summary>
        public Queue<ActorAction> Actions { get; protected set; }

        protected Brain(Actor body)
        {
            this.Body = body;
            this.State = BrainState.Idle;
            this.Actions = new Queue<ActorAction>();
        }

        protected void QueueAction(ActorAction action)
        {
            this.Actions.Enqueue(action);
        }
       
        public virtual void Update(int tickCounter)
        {
            if(this.State == BrainState.Dead || this.Body == null || this.Body.World == null)
                return;

            this.Think(tickCounter);
        }        

        /// <summary>
        /// Lets the brain think and decide the next action to take.
        /// </summary>
        public virtual void Think(int tickCounter)
        { }

        public void Move(Vector3D position, float facingAngle)
        {
            this.Body.FacingAngle = facingAngle;
            this.Body.Move(position, facingAngle);
        }

        public void Move(Actor actor)
        {
            this.Move(actor.Position, ActorHelpers.GetFacingAngle(this.Body, actor));
            this.Body.Position = actor.Position; // hack /raist.
        }
    }
}
