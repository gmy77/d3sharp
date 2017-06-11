/*
 * Copyright (C) 2011 - 2012 mooege project - http://www.mooege.org
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
using Mooege.Core.GS.Common.Types.SNO;
using Mooege.Core.GS.Common.Types.TagMap;
using Mooege.Core.GS.Common.Types.Math;

namespace Mooege.Common.MPQ.FileFormats
{
    [FileFormat(SNOGroup.MarkerSet)]
    public class MarkerSet : FileFormat
    {
        public Header Header { get; private set; }
        public List<Marker> Markers = new List<Marker>();
        public AABB AABB { get; private set; }
        public bool ContainsActorLocations { get; private set; }
        public string FileName { get; private set; }
        public int NLabel { get; private set; }
        public int SpecialIndexCount { get; private set; }
        public List<short> SpecialIndexList { get; private set; }
        public List<Circle> NoSpawns { get; private set; }

        public MarkerSet(MpqFile file)
        {
            var stream = file.Open();
            this.Header = new Header(stream);

            this.Markers = stream.ReadSerializedData<Marker>();

            stream.Position += (15 * 4);
            NoSpawns = stream.ReadSerializedData<Circle>();
            stream.Position += (14 * 4);
            this.AABB = new AABB(stream);
            int i0 = stream.ReadValueS32();
            if (i0 != 0 && i0 != 1)
                throw new System.Exception("Farmy thought this field is a bool, but apparently its not");
            this.ContainsActorLocations = i0 == 1;

            this.FileName = stream.ReadString(256, true);
            this.NLabel = stream.ReadValueS32();
            this.SpecialIndexCount = stream.ReadValueS32();
            this.SpecialIndexList = stream.ReadSerializedShorts();
            stream.Close();
        }
    }

    public class Marker : ISerializableData
    {
        public string Name { get; private set; }
        public MarkerType Type { get; private set; }
        public PRTransform PRTransform { get; private set; }
        public SNOHandle SNOHandle { get; private set; }
        public TagMap TagMap { get; private set; }
        public int MarkerLinksCount { get; private set; }
        public List<MarkerLink> MarkerLinks = new List<MarkerLink>();

        public void Read(MpqFileStream stream)
        {
            this.Name = stream.ReadString(128, true);
            this.Type = (MarkerType)stream.ReadValueS32();
            this.PRTransform = new PRTransform(stream);
            this.SNOHandle = new SNOHandle(stream);
            this.TagMap = stream.ReadSerializedItem<TagMap>();
            stream.Position += 8;
            this.MarkerLinksCount = stream.ReadValueS32();
            this.MarkerLinks = stream.ReadSerializedData<MarkerLink>();
            stream.Position += (3 * 4);
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}", Name, SNOHandle.Name);
        }
    }

    public class Circle : ISerializableData
    {
        public Vector2F Center { get; private set; }
        public float Radius { get; private set; }

        public void Read(MpqFileStream stream)
        {
            Center = new Vector2F(stream.ReadValueF32(), stream.ReadValueF32());
            Radius = stream.ReadValueF32();
        }
    }

    public class MarkerLink : ISerializableData
    {
        public string String1 { get; private set; }
        public string String2 { get; private set; }

        public void Read(MpqFileStream stream)
        {
            this.String1 = stream.ReadString(128, true);
            this.String2 = stream.ReadString(128, true);
        }
    }

    public enum MarkerType
    {
        Actor = 0,
        Light,

        AudioVolume = 4,
        AmbientSound = 6,
        Particle = 7,

        Encounter = 10,

        Script = 13,

        SubScenePosition = 16,

        MinimapMarker = 28,
        Event = 29,

        // don't blame me - farmy :-)
        GizmoLocationA = 50,
        GizmoLocationB = 51,
        GizmoLocationC = 52,
        GizmoLocationD = 53,
        GizmoLocationE = 54,
        GizmoLocationF = 55,
        GizmoLocationG = 56,
        GizmoLocationH = 57,
        GizmoLocationI = 58,
        GizmoLocationJ = 59,
        GizmoLocationK = 60,
        GizmoLocationL = 61,
        GizmoLocationM = 62,
        GizmoLocationN = 63,
        GizmoLocationO = 64,
        GizmoLocationP = 65,
        GizmoLocationQ = 66,
        GizmoLocationR = 67,
        GizmoLocationS = 68,
        GizmoLocationT = 69,
        GizmoLocationU = 70,
        GizmoLocationV = 71,
        GizmoLocationW = 72,
        GizmoLocationX = 73,
        GizmoLocationY = 74,
        GizmoLocationZ = 75,
    }
}