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
using System.IO;
using System.Linq;
using CrystalMpq;
using CrystalMpq.Utility;
using Mooege.Common;
using Mooege.Common.Helpers;

namespace Mooege.Core.MPQ
{
    public static class MPQStorage
    {
        private static readonly Logger Logger = LogManager.CreateLogger();
        private readonly static string MpqRoot = Common.Storage.Config.Instance.MPQRoot;

        public static readonly List<string> MPQList;

        static MPQStorage()
        {
            MPQList = FileHelpers.GetFilesRecursive(MpqRoot, "*.mpq");
            var chain = new MPQPatchChain(GetMPQFile("CoreData.mpq"), "/base/d3-update-base-(?<version>.*?).MPQ");
        }

        public static string GetMPQFile(string name)
        {
            return MPQList.FirstOrDefault(file => file.Contains(name));
        }

        public static void Init()
        {
            
        }
    }
}
