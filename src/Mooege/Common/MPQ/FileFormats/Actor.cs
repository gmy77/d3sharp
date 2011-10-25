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
using Mooege.Net.GS.Message.Fields;
using AABB = Mooege.Common.MPQ.FileFormats.Types.AABB;
using System.Text;
using Mooege.Common.MPQ.FileFormats.Types;

namespace Mooege.Common.MPQ.FileFormats
{
    [FileFormat(SNOGroup.Actor)]
    public class Actor : FileFormat
    {
        public Header Header;
        public int Int0;

        /// <summary>
        /// Actor Type
        /// </summary>
        public ActorType Type;

        /// <summary>
        /// SNO for Apperance
        /// </summary>
        public int ApperanceSNO;

        public int PhysMeshSNO;
        public AxialCylinder Cylinder;
        public Sphere Sphere;
        public AABB AABBBounds;

        TagMap hTagMap;
        /// <summary>
        /// SNO for actor's animset.
        /// </summary>
        public int AnimSetSNO;

        /// <summary>
        /// MonterSNO if any.
        /// </summary>
        public int MonsterSNO;
        List<MsgTriggeredEvent> arMsgTriggeredEvents = new List<MsgTriggeredEvent>();

        public int Int1;
        public Vector3D V0;
        public WeightedLook[] Looks;
        public int PhysicsSNO;
        public int Int2, Int3;
        public float Float0, Float1, Float2;
        public ActorCollisionData ActorCollisionData;
        public int[] InventoryImages;
        public int Int4;
        public string CastingNotes;
        public string VoiceOverRole;
        public int BitField0;           // 25 bits - better this this would be an uint
        public int BitField1;           // 25 bits - better this this would be an uint
        public Actor(MpqFile file)
        {
            var stream = file.Open();
            Header = new Header(stream);

            this.Int0 = stream.ReadValueS32();
            this.Type = (ActorType)stream.ReadValueS32();
            this.ApperanceSNO = stream.ReadValueS32();
            this.PhysMeshSNO = stream.ReadValueS32();
            this.Cylinder = new AxialCylinder(stream);
            this.Sphere = new Sphere(stream);
            this.AABBBounds = new AABB(stream);

            this.hTagMap = stream.ReadSerializedItem<TagMap>();
            //var tagmap = stream.GetSerializedDataPointer(); // we need to read tagmap. /raist.
            stream.Position += (2 * 4);

            this.AnimSetSNO = stream.ReadValueS32();
            this.MonsterSNO = stream.ReadValueS32();


            arMsgTriggeredEvents = stream.ReadSerializedData<MsgTriggeredEvent>();
            //var msgTriggeredEvents = stream.GetSerializedDataPointer();

            this.Int1 = stream.ReadValueS32();
            stream.Position += (3 * 4);
            this.V0 = new Vector3D(stream.ReadValueF32(), stream.ReadValueF32(), stream.ReadValueF32());

            this.Looks = new WeightedLook[8];
            for (int i = 0; i < 8; i++)
            {
                this.Looks[i] = new WeightedLook(stream);
            }

            this.PhysicsSNO = stream.ReadValueS32();
            this.Int2 = stream.ReadValueS32();
            this.Int3 = stream.ReadValueS32();
            this.Float0 = stream.ReadValueF32();
            this.Float1 = stream.ReadValueF32();
            this.Float2 = stream.ReadValueF32();

            this.ActorCollisionData = new ActorCollisionData(stream);

            this.InventoryImages = new int[10]; //Was 5*8/4 - Darklotus
            for (int i = 0; i < 10; i++)
            {
                this.InventoryImages[i] = stream.ReadValueS32();
            }
            this.Int4 = stream.ReadValueS32();
            stream.Position += 4;
            BitField0 = stream.ReadValueS32();
            var serVOCastingNotes = stream.GetSerializedDataPointer();
            if (serVOCastingNotes.Size > 0)
            {
                byte[] buf = new byte[serVOCastingNotes.Size];
                long x = stream.Position;
                stream.Position = serVOCastingNotes.Offset + 16;
                stream.Read(buf, 0, serVOCastingNotes.Size); CastingNotes = Encoding.ASCII.GetString(buf);
                stream.Position = x;
            }

            BitField1 = stream.ReadValueS32();// not sure
            var serVORole = stream.GetSerializedDataPointer();
            if (serVORole.Size > 0)
            {
                long x = stream.Position;
                stream.Position = serVOCastingNotes.Offset + 16;
                byte[] buf = new byte[serVORole.Size];
                stream.Read(buf, 0, serVORole.Size); CastingNotes = Encoding.ASCII.GetString(buf);
                stream.Position = x;
            }
            // Updated based on BoyC's 010 template and Moack's work. Think we just about read all data from actor now.- DarkLotus
            stream.Close();
        }

        public enum ActorType
        {
            Invalid = 0,
            Monster = 1,
            Gizmo = 2,
            ClientEffect = 3,
            ServerProp = 4,
            Enviroment = 5,
            Critter = 6,
            Player = 7,
            Item = 8,
            AxeSymbol = 9,
            Projectile = 10,
            CustomBrain = 11
        }
    }
    public class ActorCollisionData
    {
        ActorCollisionFlags ColFlags;
        int i0;
        AxialCylinder Cylinder;
        AABB aabb;
        float f0;
        public ActorCollisionData(MpqFileStream stream)
        {
            ColFlags = new ActorCollisionFlags(stream);
            i0 = stream.ReadValueS32();
            Cylinder = new AxialCylinder(stream);
            aabb = new AABB(stream);
            f0 = stream.ReadValueF32();
            stream.ReadValueS32();// Testing - DarkLotus
        }
    }

    public class AxialCylinder
    {
        public Vector3D Position;
        public float Ax1;
        public float Ax2;

        public AxialCylinder(MpqFileStream stream)
        {
            this.Position = new Vector3D(stream.ReadValueF32(), stream.ReadValueF32(), stream.ReadValueF32());
            Ax1 = stream.ReadValueF32();
            Ax2 = stream.ReadValueF32();
        }
    }

    public class Sphere
    {
        public Vector3D Position;
        public float Radius;

        public Sphere(MpqFileStream stream)
        {
            Position = new Vector3D(stream.ReadValueF32(), stream.ReadValueF32(), stream.ReadValueF32());
            Radius = stream.ReadValueF32();
        }
    }

    public class WeightedLook
    {
        public string LookLink;
        public int Int0;

        public WeightedLook(MpqFileStream stream)
        {
            var buf = new byte[64];
            stream.Read(buf, 0, 64);
            LookLink = Encoding.ASCII.GetString(buf);
            Int0 = stream.ReadValueS32();
        }
    }
}