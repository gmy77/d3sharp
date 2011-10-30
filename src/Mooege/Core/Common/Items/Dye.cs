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
using Mooege.Common;
using Mooege.Net.GS.Message;
using System.Diagnostics;

namespace Mooege.Core.Common.Items
{

    /// <summary>
    /// This Class handles the ColorValue of Dye Items
    ///
    /// TODO: need to implement Dye Remover and invisible Dye
    /// </summary>
    public static class DyeColor
    {

        public static readonly Logger Logger = LogManager.CreateLogger();

        private static Dictionary<int, int> DyeColorMap = new Dictionary<int,int>();

        static DyeColor()
        {
            DyeColorMap.Add(StringHashHelper.HashItemName("DYE_01"),18);  // Tanner Dye
            DyeColorMap.Add(StringHashHelper.HashItemName("DYE_02"),20);  // Pale Dye
            DyeColorMap.Add(StringHashHelper.HashItemName("DYE_03"),12);  // Winter Dye
            DyeColorMap.Add(StringHashHelper.HashItemName("DYE_04"),11);  // Aquatic Dye
            DyeColorMap.Add(StringHashHelper.HashItemName("DYE_05"),15);  // Cardinal Dye
            DyeColorMap.Add(StringHashHelper.HashItemName("DYE_06"),17);  // Ranger Dye
            DyeColorMap.Add(StringHashHelper.HashItemName("DYE_07"),13);  // Rogue Dye
            DyeColorMap.Add(StringHashHelper.HashItemName("DYE_08"),19);  // Desert Dye
            DyeColorMap.Add(StringHashHelper.HashItemName("DYE_09"), 9);  // Autumm Dye
            DyeColorMap.Add(StringHashHelper.HashItemName("DYE_10"), 4);  // Lovley Dye
            DyeColorMap.Add(StringHashHelper.HashItemName("DYE_11"), 7);  // Summer Dye
            DyeColorMap.Add(StringHashHelper.HashItemName("DYE_12"),16);  // Spring Dye
            DyeColorMap.Add(StringHashHelper.HashItemName("DYE_13"), 6);  // Elegant Dye
            DyeColorMap.Add(StringHashHelper.HashItemName("DYE_14"),14);  // Forester Dye
            DyeColorMap.Add(StringHashHelper.HashItemName("DYE_15"),10);  // Mariner Dye
            DyeColorMap.Add(StringHashHelper.HashItemName("DYE_16"), 8);  // Golden Dye
            DyeColorMap.Add(StringHashHelper.HashItemName("DYE_17"), 5);  // Royal Dye
            DyeColorMap.Add(StringHashHelper.HashItemName("DYE_18"), 2);  // Infernal Dye
            DyeColorMap.Add(StringHashHelper.HashItemName("DYE_19"), 3);  // Purity Dye
            DyeColorMap.Add(StringHashHelper.HashItemName("DYE_20"), 1);  // Abyssal Dye
        }

        private static int ColorValue(Item dye)
        {
            if (DyeColorMap.ContainsKey(dye.GBHandle.GBID))
            {
                int colorValue = DyeColorMap[dye.GBHandle.GBID];
                if (colorValue > -1)
                {
                    return colorValue;
                }
            }

            Logger.Error("Colorvalue couldn't be retrieved for item {0}", dye.DynamicID);
            return 0;
        }

        public static void DyeItem(Item dye, Item dyeable)
        {
            Debug.Assert(dye != null);
            Debug.Assert(Item.IsDye(dye.ItemType));
            Debug.Assert(dyeable != null);

            dyeable.Attributes[GameAttribute.DyeType] = DyeColor.ColorValue(dye);
        }
    }
}
