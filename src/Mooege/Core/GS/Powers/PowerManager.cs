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
        
        // HACK: used for item spawn helper in StartPower()
        private bool _spawnedHelperItems = false;

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
                    mon.Attributes[GameAttribute.Hitpoints_Max_Total] = 5f;
                    mon.Attributes[GameAttribute.Hitpoints_Max] = 5f;
                    mon.Attributes[GameAttribute.Hitpoints_Total_From_Level] = 0f;
                    mon.Attributes[GameAttribute.Hitpoints_Cur] = 5f;
                    mon.Attributes[GameAttribute.Attacks_Per_Second_Total] = 1.0f;
                    mon.Attributes[GameAttribute.Damage_Weapon_Min_Total, 0] = 5f;
                    mon.Attributes[GameAttribute.Damage_Weapon_Delta_Total, 0] = 7f;
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
                return RunPower(user, implementation, target, targetPosition, targetMessage);
            }
            else
            {
                // no power script is available, but try to play the cast effects
                var efgTag = Mooege.Core.GS.Common.Types.TagMap.PowerKeys.CastingEffectGroup_Male;
                var tagmap = PowerTagHelper.FindTagMapWithKey(powerSNO, efgTag);
                if (tagmap != null)
                    user.PlayEffectGroup(tagmap[efgTag].Id);

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
