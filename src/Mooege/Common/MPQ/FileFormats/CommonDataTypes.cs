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

using System.Text;
using CrystalMpq;
using Mooege.Common.Extensions;
using Mooege.Net.GS.Message.Fields;

namespace Mooege.Common.MPQ.FileFormats
{
    public interface ISerializableData
    {
        void Read(MpqFileStream stream);
    }

    public struct SerializableDataPointer
    {
        public readonly int Offset;
        public readonly int Size;

        public SerializableDataPointer(int offset, int size)
        {
            this.Offset = offset;
            this.Size = size;
        }
    }

    public class Header
    {
        public int DeadBeef;
        public int SnoType;
        public int Unknown1, Unknown2, Unknown3, Unknown4;
        public int SNOId;

        public Header(MpqFileStream stream)
        {
            this.DeadBeef = stream.ReadInt32();
            this.SnoType = stream.ReadInt32();
            this.Unknown1 = stream.ReadInt32();
            this.Unknown2 = stream.ReadInt32();
            this.SNOId = stream.ReadInt32();
            this.Unknown3 = stream.ReadInt32();
            this.Unknown4 = stream.ReadInt32();
        }
    }

    public class Vector2D
    {
        public readonly int Field0, FIeld1;

        public Vector2D(MpqFileStream stream)
        {
            Field0 = stream.ReadInt32();
            FIeld1 = stream.ReadInt32();
        }
    }

    // Below this was auto generated from snodata.xml by BoyC
    public class Quaternion
    {
        public float Float0;
        public Vector3D Vector3D;

        public Quaternion(MpqFileStream stream)
        {
            Float0 = stream.ReadFloat();
            Vector3D = new Vector3D(stream.ReadFloat(), stream.ReadFloat(), stream.ReadFloat());
        }
    }

    public class PRTransform
    {
        public Quaternion Q;
        public Vector3D V;

        public PRTransform(MpqFileStream stream)
        {
            Q = new Quaternion(stream);
            V = new Vector3D(stream.ReadFloat(), stream.ReadFloat(), stream.ReadFloat());
        }
    }

    public class AABB // Ambiogous refrence fix me - DarkLotus
    {
        public Vector3D Min { get; private set; }
        public Vector3D Max { get; private set; }

        public AABB(MpqFileStream stream)
        {
            this.Min = new Vector3D(stream.ReadFloat(), stream.ReadFloat(), stream.ReadFloat());
            this.Max = new Vector3D(stream.ReadFloat(), stream.ReadFloat(), stream.ReadFloat());
        }
    }

    public class SNOName
    {
        public SNOGroup SNOGroup { get; private set; }
        public int SNOId { get; private set; }
        public string Name { get; private set; }

        public SNOName(MpqFileStream stream)
        {
            this.SNOGroup = (SNOGroup) stream.ReadInt32();
            this.SNOId = stream.ReadInt32();

            if (!MPQStorage.CoreData.Assets.ContainsKey(this.SNOGroup))
                return; // it's here because of the SNOGroup 0, could it be the Act? /raist
            this.Name = MPQStorage.CoreData.Assets[this.SNOGroup].ContainsKey(this.SNOId)
                            ? MPQStorage.CoreData.Assets[this.SNOGroup][SNOId].Name
                            : ""; // put it here because it seems we miss loading some scenes there /raist.
        }
    }


    public class Sphere
    {
        public Vector3D Position;
        public float Radius;

        public Sphere(MpqFileStream stream)
        {
            Position = new Vector3D(stream.ReadFloat(), stream.ReadFloat(), stream.ReadFloat());
            Radius = stream.ReadFloat();
        }
    }

    public class AxialCylinder
    {
        public Vector3D Position;
        public float Ax1;
        public float Ax2;

        public AxialCylinder(MpqFileStream stream)
        {
            this.Position = new Vector3D(stream.ReadFloat(), stream.ReadFloat(), stream.ReadFloat());
            Ax1 = stream.ReadFloat();
            Ax2 = stream.ReadFloat();
        }
    }

    public class PostFXParams
    {
        public float[] Float0;
        public float[] Float1;

        public PostFXParams(MpqFileStream stream)
        {
            Float0 = new float[4];
            for (int i = 0; i < Float0.Length; i++)
            {
                Float0[i] = stream.ReadInt32();
            }
            Float1 = new float[4];
            for (int i = 0; i < Float1.Length; i++)
            {
                Float1[i] = stream.ReadInt32();
            }
        }
    }

    public class WeightedLook
    {
        public string LookLink;
        public int Int0;

        public WeightedLook(MpqFileStream stream)
        {
            var buf = new byte[64];
            stream.Read(buf, 0, 64);
            LookLink = Encoding.ASCII.GetString(buf);
            Int0 = stream.ReadInt32();
        }
    }

    //public class Marker
    //{
    //    public string Name;
    //    int i0;
    //    public PRTransform PRTransform;
    //    public SNOName SNOName;
    //    public SerializeData serTagMap;
    //    // Un sure about these 3 ints, 010template isnt the same as snodata.xml - DarkLotus
    //    int TagMap;
    //    int i1,i2;
    //    SerializeData serMarkerLinks;
    //    public TagMap TM;
    //    public Marker(MpqFileStream stream)
    //    {
    //        byte[] buf = new byte[128];
    //        stream.Read(buf, 0, 128); Name = Encoding.ASCII.GetString(buf);
    //        i0 = stream.ReadInt32();
    //        PRTransform = new PRTransform(stream);
    //        SNOName = new SNOName(stream);
    //        serTagMap = new SerializeData(stream);
    //        TagMap = stream.ReadInt32();
    //        i1 = stream.ReadInt32();
    //        i2 = stream.ReadInt32();
    //        serMarkerLinks = new SerializeData(stream);
    //        stream.Position += (3 * 4);
    //        long x = stream.Position;

    //        if (serTagMap.Size > 0)
    //        {
    //            stream.Position = serTagMap.Offset + 16;
    //            TM = new TagMap(stream);

    //        }
    //        stream.Position = x;
    //    }
    //}
}
