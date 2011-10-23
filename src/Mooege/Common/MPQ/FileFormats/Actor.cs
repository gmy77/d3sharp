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
        
        /// <summary>
        /// SNO for actor's animset.
        /// </summary>
        public int AnimSetSNO;

        /// <summary>
        /// MonterSNO if any.
        /// </summary>
        public int MonsterSNO;

        public int Int1;
        public Vector3D V0;
        public WeightedLook[] Looks;
        public int PhysicsSNO;
        public int Int2, Int3;
        public float Float0, Float1, Float2;
        public int[] ActorCollisionData;
        public int[] InventoryImages;
        public int Int4;
        
        public Actor(MpqFile file)
        {
            var stream = file.Open();
            Header = new Header(stream);
          
            this.Int0 = stream.ReadInt32();
            this.Type = (ActorType)stream.ReadInt32();
            this.ApperanceSNO = stream.ReadInt32();
            this.PhysMeshSNO = stream.ReadInt32();
            this.Cylinder = new AxialCylinder(stream);
            this.Sphere = new Sphere(stream);
            this.AABBBounds = new AABB(stream);

            var tagmap = stream.GetSerializedDataPointer(); // we need to read tagmap. /raist.
            stream.Position += (2*4);
            
            this.AnimSetSNO = stream.ReadInt32();
            this.MonsterSNO = stream.ReadInt32();

            var msgTriggeredEvents = stream.GetSerializedDataPointer();

            this.Int1 = stream.ReadInt32();
            stream.Position += (3*4); 
            this.V0 = new Vector3D(stream.ReadFloat(), stream.ReadFloat(), stream.ReadFloat());

            this.Looks = new WeightedLook[8];
            for (int i = 0; i < 8; i++)
            {
                this.Looks[i] = new WeightedLook(stream);
            }

            this.PhysicsSNO = stream.ReadInt32();
            this.Int2 = stream.ReadInt32(); 
            this.Int3 = stream.ReadInt32();
            this.Float0 = stream.ReadFloat(); 
            this.Float1 = stream.ReadFloat(); 
            this.Float2 = stream.ReadFloat();

            this.ActorCollisionData = new int[17]; // Was 68/4 - Darklotus 
            for (int i = 0; i < 17; i++)
            {
                this.ActorCollisionData[i] = stream.ReadInt32();
            }

            this.InventoryImages = new int[10]; //Was 5*8/4 - Darklotus
            for (int i = 0; i < 10; i++)
            {
                this.InventoryImages[i] = stream.ReadInt32();
            }

            // Updated based on BoyC's 010editoer template, looks like some data at the end still isnt parsed - Darklotus
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

    public class AxialCylinder
    {
        public Vector3D Position;
        public float Ax1;
        public float Ax2;

        public AxialCylinder(MpqFileStream stream)
        {
            this.Position = new Vector3D(stream.ReadFloat(), stream.ReadFloat(), stream.ReadFloat());
            Ax1 = stream.ReadFloat();
            Ax2 = stream.ReadFloat();
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

    public class WeightedLook
    {
        public string LookLink;
        public int Int0;

        public WeightedLook(MpqFileStream stream)
        {
            var buf = new byte[64];
            stream.Read(buf, 0, 64);
            LookLink = Encoding.ASCII.GetString(buf);
            Int0 = stream.ReadInt32();
        }
    }
}
