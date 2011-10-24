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

namespace Mooege.Common.MPQ.FileFormats
{
    [FileFormat(SNOGroup.Monster)]
    public class Monster : FileFormat
    {
        public Header Header;
        public int MonsterSNO;
        public int ActorSNO;

        public MonsterRace Race;
        public MonsterSize Size;
        public MonsterType Type;
        public Resistance Resists;
        int i0, i1;
        public Levels Level = new Levels();

        public int snoLore;
        public int snoSkillKit;


        public Monster(MpqFile file)
        {
            var stream = file.Open();
            this.Header = new Header(stream);
            this.MonsterSNO = stream.ReadInt32();
            stream.Position += (3 * 4);
            this.ActorSNO = stream.ReadInt32();
            stream.Position += 4;
            this.Type = (MonsterType)stream.ReadInt32();
            this.Race = (MonsterRace)stream.ReadInt32();
            this.Size = (MonsterSize)stream.ReadInt32();
            stream.Position = 0x48;
            this.Resists = (Resistance)stream.ReadInt32();
            this.i0 = stream.ReadInt32();
            this.i1 = stream.ReadInt32();
            this.Level.Normal = stream.ReadInt32();
            this.Level.Nightmare = stream.ReadInt32();
            this.Level.Hell = stream.ReadInt32();
            this.Level.Inferno = stream.ReadInt32();
            // 145floats follow this according to chuanhsing, not sure what they are for - DarkLotus


            stream.Position = 700 + 16;
            this.snoSkillKit = stream.ReadInt32();
            stream.Position = 956 + 16;
            this.snoLore = stream.ReadInt32();
            stream.Close();
        }
        public enum Resistance
        {
            Physical = 0,
            Fire = 1,
            Lightning = 2,
            Cold = 3,
            Poison = 4,
            Arcane = 5,
            Holy = 6
        }
        public class Levels
        {
            public int Normal;
            public int Nightmare;
            public int Hell;
            public int Inferno;
        }
        public enum MonsterSize
        {
            NULL = -1,
            Big = 3,
            Standard = 4,
            Ranged = 5,
            Swarm = 6,
            Boss = 7
        }
        public enum MonsterRace
        {
            NULL = -1, // <Entry Name="" Value="-1" /> - DarkLotus
            Fallen = 1,
            GoatMen = 2,
            Rogue = 3,
            Skeleton = 4,
            Zombie = 5,
            SPider = 6,
            Triune = 7,
            WoodWraith = 8,
            Human = 9,
            Animal = 10

        }
        public enum MonsterType
        {
            Undead = 0,
            Demon = 1,
            Beast = 2,
            Human = 3,
            Breakable = 4,
            Scenery = 5,
            Ally = 6,
            Team = 7,
            Helper = 8
        }
    }
}