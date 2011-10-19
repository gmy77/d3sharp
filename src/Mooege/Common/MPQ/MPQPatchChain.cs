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
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CrystalMpq;
using CrystalMpq.Utility;

namespace Mooege.Common.MPQ
{    
    public class MPQPatchChain
    {
        protected static readonly Logger Logger = LogManager.CreateLogger();
        protected readonly MpqFileSystem FileSystem = new MpqFileSystem();

        public string BaseMPQFile { get; private set; }
        public string PatchPattern { get; private set; }
        public readonly SortedDictionary<int, string> MPQFileList = new SortedDictionary<int, string>();

        protected MPQPatchChain(string baseMPQFile, string patchPattern=null)
        {
            var baseFile = MPQStorage.GetMPQFile(baseMPQFile);
            if (baseFile == null)
            {
                Logger.Error("Can not find base-mpq file: {0} for patch chain: {1}.", baseMPQFile, this.GetType().Name);
                return;
            }

            Logger.Info("Reading MPQ patch-chain: {0}", this.GetType().Name);

            this.BaseMPQFile = baseFile;
            this.PatchPattern = patchPattern;
            this.ConstructChain();
        }

        private void ConstructChain()
        {            
            MPQFileList.Add(0, this.BaseMPQFile); // add the base file as version 0.
            if (PatchPattern == null) return;

            /* match the mpq files for the patch chain */
            var patchRegex = new Regex(this.PatchPattern, RegexOptions.Compiled);
            foreach(var file in MPQStorage.MPQList)
            {
                var match = patchRegex.Match(file);
                if (!match.Success) continue;
                if (!match.Groups["version"].Success) continue;

                MPQFileList.Add(Int32.Parse(match.Groups["version"].Value), file);
            }

            /* add mpq's to mpq-file system */
            foreach(var pair in this.MPQFileList.Reverse())
            {
                this.FileSystem.Archives.Add(new MpqArchive(pair.Value));
            }
        }

        public List<string> FindMatchingFiles(string mask)
        {
            var list = new List<string>();
            foreach(var archive in this.FileSystem.Archives)
            {
                foreach(var file in archive.Files)
                {
                    if (!file.Name.Contains(mask)) continue;
                    if (list.Contains(file.Name)) continue;

                    list.Add(file.Name);
                }
            }

            return list;
        }
    }
}
