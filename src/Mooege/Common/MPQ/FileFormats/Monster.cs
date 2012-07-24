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
using Mooege.Common.MPQ.FileFormats.Types;
using Mooege.Core.GS.Common.Types.SNO;
using Mooege.Core.GS.Common.Types.TagMap;

namespace Mooege.Common.MPQ.FileFormats
{
    [FileFormat(SNOGroup.Monster)]
    public class Monster : FileFormat
    {
        public Header Header { get; private set; }
        public int I0 { get; private set; }
        public int ActorSNO { get; private set; }
        public int I1 { get; private set; }
        public MonsterRace Race { get; private set; }
        public MonsterSize Size { get; private set; }
        public MonsterType Type { get; private set; }
        public MonsterDef Monsterdef { get; private set; }
        public Resistance Resists { get; private set; }
        public int I2 { get; private set; }
        public int I3 { get; private set; }
        public Levels Level = new Levels();
        public float[] Floats { get; private set; }
        public float F0 { get; private set; }
        public float F1 { get; private set; }
        public float F2 { get; private set; }
        public float F3 { get; private set; }
        public float F4 { get; private set; }
        public float F5 { get; private set; }
        public int SNOInventory { get; private set; }
        public int SNOSecondaryInventory { get; private set; }
        public int SNOLore { get; private set; }
        public int I4 { get; private set; }
        public HealthDropInfo HealthDropinfo0 { get; private set; }
        public HealthDropInfo HealthDropinfo1 { get; private set; }
        public HealthDropInfo HealthDropinfo2 { get; private set; }
        public HealthDropInfo HealthDropinfo3 { get; private set; }
        public int SNOSkillKit { get; private set; }
        public MonsterPowerType PowerType { get; private set; }
        public SkillDeclaration[] SkillDeclarations { get; private set; }
        public MonsterSkillDeclaration[] MonsterSkillDeclarations { get; private set; }
        public int SNOTreasureClassFirstKill { get; private set; }
        public int SNOTreasureClass { get; private set; }
        public int SNOTreasureClassRare { get; private set; }
        public int SNOTreasureClassChampion { get; private set; }
        public int SNOTreasureClassChampionLight { get; private set; }
        public float F6 { get; private set; }
        public float F7 { get; private set; }
        public float F8 { get; private set; }
        public float F9 { get; private set; }
        public float F10 { get; private set; }
        public int I5 { get; private set; }
        public int I6 { get; private set; }
        public int I7 { get; private set; }
        public int[] AIBehavior { get; private set; }
        public int[] GbidArray0 { get; private set; } // 8
        public int[] SNOSummonActor { get; private set; } //6
        public int[] GbidArray1 { get; private set; } // 4
        public int[] GbidArray2 { get; private set; } // 6
        public int I8 { get; private set; }
        public int I9 { get; private set; }
        public int I10 { get; private set; }
        public string Name { get; private set; } // 128
        public TagMap TagMap { get; private set; }
        public int I11 { get; private set; }
        public List<MonsterMinionSpawnGroup> MonsterMinionSpawngroup { get; private set; }

