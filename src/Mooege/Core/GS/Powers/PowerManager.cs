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
using Mooege.Common;
using Mooege.Core.GS.Actors;
using Mooege.Core.GS.Common.Types.Math;
using Mooege.Core.GS.Players;
using Mooege.Net.GS.Message.Definitions.World;
using Mooege.Common.Helpers;
using Mooege.Net.GS.Message;

namespace Mooege.Core.GS.Powers
{
    public class PowerManager
    {
        static readonly Logger Logger = LogManager.CreateLogger();

        private Actor _user;

        private bool _isChanneling = false;
        private TickTimer _channelCastDelay = null;
        private IList<ClientEffect> _channelEffects = null;

        // list of all waiting to execute powers
        private class WaitingPower
        {
            public IEnumerator<TickTimer> PowerEnumerator;
            public ContinuableEffect Implementation;
        }
        private List<WaitingPower> _waitingPowers = new List<WaitingPower>();
        
        public PowerManager(Actor user)
        {
            _user = user;
        }

        public void Update()
        {
            UpdateWaitingPowers();
        }

        public bool UsePower(int powerSNO, uint targetId = uint.MaxValue, Vector3D targetPosition = null,
                             TargetMessage message = null)
        {
            Actor target;

            if (targetId == uint.MaxValue)
            {
                target = null;
            }
            else if (_user.World.GetActor(targetId) != null)
            {
                target = _user.World.GetActor(targetId);
                targetPosition = target.Position;
            }
            else
            {
                return false;
            }

            if (targetPosition == null)
                targetPosition = new Vector3D(0, 0, 0);

            #region Monster spawn HACK
            // HACK: intercept hotbar skill 1 to always spawn test mobs.
            if (_user is Player && powerSNO == (_user as Player).SkillSet.HotBarSkills[4].SNOSkill)
            {
                // number of monsters to spawn
                int spawn_count = 10;

                // list of actorSNO values to pick from when spawning
                int[] actorSNO_values = { 4282, 3893, 6652, 5428, 5346, 6024, 5393, 5433, 5467 };

                for (int n = 0; n < spawn_count; ++n)
                {
                    Vector3D position;

                    if (targetPosition.X == 0f)
                    {
                        position = new Vector3D(_user.Position);
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
                        position.Z = _user.Position.Z;
                    }

                    int actorSNO = actorSNO_values[RandomHelper.Next(actorSNO_values.Length - 1)];

                    Monster mon = new Monster(_user.World, actorSNO, position, null);
                    mon.Scale = 1.35f;
                    mon.Attributes[GameAttribute.Hitpoints_Max_Total] = 50f;
                    mon.Attributes[GameAttribute.Hitpoints_Max] = 50f;
                    mon.Attributes[GameAttribute.Hitpoints_Total_From_Level] = 0f;
                    mon.Attributes[GameAttribute.Hitpoints_Cur] = 50f;
                    _user.World.Enter(mon);
                }

                return true;
            }
            #endregion

            // find and run a power implementation
            var implementation = PowerLoader.CreateImplementationForPowerSNO(powerSNO);
            if (implementation != null)
            {
                // copy in base params
                implementation.PowerManager = this;
                implementation.PowerSNO = powerSNO;
                implementation.User = _user;
                implementation.Target = target;
                implementation.World = _user.World;
                implementation.TargetPosition = targetPosition;
                implementation.Message = message;

                // process channeled skill params
                implementation.UserIsChanneling = _isChanneling;
                implementation.ThrottledCast = false;
                if (_isChanneling && _channelCastDelay != null)
                {
                    if (_channelCastDelay.TimedOut())
                        _channelCastDelay = null;
                    else
                        implementation.ThrottledCast = true;
                }

                var powerEnum = implementation.Continue().GetEnumerator();
                // actual power will first run here, if it yielded a timer process it in the waiting list
                if (powerEnum.MoveNext() && powerEnum.Current != ContinuableEffect.StopExecution)
                {
                    _waitingPowers.Add(new WaitingPower
                    {
                        PowerEnumerator = powerEnum,
                        Implementation = implementation
                    });
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        private void UpdateWaitingPowers()
        {
            // process all powers, removing from the list the ones that expire
            _waitingPowers.RemoveAll((wait) =>
            {
                if (wait.PowerEnumerator.Current.TimedOut())
                {
                    if (wait.PowerEnumerator.MoveNext())
                        return wait.PowerEnumerator.Current == ContinuableEffect.StopExecution;
                    else
                        return true;
                }
                else
                {
                    return false;
                }
            });
        }

        public void RegisterChannelingPower(TickTimer channelCastDelay = null)
        {
            _isChanneling = true;
            if (_channelCastDelay == null)
                _channelCastDelay = channelCastDelay;
        }

        public void CancelChanneledPower(int powerSNO)
        {
            _isChanneling = false;
            _channelCastDelay = null;
            if (_channelEffects != null)
            {
                foreach (ClientEffect effect in _channelEffects)
                    effect.Destroy();

                _channelEffects = null;
            }
        }

        public ClientEffect GetChanneledEffect(Actor user, int index, int actorSNO, Vector3D position, bool snapped)
        {
            if (!_isChanneling) return null;
            
            if (_channelEffects == null)
                _channelEffects = new List<ClientEffect>();

            // ensure effects list is at least big enough for specified index
            while (_channelEffects.Count < index + 1)
            {
                _channelEffects.Add(new ClientEffect(user.World, actorSNO, position, 0, null));
            }

            if (snapped)
                _channelEffects[index].MoveSnapped(position);
            else
                _channelEffects[index].MoveNormal(position, 8f);

            return _channelEffects[index];
        }

        public ClientEffect GetChanneledProxy(Actor user, int index, Vector3D position, bool snapped)
        {
            return GetChanneledEffect(user, index, 187359, position, snapped);
        }
    }
}
