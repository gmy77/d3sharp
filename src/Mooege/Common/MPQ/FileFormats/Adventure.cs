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

using CrystalMpq;
using Gibbed.IO;
using Mooege.Common.MPQ.FileFormats.Types;

namespace Mooege.Common.MPQ.FileFormats
{
    [FileFormat(SNOGroup.Adventure)]
    public class Adventure : FileFormat
    {
        public Header Header;
        public int snoSymbolActor;
        float f0, f1, f2, f3;
        int snoMarkerSet;
        public Adventure(MpqFile file)
        {
            var stream = file.Open();
            this.Header = new Types.Header(stream);
            this.snoSymbolActor = stream.ReadValueS32();
            this.f0 = stream.ReadValueF32();
            this.f1 = stream.ReadValueF32();
            this.f2 = stream.ReadValueF32();
            this.f3 = stream.ReadValueF32();
            this.snoMarkerSet = stream.ReadValueS32();
            stream.Close();
        }
    }
}
