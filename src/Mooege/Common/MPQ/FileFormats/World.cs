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
        public int Int1 { get; private set; }
        public int Int2 { get; private set; }

        [PersistentProperty("DRLGParams")]
        public List<DRLGParams> DRLGParams { get; private set; }
        [PersistentProperty("SceneParams")]
        public SceneParams SceneParams { get; private set; }
        public List<int> MarkerSets = new List<int>();
        public Environment Environment { get; private set; }
        [PersistentProperty("LabelRuleSet")]
        public LabelRuleSet LabelRuleSet { get; private set; }
        [PersistentProperty("SceneClusterSet")]
        public SceneClusterSet SceneClusterSet { get; private set; }
        [PersistentProperty("SNONavMeshFunctions", 4)]
        public int[] SNONavMeshFunctions = new int[4];
        public int Int4 { get; private set; }
        public float Float0 { get; private set; }
        public int Int5 { get; private set; }
        public int SNOScript { get; private set; }
        public int Int6 { get; private set; }

        public List<ServerData> ServerData { get; private set; }

        public World() { }

        public World(MpqFile file)
        {
            var stream = file.Open();

            this.Header = new Header(stream);

            this.IsGenerated = (stream.ReadValueS32() != 0);
            stream.Position += 8;
            this.ServerData = stream.ReadSerializedData<ServerData>();

            //this.Int1 = stream.ReadValueS32();
            //this.Int2 = stream.ReadValueS32();

            ////this.DRLGParams = stream.ReadSerializedData<DRLGParams>(); // I'm not sure if we can have a list of drlgparams. (then should be calling it with pointer.Size/120) /raist
            //stream.Position += 8; // skips reading of DRLG Pointer
            //stream.Position += (2 * 4);
            //this.SceneParams = stream.ReadSerializedItem<SceneParams>(); // I'm not sure if we can have a list of drlgparams. (then should be calling it with pointer.Size/24) /raist

            //stream.Position += (2 * 4);
            this.MarkerSets = stream.ReadSerializedInts();

            stream.Position += (14 * 4);
            this.Environment = new Environment(stream);

            //stream.Position += 4;
            //LabelRuleSet = new LabelRuleSet(stream);
            //this.Int4 = stream.ReadValueS32();

            //stream.Position += 4;
            //this.SceneClusterSet = new SceneClusterSet(stream);

            //for (int i = 0; i < SNONavMeshFunctions.Length; i++)
            //{
            //    SNONavMeshFunctions[i] = stream.ReadValueS32();
            //}

            //stream.Position += 4;
            Float0 = stream.ReadValueF32();
            Int5 = stream.ReadValueS32();
            //SNOScript = stream.ReadValueS32();
            //Int6 = stream.ReadValueS32();

            stream.Close();
        }
    }

    #region scene-params

    public class SceneParams : ISerializableData
    {
        [PersistentProperty("SceneChunks")]
        public List<SceneChunk> SceneChunks { get; private set; }
        [PersistentProperty("ChunkCount")]
        public int ChunkCount { get; set; }

        public SceneParams() { }

        public void Read(MpqFileStream stream)
        {
            this.SceneChunks = stream.ReadSerializedData<SceneChunk>();
            this.ChunkCount = stream.ReadValueS32();
            stream.Position += (3 * 4);
        }
    }

    public class SceneChunk : ISerializableData
    {
        [PersistentProperty("SNOHandle")]
        public SNOHandle SNOHandle { get; set; }
        [PersistentProperty("PRTransform")]
        public PRTransform PRTransform { get; set; }
        [PersistentProperty("SceneSpecification")]
        public SceneSpecification SceneSpecification { get; set; }

        public SceneChunk() { }

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

    public enum TileExits
    {
        West = 1,
        East = 2,
        North = 4,
        South = 8,
    }

    public enum TileTypes
    {
        Normal = 100,
        EventTile1 = 101, // Jar of souls? more? Deadend?
        EventTile2 = 102, // 1000 dead
        Entrance = 200,
        UEntrance1 = 201, // Defiled crypt what there?
        Exit = 300,
        Filler = 401
    }

    public class TileInfo : ISerializableData
    {
        [PersistentProperty("Int0")]
        public int ExitDirectionBits { get; private set; }

        [PersistentProperty("Int1")]
        public int TileType { get; private set; }

        [PersistentProperty("SNOScene")]
        public int SNOScene { get; private set; }

        [PersistentProperty("Int2")]
        public int Probability { get; private set; }

        [PersistentProperty("TagMap")]
        public TagMap TagMap { get; private set; }

        [PersistentProperty("CustomTileInfo")]
        public CustomTileInfo CustomTileInfo { get; private set; }

        public void Read(MpqFileStream stream)
        {
            ExitDirectionBits = stream.ReadValueS32();
            TileType = stream.ReadValueS32();
            SNOScene = stream.ReadValueS32();
            Probability = stream.ReadValueS32();
            this.TagMap = stream.ReadSerializedItem<TagMap>();

            stream.Position += (2 * 4);
            CustomTileInfo = new CustomTileInfo(stream);
        }
    }

    public enum CommandType
    {
        Waypoint = 0,
        BridleEntrance = 1,
        AddExit = 2,
        AddHub = 3,
        AddSpoke = 4,
        Group = 9, //used in DRLG to group tiles together
    }

    public class DRLGCommand : ISerializableData
    {
        [PersistentProperty("Name")]
        public string Name { get; private set; }

        [PersistentProperty("I0")]
        public int CommandType { get; private set; }

        [PersistentProperty("TagMap")]
        public TagMap TagMap { get; private set; }

        public void Read(MpqFileStream stream)
        {
            this.Name = stream.ReadString(128, true);
            CommandType = stream.ReadValueS32();
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
        [PersistentProperty("ClusterCount")]
        public int ClusterCount { get; private set; }
        [PersistentProperty("SceneClusters")]
        public List<SceneCluster> SceneClusters { get; private set; }

        public SceneClusterSet() { }

        public SceneClusterSet(MpqFileStream stream)
        {
            this.ClusterCount = stream.ReadValueS32();
            stream.Position += (4 * 3);
            this.SceneClusters = stream.ReadSerializedData<SceneCluster>();
        }
    }

    public class SceneCluster : ISerializableData
    {
        [PersistentProperty("Name")]
        public string Name { get; private set; }
        [PersistentProperty("ClusterId")]
        public int ClusterId { get; private set; }
        [PersistentProperty("GroupCount")]
        public int GroupCount { get; private set; }
        [PersistentProperty("SubSceneGroups")]
        public List<SubSceneGroup> SubSceneGroups { get; private set; }
        public SubSceneGroup Default { get; private set; }

        public SceneCluster() { }

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
        [PersistentProperty("I0")]
        public int I0 { get; private set; }
        [PersistentProperty("SubSceneCount")]
        public int SubSceneCount { get; private set; }
        [PersistentProperty("Entries")]
        public List<SubSceneEntry> Entries { get; private set; }

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
        [PersistentProperty("SNOScene")]
        public int SNOScene { get; private set; }
        [PersistentProperty("Probability")]
        public int Probability { get; private set; }
        [PersistentProperty("LabelCount")]
        public int LabelCount { get; private set; }
        [PersistentProperty("Labels")]
        public List<SubSceneLabel> Labels { get; private set; }

        public SubSceneEntry() { }

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
        [PersistentProperty("GBId")]
        public int GBId { get; private set; }
        [PersistentProperty("I0")]
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
        [PersistentProperty("Rulecount")]
        public int Rulecount { get; private set; }
        [PersistentProperty("LabelRules")]
        public List<LabelRule> LabelRules { get; private set; }

        public LabelRuleSet() { }

        public LabelRuleSet(MpqFileStream stream)
        {
            Rulecount = stream.ReadValueS32();
            stream.Position += (3 * 4);
            this.LabelRules = stream.ReadSerializedData<LabelRule>();
        }
    }

    public class LabelRule : ISerializableData
    {
        [PersistentProperty("Name")]
        public string Name { get; private set; }
        [PersistentProperty("LabelCondition")]
        public LabelCondition LabelCondition { get; private set; }
        [PersistentProperty("Int0")]
        public int Int0 { get; private set; }
        [PersistentProperty("LabelCount")]
        public int LabelCount { get; private set; }
        [PersistentProperty("Entries")]
        public List<LabelEntry> Entries { get; private set; }

        public LabelRule() { }

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
        [PersistentProperty("GBIdLabel")]
        public int GBIdLabel { get; private set; }
        [PersistentProperty("Int0")]
        public int Int0 { get; private set; }
        [PersistentProperty("Float0")]
        public float Float0 { get; private set; }
        [PersistentProperty("Int1")]
        public int Int1 { get; private set; }
        [PersistentProperty("Int2")]
        public int Int2 { get; private set; }

        public LabelEntry() { }

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
        [PersistentProperty("Enum0")]
        public DT_ENUM0 Enum0 { get; private set; }
        [PersistentProperty("Int0")]
        public int Int0 { get; private set; }
        [PersistentProperty("Int1",4)]
        public int[] Int1 { get; private set; }

        public LabelCondition() { }

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
        }
    }

    #endregion

    public class ServerData : ISerializableData
    {
        public List<DRLGParams> DRLGParams { get; private set; }
        public SceneParams SceneParams { get; private set; }
        public LabelRuleSet LabelRuleSet { get; private set; }
        public int Int1 { get; private set; }
        public SceneClusterSet SceneClusterSet { get; private set; }
        public int[] SNONavMeshFunctions = new int[4];
        public int SNOScript { get; private set; }
        public int Int2 { get; private set; }

        public void Read(MpqFileStream stream)
        {
            this.DRLGParams = stream.ReadSerializedData<DRLGParams>();
            stream.Position += 8;
            this.SceneParams = stream.ReadSerializedItem<SceneParams>();
            stream.Position += 8;
            LabelRuleSet = new LabelRuleSet(stream);
            this.Int1 = stream.ReadValueS32();
            this.SceneClusterSet = new SceneClusterSet(stream);
            for (int i = 0; i < SNONavMeshFunctions.Length; i++)
            {
                SNONavMeshFunctions[i] = stream.ReadValueS32();
            }
            SNOScript = stream.ReadValueS32();
            Int2 = stream.ReadValueS32();
        }
    }

}