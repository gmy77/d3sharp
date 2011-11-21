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
            List<Actor> emptyKeys = new List<Actor>();

            foreach (var buffKV in _buffs)
            {
                buffKV.Value.RemoveAll((buff) =>
                {
                    bool removing = buff.Update();
                    if (removing)
                        buff.Remove();

                    return removing;
                });

                if (buffKV.Value.Count == 0)
                    emptyKeys.Add(buffKV.Key);
            }

            // clean up empty buff lists
            foreach (var key in emptyKeys)
                _buffs.Remove(key);
        }

        public bool AddBuff(Actor user, Actor target, Buff buff)
        {
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

        private bool _AddBuff(Buff buff)
        {
            // look up or create a buff list for the target, then add/stack the buff according to its class type.

            // the logic is a bit more complex that it seems necessary because we ensure the buff appears in the
            // active buff list before calling Apply(), if Apply() fails we undo adding it. This allows buffs to
            // recursively add/stack more of their own buff type without worrying about overwriting existing buffs.
            if (_buffs.ContainsKey(buff.Target))
            {
                Type buffType = buff.GetType();
                Buff existingBuff = _buffs[buff.Target].FirstOrDefault((b) => b.GetType() == buffType);
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
    }
}
