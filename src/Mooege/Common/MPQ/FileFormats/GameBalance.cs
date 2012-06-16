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

using System;
using System.IO;
using System.Collections.Generic;
using CrystalMpq;
using Gibbed.IO;
using Mooege.Common.Extensions;
using Mooege.Common.MPQ.FileFormats.Types;
using Mooege.Core.GS.Common.Types.SNO;
using Mooege.Common.Helpers;
using Mooege.Common.Helpers.Hash;
using Mooege.Common.Storage;
using Mooege.Net.GS.Message.Fields;


namespace Mooege.Common.MPQ.FileFormats
{
    // this file should be fixed with our naming-conventions. /raist

    [FileFormat(SNOGroup.GameBalance)]
    public class GameBalance : FileFormat
    {
        public Header Header { get; private set; }
        public BalanceType Type { get; private set; }
        public string Gbi { get; private set; }
        public string Xls { get; private set; }
        public int I0 { get; private set; }
        public int I1 { get; private set; }
        public List<ItemTypeTable> ItemType { get; private set; }
        public List<ItemTable> Item { get; private set; }
        public List<ExperienceTable> Experience { get; private set; }
        public List<HelpCodesTable> HelpCodes { get; private set; }
        public List<MonsterLevelTable> MonsterLevel { get; private set; }
        public List<AffixTable> Affixes { get; private set; }
        public List<HeroTable> Heros { get; private set; }

        [PersistentProperty("MovementStyles")]
        public List<MovementStyle> MovementStyles { get; private set; }
        public List<LabelGBIDTable> Labels { get; private set; }

        [PersistentProperty("LootDistribution")]
        public List<LootDistributionTableEntry> LootDistribution { get; private set; }
        public List<RareItemNamesTable> RareItemNames { get; private set; }
        public List<MonsterAffixesTable> MonsterAffixes { get; private set; }
        public List<MonsterNamesTable> RareMonsterNames { get; private set; }
        public List<SocketedEffectTable> SocketedEffects { get; private set; }
        public List<ItemEnhancementTable> ItemEnhancement { get; private set; }

        [PersistentProperty("ItemDropTable")]
        public List<ItemDropTableEntry> ItemDropTable { get; private set; }

        [PersistentProperty("ItemLevelModifiers")]
        public List<ItemLevelModifier> ItemLevelModifiers { get; private set; }

        [PersistentProperty("QualityClasses")]
        public List<QualityClass> QualityClasses { get; private set; }
        public List<HirelingTable> Hirelings { get; private set; }
        public List<SetItemBonusTable> SetItemBonus { get; private set; }

        [PersistentProperty("EliteModifiers")]
        public List<EliteModifier> EliteModifiers { get; private set; }

        [PersistentProperty("ItemTiers")]
        public List<ItemTier> ItemTiers { get; private set; }
        public List<PowerFormulaTable> PowerFormula { get; private set; }
        public List<RecipeTable> Recipes { get; private set; }

        [PersistentProperty("ScriptedAchievementEvents")]
        public List<ScriptedAchievementEventsTable> ScriptedAchievementEvents { get; private set; }

        public GameBalance() { }

        public GameBalance(MpqFile file)
        {
            var stream = file.Open();
            this.Header = new Header(stream);
            this.Type = (BalanceType)stream.ReadValueS32();
            Gbi = stream.ReadString(256, true);
            Xls = stream.ReadString(256, true);
            this.I0 = stream.ReadValueS32();
            this.I1 = stream.ReadValueS32();
            this.ItemType = stream.ReadSerializedData<ItemTypeTable>(); //536
            stream.Position += 8;
            this.Item = stream.ReadSerializedData<ItemTable>(); //552
            stream.Position += 8;
            this.Experience = stream.ReadSerializedData<ExperienceTable>(); //568
            stream.Position += 8;
            this.HelpCodes = stream.ReadSerializedData<HelpCodesTable>(); //584
            stream.Position += 8;
            this.MonsterLevel = stream.ReadSerializedData<MonsterLevelTable>(); //600
            stream.Position += 8;
            this.Affixes = stream.ReadSerializedData<AffixTable>(); //616
            stream.Position += 8;
            this.Heros = stream.ReadSerializedData<HeroTable>(); //632
            stream.Position += 8;
            this.MovementStyles = stream.ReadSerializedData<MovementStyle>(); //648
            stream.Position += 8;
            this.Labels = stream.ReadSerializedData<LabelGBIDTable>(); //664
            stream.Position += 8;
            this.LootDistribution = stream.ReadSerializedData<LootDistributionTableEntry>(); //680
            stream.Position += 8;
            this.RareItemNames = stream.ReadSerializedData<RareItemNamesTable>(); //696
            stream.Position += 8;
            this.MonsterAffixes = stream.ReadSerializedData<MonsterAffixesTable>(); //712
            stream.Position += 8;
            this.RareMonsterNames = stream.ReadSerializedData<MonsterNamesTable>(); //728
            stream.Position += 8;
            this.SocketedEffects = stream.ReadSerializedData<SocketedEffectTable>(); //744
            stream.Position += 8;
            this.ItemEnhancement = stream.ReadSerializedData<ItemEnhancementTable>(); //760
            stream.Position += 8;
            this.ItemDropTable = stream.ReadSerializedData<ItemDropTableEntry>(); //776
            stream.Position += 8;
            this.ItemLevelModifiers = stream.ReadSerializedData<ItemLevelModifier>(); //792
            stream.Position += 8;
            this.QualityClasses = stream.ReadSerializedData<QualityClass>(); //808
            stream.Position += 8;
            this.Hirelings = stream.ReadSerializedData<HirelingTable>(); //824
            stream.Position += 8;
            this.SetItemBonus = stream.ReadSerializedData<SetItemBonusTable>(); //840
            stream.Position += 8;
            this.EliteModifiers = stream.ReadSerializedData<EliteModifier>(); //856
            stream.Position += 8;
            this.ItemTiers = stream.ReadSerializedData<ItemTier>(); //872
            stream.Position += 8;
            this.PowerFormula = stream.ReadSerializedData<PowerFormulaTable>(); //888
            stream.Position += 8;
            this.Recipes = stream.ReadSerializedData<RecipeTable>(); //904
            stream.Position += 8;
            this.ScriptedAchievementEvents = stream.ReadSerializedData<ScriptedAchievementEventsTable>(); //920
            stream.Close();
        }

    }

    public enum BalanceType : int
    {
        ItemTypes = 1,
        Items = 2,
        ExperienceTable = 3,
        HelpCodes = 24,
        MonsterLevels = 5,
        Heros = 7,
        AffixList = 8,
        MovementStyles = 10,
        Labels = 11,
        LootDistribution = 12,
        RareItemNames = 16,
        MonsterAffixes = 18,
        MonsterNames = 19,
        SocketedEffects = 21,
        ItemEnhancements = 23,
        ItemDropTable = 25,
        ItemLevelModifiers = 26,
        QualityClasses = 27,
        Scenery = 17,
        Hirelings = 4,
        SetItemBonuses = 33,
        EliteModifiers = 34,
        ItemTiers = 35,
        PowerFormulaTables = 36,
        Recipes = 32,
        ScripedAchievementEvents = 37
    }

    public class ItemTypeTable : ISerializableData
    {
        //Total Length: 320
        public int Hash { get; private set; }
        public string Name { get; private set; }
        public int ParentType { get; private set; }
        public int I0 { get; private set; }
        public ItemFlags Flags { get; private set; }
        public eItemType Type0 { get; private set; }
        public eItemType Type1 { get; private set; }
        public eItemType Type2 { get; private set; }
        public eItemType Type3 { get; private set; }
        public int InheritedAffix0 { get; private set; }
        public int InheritedAffix1 { get; private set; }
        public int InheritedAffix2 { get; private set; }
        public int InheritedAffixFamily0 { get; private set; }
        public int[] Array { get; private set; }

