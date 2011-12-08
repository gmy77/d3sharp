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
using Mooege.Core.GS.Common.Types.Misc;
using Mooege.Core.GS.Common.Types.SNO;
using System.Linq;

namespace Mooege.Common.MPQ.FileFormats
{
    [FileFormat(SNOGroup.Globals)]
    public class Globals : FileFormat
    {
        public Header Header { get; private set; }
        public DifficultyTuningParams[] TuningParams { get; private set; } //len 4
        public float F0 { get; private set; }
        public float F1 { get; private set; }
        public float F2 { get; private set; }
        public int I0 { get; private set; }
        public Dictionary<int, ActorGroup> ActorGroup { get; private set; }
        public int I1 { get; private set; }
        public Dictionary<int, StartLocationName> StartLocationNames { get; private set; }
        public List<GlobalScriptVariable> ScriptGlobalVars { get; private set; }
        public float F3 { get; private set; }
        public float F4 { get; private set; }
        public float F5 { get; private set; }
        public float F6 { get; private set; }
        public float F7 { get; private set; }
        public float F33 { get; private set; }
        public float F34 { get; private set; }
        public RGBAColor[] Colors { get; private set; }
        public int I2 { get; private set; }
        public int I3 { get; private set; }
        public int I4 { get; private set; }
        public int I5 { get; private set; }
        public float F8 { get; private set; }
        public float F9 { get; private set; }
        public int I6 { get; private set; }
        public float F10 { get; private set; }
        public float F11 { get; private set; }
        public float F12 { get; private set; }
        public float F13 { get; private set; }
        public float F14 { get; private set; }
        public float F15 { get; private set; }
        public float F16 { get; private set; }
        public int I7 { get; private set; }
        public int I8 { get; private set; }
        public float F17 { get; private set; }
        public float F18 { get; private set; }
        public float F19 { get; private set; }
        public float F20 { get; private set; }
        public float F21 { get; private set; }
        public int I9 { get; private set; }
        public int[] I10 { get; private set; } //len 4
        public BannerParams BannerParams { get; private set; }
        public int I11 { get; private set; }
        public int I12 { get; private set; }
        public int I13 { get; private set; }
        public int I14 { get; private set; }
        public int I15 { get; private set; }
        public float F22 { get; private set; }
        public float F23 { get; private set; }
        public float F24 { get; private set; }
        public float F25 { get; private set; }
        public float F26 { get; private set; }
        public float F27 { get; private set; }
        public float F28 { get; private set; }
        public float F29 { get; private set; }
        public float F30 { get; private set; }
        public float F31 { get; private set; }
        public float F32 { get; private set; }

