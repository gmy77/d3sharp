using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CrystalMpq;
using Mooege.Common.Extensions;
using Mooege.Net.GS.Message.Fields;
namespace Mooege.Common.MPQ.DataTypes
{
	public  class SerializeData
	   {
		   public readonly int Offset; // format hex? - Darklotus
		   public readonly int Size;

		   public SerializeData(MpqFileStream stream)
		   {
			   Offset = stream.ReadInt32();
			   Size = stream.ReadInt32();
		   }
	   }

	public class Header
	{
	   int DeadBeef;
	   public int SnoType;
	   int unknown1, unknown2;
	   public Header(MpqFileStream stream)
	   {
		   DeadBeef = stream.ReadInt32();
		   SnoType  = stream.ReadInt32();
		   unknown1 = stream.ReadInt32();
		   unknown2 = stream.ReadInt32();
	   }
	}
	public class TagMapEntry
	   {
		   int i0, i1;
		   float f0;
		   public TagMapEntry(MpqFileStream stream)
		   {
			   i0 = stream.ReadInt32();
			   i1 = stream.ReadInt32();
			   f0 = stream.ReadFloat();
		   }
	   }
	   public class TagMap
	   {
		   int tagmapsize;
		   TagMapEntry[] TagMapEntry;
		   public TagMap(MpqFileStream stream)
		   {
			   tagmapsize = stream.ReadInt32();
			   TagMapEntry = new TagMapEntry[tagmapsize];
			   for (int i = 0; i < tagmapsize; i++)
			   {
				   TagMapEntry[i] = new TagMapEntry(stream);
			   }
		   }
	   }
	   public class AABB_ // Ambiogous refrence fix me - DarkLotus
	   {
		   public Vector3D Min { get; private set; }
		   public Vector3D Max { get; private set; }

		   public AABB_(MpqFileStream stream)
		   {
			   this.Min = new Vector3D(stream.ReadFloat(), stream.ReadFloat(), stream.ReadFloat());
			   this.Max = new Vector3D(stream.ReadFloat(), stream.ReadFloat(), stream.ReadFloat());
		   }
	   }

