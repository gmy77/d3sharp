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

using System;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using CrystalMpq;
using Mooege.Common.Extensions;
using Mooege.Net.GS.Message.Fields;

namespace Mooege.Common.MPQ
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

	public class SceneParams : ISerializableData
	{
		public List<SceneChunk> SceneChunks = new List<SceneChunk>();
		public int ChunkCount { get; private set; }   

		public void Read(MpqFileStream stream)
		{
			var pointer = stream.GetSerializedDataPointer();
			this.ChunkCount = stream.ReadInt32();
			stream.Position += (3 * 4);
			this.SceneChunks = stream.ReadSerializedData<SceneChunk>(pointer, this.ChunkCount);
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

			switch(this.Int0)
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

	   public class Marker : ISerializableData
	   {
		   public string Name;
		   int i0;
		   public PRTransform PRTransform;
		   public SNOName SNOName;
		   int i1;
		   public TagMap TM;
		   public List<int> MarkerLinks = new List<int>();
		   public void Read(MpqFileStream stream)
		   {
			   byte[] buf = new byte[128];
			  stream.Read(buf, 0, 128); Name = Encoding.ASCII.GetString(buf);
			   i0 = stream.ReadInt32();
			   PRTransform = new PRTransform(stream);
			   SNOName = new SNOName(stream);
			   this.TM = stream.ReadSerializedData<TagMap>();
			   stream.Position += (2 * 4);
			   i1 = stream.ReadInt32();
			   // Only the SerializeData is read in the 010 template, not the actual payload.
			   this.MarkerLinks = stream.ReadSerializedInts();
			   stream.Position += (3 * 4);
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
		public Quaternion Q;
		public Vector3D V;
		public PRTransform(MpqFileStream stream)
		{
			Q = new Quaternion(stream);
			V = new Vector3D(stream.ReadFloat(), stream.ReadFloat(), stream.ReadFloat());
		}
	}

	   public class SNOName
	   {
		   public SNOGroup SNOGroup { get; private set; }
		   public int SNOId { get; private set; }
		   public string Name { get; private set; }

		   public SNOName(MpqFileStream stream)
		   {
			   this.SNOGroup = (SNOGroup)stream.ReadInt32();
			   this.SNOId = stream.ReadInt32();

			   if (!MPQStorage.CoreData.Assets.ContainsKey(this.SNOGroup)) return; // it's here because of the SNOGroup 0, could it be the Act? /raist
			   this.Name = MPQStorage.CoreData.Assets[this.SNOGroup].ContainsKey(this.SNOId) ? MPQStorage.CoreData.Assets[this.SNOGroup][SNOId].Name : ""; // put it here because it seems we miss loading some scenes there /raist.
		   }
	   }
	   public class SubSceneLabel : ISerializableData
	   {
		   public int GBId;
		   public int I0;

		   public void Read(MpqFileStream stream)
		   {
			   GBId = stream.ReadInt32();
			   I0 = stream.ReadInt32();
		   }
	   }

	   public class SubSceneEntry: ISerializableData
	   {
		   public int SNOScene;
		   public int Probability;
		   public int LabelCount;
		   public List<SubSceneLabel> Labels = new List<SubSceneLabel>();
		   
		   public void Read(MpqFileStream stream)
		   {
			   this.SNOScene = stream.ReadInt32();
			   this.Probability = stream.ReadInt32();
			   stream.Position += (3 * 4);
			   this.LabelCount = stream.ReadInt32();
			   this.Labels = stream.ReadSerializedData<SubSceneLabel>(this.LabelCount);
		   }
	   }

	public class SubSceneGroup : ISerializableData
	{
		public int I0;
		public int SubSceneCount;
		public List<SubSceneEntry> Entries = new List<SubSceneEntry>();

		public SubSceneGroup() { }

		public SubSceneGroup(MpqFileStream stream)
		{
			this.Read(stream);
		}

		public void Read(MpqFileStream stream)
		{
			this.I0 = stream.ReadInt32();
			this.SubSceneCount = stream.ReadInt32();
			stream.Position += (2 * 4);
			this.Entries = stream.ReadSerializedData<SubSceneEntry>(this.SubSceneCount);
		}
	}

	public class SceneCluster : ISerializableData
	{
		public string Name;
		public int ClusterId;
		public int GroupCount;
		public List<SubSceneGroup> SubSceneGroups = new List<SubSceneGroup>();
		public SubSceneGroup Default;

		public void Read(MpqFileStream stream)
		{
			var buf = new byte[128];
			stream.Read(buf, 0, 128);
			this.Name = Encoding.ASCII.GetString(buf);
			this.ClusterId = stream.ReadInt32();
			this.GroupCount = stream.ReadInt32();
			stream.Position += (2 * 4);
			this.SubSceneGroups = stream.ReadSerializedData<SubSceneGroup>(this.GroupCount);

			this.Default = new SubSceneGroup(stream);
		}
	}
	public class SceneClusterSet
	{
		public int ClusterCount;
		public List<SceneCluster> SceneClusters = new List<SceneCluster>();
		
		public SceneClusterSet(MpqFileStream stream)
		{
			this.ClusterCount = stream.ReadInt32();
			stream.Position += (4 * 3);
			this.SceneClusters = stream.ReadSerializedData<SceneCluster>(this.ClusterCount);           
		}
	}

	public class SceneChunk : ISerializableData
	{
		public SNOName SNOName { get; private set; }
		public PRTransform Position { get; private set; }
		public SceneSpecification SceneSpecification { get; private set; }
	
		public void Read(MpqFileStream stream)
		{
			this.SNOName = new SNOName(stream);
			this.Position = new PRTransform(stream);
			this.SceneSpecification = new SceneSpecification(stream);
		}
	}

	public class SceneSpecification
	{
		public int CellZ;
		public Vector2D V0;
		public int[] SNOLevelAreas;
		public int SNOPrevWorld;
		public int Int1;
		public int SNOPrevLevelArea;
		public int SNONextWorld;
		public int Int3;
		public int SNONextLevelArea;
		public int SNOMusic;
		public int SNOCombatMusic;
		public int SNOAmbient;
		public int SNOReverb;
		public int SNOWeather;
		public int SNOPresetWorld;
		public int Int4;
		public int Int5;
		public int Int6;
		public int Int7;
		public SceneCachedValues SceneCachedValues;

		public SceneSpecification(MpqFileStream stream)
		{
			CellZ = stream.ReadInt32();
			V0 = new Vector2D(stream);
			SNOLevelAreas = new int[4];

			for (int i = 0; i < SNOLevelAreas.Length; i++)
			{
				SNOLevelAreas[i] = stream.ReadInt32();
			}

			SNOPrevWorld = stream.ReadInt32();
			Int1 = stream.ReadInt32();
			SNOPrevLevelArea = stream.ReadInt32();
			SNONextWorld = stream.ReadInt32();
			Int3 = stream.ReadInt32();
			SNONextLevelArea = stream.ReadInt32();
			SNOMusic = stream.ReadInt32();
			SNOCombatMusic = stream.ReadInt32();
			SNOAmbient = stream.ReadInt32();
			SNOReverb = stream.ReadInt32();
			SNOWeather = stream.ReadInt32();
			SNOPresetWorld = stream.ReadInt32();
			Int4 = stream.ReadInt32();
			Int5 = stream.ReadInt32();
			Int6 = stream.ReadInt32();

			stream.Position += (9 * 4);

			Int7 = stream.ReadInt32();
			SceneCachedValues = new SceneCachedValues(stream);
		}
	}

	public class SceneCachedValues
	{
		public int Int0;
		public int Int1;
		public int Int2;
		public AABB AABB1;
		public AABB AABB2;
		public int[] Int5;
		public int Int6;

		public SceneCachedValues(MpqFileStream stream)
		{
			Int0 = stream.ReadInt32();
			Int1 = stream.ReadInt32();
			Int2 = stream.ReadInt32();
			AABB1 = new AABB(stream);
			AABB2 = new AABB(stream);
			Int5 = new int[4];
			for (int i = 0; i < Int5.Length; i++)
			{
				Int5[i] = stream.ReadInt32();
			}
			Int6 = stream.ReadInt32();
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

	public class LabelEntry : ISerializableData
	{
		public int GBIdLabel;
		public int Int0;
		public float Float0;
		public int Int1;
		public int Int2;

		public void Read(MpqFileStream stream)
		{
			this.GBIdLabel = stream.ReadInt32();
			Int0 = stream.ReadInt32();
			Float0 = stream.ReadFloat();
			Int1 = stream.ReadInt32();
			Int2 = stream.ReadInt32();
		}
	}

	public class LabelCondition
	{
		public int DT_ENUM0;
		public int Int0;
		public int[] Int1;

		public LabelCondition(MpqFileStream stream)
		{
			Int0 = stream.ReadInt32();
			Int1 = new int[4];

			for (int i = 0; i < Int1.Length; i++)
			{
				Int1[i] = stream.ReadInt32();
			}
		}
	}
	public class LabelRule : ISerializableData
	{
		public string Name;
		public LabelCondition LabelCondition;
		public int Int0;
		public int LabelCount;
		public List<LabelEntry> Entries = new List<LabelEntry>();

		public void Read(MpqFileStream stream)
		{
			var buf = new byte[128];
			stream.Read(buf, 0, 128); 
			this.Name = Encoding.ASCII.GetString(buf);
			LabelCondition = new LabelCondition(stream);
			stream.Position += 4;
			Int0 = stream.ReadInt32();
			LabelCount = stream.ReadInt32();
			stream.Position += (2 * 4);
			this.Entries = stream.ReadSerializedData<LabelEntry>(this.LabelCount);
		}
	}

	public class LabelRuleSet
	{
		public int Rulecount;
		public List<LabelRule> LabelRules = new List<LabelRule>();

		public LabelRuleSet(MpqFileStream stream)
		{
			Rulecount = stream.ReadInt32();
			stream.Position += (3 * 4);
			this.LabelRules = stream.ReadSerializedData<LabelRule>(this.Rulecount);
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

	public class CustomTileInfo
	{
		public int Int0;
		public int Int1;
		public int Int2;
		public Vector2D V0;
		
		public CustomTileInfo(MpqFileStream stream)
		{
			Int0 = stream.ReadInt32();
			Int1 = stream.ReadInt32();
			Int2 = stream.ReadInt32();
			V0 = new Vector2D(stream);
			stream.Position += (5 * 4);
		}
	}

	public class TileInfo : ISerializableData
	{
		public int Int0;
		public int Int1;
		public int SNOScene;
		public int Int2;
		public TagMap TileTagMap;
		public CustomTileInfo CustomTileInfo;

		public void Read(MpqFileStream stream)
		{
			Int0 = stream.ReadInt32();
			Int1 = stream.ReadInt32();
			SNOScene = stream.ReadInt32();
			Int2 = stream.ReadInt32();
			this.TileTagMap = stream.ReadSerializedData<TagMap>();

			stream.Position += (2 * 4);
			CustomTileInfo = new CustomTileInfo(stream);            
		}
	}

	public class DRLGCommand : ISerializableData
	{
		public string Name;
		public int Int0;
		public TagMap CommandTagMap;

		public void Read(MpqFileStream stream)
		{
			var buf = new byte[128];
			stream.Read(buf, 0, 128);
			Name = Encoding.ASCII.GetString(buf);
			Int0 = stream.ReadInt32();
			this.CommandTagMap = stream.ReadSerializedData<TagMap>();
			stream.Position += (3 * 4);
		}
	}

	public class DRLGParams : ISerializableData
	{
		public List<TileInfo> DRLGTiles = new List<TileInfo>();        
		public int CommandCount;
		public List<DRLGCommand> DRLGCommands = new List<DRLGCommand>();
		public List<int> ParentIndices = new List<int>();
		public TagMap DRLGTagMap;

		public void Read(MpqFileStream stream)
		{
			var pointer = stream.GetSerializedDataPointer();
			this.DRLGTiles = stream.ReadSerializedData<TileInfo>(pointer, pointer.Size/72);

			stream.Position += (14 * 4);
			this.CommandCount = stream.ReadInt32();
			this.DRLGCommands = stream.ReadSerializedData<DRLGCommand>(this.CommandCount);

			stream.Position += (3 * 4);
			this.ParentIndices = stream.ReadSerializedInts();

			stream.Position += (2 * 4);
			this.DRLGTagMap = stream.ReadSerializedData<TagMap>();
		}
	}

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
	public class LookLink
	{
		public char[] Name;
		public LookLink(MpqFileStream stream)
		{
			Name = new char[64];
			 for (int i = 0; i < 64; i++)
			{
				Name[i] = (char) stream.ReadByte();
			}
		}
	}
	public class ConstraintLink
	{
		public char[] Name;
		public ConstraintLink(MpqFileStream stream)
		{
			Name = new char[64];
			for (int i = 0; i < 64; i++)
			{
				Name[i] = (char)stream.ReadByte();
			}
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
			stream.Read(buf,0,4);
			byte0 = buf[0];
			byte1 = buf[1];
			byte2 = buf[2];
			byte3 = buf[3];
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
		public LookLink LookLink;
		public ConstraintLink ConstraintLink;
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
			LookLink = new LookLink(stream);
			ConstraintLink = new ConstraintLink(stream);
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
}
