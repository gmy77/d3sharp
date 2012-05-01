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
using Mooege.Core.GS.Actors;

namespace Mooege.Core.GS.Powers
{
    public class BuffManager
    {
        private Dictionary<Actor, List<Buff>> _buffs = new Dictionary<Actor, List<Buff>>();

        public void Update()
        {
            // make copy of keys as the dictionary will be modified during update/cleaning
            Actor[] keys = _buffs.Keys.ToArray();

            // update buffs and mark finished ones as removed
            foreach (Actor target in keys)
                _RemoveBuffsIf(target, buff => buff.Update());

            // clean up removed buffs
            foreach (Actor target in keys)
            {
                _buffs[target].RemoveAll(buff => buff == null);
                if (_buffs[target].Count == 0)
                    _buffs.Remove(target);
            }
        }

        public bool AddBuff(Actor user, Actor target, Buff buff)
        {
            if (user.World == null || target.World == null) return false;

            buff.User = user;
            buff.Target = target;
            buff.World = target.World;

            // try to load in power sno from class attribute first, then try parent class (if there is one)
            Type buffType = buff.GetType();
            int powerSNO = ImplementsPowerSNO.GetPowerSNOForClass(buffType);
            if (powerSNO != -1)
            {
                buff.PowerSNO = powerSNO;
            }
            else if (buffType.IsNested)
            {
                powerSNO = ImplementsPowerSNO.GetPowerSNOForClass(buffType.DeclaringType);
                if (powerSNO != -1)
                    buff.PowerSNO = powerSNO;
            }

            buff.Init();

            return _AddBuff(buff);
        }

        public void RemoveBuffs(Actor target, Type buffClass)
        {
            if (!_buffs.ContainsKey(target)) return;

            _RemoveBuffsIf(target, buff => buff.GetType() == buffClass);
        }

        public void RemoveBuffs(Actor target, int powerSNO)
        {
            if (!_buffs.ContainsKey(target)) return;

            _RemoveBuffsIf(target, buff => buff.PowerSNO == powerSNO);
        }

        public void RemoveAllBuffs(Actor target)
        {
            if (!_buffs.ContainsKey(target)) return;

            _RemoveBuffsIf(target, buff => true);
        }

        public T GetFirstBuff<T>(Actor target) where T : Buff
        {
            if (!_buffs.ContainsKey(target)) return null;

            Buff buff = _buffs[target].FirstOrDefault(b => b != null && b.GetType() == typeof(T));
            if (buff != null)
                return (T)buff;
            else
                return null;
        }

        public Buff[] GetAllBuffs(Actor target)
        {
            if (!_buffs.ContainsKey(target)) return null;
            return _buffs[target].Where(b => b != null).ToArray();
        }

        public bool HasBuff<T>(Actor target) where T : Buff
        {
            return GetFirstBuff<T>(target) != null;
        }

        public void SendTargetPayload(Actor target, Payloads.Payload payload)
        {
            if (_buffs.ContainsKey(target))
            {
                List<Buff> buffs = _buffs[target];
                int buffCount = buffs.Count;
                for (int i = 0; i < buffCount; ++i)
                {
                    if (buffs[i] != null)
                        buffs[i].OnPayload(payload);
                }
            }
        }

        private bool _AddBuff(Buff buff)
        {
            // look up or create a buff list for the target, then add/stack the buff according to its class type.

            // the logic is a bit more complex that it seems necessary because we ensure the buff appears in the
            // active buff list before calling Apply(), if Apply() fails we undo adding it. This allows buffs to
            // recursively add/stack more of their own buff type without worrying about overwriting existing buffs.
            if (_buffs.ContainsKey(buff.Target))
            {
                Type buffType = buff.GetType();
                Buff existingBuff = _buffs[buff.Target].FirstOrDefault(b => b != null && b.GetType() == buffType);
                if (existingBuff != null)
                {
                    if (existingBuff.Stack(buff))
                        return true;
                    // buff is non-stacking, just add normally
                }

                _buffs[buff.Target].Add(buff);
                if (buff.Apply())
                {
                    return true;
                }
                else
                {
                    _buffs[buff.Target].Remove(buff);
                    return false;
                }
            }
            else
            {
                var keyBuffs = new List<Buff>();
                keyBuffs.Add(buff);
                _buffs[buff.Target] = keyBuffs;
                if (buff.Apply())
                {
                    return true;
                }
                else
                {
                    _buffs.Remove(buff.Target);
                    return false;
                }
            }
        }

        private void _RemoveBuffsIf(Actor target, Func<Buff, bool> pred)
        {
            List<Buff> buffs = _buffs[target];
            int buffCount = buffs.Count;
            for (int i = 0; i < buffCount; ++i)
            {
                if (buffs[i] != null)
                {
                    if (pred(buffs[i]))
                    {
                        if (buffs[i] != null)
                        {
                            buffs[i].Remove();
                            buffs[i] = null;
                        }
                    }
                }
            }
        }
    }
}
