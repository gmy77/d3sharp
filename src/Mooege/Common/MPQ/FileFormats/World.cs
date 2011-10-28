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
using Mooege.Core.GS.Common.Types.Math;
using Mooege.Core.GS.Common.Types.SNO;
using Mooege.Core.GS.Common.Types.Scene;

namespace Mooege.Common.MPQ.FileFormats
{
    [FileFormat(SNOGroup.Worlds)]
    public class World : FileFormat
    {
        public Header Header { get; private set; }
        public DRLGParams DRLGParams = new DRLGParams();
        public SceneParams SceneParams = new SceneParams();
        public List<int> MarkerSets = new List<int>();
        public Environment Environment { get; private set; }
        public LabelRuleSet LabelRuleSet { get; private set; }        
        public SceneClusterSet SceneClusterSet { get; private set; }
        public int[] SNONavMeshFunctions = new int[4];
        public int Int0 { get; private set; }
        public float Float0 { get; private set; }
        public int Int1 { get; private set; }
        public int SNOScript { get; private set; }
        public int Int2 { get; private set; }

        public World(MpqFile file)
        {
            var stream = file.Open();

            this.Header = new Header(stream);

            this.DRLGParams = stream.ReadSerializedItem<DRLGParams>(); // I'm not sure if we can have a list of drlgparams. (then should be calling it with pointer.Size/120) /raist

            stream.Position += (3*4);
            this.SceneParams = stream.ReadSerializedItem<SceneParams>(); // I'm not sure if we can have a list of drlgparams. (then should be calling it with pointer.Size/24) /raist

            stream.Position += (2*4);
            this.MarkerSets = stream.ReadSerializedInts();

            stream.Position += (14*4);
            this.Environment = new Environment(stream);

            LabelRuleSet = new LabelRuleSet(stream);
            this.Int0 = stream.ReadValueS32();

            stream.Position += 4;
            this.SceneClusterSet = new SceneClusterSet(stream);

            for (int i = 0; i < SNONavMeshFunctions.Length; i++)
            {
                SNONavMeshFunctions[i] = stream.ReadValueS32();
            }

            stream.Position += 4;
            Float0 = stream.ReadValueF32();
            Int1 = stream.ReadValueS32();
            SNOScript = stream.ReadValueS32();
            Int2 = stream.ReadValueS32();

            stream.Close();
        }
    }

    #region scene-params

    public class SceneParams : ISerializableData
    {
        public List<SceneChunk> SceneChunks = new List<SceneChunk>();
        public int ChunkCount { get; private set; }

        public void Read(MpqFileStream stream)
        {
            var pointer = stream.GetSerializedDataPointer();
            this.ChunkCount = stream.ReadValueS32();
            stream.Position += (3 * 4);
            this.SceneChunks = stream.ReadSerializedData<SceneChunk>(pointer, this.ChunkCount);
        }
    }

    public class SceneChunk : ISerializableData
    {
        public SNOName SNOName { get; private set; }
        public PRTransform PRTransform { get; private set; }
        public SceneSpecification SceneSpecification { get; private set; }

        public void Read(MpqFileStream stream)
        {
            this.SNOName = new SNOName(stream);
            this.PRTransform = new PRTransform(stream);
            this.SceneSpecification = new SceneSpecification(stream);
        }
    }
  
    #endregion

    #region drlg-params

    public class DRLGParams : ISerializableData
    {
        public List<TileInfo> DRLGTiles = new List<TileInfo>();
        public int CommandCount { get; private set; }
        public List<DRLGCommand> DRLGCommands = new List<DRLGCommand>();
        public List<int> ParentIndices = new List<int>();
        public TagMap DRLGTagMap { get; private set; }

        public void Read(MpqFileStream stream)
        {
            var pointer = stream.GetSerializedDataPointer();
            this.DRLGTiles = stream.ReadSerializedData<TileInfo>(pointer, pointer.Size / 72);

            stream.Position += (14 * 4);
            this.CommandCount = stream.ReadValueS32();
            this.DRLGCommands = stream.ReadSerializedData<DRLGCommand>(this.CommandCount);

            stream.Position += (3 * 4);
            this.ParentIndices = stream.ReadSerializedInts();

            stream.Position += (2 * 4);
            this.DRLGTagMap = stream.ReadSerializedItem<TagMap>();
        }
    }

