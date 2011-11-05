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
    }
}
