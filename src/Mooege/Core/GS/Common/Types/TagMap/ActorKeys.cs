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
using System.Reflection;

namespace Mooege.Core.GS.Common.Types.TagMap
{
    class ActorKeys
    {
        # region compile a dictionary to access keys from ids. If you need a readable name for a TagID, look up its key and get its name
        private static Dictionary<int, TagKey> tags = new Dictionary<int, TagKey>();

        public static TagKey GetKey(int index)
        {
            return tags.ContainsKey(index) ? tags[index] : null;
        }

        static ActorKeys()
        {
            foreach (FieldInfo field in typeof(ActorKeys).GetFields())
            {
                TagKey key = field.GetValue(null) as TagKey;
                key.Name = field.Name;
                tags.Add(key.ID, key);
            }
        }
        #endregion

        public static TagKeyInt TeamID = new TagKeyInt(65556);
        public static TagKeySNO Flippy = new TagKeySNO(65688);
        public static TagKeySNO Projectile = new TagKeySNO(66138);
        public static TagKeySNO Lore = new TagKeySNO(67331);

        public static TagKeySNO MinimapMarker = new TagKeySNO(458752);

        public static TagKeySNO FireEffectGroup = new TagKeySNO(74064);
        public static TagKeySNO ColdEffectGroup = new TagKeySNO(74065);
        public static TagKeySNO LightningEffectGroup = new TagKeySNO(74066);
        public static TagKeySNO PoisonEffectGroup = new TagKeySNO(74067);
        public static TagKeySNO ArcaneEffectGroup = new TagKeySNO(74068);

        public static TagKeySNO LifeStealEffectGroup = new TagKeySNO(74070);
        public static TagKeySNO ManaStealEffectGroup = new TagKeySNO(74071);
        public static TagKeySNO MagicFindEffectGroup = new TagKeySNO(74072);
        public static TagKeySNO GoldFindEffectGroup = new TagKeySNO(74073);
        public static TagKeySNO AttackEffectGroup = new TagKeySNO(74074);
        public static TagKeySNO CastEffectGroup = new TagKeySNO(74075);
        public static TagKeySNO HolyEffectGroup = new TagKeySNO(74076);
        public static TagKeySNO Spell1EffectGroup = new TagKeySNO(74077);
        public static TagKeySNO Spell2EffectGroup = new TagKeySNO(74078);
    }
}
