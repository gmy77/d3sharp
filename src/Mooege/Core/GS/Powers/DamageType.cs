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
using Mooege.Core.GS.Common.Types.TagMap;

namespace Mooege.Core.GS.Powers
{
    public class DamageType
    {
        public enum HitEffectType : int
        {
            Physical = 0,
            Fire = 1,
            Lightning = 2,
            Cold = 3,
            Poison = 4,
            Arcane = 5,
            Holy = 6,
            UnknownFlicker = 7
        }

        public HitEffectType HitEffect;
        public int AttributeKey;  // GameAttributeMap key for a given damage type
        public TagKeyInt DeathAnimationTag;

        public static readonly DamageType Physical = new DamageType
        {
            HitEffect = HitEffectType.Physical,
            AttributeKey = 0,
            DeathAnimationTag = AnimationSetKeys.DeathDefault,
        };
        public static readonly DamageType Arcane = new DamageType
        {
            HitEffect = HitEffectType.Arcane,
            AttributeKey = 5,
            DeathAnimationTag = AnimationSetKeys.DeathArcane,
        };
        public static readonly DamageType Cold = new DamageType
        {
            HitEffect = HitEffectType.Cold,
            AttributeKey = 3,
            DeathAnimationTag = AnimationSetKeys.DeathCold,
        };
        public static readonly DamageType Fire = new DamageType
        {
            HitEffect = HitEffectType.Fire,
            AttributeKey = 1,
            DeathAnimationTag = AnimationSetKeys.DeathFire,
        };
        public static readonly DamageType Lightning = new DamageType
        {
            HitEffect = HitEffectType.Lightning,
            AttributeKey = 2,
            DeathAnimationTag = AnimationSetKeys.DeathLightning,
        };
        public static readonly DamageType Poison = new DamageType
        {
            HitEffect = HitEffectType.Poison,
            AttributeKey = 4,
            DeathAnimationTag = AnimationSetKeys.DeathPoison,
        };
        public static readonly DamageType Holy = new DamageType
        {
            HitEffect = HitEffectType.Holy,
            AttributeKey = 6,
            DeathAnimationTag = AnimationSetKeys.DeathHoly,
        };

        public static readonly DamageType[] AllTypes = new DamageType[]
        {
            Physical,
            Arcane,
            Cold,
            Fire,
            Lightning,
            Poison,
            Holy
        };
    }
}
