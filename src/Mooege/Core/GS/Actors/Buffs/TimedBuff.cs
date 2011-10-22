using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mooege.Core.GS.Powers;

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
