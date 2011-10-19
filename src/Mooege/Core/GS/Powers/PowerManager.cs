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
using Mooege.Net.GS.Message.Fields;
using Mooege.Core.GS.Actors;
using Mooege.Net.GS;
using Mooege.Net.GS.Message.Definitions.Combat;
using Mooege.Core.GS.Skills;
using Mooege.Net.GS.Message.Definitions.ACD;
using Mooege.Net.GS.Message.Definitions.Misc;
using Mooege.Common.Helpers;
using Mooege.Net.GS.Message.Definitions.Animation;
using Mooege.Net.GS.Message;
using Mooege.Net.GS.Message.Definitions.Effect;
using Mooege.Net.GS.Message.Definitions.Attribute;
using Mooege.Net.GS.Message.Definitions.Player;
using Mooege.Common;
using Mooege.Core.GS.Map;
using Mooege.Net.GS.Message.Definitions.World;
using Mooege.Net.GS.Message.Definitions.Actor;

namespace Mooege.Core.GS.Powers
{
    public class PowerManager
    {
        static readonly Logger Logger = LogManager.CreateLogger();

        private bool _isChanneling = false;
        private TickTimer _channelCastDelay = null;
        private IList<Effect> _channelEffects = null;

        // list of all waiting to execute powers
        private class WaitingPower
        {
            public IEnumerator<TickTimer> PowerEnumerator;
            public PowerImplementation Implementation;
        }
        private List<WaitingPower> _waitingPowers = new List<WaitingPower>();
        
        public PowerManager()
        {
        }

        public void Update()
        {
            UpdateWaitingPowers();
        }

        public void UsePower(Actor user, int powerSNO, uint targetId = uint.MaxValue, Vector3D targetPosition = null,
                             TargetMessage message = null)
        {
            Actor target;

            if (targetId == uint.MaxValue)
            {
                target = null;
            }
            else if (user.World.GetActor(targetId) != null)
            {
                target = user.World.GetActor(targetId);
                targetPosition = target.Position;
            }
            else
            {
                return;
            }

            if (targetPosition == null)
                targetPosition = new Vector3D(0, 0, 0);

            // HACK: intercept hotbar skill 1 to always spawn test mobs.
            if (user is Player.Player &&
                powerSNO == ((Player.Player)user).SkillSet.HotBarSkills[4].SNOSkill)
            {
                PowersTestMonster.CreateTestMonsters(user.World, user.Position, 10);
                return;
            }

            // find and run a power implementation
            var implementation = PowerLoader.CreateImplementationForPowerSNO(powerSNO);
            if (implementation != null)
            {
                // copy in base params
                implementation.PowerManager = this;
                implementation.User = user;
                implementation.Target = target;
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

                var powerEnum = implementation.Run().GetEnumerator();
                // actual power will first run here, if it yielded a timer process it in the waiting list
                if (powerEnum.MoveNext() && powerEnum.Current != PowerImplementation.StopExecution)
                {
                    _waitingPowers.Add(new WaitingPower
                    {
                        PowerEnumerator = powerEnum,
                        Implementation = implementation
                    });
                }
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
                        return wait.PowerEnumerator.Current == PowerImplementation.StopExecution;
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
                foreach (Effect effect in _channelEffects)
                    effect.Destroy();

                _channelEffects = null;
            }
        }

        public Effect GetChanneledEffect(Actor user, int index, int actorSNO, Vector3D position)
        {
            if (!_isChanneling) return null;

            if (_channelEffects == null)
                _channelEffects = new List<Effect>();

            // ensure effects list is at least big enough for specified index
            while (_channelEffects.Count < index + 1)
            {
                _channelEffects.Add(new Effect(user.World, actorSNO, position, 0, null));
            }

            _channelEffects[index].MoveTranslate(position, 8f);

            return _channelEffects[index];
        }

        public Effect GetChanneledProxy(Actor user, int index, Vector3D position)
        {
            return GetChanneledEffect(user, index, 187359, position);
        }
    }
}
