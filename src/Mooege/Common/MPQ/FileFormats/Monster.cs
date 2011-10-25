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
using Mooege.Common.MPQ.FileFormats.Types;
using System.Text;

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
        public MonsterDef Monsterdef;
        public Resistance Resists;
        int i0, i1;
        public Levels Level = new Levels();
        public float[] Floats;
        public float f0, f1;
        public int snoInventory, snoSecondaryInventory, snoLore;
        int i3;
        public HealthDropInfo HealthDropinfo0;
        public HealthDropInfo HealthDropinfo1;
        public HealthDropInfo HealthDropinfo2;
        public int snoSkillKit;
        public MonsterPowerType PowerType;
        public SkillDeclaration[] SkillDeclarations;
        public MonsterSkillDeclaration[] MonsterSkillDeclarations;
        public int snoTreasureClassFirstKill;
        public int snoTreasureClass;
        public int snoTreasureClassRare;
        public int snoTreasureClassChampion;
        public int snoTreasureClassChampionLight;
        public float f2, f3, f4, f5,f6;
        public int i4, i5, i6;
        public int[] AIBehavior;
        public int[] GbidArray0; // 8
        public int[] snoSummonActor; //6
        public int[] GbidArray1; // 4
        public int[] GbidArray2; // 6
        public int i7, i8, i9;
        public string Name; // 128
        public TagMap TagMap;
        public int i10;
        MonsterMinionSpawnGroup MonsterMinionSpawngroup;
        public Monster(MpqFile file)
        {
            var stream = file.Open();
            this.Header = new Header(stream);
            stream.Position += (1 * 4);
            this.ActorSNO = stream.ReadValueS32();
            stream.Position += 4;
            this.Type = (MonsterType)stream.ReadValueS32();
            this.Race = (MonsterRace)stream.ReadValueS32();
            this.Size = (MonsterSize)stream.ReadValueS32();
            this.Monsterdef = new MonsterDef(stream);
            this.Resists = (Resistance)stream.ReadValueS32();
            this.i0 = stream.ReadValueS32();
            this.i1 = stream.ReadValueS32();
            this.Level.Normal = stream.ReadValueS32();
            this.Level.Nightmare = stream.ReadValueS32();
            this.Level.Hell = stream.ReadValueS32();
            this.Level.Inferno = stream.ReadValueS32();
            Floats = new float[144];
            for (int i = 0; i < 144; i++)
            {
                Floats[i] = stream.ReadValueF32();
            }


            this.i3 = stream.ReadValueS32();
            this.HealthDropinfo0 = new HealthDropInfo(stream);
            this.HealthDropinfo1 = new HealthDropInfo(stream);
            this.HealthDropinfo2 = new HealthDropInfo(stream);
            this.snoSkillKit = stream.ReadValueS32();
            this.SkillDeclarations = new SkillDeclaration[8];
            for (int i = 0; i < 8; i++)
            {
                this.SkillDeclarations[i] = new SkillDeclaration(stream);
            }
            this.MonsterSkillDeclarations = new MonsterSkillDeclaration[8];
            for (int i = 0; i < 8; i++)
            {
                this.MonsterSkillDeclarations[i] = new MonsterSkillDeclaration(stream);
            }
            this.snoTreasureClassFirstKill = stream.ReadValueS32();
            this.snoTreasureClass = stream.ReadValueS32();
            this.snoTreasureClassRare = stream.ReadValueS32();
            this.snoTreasureClassChampion = stream.ReadValueS32();
            this.snoTreasureClassChampionLight = stream.ReadValueS32();
            // at 916 here
            this.f2 = stream.ReadValueF32();
            this.f3 = stream.ReadValueF32();
            this.f4 = stream.ReadValueF32();
            this.f5 = stream.ReadValueF32();
            i4 = stream.ReadValueS32();
            f6 = stream.ReadValueF32();
            i5 = stream.ReadValueS32();
            i6 = stream.ReadValueS32();
            //948
            this.snoInventory = stream.ReadValueS32();
            this.snoSecondaryInventory = stream.ReadValueS32();
            this.snoLore = stream.ReadValueS32();
            AIBehavior = new int[6];
            for (int i = 0; i < 6; i++)
            {
                this.AIBehavior[i] = stream.ReadValueS32();
            }
            GbidArray0 = new int[8];
            for (int i = 0; i < 8; i++)
            {
                this.GbidArray0[i] = stream.ReadValueS32();
            }
            this.snoSummonActor = new int[6];
            for (int i = 0; i < 6; i++)
            {
                this.snoSummonActor[i] = stream.ReadValueS32();
            }
            GbidArray1 = new int[4];
            for (int i = 0; i < 4; i++)
            {
                this.GbidArray1[i] = stream.ReadValueS32();
            }
            GbidArray2 = new int[6];
            for (int i = 0; i < 6; i++)
            {
                this.GbidArray2[i] = stream.ReadValueS32();
            }
            this.i7 = stream.ReadValueS32();
            this.i8 = stream.ReadValueS32();
            this.i9 = stream.ReadValueS32();
            this.PowerType = (MonsterPowerType)stream.ReadValueS32();

            stream.Position += (6*4);
            this.TagMap = stream.ReadSerializedItem<TagMap>();
            stream.Position += (2 * 4);
            this.i10 = stream.ReadValueS32();
            stream.Position += (3 * 4);
            this.MonsterMinionSpawngroup = stream.ReadSerializedItem<MonsterMinionSpawnGroup>();
            Name = stream.ReadString(128, true);         
            stream.Close();
        }

        public class MonsterMinionSpawnGroup : ISerializableData
        {
            public float f0;
            public int i1;
            public List<MonsterMinionSpawnItem> SpawnItems = new List<MonsterMinionSpawnItem>();
            public void Read(MpqFileStream stream)
            {
                this.f0 = stream.ReadValueF32();
                this.i1 = stream.ReadValueS32();
                SpawnItems = stream.ReadSerializedData<MonsterMinionSpawnItem>();
            }
        }

        public class MonsterMinionSpawnItem : ISerializableData
        {
            public int snoSpawn, i0, i1, i2, i3;
            public void Read(MpqFileStream stream)
            {
                this.snoSpawn = stream.ReadValueS32();
                this.i0 = stream.ReadValueS32();
                this.i1 = stream.ReadValueS32();
                this.i2 = stream.ReadValueS32();
                this.i3 = stream.ReadValueS32();
            }
        }
        public class MonsterDef
        {
            public float f0,f1,f2,f3;
            public int i0;
            public MonsterDef(MpqFileStream stream)
            {
                f0 = stream.ReadValueF32();
                f1 = stream.ReadValueF32();
                f2 = stream.ReadValueF32();
                f3 = stream.ReadValueF32();
                i0 = stream.ReadValueS32();
            }
        }
        public class MonsterSkillDeclaration
        {
            public float f0, f1;
            public int i0;
            public float f2;
            public MonsterSkillDeclaration(MpqFileStream stream)
            {
                f0 = stream.ReadValueF32();
                f1 = stream.ReadValueF32();
                i0 = stream.ReadValueS32();
                f2 = stream.ReadValueF32();
            }
        }
        public class SkillDeclaration
        {
            public int snoPower;
            public int i0;
            public SkillDeclaration(MpqFileStream stream)
            {
                snoPower = stream.ReadValueS32();
                i0 = stream.ReadValueS32();
            }
        }
        public enum MonsterPowerType // No idea what this is called - DarkLotus
        {
            Mana,
            Arcanum,
            Fury,
            Spirit,
            Power,
            Hatred,
            Discipline
        }
        public class HealthDropInfo
        {
            public float f0;
            public int GBID;
            int i1;
            public HealthDropInfo(MpqFileStream stream)
            {
                this.f0 = stream.ReadValueF32();
                this.GBID = stream.ReadValueS32();
                this.i1 = stream.ReadValueS32();
            }
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