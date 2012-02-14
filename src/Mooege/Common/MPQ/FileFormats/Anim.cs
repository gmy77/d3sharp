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
using Mooege.Core.GS.Common.Types.SNO;
using Mooege.Common.MPQ.FileFormats.Types;
using Mooege.Core.GS.Common.Types.Math;

namespace Mooege.Common.MPQ.FileFormats
{
    [FileFormat(SNOGroup.Anim)]
    public class Anim : FileFormat
    {
        public Header Header { get; private set; }
        public int I0 { get; private set; }
        public int I1 { get; private set; }
        public int SNOAppearance { get; private set; }
        public List<AnimPermutation> Permutations { get; private set; }
        public int I2 { get; private set; }
        public int I3 { get; private set; }

        public Anim(MpqFile file)
        {
            var stream = file.Open();
            this.Header = new Header(stream);
            this.I0 = stream.ReadValueS32();
            this.I1 = stream.ReadValueS32();
            this.SNOAppearance = stream.ReadValueS32();
            this.Permutations = stream.ReadSerializedData<AnimPermutation>();
            this.I2 = stream.ReadValueS32();
            stream.Position += 12;
            this.I3 = stream.ReadValueS32();
            stream.Close();
        }
    }

    public class AnimPermutation : ISerializableData
    {
        public int I0 { get; private set; }
        public string AnimName { get; private set; }
        public float Velocity { get; private set; }
        public float F0 { get; private set; }
        public float F1 { get; private set; }
        public float F2 { get; private set; }
        public float F3 { get; private set; }
        public int Time1 { get; private set; }
        public int Time2 { get; private set; }
        public int I1 { get; private set; }
        public float F4 { get; private set; }
        public float F5 { get; private set; }
        public float F6 { get; private set; }
        public float F7 { get; private set; }
        public int BoneNameCount { get; private set; }
        public List<BoneName> BoneNames { get; private set; }
        public int KeyframePosCount { get; private set; }
        public List<TranslationCurve> TranslationCurves { get; private set; }
        public List<RotationCurve> RotationCurves { get; private set; }
        public List<ScaleCurve> ScaleCurves { get; private set; }
        public float F8 { get; private set; }
        public float F9 { get; private set; }
        public float F10 { get; private set; }
        public float F11 { get; private set; }
        public Vector3D V0 { get; private set; }
        public Vector3D V1 { get; private set; }
        public Vector3D V2 { get; private set; }
        public Vector3D V3 { get; private set; }
        public float F12 { get; private set; }
        public int KeyedAttachmentsCount { get; private set; }
        public List<KeyframedAttachment> KeyedAttachments { get; private set; }
        public List<Vector3D> KeyframePosList { get; private set; }
        public List<Vector3D> NonlinearOffset { get; private set; }
        public VelocityVector3D Velocity3D { get; private set; }
        public HardPointLink Link { get; private set; }
        public string S0 { get; private set; }
        public string S1 { get; private set; }

        public void Read(MpqFileStream stream)
        {
            this.I0 = stream.ReadValueS32();
            this.AnimName = stream.ReadString(65, true);
            stream.Position += 3;
            this.Velocity = stream.ReadValueF32();
            this.F0 = stream.ReadValueF32();
            this.F1 = stream.ReadValueF32();
            this.F2 = stream.ReadValueF32();
            this.F3 = stream.ReadValueF32();
            this.Time1 = stream.ReadValueS32();
            this.Time2 = stream.ReadValueS32();
            this.I1 = stream.ReadValueS32();
            this.F4 = stream.ReadValueF32();
            this.F5 = stream.ReadValueF32();
            this.F6 = stream.ReadValueF32();
            this.F7 = stream.ReadValueF32();
            this.BoneNameCount = stream.ReadValueS32();
            this.BoneNames = stream.ReadSerializedData<BoneName>();
            stream.Position += 12;
            this.KeyframePosCount = stream.ReadValueS32();
            this.TranslationCurves = stream.ReadSerializedData<TranslationCurve>();
            stream.Position += 12;
            this.RotationCurves = stream.ReadSerializedData<RotationCurve>();
            stream.Position += 8;
            this.ScaleCurves = stream.ReadSerializedData<ScaleCurve>();
            stream.Position += 8;
            this.F8 = stream.ReadValueF32();
            this.F9 = stream.ReadValueF32();
            this.F10 = stream.ReadValueF32();
            this.F11 = stream.ReadValueF32();
            this.V0 = new Vector3D(stream);
            this.V1 = new Vector3D(stream);
            this.V2 = new Vector3D(stream);
            this.V3 = new Vector3D(stream);
            this.F12 = stream.ReadValueF32();
            this.KeyedAttachments = stream.ReadSerializedData<KeyframedAttachment>();
            this.KeyedAttachmentsCount = stream.ReadValueS32();
            stream.Position += 8;
            this.KeyframePosList = stream.ReadSerializedData<Vector3D>();
            stream.Position += 8;
            this.NonlinearOffset = stream.ReadSerializedData<Vector3D>();
            stream.Position += 8;
            this.Velocity3D = new VelocityVector3D(stream);
            this.Link = new HardPointLink(stream);
            this.S0 = stream.ReadString(256, true);
            this.S1 = stream.ReadString(256, true);
            stream.Position += 8;
        }
    }

