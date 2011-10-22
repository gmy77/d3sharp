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
using Mooege.Common.MPQ.DataTypes;

namespace Mooege.Common.MPQ.FileFormats
{
    [FileFormat(SNOGroup.Actor)]
    public class Actor: FileFormat
    {
        public Header Header;
        /// <summary>
        /// SNO for actor.
        /// </summary>
        public int ActorSNO;
        public int i0;
        public int type;
        /// <summary>
        /// Actor Type
        /// </summary>
        public ActorType Type { get { return (ActorType)type; } }
        /// <summary>
        /// SNO for Apperance
        /// </summary>
        public int ApperanceSNO;
        public int snoPhysMesh;
        public AxialCylinder Cyl;
        public Sphere s;
        public AABB_ aabbBounds;
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
        public float f0, f1, f2;
        public int[] padActorCollisionData;
        public int[] padInventoryImages;
        public int i4;
        // Updated based on BoyC's 010editoer template, looks like some data at the end still isnt parsed - Darklotus
        public Actor(MpqFile file)
        {
            var stream = file.Open();
            Header = new Header(stream);
            this.ActorSNO = stream.ReadInt32();
            stream.Position += 8; // pad 2;
            this.i0 = stream.ReadInt32();
            this.type = stream.ReadInt32();
            this.ApperanceSNO = stream.ReadInt32();
            this.snoPhysMesh = stream.ReadInt32();
            this.Cyl = new AxialCylinder(stream);
            this.s = new Sphere(stream);
            this.aabbBounds = new AABB_(stream);
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
}
