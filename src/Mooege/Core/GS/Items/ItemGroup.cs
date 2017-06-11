/*
 * Copyright (C) 2011 - 2012 mooege project - http://www.mooege.org
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
using Mooege.Common.Helpers.Hash;
using Mooege.Common.MPQ.FileFormats;
using Mooege.Common.MPQ;
using Mooege.Core.GS.Common.Types.SNO;

namespace Mooege.Core.GS.Items
{
    public static class ItemGroup
    {
        private static Dictionary<int, ItemTypeTable> ItemTypes = new Dictionary<int, ItemTypeTable>();

        static ItemGroup()
        {
            foreach (var asset in MPQStorage.Data.Assets[SNOGroup.GameBalance].Values)
            {
                GameBalance data = asset.Data as GameBalance;
                if (data != null && data.Type == BalanceType.ItemTypes)
                {
                    foreach (var itemTypeDef in data.ItemType)
                    {
                        ItemTypes.Add(itemTypeDef.Hash, itemTypeDef);
                    }
                }
            }
        }

        public static List<ItemTypeTable> HierarchyToList(ItemTypeTable itemType)
        {
            List<ItemTypeTable> result = new List<ItemTypeTable>();
            var curType = itemType;
            if (curType != null)
            {
                result.Add(curType);
                while (curType.ParentType != -1)
                {
                    curType = ItemTypes[curType.ParentType];
                    result.Add(curType);
                }
            }
            return result;
        }

        public static List<int> HierarchyToHashList(ItemTypeTable itemType)
        {
            List<int> result = new List<int>();
            var types = HierarchyToList(itemType);
            foreach (var type in types)
            {
                result.Add(type.Hash);
            }
            return result;
        }

        public static List<int> SubTypesToHashList(string name)
        {
            List<int> result = new List<int>();
            ItemTypeTable rootType = FromString(name);
            if (rootType != null)
            {
                result.Add(rootType.Hash);
                for (int i = 0; i < result.Count; ++i)
                {
                    foreach (var type in ItemTypes.Values)
                        if (type.ParentType == result[i])
                            result.Add(type.Hash);
                }
            }
            return result;
        }

        public static ItemTypeTable FromString(string name)
        {
            int hash = StringHashHelper.HashItemName(name);
            return FromHash(hash);
        }

        public static ItemTypeTable FromHash(int hash)
        {
            ItemTypeTable result = null;
            if (ItemTypes.TryGetValue(hash, out result))
            {
                return result;
            }
            return null;
        }

        public static bool IsSubType(ItemTypeTable type, string rootTypeName)
        {
            return IsSubType(type, StringHashHelper.HashItemName(rootTypeName));
        }

        public static bool IsSubType(ItemTypeTable type, int rootTypeHash)
        {
            if (type == null)
                return false;

            if (type.Hash == rootTypeHash)
                return true;
            var curType = type;
            while (curType.ParentType != -1)
            {
                curType = ItemTypes[curType.ParentType];
                if (curType.Hash == rootTypeHash)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool Is2H(ItemTypeTable type)
        {
            return (type.Array[0] & 0x400) != 0;
        }
    }
}