        public Globals(MpqFile file)
        {
            var stream = file.Open();
            this.Header = new Header(stream);
            this.TuningParams = new DifficultyTuningParams[4];
            for (int i = 0; i < 4; i++)
                this.TuningParams[i] = new DifficultyTuningParams(stream);
            this.F0 = stream.ReadValueF32(); //140
            this.F1 = stream.ReadValueF32(); //144
            this.F2 = stream.ReadValueF32(); //148
            this.I0 = stream.ReadValueS32(); //152
            stream.Position += 12;

            this.ActorGroup = new Dictionary<int, FileFormats.ActorGroup>();
            foreach(var group in stream.ReadSerializedData<ActorGroup>()) //166
                this.ActorGroup.Add(group.UHash, group);
           

            this.I1 = stream.ReadValueS32(); //176
            stream.Position += 12;

            this.StartLocationNames = new Dictionary<int, FileFormats.StartLocationName>();
            foreach (var startLocation in stream.ReadSerializedData<StartLocationName>())  //168
                StartLocationNames.Add(startLocation.I0, startLocation);

            stream.Position += 8;
            this.ScriptGlobalVars = stream.ReadSerializedData<GlobalScriptVariable>();  //208
            this.F3 = stream.ReadValueF32(); //216
            this.F4 = stream.ReadValueF32(); //220
            this.F5 = stream.ReadValueF32(); //224
            this.F6 = stream.ReadValueF32(); //228
            this.F7 = stream.ReadValueF32(); //232
            this.F33 = stream.ReadValueF32(); //236
            this.F34 = stream.ReadValueF32(); //240
            Colors = new RGBAColor[400]; //244
            for (int i = 0; i < 400; i++)
                Colors[i] = new RGBAColor(stream);
            this.I2 = stream.ReadValueS32(); //1844
            this.I3 = stream.ReadValueS32(); //1848
            this.I4 = stream.ReadValueS32(); //1852
            this.I5 = stream.ReadValueS32(); //1856
            this.F8 = stream.ReadValueF32(); //1860
            this.F9 = stream.ReadValueF32(); //1864
            this.I6 = stream.ReadValueS32(); //1868
            this.F10 = stream.ReadValueF32(); //1872
            this.F11 = stream.ReadValueF32(); //1876
            this.F12 = stream.ReadValueF32(); //1880
            this.F13 = stream.ReadValueF32(); //1884
            this.F14 = stream.ReadValueF32(); //1888
            this.F15 = stream.ReadValueF32(); //1892
            this.F16 = stream.ReadValueF32(); //1896
            this.I7 = stream.ReadValueS32(); //1900
            this.I8 = stream.ReadValueS32(); //1904
            this.F17 = stream.ReadValueF32(); //1908
            this.F18 = stream.ReadValueF32(); //1912
            this.F19 = stream.ReadValueF32(); //1916
            this.F20 = stream.ReadValueF32(); //1920
            this.F21 = stream.ReadValueF32(); //1924
            this.I9 = stream.ReadValueS32(); //1928
            this.I10 = new int[4]; //1932
            for (int i = 0; i < 4; i++)
                this.I10[i] = stream.ReadValueS32();
            stream.Position += 4;
            this.BannerParams = new BannerParams(stream); //1952
            this.I11 = stream.ReadValueS32(); //2184
            this.I12 = stream.ReadValueS32(); //2188
            this.I13 = stream.ReadValueS32(); //2192
            this.I14 = stream.ReadValueS32(); //2196
            this.I15 = stream.ReadValueS32(); //2200
            this.F22 = stream.ReadValueF32(); //2204
            this.F23 = stream.ReadValueF32(); //2208
            this.F24 = stream.ReadValueF32(); //2212
            this.F25 = stream.ReadValueF32(); //2216
            this.F26 = stream.ReadValueF32(); //2220
            this.F27 = stream.ReadValueF32(); //2224
            this.F28 = stream.ReadValueF32(); //2228
            this.F29 = stream.ReadValueF32(); //2232
            this.F30 = stream.ReadValueF32(); //2236
            this.F31 = stream.ReadValueF32(); //2240
            this.F32 = stream.ReadValueF32(); //2244
            stream.Close();
        }
    }

    public class DifficultyTuningParams
    {
        public float F0 { get; private set; }
        public float F1 { get; private set; }
        public float F2 { get; private set; }
        public float F3 { get; private set; }
        public float F4 { get; private set; }
        public float F5 { get; private set; }
        public float F6 { get; private set; }
        public float F7 { get; private set; }

        public DifficultyTuningParams(MpqFileStream stream)
        {
            this.F0 = stream.ReadValueF32();
            this.F1 = stream.ReadValueF32();
            this.F2 = stream.ReadValueF32();
            this.F3 = stream.ReadValueF32();
            this.F4 = stream.ReadValueF32();
            this.F5 = stream.ReadValueF32();
            this.F6 = stream.ReadValueF32();
            this.F7 = stream.ReadValueF32();
        }
    }

    public class ActorGroup : ISerializableData
    {
        public int UHash { get; private set; }
        public string S0 { get; private set; }

        public void Read(MpqFileStream stream)
        {
            this.UHash = stream.ReadValueS32();
            this.S0 = stream.ReadString(64, true);
        }
    }

    public class StartLocationName : ISerializableData
    {
        public int I0 { get; private set; }
        public string S0 { get; private set; }

        public void Read(MpqFileStream stream)
        {
            this.I0 = stream.ReadValueS32();
            this.S0 = stream.ReadString(64, true);
        }
    }

    public class GlobalScriptVariable : ISerializableData
    {
        public int UHash { get; private set; }
        public string S0 { get; private set; }
        public float F0 { get; private set; }

        public void Read(MpqFileStream stream)
        {
            this.UHash = stream.ReadValueS32();
            this.S0 = stream.ReadString(32, true);
            this.F0 = stream.ReadValueF32();
        }
    }

