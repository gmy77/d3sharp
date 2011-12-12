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
using Mooege.Core.GS.Common.Types.TagMap;
using Mooege.Common.MPQ.FileFormats;
using Mooege.Common.MPQ;
using Mooege.Core.GS.Common.Types.SNO;

namespace Mooege.Core.GS.Powers
{
    public class PowerTagHelper
    {
        public static TagKeyScript GenerateTagForScriptFormula(int SF_N)
        {
            return new TagKeyScript(266496 + 256 * (SF_N / 10) + 16 * (SF_N % 10));
        }

        public static TagMap FindTagMapWithKey(int powerSNO, TagKey key)
        {
            Power power = (Power)MPQStorage.Data.Assets[SNOGroup.Power][powerSNO].Data;

            // TODO: figure out which tagmaps to search and in what order, eventually will probably
            // have to reorder them based on whether PvPing or not.
            TagMap[] tagMaps = new TagMap[]
            {
                power.Powerdef.GeneralTagMap,
                power.Powerdef.TagMap,
                power.Powerdef.ContactTagMap0,
                power.Powerdef.ContactTagMap1,
                power.Powerdef.ContactTagMap2,
                power.Powerdef.ContactTagMap3,
                power.Powerdef.PVPGeneralTagMap,
                power.Powerdef.PVPContactTagMap0,
                power.Powerdef.PVPContactTagMap1,
                power.Powerdef.PVPContactTagMap2,
                power.Powerdef.PVPContactTagMap3,
            };

            foreach (TagMap tagmap in tagMaps)
            {
                if (tagmap.ContainsKey(key))
                    return tagmap;
            }

            return null;
        }
    }
}
