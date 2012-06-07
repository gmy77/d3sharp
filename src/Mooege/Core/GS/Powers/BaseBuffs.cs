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

namespace Mooege.Core.GS.Powers
{
    public abstract class Buff : PowerContext
    {
        public virtual bool Apply()
        {
            if (PowerSNO != 0)
            {
                Target.Attributes[GameAttribute.Buff_Active, PowerSNO] = true;
                Target.Attributes.BroadcastChangedIfRevealed();
            }
            return true;
        }

        public virtual void Remove()
        {
            if (PowerSNO != 0)
            {
                Target.Attributes[GameAttribute.Buff_Active, PowerSNO] = false;
                Target.Attributes.BroadcastChangedIfRevealed();
            }
        }

        public virtual void Init() { }
        public virtual bool Update() { return false; }
        public virtual bool Stack(Buff buff) { return false; }
        public virtual void OnPayload(Payloads.Payload payload) { }
    }

    public abstract class TimedBuff : Buff
    {
        public TickTimer Timeout;

        public override bool Update()
        {
            return this.Timeout != null && this.Timeout.TimedOut;
        }

        public override bool Stack(Buff buff)
        {
            TimedBuff newbuff = (TimedBuff)buff;
            // update buff if new timeout is longer than current one, or if new buff has no timeout
            if (newbuff.Timeout == null || this.Timeout != null && newbuff.Timeout.TimeoutTick > this.Timeout.TimeoutTick)
                this.Timeout = newbuff.Timeout;

            return true;
        }
    }

    public abstract class PowerBuff : TimedBuff
    {
        public int BuffSlot = 0;
        public bool IsCountingStacks = false;
        public int StackCount = 0;
        public int MaxStackCount = 0;

        public PowerBuff()
        {
            // try to load buff options from attribute
            var attributes = (ImplementsPowerBuff[])this.GetType().GetCustomAttributes(typeof(ImplementsPowerBuff), true);
            foreach (var attr in attributes)
            {
                BuffSlot = attr.BuffSlot;
                IsCountingStacks = attr.CountStacks;
            }
        }

        public override bool Apply()
        {
            base.Apply();

            Target.Attributes[_Power_Buff_N_VisualEffect_R, PowerSNO] = true;
            if (this.Timeout != null)
            {
                Target.Attributes[_Buff_Icon_Start_TickN, PowerSNO] = this.Timeout.TimeoutTick;
                Target.Attributes[_Buff_Icon_End_TickN, PowerSNO] = this.Timeout.TimeoutTick;
                Target.Attributes[_Buff_Icon_CountN, PowerSNO] = 1;
            }
            Target.Attributes.BroadcastChangedIfRevealed();

            this.StackCount = 1;

            return true;
        }

        public override void Remove()
        {
            base.Remove();

            Target.Attributes[_Power_Buff_N_VisualEffect_R, PowerSNO] = false;
            if (this.Timeout != null)
            {
                Target.Attributes[_Buff_Icon_Start_TickN, PowerSNO] = 0;
                Target.Attributes[_Buff_Icon_End_TickN, PowerSNO] = 0;
                Target.Attributes[_Buff_Icon_CountN, PowerSNO] = 0;
            }
            Target.Attributes.BroadcastChangedIfRevealed();

            this.StackCount = 0;
        }

        public override bool Stack(Buff buff)
        {
            base.Stack(buff);

            bool canStack = this.IsCountingStacks && StackCount != MaxStackCount;

            if (this.Timeout != null)
            {
                Target.Attributes[_Buff_Icon_Start_TickN, PowerSNO] = this.Timeout.TimeoutTick;
                Target.Attributes[_Buff_Icon_End_TickN, PowerSNO] = this.Timeout.TimeoutTick;
                if (canStack)
                    Target.Attributes[_Buff_Icon_CountN, PowerSNO] += 1;
            }
            Target.Attributes.BroadcastChangedIfRevealed();

            if (canStack)
                this.StackCount += 1;

            return true;
        }

