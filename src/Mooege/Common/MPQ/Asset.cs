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

namespace Mooege.Common.MPQ
{
    public class Asset
    {
        public SNOGroup Group {get; private set;}
        public Int32 SNOId {get; private set;}
        public string Name {get; private set;}
        public string FileName {get; private set;}
        public FileFormat Data {get; private set;}

        public Asset(SNOGroup group, Int32 snoId, string name)
        {
            this.Data = null;
            this.Group = group;
            this.SNOId = snoId;
            this.Name = name;
            this.FileName = group + "\\" + this.Name + FileExtensions.Extensions[(int)group];

            this.Load();
        }

        private void Load()
        {
            if (!MPQStorage.Data.AssetFormats.ContainsKey(this.Group)) return;
            var formatType = MPQStorage.Data.AssetFormats[this.Group];
            
            var file = MPQStorage.Data.FileSystem.FindFile(this.FileName);
            if (file == null || file.Size < 10) return;

            this.Data = (FileFormat) Activator.CreateInstance(formatType, new object[] {file});
        }
    }
}
