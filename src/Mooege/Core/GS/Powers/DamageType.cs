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

namespace Mooege.Core.GS.Powers
{
    public class DamageType
    {
        public enum HitEffectType : int
        {
            Blood = 0,
            Fire = 1,
            Lightning = 2,
            Cold = 3,
            Poison = 4,
            Arcane = 5,
            Holy = 6,
            UnknownFlicker = 7
        }

        public HitEffectType HitEffect;

        public static readonly DamageType Physical = new DamageType { HitEffect = HitEffectType.Blood };
        public static readonly DamageType Arcane = new DamageType { HitEffect = HitEffectType.Arcane };
        public static readonly DamageType Cold = new DamageType { HitEffect = HitEffectType.Cold };
        public static readonly DamageType Fire = new DamageType { HitEffect = HitEffectType.Fire };
        public static readonly DamageType Lightning = new DamageType { HitEffect = HitEffectType.Lightning };
        public static readonly DamageType Poison = new DamageType { HitEffect = HitEffectType.Poison };
        public static readonly DamageType Holy = new DamageType { HitEffect = HitEffectType.Holy };
    }
}