    public class BannerParams
    {
        //Total Length: 232
        public List<BannerTexturePair> TexBackgrounds { get; private set; }
        public int I0 { get; private set; }
        public List<BannerTexturePair> TexPatterns { get; private set; }
        public List<BannerTexturePair> TexMainSigils { get; private set; }
        public List<BannerTexturePair> TexVariantSigils { get; private set; }
        public List<BannerTexturePair> TexSigilAccents { get; private set; }
        public List<BannerColorSet> ColorSets { get; private set; }
        public List<BannerSigilPlacement> SigilPlacements { get; private set; }
        public List<int> SNOActorBases { get; private set; }
        public List<int> SNOActorCaps { get; private set; }
        public List<int> SNOActorPoles { get; private set; }
        public List<int> SNOActorRibbons { get; private set; }
        public List<EpicBannerDescription> EpicBannerDescriptions { get; private set; }

        public BannerParams(MpqFileStream stream)
        {
            stream.Position += 8;
            this.TexBackgrounds = stream.ReadSerializedData<BannerTexturePair>();
            this.I0 = stream.ReadValueS32(); //16
            stream.Position += 12;
            this.TexPatterns = stream.ReadSerializedData<BannerTexturePair>();
            this.I0 = stream.ReadValueS32(); //40
            stream.Position += 12;
            this.TexMainSigils = stream.ReadSerializedData<BannerTexturePair>();
            stream.Position += 8;
            this.TexVariantSigils = stream.ReadSerializedData<BannerTexturePair>();
            this.I0 = stream.ReadValueS32(); //80
            stream.Position += 12;
            this.TexSigilAccents = stream.ReadSerializedData<BannerTexturePair>();
            this.I0 = stream.ReadValueS32(); //104
            stream.Position += 12;
            this.ColorSets = stream.ReadSerializedData<BannerColorSet>();
            stream.Position += 8;
            this.SigilPlacements = stream.ReadSerializedData<BannerSigilPlacement>();
            stream.Position += 8;
            this.SNOActorBases = stream.ReadSerializedInts();
            stream.Position += 8;
            this.SNOActorCaps = stream.ReadSerializedInts();
            stream.Position += 8;
            this.SNOActorPoles = stream.ReadSerializedInts();
            stream.Position += 8;
            this.SNOActorRibbons = stream.ReadSerializedInts();
            stream.Position += 8;
            this.EpicBannerDescriptions = stream.ReadSerializedData<EpicBannerDescription>();
            stream.Position += 8;
        }
    }

    public class BannerTexturePair : ISerializableData
    {
        public int SNOTexture { get; private set; }
        public int I0 { get; private set; }

        public void Read(MpqFileStream stream)
        {
            this.SNOTexture = stream.ReadValueS32();
            this.I0 = stream.ReadValueS32();
            stream.Position += 4;
        }
    }

    public class BannerColorSet : ISerializableData
    {
        public RGBAColor[] Color { get; private set; }
        public string String1 { get; private set; }
        public int I0 { get; private set; }
        public int I1 { get; private set; }

        public void Read(MpqFileStream stream)
        {
            this.Color = new RGBAColor[2];
            for (int i = 0; i < 2; i++)
                this.Color[i] = new RGBAColor(stream);
            this.String1 = stream.ReadString(64, true);
            this.I0 = stream.ReadValueS32();
            this.I1 = stream.ReadValueS32();
            stream.Position += 4;
        }
    }

    public class BannerSigilPlacement : ISerializableData
    {
        public string S0 { get; private set; }
        public int I0 { get; private set; }

        public void Read(MpqFileStream stream)
        {
            this.S0 = stream.ReadString(64, true);
            this.I0 = stream.ReadValueS32();
        }
    }

    public class EpicBannerDescription : ISerializableData
    {
        public int SNOBannerShape { get; private set; }
        public int SNOBannerBase { get; private set; }
        public int SNOBannerPole { get; private set; }
        public int I3 { get; private set; }
        public string S0 { get; private set; }

        public void Read(MpqFileStream stream)
        {
            this.SNOBannerShape = stream.ReadValueS32();
            this.SNOBannerBase = stream.ReadValueS32();
            this.SNOBannerPole = stream.ReadValueS32();
            this.I3 = stream.ReadValueS32();
            this.S0 = stream.ReadString(128, true);
        }
    }
}
