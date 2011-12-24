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
using Mooege.Core.GS.Common.Types.TagMap;
using Mooege.Common.Storage;

namespace Mooege.Common.MPQ.FileFormats
{
    [FileFormat(SNOGroup.Worlds)]
    public class World : FileFormat
    {
        public Header Header { get; private set; }
        public bool IsGenerated { get; private set; }
        public List<WorldServerData> ServerData { get; private set; }
        public List<int> MarkerSets = new List<int>();
        public Environment Environment { get; private set; }
        public float F0 { get; private set; }
        public int I0 { get; private set; }

        public World(MpqFile file)
        {
            var stream = file.Open();

            this.Header = new Header(stream);

            this.IsGenerated = (stream.ReadValueS32() != 0);

            stream.Position += 8;
            this.ServerData = stream.ReadSerializedData<WorldServerData>();

            this.MarkerSets = stream.ReadSerializedInts();
            stream.Position += (14 * 4);

            this.Environment = new Environment(stream); //96 4 bytes, 8 floats, ++ more
            F0 = stream.ReadValueF32(); //280
            I0 = stream.ReadValueS32(); //284

            stream.Close();
        }
    }

    #region server-data
    public class WorldServerData : ISerializableData
    {
        [PersistentProperty("DRLGParams")]
        public List<DRLGParams> DRLGParams { get; private set; }
        public List<SceneParams> SceneParams { get; private set; }
        public LabelRuleSet LabelRuleSet { get; private set; }
        public int Int0 { get; private set; }
        public SceneClusterSet SceneClusterSet { get; private set; }
        public int[] SNONavMeshFunctions { get; private set; }
        public int SNOScript { get; private set; }
        public int Int1 { get; private set; }

        public void Read(MpqFileStream stream)
        {
            this.DRLGParams = stream.ReadSerializedData<DRLGParams>();
            stream.Position += 8;
            this.SceneParams = stream.ReadSerializedData<SceneParams>();
            stream.Position += 8;

            LabelRuleSet = new LabelRuleSet(stream); //32
            this.Int0 = stream.ReadValueS32(); //56
            stream.Position += 4;
            this.SceneClusterSet = new SceneClusterSet(stream); //64

            SNONavMeshFunctions = new int[4]; //88
            for (int i = 0; i < SNONavMeshFunctions.Length; i++)
            {
                SNONavMeshFunctions[i] = stream.ReadValueS32();
            }

            stream.Position += 4;
            SNOScript = stream.ReadValueS32(); //104
            Int1 = stream.ReadValueS32(); //108
        }
    }
    #endregion

    #region scene-params

    public class SceneParams : ISerializableData
    {
        public List<SceneChunk> SceneChunks = new List<SceneChunk>();
        public int ChunkCount { get; private set; }

        public void Read(MpqFileStream stream)
        {
            this.SceneChunks = stream.ReadSerializedData<SceneChunk>();
            this.ChunkCount = stream.ReadValueS32();
            stream.Position += (3 * 4);
        }
    }

    public class SceneChunk : ISerializableData
    {
        public SNOHandle SNOHandle { get; private set; }
        public PRTransform PRTransform { get; private set; }
        public SceneSpecification SceneSpecification { get; private set; }

        public void Read(MpqFileStream stream)
        {
            this.SNOHandle = new SNOHandle(stream);
            this.PRTransform = new PRTransform(stream);
            this.SceneSpecification = new SceneSpecification(stream);
        }
    }

    #endregion

    #region drlg-params

    public class DRLGParams : ISerializableData
    {
        [PersistentProperty("Tiles")]
        public List<TileInfo> Tiles { get; private set; }

        [PersistentProperty("CommandCount")]
        public int CommandCount { get; private set; }

        [PersistentProperty("Commands")]
        public List<DRLGCommand> Commands { get; private set; }

        [PersistentProperty("ParentIndices")]
        public List<int> ParentIndices { get; private set; }

        [PersistentProperty("TagMap")]
        public TagMap TagMap { get; private set; }

        public void Read(MpqFileStream stream)
        {
            Tiles = stream.ReadSerializedData<TileInfo>();

            stream.Position += (14 * 4);
            this.CommandCount = stream.ReadValueS32();
            this.Commands = stream.ReadSerializedData<DRLGCommand>();

            stream.Position += (3 * 4);
            this.ParentIndices = stream.ReadSerializedInts();

            stream.Position += (2 * 4);
            this.TagMap = stream.ReadSerializedItem<TagMap>();
            stream.Position += (2 * 4);
        }
    }

    public class TileInfo : ISerializableData
    {
        [PersistentProperty("Int0")]
        public int Int0 { get; private set; }

        [PersistentProperty("Int1")]
        public int Int1 { get; private set; }

        [PersistentProperty("SNOScene")]
        public int SNOScene { get; private set; }

        [PersistentProperty("Int2")]
        public int Int2 { get; private set; }

        [PersistentProperty("TagMap")]
        public TagMap TagMap { get; private set; }

        [PersistentProperty("CustomTileInfo")]
        public CustomTileInfo CustomTileInfo { get; private set; }

        public void Read(MpqFileStream stream)
        {
            Int0 = stream.ReadValueS32();
            Int1 = stream.ReadValueS32();
            SNOScene = stream.ReadValueS32();
            Int2 = stream.ReadValueS32();
            this.TagMap = stream.ReadSerializedItem<TagMap>();

            stream.Position += (2 * 4);
            CustomTileInfo = new CustomTileInfo(stream);
        }
    }

