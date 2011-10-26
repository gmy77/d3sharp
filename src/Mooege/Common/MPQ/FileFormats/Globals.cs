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

using System.IO;
using System.Collections.Generic;
using CrystalMpq;
using Gibbed.IO;
using Mooege.Common.Extensions;
using Mooege.Common.MPQ.FileFormats.Types;

namespace Mooege.Common.MPQ.FileFormats
{
    [FileFormat(SNOGroup.Globals)]
    class Globals : FileFormat
    {
        public Header header;
        public DifficultyTuningParams[] tuningParams; //len 4
        public float f0;
        public float f1;
        public float f2;
        public int i0;
        public List<ActorGroup> actorGroup;
        public int i1;
        public List<StartLocationName> StartLocationNames;
        public List<GlobalScriptVariable> ScriptGlobalVars;
        public float f3;
        public float f4;
        public float f5;
        public float f6;
        public float f7;
        public RGBAColor[] Colors;
        public int i2;
        public int i3;
        public int i4;
        public int i5;
        public float f8;
        public float f9;
        public int i6;
        public float f10;
        public float f11;
        public float f12;
        public float f13;
        public float f14;
        public float f15;
        public float f16;
        public int i7;
        public int i8;
        public float f17;
        public float f18;
        public float f19;
        public float f20;
        public float f21;
        public int i9;
        public int[] i10; //len 4
        public BannerParams bannerParams;
        public int i11;
        public int i12;
        public int i13;
        public int i14;
        public int i15;
        public float f22;
        public float f23;
        public float f24;
        public float f25;
        public float f26;

        public Globals(MpqFile file)
        {
            var stream = file.Open();
            this.header = new Header(stream);
            this.tuningParams = new DifficultyTuningParams[4];
            for (int i = 0; i < 4; i++)
                this.tuningParams[i] = new DifficultyTuningParams(stream);
            this.f0 = stream.ReadValueF32(); //124
            this.f1 = stream.ReadValueF32(); //128
            this.f2 = stream.ReadValueF32(); //132
            this.i0 = stream.ReadValueS32(); //136
            stream.Position += 12;
            this.actorGroup = stream.ReadSerializedData<ActorGroup>(); //144
            stream.Position += 12;
            this.i1 = stream.ReadValueS32(); //160
            this.StartLocationNames = stream.ReadSerializedData<StartLocationName>(); //168
            stream.Position += 8;
            this.ScriptGlobalVars = stream.ReadSerializedData<GlobalScriptVariable>();  //184
            this.f3 = stream.ReadValueF32(); //200
            this.f4 = stream.ReadValueF32(); //204
            this.f5 = stream.ReadValueF32(); //208
            this.f6 = stream.ReadValueF32(); //212
            this.f7 = stream.ReadValueF32(); //216
            Colors = new RGBAColor[400]; //220
            for (int i = 0; i < 400; i++)
                Colors[i] = new RGBAColor(stream);
            this.i2 = stream.ReadValueS32(); //1820
            this.i3 = stream.ReadValueS32(); //1824
            this.i4 = stream.ReadValueS32(); //1828
            this.i5 = stream.ReadValueS32(); //1832
            this.f8 = stream.ReadValueF32(); //1836
            this.f9 = stream.ReadValueF32(); //1840
            this.i6 = stream.ReadValueS32(); //1844
            this.f10 = stream.ReadValueF32(); //1848
            this.f11 = stream.ReadValueF32(); //1852
            this.f12 = stream.ReadValueF32(); //1856
            this.f13 = stream.ReadValueF32(); //1860
            this.f14 = stream.ReadValueF32(); //1864
            this.f15 = stream.ReadValueF32(); //1868
            this.f16 = stream.ReadValueF32(); //1872
            this.i7 = stream.ReadValueS32(); //1876
            this.i8 = stream.ReadValueS32(); //1880
            this.f17 = stream.ReadValueF32(); //1884
            this.f18 = stream.ReadValueF32(); //1888
            this.f19 = stream.ReadValueF32(); //1892
            this.f20 = stream.ReadValueF32(); //1896
            this.f21 = stream.ReadValueF32(); //1900
            this.i9 = stream.ReadValueS32(); //1904
            this.i10 = new int[4]; //1908
            for (int i = 0; i < 4; i++)
                this.i10[i] = stream.ReadValueS32();
            stream.Position += 4;
            this.bannerParams = new BannerParams(stream); //1928
            this.i11 = stream.ReadValueS32(); //2120
            this.i12 = stream.ReadValueS32(); //2124
            this.i13 = stream.ReadValueS32(); //2128
            this.i14 = stream.ReadValueS32(); //2132
            this.i15 = stream.ReadValueS32(); //2136
            this.f22 = stream.ReadValueF32(); //2140
            this.f23 = stream.ReadValueF32(); //2144
            this.f24 = stream.ReadValueF32(); //2148
            this.f25 = stream.ReadValueF32(); //2152
            this.f26 = stream.ReadValueF32(); //2156
            stream.Close();
        }
    }

