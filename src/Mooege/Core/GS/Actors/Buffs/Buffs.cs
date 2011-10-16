using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mooege.Common.Helpers;
using Mooege.Core.GS.Map;
using Mooege.Net.GS.Message;
using Mooege.Net.GS.Message.Definitions.Tick;
using Mooege.Net.GS.Message.Definitions.World;
using Mooege.Net.GS.Message.Fields;
using Mooege.Net.GS.Message.Definitions.Animation;
using Mooege.Net.GS.Message.Definitions.Effect;
using Mooege.Net.GS.Message.Definitions.Misc;

namespace Mooege.Core.GS.Actors.Buffs
{
    //Mob appears as frozen (animation freezes too), be sure to remove this before playing die animation
    public class FreezeBuff : BooleanPropertyBuff
    {
        public FreezeBuff(float duration, Actor target)
            : base(duration, target, GameAttribute.Frozen, FloatingNumberMessage.FloatType.Frozen)
        {
        }
    }


    //appears to be some kind of arcane slow
    public class SlowBuff : BooleanPropertyBuff
    {
        public SlowBuff(float duration, float amount, Actor target)
            : base(duration, target, GameAttribute.Slow, null)
        {
            Amount = amount;
        }

        public float Amount = 1;
    }


    //stars floating around monster head as well as stun animation
    public class StunBuff : BooleanPropertyBuff
    {
        public StunBuff(float duration, Actor target)
            : base(duration, target, GameAttribute.Stunned, null)
        {
        }
    }


    //monster appears to be chilled: blue hue
    public class ChilledBuff : BooleanPropertyBuff
    {
        public ChilledBuff(float duration, Actor target)
            : base(duration, target, GameAttribute.Chilled, null)
        {
        }
    }
}