	   public class Marker
	   {
		   public string Name;
		   int i0;
		   public PRTransform PRTransform;
		   public SNOName SNOName;
		   public SerializeData serTagMap;
		   // Un sure about these 3 ints, 010template isnt the same as snodata.xml - DarkLotus
		   int TagMap;
		   int i1,i2;
		   SerializeData serMarkerLinks;
		   public TagMap TM;
		   public Marker(MpqFileStream stream)
		   {
			   byte[] buf = new byte[128];
			   stream.Read(buf, 0, 128); Name = Encoding.ASCII.GetString(buf);
			   i0 = stream.ReadInt32();
			   PRTransform = new PRTransform(stream);
			   SNOName = new SNOName(stream);
			   serTagMap = new SerializeData(stream);
			   TagMap = stream.ReadInt32();
			   i1 = stream.ReadInt32();
			   i2 = stream.ReadInt32();
			   serMarkerLinks = new SerializeData(stream);
			   stream.Position += (3 * 4);
			   long x = stream.Position;
			   
			   if (serTagMap.Size > 0)
			   {
				   stream.Position = serTagMap.Offset + 16;
				   TM = new TagMap(stream);

			   }
			   stream.Position = x;
		   }
	   }
	   public class WeightedLook
	   {
		   string LookLink;
		   int i0;
		   public WeightedLook(MpqFileStream stream)
		   {
			   byte[] buf = new byte[64];
			   stream.Read(buf, 0, 64); LookLink = Encoding.ASCII.GetString(buf);
			   i0 = stream.ReadInt32();

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
		   public float ax1;
		   public float ax2;
		   public AxialCylinder(MpqFileStream stream)
		   {
			   this.Position = new Vector3D(stream.ReadFloat(), stream.ReadFloat(), stream.ReadFloat());
			   ax1 = stream.ReadFloat();
			   ax2 = stream.ReadFloat();
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
	   public class Quaternion{
		public float float0;
		public Vector3D Vector3D1;
		public Quaternion(MpqFileStream stream)
		{
			float0 = stream.ReadFloat();
			Vector3D1 = new Vector3D(stream.ReadFloat(), stream.ReadFloat(), stream.ReadFloat());
		}
	}
	   public class PRTransform{
		public Quaternion Quaternion0;
		public Vector3D Vector3D1;
		public PRTransform(MpqFileStream stream)
		{
			Quaternion0 = new Quaternion(stream);
			Vector3D1 = new Vector3D(stream.ReadFloat(), stream.ReadFloat(), stream.ReadFloat());
		}
	}

	   public class SNOName
	   {
		   public int int0;
		   public int int1;
		   public SNOName(MpqFileStream stream)
		   {
			   int0 = stream.ReadInt32();
			   int1 = stream.ReadInt32();
		   }
	   }
	   public class SubSceneLabel
	   {
		   public int GBID;
		   public int i0;
		   public SubSceneLabel(MpqFileStream stream)
		   {
			   GBID = stream.ReadInt32();
			   i0 = stream.ReadInt32();
		   }
	   }

	   public class SubSceneEntry
	   {
		   public int snoScene;
		   public int Probability;
		   public int LabelCount;
		   private SerializeData serLabels;
		   public SubSceneLabel[] Labels;
		   public SubSceneEntry(MpqFileStream stream)
		   {
			   
			   snoScene = stream.ReadInt32();
			   Probability = stream.ReadInt32();
			   stream.Position += (3 * 4);
			   LabelCount = stream.ReadInt32();
			   serLabels = new SerializeData(stream);
			   long x = stream.Position;
			   if (serLabels.Size > 0)
			   {
				   Labels = new SubSceneLabel[serLabels.Size];
				   stream.Position = serLabels.Offset + 16;
				   for (int i = 0; i < serLabels.Size; i++)
				   {
					   Labels[i] = new SubSceneLabel(stream);
				   }
			   }
			   stream.Position = x; 
		   }
	   }

	public class SubSceneGroup
	{
		int i0, SubSceneCount;
		SerializeData serSubScenes;
		SubSceneEntry[] SubScene;
		public SubSceneGroup(MpqFileStream stream)
		{
			i0 = stream.ReadInt32();
			SubSceneCount = stream.ReadInt32();
			stream.Position += (2 * 4);
			serSubScenes = new SerializeData(stream);
			long x = stream.Position;

			if (serSubScenes.Size > 0)
			{
				SubScene = new SubSceneEntry[serSubScenes.Size];
				stream.Position = serSubScenes.Offset + 16;
				for (int i = 0; i < serSubScenes.Size; i++)
				{
					SubScene[i] = new SubSceneEntry(stream);
				}
			}
			stream.Position = x;
		}
	}
	public class SceneCluster
	{
		public string Name;
		public int ClusterID;
		public int GroupCount;
		public SerializeData serSubSceneGroups;
		public SubSceneGroup[] Group;
		public SubSceneGroup Default;
		public SceneCluster(MpqFileStream stream)
		{
			byte[] buf = new byte[128];
			stream.Read(buf, 0, 128); Name = Encoding.ASCII.GetString(buf);

			ClusterID = stream.ReadInt32();
			GroupCount = stream.ReadInt32();
			stream.Position += (2 * 4);
			serSubSceneGroups = new SerializeData(stream);

			long x = stream.Position;
			if (serSubSceneGroups.Size > 0)
			{
				Group = new SubSceneGroup[GroupCount];
				stream.Position = serSubSceneGroups.Offset + 16;
				for (int i = 0; i < GroupCount; i++)
				{
					Group[i] = new SubSceneGroup(stream);
				}
			}
			stream.Position = x;
			Default = new SubSceneGroup(stream);
		}
	}
	public class SceneClusterSet
	{
		public int ClusterCount;
		public SerializeData serClusters;
		public SceneCluster[] SceneCluster;
		public SceneClusterSet(MpqFileStream stream)
		{

			ClusterCount = stream.ReadInt32();
			stream.Position += (4 * 3);
			serClusters = new SerializeData(stream);
			long x = stream.Position;
			if (serClusters.Size > 0)
			{
				SceneCluster = new SceneCluster[ClusterCount];
				stream.Position = serClusters.Offset + 16;
				for (int i = 0; i < ClusterCount; i++)
				{
					SceneCluster[i] = new SceneCluster(stream);
				}
			}
			stream.Position = x;
		}
	}
	public class SceneChunk
	{
		public SNOName SNOName;
		public PRTransform Position;
		public SceneSpecification_ f;
		public SceneChunk(MpqFileStream stream)
		{
			SNOName = new SNOName(stream);
			Position = new PRTransform(stream);   
			f = new SceneSpecification_(stream);
		}
	}

	public class SceneSpecification_
	{
		public int CellZ;
		public Vector2D Vector2D1;
		public int[] SnoLevelAreas;
		public int snoPrevWorld;
		public int int2;
		public int snoPrevLevelArea;
		public int snoNextWorld;
		public int int3;
		public int snoNextLevelArea;
		public int snoMusic;
		public int snoCombatMusic;
		public int snoAmbient;
		public int snoReverb;
		public int snoWeather;
		public int snoPresetWorld;
		public int int4;
		public int int5;
		public int int6;
		public int int7;
		public SceneCachedValues_ tCachedValues;
		public SceneSpecification_(MpqFileStream stream)
		{
			CellZ = stream.ReadInt32();
			Vector2D1 = new Vector2D(stream);
			SnoLevelAreas = new int[4];
			for (int i = 0; i < SnoLevelAreas.Length; i++)
			{
				SnoLevelAreas[i] = stream.ReadInt32();
			}
			snoPrevWorld = stream.ReadInt32();
			int2 = stream.ReadInt32();
			snoPrevLevelArea = stream.ReadInt32();
			snoNextWorld = stream.ReadInt32();
			int3 = stream.ReadInt32();
			snoNextLevelArea = stream.ReadInt32();
			snoMusic = stream.ReadInt32();
			snoCombatMusic = stream.ReadInt32();
			snoAmbient = stream.ReadInt32();
			snoReverb = stream.ReadInt32();
			snoWeather = stream.ReadInt32();
			snoPresetWorld = stream.ReadInt32();
			int4 = stream.ReadInt32();
			int5 = stream.ReadInt32();
			int6 = stream.ReadInt32();
			int7 = stream.ReadInt32();
			tCachedValues = new SceneCachedValues_(stream);
		}
	}
	public class SceneCachedValues_
	{
		public int int0;
		public int int1;
		public int int2;
		public AABB_ AABB3;
		public AABB_ AABB4;
		public int[] int5;
		public int int6;
		public SceneCachedValues_(MpqFileStream stream)
		{
			int0 = stream.ReadInt32();
			int1 = stream.ReadInt32();
			int2 = stream.ReadInt32();
			AABB3 = new AABB_(stream);
			AABB4 = new AABB_(stream);
			int5 = new int[4];
			for (int i = 0; i < int5.Length; i++)
			{
				int5[i] = stream.ReadInt32();
			}
			int6 = stream.ReadInt32();
		}
	}
	public class SceneParams
	{
		public SerializeData serSceneChunks;
		public int ChunkCount;
		public SceneChunk[] SceneChunks;
		
		public SceneParams(MpqFileStream stream)
		{
			serSceneChunks = new SerializeData(stream);
			ChunkCount = stream.ReadInt32();
			stream.Position += (3 * 4);
			long x = stream.Position;
			if (serSceneChunks.Size > 0)
			{
				SceneChunks = new SceneChunk[ChunkCount];
				stream.Position = serSceneChunks.Offset + 16;
				for (int i = 0; i < ChunkCount; i++)
				{
					SceneChunks[i] = new SceneChunk(stream);
				}
			}
			stream.Position = x;
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
		public int[] Env;
		public Environment(MpqFileStream stream)
		{
			Env = new int[46];
			for (int i = 0; i < 46; i++)
			{
				Env[i] = stream.ReadInt32();
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
	public class PostFXParams
	{
		public float[] float0;
		public float[] float1;
		public PostFXParams(MpqFileStream stream)
		{
			float0 = new float[4];
			for (int i = 0; i < float0.Length; i++)
			{
				float0[i] = stream.ReadInt32();
			}
			float1 = new float[4];
			for (int i = 0; i < float1.Length; i++)
			{
				float1[i] = stream.ReadInt32();
			}
		}
	}
	public class LabelEntry
	{
		public int gbidLabel;
		public int int0;
		public float float1;
		public int int2;
		public int int3;
		public LabelEntry(MpqFileStream stream)
		{
			int0 = stream.ReadInt32();
			float1 = stream.ReadFloat();
			int2 = stream.ReadInt32();
			int3 = stream.ReadInt32();
		}
	}
	public class LabelCondition
	{
		public int DT_ENUM0;
		public int int1;
		public int[] int2;
		public LabelCondition(MpqFileStream stream)
		{
			int1 = stream.ReadInt32();
			int2 = new int[4];
			for (int i = 0; i < int2.Length; i++)
			{
				int2[i] = stream.ReadInt32();
			}
		}
	}
	public class LabelRule
	{
		public string Name;
		public LabelCondition LabelCondition1;
		public int i0;
		public int labelcount;
		public SerializeData serEntries;
		public LabelEntry[] LabelEntry;
		public LabelRule(MpqFileStream stream)
		{
			byte[] buf = new byte[128];
			stream.Read(buf, 0, 128); Name = Encoding.ASCII.GetString(buf);
			LabelCondition1 = new LabelCondition(stream);
			i0 = stream.ReadInt32();
			labelcount = stream.ReadInt32();
			stream.Position += (2 * 4);
			serEntries = new SerializeData(stream);
			long x = stream.Position;
			if (labelcount > 0)
			{
				LabelEntry = new LabelEntry[labelcount];
				stream.Position = serEntries.Offset + 16;
				for (int i = 0; i < labelcount; i++)
				{
					LabelEntry[i] = new LabelEntry(stream);
				}
			}
			stream.Position = x;
		}
	}
	public class LabelRuleSet
	{
		public int Rulecount;
		public SerializeData serRules;
		public LabelRule[] LabelRule;
		public LabelRuleSet(MpqFileStream stream)
		{
			Rulecount = stream.ReadInt32();
			stream.Position += (3 * 4);
			serRules = new SerializeData(stream);
			long x = stream.Position;
			if (Rulecount > 0)
			{
				LabelRule = new LabelRule[Rulecount];
				stream.Position = serRules.Offset + 16;
				for (int i = 0; i < Rulecount; i++)
				{
					LabelRule[i] = new LabelRule(stream);
				}
			}
			stream.Position = x;
		}
	}
    public class CustomTileCell{
		public int int0;
		public int int1;
		public int int2;
		public int snoScene;
		public int int3;
		public int[] int4;
		public CustomTileCell(MpqFileStream stream)
		{
			int0 = stream.ReadInt32();
			int1 = stream.ReadInt32();
			int2 = stream.ReadInt32();
			snoScene = stream.ReadInt32();
			int3 = stream.ReadInt32();
			int4 = new int[4];
			for(int i = 0; i < int4.Length;i++) {
				int4[i] = stream.ReadInt32(); }
		}
	}
    public class CustomTileInfo{
		public int int0;
		public int int1;
		public int int2;
		public Vector2D v0;
		public SerializeData serTiles;
        public CustomTileInfo(MpqFileStream stream)
		{
			int0 = stream.ReadInt32();
			int1 = stream.ReadInt32();
			int2 = stream.ReadInt32();
            v0 = new Vector2D(stream);
            serTiles = new SerializeData(stream);
            stream.Position += (3 * 4);
		}
	}

    public class TileInfo
    {
        public int int0;
        public int int1;
        public int SNOScene;
        public int int2;
        public SerializeData serTileTagMap;
        public TagMap TileTagMap;
        public CustomTileInfo CustomTileInfo3;
        public TileInfo(MpqFileStream stream)
        {
            int0 = stream.ReadInt32();
            int1 = stream.ReadInt32();
            SNOScene = stream.ReadInt32();
            int2 = stream.ReadInt32();           
            serTileTagMap = new SerializeData(stream);
            long x = stream.Position;
            if (serTileTagMap.Size > 0)
            {
                stream.Position = serTileTagMap.Offset + 16;
                TileTagMap = new TagMap(stream);
            }
            stream.Position = x; ;
            CustomTileInfo3 = new CustomTileInfo(stream);
        }
    }
    public class DRLGCommand
    {
        public string Name;
        public int int1;
        public SerializeData serCommandTagMap;
        public TagMap CommandTagMap;
        public DRLGCommand(MpqFileStream stream)
        {
            byte[] buf = new byte[128];
            stream.Read(buf, 0, 128); Name = Encoding.ASCII.GetString(buf);
            int1 = stream.ReadInt32();
            serCommandTagMap = new SerializeData(stream);
            long x = stream.Position;
            if (serCommandTagMap.Size > 0)
            {
                stream.Position = serCommandTagMap.Offset + 16;
                CommandTagMap = new TagMap(stream);
            }
            stream.Position = x; ;
        }
    }
    public class DRLGParams
    {
        public SerializeData serTiles;
        public TileInfo[] TileInfo;
        public int CommandCount;
        public SerializeData serCommands;
        public DRLGCommand[] Command;
        public SerializeData serParentIndices;
        public int[] ParentIndices;
        public SerializeData serDRLGTagMap;
        public TagMap DRLGTagMap;
        public DRLGParams(MpqFileStream stream)
        {
            serTiles = new SerializeData(stream);
            long x = stream.Position;
            if (serTiles.Size > 0)
            {
                TileInfo = new TileInfo[serTiles.Size / 72];
                stream.Position = serTiles.Offset + 16;
                for (int i = 0; i < serTiles.Size/72; i++)
                {
                    TileInfo[i] = new TileInfo(stream);
                }
            }
            stream.Position = x;
            stream.Position += (14 * 4);
            CommandCount = stream.ReadInt32();
            serCommands = new SerializeData(stream);
            if (serCommands.Size > 0)
            {
                x = stream.Position;
                Command = new DRLGCommand[CommandCount];
                stream.Position = serCommands.Offset + 16;
                for (int i = 0; i < CommandCount; i++)
                {
                    Command[i] = new DRLGCommand(stream);
                }
            }
            stream.Position = x;
            stream.Position += (3 * 4);
            serParentIndices = new SerializeData(stream);
            stream.Position += (2 * 4);
            ParentIndices = new int[CommandCount];
            x = stream.Position;
            stream.Position = serParentIndices.Offset + 16;
            for (int i = 0; i < CommandCount; i++)
            {
                ParentIndices[i] = stream.ReadInt32();
            }
            stream.Position = x;
           
            serDRLGTagMap = new SerializeData(stream);
            DRLGTagMap = new TagMap(stream);
        }
    }
}