    public class BoneName : ISerializableData
    {
        public string Name { get; private set; }

        public void Read(MpqFileStream stream)
        {
            this.Name = stream.ReadString(64, true);
        }
    }

    public class TranslationCurve : ISerializableData
    {
        public int I0 { get; private set; }
        public List<TranslationKey> Keys { get; private set; }

        public void Read(MpqFileStream stream)
        {
            this.I0 = stream.ReadValueS32();
            this.Keys = stream.ReadSerializedData<TranslationKey>();
        }
    }

    public class RotationCurve : ISerializableData
    {
        public int I0 { get; private set; }
        public List<RotationKey> Keys { get; private set; }

        public void Read(MpqFileStream stream)
        {
            this.I0 = stream.ReadValueS32();
            this.Keys = stream.ReadSerializedData<RotationKey>();
        }
    }

    public class ScaleCurve : ISerializableData
    {
        public int I0 { get; private set; }
        public List<ScaleKey> Keys { get; private set; }

        public void Read(MpqFileStream stream)
        {
            this.I0 = stream.ReadValueS32();
            this.Keys = stream.ReadSerializedData<ScaleKey>();
        }
    }

    public class TranslationKey : ISerializableData
    {
        public int I0 { get; private set; }
        public Vector3D Location { get; private set; }

        public void Read(MpqFileStream stream)
        {
            this.I0 = stream.ReadValueS32();
            this.Location = new Vector3D(stream);
        }
    }

    public class RotationKey : ISerializableData
    {
        public int I0 { get; private set; }
        public Quaternion16 Q0 { get; private set; }

        public void Read(MpqFileStream stream)
        {
            this.I0 = stream.ReadValueS32();
            this.Q0 = new Quaternion16(stream);
        }
    }

    public class ScaleKey : ISerializableData
    {
        public int I0 { get; private set; }
        public float Scale { get; private set; }

        public void Read(MpqFileStream stream)
        {
            this.I0 = stream.ReadValueS32();
            this.Scale = stream.ReadValueF32();
        }
    }

    public class KeyframedAttachment : ISerializableData
    {
        public float F0 { get; private set; }
        public TriggerEvent Event { get; private set; }

        public void Read(MpqFileStream stream)
        {
            this.F0 = stream.ReadValueF32();
            this.Event = new TriggerEvent(stream);
        }
    }

    public class VelocityVector3D
    {
        public float VelocityX { get; private set; }
        public float VelocityY { get; private set; }
        public float VelocityZ { get; private set; }

        public VelocityVector3D(MpqFileStream stream)
        {
            this.VelocityX = stream.ReadValueF32();
            this.VelocityY = stream.ReadValueF32();
            this.VelocityZ = stream.ReadValueF32();
        }
    }

    public class Quaternion16
    {
        public short Short0;
        public short Short1;
        public short Short2;
        public short Short3;

        public Quaternion16() { }

        /// <summary>
        /// Reads Quaternion16 from given MPQFileStream.
        /// </summary>
        /// <param name="stream">The MPQFileStream to read from.</param>
        public Quaternion16(MpqFileStream stream)
        {
            this.Short0 = stream.ReadValueS16();
            this.Short1 = stream.ReadValueS16();
            this.Short2 = stream.ReadValueS16();
            this.Short3 = stream.ReadValueS16();
        }
    }
}
