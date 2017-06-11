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
using Mooege.Core.GS.Actors;
using Mooege.Core.GS.Common.Types.Collision;
using Mooege.Core.GS.Common.Types.Math;
using Mooege.Core.GS.Common.Types.SNO;
using Mooege.Common.MPQ.FileFormats.Types;
using Mooege.Core.GS.Common.Types.TagMap;

namespace Mooege.Common.MPQ.FileFormats
{
    [FileFormat(SNOGroup.Actor)]
    public class Actor : FileFormat
    {
        public Header Header { get; private set; }
        public int Int0 { get; private set; }

        /// <summary>
        /// Actor Type
        /// </summary>
        public ActorType Type { get; private set; }

        /// <summary>
        /// SNO for Apperance
        /// </summary>
        public int ApperanceSNO { get; private set; }

        public int PhysMeshSNO { get; private set; }
        public AxialCylinder Cylinder { get; private set; }
        public Sphere Sphere { get; private set; }
        public AABB AABBBounds { get; private set; }
        public TagMap TagMap { get; private set; }

        /// <summary>
        /// SNO for actor's animset.
        /// </summary>
        public int AnimSetSNO { get; private set; }

        /// <summary>
        /// MonterSNO if any.
        /// </summary>
        public int MonsterSNO { get; private set; }
        public List<MsgTriggeredEvent> MsgTriggeredEvents = new List<MsgTriggeredEvent>();

        public int Int1 { get; private set; }
        public Vector3D V0 { get; private set; }
        public WeightedLook[] Looks { get; private set; }
        public int PhysicsSNO { get; private set; }
        public int Int2 { get; private set; }
        public int Int3 { get; private set; }
        public float Float0 { get; private set; }
        public float Float1 { get; private set; }
        public float Float2 { get; private set; }
        public ActorCollisionData ActorCollisionData { get; private set; }
        public int[] InventoryImages { get; private set; }
        public int Int4 { get; private set; }
        public string CastingNotes { get; private set; }
        public string VoiceOverRole { get; private set; }
        public int BitField0 { get; private set; }           // 25 bits - better this this would be an uint
        public int BitField1 { get; private set; }           // 25 bits - better this this would be an uint

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

            this.TagMap = stream.ReadSerializedItem<TagMap>();
            stream.Position += (2 * 4);

            this.AnimSetSNO = stream.ReadValueS32();
            this.MonsterSNO = stream.ReadValueS32();

            MsgTriggeredEvents = stream.ReadSerializedData<MsgTriggeredEvent>();

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
            CastingNotes = stream.ReadSerializedString();
            VoiceOverRole = stream.ReadSerializedString();

            // Updated based on BoyC's 010 template and Moack's work. Think we just about read all data from actor now.- DarkLotus
            stream.Close();
        }
    }
    public class ActorCollisionData
    {
        public ActorCollisionFlags ColFlags { get; private set; }
        public int I0 { get; private set; }
        public AxialCylinder Cylinder { get; private set; }
        public AABB AABB { get; private set; }
        public float F0 { get; private set; }

        public ActorCollisionData(MpqFileStream stream)
        {
            ColFlags = new ActorCollisionFlags(stream);
            I0 = stream.ReadValueS32();
            Cylinder = new AxialCylinder(stream);
            AABB = new AABB(stream);
            F0 = stream.ReadValueF32();
            stream.ReadValueS32();// Testing - DarkLotus
        }
    }

    public class AxialCylinder
    {
        public Vector3D Position { get; private set; }
        public float Ax1 { get; private set; }
        public float Ax2 { get; private set; }

        public AxialCylinder(MpqFileStream stream)
        {
            this.Position = new Vector3D(stream.ReadValueF32(), stream.ReadValueF32(), stream.ReadValueF32());
            Ax1 = stream.ReadValueF32();
            Ax2 = stream.ReadValueF32();
        }
    }

    public class Sphere
    {
        public Vector3D Position { get; private set; }
        public float Radius { get; private set; }

        public Sphere(MpqFileStream stream)
        {
            Position = new Vector3D(stream.ReadValueF32(), stream.ReadValueF32(), stream.ReadValueF32());
            Radius = stream.ReadValueF32();
        }
    }

    public class WeightedLook
    {
        public string LookLink { get; private set; }
        public int Int0 { get; private set; }

        public WeightedLook(MpqFileStream stream)
        {
            this.LookLink = stream.ReadString(64, true);
            Int0 = stream.ReadValueS32();
        }
    }
}