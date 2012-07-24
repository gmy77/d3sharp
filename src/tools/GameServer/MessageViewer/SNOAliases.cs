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
using System.Text;
using System.IO;
using Mooege.Common.MPQ;
using Mooege.Core.GS.Common.Types.SNO;
using System.Linq;

namespace GameMessageViewer
{
    class SNOAliases
    {
        public static string GetAlias(int sno)
        {
            foreach (var group in MPQStorage.Data.Assets.Values)
                if (group.ContainsKey(sno))
                    return Path.GetFileName(group[sno].FileName);

            return "";
        }

        public static string GetGroup(int uhash)
        {
            var dic = MPQStorage.Data.Assets[SNOGroup.Globals].First().Value.Data as Mooege.Common.MPQ.FileFormats.Globals;
            //if (dic.ActorGroup.ContainsKey(uhash))
            //    return dic.ActorGroup[uhash].S0;

            return "";
        }

    }
}