        public void Read(MpqFileStream stream)
        {
            stream.Position += 4;
            this.Name = stream.ReadString(256, true);
            this.Hash = StringHashHelper.HashItemName(this.Name);
            this.ParentType = stream.ReadValueS32();
            this.I0 = stream.ReadValueS32();
            this.Flags = (ItemFlags)stream.ReadValueS32();
            this.Type0 = (eItemType)stream.ReadValueS32();
            this.Type1 = (eItemType)stream.ReadValueS32();
            this.Type2 = (eItemType)stream.ReadValueS32();
            this.Type3 = (eItemType)stream.ReadValueS32();
            this.InheritedAffix0 = stream.ReadValueS32();
            this.InheritedAffix1 = stream.ReadValueS32();
            this.InheritedAffix2 = stream.ReadValueS32();
            this.InheritedAffixFamily0 = stream.ReadValueS32();
            this.Array = new int[4];
            for (int i = 0; i < 4; i++)
                this.Array[i] = stream.ReadValueS32();
        }
    }

    [Flags]
    public enum ItemFlags
    {
        NotEquipable1 = 0x1,
        AtLeastMagical = 0x2,
        Gem = 0x8,
        NotEquipable2 = 0x40,
        Socketable = 0x80,
        Unknown = 0x1000,
        Barbarian = 0x100,
        Wizard = 0x200,
        WitchDoctor = 0x400,
        DemonHunter = 0x800,
        Monk = 0x2000,
    }

    public enum eItemType
    {
        PlayerBackpack = 0,
        PlayerHead = 1,
        PlayerTorso = 2,
        PlayerRightHand = 3,
        PlayerLeftHand = 4,
        PlayerHands = 5,
        PlayerWaist = 6,
        PlayerFeet = 7,
        PlayerShoulders = 8,
        PlayerLegs = 9,
        PlayerBracers = 10,
        PlayerLeftFinger = 11,
        PlayerRightFinger = 12,
        PlayerNeck = 13,
        Merchant = 18,
        PetRightHand = 21,
        PetLeftHand = 22,
        PetSpecial = 23,
        PetNeck = 24,
        PetRightFinger = 25,
        PetLeftFinger = 26,
    }

    public class ItemTable : ISerializableData
    {
        //Total Length: 1488
        public int Hash { get; private set; }
        public string Name { get; private set; }
        public int SNOActor { get; private set; }
        public int ItemType1 { get; private set; }
        public int I0 { get; private set; }
        public eItem E0 { get; private set; }
        public int ItemLevel { get; private set; }
        public int I2 { get; private set; }
        public int RandomPropertiesCount { get; private set; }
        public int MaxSockets { get; private set; }
        public int MaxStackAmount { get; private set; }
        public int BaseGoldValue { get; private set; }
        public int I7 { get; private set; }
        public int RequiredLevel { get; private set; }
        public int DurabilityMin { get; private set; }
        public int DurabilityDelta { get; private set; }
        public int I8 { get; private set; }
        public int SNOBaseItem { get; private set; }
        public int SNOSet { get; private set; }
        public int SNOComponentTreasureClass { get; private set; }
        public int SNOComponentTreasureClassMagic { get; private set; }
        public int SNOComponentTreasureClassRare { get; private set; }
        public int SNORareNamePrefixStringList { get; private set; }
        public int SNORareNameSuffixStringList { get; private set; }
        public int Flags { get; private set; }
        public float WeaponDamageMin { get; private set; }
        public float WeaponDamageDelta { get; private set; }
        public float ArmorValue { get; private set; }
        public float F3 { get; private set; }
        public float AttacksPerSecond { get; private set; }
        public float F4 { get; private set; }
        public float F5 { get; private set; }
        public int SNOSkill0 { get; private set; }
        public int I11 { get; private set; }
        public int SNOSkill1 { get; private set; }
        public int I12 { get; private set; }
        public int SNOSkill2 { get; private set; }
        public int I13 { get; private set; }
        public int SNOSkill3 { get; private set; }
        public int I14 { get; private set; }
        public int[] I15 { get; private set; }
        public AttributeSpecifier[] Attribute { get; private set; }
        public ItemQuality Quality { get; private set; }
        public int[] RecipeToGrant { get; private set; }
        public int EnhancementToGrant { get; private set; }
        public int[] LegendaryAffixFamily { get; private set; }
        public int[] MaxAffixLevel { get; private set; }
        public int[] I18 { get; private set; }
        public GemType Gem { get; private set; }
        public int I16 { get; private set; }
        public Alpha I17 { get; private set; }

        public void Read(MpqFileStream stream)
        {
            stream.Position += 4;
            this.Name = stream.ReadString(256, true);
            this.Hash = StringHashHelper.HashItemName(this.Name);
            this.SNOActor = stream.ReadValueS32(); //260
            this.ItemType1 = stream.ReadValueS32(); //264
            this.Flags = stream.ReadValueS32(); //268
            this.I0 = stream.ReadValueS32(); //272
            this.ItemLevel = stream.ReadValueS32(); //276
            this.E0 = (eItem)stream.ReadValueS32(); //280
            this.I2 = stream.ReadValueS32(); //284
            this.RandomPropertiesCount = stream.ReadValueS32(); //288
            this.MaxSockets = stream.ReadValueS32(); //292
            this.MaxStackAmount = stream.ReadValueS32(); //296
            this.BaseGoldValue = stream.ReadValueS32(); //300
            this.I7 = stream.ReadValueS32(); //304
            this.RequiredLevel = stream.ReadValueS32(); //308
            this.DurabilityMin = stream.ReadValueS32(); //312
            this.DurabilityDelta = stream.ReadValueS32(); //316
            this.I8 = stream.ReadValueS32(); //320
            this.SNOBaseItem = stream.ReadValueS32(); //324
            this.SNOSet = stream.ReadValueS32(); //328
            this.SNOComponentTreasureClass = stream.ReadValueS32(); //332
            this.SNOComponentTreasureClassMagic = stream.ReadValueS32(); //336
            this.SNOComponentTreasureClassRare = stream.ReadValueS32(); //340
            this.SNORareNamePrefixStringList = stream.ReadValueS32(); //344
            this.SNORareNameSuffixStringList = stream.ReadValueS32(); //348
            this.I15 = new int[4]; //352
            for (int i = 0; i < 4; i++)
                this.I15[i] = stream.ReadValueS32();
            stream.Position += 88;
            this.WeaponDamageMin = stream.ReadValueF32(); //456
            this.WeaponDamageDelta = stream.ReadValueF32(); //460
            stream.Position += 84;
            this.ArmorValue = stream.ReadValueF32(); //548
            this.F3 = stream.ReadValueF32(); //552
            stream.Position += 168;
            this.AttacksPerSecond = stream.ReadValueF32(); //724
            stream.Position += 84;
            this.F4 = stream.ReadValueF32(); //812
            this.F5 = stream.ReadValueF32(); //816
            stream.Position += 104;
            this.SNOSkill0 = stream.ReadValueS32(); //924
            this.I11 = stream.ReadValueS32(); //928
            this.SNOSkill1 = stream.ReadValueS32(); //932
            this.I12 = stream.ReadValueS32(); //936
            this.SNOSkill2 = stream.ReadValueS32(); //940
            this.I13 = stream.ReadValueS32(); //944
            this.SNOSkill3 = stream.ReadValueS32(); //948
            this.I14 = stream.ReadValueS32(); //952
            stream.Position += 44;
            this.Attribute = new AttributeSpecifier[16];
            for (int i = 0; i < 16; i++)
                this.Attribute[i] = new AttributeSpecifier(stream);
            this.Quality = (ItemQuality)stream.ReadValueS32(); //1384
            this.RecipeToGrant = new int[10]; //1388
            for (int i = 0; i < 10; i++)
                this.RecipeToGrant[i] = stream.ReadValueS32();
            this.EnhancementToGrant = stream.ReadValueS32(); //1428
            this.LegendaryAffixFamily = new int[6];
            for (int i = 0; i < 6; i++)
                this.LegendaryAffixFamily[i] = stream.ReadValueS32(); //1432
            this.MaxAffixLevel = new int[6];
            for (int i = 0; i < 6; i++)
                this.MaxAffixLevel[i] = stream.ReadValueS32(); //1456
            this.I18 = new int[6];
            for (int i = 0; i < 6; i++)
                this.I18[i] = stream.ReadValueS32(); //1446
            this.Gem = (GemType)stream.ReadValueS32(); //1504
            this.I16 = stream.ReadValueS32(); //1508
            this.I17 = (Alpha)stream.ReadValueS32(); //1512
            stream.Position += 4;
        }

