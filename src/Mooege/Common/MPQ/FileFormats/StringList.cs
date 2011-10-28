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
using CrystalMpq;
using Gibbed.IO;
using Mooege.Common.MPQ.FileFormats.Types;
using Mooege.Core.GS.Common.Types.SNO;

namespace Mooege.Common.MPQ.FileFormats
{
    [FileFormat(SNOGroup.StringList)]
    public class StringList : FileFormat
    {
        public Header Header { get; private set; }
        public List<StringTableEntry> StringTable;

        public StringList(MpqFile file)
        {
            MpqFileStream stream = file.Open();
            Header = new Header(stream);
            stream.Position += 12;
            StringTable = stream.ReadSerializedData<StringTableEntry>();
            stream.Close();
        }
    }

    public class StringTableEntry : ISerializableData
    {
        string Label;
        string Text;
        string Comment;
        string Speaker;
        int I0;
        int I1;
        int I2;

        public void Read(MpqFileStream stream)
        {
            stream.Position += 8;
            Label = stream.ReadSerializedString();
            stream.Position += 8;
            Text = stream.ReadSerializedString();
            stream.Position += 8;
            Comment = stream.ReadSerializedString();
            stream.Position += 8;
            Speaker = stream.ReadSerializedString();
            I0 = stream.ReadValueS32();
            I1 = stream.ReadValueS32();
            I2 = stream.ReadValueS32();
            stream.Position += 4;
        }
    }
}