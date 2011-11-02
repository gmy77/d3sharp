using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mooege.Common.MPQ.FileFormats;
using Mooege.Common.MPQ;
using Mooege.Core.GS.Common.Types.SNO;
using Mooege.Common.Helpers;

namespace Mooege.Core.Common.Items
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