        public enum ItemQuality
        {
            Invalid = -1,
            Inferior,
            Normal,
            Superior,
            Magic1,
            Magic2,
            Magic3,
            Rare4,
            Rare5,
            Rare6,
            Legendary,
            Artifact,
        }

        [Flags]
        public enum eItem
        {
            Invalid = -1,
            A1 = 0,
            A2 = 100,
            A3 = 200,
            A4 = 300,
            Test = 1000,
        }

        public enum GemType : int
        {
            Amethyst = 1,
            Emerald,
            Ruby,
            Topaz,
        }

        public enum Alpha : int
        {
            A = 1,
            B,
            C,
            D,
        }
    }

    public class ExperienceTable : ISerializableData
    {
        //Total Length: 224
        public int Exp { get; private set; }
        public int I1 { get; private set; }
        public float F0 { get; private set; }
        public float F1 { get; private set; }
        public int I2 { get; private set; }
        public int I3 { get; private set; }
        public int I4 { get; private set; }
        public int I5 { get; private set; }
        public int I6 { get; private set; }
        public int I7 { get; private set; }
        public int I8 { get; private set; }
        public int I9 { get; private set; }
        public int I10 { get; private set; }
        public int I11 { get; private set; }
        public int I12 { get; private set; }
        public int I13 { get; private set; }
        public int I14 { get; private set; }
        public int I15 { get; private set; }
        public int I16 { get; private set; }
        public float Multiplier { get; private set; }
        public int I17 { get; private set; }
        public int I18 { get; private set; }
        public int I19 { get; private set; }
        public int I20 { get; private set; }
        public int I21 { get; private set; }
        public int I22 { get; private set; }
        public int I23 { get; private set; }
        public int I24 { get; private set; }
        public int I25 { get; private set; }
        public int I26 { get; private set; }
        public int I27 { get; private set; }
        public int I28 { get; private set; }
        public int I29 { get; private set; }
        public int I30 { get; private set; }
        public int I31 { get; private set; }
        public int I32 { get; private set; }
        public int I33 { get; private set; }
        public int I34 { get; private set; }
        public int I35 { get; private set; }
        public int I36 { get; private set; }
        public int I37 { get; private set; }
        public int I38 { get; private set; }
        public int I39 { get; private set; }
        public int I40 { get; private set; }
        public int I41 { get; private set; }
        public int I42 { get; private set; }
        public int I43 { get; private set; }
        public int I44 { get; private set; }
        public int I45 { get; private set; }
        public int I46 { get; private set; }
        public int I47 { get; private set; }
        public float F2 { get; private set; }
        public float F3 { get; private set; }
        public float F4 { get; private set; }

        public void Read(MpqFileStream stream)
        {
            this.Exp = stream.ReadValueS32();
            this.I1 = stream.ReadValueS32();
            this.F0 = stream.ReadValueF32();
            this.F1 = stream.ReadValueF32();
            this.I2 = stream.ReadValueS32();
            this.I3 = stream.ReadValueS32();
            this.I4 = stream.ReadValueS32();
            this.I5 = stream.ReadValueS32();
            this.I6 = stream.ReadValueS32();
            this.I7 = stream.ReadValueS32();
            this.I8 = stream.ReadValueS32();
            this.I9 = stream.ReadValueS32();
            this.I10 = stream.ReadValueS32();
            this.I11 = stream.ReadValueS32();
            this.I12 = stream.ReadValueS32();
            this.I13 = stream.ReadValueS32();
            this.I14 = stream.ReadValueS32();
            this.I15 = stream.ReadValueS32();
            this.I16 = stream.ReadValueS32();
            this.Multiplier = stream.ReadValueF32(); //76
            this.I16 = stream.ReadValueS32(); //80
            this.I17 = stream.ReadValueS32(); //84
            this.I18 = stream.ReadValueS32(); //88
            this.I19 = stream.ReadValueS32(); //92
            this.I20 = stream.ReadValueS32(); //96
            this.I21 = stream.ReadValueS32(); //100
            this.I22 = stream.ReadValueS32(); //104
            this.I23 = stream.ReadValueS32(); //108
            this.I24 = stream.ReadValueS32(); //112
            this.I25 = stream.ReadValueS32(); //116
            this.I26 = stream.ReadValueS32(); //120
            this.I27 = stream.ReadValueS32(); //124
            this.I28 = stream.ReadValueS32(); //128
            this.I29 = stream.ReadValueS32(); //132
            this.I30 = stream.ReadValueS32(); //136
            this.I31 = stream.ReadValueS32(); //140
            this.I32 = stream.ReadValueS32(); //144
            this.I33 = stream.ReadValueS32(); //148
            this.I34 = stream.ReadValueS32(); //152
            this.I35 = stream.ReadValueS32(); //156
            stream.Position += 16;
            this.I36 = stream.ReadValueS32(); //176
            this.I37 = stream.ReadValueS32();
            this.I38 = stream.ReadValueS32();
            this.I39 = stream.ReadValueS32();
            this.I40 = stream.ReadValueS32();
            this.I41 = stream.ReadValueS32();
            this.I42 = stream.ReadValueS32();
            this.I43 = stream.ReadValueS32();
            this.I44 = stream.ReadValueS32();
            this.I45 = stream.ReadValueS32();
            this.I46 = stream.ReadValueS32();
            this.I47 = stream.ReadValueS32();
            stream.Position += 4;
            this.F2 = stream.ReadValueF32();
            this.F3 = stream.ReadValueF32();
            this.F4 = stream.ReadValueF32();
        }
    }

    public class HelpCodesTable : ISerializableData //unused
    {
        //Total Length: 640
        public string S0 { get; private set; }
        public string S1 { get; private set; }
        public string S2 { get; private set; }

        public void Read(MpqFileStream stream)
        {
            this.S0 = stream.ReadString(256, true);
            this.S1 = stream.ReadString(256, true);
            this.S2 = stream.ReadString(128, true);
        }
    }

    public class MonsterLevelTable : ISerializableData
    {
        //Total Length: 0x22C (556)
        public int I0 { get; private set; }
        public float F0 { get; private set; }
        public float F1 { get; private set; }
        public float F2 { get; private set; }
        public float F3 { get; private set; }
        public float F4 { get; private set; }
        public float F5 { get; private set; }
        public float F6 { get; private set; }
        public float F7 { get; private set; }
        public float F8 { get; private set; }
        public float F9 { get; private set; }
        public float F10 { get; private set; }
        public float F11 { get; private set; }
        public float F12 { get; private set; }
        public float F13 { get; private set; }
        public float F14 { get; private set; }
        public float F15 { get; private set; }
        public float F16 { get; private set; }
        public float F17 { get; private set; }
        public float F18 { get; private set; }
        public float F19 { get; private set; }
        public float F20 { get; private set; }
        public float F21 { get; private set; }
        public float F22 { get; private set; }
        public float F23 { get; private set; }
        public float F24 { get; private set; }
        public float F25 { get; private set; }
        public float F26 { get; private set; }
        public float F27 { get; private set; }
        public float F28 { get; private set; }
        public float F29 { get; private set; }
        public float F30 { get; private set; }
        public float F31 { get; private set; }
        public float F32 { get; private set; }
        public float F33 { get; private set; }
        public float F34 { get; private set; }
        public float F35 { get; private set; }
        public float F36 { get; private set; }
        public float F37 { get; private set; }
        public float F38 { get; private set; }
        public float F39 { get; private set; }
        public float F40 { get; private set; }
        public float F41 { get; private set; }
        public float F42 { get; private set; }
        public float F43 { get; private set; }
        public float F44 { get; private set; }
        public float F45 { get; private set; }
        public float F46 { get; private set; }
        public float F47 { get; private set; }
        public float F48 { get; private set; }
        public float F49 { get; private set; }
        public float F50 { get; private set; }
        public float F51 { get; private set; }

