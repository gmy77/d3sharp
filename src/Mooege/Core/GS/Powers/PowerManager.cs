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
using Mooege.Core.GS.Actors;
using Mooege.Core.GS.Common.Types.Math;
using Mooege.Core.GS.Players;
using Mooege.Net.GS.Message;
using Mooege.Net.GS.Message.Definitions.World;
using System.Linq;
using Mooege.Core.GS.Ticker;
using Mooege.Common.Helpers.Math;
using Mooege.Common.Logging;

namespace Mooege.Core.GS.Powers
{
    public class PowerManager
    {
        static readonly Logger Logger = LogManager.CreateLogger();

        // list of all actively channeled powers
        private List<ChanneledPower> _channeledPowers = new List<ChanneledPower>();

        // list of all waiting to execute powers
        private class WaitingPower
        {
            public IEnumerator<TickTimer> PowerEnumerator;
            public PowerScript Implementation;
        }
        private List<WaitingPower> _waitingPowers = new List<WaitingPower>();
        
        public PowerManager()
        {
        }

        public void Update()
        {
            UpdateWaitingPowers();
        }

        public bool UsePower(Actor user, PowerScript power, Actor target = null,
                             Vector3D targetPosition = null, TargetMessage targetMessage = null)
        {
            // replace power with existing channel instance if one exists
            if (power is ChanneledPower)
            {
                var chanpow = _FindChannelingPower(user, power.PowerSNO);
                if (chanpow != null)
                    power = chanpow;
            }

            // copy in context params
            power.User = user;
            power.Target = target;
            power.World = user.World;
            power.TargetPosition = targetPosition;
            power.TargetMessage = targetMessage;

            // process channeled power events
            var channeledPower = power as ChanneledPower;
            if (channeledPower != null)
            {
                if (channeledPower.ChannelOpen)
                {
                    channeledPower.OnChannelUpdated();
                }
                else
                {
                    channeledPower.OnChannelOpen();
                    channeledPower.ChannelOpen = true;
                    _channeledPowers.Add(channeledPower);
                }
            }

            var powerEnum = power.Run().GetEnumerator();
            // actual power will first run here, if it yielded a timer process it in the waiting list
            if (powerEnum.MoveNext() && powerEnum.Current != PowerScript.StopExecution)
            {
                _waitingPowers.Add(new WaitingPower
                {
                    PowerEnumerator = powerEnum,
                    Implementation = power
                });
            }

            return true;
        }
        
        // HACK: used for item spawn helper in UsePower()
        private bool _spawnedHelperItems = false;

        public bool UsePower(Actor user, int powerSNO, uint targetId = uint.MaxValue, Vector3D targetPosition = null,
                             TargetMessage targetMessage = null)
        {
            Actor target;

            if (targetId == uint.MaxValue)
            {
                target = null;
            }
            else
            {
                target = user.World.GetActorByDynamicId(targetId);
                if (target == null)
                    return false;

                targetPosition = target.Position;
            }
                        
            #region Items and Monster spawn HACK
            // HACK: intercept hotbar skill 1 to always spawn test mobs.
            if (user is Player && powerSNO == (user as Player).SkillSet.HotBarSkills[4].SNOSkill)
            {
                // number of monsters to spawn
                int spawn_count = 10;

                // list of actorSNO values to pick from when spawning
                int[] actorSNO_values = { 4282, 3893, 6652, 5428, 5346, 6024, 5393, 5467 };
                int actorSNO = actorSNO_values[RandomHelper.Next(actorSNO_values.Length - 1)];
                Logger.Debug("10 monsters spawning with actor sno {0}", actorSNO);

                for (int n = 0; n < spawn_count; ++n)
                {
                    Vector3D position;

                    if (targetPosition.X == 0f)
                    {
                        position = new Vector3D(user.Position);
                        if ((n % 2) == 0)
                        {
                            position.X += (float)(RandomHelper.NextDouble() * 20);
                            position.Y += (float)(RandomHelper.NextDouble() * 20);
                        }
                        else
                        {
                            position.X -= (float)(RandomHelper.NextDouble() * 20);
                            position.Y -= (float)(RandomHelper.NextDouble() * 20);
                        }
                    }
                    else
                    {
                        position = new Vector3D(targetPosition);
                        position.X += (float)(RandomHelper.NextDouble() - 0.5) * 20;
                        position.Y += (float)(RandomHelper.NextDouble() - 0.5) * 20;
                        position.Z = user.Position.Z;
                    }

                    Monster mon = new Monster(user.World, actorSNO, null);
                    mon.Position = position;
                    mon.Scale = 1.35f;
                    mon.Attributes[GameAttribute.Hitpoints_Max_Total] = 50f;
                    mon.Attributes[GameAttribute.Hitpoints_Max] = 50f;
                    mon.Attributes[GameAttribute.Hitpoints_Total_From_Level] = 0f;
                    mon.Attributes[GameAttribute.Hitpoints_Cur] = 50f;
                    user.World.Enter(mon);
                }

                // spawn some useful items for testing at the ground of the player
                if (!_spawnedHelperItems)
                {
                    _spawnedHelperItems = true;
                    Items.ItemGenerator.Cook((Players.Player)user, "Sword_2H_205").EnterWorld(user.Position);
                    Items.ItemGenerator.Cook((Players.Player)user, "Crossbow_102").EnterWorld(user.Position);
                    for (int n = 0; n < 30; ++n)
                        Items.ItemGenerator.Cook((Players.Player)user, "Runestone_Unattuned_07").EnterWorld(user.Position);
                }
                
                return true;
            }
            #endregion

            // find and run a power implementation
            var implementation = PowerLoader.CreateImplementationForPowerSNO(powerSNO);
            if (implementation != null)
            {
                implementation.PowerSNO = powerSNO;
                return UsePower(user, implementation, target, targetPosition, targetMessage);
            }
            else
            {
                return false;
            }
        }

        public void UpdateWaitingPowers()
        {
            // process all powers, removing from the list the ones that expire
            _waitingPowers.RemoveAll((wait) =>
            {
                if (wait.PowerEnumerator.Current.TimedOut)
                {
                    if (wait.PowerEnumerator.MoveNext())
                        return wait.PowerEnumerator.Current == PowerScript.StopExecution;
                    else
                        return true;
                }
                else
                {
                    return false;
                }
            });
        }

        public void CancelChanneledPower(Actor user, int powerSNO)
        {
            var channeledPower = _FindChannelingPower(user, powerSNO);
            if (channeledPower != null)
            {
                channeledPower.OnChannelClose();
                channeledPower.ChannelOpen = false;
                _channeledPowers.Remove(channeledPower);
            }
            else
            {
                Logger.Debug("cancel channel for power {0}, but it doesn't have an open channel to cancel", powerSNO);
            }
        }

        private ChanneledPower _FindChannelingPower(Actor user, int powerSNO)
        {
            return _channeledPowers.FirstOrDefault(impl => impl.User == user &&
                                                           impl.PowerSNO == powerSNO &&
                                                           impl.ChannelOpen);
        }
    }
}
