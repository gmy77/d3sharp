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
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Mooege.Common.MPQ;
using Mooege.Core.GS.Common.Types.SNO;
using Mooege.Common.MPQ.FileFormats;
namespace Mooege.Common.Helpers.Assets
{
    /// <summary>
    /// Helper for socketting spells. Has dictionary with PowerSNO->index
    /// </summary>
    public class SpellRuneIndexHelper : AssetHelper
    {
        /// <summary>
        /// Dictionary with PowerSNOs as keys and their indexes as values
        /// </summary>
        public static ConcurrentDictionary<int, int> PowerToRuneIndex = new ConcurrentDictionary<int, int>();

        /// <summary>
        /// Returns index of Power. Needed for visual feedback on socketting skills
        /// </summary>
        /// <param name="PowerSNOId"></param>
        /// <returns></returns>
        public static int GetRuneIndex(int PowerSNOId)
        {
            return PowerToRuneIndex[PowerSNOId];
        }

        public static new List<Task> GetTasks(Data data)
        {
            List<Task> tasks = new List<Task>();
            foreach (var key in data.Assets[SNOGroup.SkillKit].Keys)
            {
                Asset asset = new Asset(SNOGroup.SkillKit, -1, "");
                if (data.Assets[SNOGroup.SkillKit].TryGetValue(key, out asset))
                {
                    tasks.Add(new Task(() => AddHelperValue(asset)));
                }
            }
            return tasks;
        }

        public static new void AddHelperValue(Asset asset)
        {
            var activeSkillEntries = (asset.Data as SkillKit).ActiveSkillEntries;
            for (int i = 0; i < activeSkillEntries.Count; i++)
            {
                PowerToRuneIndex.AddOrUpdate(activeSkillEntries[i].SNOPower, i, (key, oldValue) => i);
            }
        }
    }
}