        public void Read(MpqFileStream stream)
        {
            this.I0 = stream.ReadValueS32(); //0
            stream.Position += 16;
            this.F0 = stream.ReadValueF32(); //20
            stream.Position += 8;
            this.F1 = stream.ReadValueF32(); //32
            this.F2 = stream.ReadValueF32(); //36
            this.F3 = stream.ReadValueF32(); //40
            stream.Position += 4;
            this.F4 = stream.ReadValueF32(); //48
            stream.Position += 40;
            this.F5 = stream.ReadValueF32(); //92
            this.F6 = stream.ReadValueF32(); //96
            this.F7 = stream.ReadValueF32(); //100
            this.F8 = stream.ReadValueF32(); //104
            this.F9 = stream.ReadValueF32(); //108
            this.F10 = stream.ReadValueF32(); //112
            this.F11 = stream.ReadValueF32(); //116
            this.F12 = stream.ReadValueF32(); //120
            this.F13 = stream.ReadValueF32(); //124
            this.F14 = stream.ReadValueF32(); //128
            this.F15 = stream.ReadValueF32(); //132
            this.F16 = stream.ReadValueF32(); //136
            this.F17 = stream.ReadValueF32(); //140
            this.F18 = stream.ReadValueF32(); //144
            this.F19 = stream.ReadValueF32(); //148
            this.F20 = stream.ReadValueF32(); //152
            this.F21 = stream.ReadValueF32(); //156
            this.F22 = stream.ReadValueF32(); //160
            stream.Position += 12;
            this.F23 = stream.ReadValueF32(); //176
            this.F24 = stream.ReadValueF32(); //180
            this.F25 = stream.ReadValueF32(); //184
            stream.Position += 32;
            this.F26 = stream.ReadValueF32(); //220
            this.F27 = stream.ReadValueF32(); //224
            stream.Position += 4;
            this.F28 = stream.ReadValueF32(); //232
            this.F29 = stream.ReadValueF32(); //236
            stream.Position += 28;
            this.F30 = stream.ReadValueF32(); //268
            this.F31 = stream.ReadValueF32(); //272
            this.F32 = stream.ReadValueF32(); //276
            this.F33 = stream.ReadValueF32(); //280
            this.F34 = stream.ReadValueF32(); //284
            this.F35 = stream.ReadValueF32(); //288
            this.F36 = stream.ReadValueF32(); //292
            this.F37 = stream.ReadValueF32(); //296
            this.F38 = stream.ReadValueF32(); //300
            stream.Position += 136;
            this.F39 = stream.ReadValueF32(); //440
            this.F40 = stream.ReadValueF32(); //444
            stream.Position += 12;
            this.F41 = stream.ReadValueF32(); //460
            this.F42 = stream.ReadValueF32(); //464
            this.F43 = stream.ReadValueF32(); //468
            this.F44 = stream.ReadValueF32(); //472
            this.F45 = stream.ReadValueF32(); //476
            stream.Position += 4;
            this.F46 = stream.ReadValueF32(); //484
            this.F47 = stream.ReadValueF32(); //488
            this.F48 = stream.ReadValueF32(); //492
            this.F49 = stream.ReadValueF32(); //496
            stream.Position += 4;
            this.F50 = stream.ReadValueF32(); //504
            stream.Position += 12;
            this.F51 = stream.ReadValueF32(); //520
            stream.Position += 36;
        }
    }

    public class HeroTable : ISerializableData
    {
        //Total Length: 868
        public string Name { get; private set; }
        public int SNOMaleActor { get; private set; }
        public int SNOFemaleActor { get; private set; }
        public int SNOInventory { get; private set; }
        public int I0 { get; private set; }
        public int SNOStartingLMBSkill { get; private set; }
        public int SNOStartingRMBSkill { get; private set; }
        public int SNOSKillKit0 { get; private set; }
        public int SNOSKillKit1 { get; private set; }
        public int SNOSKillKit2 { get; private set; }
        public int SNOSKillKit3 { get; private set; }
        public Resource PrimaryResource { get; private set; }
        public Resource SecondaryResource { get; private set; }
        public PrimaryAttribute CoreAttribute { get; private set; }
        public float F0 { get; private set; }
        public int I1 { get; private set; }
        public float HitpointsMax { get; private set; }
        public float HitpointsFactorLevel { get; private set; }
        public float F3 { get; private set; }
        public float PrimaryResourceMax { get; private set; }
        public float PrimaryResourceFactorLevel { get; private set; }
        public float PrimaryResourceRegenPerSecond { get; private set; }
        public float SecondaryResourceMax { get; private set; }
        public float SecondaryResourceFactorLevel { get; private set; }
        public float SecondaryResourceRegenPerSecond { get; private set; }
        public float F10 { get; private set; }
        public float F11 { get; private set; }
        public float CritPercentCap { get; private set; }
        public float F13 { get; private set; }
        public float F14 { get; private set; }
        public float WalkingRate { get; private set; }
        public float RunningRate { get; private set; }
        public float F17 { get; private set; }
        public float F18 { get; private set; }
        public float F19 { get; private set; }
        public float F20 { get; private set; }
        public float F21 { get; private set; }
        public float F22 { get; private set; }
        public float F23 { get; private set; }
        public float F24 { get; private set; } //Resistance?
        public float F25 { get; private set; } //ResistanceTotal?
        public float F26 { get; private set; } //CastingSpeed?
        public float F27 { get; private set; }
        public float F28 { get; private set; }
        public float F29 { get; private set; }
        public float F30 { get; private set; } //HitChance?
        public float F31 { get; private set; }
        public float F32 { get; private set; }
        public float F33 { get; private set; }
        public float F34 { get; private set; }
        public float Strength { get; private set; }
        public float Dexterity { get; private set; }
        public float Vitality { get; private set; }
        public float Intelligence { get; private set; }
        public float GetHitMaxBase { get; private set; }
        public float GetHitMaxPerLevel { get; private set; }
        public float GetHitRecoveryBase { get; private set; }
        public float GetHitRecoveryPerLevel { get; private set; }
        public float F35 { get; private set; }

        public void Read(MpqFileStream stream)
        {
            stream.Position += 4;
            this.Name = stream.ReadString(256, true);
            this.SNOMaleActor = stream.ReadValueS32();
            this.SNOFemaleActor = stream.ReadValueS32();
            this.SNOInventory = stream.ReadValueS32();
            this.I0 = stream.ReadValueS32();
            this.SNOStartingLMBSkill = stream.ReadValueS32();
            this.SNOStartingRMBSkill = stream.ReadValueS32();
            this.SNOSKillKit0 = stream.ReadValueS32();
            this.SNOSKillKit1 = stream.ReadValueS32();
            this.SNOSKillKit2 = stream.ReadValueS32();
            this.SNOSKillKit3 = stream.ReadValueS32();
            this.PrimaryResource = (Resource)stream.ReadValueS32();
            this.SecondaryResource = (Resource)stream.ReadValueS32();
            this.CoreAttribute = (PrimaryAttribute)stream.ReadValueS32();
            this.F0 = stream.ReadValueF32(); //312
            this.I1 = stream.ReadValueS32(); //316
            stream.Position += 16;
            this.HitpointsMax = stream.ReadValueF32(); //336
            this.HitpointsFactorLevel = stream.ReadValueF32(); //340
            stream.Position += 8;
            this.F3 = stream.ReadValueF32(); //352
            this.PrimaryResourceMax = stream.ReadValueF32(); //356
            this.PrimaryResourceFactorLevel = stream.ReadValueF32(); //360
            this.PrimaryResourceRegenPerSecond = stream.ReadValueF32(); //364
            stream.Position += 4;
            this.SecondaryResourceMax = stream.ReadValueF32(); //372
            this.SecondaryResourceFactorLevel = stream.ReadValueF32(); //376
            this.SecondaryResourceRegenPerSecond = stream.ReadValueF32(); //380
            stream.Position += 24;
            this.F10 = stream.ReadValueF32(); //408
            stream.Position += 72;
            this.F11 = stream.ReadValueF32(); //484
            this.CritPercentCap = stream.ReadValueF32(); //488
            this.F13 = stream.ReadValueF32(); //492
            stream.Position += 4;
            this.F14 = stream.ReadValueF32(); //500
            stream.Position += 32;
            this.WalkingRate = stream.ReadValueF32(); //536
            this.RunningRate = stream.ReadValueF32(); //540
            stream.Position += 4;
            this.F17 = stream.ReadValueF32(); //548
            stream.Position += 32;
            this.F18 = stream.ReadValueF32(); //584
            this.F19 = stream.ReadValueF32(); //588
            this.F20 = stream.ReadValueF32(); //592
            this.F21 = stream.ReadValueF32(); //596
            this.F22 = stream.ReadValueF32(); //600
            this.F23 = stream.ReadValueF32(); //604
            stream.Position += 4;
            this.F24 = stream.ReadValueF32(); //612
            this.F25 = stream.ReadValueF32(); //616
            stream.Position += 60;
            this.F26 = stream.ReadValueF32(); //680
            stream.Position += 8;
            this.F27 = stream.ReadValueF32(); //692
            this.F28 = stream.ReadValueF32(); //696
            this.F29 = stream.ReadValueF32(); //700
            this.F30 = stream.ReadValueF32(); //704
            stream.Position += 12;
            this.F31 = stream.ReadValueF32(); //720
            this.F32 = stream.ReadValueF32(); //724
            this.F33 = stream.ReadValueF32(); //728
            stream.Position += 40;
            this.F34 = stream.ReadValueF32(); //772
            stream.Position += 24;
            this.Strength = stream.ReadValueF32(); //800
            this.Dexterity = stream.ReadValueF32(); //804
            this.Vitality = stream.ReadValueF32(); //808
            this.Intelligence = stream.ReadValueF32(); //812
            stream.Position += 40;
            this.GetHitMaxBase = stream.ReadValueF32(); //856
            this.GetHitMaxPerLevel = stream.ReadValueF32(); //860
            this.GetHitRecoveryBase = stream.ReadValueF32(); //864
            this.GetHitRecoveryPerLevel = stream.ReadValueF32(); //866
            this.F35 = stream.ReadValueF32();
        }