    public class TileInfo : ISerializableData
    {
        public int Int0 { get; private set; }
        public int Int1 { get; private set; }
        public int SNOScene { get; private set; }
        public int Int2 { get; private set; }
        public TagMap TileTagMap { get; private set; }
        public CustomTileInfo CustomTileInfo { get; private set; }

        public void Read(MpqFileStream stream)
        {
            Int0 = stream.ReadValueS32();
            Int1 = stream.ReadValueS32();
            SNOScene = stream.ReadValueS32();
            Int2 = stream.ReadValueS32();
            this.TileTagMap = stream.ReadSerializedItem<TagMap>();

            stream.Position += (2 * 4);
            CustomTileInfo = new CustomTileInfo(stream);
        }
    }

    public class DRLGCommand : ISerializableData
    {
        public string Name { get; private set; }
        public int Int0 { get; private set; }
        public TagMap CommandTagMap { get; private set; }

        public void Read(MpqFileStream stream)
        {
            this.Name = stream.ReadString(128, true);
            Int0 = stream.ReadValueS32();
            this.CommandTagMap = stream.ReadSerializedItem<TagMap>();
            stream.Position += (3 * 4);
        }
    }

    public class CustomTileInfo
    {
        public int Int0 { get; private set; }
        public int Int1 { get; private set; }
        public int Int2 { get; private set; }
        public Vector2D V0 { get; private set; }

        public CustomTileInfo(MpqFileStream stream)
        {
            Int0 = stream.ReadValueS32();
            Int1 = stream.ReadValueS32();
            Int2 = stream.ReadValueS32();
            V0 = new Vector2D(stream);
            stream.Position += (5 * 4);
        }
    }

    //public class CustomTileCell // we're not using this yet. /raist.
    //{
    //    public int Int0;
    //    public int Int1;
    //    public int Int2;
    //    public int SNOScene;
    //    public int Int3;
    //    public int[] Int4;

    //    public CustomTileCell(MpqFileStream stream)
    //    {
    //        Int0 = stream.ReadInt32();
    //        Int1 = stream.ReadInt32();
    //        Int2 = stream.ReadInt32();
    //        SNOScene = stream.ReadInt32();
    //        Int3 = stream.ReadInt32();
    //        Int4 = new int[4];
    //        for (int i = 0; i < Int4.Length; i++)
    //        {
    //            Int4[i] = stream.ReadInt32();
    //        }
    //    }
    //}

    #endregion

    #region scene-cluster

    public class SceneClusterSet
    {
        public int ClusterCount { get; private set; }
        public List<SceneCluster> SceneClusters = new List<SceneCluster>();

        public SceneClusterSet(MpqFileStream stream)
        {
            this.ClusterCount = stream.ReadValueS32();
            stream.Position += (4*3);
            this.SceneClusters = stream.ReadSerializedData<SceneCluster>(this.ClusterCount);
        }
    }

    public class SceneCluster : ISerializableData
    {
        public string Name { get; private set; }
        public int ClusterId { get; private set; }
        public int GroupCount { get; private set; }
        public List<SubSceneGroup> SubSceneGroups = new List<SubSceneGroup>();
        public SubSceneGroup Default { get; private set; }

        public void Read(MpqFileStream stream)
        {
            this.Name = stream.ReadString(128, true);
            this.ClusterId = stream.ReadValueS32();
            this.GroupCount = stream.ReadValueS32();
            stream.Position += (2*4);
            this.SubSceneGroups = stream.ReadSerializedData<SubSceneGroup>(this.GroupCount);

            this.Default = new SubSceneGroup(stream);
        }
    }

    public class SubSceneGroup : ISerializableData
    {
        public int I0 { get; private set; }
        public int SubSceneCount { get; private set; }
        public List<SubSceneEntry> Entries = new List<SubSceneEntry>();

        public SubSceneGroup() { }

        public SubSceneGroup(MpqFileStream stream)
        {
            this.Read(stream);
        }

        public void Read(MpqFileStream stream)
        {
            this.I0 = stream.ReadValueS32();
            this.SubSceneCount = stream.ReadValueS32();
            stream.Position += (2 * 4);
            this.Entries = stream.ReadSerializedData<SubSceneEntry>(this.SubSceneCount);
        }
    }

