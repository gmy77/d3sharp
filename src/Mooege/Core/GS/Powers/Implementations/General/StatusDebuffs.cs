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
using Mooege.Core.GS.Ticker;
using Mooege.Net.GS.Message;
using Mooege.Net.GS.Message.Definitions.Misc;
using Mooege.Core.GS.Players;

namespace Mooege.Core.GS.Powers.Implementations
{
    public class SimpleBooleanStatusDebuff : PowerBuff
    {
        GameAttributeB _statusAttribute;
        GameAttributeB _immuneCheckAttribute;
        FloatingNumberMessage.FloatType? _floatMessage;
        bool _immuneBlocked;

        public SimpleBooleanStatusDebuff(GameAttributeB statusAttribute, GameAttributeB immuneCheckAttribute,
            FloatingNumberMessage.FloatType? floatMessage = null)
        {
            _statusAttribute = statusAttribute;
            _immuneCheckAttribute = immuneCheckAttribute;
            _floatMessage = floatMessage;
            _immuneBlocked = false;
        }

        public override void Init()
        {
            if (_immuneCheckAttribute != null)
                _immuneBlocked = Target.Attributes[_immuneCheckAttribute];
        }

        public override bool Apply()
        {
            if (!base.Apply())
                return false;

            if (_immuneBlocked)
                return false;  // TODO: play immune float message?

            Target.Attributes[_statusAttribute] = true;
            Target.Attributes.BroadcastChangedIfRevealed();

            if (_floatMessage != null)
            {
                if (User is Player)
                {
                    (User as Player).InGameClient.SendMessage(new FloatingNumberMessage
                    {
                        ActorID = this.Target.DynamicID,
                        Type = _floatMessage.Value
                    });
                }
            }

            return true;
        }

        public override void Remove()
        {
            base.Remove();
            Target.Attributes[_statusAttribute] = false;
            Target.Attributes.BroadcastChangedIfRevealed();
        }

        public override bool Stack(Buff buff)
        {
            if (((SimpleBooleanStatusDebuff)buff)._immuneBlocked)
                return true;  // swallow buff if it was blocked

            return base.Stack(buff);
        }
    }

    [ImplementsPowerSNO(103216)] // DebuffBlind.pow
    [ImplementsPowerBuff(0)]
    public class DebuffBlind : SimpleBooleanStatusDebuff
    {
        public DebuffBlind(TickTimer timeout)
            : base(GameAttribute.Blind, GameAttribute.Immune_To_Blind, FloatingNumberMessage.FloatType.Blinded)
        {
            Timeout = timeout;
        }
    }

    [ImplementsPowerSNO(30195)] // DebuffChilled.pow
    [ImplementsPowerBuff(0)]
    public class DebuffChilled : SimpleBooleanStatusDebuff
    {
        public float Percentage;

        public DebuffChilled(float percentage, TickTimer timeout)
            : base(GameAttribute.Chilled, null, null)
        {
            Percentage = percentage;
            Timeout = timeout;
        }
        public override bool Apply()
        {
            if (!base.Apply())
                return false;
            Target.Attributes[GameAttribute.Attacks_Per_Second_Percent] -= Percentage;
            Target.Attributes[GameAttribute.Movement_Scalar_Reduction_Percent] += Percentage;
            Target.Attributes.BroadcastChangedIfRevealed();
            return true;
        }

        public override void Remove()
        {
            base.Remove();
            Target.Attributes[GameAttribute.Attacks_Per_Second_Percent] += Percentage;
            Target.Attributes[GameAttribute.Movement_Scalar_Reduction_Percent] -= Percentage;
            Target.Attributes.BroadcastChangedIfRevealed();
        }
    }

    [ImplementsPowerSNO(101000)] // DebuffStunned.pow
    [ImplementsPowerBuff(0)]
    public class DebuffStunned : SimpleBooleanStatusDebuff
    {
        public DebuffStunned(TickTimer timeout)
            : base(GameAttribute.Stunned, GameAttribute.Stun_Immune, FloatingNumberMessage.FloatType.Stunned)
        {
            Timeout = timeout;
        }
    }



    [ImplementsPowerSNO(101002)] // DebuffFeared.pow
    [ImplementsPowerBuff(0)]
    public class DebuffFeared : SimpleBooleanStatusDebuff
    {
        public DebuffFeared(TickTimer timeout)
            : base(GameAttribute.Feared, GameAttribute.Fear_Immune, FloatingNumberMessage.FloatType.Feared)
        {
            Timeout = timeout;
        }
    }

