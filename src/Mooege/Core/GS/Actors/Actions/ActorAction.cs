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

using Mooege.Core.GS.AI;

namespace Mooege.Core.GS.Actors.Actions
{
    public abstract class ActorAction
    {
        /// <summary>
        /// Action's owner brain.
        /// </summary>
        public Brain Owner { get; private set; }

        public abstract void Start(int tickCounter);

        public abstract void Update(int tickCounter);

        public abstract void Stop(int tickCounter);
    }
}
