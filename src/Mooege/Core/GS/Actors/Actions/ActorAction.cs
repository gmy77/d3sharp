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

using Mooege.Common.Logging;

namespace Mooege.Core.GS.Actors.Actions
{
    public abstract class ActorAction
    {
        protected static readonly Logger Logger = LogManager.CreateLogger();

        /// <summary>
        /// The action owner actor.
        /// </summary>
        public Actor Owner { get; private set; }

        /// <summary>
        /// Returns true if the action is completed.
        /// </summary>
        public bool Done { get; protected set; }

        /// <summary>
        /// Returns true if the action is already started.
        /// </summary>
        public bool Started { get; protected set; }

        protected ActorAction(Actor owner)
        {
            this.Owner = owner;
            this.Started = false;
            this.Done = false;
        }

        public abstract void Start(int tickCounter);

        public abstract void Update(int tickCounter);

        public abstract void Cancel(int tickCounter);
    }
}
