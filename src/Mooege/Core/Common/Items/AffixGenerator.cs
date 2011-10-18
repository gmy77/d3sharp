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
using Mooege.Common.Helpers;
using Mooege.Net.GS.Message;
using Mooege.Common;
using System.Reflection;

namespace Mooege.Core.Common.Items
{
    static class AffixGenerator
    {
        public static readonly Logger Logger = LogManager.CreateLogger();

        public static void Generate(Item item, int affixesCount)
        {
            if (!Item.IsWeapon(item.ItemType) && !Item.IsArmor(item.ItemType) && !Item.IsAccessory(item.ItemType))
                return;
            if (affixesCount > AffixDefinition.Definitions.Length)
                affixesCount = AffixDefinition.Definitions.Length;

            ItemRandomHelper irh = new ItemRandomHelper(item.Attributes[GameAttribute.Seed]);
            irh.Next(); // 1 random is always skipped
            if(Item.IsArmor(item.ItemType))
                irh.Next(); // next value is used but unknown if armor
            irh.ReinitSeed();
            if(Item.IsWeapon(item.ItemType))
            {
                irh.Next(); // unknown
                irh.Next(); // unknown
            }
            IEnumerable<AffixDefinition> selected = AffixDefinition.Definitions.OrderBy(x => RandomHelper.Next()).Take(affixesCount);
            foreach (var definition in selected)
            {
                Logger.Debug("Generating affix " + definition.Name);
                item.AffixList.Add(new Affix(definition.AffixGbid));
                foreach (var effect in definition.Effects)
                {
                    if(effect.EffectValueType == AffixEffectValueType.Int)
                    {
                        uint r = irh.Next(effect.MinI, effect.MaxI);
                        Logger.Debug("Randomized value for attribute " + effect.EffectAttribute + " is " + r);
                        var attr = (GameAttributeF)typeof(GameAttribute).GetField(effect.EffectAttribute).GetValue(null);
                        item.Attributes[attr] += r;
                    }
                }
            }
        }
    }

    public enum AffixType
    {
        Prefix,
        Suffix
    }

    public enum AffixEffectValueType
    {
        Int,
        Float
    }

    class AffixDefinition
    {
        public static AffixDefinition[] Definitions = new AffixDefinition[] {
            new AffixDefinition("AttPrec I", AffixType.Prefix).AddEffect("Attack", 1, 6).AddEffect("Precision", 1, 6),
            new AffixDefinition("AttPrec II", AffixType.Prefix).AddEffect("Attack", 6, 16).AddEffect("Precision", 6, 16),

            new AffixDefinition("Att 1", AffixType.Suffix).AddEffect("Attack", 1, 8),
        };

        public string Name;
        public int AffixGbid;
        public AffixType Type;
        public List<AffixEffect> Effects;

        public class AffixEffect
        {
            public string EffectAttribute;
            public AffixEffectValueType EffectValueType;
            public int MinI;
            public int MaxI;
            public float MinF;
            public float MaxF;

            public AffixEffect(string effectAttr, int min, int max)
            {
                EffectAttribute = effectAttr;
                EffectValueType = AffixEffectValueType.Int;
                MinI = min;
                MaxI = max;
            }

            public AffixEffect(string effectAttr, float min, float max)
            {
                EffectAttribute = effectAttr;
                EffectValueType = AffixEffectValueType.Float;
                MinF = min;
                MaxF = max;
            }
        }

        public AffixDefinition(string a, AffixType t)
        {
            Name = a;
            AffixGbid = StringHashHelper.HashItemName(Name);
            Type = t;
            Effects = new List<AffixEffect>();
        }

        public AffixDefinition AddEffect(string effectAttr, int min, int max)
        {
            Effects.Add(new AffixEffect(effectAttr, min, max));
            return this;
        }

        public AffixDefinition AddEffect(string effectAttr, float min, float max)
        {
            Effects.Add(new AffixEffect(effectAttr, min, max));
            return this;
        }
    }
}
