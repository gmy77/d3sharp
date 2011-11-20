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
                User.Attributes[GameAttribute.Buff_Active, PowerSNO] = true;
                BroadcastChangedAttributes(User);
            }
            return true;
        }

        public virtual void Remove()
        {
            if (PowerSNO != 0)
            {
                User.Attributes[GameAttribute.Buff_Active, PowerSNO] = false;
                BroadcastChangedAttributes(User);
            }
        }

        public virtual void Init() { }
        public virtual bool Update() { return false; }
        public virtual void Stack(Buff buff) { }
        // OnPayload
        // OnFinished
    }

    public abstract class TimedBuff : Buff
    {
        public TickTimer Timeout;

        public override bool Update()
        {
            return this.Timeout != null && this.Timeout.TimedOut;
        }

        public override void Stack(Buff buff)
        {
            TimedBuff newbuff = (TimedBuff)buff;
            // update buff if new timeout is longer than current one, or if new buff has no timeout
            if (newbuff.Timeout == null || this.Timeout != null && newbuff.Timeout.TimeoutTick > this.Timeout.TimeoutTick)
                this.Timeout = newbuff.Timeout;
        }
    }

    public abstract class PowerBuff : TimedBuff
    {
        public int BuffSlot = 0;

        public PowerBuff()
        {
            // try to load BuffSlot from attribute
            var attributes = (ImplementsBuffSlot[])this.GetType().GetCustomAttributes(typeof(ImplementsBuffSlot), true);
            foreach (var slotAttribute in attributes)
            {
                BuffSlot = slotAttribute.Slot;
            }
        }

        public override bool Apply()
        {
            base.Apply();

            if (this.Timeout != null)
            {
                User.Attributes[_Buff_Icon_Start_TickN, PowerSNO] = this.Timeout.TimeoutTick;
                User.Attributes[_Buff_Icon_End_TickN, PowerSNO] = this.Timeout.TimeoutTick;
                User.Attributes[_Buff_Icon_CountN, PowerSNO] = 1;
                User.Attributes[_Power_Buff_N_VisualEffect_R, PowerSNO] = true;
                BroadcastChangedAttributes(User);
            }
            return true;
        }

        public override void Remove()
        {
            base.Remove();

            if (this.Timeout != null)
            {
                User.Attributes[_Power_Buff_N_VisualEffect_R, PowerSNO] = false;
                User.Attributes[_Buff_Icon_CountN, PowerSNO] = 0;
                BroadcastChangedAttributes(User);
            }
        }

        public override void Stack(Buff buff)
        {
            base.Stack(buff);

            //User.Attributes[GameAttribute.Buff_Icon_Count0, PowerSNO] += 1;
            if (this.Timeout != null)
            {
                User.Attributes[_Buff_Icon_Start_TickN, PowerSNO] = this.Timeout.TimeoutTick;
                User.Attributes[_Buff_Icon_End_TickN, PowerSNO] = this.Timeout.TimeoutTick;
                BroadcastChangedAttributes(User);
            }
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