    [ImplementsPowerSNO(101003)] // DebuffRooted.pow
    [ImplementsPowerBuff(0)]
    public class DebuffRooted : SimpleBooleanStatusDebuff
    {
        public DebuffRooted(TickTimer timeout)
            : base(GameAttribute.IsRooted, GameAttribute.Root_Immune, FloatingNumberMessage.FloatType.Rooted)
        {
            Timeout = timeout;
        }
        //Seems there is no Rooted attribute.. so Stunned does the same thing.
        public override bool Apply()
        {
            if (!base.Apply())
                return false;
            Target.Attributes[GameAttribute.Stunned] = true;
            Target.Attributes.BroadcastChangedIfRevealed();
            return true;
        }

        public override void Remove()
        {
            base.Remove();
            Target.Attributes[GameAttribute.Stunned] = false;
            Target.Attributes.BroadcastChangedIfRevealed();
        }
    }


    [ImplementsPowerSNO(100971)] // DebuffSlowed.pow
    [ImplementsPowerBuff(0)]
    public class DebuffSlowed : SimpleBooleanStatusDebuff
    {
        public float Percentage;

        public DebuffSlowed(float percentage, TickTimer timeout)
            : base(GameAttribute.Slow, GameAttribute.Slowdown_Immune, FloatingNumberMessage.FloatType.Snared)
        {
            Percentage = percentage;
            Timeout = timeout;
        }
        public override bool Apply()
        {
            if (!base.Apply())
                return false;
            Target.Attributes[GameAttribute.Attacks_Per_Second_Percent] -= Percentage;
            Target.Attributes[GameAttribute.Movement_Scalar_Reduction_Percent] += Percentage;
            Target.Attributes.BroadcastChangedIfRevealed();
            return true;
        }

        public override void Remove()
        {
            base.Remove();
            Target.Attributes[GameAttribute.Attacks_Per_Second_Percent] += Percentage;
            Target.Attributes[GameAttribute.Movement_Scalar_Reduction_Percent] -= Percentage;
            Target.Attributes.BroadcastChangedIfRevealed();
        }
    }

    [ImplementsPowerBuff(0)]
    public class DebuffFrozen : SimpleBooleanStatusDebuff
    {
        public DebuffFrozen(TickTimer timeout)
            : base(GameAttribute.Frozen, GameAttribute.Freeze_Immune, FloatingNumberMessage.FloatType.Frozen)
        {
            Timeout = timeout;
        }
    }
    //----------------------------------------------------------------------------------------------
    //These are Skill-Related Powers
    [ImplementsPowerSNO(1755)] // SlowTimeDebuff.pow
    [ImplementsPowerBuff(0)]
    public class SlowTimeDebuff : SimpleBooleanStatusDebuff
    {
        public float Percentage;

        public SlowTimeDebuff(float percentage, TickTimer timeout)
            : base(GameAttribute.Slow, GameAttribute.Slowdown_Immune, FloatingNumberMessage.FloatType.Snared)
        {
            Percentage = percentage;
            Timeout = timeout;
        }
        public override bool Apply()
        {
            if (!base.Apply())
                return false;
            //is my projectile speed correct?
            Target.Attributes[GameAttribute.Projectile_Speed] += Target.Attributes[GameAttribute.Projectile_Speed] * 0.1f;
            Target.Attributes[GameAttribute.Attacks_Per_Second_Percent] -= Percentage;
            Target.Attributes[GameAttribute.Movement_Scalar_Reduction_Percent] += Percentage;
            Target.Attributes.BroadcastChangedIfRevealed();
            return true;
        }

        public override void Remove()
        {
            base.Remove();
            Target.Attributes[GameAttribute.Projectile_Speed] += Target.Attributes[GameAttribute.Projectile_Speed] / 0.1f;
            Target.Attributes[GameAttribute.Attacks_Per_Second_Percent] += Percentage;
            Target.Attributes[GameAttribute.Movement_Scalar_Reduction_Percent] -= Percentage;
            Target.Attributes.BroadcastChangedIfRevealed();
        }
    }

    //Wrong Section but i'm just gonna put it here.
    [ImplementsPowerSNO(74499)]
    [ImplementsPowerBuff(0)]
    public class MovementBuff : PowerBuff
    {
        public float Percentage;

        public MovementBuff(float percentage, TickTimer timeout)
        {
            Percentage = percentage;
            Timeout = timeout;
        }
        public override bool Apply()
        {
            if (!base.Apply())
                return false;
            Target.Attributes[GameAttribute.Movement_Bonus_Run_Speed] += Percentage;
            Target.Attributes.BroadcastChangedIfRevealed();
            return true;
        }

        public override void Remove()
        {
            base.Remove();
            Target.Attributes[GameAttribute.Movement_Bonus_Run_Speed] -= Percentage;
            Target.Attributes.BroadcastChangedIfRevealed();
        }
    }

    [ImplementsPowerSNO(1769)]
    [ImplementsPowerBuff(0)]
    public class SpeedBuff : PowerBuff
    {
        public float Percentage;