    class DifficultyTuningParams
    {
        public float f0;
        public float f1;
        public float f2;
        public float f3;
        public float f4;
        public float f5;
        public float f6;

        public DifficultyTuningParams(MpqFileStream stream)
        {
            this.f0 = stream.ReadValueF32();
            this.f1 = stream.ReadValueF32();
            this.f2 = stream.ReadValueF32();
            this.f3 = stream.ReadValueF32();
            this.f4 = stream.ReadValueF32();
            this.f5 = stream.ReadValueF32();
            this.f6 = stream.ReadValueF32();
        }
    }

    class ActorGroup : ISerializableData
    {
        public int uHash;
        public string s0;

        public void Read(MpqFileStream stream)
        {
            this.uHash = stream.ReadValueS32();
            this.s0 = stream.ReadString(64, true);
        }
    }

    class StartLocationName : ISerializableData
    {
        public int i0;
        public string s0;

        public void Read(MpqFileStream stream)
        {
            this.i0 = stream.ReadValueS32();
            this.s0 = stream.ReadString(64, true);
        }
    }

    class GlobalScriptVariable : ISerializableData
    {
        public int uHash;
        public string s0;
        public float f0;

        public void Read(MpqFileStream stream)
        {
            this.uHash = stream.ReadValueS32();
            this.s0 = stream.ReadString(64, true);
            this.f0 = stream.ReadValueF32();
        }
    }

    class BannerParams
    {
        //Total Length: 192
        public List<BannerTexturePair> TexBackgrounds;
        public int i0;
        public List<BannerTexturePair> TexPatterns;
        public List<BannerTexturePair> TexMainSigils;
        public List<BannerTexturePair> TexVariantSigils;
        public List<BannerTexturePair> TexSigilAccents;
        public List<BannerColorSet> ColorSets;
        public List<int> snoActorBases;
        public List<int> snoActorCaps;
        public List<int> snoActorPoles;
        public List<int> snoActorRibbons;

        public BannerParams(MpqFileStream stream)
        {
            stream.Position += 8;
            this.TexBackgrounds = stream.ReadSerializedData<BannerTexturePair>();
            this.i0 = stream.ReadValueS32(); //16
            stream.Position += 12;
            this.TexPatterns = stream.ReadSerializedData<BannerTexturePair>();
            this.i0 = stream.ReadValueS32(); //40
            stream.Position += 12;
            this.TexMainSigils = stream.ReadSerializedData<BannerTexturePair>();
            stream.Position += 8;
            this.TexVariantSigils = stream.ReadSerializedData<BannerTexturePair>();
            this.i0 = stream.ReadValueS32(); //80
            stream.Position += 12;
            this.TexSigilAccents = stream.ReadSerializedData<BannerTexturePair>();
            this.i0 = stream.ReadValueS32(); //104
            stream.Position += 12;
            this.ColorSets = stream.ReadSerializedData<BannerColorSet>();
            stream.Position += 8;
            this.snoActorBases = stream.ReadSerializedInts();
            stream.Position += 8;
            this.snoActorCaps = stream.ReadSerializedInts();
            stream.Position += 8;
            this.snoActorPoles = stream.ReadSerializedInts();
            stream.Position += 8;
            this.snoActorRibbons = stream.ReadSerializedInts();
        }
    }

    class BannerTexturePair : ISerializableData
    {
        public int snoTexture;
        public int i0;

        public void Read(MpqFileStream stream)
        {
            this.snoTexture = stream.ReadValueS32();
            this.i0 = stream.ReadValueS32();
        }

    }

    class BannerColorSet : ISerializableData
    {
        RGBAColor[] Color;
        int i0;
        string s0;

        public void Read(MpqFileStream stream)
        {
            this.Color = new RGBAColor[2];
            for (int i = 0; i < 2; i++)
                this.Color[i] = new RGBAColor(stream);
            this.i0 = stream.ReadValueS32();
            this.s0 = stream.ReadString(64,true);
        }
    }
}
