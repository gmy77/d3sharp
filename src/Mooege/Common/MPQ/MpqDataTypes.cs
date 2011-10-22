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
}