        public Monster(MpqFile file)
        {
            var stream = file.Open();
            this.Header = new Header(stream);
            this.I0 = stream.ReadValueS32();
            this.ActorSNO = stream.ReadValueS32();
            this.I2 = stream.ReadValueS32();
            this.Type = (MonsterType)stream.ReadValueS32();
            this.Race = (MonsterRace)stream.ReadValueS32();
            this.Size = (MonsterSize)stream.ReadValueS32();
            this.Monsterdef = new MonsterDef(stream);
            this.Resists = (Resistance)stream.ReadValueS32();
            this.I2 = stream.ReadValueS32();
            this.I3 = stream.ReadValueS32();
            this.Level.Normal = stream.ReadValueS32();
            this.Level.Nightmare = stream.ReadValueS32();
            this.Level.Hell = stream.ReadValueS32();
            this.Level.Inferno = stream.ReadValueS32();
            // 84 - last 6 of these floats are not in the array actually.
            Floats = new float[139];
            for (int i = 0; i < 139; i++)
            {
                Floats[i] = stream.ReadValueF32();
            }
            F0 = stream.ReadValueF32();
            F1 = stream.ReadValueF32();
            F2 = stream.ReadValueF32();
            F3 = stream.ReadValueF32();
            F4 = stream.ReadValueF32();
            F5 = stream.ReadValueF32();

            // 664
            this.I4 = stream.ReadValueS32();
            this.HealthDropinfo0 = new HealthDropInfo(stream);
            this.HealthDropinfo1 = new HealthDropInfo(stream);
            this.HealthDropinfo2 = new HealthDropInfo(stream);
            this.HealthDropinfo3 = new HealthDropInfo(stream);
            // 716
            this.SNOSkillKit = stream.ReadValueS32();
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
            // 912
            this.SNOTreasureClassFirstKill = stream.ReadValueS32();
            this.SNOTreasureClass = stream.ReadValueS32();
            this.SNOTreasureClassRare = stream.ReadValueS32();
            this.SNOTreasureClassChampion = stream.ReadValueS32();
            this.SNOTreasureClassChampionLight = stream.ReadValueS32();
            // 932
            this.F6 = stream.ReadValueF32();
            this.F7 = stream.ReadValueF32();
            this.F8 = stream.ReadValueF32();
            this.F9 = stream.ReadValueF32();
            this.I5 = stream.ReadValueS32();
            this.F10 = stream.ReadValueF32();
            this.I6 = stream.ReadValueS32();
            this.I7 = stream.ReadValueS32();
            //964
            this.SNOInventory = stream.ReadValueS32();
            this.SNOSecondaryInventory = stream.ReadValueS32();
            this.SNOLore = stream.ReadValueS32();
            this.AIBehavior = new int[6];
            for (int i = 0; i < 6; i++)
            {
                this.AIBehavior[i] = stream.ReadValueS32();
            }
            this.GbidArray0 = new int[8];
            for (int i = 0; i < 8; i++)
            {
                this.GbidArray0[i] = stream.ReadValueS32();
            }
            this.SNOSummonActor = new int[6];
            for (int i = 0; i < 6; i++)
            {
                this.SNOSummonActor[i] = stream.ReadValueS32();
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
            // 1096
            this.I8 = stream.ReadValueS32();
            this.I9 = stream.ReadValueS32();
            this.I10 = stream.ReadValueS32();
            this.PowerType = (MonsterPowerType)stream.ReadValueS32();
            //1112
            stream.Position += (6 * 4);
            // 1136
            this.TagMap = stream.ReadSerializedItem<TagMap>();
            stream.Position += (3 * 4);
            this.I11 = stream.ReadValueS32();
            stream.Position += (2 * 4);
            this.MonsterMinionSpawngroup = stream.ReadSerializedData<MonsterMinionSpawnGroup>();
            this.Name = stream.ReadString(128, true);
            stream.Close();
        }

        public class MonsterMinionSpawnGroup : ISerializableData
        {
            public float F0 { get; private set; }
            public int I1 { get; private set; }
            public List<MonsterMinionSpawnItem> SpawnItems = new List<MonsterMinionSpawnItem>();
            public void Read(MpqFileStream stream)
            {
                this.F0 = stream.ReadValueF32();
                this.I1 = stream.ReadValueS32();
                stream.Position += 8;
                SpawnItems = stream.ReadSerializedData<MonsterMinionSpawnItem>();
            }
        }

        public class MonsterMinionSpawnItem : ISerializableData
        {
            public int SNOSpawn { get; private set; }
            public int I0 { get; private set; }
            public int I1 { get; private set; }
            public int I2 { get; private set; }
            public int I3 { get; private set; }

            public void Read(MpqFileStream stream)
            {
                this.SNOSpawn = stream.ReadValueS32();
                this.I0 = stream.ReadValueS32();
                this.I1 = stream.ReadValueS32();
                this.I2 = stream.ReadValueS32();
                this.I3 = stream.ReadValueS32();
            }
        }

        public class MonsterDef
        {
            public float F0 { get; private set; }
            public float F1 { get; private set; }
            public float F2 { get; private set; }
            public float F3 { get; private set; }
            public int I0 { get; private set; }

            public MonsterDef(MpqFileStream stream)
            {
                F0 = stream.ReadValueF32();
                F1 = stream.ReadValueF32();
                F2 = stream.ReadValueF32();
                F3 = stream.ReadValueF32();
                I0 = stream.ReadValueS32();
            }
        }
        public class MonsterSkillDeclaration
        {
            public float F0 { get; private set; }
            public float F1 { get; private set; }
            public int I0 { get; private set; }
            public float F2 { get; private set; }

            public MonsterSkillDeclaration(MpqFileStream stream)
            {
                F0 = stream.ReadValueF32();
                F1 = stream.ReadValueF32();
                I0 = stream.ReadValueS32();
                F2 = stream.ReadValueF32();
            }
        }
        public class SkillDeclaration
        {
            public int SNOPower { get; private set; }
            public int I0 { get; private set; }

            public SkillDeclaration(MpqFileStream stream)
            {
                SNOPower = stream.ReadValueS32();
                I0 = stream.ReadValueS32();
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
            public float F0 { get; private set; }
            public int GBID { get; private set; }
            public int I1 { get; private set; }

            public HealthDropInfo(MpqFileStream stream)
            {
                this.F0 = stream.ReadValueF32();
                this.GBID = stream.ReadValueS32();
                this.I1 = stream.ReadValueS32();
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
            Unknown = -1,
            Big = 3,
            Standard = 4,
            Ranged = 5,
            Swarm = 6,
            Boss = 7
        }

        public enum MonsterRace
        {
            Unknown = -1,
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
            Unknown = -1,
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