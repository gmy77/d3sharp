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
using CrystalMpq;
using Mooege.Common.Extensions;
using Mooege.Net.GS.Message.Fields;
using System.Text;

namespace Mooege.Common.MPQ.FileFormats
{
    [FileFormat(SNOGroup.Actor)]
    public class Actor: FileFormat
    {
        /// <summary>
        /// SNO for actor.
        /// </summary>
        public int ActorSNO;
        private int i0;
        public int Type;
        /// <summary>
        /// SNO for Apperance
        /// </summary>
        public int ApperanceSNO;
        public int snoPhysMesh;
        public AxialCylinder Cyl;
        public Sphere s;
        public AABB aabbBounds;
        private SerializeData serTagMap;
        
        /// <summary>
        /// SNO for actor's animset.
        /// </summary>
        public int AnimSetSNO;

        public int MonsterSNO;
        private SerializeData serMsgTriggeredEvents;
        public int i1;
        public Vector3D v0;      
        public WeightedLook[] Looks;
        public int PhysicsSNO;
        public int i2, i3;
        float f0, f1, f2;
        private int[] padActorCollisionData;
        private int[] padInventoryImages;
        int i4;
        // Updated based on BoyC's 010editoer template, looks like some data at the end still isnt parsed - Darklotus
        public Actor(MpqFile file)
        {
            var stream = file.Open();
            stream.Seek(16, SeekOrigin.Begin);
            this.ActorSNO = stream.ReadInt32();
            stream.Position += 8; // pad 2;
            this.i0 = stream.ReadInt32();
            this.Type = stream.ReadInt32();
            this.ApperanceSNO = stream.ReadInt32();
            this.snoPhysMesh = stream.ReadInt32();
            this.Cyl = new AxialCylinder(stream);
            this.s = new Sphere(stream);
            this.aabbBounds = new AABB(stream);
            this.serTagMap = new SerializeData(stream);
            stream.Position += 8; // pad 2
            this.AnimSetSNO = stream.ReadInt32();
            this.MonsterSNO = stream.ReadInt32();
            this.serMsgTriggeredEvents = new SerializeData(stream);

            this.i1 = stream.ReadInt32();
            stream.Position += 12; // pad 3 int - DarkLotus
            this.v0 = new Vector3D(stream.ReadFloat(), stream.ReadFloat(), stream.ReadFloat());
            this.Looks = new WeightedLook[8];
            for (int i = 0; i < 8; i++)
            {
                this.Looks[i] = new WeightedLook(stream);
            }
            this.PhysicsSNO = stream.ReadInt32();
            this.i2 = stream.ReadInt32(); this.i3 = stream.ReadInt32();
            this.f0 = stream.ReadFloat(); this.f1 = stream.ReadFloat(); this.f2 = stream.ReadFloat();
            this.padActorCollisionData = new int[17]; // Was 68/4 - Darklotus 
            for (int i = 0; i < 17; i++)
            {
                this.padActorCollisionData[i] = stream.ReadInt32();
            }
            this.padInventoryImages = new int[10]; //Was 5*8/4 - Darklotus
            for (int i = 0; i < 10; i++)
            {
                this.padInventoryImages[i] = stream.ReadInt32();
            }
            stream.Close();
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

        public class AABB
        {
            public Vector3D Min { get; private set; }
            public Vector3D Max { get; private set; }

            public AABB(MpqFileStream stream)
            {
                this.Min = new Vector3D(stream.ReadFloat(), stream.ReadFloat(), stream.ReadFloat());
                this.Max = new Vector3D(stream.ReadFloat(), stream.ReadFloat(), stream.ReadFloat());
            }
        }
        private class SerializeData
        {
            public readonly int Offset;
            public readonly int Size;

            public SerializeData(MpqFileStream stream)
            {
                Offset = stream.ReadInt32();
                Size = stream.ReadInt32();
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

    }
}