        public SpeedBuff(float percentage, TickTimer timeout)
        {
            Percentage = percentage;
            Timeout = timeout;
        }
        public override bool Apply()
        {
            if (!base.Apply())
                return false;
            Target.Attributes[GameAttribute.Casting_Speed_Percent] += Percentage;
            Target.Attributes[GameAttribute.Attacks_Per_Second_Percent] += Percentage;
            Target.Attributes.BroadcastChangedIfRevealed();
            return true;
        }

        public override void Remove()
        {
            base.Remove();
            Target.Attributes[GameAttribute.Casting_Speed_Percent] -= Percentage;
            Target.Attributes[GameAttribute.Attacks_Per_Second_Percent] -= Percentage;
            Target.Attributes.BroadcastChangedIfRevealed();
        }
    }
    [ImplementsPowerSNO(86991)]
    [ImplementsPowerBuff(0)]
    public class EnergyArmorPowers : PowerBuff
    {
        public EnergyArmorPowers(TickTimer timeout)
        {
            Timeout = timeout;
        }
        public override bool Apply()
        {
            if (!base.Apply())
                return false;
            if (Rune_A > 0)
            {
                Target.Attributes[GameAttribute.Defense_Bonus_Percent] += ScriptFormula(1);
                Target.Attributes[GameAttribute.Resource_Max_Bonus] -= ScriptFormula(2);
                Target.Attributes[GameAttribute.Resistance] += ScriptFormula(4);
                Target.Attributes.BroadcastChangedIfRevealed();
            }
            if (Rune_B > 0)
            {
                Target.Attributes[GameAttribute.Defense_Bonus_Percent] += ScriptFormula(1);
                Target.Attributes[GameAttribute.Resource_Max_Bonus] += ScriptFormula(6);
                Target.Attributes.BroadcastChangedIfRevealed();
            }
            if (Rune_E > 0)
            {
                Target.Attributes[GameAttribute.Defense_Bonus_Percent] += ScriptFormula(1);
                Target.Attributes[GameAttribute.Resource_Max_Bonus] -= ScriptFormula(2);
                Target.Attributes[GameAttribute.Precision_Bonus_Percent] += ScriptFormula(13);
                Target.Attributes.BroadcastChangedIfRevealed();
            }
            else
                Target.Attributes[GameAttribute.Defense_Bonus_Percent] += ScriptFormula(1);
            Target.Attributes[GameAttribute.Resource_Max_Bonus] -= ScriptFormula(2);
            Target.Attributes.BroadcastChangedIfRevealed();
            return true;
        }

        public override void Remove()
        {
            base.Remove();
            if (Rune_A > 0)
            {
                Target.Attributes[GameAttribute.Defense_Bonus_Percent] -= ScriptFormula(1);
                Target.Attributes[GameAttribute.Resource_Max_Bonus] += ScriptFormula(2);
                Target.Attributes[GameAttribute.Resistance] -= ScriptFormula(4);
                Target.Attributes.BroadcastChangedIfRevealed();
            }
            if (Rune_B > 0)
            {
                Target.Attributes[GameAttribute.Defense_Bonus_Percent] -= ScriptFormula(1);
                Target.Attributes[GameAttribute.Resource_Max_Bonus] -= ScriptFormula(6);
                Target.Attributes.BroadcastChangedIfRevealed();
            }
            if (Rune_E > 0)
            {
                Target.Attributes[GameAttribute.Defense_Bonus_Percent] -= ScriptFormula(1);
                Target.Attributes[GameAttribute.Resource_Max_Bonus] += ScriptFormula(2);
                Target.Attributes[GameAttribute.Precision_Bonus_Percent] -= ScriptFormula(13);
                Target.Attributes.BroadcastChangedIfRevealed();
            }
            else
                Target.Attributes[GameAttribute.Defense_Bonus_Percent] -= ScriptFormula(1);
            Target.Attributes[GameAttribute.Resource_Max_Bonus] += ScriptFormula(2);
            Target.Attributes.BroadcastChangedIfRevealed();
        }
    }
    [ImplementsPowerSNO(30680)] //blizzard.pow
    [ImplementsPowerBuff(0)]
    public class BlizzardPowers : PowerBuff
    {
        public BlizzardPowers(TickTimer timeout)
        {
            Timeout = timeout;
        }
        public override bool Apply()
        {
            if (!base.Apply())
                return false;
            Target.Attributes[GameAttribute.Crit_Damage_Percent] += (int)ScriptFormula(9);
            Target.Attributes.BroadcastChangedIfRevealed();
            return true;
        }

        public override void Remove()
        {
            base.Remove();
            Target.Attributes[GameAttribute.Crit_Damage_Percent] -= (int)ScriptFormula(9);
            Target.Attributes.BroadcastChangedIfRevealed();
        }
    }
}