    public class SubSceneEntry : ISerializableData
    {
        public int SNOScene { get; private set; }
        public int Probability { get; private set; }
        public int LabelCount { get; private set; }
        public List<SubSceneLabel> Labels = new List<SubSceneLabel>();

        public void Read(MpqFileStream stream)
        {
            this.SNOScene = stream.ReadValueS32();
            this.Probability = stream.ReadValueS32();
            stream.Position += (3 * 4);
            this.LabelCount = stream.ReadValueS32();
            this.Labels = stream.ReadSerializedData<SubSceneLabel>(this.LabelCount);
        }
    }

    public class SubSceneLabel : ISerializableData
    {
        public int GBId { get; private set; }
        public int I0 { get; private set; }

        public void Read(MpqFileStream stream)
        {
            GBId = stream.ReadValueS32();
            I0 = stream.ReadValueS32();
        }
    }

    #endregion

    #region others

    public class LabelRuleSet
    {
        public int Rulecount { get; private set; }
        public List<LabelRule> LabelRules = new List<LabelRule>();

        public LabelRuleSet(MpqFileStream stream)
        {
            Rulecount = stream.ReadValueS32();
            stream.Position += (3 * 4);
            this.LabelRules = stream.ReadSerializedData<LabelRule>(this.Rulecount);
        }
    }

    public class LabelRule : ISerializableData
    {
        public string Name { get; private set; }
        public LabelCondition LabelCondition { get; private set; }
        public int Int0 { get; private set; }
        public int LabelCount { get; private set; }
        public List<LabelEntry> Entries = new List<LabelEntry>();

        public void Read(MpqFileStream stream)
        {
            this.Name = stream.ReadString(128, true);
            LabelCondition = new LabelCondition(stream);
            stream.Position += 4;
            Int0 = stream.ReadValueS32();
            LabelCount = stream.ReadValueS32();
            stream.Position += (2 * 4);
            this.Entries = stream.ReadSerializedData<LabelEntry>(this.LabelCount);
        }
    }

    public class LabelEntry : ISerializableData
    {
        public int GBIdLabel { get; private set; }
        public int Int0 { get; private set; }
        public float Float0 { get; private set; }
        public int Int1 { get; private set; }
        public int Int2 { get; private set; }

        public void Read(MpqFileStream stream)
        {
            this.GBIdLabel = stream.ReadValueS32();
            Int0 = stream.ReadValueS32();
            Float0 = stream.ReadValueF32();
            Int1 = stream.ReadValueS32();
            Int2 = stream.ReadValueS32();
        }
    }

    public class LabelCondition
    {
        public int DT_ENUM0 { get; private set; }
        public int Int0 { get; private set; }
        public int[] Int1 { get; private set; }

        public LabelCondition(MpqFileStream stream)
        {
            Int0 = stream.ReadValueS32();
            Int1 = new int[4];

            for (int i = 0; i < Int1.Length; i++)
            {
                Int1[i] = stream.ReadValueS32();
            }
        }
    }

    public class Environment
    {
        /*public RGBAColor RGBAColor0;
        public PostFXParams PostFXParams1;
        public int int2;
        public int int3;
        public UberMaterial UberMaterial4;
        public int snoMusic;
        public int snoCombatMusic;
        public int snoAmbient;
        public int snoReverb;
        public int snoWeather;
        public int snoIrradianceTex;
        public int snoIrradianceTexDead;*/

        public int[] Env { get; private set; }
        public Environment(MpqFileStream stream)
        {
            Env = new int[46];
            for (int i = 0; i < 46; i++)
            {
                Env[i] = stream.ReadValueS32();
            }

            /* RGBAColor0 = new RGBAColor(stream);
             PostFXParams1 = new PostFXParams(stream);
             int2 = stream.ReadInt32();
             int3 = stream.ReadInt32();
             UberMaterial4 = new UberMaterial(stream);
             snoMusic = stream.ReadInt32();
             snoCombatMusic = stream.ReadInt32();
             snoAmbient = stream.ReadInt32();
             snoReverb = stream.ReadInt32();
             snoWeather = stream.ReadInt32();
             snoIrradianceTex = stream.ReadInt32();
             snoIrradianceTexDead = stream.ReadInt32();*/
        }
    }

    #endregion
}