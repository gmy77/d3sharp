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
using Mooege.Core.GS.Powers;

namespace Mooege.Core.GS.Actors.Buffs
{
    //Mob appears as frozen (animation freezes too), be sure to remove this before playing die animation
    public class FreezeBuff : BooleanAttributeBuff
    {
        public FreezeBuff(TickTimer timeout)
            : base(timeout, GameAttribute.Frozen, FloatingNumberMessage.FloatType.Frozen)
        {
        }
    }


    //appears to be some kind of arcane slow
    public class SlowBuff : BooleanAttributeBuff
    {
        public SlowBuff(TickTimer timeout)
            : base(timeout, GameAttribute.Slow, null)
        {
        }
    }


    //stars floating around monster head as well as stun animation
    public class StunBuff : BooleanAttributeBuff
    {
        public StunBuff(TickTimer timeout)
            : base(timeout, GameAttribute.Stunned, null)
        {
        }
    }


    //monster appears to be chilled: blue hue
    public class ChilledBuff : BooleanAttributeBuff
    {
        public ChilledBuff(TickTimer timeout)
            : base(timeout, GameAttribute.Chilled, null)
        {
        }
    }
}
