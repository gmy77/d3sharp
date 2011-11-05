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

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Mooege.Common.MPQ;
using Mooege.Core.GS.Common.Types.SNO;
using Mooege.Common.MPQ.FileFormats;
using Mooege.Core.GS.Markers;
namespace Mooege.Common.Helpers.Assets
{
    /// <summary>
    /// Helper for lores. Has dictionary with ActorSNOId->LoreSNOId.
    /// Dictionary is lazy-filled from Book item ctor and Monster's Die method
    /// </summary>
    public class LoreAssetHelper : AssetHelper
    {
        public static ConcurrentDictionary<int, int> Lores = new ConcurrentDictionary<int, int>();

        public static new List<Task> GetTasks(Data data)
        {
            return new List<Task>();
        }

        /// <summary>
        /// Returns lore's SNOId for item's SNOId. If item is not present in dictionary, it's added.
        /// </summary>
        /// <param name="itemSNOId"></param>
        /// <returns></returns>
        public static int GetLoreForItem(int itemSNOId)
        {
            if (!LoreAssetHelper.Lores.ContainsKey(itemSNOId))
            {
                AddLoreForItem(itemSNOId);
            }
            return Lores[itemSNOId];
        }

        /// <summary>
        /// Returns lore's SNOId for monster's actorSNOId. If item is not present in dictionary, it's added.
        /// </summary>
        /// <param name="itemSNOId"></param>
        /// <returns></returns>
        public static int GetLoreForMonster(int monsterSNOId)
        {
            if (!LoreAssetHelper.Lores.ContainsKey(monsterSNOId))
            {
                AddLoreForMonster(monsterSNOId);
            }
            return Lores[monsterSNOId];
        }

        /// <summary>
        /// Adds actorSNOId->LoreSNOId mapping if not present in dictionary for Item.
        /// If loreSNOId is not found (or item doesn't have lore), sets loreSNOId to -1 to prevent subsequent searches.
        /// </summary>
        /// <param name="itemSNOId"></param>
        public static void AddLoreForItem(int itemSNOId)
        {
            // lazy initialization of helper's value
            // find actor's asset
            var actorAssetPair = MPQStorage.Data.Assets[SNOGroup.Actor].FirstOrDefault(x => x.Value.SNOId == itemSNOId);
            // find lore tag
            var loreTag = (actorAssetPair.Value.Data as Mooege.Common.MPQ.FileFormats.Actor).TagMap.TagMapEntries.FirstOrDefault(z => z.TagID == (int)MarkerTagTypes.LoreSNOId);
            int loreSNOId = -1;
            if (loreTag != null)
            {
                loreSNOId = loreTag.Int2;
            }
            LoreAssetHelper.Lores.TryAdd(itemSNOId, loreSNOId);
        }

        /// <summary>
        /// Adds actorSNOId->LoreSNOId mapping if not present in dictionary for Monster.
        /// If loreSNOId is not found (or monster doesn't have lore), sets loreSNOId to -1 to prevent subsequent searches.
        /// </summary>
        /// <param name="monsterSNOId"></param>
        public static void AddLoreForMonster(int monsterSNOId)
        {
            // lazy initialization of helper's value
            // find monster's asset
            var monsterAssetPair = MPQStorage.Data.Assets[SNOGroup.Monster].FirstOrDefault(x => (x.Value.Data as Mooege.Common.MPQ.FileFormats.Monster).ActorSNO == monsterSNOId);
            int loreSNOId = -1;
            if (monsterAssetPair.Value != null)
            {
                loreSNOId = (monsterAssetPair.Value.Data as Mooege.Common.MPQ.FileFormats.Monster).SNOLore;
            }
            LoreAssetHelper.Lores.TryAdd(monsterSNOId, loreSNOId);
        }
    }
}