        public enum Resource : int
        {
            None = -1,
            Mana = 0,
            Arcanum,
            Fury,
            Spirit,
            Power,
            Hatred,
            Discipline,
        }

    }

    public class MovementStyle : ISerializableData //0 byte file
    {
        //Total Length: 384
        [PersistentPropertyAttribute("Name")]
        public string Name { get; private set; }

        [PersistentPropertyAttribute("I0")]
        public int I0 { get; private set; }

        [PersistentPropertyAttribute("I1")]
        public int I1 { get; private set; }

        [PersistentPropertyAttribute("I2")]
        public int I2 { get; private set; }

        [PersistentPropertyAttribute("I3")]
        public int I3 { get; private set; }

        [PersistentPropertyAttribute("I4")]
        public int I4 { get; private set; }

        [PersistentPropertyAttribute("I5")]
        public int I5 { get; private set; }

        [PersistentPropertyAttribute("I6")]
        public int I6 { get; private set; }

        [PersistentPropertyAttribute("I7")]
        public int I7 { get; private set; }

        [PersistentPropertyAttribute("F0")]
        public float F0 { get; private set; }

        [PersistentPropertyAttribute("F1")]
        public float F1 { get; private set; }

        [PersistentPropertyAttribute("F2")]
        public float F2 { get; private set; }

        [PersistentPropertyAttribute("F3")]
        public float F3 { get; private set; }

        [PersistentPropertyAttribute("F4")]
        public float F4 { get; private set; }

        [PersistentPropertyAttribute("F5")]
        public float F5 { get; private set; }

        [PersistentPropertyAttribute("F6")]
        public float F6 { get; private set; }

        [PersistentPropertyAttribute("F7")]
        public float F7 { get; private set; }

        [PersistentPropertyAttribute("F8")]
        public float F8 { get; private set; }

        [PersistentPropertyAttribute("F9")]
        public float F9 { get; private set; }

        [PersistentPropertyAttribute("F10")]
        public float F10 { get; private set; }

        [PersistentPropertyAttribute("F11")]
        public float F11 { get; private set; }

        [PersistentPropertyAttribute("F12")]
        public float F12 { get; private set; }

        [PersistentPropertyAttribute("F13")]
        public float F13 { get; private set; }

        [PersistentPropertyAttribute("F14")]
        public float F14 { get; private set; }

        [PersistentPropertyAttribute("F15")]
        public float F15 { get; private set; }

        [PersistentPropertyAttribute("F16")]
        public float F16 { get; private set; }

        [PersistentPropertyAttribute("F17")]
        public float F17 { get; private set; }

        [PersistentPropertyAttribute("F18")]
        public float F18 { get; private set; }

        [PersistentPropertyAttribute("F19")]
        public float F19 { get; private set; }

        [PersistentPropertyAttribute("F20")]
        public float F20 { get; private set; }

        [PersistentPropertyAttribute("F21")]
        public float F21 { get; private set; }

        [PersistentPropertyAttribute("SNOPowerToBreakObjects")]
        public int SNOPowerToBreakObjects { get; private set; }

        public void Read(MpqFileStream stream)
        {
            stream.Position += 4;
            this.Name = stream.ReadString(256, true);
            this.I0 = stream.ReadValueS32();
            this.I1 = stream.ReadValueS32();
            this.I2 = stream.ReadValueS32();
            this.I3 = stream.ReadValueS32();
            this.I4 = stream.ReadValueS32();
            this.I5 = stream.ReadValueS32();
            this.I6 = stream.ReadValueS32();
            this.I7 = stream.ReadValueS32();
            this.F0 = stream.ReadValueF32();
            this.F1 = stream.ReadValueF32();
            this.F2 = stream.ReadValueF32();
            this.F3 = stream.ReadValueF32();
            this.F4 = stream.ReadValueF32();
            this.F5 = stream.ReadValueF32();
            this.F6 = stream.ReadValueF32();
            this.F7 = stream.ReadValueF32();
            this.F8 = stream.ReadValueF32();
            this.F9 = stream.ReadValueF32();
            this.F10 = stream.ReadValueF32();
            this.F11 = stream.ReadValueF32();
            this.F12 = stream.ReadValueF32();
            this.F13 = stream.ReadValueF32();
            this.F14 = stream.ReadValueF32();
            this.F15 = stream.ReadValueF32();
            this.F16 = stream.ReadValueF32();
            this.F17 = stream.ReadValueF32();
            this.F18 = stream.ReadValueF32();
            this.F19 = stream.ReadValueF32();
            this.F20 = stream.ReadValueF32();
            this.F21 = stream.ReadValueF32();
            this.SNOPowerToBreakObjects = stream.ReadValueS32();
        }
    }

    public class LabelGBIDTable : ISerializableData
    {
        //Total Length: 264
        public string Name { get; private set; }
        public int I0 { get; private set; }

        public void Read(MpqFileStream stream)
        {
            stream.Position += 4;
            this.Name = stream.ReadString(256, true);
            this.I0 = stream.ReadValueS32();
        }
    }

    public class AffixTable : ISerializableData
    {
        //Total Length: 544
        public int Hash { get; private set; }
        public string Name { get; private set; }
        public int I0 { get; private set; }
        public int AffixLevel { get; private set; }
        public int SupMask { get; private set; }
        public int I3 { get; private set; }
        public int I4 { get; private set; }
        public int I5 { get; private set; }
        public int I8 { get; private set; }
        public int I9 { get; private set; }
        public AffixType1 AffixType1 { get; private set; }
        public int I6 { get; private set; }
        public int SNORareNamePrefixStringList { get; private set; }
        public int SNORareNameSuffixStringList { get; private set; }
        public int AffixFamily0 { get; private set; }
        public int AffixFamily1 { get; private set; }
        public Class Class { get; private set; }
        public int ExclusionCategory { get; private set; }
        public int[] I7 { get; private set; }
        public int[] ItemGroup { get; private set; }
        public int[] I10 { get; private set; }
        public QualityMask QualityMask { get; private set; }
        public AffixType2 AffixType2 { get; private set; }
        public int AssociatedAffix { get; private set; }
        public AttributeSpecifier[] AttributeSpecifier { get; private set; }

