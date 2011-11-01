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
using CrystalMpq;
using Gibbed.IO;
using Mooege.Common.MPQ.FileFormats.Types;
using Mooege.Core.GS.Common.Types.Collision;
using Mooege.Core.GS.Common.Types.Math;
using Mooege.Core.GS.Common.Types.SNO;

namespace Mooege.Common.MPQ.FileFormats
{
    [FileFormat(SNOGroup.MarkerSet)]
    public class MarkerSet : FileFormat
    {
        public Header Header { get; private set; }
        public List<Marker> Markers = new List<Marker>();
        public AABB AABB { get; private set; }
        public int Int0 { get; private set; }
        public string FileName { get; private set; }
        public int NLabel { get; private set; }
        public int SpecialIndexCount { get; private set; }

        public MarkerSet(MpqFile file)
        {
            var stream = file.Open();
            this.Header = new Header(stream);

            var pointerMarkers = stream.GetSerializedDataPointer();
            this.Markers = stream.ReadSerializedData<Marker>(pointerMarkers, pointerMarkers.Size/208);

            stream.Position += (15 * 4);
            var pointerSpawns = stream.GetSerializedDataPointer();
            
            stream.Position += (14 * 4);
            this.AABB = new AABB(stream);
            this.Int0 = stream.ReadValueS32();

            this.FileName = stream.ReadString(256, true);

            this.NLabel = stream.ReadValueS32();
            SpecialIndexCount = stream.ReadValueS32();

            var pointerSpecialIndexList = stream.GetSerializedDataPointer();

            stream.Close();
        }
    }

    public class Marker : ISerializableData
    {
        public string Name { get; private set; }
        public int Int0 { get; private set; }
        public PRTransform PRTransform { get; private set; }
        public SNOName SNOName { get; private set; }
        public TagMap TagMap { get; private set; }
        public int IntTagMap { get; private set; }
        public int Int1 { get; private set; }
        public int Int2 { get; private set; }

        public void Read(MpqFileStream stream)
        {
            this.Name = stream.ReadString(128, true);
            this.Int0 = stream.ReadValueS32();
            this.PRTransform = new PRTransform(stream);
            this.SNOName = new SNOName(stream);

            this.TagMap = stream.ReadSerializedItem<TagMap>();

            // Un sure about these 3 ints, 010template isnt the same as snodata.xml - DarkLotus
            this.IntTagMap = stream.ReadValueS32();
            Int1 = stream.ReadValueS32();
            Int2 = stream.ReadValueS32();

            var pointerMarkerLinks = stream.GetSerializedDataPointer();
            stream.Position += (3 * 4);
        }
    }
}