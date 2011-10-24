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

            if (!MPQStorage.Data.Assets.ContainsKey(this.SNOGroup))
                return; // it's here because of the SNOGroup 0, could it be the Act? /raist
            this.Name = MPQStorage.Data.Assets[this.SNOGroup].ContainsKey(this.SNOId)
                            ? MPQStorage.Data.Assets[this.SNOGroup][SNOId].Name
                            : ""; // put it here because it seems we miss loading some scenes there /raist.
        }
    }

    public class TagMap : ISerializableData
    {
        public int TagMapSize;
        public TagMapEntry[] TagMapEntries;

        public void Read(MpqFileStream stream)
        {
            TagMapSize = stream.ReadInt32();
            TagMapEntries = new TagMapEntry[TagMapSize];

            for (int i = 0; i < TagMapSize; i++)
            {
                TagMapEntries[i] = new TagMapEntry(stream);
            }
        }
    }

    public class TagMapEntry
    {
        public int Int0;
        public int Int1;
        public int Int2;
        public float Float0;

        public TagMapEntry(MpqFileStream stream)
        {
            this.Int0 = stream.ReadInt32();
            this.Int1 = stream.ReadInt32();

            switch (this.Int0)
            {
                case 1:
                    Float0 = stream.ReadFloat();
                    break;
                default:
                    this.Int2 = stream.ReadInt32();
                    break;
            }
        }
    }

    //public class PostFXParams // unused for now. /raist.
    //{
    //    public float[] Float0;
    //    public float[] Float1;

    //    public PostFXParams(MpqFileStream stream)
    //    {
    //        Float0 = new float[4];
    //        for (int i = 0; i < Float0.Length; i++)
    //        {
    //            Float0[i] = stream.ReadInt32();
    //        }
    //        Float1 = new float[4];
    //        for (int i = 0; i < Float1.Length; i++)
    //        {
    //            Float1[i] = stream.ReadInt32();
    //        }
    //    }
    //}

    // Replace each Look with just a chararay? DarkLotus
    public class HardPointLink
    {
        public string Name;
        public int i0;
        public HardPointLink(MpqFileStream stream)
        {
            var buf = new byte[64];
            stream.Read(buf, 0, 64); Name = Encoding.ASCII.GetString(buf);
            i0 = stream.ReadInt32();
        }
    }
    
    public class TriggerConditions
    {
        // Unsure if these should be ints or floats - DarkLotus
        public float float0;
        public int int1;
        public int int2;
        public int int3;
        public int int4;
        public int int5;
        public int int6;
        public int int7;
        public int int8;
        public TriggerConditions(MpqFileStream stream)
        {
            float0 = stream.ReadFloat();
            int1 = stream.ReadInt32();
            int2 = stream.ReadInt32();
            int3 = stream.ReadInt32();
            int4 = stream.ReadInt32();
            int5 = stream.ReadInt32();
            int6 = stream.ReadInt32();
            int7 = stream.ReadInt32();
            int8 = stream.ReadInt32();
        }
    }
    public class TriggerEvent
    {
        public int i0;
        public TriggerConditions TriggerConditions;
        public int i1;
        public SNOName SnoName;
        public int i2, i3;
        //pad 12
        public HardPointLink[] HardPointLinks;
        public string LookLink;
        public string ConstraintLink;
        int i4;
        float f0;
        int i5, i6, i7, i8, i9;
        float f1, f2;
        int i10, i11;
        float f3;
        int i12;
        float Velocity;
        int i13; // DT_TIME
        int RuneType, UseRuneType;
        public RGBAColor Color1;
        int i14; // DT_TIME
        public RGBAColor Color2;
        int i15; // DT_TIME
        public TriggerEvent(MpqFileStream stream)
        {
            i0 = stream.ReadInt32();
            TriggerConditions = new TriggerConditions(stream);
            i1 = stream.ReadInt32();
            SnoName = new SNOName(stream);
            i2 = stream.ReadInt32();
            i3 = stream.ReadInt32();
            HardPointLinks = new HardPointLink[2];
            HardPointLinks[0] = new HardPointLink(stream);
            HardPointLinks[1] = new HardPointLink(stream);
            var buf = new byte[64];
            stream.Read(buf, 0, 64);
            this.LookLink = Encoding.ASCII.GetString(buf);

            buf = new byte[64];
            stream.Read(buf, 0, 64);
            this.ConstraintLink = Encoding.ASCII.GetString(buf);
            i4 = stream.ReadInt32();
            f0 = stream.ReadFloat();
            i5 = stream.ReadInt32();
            i6 = stream.ReadInt32();
            i7 = stream.ReadInt32();
            i8 = stream.ReadInt32();
            i9 = stream.ReadInt32();
            f1 = stream.ReadFloat();
            f2 = stream.ReadFloat();
            i10 = stream.ReadInt32();
            i11 = stream.ReadInt32();
            f3 = stream.ReadFloat();
            i12 = stream.ReadInt32();
            Velocity = stream.ReadFloat();
            i13 = stream.ReadInt32();
            RuneType = stream.ReadInt32();
            UseRuneType = stream.ReadInt32();
            Color1 = new RGBAColor(stream);
            i14 = stream.ReadInt32();
            Color2 = new RGBAColor(stream);
            i15 = stream.ReadInt32();

        }
    }

    public class MsgTriggeredEvent : ISerializableData
    {
        int i0;
        TriggerEvent TriggerEvent;
        public void Read(MpqFileStream stream)
        {
            i0 = stream.ReadInt32();
            TriggerEvent = new TriggerEvent(stream);
        }
    }
    public class RGBAColor
    {
        public byte byte0;
        public byte byte1;
        public byte byte2;
        public byte byte3;
        public RGBAColor(MpqFileStream stream)
        {
            byte[] buf = new byte[4];
            stream.Read(buf, 0, 4);
            byte0 = buf[0];
            byte1 = buf[1];
            byte2 = buf[2];
            byte3 = buf[3];
        }
    }
}
