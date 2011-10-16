using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mooege.Core.GS.Actors.Buffs
{
    //bumbasher: base for a time expiration buff
    public class TimedBuff
    {
        //duration in miliseconds
        public TimedBuff(float duration, Actor target)
        {
            ResetDurationTo(duration);
            _target = target;
        }

        DateTime _endTime;
        Actor _target;

        public Actor Target
        {
            get { return _target; }
            set { _target = value; }
        }

        public void ResetDurationTo(float miliseconds)
        {
            _endTime = DateTime.Now.AddMilliseconds(miliseconds);
        }

        public void IncreaseDuration(float miliseconds)
        {
            _endTime.AddMilliseconds(miliseconds);
        }

        //return true if its over
        public bool Update()
        {
            if (DateTime.Now >= _endTime)
            {
                Remove();
                return true;
            }

            return false;
        }

        //apply the effect here
        public virtual void Apply() { }

        //remove the effect here
        public virtual void Remove() { }
    }
}