        public void Read(MpqFileStream stream)
        {
            stream.Position += 4;
            this.Name = stream.ReadString(256, true);
            this.Hash = StringHashHelper.HashItemName(this.Name);
            this.I0 = stream.ReadValueS32(); //260
            this.AffixLevel = stream.ReadValueS32(); //264
            this.SupMask = stream.ReadValueS32(); //268
            this.I3 = stream.ReadValueS32(); //272
            this.I4 = stream.ReadValueS32(); //276
            this.I5 = stream.ReadValueS32(); //280
            this.I8 = stream.ReadValueS32(); //284
            this.I9 = stream.ReadValueS32(); //288
            this.AffixType1 = (AffixType1)stream.ReadValueS32(); //292
            this.I6 = stream.ReadValueS32(); //296
            this.SNORareNamePrefixStringList = stream.ReadValueS32(); //300
            this.SNORareNameSuffixStringList = stream.ReadValueS32(); //304
            this.AffixFamily0 = stream.ReadValueS32(); //308
            this.AffixFamily1 = stream.ReadValueS32(); //312
            this.Class = (Class)stream.ReadValueS32(); //316
            this.ExclusionCategory = stream.ReadValueS32(); //320
            this.I7 = new int[6]; //324
            for (int i = 0; i < 6; i++)
                this.I7[i] = stream.ReadValueS32();
            this.ItemGroup = new int[16]; //348
            for (int i = 0; i < 16; i++)
                this.ItemGroup[i] = stream.ReadValueS32();
            this.I10 = new int[16]; //412
            for (int i = 0; i < 16; i++)
                this.I10[i] = stream.ReadValueS32();
            this.QualityMask = (QualityMask)stream.ReadValueS32(); //476
            this.AffixType2 = (AffixType2)stream.ReadValueS32(); //480
            this.AssociatedAffix = stream.ReadValueS32(); //484
            this.AttributeSpecifier = new AttributeSpecifier[4]; //488
            for (int i = 0; i < 4; i++)
                this.AttributeSpecifier[i] = new AttributeSpecifier(stream);
            stream.Position += 72;
        }
    }

    public enum AffixType1
    {
        None = 0,
        Lightning,
        Cold,
        Fire,
        Poison,
        Arcane,
        WitchdoctorDamage,
        LifeSteal,
        ManaSteal,
        MagicFind,
        GoldFind,
        AttackSpeedBonus,
        Holy,
        WizardDamage,
    }

    public enum AffixType2
    {
        Prefix = 0,
        Suffix = 1,
        Inherit = 2,
        Title = 5,
        Quality = 6,
        Immunity = 7,
        Random = 9,
        Enhancement = 10,
        SocketEnhancement = 11,
    }

    [Flags]
    public enum QualityMask
    {
        Inferior = 0x1,
        Normal = 0x2,
        Superior = 0x4,
        Magic1 = 0x8,
        Magic2 = 0x10,
        Magic3 = 0x20,
        Rare4 = 0x40,
        Rare5 = 0x80,
        Rare6 = 0x100,
        Legendary = 0x200,
    }

    public class AttributeSpecifier
    {
        //Length: 24
        public int AttributeId { get; private set; }
        public int SNOParam { get; private set; }
        public List<int> Formula { get; private set; }

        public AttributeSpecifier(MpqFileStream stream)
        {
            this.AttributeId = stream.ReadValueS32();
            this.SNOParam = stream.ReadValueS32();
            stream.Position += 8;
            this.Formula = stream.ReadSerializedInts();
        }
    }

    public class LootDistributionTableEntry : ISerializableData //0 byte file
    {
        //Total Length: 92
        [PersistentPropertyAttribute("I0")]
        public int I0 { get; private set; }

        [PersistentPropertyAttribute("I1")]
        public int I1 { get; private set; }

        [PersistentPropertyAttribute("I2")]
        public int I2 { get; private set; }

        [PersistentPropertyAttribute("I3")]
        public int I3 { get; private set; }

        [PersistentPropertyAttribute("I4")]
        public int I4 { get; private set; }

        [PersistentPropertyAttribute("I5")]
        public int I5 { get; private set; }

        [PersistentPropertyAttribute("I6")]
        public int I6 { get; private set; }

        [PersistentPropertyAttribute("I7")]
        public int I7 { get; private set; }

        [PersistentPropertyAttribute("I8")]
        public int I8 { get; private set; }

        [PersistentPropertyAttribute("I9")]
        public int I9 { get; private set; }

        [PersistentPropertyAttribute("F0")]
        public float F0 { get; private set; }

        [PersistentPropertyAttribute("F1")]
        public float F1 { get; private set; }

        [PersistentPropertyAttribute("F2")]
        public float F2 { get; private set; }

        [PersistentPropertyAttribute("F3")]
        public float F3 { get; private set; }

        [PersistentPropertyAttribute("F4")]
        public float F4 { get; private set; }

        [PersistentPropertyAttribute("F5")]
        public float F5 { get; private set; }

        [PersistentPropertyAttribute("F6")]
        public float F6 { get; private set; }

        [PersistentPropertyAttribute("F7")]
        public float F7 { get; private set; }

        [PersistentPropertyAttribute("F8")]
        public float F8 { get; private set; }

        [PersistentPropertyAttribute("F9")]
        public float F9 { get; private set; }

        [PersistentPropertyAttribute("F10")]
        public float F10 { get; private set; }

        [PersistentPropertyAttribute("I10")]
        public int I10 { get; private set; }

        [PersistentPropertyAttribute("I11")]
        public int I11 { get; private set; }

        public void Read(MpqFileStream stream)
        {
            this.I0 = stream.ReadValueS32();
            this.I1 = stream.ReadValueS32();
            this.I2 = stream.ReadValueS32();
            this.I3 = stream.ReadValueS32();
            this.I4 = stream.ReadValueS32();
            this.I5 = stream.ReadValueS32();
            this.I6 = stream.ReadValueS32();
            this.I7 = stream.ReadValueS32();
            this.I8 = stream.ReadValueS32();
            this.I9 = stream.ReadValueS32();
            this.F0 = stream.ReadValueF32();
            this.F1 = stream.ReadValueF32();
            this.F2 = stream.ReadValueF32();
            this.F3 = stream.ReadValueF32();
            this.F4 = stream.ReadValueF32();
            this.F5 = stream.ReadValueF32();
            this.F6 = stream.ReadValueF32();
            this.F7 = stream.ReadValueF32();
            this.F8 = stream.ReadValueF32();
            this.F9 = stream.ReadValueF32();
            this.F10 = stream.ReadValueF32();
            this.I10 = stream.ReadValueS32();
            this.I11 = stream.ReadValueS32();
        }
    }

    public class RareItemNamesTable : ISerializableData
    {
        //Total Length: 272
        public string Name { get; private set; }
        public BalanceType Type { get; private set; }
        public int RelatedAffixOrItemType { get; private set; }
        public AffixType2 AffixType { get; private set; }

        public void Read(MpqFileStream stream)
        {
            stream.Position += 4;
            this.Name = stream.ReadString(256, true);
            this.Type = (BalanceType)stream.ReadValueS32();
            this.RelatedAffixOrItemType = stream.ReadValueS32();
            this.AffixType = (AffixType2)stream.ReadValueS32();
        }
    }

    public class MonsterAffixesTable : ISerializableData
    {
        //Total Length: 792
        public string Name { get; private set; }
        public int I0 { get; private set; }
        public int I1 { get; private set; }
        public int I2 { get; private set; }
        public MonsterAffix MonsterAffix { get; private set; }
        public Resistance Resistance { get; private set; }
        public AffixType2 AffixType { get; private set; }
        public int I3 { get; private set; }
        public int I4 { get; private set; }
        public int I5 { get; private set; }
        public AttributeSpecifier[] Attributes { get; private set; }
        public AttributeSpecifier[] MinionAttributes { get; private set; }
        public int SNOOnSpawnPowerMinion { get; private set; }
        public int SNOOnSpawnPowerChampion { get; private set; }
        public int SNOOnSpawnPowerRare { get; private set; }

