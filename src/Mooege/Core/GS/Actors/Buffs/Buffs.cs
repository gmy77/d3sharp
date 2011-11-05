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
using Mooege.Core.GS.Players;
using Mooege.Net.GS.Message;
using Mooege.Net.GS.Message.Definitions.Misc;

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

    // whirlwind effect
    public class WhirlWindEffectBuff : TimedBuff
    {
        public WhirlWindEffectBuff(TickTimer timeout)
            : base(timeout)
        {
        }

        // TODO: broadcast attributes to to all clients
        
        public override void Apply()
        {
            GameAttributeMap map = new GameAttributeMap();
            map[GameAttribute.Power_Buff_0_Visual_Effect_None, Skills.Skills.Barbarian.FurySpenders.Whirlwind] = true; // switch on effect
            map.SendMessage(((Player)Target).InGameClient, Target.DynamicID);
        }

        public override void Remove()
        {
            GameAttributeMap map = new GameAttributeMap();
            map[GameAttribute.Power_Buff_0_Visual_Effect_None, Skills.Skills.Barbarian.FurySpenders.Whirlwind] = false; // switch on effect
            map.SendMessage(((Player)Target).InGameClient, Target.DynamicID);
        }
    }
}
