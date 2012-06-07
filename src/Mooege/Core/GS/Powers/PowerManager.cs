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
using System.Linq;
using Mooege.Common.Logging;
using Mooege.Core.GS.Actors;
using Mooege.Core.GS.Common.Types.Math;
using Mooege.Core.GS.Ticker;
using Mooege.Net.GS.Message.Definitions.World;

namespace Mooege.Core.GS.Powers
{
    public class PowerManager
    {
        static readonly Logger Logger = LogManager.CreateLogger();

        // list of all actively channeled skills
        private List<ChanneledSkill> _channeledSkills = new List<ChanneledSkill>();

        // list of all executing power scripts
        private class ExecutingScript
        {
            public IEnumerator<TickTimer> PowerEnumerator;
            public PowerScript Script;
        }
        private List<ExecutingScript> _executingScripts = new List<ExecutingScript>();

        // list of actors that were killed and are waiting to be deleted
        // rather ugly hack needed because deleting actors immediatly when they have visual buff effects
        // applied causes the effects to stay around forever.
        private Dictionary<Actor, TickTimer> _deletingActors = new Dictionary<Actor, TickTimer>();

        public PowerManager()
        {
        }

        public void Update()
        {
            _UpdateDeletingActors();
            _UpdateExecutingScripts();
        }

        public bool RunPower(Actor user, PowerScript power, Actor target = null,
                             Vector3D targetPosition = null, TargetMessage targetMessage = null)
        {
            // replace power with existing channel instance if one exists
            if (power is ChanneledSkill)
            {
                var existingChannel = _FindChannelingSkill(user, power.PowerSNO);
                if (existingChannel != null)
                {
                    power = existingChannel;
                }
                else  // new channeled skill, add it to the list
                {
                    _channeledSkills.Add((ChanneledSkill)power);
                }
            }

            // copy in context params
            power.User = user;
            power.Target = target;
            power.World = user.World;
            power.TargetPosition = targetPosition;
            power.TargetMessage = targetMessage;

            _StartScript(power);
            return true;
        }

        public bool RunPower(Actor user, int powerSNO, uint targetId = uint.MaxValue, Vector3D targetPosition = null,
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

            // find and run a power implementation
            var implementation = PowerLoader.CreateImplementationForPowerSNO(powerSNO);
            if (implementation != null)
            {
                return RunPower(user, implementation, target, targetPosition, targetMessage);
            }
            else
            {
                return false;
            }
        }

        private void _UpdateExecutingScripts()
        {
            // process all powers, removing from the list the ones that expire
            _executingScripts.RemoveAll(script =>
            {
                if (script.PowerEnumerator.Current.TimedOut)
                {
                    if (script.PowerEnumerator.MoveNext())
                        return script.PowerEnumerator.Current == PowerScript.StopExecution;
                    else
                        return true;
                }
                else
                {
                    return false;
                }
            });
        }

        public void CancelChanneledSkill(Actor user, int powerSNO)
        {
            var channeledSkill = _FindChannelingSkill(user, powerSNO);
            if (channeledSkill != null)
            {
                channeledSkill.CloseChannel();
                _channeledSkills.Remove(channeledSkill);
            }
            else
            {
                Logger.Debug("cancel channel for power {0}, but it doesn't have an open channel to cancel", powerSNO);
            }
        }

        private ChanneledSkill _FindChannelingSkill(Actor user, int powerSNO)
        {
            return _channeledSkills.FirstOrDefault(impl => impl.User == user &&
                                                           impl.PowerSNO == powerSNO &&
                                                           impl.IsChannelOpen);
        }

        private void _StartScript(PowerScript script)
        {
            var powerEnum = script.Run().GetEnumerator();
            if (powerEnum.MoveNext() && powerEnum.Current != PowerScript.StopExecution)
            {
                _executingScripts.Add(new ExecutingScript
                {
                    PowerEnumerator = powerEnum,
                    Script = script
                });
            }
        }

        private void _UpdateDeletingActors()
        {
            foreach (var key in _deletingActors.Keys.ToArray())
            {
                if (_deletingActors[key].TimedOut)
                {
                    key.Destroy();
                    _deletingActors.Remove(key);
                }
            }
        }

        public void AddDeletingActor(Actor actor)
        {
            _deletingActors.Add(actor, new SecondsTickTimer(actor.World.Game, 0.2f));
        }

        public bool IsDeletingActor(Actor actor)
        {
            return _deletingActors.ContainsKey(actor);
        }

        public void CancelAllPowers(Actor user)
        {
            _channeledSkills.RemoveAll(impl =>
            {
                if (impl.User == user && impl.IsChannelOpen)
                {
                    impl.CloseChannel();
                    return true;
                }
                return false;
            });

            _executingScripts.RemoveAll((script) => script.Script.User == user);
        }
    }
}
