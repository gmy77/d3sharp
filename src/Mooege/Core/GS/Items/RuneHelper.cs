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
using System.Linq;
using Mooege.Common.MPQ;
using Mooege.Common.MPQ.FileFormats;

namespace Mooege.Core.GS.Items
{
    public static class RuneHelper
    {
        /// <summary>
        /// Dictionary with PowerSNOs as keys and their indexes as values
        /// </summary>
        private readonly static Dictionary<int, int> PowerToRuneIndexMap = new Dictionary<int, int>();

        static RuneHelper()
        {
            foreach (var entry in MPQStorage.Data.Assets[Common.Types.SNO.SNOGroup.SkillKit].Values)
            {
                if (entry.Data == null) continue;

                var skillKit = entry.Data as SkillKit;
                for (int i = 0; i < skillKit.ActiveSkillEntries.Count; i++)
                {
                    PowerToRuneIndexMap.Add(skillKit.ActiveSkillEntries[i].SNOPower, i);
                }
            }
        }

        /// <summary>
        /// Returns index of Power. Needed for visual feedback on socketting skills.
        /// </summary>
        /// <param name="powerSNOId"></param>
        /// <returns></returns>
        public static int GetRuneIndexForPower(int powerSNOId)
        {
            if (!PowerToRuneIndexMap.Keys.Contains(powerSNOId))
                return -1;

            return PowerToRuneIndexMap[powerSNOId];
        }
    }
}
