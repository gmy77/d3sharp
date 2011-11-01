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
using System.Globalization;
using System.Threading;
using CrystalMpq;
using Mooege.Core.GS.Common.Types.SNO;

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
        }

        public void RunParser(Type parser, MpqFile file)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture; // Use invariant culture so that we don't hit pitfalls in non en/US systems with different number formats.
            this.Data = (FileFormat) Activator.CreateInstance(parser, new object[] {file});
        }
    }
}