        private GameAttributeB _Power_Buff_N_VisualEffect_R
        {
            get
            {
                switch (BuffSlot)
                {
                    default:
                    case 0:
                        return RuneSelect(GameAttribute.Power_Buff_0_Visual_Effect_None,
                                          GameAttribute.Power_Buff_0_Visual_Effect_A,
                                          GameAttribute.Power_Buff_0_Visual_Effect_B,
                                          GameAttribute.Power_Buff_0_Visual_Effect_C,
                                          GameAttribute.Power_Buff_0_Visual_Effect_D,
                                          GameAttribute.Power_Buff_0_Visual_Effect_E);
                    case 1:
                        return RuneSelect(GameAttribute.Power_Buff_1_Visual_Effect_None,
                                          GameAttribute.Power_Buff_1_Visual_Effect_A,
                                          GameAttribute.Power_Buff_1_Visual_Effect_B,
                                          GameAttribute.Power_Buff_1_Visual_Effect_C,
                                          GameAttribute.Power_Buff_1_Visual_Effect_D,
                                          GameAttribute.Power_Buff_1_Visual_Effect_E);
                    case 2:
                        return RuneSelect(GameAttribute.Power_Buff_2_Visual_Effect_None,
                                          GameAttribute.Power_Buff_2_Visual_Effect_A,
                                          GameAttribute.Power_Buff_2_Visual_Effect_B,
                                          GameAttribute.Power_Buff_2_Visual_Effect_C,
                                          GameAttribute.Power_Buff_2_Visual_Effect_D,
                                          GameAttribute.Power_Buff_2_Visual_Effect_E);
                    case 3:
                        return RuneSelect(GameAttribute.Power_Buff_3_Visual_Effect_None,
                                          GameAttribute.Power_Buff_3_Visual_Effect_A,
                                          GameAttribute.Power_Buff_3_Visual_Effect_B,
                                          GameAttribute.Power_Buff_3_Visual_Effect_C,
                                          GameAttribute.Power_Buff_3_Visual_Effect_D,
                                          GameAttribute.Power_Buff_3_Visual_Effect_E);
                    case 4:
                        return RuneSelect(GameAttribute.Power_Buff_4_Visual_Effect_None,
                                          GameAttribute.Power_Buff_4_Visual_Effect_A,
                                          GameAttribute.Power_Buff_4_Visual_Effect_B,
                                          GameAttribute.Power_Buff_4_Visual_Effect_C,
                                          GameAttribute.Power_Buff_4_Visual_Effect_D,
                                          GameAttribute.Power_Buff_4_Visual_Effect_E);
                    case 5:
                        return RuneSelect(GameAttribute.Power_Buff_5_Visual_Effect_None,
                                          GameAttribute.Power_Buff_5_Visual_Effect_A,
                                          GameAttribute.Power_Buff_5_Visual_Effect_B,
                                          GameAttribute.Power_Buff_5_Visual_Effect_C,
                                          GameAttribute.Power_Buff_5_Visual_Effect_D,
                                          GameAttribute.Power_Buff_5_Visual_Effect_E);
                    case 6:
                        return RuneSelect(GameAttribute.Power_Buff_6_Visual_Effect_None,
                                          GameAttribute.Power_Buff_6_Visual_Effect_A,
                                          GameAttribute.Power_Buff_6_Visual_Effect_B,
                                          GameAttribute.Power_Buff_6_Visual_Effect_C,
                                          GameAttribute.Power_Buff_6_Visual_Effect_D,
                                          GameAttribute.Power_Buff_6_Visual_Effect_E);
                    case 7:
                        return RuneSelect(GameAttribute.Power_Buff_7_Visual_Effect_None,
                                          GameAttribute.Power_Buff_7_Visual_Effect_A,
                                          GameAttribute.Power_Buff_7_Visual_Effect_B,
                                          GameAttribute.Power_Buff_7_Visual_Effect_C,
                                          GameAttribute.Power_Buff_7_Visual_Effect_D,
                                          GameAttribute.Power_Buff_7_Visual_Effect_E);
                }
            }
        }

        private GameAttributeI _Buff_Icon_Start_TickN
        {
            get
            {
                switch (BuffSlot)
                {
                    default:
                    case 0: return GameAttribute.Buff_Icon_Start_Tick0;
                    case 1: return GameAttribute.Buff_Icon_Start_Tick1;
                    case 2: return GameAttribute.Buff_Icon_Start_Tick2;
                    case 3: return GameAttribute.Buff_Icon_Start_Tick3;
                    case 4: return GameAttribute.Buff_Icon_Start_Tick4;
                    case 5: return GameAttribute.Buff_Icon_Start_Tick5;
                    case 6: return GameAttribute.Buff_Icon_Start_Tick6;
                    case 7: return GameAttribute.Buff_Icon_Start_Tick7;
                }
            }
        }

        private GameAttributeI _Buff_Icon_End_TickN
        {
            get
            {
                switch (BuffSlot)
                {
                    default:
                    case 0: return GameAttribute.Buff_Icon_End_Tick0;
                    case 1: return GameAttribute.Buff_Icon_End_Tick1;
                    case 2: return GameAttribute.Buff_Icon_End_Tick2;
                    case 3: return GameAttribute.Buff_Icon_End_Tick3;
                    case 4: return GameAttribute.Buff_Icon_End_Tick4;
                    case 5: return GameAttribute.Buff_Icon_End_Tick5;
                    case 6: return GameAttribute.Buff_Icon_End_Tick6;
                    case 7: return GameAttribute.Buff_Icon_End_Tick7;
                }
            }
        }

        private GameAttributeI _Buff_Icon_CountN
        {
            get
            {
                switch (BuffSlot)
                {
                    default:
                    case 0: return GameAttribute.Buff_Icon_Count0;
                    case 1: return GameAttribute.Buff_Icon_Count1;
                    case 2: return GameAttribute.Buff_Icon_Count2;
                    case 3: return GameAttribute.Buff_Icon_Count3;
                    case 4: return GameAttribute.Buff_Icon_Count4;
                    case 5: return GameAttribute.Buff_Icon_Count5;
                    case 6: return GameAttribute.Buff_Icon_Count6;
                    case 7: return GameAttribute.Buff_Icon_Count7;
                }
            }
        }
    }
}