    public class DRLGCommand : ISerializableData
    {
        [PersistentProperty("Name")]
        public string Name { get; private set; }

        [PersistentProperty("I0")]
        public int Int0 { get; private set; }

        [PersistentProperty("TagMap")]
        public TagMap TagMap { get; private set; }

        public void Read(MpqFileStream stream)
        {
            this.Name = stream.ReadString(128, true);
            Int0 = stream.ReadValueS32();
            this.TagMap = stream.ReadSerializedItem<TagMap>();
            stream.Position += (3 * 4);
        }
    }

    public class CustomTileInfo
    {
        [PersistentProperty("I0")]
        public int Int0 { get; private set; }

        [PersistentProperty("I1")]
        public int Int1 { get; private set; }

        [PersistentProperty("I2")]
        public int Int2 { get; private set; }

        [PersistentProperty("V0")]
        public Vector2D V0 { get; private set; }

        [PersistentProperty("CustomTileCells")]
        public List<CustomTileCell> CustomTileCells { get; private set; }

        public CustomTileInfo() { }

        public CustomTileInfo(MpqFileStream stream)
        {
            Int0 = stream.ReadValueS32();
            Int1 = stream.ReadValueS32();
            Int2 = stream.ReadValueS32();
            V0 = new Vector2D(stream);
            CustomTileCells = stream.ReadSerializedData<CustomTileCell>();
            stream.Position += (3 * 4);
        }
    }

    public class CustomTileCell : ISerializableData // we're not using this yet. /raist.
    {
        [PersistentProperty("I0")]
        public int Int0 { get; private set; }

        [PersistentProperty("I1")]
        public int Int1 { get; private set; }

        [PersistentProperty("I2")]
        public int Int2 { get; private set; }

        [PersistentProperty("SNOScene")]
        public int SNOScene { get; private set; }

        [PersistentProperty("I3")]
        public int Int3 { get; private set; }

        [PersistentProperty("I4", 4)]
        public int[] Int4 { get; private set; }

        public CustomTileCell() { }

        public void Read(MpqFileStream stream)
        {
            Int0 = stream.ReadValueS32();
            Int1 = stream.ReadValueS32();
            Int2 = stream.ReadValueS32();
            SNOScene = stream.ReadValueS32();
            Int3 = stream.ReadValueS32();
            Int4 = new int[4];
            for (int i = 0; i < Int4.Length; i++)
            {
                Int4[i] = stream.ReadValueS32();
            }
        }
    }

    #endregion

    #region scene-cluster

    public class SceneClusterSet
    {
        public int ClusterCount { get; private set; }
        public List<SceneCluster> SceneClusters = new List<SceneCluster>();

        public SceneClusterSet(MpqFileStream stream)
        {
            this.ClusterCount = stream.ReadValueS32();
            stream.Position += (4 * 3);
            this.SceneClusters = stream.ReadSerializedData<SceneCluster>();
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
            stream.Position += (2 * 4);
            this.SubSceneGroups = stream.ReadSerializedData<SubSceneGroup>();

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
            this.Entries = stream.ReadSerializedData<SubSceneEntry>();
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
            this.Labels = stream.ReadSerializedData<SubSceneLabel>();
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
            this.LabelRules = stream.ReadSerializedData<LabelRule>();
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
            Int0 = stream.ReadValueS32();
            LabelCount = stream.ReadValueS32();
            stream.Position += (2 * 4);
            this.Entries = stream.ReadSerializedData<LabelEntry>();
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
        public DT_ENUM0 Enum0 { get; private set; }
        public int Int0 { get; private set; }
        public int[] Int1 { get; private set; }

        public LabelCondition(MpqFileStream stream)
        {
            Enum0 = (DT_ENUM0)stream.ReadValueS32();
            Int0 = stream.ReadValueS32();
            Int1 = new int[4];

            for (int i = 0; i < Int1.Length; i++)
            {
                Int1[i] = stream.ReadValueS32();
            }
        }
    }

    public enum DT_ENUM0
    {
        Always = 0,
        GameDifficulty = 1,
        LabelAlreadySet = 2,
    }

    public class Environment
    {
        /*public RGBAColor RGBAColor0;
        public PostFXParams PostFXParams1;
        public int int2;
        public int int3;
        public UberMaterial UberMaterial4;
        */
        public int[] Unknown { get; private set; }
        public int snoMusic { get; private set; }
        public int snoCombatMusic { get; private set; }
        public int snoAmbient { get; private set; }
        public int snoReverb { get; private set; }
        public int snoWeather { get; private set; }
        public int snoIrradianceTex { get; private set; }
        public int snoIrradianceTexDead { get; private set; }

        public Environment(MpqFileStream stream)
        {
            Unknown = new int[38];
            for (int i = 0; i < 38; i++)
            {
                Unknown[i] = stream.ReadValueS32();
            }

            /* RGBAColor0 = new RGBAColor(stream);
             PostFXParams1 = new PostFXParams(stream);
             int2 = stream.ReadInt32();
             int3 = stream.ReadInt32();
             UberMaterial4 = new UberMaterial(stream);
             * */
            snoMusic = stream.ReadValueS32();
            snoCombatMusic = stream.ReadValueS32();
            snoAmbient = stream.ReadValueS32();
            snoReverb = stream.ReadValueS32();
            snoWeather = stream.ReadValueS32();
            snoIrradianceTex = stream.ReadValueS32();
            snoIrradianceTexDead = stream.ReadValueS32();
            stream.Position += 4;
        }
    }

    #endregion
}