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
using Mooege.Core.GS.Common.Types.SNO;

namespace Mooege.Common.MPQ.FileFormats
{
    [FileFormat(SNOGroup.PhysMesh)]
    public class PhysMesh : FileFormat
    {
        public Header Header { get; private set; }
        public int I0 { get; private set; }
        public int CollisionMeshCount { get; private set; }
        public List<CollisionMesh> CollisionMeshes { get; private set; }
        public int I1 { get; private set; }
        public string S0 { get; private set; }
        public string S1 { get; private set; }

        public PhysMesh(MpqFile file)
        {
            var stream = file.Open();
            this.Header = new Header(stream);
            this.I0 = stream.ReadValueS32();
            this.CollisionMeshCount = stream.ReadValueS32();
            this.CollisionMeshes = stream.ReadSerializedData<CollisionMesh>();
            stream.Position += 12;
            this.S0 = stream.ReadString(256);
            this.S1 = stream.ReadString(256);
            this.I1 = stream.ReadValueS32();
            stream.Close();
        }
    }

    public class CollisionMesh : ISerializableData
    {
        public Float3 F0 { get; private set; }
        public Float3 F1 { get; private set; }
        public Float3 F2 { get; private set; }
        public int I0 { get; private set; }
        public int I1 { get; private set; }
        public int I2 { get; private set; }
        public int I3 { get; private set; }
        public int I4 { get; private set; }
        public int I5 { get; private set; }
        public List<Float3> F3 { get; private set; }
        public List<MeshTriangle> DominoTriangles { get; private set; }
        public List<MeshNode> DominoNodes { get; private set; }
        public List<MeshEdge> DominoEdges { get; private set; }
        public int I6 { get; private set; }
        public int I7 { get; private set; }

        public void Read(MpqFileStream stream)
        {
            this.F0 = new Float3(stream);
            this.F1 = new Float3(stream);
            this.F2 = new Float3(stream);
            stream.Position += 36;
            this.I0 = stream.ReadValueS32();
            this.I1 = stream.ReadValueS32();
            this.I2 = stream.ReadValueS32();
            this.I3 = stream.ReadValueS32();
            this.I4 = stream.ReadValueS32();
            this.I5 = stream.ReadValueS32();
            this.F3 = stream.ReadSerializedData<Float3>();
            this.DominoTriangles = stream.ReadSerializedData<MeshTriangle>();
            this.DominoNodes = stream.ReadSerializedData<MeshNode>();
            this.DominoEdges = stream.ReadSerializedData<MeshEdge>();
            this.I6 = stream.ReadValueS32();
            this.I7 = stream.ReadValueS32();
        }
    }

    public class Float3 : ISerializableData
    {
        public float F0 { get; private set; }
        public float F1 { get; private set; }
        public float F2 { get; private set; }

        public Float3() { }

        public Float3(MpqFileStream stream)
        {
            F0 = stream.ReadValueF32();
            F1 = stream.ReadValueF32();
            F2 = stream.ReadValueF32();
        }

        public void Read(MpqFileStream stream)
        {
            F0 = stream.ReadValueF32();
            F1 = stream.ReadValueF32();
            F2 = stream.ReadValueF32();
        }
    }

    public class MeshTriangle : ISerializableData
    {
        public int I0 { get; private set; }
        public int I1 { get; private set; }
        public int I2 { get; private set; }
        public int I3 { get; private set; }
        public int I4 { get; private set; }
        public int I5 { get; private set; }
        public short I6 { get; private set; }

        public void Read(MpqFileStream stream)
        {
            I0 = stream.ReadValueS32();
            I1 = stream.ReadValueS32();
            I2 = stream.ReadValueS32();
            I3 = stream.ReadValueS32();
            I4 = stream.ReadValueS32();
            I5 = stream.ReadValueS32();
            I6 = stream.ReadValueS16();
        }
    }

    public class MeshEdge : ISerializableData
    {
        public int I0 { get; private set; }
        public int I1 { get; private set; }
        public int I2 { get; private set; }
        public int I3 { get; private set; }
        public int I4 { get; private set; }

        public void Read(MpqFileStream stream)
        {
            I0 = stream.ReadValueS32();
            I1 = stream.ReadValueS32();
            I2 = stream.ReadValueS32();
            I3 = stream.ReadValueS32();
            I4 = stream.ReadValueS32();
        }
    }

    public class MeshNode : ISerializableData
    {
        public int I0 { get; private set; }
        public short I1 { get; private set; }
        public short I2 { get; private set; }
        public sbyte B1 { get; private set; }
        public sbyte B2 { get; private set; }

        public void Read(MpqFileStream stream)
        {
            I0 = stream.ReadValueS32();
            I1 = stream.ReadValueS16();
            I2 = stream.ReadValueS16();
            B1 = stream.ReadValueS8();
            B2 = stream.ReadValueS8();
        }
    }
}
