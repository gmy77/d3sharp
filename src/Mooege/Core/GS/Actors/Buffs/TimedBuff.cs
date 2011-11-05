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

using Mooege.Core.GS.Common.Types;

namespace Mooege.Core.GS.Actors.Buffs
{
    //bumbasher: base for a time expiration buff
    public abstract class TimedBuff
    {
        public TimedBuff(TickTimer timeout)
        {
            Timeout = timeout;
        }

        public TickTimer Timeout;
        public Actor Target;
        
        // return true if its over
        public bool Update()
        {
            bool timedout = Timeout.TimedOut();
            if (timedout)
                Remove();

            return timedout;
        }

        //apply the effect here
        public abstract void Apply();

        //remove the effect here
        public abstract void Remove();
    }
}
