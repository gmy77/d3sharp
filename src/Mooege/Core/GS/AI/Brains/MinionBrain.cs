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

using System.Collections.Generic;
using Mooege.Common.Helpers.Math;
using Mooege.Core.GS.Actors;
using Mooege.Core.GS.Actors.Movement;
using Mooege.Core.GS.Common.Types.Math;
using Mooege.Core.GS.Players;
using Mooege.Core.GS.Actors.Actions;
using Mooege.Net.GS.Message;
using Mooege.Common.MPQ;
using Mooege.Core.GS.Common.Types.SNO;
using Mooege.Core.GS.Ticker;

namespace Mooege.Core.GS.AI.Brains
{
    public class MinionBrain : Brain
    {
        // list of power SNOs that are defined for the monster
        public List<int> PresetPowers { get; private set; }

        private TickTimer _powerDelay;

        public MinionBrain(Actor body)
            : base(body)
        {
            this.PresetPowers = new List<int>();

            // build list of powers defined in monster mpq data
            if (body.ActorData.MonsterSNO > 0)
            {
                var monsterData = (Mooege.Common.MPQ.FileFormats.Monster)MPQStorage.Data.Assets[SNOGroup.Monster][body.ActorData.MonsterSNO].Data;
                foreach (var monsterSkill in monsterData.SkillDeclarations)
                {
                    if (monsterSkill.SNOPower > 0)
                    {
                        this.PresetPowers.Add(monsterSkill.SNOPower);
                    }
                }
            }
        }

        public override void Think(int tickCounter)
        {
            // this needed? /mdz
            if (this.Body is NPC) return;

            // check if in disabled state, if so cancel any action then do nothing
            if (this.Body.Attributes[GameAttribute.Frozen] ||
                this.Body.Attributes[GameAttribute.Stunned] ||
                this.Body.Attributes[GameAttribute.Blind] ||
                this.Body.World.BuffManager.GetFirstBuff<Powers.Implementations.KnockbackBuff>(this.Body) != null)
            {
                if (this.CurrentAction != null)
                {
                    this.CurrentAction.Cancel(tickCounter);
                    this.CurrentAction = null;
                }
                _powerDelay = null;

                return;
            }

            // select and start executing a power if no active action
            if (this.CurrentAction == null)
            {
                // do a little delay so groups of monsters don't all execute at once
                if (_powerDelay == null)
                    _powerDelay = new SecondsTickTimer(this.Body.World.Game, (float)RandomHelper.NextDouble());

                if (_powerDelay.TimedOut)
                {
                    if (this.Body.GetObjectsInRange<Monster>(20f).Count != 0)
                    {
                        //System.Console.Out.WriteLine("Enemy in range, use powers");
                        //This will only attack when you and your minions are not moving..TODO: FIX.
                        int powerToUse = PickPowerToUse();
                        if (powerToUse > 0)
                            this.CurrentAction = new PowerAction(this.Body, powerToUse);
                    }
                    else
                    {
                        //System.Console.Out.WriteLine("No enemies in range, return to master");
                        //TODO: Minions need to be behind Toons on either side. 1st Master 2nd 3rd
                        this.CurrentAction = new MoveToPointAction(this.Body, (this.Body as Minion).Master.Position);
                    }
                }
            }
        }

        protected virtual int PickPowerToUse()
        {
            // randomly used an implemented power
            if (this.PresetPowers.Count > 0)
            {
                int powerIndex = RandomHelper.Next(this.PresetPowers.Count);
                if (Powers.PowerLoader.HasImplementationForPowerSNO(this.PresetPowers[powerIndex]))
                    return this.PresetPowers[powerIndex];
            }

            // no usable power
            return -1;
        }

        public void AddPresetPower(int powerSNO)
        {
            this.PresetPowers.Add(powerSNO);
        }
    }
}