        public void Read(MpqFileStream stream)
        {
            stream.Position += 4;
            this.Name = stream.ReadString(256, true);
            this.I0 = stream.ReadValueS32();
            this.I1 = stream.ReadValueS32();
            this.I2 = stream.ReadValueS32();
            this.MonsterAffix = (MonsterAffix)stream.ReadValueS32();
            this.Resistance = (Resistance)stream.ReadValueS32();
            this.AffixType = (AffixType2)stream.ReadValueS32();
            this.I3 = stream.ReadValueS32();
            this.I4 = stream.ReadValueS32();
            this.I5 = stream.ReadValueS32();
            this.Attributes = new AttributeSpecifier[10];
            for (int i = 0; i < 10; i++)
                this.Attributes[i] = new AttributeSpecifier(stream);
            this.MinionAttributes = new AttributeSpecifier[10];
            for (int i = 0; i < 10; i++)
                this.MinionAttributes[i] = new AttributeSpecifier(stream);
            stream.Position += 4;
            this.SNOOnSpawnPowerMinion = stream.ReadValueS32();
            this.SNOOnSpawnPowerChampion = stream.ReadValueS32();
            this.SNOOnSpawnPowerRare = stream.ReadValueS32();
        }
    }

    public enum MonsterAffix
    {
        All = 0,
        Rares,
        Shooters,
        Champions
    }

    public enum Resistance
    {
        Physical = 0,
        Fire,
        Lightning,
        Cold,
        Poison,
        Arcane,
        Holy,
    }

    public class MonsterNamesTable : ISerializableData
    {
        //Total Length: 392
        public string Name { get; private set; }
        public AffixType2 AffixType { get; private set; }
        public string S0 { get; private set; }

        public void Read(MpqFileStream stream)
        {
            stream.Position += 4;
            this.Name = stream.ReadString(256, true);
            this.AffixType = (AffixType2)stream.ReadValueS32();
            this.S0 = stream.ReadString(128, true);
        }

    }

    public class SocketedEffectTable : ISerializableData
    {
        //Total Length: 1416
        public string Name { get; private set; }
        public int Item { get; private set; }
        public int ItemType { get; private set; }
        public AttributeSpecifier[] Attribute { get; private set; }
        public AttributeSpecifier[] ReqAttribute { get; private set; }
        public string S0 { get; private set; }

        public void Read(MpqFileStream stream)
        {
            stream.Position += 4;
            this.Name = stream.ReadString(256, true);
            this.Item = stream.ReadValueS32(); //260
            this.ItemType = stream.ReadValueS32(); //264
            stream.Position += 4;
            this.Attribute = new AttributeSpecifier[3];
            for (int i = 0; i < 3; i++)
                this.Attribute[i] = new AttributeSpecifier(stream);
            this.ReqAttribute = new AttributeSpecifier[2];
            for (int i = 0; i < 2; i++)
                this.ReqAttribute[i] = new AttributeSpecifier(stream);
            this.S0 = stream.ReadString(1024, true);
        }
    }

    public class ItemEnhancementTable : ISerializableData
    {
        //Total Length: 696
        public string Name { get; private set; }
        public int I0 { get; private set; }
        public int I1 { get; private set; }
        public int I2 { get; private set; }
        public int I3 { get; private set; }
        public AttributeSpecifier[] Attribute { get; private set; }
        public int I4 { get; private set; }
        public RecipeIngredient[] Ingredients { get; private set; }

        public void Read(MpqFileStream stream)
        {
            stream.Position += 4;
            this.Name = stream.ReadString(256, true);
            this.I0 = stream.ReadValueS32(); //260
            this.I1 = stream.ReadValueS32(); //264
            this.I2 = stream.ReadValueS32(); //268
            this.I3 = stream.ReadValueS32(); //272
            stream.Position += 4;
            this.Attribute = new AttributeSpecifier[16]; //280
            for (int i = 0; i < 16; i++)
                this.Attribute[i] = new AttributeSpecifier(stream);
            this.I4 = stream.ReadValueS32(); //664
            this.Ingredients = new RecipeIngredient[3]; //668
            for (int i = 0; i < 3; i++)
                this.Ingredients[i] = new RecipeIngredient(stream);
            stream.Position += 4;
        }
    }

    public class RecipeIngredient
    {
        public int ItemGBId { get; private set; }
        public int Count { get; private set; }

        public RecipeIngredient(MpqFileStream stream)
        {
            this.ItemGBId = stream.ReadValueS32();
            this.Count = stream.ReadValueS32();
        }
    }

    public class ItemDropTableEntry : ISerializableData //0 byte file
    {
        //Total Length: 1140
        [PersistentProperty("Name")]
        public string Name { get; private set; }

        [PersistentProperty("I0", 220)]
        public int[] I0 { get; private set; }

        public void Read(MpqFileStream stream)
        {
            stream.Position += 4;
            this.Name = stream.ReadString(256, true);
            this.I0 = new int[220];
            for (int i = 0; i < 220; i++)
                this.I0[i] = stream.ReadValueS32();
        }
    }

    public class ItemLevelModifier : ISerializableData //0 byte file
    {
        //Total Length: 92
        [PersistentPropertyAttribute("I0")]
        public int I0 { get; private set; }

        [PersistentPropertyAttribute("F0")]
        public float F0 { get; private set; }

        [PersistentPropertyAttribute("F1")]
        public float F1 { get; private set; }

        [PersistentPropertyAttribute("F2")]
        public float F2 { get; private set; }

        [PersistentPropertyAttribute("F3")]
        public float F3 { get; private set; }

        [PersistentPropertyAttribute("F4")]
        public float F4 { get; private set; }

        [PersistentPropertyAttribute("F5")]
        public float F5 { get; private set; }

        [PersistentPropertyAttribute("F6")]
        public float F6 { get; private set; }

        [PersistentPropertyAttribute("F7")]
        public float F7 { get; private set; }

        [PersistentPropertyAttribute("F8")]
        public float F8 { get; private set; }

        [PersistentPropertyAttribute("F9")]
        public float F9 { get; private set; }

        [PersistentPropertyAttribute("F10")]
        public float F10 { get; private set; }

        [PersistentPropertyAttribute("I1")]
        public int I1 { get; private set; }

        [PersistentPropertyAttribute("I2")]
        public int I2 { get; private set; }

        [PersistentPropertyAttribute("I3")]
        public int I3 { get; private set; }

        [PersistentPropertyAttribute("I4")]
        public int I4 { get; private set; }

        [PersistentPropertyAttribute("I5")]
        public int I5 { get; private set; }

        [PersistentPropertyAttribute("I6")]
        public int I6 { get; private set; }

        [PersistentPropertyAttribute("I7")]
        public int I7 { get; private set; }

        [PersistentPropertyAttribute("I8")]
        public int I8 { get; private set; }

        [PersistentPropertyAttribute("I9")]
        public int I9 { get; private set; }

        [PersistentPropertyAttribute("I10")]
        public int I10 { get; private set; }

        [PersistentPropertyAttribute("I11")]
        public int I11 { get; private set; }

        public void Read(MpqFileStream stream)
        {
            this.I0 = stream.ReadValueS32();
            this.F0 = stream.ReadValueF32();
            this.F1 = stream.ReadValueF32();
            this.F2 = stream.ReadValueF32();
            this.F3 = stream.ReadValueF32();
            this.F4 = stream.ReadValueF32();
            this.F5 = stream.ReadValueF32();
            this.F6 = stream.ReadValueF32();
            this.F7 = stream.ReadValueF32();
            this.F8 = stream.ReadValueF32();
            this.F9 = stream.ReadValueF32();
            this.F10 = stream.ReadValueF32();
            this.I1 = stream.ReadValueS32();
            this.I2 = stream.ReadValueS32();
            this.I3 = stream.ReadValueS32();
            this.I4 = stream.ReadValueS32();
            this.I5 = stream.ReadValueS32();
            this.I6 = stream.ReadValueS32();
            this.I7 = stream.ReadValueS32();
            this.I8 = stream.ReadValueS32();
            this.I9 = stream.ReadValueS32();
            this.I10 = stream.ReadValueS32();
            this.I11 = stream.ReadValueS32();
        }
    }

    public class QualityClass : ISerializableData //0 byte file
    {
        //Total Length: 352
        [PersistentProperty("Name")]
        public string Name { get; private set; }

        [PersistentProperty("F0", 22)]
        public float[] F0 { get; private set; }

        public void Read(MpqFileStream stream)
        {
            stream.Position += 4;
            this.Name = stream.ReadString(256, true);
            stream.Position += 4;
            this.F0 = new float[22];
            for (int i = 0; i < 22; i++)
                this.F0[i] = stream.ReadValueF32();
        }
    }

