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
using System.Text.RegularExpressions;
using Mooege.Common;

namespace Mooege.Core.MPQ
{
    public class MPQPatchChain
    {
        private static readonly Logger Logger = LogManager.CreateLogger();

        public string BaseMPQFile { get; private set; }
        public string PatchPattern { get; private set; }
        public readonly SortedDictionary<int, string> MPQFileList = new SortedDictionary<int, string>();

        public MPQPatchChain(string baseMPQFile, string patchPattern=null)
        {
            this.BaseMPQFile = baseMPQFile;
            this.PatchPattern = patchPattern;
            this.ConstructChain();
        }

        private void ConstructChain()
        {
            MPQFileList.Add(0, this.BaseMPQFile); // add the base file as version 0.

            if (PatchPattern == null) return;
            var patchRegex = new Regex(this.PatchPattern, RegexOptions.Compiled);
            foreach(var file in MPQStorage.MPQList)
            {
                var match = patchRegex.Match(file);
                if (!match.Success) continue;
                if (!match.Groups["version"].Success) continue;

                MPQFileList.Add(Int32.Parse(match.Groups["version"].Value), file);
            }
        }
    }
}
