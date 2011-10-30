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
using Mooege.Common.MPQ.FileFormats;
using Mooege.Common.MPQ;
using Mooege.Core.GS.Common.Types.SNO;
using Mooege.Core.Common.Scripting;

namespace Mooege.Core.Common.Items
{
    static class AffixGenerator
    {
        public static readonly Logger Logger = LogManager.CreateLogger();

        private static Dictionary<int, Dictionary<int, AffixTable>> affixList = new Dictionary<int, Dictionary<int, AffixTable>>();

        static AffixGenerator()
        {
            foreach (var asset in MPQStorage.Data.Assets[SNOGroup.GameBalance].Values)
            {
                GameBalance data = asset.Data as GameBalance;
                if (data != null && data.Type == BalanceType.AffixList)
                {
                    foreach (var affixDef in data.Affixes)
                    {
                        if (affixDef.Name.Contains("REQ")) continue; // crashes the client // dark0ne

                        Dictionary<int, AffixTable> list;
                        if (!affixList.TryGetValue(affixDef.AffixFamily0, out list))
                        {
                            list = new Dictionary<int, AffixTable>();
                            affixList.Add(affixDef.AffixFamily0, list);
                        }
                        list.Add(StringHashHelper.HashItemName(affixDef.Name), affixDef);
                    }
                }
            }
        }

        public static void Generate(Item item, int affixesCount)
        {
            if (!Item.IsWeapon(item.ItemType) && !Item.IsArmor(item.ItemType) && !Item.IsAccessory(item.ItemType))
                return;

            ItemRandomHelper irh = item.RandomGenerator;

            var selected = affixList.OrderBy(x => RandomHelper.Next()).Take(affixesCount);
            foreach (var definitions in selected)
            {
                var bestDef = definitions.Value.Values.OrderByDescending(x => x.AffixLevel).Where(x => x.AffixLevel <= item.ItemLevel).FirstOrDefault();

                if (bestDef != null)
                {
                    Logger.Debug("Generating affix " + bestDef.Name + " (aLvl:" + bestDef.AffixLevel + ")");
                    item.AffixList.Add(new Affix(StringHashHelper.HashItemName(bestDef.Name)));
                    foreach (var effect in bestDef.AttributeSpecifier)
                    {
                        float result;
                        if (FormulaScript.Evaluate(effect.Formula.ToArray(), irh, out result))
                        {
                            var attr = GameAttribute.GameAttributeArray[effect.AttributeId] as GameAttributeF;
                            if (attr != null)
                            {
                                Logger.Debug("Randomized value for attribute " + attr.Name + " is " + result);
                                if (effect.SNOParam != -1)
                                    item.Attributes[attr, effect.SNOParam] += result;
                                else
                                    item.Attributes[attr] += result;
                            }
                        }
                    }
                }
            }
        }
    }
}