    public class HirelingTable : ISerializableData
    {
        //Total Length: 824
        public string Name { get; private set; }
        public int SNOActor { get; private set; }
        public int SNOProxy { get; private set; }
        public int SNOInventory { get; private set; }
        public PrimaryAttribute Attribute { get; private set; }
        public float F0 { get; private set; }
        public float F1 { get; private set; }
        public float F2 { get; private set; }
        public float F3 { get; private set; }
        public float F4 { get; private set; }
        public float F5 { get; private set; }
        public float F6 { get; private set; }
        public float F7 { get; private set; }
        public float F8 { get; private set; }
        public float F9 { get; private set; }

        public void Read(MpqFileStream stream)
        {
            stream.Position += 4;
            this.Name = stream.ReadString(256, true);
            this.SNOActor = stream.ReadValueS32();
            this.SNOProxy = stream.ReadValueS32();
            this.SNOInventory = stream.ReadValueS32();
            this.Attribute = (PrimaryAttribute)stream.ReadValueS32();
            stream.Position += 164;
            this.F0 = stream.ReadValueF32(); //440
            this.F1 = stream.ReadValueF32(); //444
            stream.Position += 280;
            this.F2 = stream.ReadValueF32(); //728
            stream.Position += 24;
            this.F3 = stream.ReadValueF32(); //756
            this.F4 = stream.ReadValueF32(); //760
            this.F5 = stream.ReadValueF32(); //764
            this.F6 = stream.ReadValueF32(); //768
            stream.Position += 8;
            this.F7 = stream.ReadValueF32(); //780
            this.F8 = stream.ReadValueF32(); //784
            this.F9 = stream.ReadValueF32(); //788
            stream.Position += 40;
        }

    }

    public class SetItemBonusTable : ISerializableData
    {
        //Total Length: 464
        public string Name { get; private set; }
        public int I0 { get; private set; }
        public int Set { get; private set; }
        public AttributeSpecifier[] Attribute { get; private set; }

        public void Read(MpqFileStream stream)
        {
            stream.Position += 4;
            this.Name = stream.ReadString(256, true);
            this.Set = stream.ReadValueS32();
            this.I0 = stream.ReadValueS32();
            stream.Position += 4;
            Attribute = new AttributeSpecifier[8];
            for (int i = 0; i < 8; i++)
                Attribute[i] = new AttributeSpecifier(stream);
        }

    }

    public class EliteModifier : ISerializableData //0 byte file
    {
        //Total Length: 344

        [PersistentProperty("Name")]
        public string Name { get; private set; }

        [PersistentProperty("F0")]
        public float F0 { get; private set; }

        [PersistentProperty("Time0")]
        public int Time0 { get; private set; }

        [PersistentProperty("F1")]
        public float F1 { get; private set; }

        [PersistentProperty("Time1")]
        public int Time1 { get; private set; }

        [PersistentProperty("F2")]
        public float F2 { get; private set; }

        [PersistentProperty("Time2")]
        public int Time2 { get; private set; }

        [PersistentProperty("F3")]
        public float F3 { get; private set; }

        [PersistentProperty("Time3")]
        public int Time3 { get; private set; }

        [PersistentProperty("F4")]
        public float F4 { get; private set; }

        [PersistentProperty("Time4")]
        public int Time4 { get; private set; }

        [PersistentProperty("F5")]
        public float F5 { get; private set; }

        [PersistentProperty("Time5")]
        public int Time5 { get; private set; }

        [PersistentProperty("F6")]
        public float F6 { get; private set; }

        [PersistentProperty("Time6")]
        public int Time6 { get; private set; }

        [PersistentProperty("F7")]
        public float F7 { get; private set; }

        [PersistentProperty("F8")]
        public float F8 { get; private set; }

        [PersistentProperty("Time7")]
        public int Time7 { get; private set; }

        [PersistentProperty("F9")]
        public float F9 { get; private set; }

        [PersistentProperty("F10")]
        public float F10 { get; private set; }

        [PersistentProperty("F11")]
        public float F11 { get; private set; }

        [PersistentProperty("F12")]
        public float F12 { get; private set; }

        public void Read(MpqFileStream stream)
        {
            stream.Position += 4;
            this.Name = stream.ReadString(256, true);
            this.F0 = stream.ReadValueF32();
            this.Time0 = stream.ReadValueS32();
            this.F1 = stream.ReadValueF32();
            this.Time1 = stream.ReadValueS32();
            this.F2 = stream.ReadValueF32();
            this.Time2 = stream.ReadValueS32();
            this.F3 = stream.ReadValueF32();
            this.Time3 = stream.ReadValueS32();
            this.F4 = stream.ReadValueF32();
            this.Time4 = stream.ReadValueS32();
            this.F5 = stream.ReadValueF32();
            this.Time5 = stream.ReadValueS32();
            this.F6 = stream.ReadValueF32();
            this.Time6 = stream.ReadValueS32();
            this.F7 = stream.ReadValueF32();
            this.F8 = stream.ReadValueF32();
            this.Time7 = stream.ReadValueS32();
            this.F9 = stream.ReadValueF32();
            this.F10 = stream.ReadValueF32();
            this.F11 = stream.ReadValueF32();
            this.F12 = stream.ReadValueF32();
        }
    }

    public class ItemTier : ISerializableData //0 byte file
    {
        //Total Length: 32

        [PersistentPropertyAttribute("Head")]
        public int Head { get; private set; }

        [PersistentPropertyAttribute("Torso")]
        public int Torso { get; private set; }

        [PersistentPropertyAttribute("Feet")]
        public int Feet { get; private set; }

        [PersistentPropertyAttribute("Hands")]
        public int Hands { get; private set; }

        [PersistentPropertyAttribute("Shoulders")]
        public int Shoulders { get; private set; }

        [PersistentPropertyAttribute("Bracers")]
        public int Bracers { get; private set; }

        [PersistentPropertyAttribute("Belt")]
        public int Belt { get; private set; }

        public void Read(MpqFileStream stream)
        {
            this.Head = stream.ReadValueS32();
            this.Torso = stream.ReadValueS32();
            this.Feet = stream.ReadValueS32();
            this.Hands = stream.ReadValueS32();
            this.Shoulders = stream.ReadValueS32();
            this.Bracers = stream.ReadValueS32();
            this.Belt = stream.ReadValueS32();
        }
    }

    public class PowerFormulaTable : ISerializableData
    {
        //Total Length: 1268
        public string S0 { get; private set; }
        public float[] F0 { get; private set; }

        public void Read(MpqFileStream stream)
        {
            this.S0 = stream.ReadString(1024, true);
            this.F0 = new float[61];
            for (int i = 0; i < 61; i++)
                this.F0[i] = stream.ReadValueF32();
        }
    }

    public class RecipeTable : ISerializableData
    {
        //Total Length: 332
        public string Name { get; private set; }
        public int SNORecipe { get; private set; }
        public RecipeType Type { get; private set; }
        public int I0 { get; private set; }
        public int I1 { get; private set; }
        public int I2 { get; private set; }
        public int I3 { get; private set; }
        public RecipeIngredient[] Ingredients { get; private set; }

        public void Read(MpqFileStream stream)
        {
            stream.Position += 4;
            this.Name = stream.ReadString(256, true);
            this.SNORecipe = stream.ReadValueS32();
            this.Type = (RecipeType)stream.ReadValueS32();
            this.I0 = stream.ReadValueS32();
            this.I1 = stream.ReadValueS32();
            this.I2 = stream.ReadValueS32();
            this.I3 = stream.ReadValueS32();
            this.Ingredients = new RecipeIngredient[6];
            for (int i = 0; i < 6; i++)
                this.Ingredients[i] = new RecipeIngredient(stream);
        }
    }

    public enum RecipeType
    {
        Blacksmith = 0,
        Jeweler,
        Mystic,
        None,
    }

    public class ScriptedAchievementEventsTable : ISerializableData //0 byte file
    {
        //Total Length: 260
        [PersistentPropertyAttribute("Name")]
        public string Name { get; private set; }

        public void Read(MpqFileStream stream)
        {
            stream.Position += 4;
            this.Name = stream.ReadString(256, true);
        }
    }

    public enum PrimaryAttribute : int
    {
        None = -1,
        Strength,
        Dexterity,
        Intelligence,
    }
}
