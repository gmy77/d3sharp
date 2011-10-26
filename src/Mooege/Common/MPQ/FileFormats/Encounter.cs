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
using System.Collections.Generic;

namespace Mooege.Common.MPQ.FileFormats
{
    [FileFormat(SNOGroup.Encounter)]
    public class Encounter : FileFormat
    {
        public Header Header { get; private set; }
        public int snoSpawn { get; private set; }
        List<EncounterSpawnOptions> Spawnoptions = new List<EncounterSpawnOptions>();
        public Encounter(MpqFile file)
        {
            var stream = file.Open();
            this.Header = new Header(stream);
            this.snoSpawn = stream.ReadValueS32();
            stream.Position += (2 * 4);// pad 2 int
            this.Spawnoptions = stream.ReadSerializedData<EncounterSpawnOptions>();
            stream.Close();
        }
    }

    public class EncounterSpawnOptions : ISerializableData
    {
        public int snoSpawn { get; private set; }
        public int i0 { get; private set; }
        public int i1 { get; private set; }
        public void Read(MpqFileStream stream)
        {
            this.snoSpawn = stream.ReadValueS32();
            this.i0 = stream.ReadValueS32();
            this.i1 = stream.ReadValueS32();
        }
    }
}
