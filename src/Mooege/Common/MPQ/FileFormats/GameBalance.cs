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
using Mooege.Common.Extensions;
using Mooege.Common.MPQ.FileFormats.Types;

namespace Mooege.Common.MPQ.FileFormats
{
    [FileFormat(SNOGroup.GameBalance)]
    class GameBalance : FileFormat
    {
        public Header header;
        public BalanceType type;
        public string gbi;
        public string xls;
        public int i0;
        public int i1;
        public List<ItemTypeTable> itemType; //536
        public List<ItemTable> Item; //552
        public List<ExperienceTable> Experience; //568
        public List<HelpCodesTable> HelpCodes; //584
        public List<MonsterLevelTable> MonsterLevel; //600
        public List<AffixTable> Affixes; //616
        public List<HeroTable> Heros; //632
        public List<MovementStyleTable> MovementStyles; //648
        public List<LabelGBIDTable> Labels; //664
        public List<LootDistTable> LootDistribution; //680
        public List<RareItemNamesTable> RareItemNames; //696
        public List<MonsterAffixesTable> MonsterAffixes; //712
        public List<MonsterNamesTable> RareMonsterNames; //728
        public List<SocketedEffectTable> SocketedEffects; //744
        public List<ItemEnhancementTable> ItemEnhancement; //760
        public List<ItemDropTable> ItemDrop; //776
        public List<ItemLevelModTable> ItemLevelMod; //792
        public List<QualityClassTable> QualityClass; //808
        public List<HirelingTable> Hirelings; //824
        public List<SetItemBonusTable> SetItemBonus; //840
        public List<EliteModTable> EliteModifiers; //856
        public List<ItemTierTable> ItemTiers; //872
        public List<PowerFormulaTable> PowerFormula; //888
        public List<RecipeTable> Recipes; //904
        public List<ScriptedAchievementEventsTable> ScriptedAchievementEvents; //920

        public GameBalance(MpqFile file)
        {
            var stream = file.Open();
            this.header = new Header(stream);
            this.type = (BalanceType)stream.ReadValueS32();
            gbi = stream.ReadString(256,true);
            xls = stream.ReadString(256,true);
            this.i0 = stream.ReadValueS32();
            this.i1 = stream.ReadValueS32();
            this.itemType = stream.ReadSerializedData<ItemTypeTable>(); //536
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
            this.MovementStyles = stream.ReadSerializedData<MovementStyleTable>(); //648
            stream.Position += 8;
            this.Labels = stream.ReadSerializedData<LabelGBIDTable>(); //664
            stream.Position += 8;
            this.LootDistribution = stream.ReadSerializedData<LootDistTable>(); //680
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
            this.ItemDrop = stream.ReadSerializedData<ItemDropTable>(); //776
            stream.Position += 8;
            this.ItemLevelMod = stream.ReadSerializedData<ItemLevelModTable>(); //792
            stream.Position += 8;
            this.QualityClass = stream.ReadSerializedData<QualityClassTable>(); //808
            stream.Position += 8;
            this.Hirelings = stream.ReadSerializedData<HirelingTable>(); //824
            stream.Position += 8;
            this.SetItemBonus = stream.ReadSerializedData<SetItemBonusTable>(); //840
            stream.Position += 8;
            this.EliteModifiers = stream.ReadSerializedData<EliteModTable>(); //856
            stream.Position += 8;
            this.ItemTiers = stream.ReadSerializedData<ItemTierTable>(); //872
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
        public string Name; //256
        public int ParentType;
        public int i0;
        public ItemFlags Flags;
        public eItemType type0;
        public eItemType type1;
        public eItemType type2;
        public eItemType type3;
        public int InheritedAffix0;
        public int InheritedAffix1;
        public int InheritedAffix2;
        public int InheritedAffixFamily0;
        public int[] array; //len 4

        public void Read(MpqFileStream stream)
        {
            stream.Position += 4;
            this.Name = stream.ReadString(256,true);
            this.ParentType = stream.ReadValueS32();
            this.i0 = stream.ReadValueS32();
            this.Flags = (ItemFlags)stream.ReadValueS32();
            this.type0 = (eItemType)stream.ReadValueS32();
            this.type1 = (eItemType)stream.ReadValueS32();
            this.type2 = (eItemType)stream.ReadValueS32();
            this.type3 = (eItemType)stream.ReadValueS32();
            this.InheritedAffix0 = stream.ReadValueS32();
            this.InheritedAffix1 = stream.ReadValueS32();
            this.InheritedAffix2 = stream.ReadValueS32();
            this.InheritedAffixFamily0 = stream.ReadValueS32();
            this.array = new int[4];
            for (int i = 0; i < 4; i++)
                this.array[i] = stream.ReadValueS32();
        }
    }

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
        PlayerTalisman = 14,
        Merchant = 20,
        PetRightHand = 23,
        PetLeftHand = 24,
        PetSpecial = 25,
        PetLeftFinger = 28,
        PetRightFinger = 27,
        PetNeck = 26,
    }

    public class ItemTable : ISerializableData
    {
        //Total Length: 1456
        public string Name;
        public int snoActor;
        public int ItemType1;
        public int i0;
        public eItem e0;
        public int ItemLevel;
        public int i2;
        public int RandomPropertiesCount;
        public int MaxSockets;
        public int i5;
        public int BaseGoldValue;
        public int i7;
        public int RequiredLevel;
        public int DurabilityMin;
        public int DurabilityDelta;
        public int snoBaseItem;
        public int snoSet;
        public int snoComponentTreasureClass;
        public int snoComponentTreasureClassMagic;
        public int snoComponentTreasureClassRare;
        public int snoRareNamePrefixStringList;
        public int snoRareNameSuffixStringList;
        public int Flags;
        public float WeaponDamageMin;
        public float WeaponDamageDelta;
        public float ArmorValue;
        public float f3;
        public float AttacksPerSecond;
        public int snoSkill0;
        public int i11;
        public int snoSkill1;
        public int i12;
        public int snoSkill2;
        public int i13;
        public int snoSkill3;
        public int i14;
        public int[] i15; //len 4
        public AttributeSpecifier[] Attribute; //Len 16
        public ItemQuality Quality;
        public int[] RecipeToGrant; //len 6
        public int EnhancementToGrant;
        public int[] LegendaryAffixFamily;
        public int[] MaxAffixLevel;

        public void Read(MpqFileStream stream)
        {
            stream.Position += 4;
            this.Name = stream.ReadString(256,true);
            this.snoActor = stream.ReadValueS32(); //260
            this.ItemType1 = stream.ReadValueS32(); //264
            this.Flags = stream.ReadValueS32(); //268
            this.i0 = stream.ReadValueS32(); //272
            this.ItemLevel = stream.ReadValueS32(); //276
            this.e0 = (eItem)stream.ReadValueS32(); //280
            this.i2 = stream.ReadValueS32(); //284
            this.RandomPropertiesCount = stream.ReadValueS32(); //288
            this.MaxSockets = stream.ReadValueS32(); //292
            this.i5 = stream.ReadValueS32(); //296
            this.BaseGoldValue = stream.ReadValueS32(); //300
            this.i7 = stream.ReadValueS32(); //304
            this.RequiredLevel = stream.ReadValueS32(); //308
            this.DurabilityMin = stream.ReadValueS32(); //312
            this.DurabilityDelta = stream.ReadValueS32(); //316
            this.snoBaseItem = stream.ReadValueS32(); //320
            this.snoSet = stream.ReadValueS32(); //324
            this.snoComponentTreasureClass = stream.ReadValueS32(); //328
            this.snoComponentTreasureClassMagic = stream.ReadValueS32(); //332
            this.snoComponentTreasureClassRare = stream.ReadValueS32(); //336
            this.snoRareNamePrefixStringList = stream.ReadValueS32(); //340
            this.snoRareNameSuffixStringList = stream.ReadValueS32(); //344
            this.i15 = new int[4]; //348
            for (int i = 0; i < 4; i++)
                this.i15[i] = stream.ReadValueS32();
            stream.Position += 88;
            this.WeaponDamageMin = stream.ReadValueF32(); //452
            this.WeaponDamageDelta = stream.ReadValueF32(); //456
            stream.Position += 84;
            this.ArmorValue = stream.ReadValueF32(); //544
            this.f3 = stream.ReadValueF32(); //548
            stream.Position += 168;
            this.AttacksPerSecond = stream.ReadValueF32(); //720
            stream.Position += 192;
            this.snoSkill0 = stream.ReadValueS32(); //916
            this.i11 = stream.ReadValueS32(); //920
            this.snoSkill1 = stream.ReadValueS32(); //924
            this.i12 = stream.ReadValueS32(); //928
            this.snoSkill2 = stream.ReadValueS32(); //932
            this.i13 = stream.ReadValueS32(); //936
            this.snoSkill3 = stream.ReadValueS32(); //940
            this.i14 = stream.ReadValueS32(); //944
            stream.Position += 44;
            this.Attribute = new AttributeSpecifier[16];
            for (int i = 0; i < 16; i++)
                this.Attribute[i] = new AttributeSpecifier(stream);
            this.Quality = (ItemQuality)stream.ReadValueS32(); //1376
            this.RecipeToGrant = new int[6]; //1380
            for (int i = 0; i < 6; i++)
                this.RecipeToGrant[i] = stream.ReadValueS32();
            this.EnhancementToGrant = stream.ReadValueS32(); //1404
            this.LegendaryAffixFamily = new int[6];
            for (int i = 0; i < 6; i++)
                this.LegendaryAffixFamily[i] = stream.ReadValueS32(); //1408
            this.MaxAffixLevel = new int[6];
            for (int i = 0; i < 6; i++)
                this.MaxAffixLevel[i] = stream.ReadValueS32(); //1432
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

        public enum eItem
        {
            Invalid = -1,
            A1 = 0,
            A2 = 100,
            A3 = 200,
            A4 = 300,
            Test = 1000,
        }
    }

    public class ExperienceTable : ISerializableData
    {
        //Total Length: 224
        public int exp;
        public int i1;
        public float f0;
        public float f1;
        public int i2;
        public int i3;
        public int i4;
        public int i5;
        public int i6;
        public int i7;
        public int i8;
        public int i9;
        public int i10;
        public int i11;
        public int i12;
        public int i13;
        public int i14;
        public int i15;
        public int i16;
        public float multiplier;
        public int i17;
        public int i18;
        public int i19;
        public int i20;
        public int i21;
        public int i22;
        public int i23;
        public int i24;
        public int i25;
        public int i26;
        public int i27;
        public int i28;
        public int i29;
        public int i30;
        public int i31;
        public int i32;
        public int i33;
        public int i34;
        public int i35;
        public int i36;
        public int i37;
        public int i38;
        public int i39;
        public int i40;
        public int i41;
        public int i42;
        public int i43;
        public int i44;
        public int i45;
        public int i46;
        public int i47;

        public void Read(MpqFileStream stream)
        {
            this.exp = stream.ReadValueS32();
            this.i1 = stream.ReadValueS32();
            this.f0 = stream.ReadValueF32();
            this.f1 = stream.ReadValueF32();
            this.i2 = stream.ReadValueS32();
            this.i3 = stream.ReadValueS32();
            this.i4 = stream.ReadValueS32();
            this.i5 = stream.ReadValueS32();
            this.i6 = stream.ReadValueS32();
            this.i7 = stream.ReadValueS32();
            this.i8 = stream.ReadValueS32();
            this.i9 = stream.ReadValueS32();
            this.i10 = stream.ReadValueS32();
            this.i11 = stream.ReadValueS32();
            this.i12 = stream.ReadValueS32();
            this.i13 = stream.ReadValueS32();
            this.i14 = stream.ReadValueS32();
            this.i15 = stream.ReadValueS32();
            this.i16 = stream.ReadValueS32();
            this.multiplier = stream.ReadValueF32(); //76
            this.i16 = stream.ReadValueS32(); //80
            this.i17 = stream.ReadValueS32(); //84
            this.i18 = stream.ReadValueS32(); //88
            this.i19 = stream.ReadValueS32(); //92
            this.i20 = stream.ReadValueS32(); //96
            this.i21 = stream.ReadValueS32(); //100
            this.i22 = stream.ReadValueS32(); //104
            this.i23 = stream.ReadValueS32(); //108
            this.i24 = stream.ReadValueS32(); //112
            this.i25 = stream.ReadValueS32(); //116
            this.i26 = stream.ReadValueS32(); //120
            this.i27 = stream.ReadValueS32(); //124
            this.i28 = stream.ReadValueS32(); //128
            this.i29 = stream.ReadValueS32(); //132
            this.i30 = stream.ReadValueS32(); //136
            this.i31 = stream.ReadValueS32(); //140
            this.i32 = stream.ReadValueS32(); //144
            this.i33 = stream.ReadValueS32(); //148
            this.i34 = stream.ReadValueS32(); //152
            this.i35 = stream.ReadValueS32(); //156
            stream.Position += 16;
            this.i36 = stream.ReadValueS32(); //176
            this.i37 = stream.ReadValueS32();
            this.i38 = stream.ReadValueS32();
            this.i39 = stream.ReadValueS32();
            this.i40 = stream.ReadValueS32();
            this.i41 = stream.ReadValueS32();
            this.i42 = stream.ReadValueS32();
            this.i43 = stream.ReadValueS32();
            this.i44 = stream.ReadValueS32();
            this.i45 = stream.ReadValueS32();
            this.i46 = stream.ReadValueS32();
            this.i47 = stream.ReadValueS32();
        }
    }

    public class HelpCodesTable : ISerializableData //unused
    {
        //Total Length: 640
        public string s0; //0
        public string s1; //256
        public string s2; //512

        public void Read(MpqFileStream stream)
        {
            this.s0 = stream.ReadString(256,true);
            this.s1 = stream.ReadString(256,true);
            this.s2 = stream.ReadString(128,true);
        }
    }

    public class MonsterLevelTable : ISerializableData
    {
        //Total Length: 0x22C (556)
        public int i0; //0
        public float f0; //20
        public float f1; //32
        public float f2; //36
        public float f3; //40
        public float f4; //48
        public float f5; //92
        public float f6; //96
        public float f7; //100
        public float f8; //104
        public float f9; //108
        public float f10; //112
        public float f11; //116
        public float f12; //120
        public float f13; //124
        public float f14; //128
        public float f15; //132
        public float f16; //136
        public float f17; //140
        public float f18; //144
        public float f19; //148
        public float f20; //152
        public float f21; //156
        public float f22; //160
        public float f23; //176
        public float f24; //180
        public float f25; //184
        public float f26; //220
        public float f27; //224
        public float f28; //232
        public float f29; //236
        public float f30; //268
        public float f31; //272
        public float f32; //276
        public float f33; //280
        public float f34; //284
        public float f35; //288
        public float f36; //292
        public float f37; //296
        public float f38; //300
        public float f39; //440
        public float f40; //444
        public float f41; //460
        public float f42; //464
        public float f43; //468
        public float f44; //472
        public float f45; //476
        public float f46; //484
        public float f47; //488
        public float f48; //492
        public float f49; //496
        public float f50; //504
        public float f51; //520

        public void Read(MpqFileStream stream)
        {
            this.i0 = stream.ReadValueS32(); //0
            stream.Position += 16;
            this.f0 = stream.ReadValueF32(); //20
            stream.Position += 8;
            this.f1 = stream.ReadValueF32(); //32
            this.f2 = stream.ReadValueF32(); //36
            this.f3 = stream.ReadValueF32(); //40
            stream.Position += 4;
            this.f4 = stream.ReadValueF32(); //48
            stream.Position += 40;
            this.f5 = stream.ReadValueF32(); //92
            this.f6 = stream.ReadValueF32(); //96
            this.f7 = stream.ReadValueF32(); //100
            this.f8 = stream.ReadValueF32(); //104
            this.f9 = stream.ReadValueF32(); //108
            this.f10 = stream.ReadValueF32(); //112
            this.f11 = stream.ReadValueF32(); //116
            this.f12 = stream.ReadValueF32(); //120
            this.f13 = stream.ReadValueF32(); //124
            this.f14 = stream.ReadValueF32(); //128
            this.f15 = stream.ReadValueF32(); //132
            this.f16 = stream.ReadValueF32(); //136
            this.f17 = stream.ReadValueF32(); //140
            this.f18 = stream.ReadValueF32(); //144
            this.f19 = stream.ReadValueF32(); //148
            this.f20 = stream.ReadValueF32(); //152
            this.f21 = stream.ReadValueF32(); //156
            this.f22 = stream.ReadValueF32(); //160
            stream.Position += 12;
            this.f23 = stream.ReadValueF32(); //176
            this.f24 = stream.ReadValueF32(); //180
            this.f25 = stream.ReadValueF32(); //184
            stream.Position += 32;
            this.f26 = stream.ReadValueF32(); //220
            this.f27 = stream.ReadValueF32(); //224
            stream.Position += 4;
            this.f28 = stream.ReadValueF32(); //232
            this.f29 = stream.ReadValueF32(); //236
            stream.Position += 28;
            this.f30 = stream.ReadValueF32(); //268
            this.f31 = stream.ReadValueF32(); //272
            this.f32 = stream.ReadValueF32(); //276
            this.f33 = stream.ReadValueF32(); //280
            this.f34 = stream.ReadValueF32(); //284
            this.f35 = stream.ReadValueF32(); //288
            this.f36 = stream.ReadValueF32(); //292
            this.f37 = stream.ReadValueF32(); //296
            this.f38 = stream.ReadValueF32(); //300
            stream.Position += 136;
            this.f39 = stream.ReadValueF32(); //440
            this.f40 = stream.ReadValueF32(); //444
            stream.Position += 12;
            this.f41 = stream.ReadValueF32(); //460
            this.f42 = stream.ReadValueF32(); //464
            this.f43 = stream.ReadValueF32(); //468
            this.f44 = stream.ReadValueF32(); //472
            this.f45 = stream.ReadValueF32(); //476
            stream.Position += 4;
            this.f46 = stream.ReadValueF32(); //484
            this.f47 = stream.ReadValueF32(); //488
            this.f48 = stream.ReadValueF32(); //492
            this.f49 = stream.ReadValueF32(); //496
            stream.Position += 4;
            this.f50 = stream.ReadValueF32(); //504
            stream.Position += 12;
            this.f51 = stream.ReadValueF32(); //520
            stream.Position += 32;
        }
    }

    public class HeroTable : ISerializableData
    {
        //Total Length: 868
        public string Name;
        public int snoMaleActor;
        public int snoFemaleActor;
        public int snoInventory;
        public int i0;
        public int snoStartingLMBSkill;
        public int snoStartingRMBSkill;
        public int snoSKillKit0;
        public int snoSKillKit1;
        public int snoSKillKit2;
        public int snoSKillKit3;
        public Resource PrimaryResource;
        public Resource SecondaryResource;
        public float f0;
        public int i1;
        public float f1;
        public float f2;
        public float f3;
        public float f4;
        public float f5;
        public float f6;
        public float f7;
        public float f8;
        public float f9;
        public float f10;
        public float f11;
        public float f12;
        public float f13;
        public float f14;
        public float f15;
        public float f16;
        public float f17;
        public float f18;
        public float f19;
        public float f20;
        public float f21;
        public float f22;
        public float f23;
        public float f24;
        public float f25;
        public float f26;
        public float f27;
        public float f28;
        public float f29;
        public float f30;
        public float f31;
        public float f32;
        public float f33;
        public float f34;
        public float f35;
        public float f36;
        public float f37;
        public float f38;
        public float f39;
        public float f40;
        public float f41;
        public float f42;

        public void Read(MpqFileStream stream)
        {
            stream.Position += 4;
            this.Name = stream.ReadString(256,true);
            this.snoMaleActor = stream.ReadValueS32();
            this.snoFemaleActor = stream.ReadValueS32();
            this.snoInventory = stream.ReadValueS32();
            this.i0 = stream.ReadValueS32();
            this.snoStartingLMBSkill = stream.ReadValueS32();
            this.snoStartingRMBSkill = stream.ReadValueS32();
            this.snoSKillKit0 = stream.ReadValueS32();
            this.snoSKillKit1 = stream.ReadValueS32();
            this.snoSKillKit2 = stream.ReadValueS32();
            this.snoSKillKit3 = stream.ReadValueS32();
            this.PrimaryResource = (Resource)stream.ReadValueS32();
            this.SecondaryResource = (Resource)stream.ReadValueS32();
            this.f0 = stream.ReadValueF32(); //308
            this.i1 = stream.ReadValueS32(); //312
            stream.Position += 16;
            this.f1 = stream.ReadValueF32(); //332
            this.f2 = stream.ReadValueF32(); //336
            stream.Position += 8;
            this.f3 = stream.ReadValueF32(); //348
            this.f4 = stream.ReadValueF32(); //352
            this.f5 = stream.ReadValueF32(); //356
            this.f6 = stream.ReadValueF32(); //360
            stream.Position += 4;
            this.f7 = stream.ReadValueF32(); //368
            this.f8 = stream.ReadValueF32(); //372
            this.f9 = stream.ReadValueF32(); //376
            stream.Position += 24;
            this.f10 = stream.ReadValueF32(); //404
            stream.Position += 72;
            this.f11 = stream.ReadValueF32(); //480
            this.f12 = stream.ReadValueF32(); //484
            this.f13 = stream.ReadValueF32(); //488
            stream.Position += 4;
            this.f14 = stream.ReadValueF32(); //496
            stream.Position += 32;
            this.f15 = stream.ReadValueF32(); //532
            this.f16 = stream.ReadValueF32(); //536
            stream.Position += 4;
            this.f17 = stream.ReadValueF32(); //544
            stream.Position += 32;
            this.f18 = stream.ReadValueF32(); //580
            this.f19 = stream.ReadValueF32(); //584
            this.f20 = stream.ReadValueF32(); //588
            this.f21 = stream.ReadValueF32(); //592
            this.f22 = stream.ReadValueF32(); //596
            this.f23 = stream.ReadValueF32(); //600
            stream.Position += 4;
            this.f24 = stream.ReadValueF32(); //608
            this.f25 = stream.ReadValueF32(); //612
            stream.Position += 60;
            this.f26 = stream.ReadValueF32(); //676
            stream.Position += 8;
            this.f27 = stream.ReadValueF32(); //688
            this.f28 = stream.ReadValueF32(); //692
            this.f29 = stream.ReadValueF32(); //696
            this.f30 = stream.ReadValueF32(); //700
            stream.Position += 12;
            this.f31 = stream.ReadValueF32(); //716
            this.f32 = stream.ReadValueF32(); //720
            this.f33 = stream.ReadValueF32(); //724
            stream.Position += 40;
            this.f34 = stream.ReadValueF32(); //768
            stream.Position += 24;
            this.f35 = stream.ReadValueF32(); //796
            this.f36 = stream.ReadValueF32(); //800
            this.f37 = stream.ReadValueF32(); //804
            this.f38 = stream.ReadValueF32(); //808
            stream.Position += 40;
            this.f39 = stream.ReadValueF32(); //852
            this.f40 = stream.ReadValueF32(); //856
            this.f41 = stream.ReadValueF32(); //860
            this.f42 = stream.ReadValueF32(); //864
        }

        public enum Resource : int
        {
            Mana = 0,
            Arcanum,
            Fury,
            Spirit,
            Power,
            Hatred,
            Discipline,
        }
    }

    public class MovementStyleTable : ISerializableData //0 byte file
    {
        //Total Length: 384
        public string Name; //0
        public int i0; //256
        public int i1;
        public int i2;
        public int i3;
        public int i4;
        public int i5;
        public int i6;
        public int i7;
        public float f0;
        public float f1;
        public float f2;
        public float f3;
        public float f4;
        public float f5;
        public float f6;
        public float f7;
        public float f8;
        public float f9;
        public float f10;
        public float f11;
        public float f12;
        public float f13;
        public float f14;
        public float f15;
        public float f16;
        public float f17;
        public float f18;
        public float f19;
        public float f20;
        public float f21;
        public int snoPowerToBreakObjects;

        public void Read(MpqFileStream stream)
        {
            stream.Position += 4;
            this.Name = stream.ReadString(256,true);
            this.i0 = stream.ReadValueS32();
            this.i1 = stream.ReadValueS32();
            this.i2 = stream.ReadValueS32();
            this.i3 = stream.ReadValueS32();
            this.i4 = stream.ReadValueS32();
            this.i5 = stream.ReadValueS32();
            this.i6 = stream.ReadValueS32();
            this.i7 = stream.ReadValueS32();
            this.f0 = stream.ReadValueF32();
            this.f1 = stream.ReadValueF32();
            this.f2 = stream.ReadValueF32();
            this.f3 = stream.ReadValueF32();
            this.f4 = stream.ReadValueF32();
            this.f5 = stream.ReadValueF32();
            this.f6 = stream.ReadValueF32();
            this.f7 = stream.ReadValueF32();
            this.f8 = stream.ReadValueF32();
            this.f9 = stream.ReadValueF32();
            this.f10 = stream.ReadValueF32();
            this.f11 = stream.ReadValueF32();
            this.f12 = stream.ReadValueF32();
            this.f13 = stream.ReadValueF32();
            this.f14 = stream.ReadValueF32();
            this.f15 = stream.ReadValueF32();
            this.f16 = stream.ReadValueF32();
            this.f17 = stream.ReadValueF32();
            this.f18 = stream.ReadValueF32();
            this.f19 = stream.ReadValueF32();
            this.f20 = stream.ReadValueF32();
            this.f21 = stream.ReadValueF32();
            this.snoPowerToBreakObjects = stream.ReadValueS32();
        }
    }

    public class LabelGBIDTable : ISerializableData
    {
        //Total Length: 264
        public string Name;
        public int i0;

        public void Read(MpqFileStream stream)
        {
            stream.Position += 4;
            this.Name = stream.ReadString(256,true);
            this.i0 = stream.ReadValueS32();
        }
    }

    public class AffixTable : ISerializableData
    {
        //Total Length: 544
        public string Name;
        public int i0;
        public int AffixLevel;
        public int SupMask;
        public int i3;
        public int i4;
        public int i5;
        public AffixType1 affixType1;
        public int i6;
        public int snoRareNamePrefixStringList;
        public int snoRareNameSuffixStringList;
        public int AffixFamily0;
        public int AffixFamily1;
        public int ExclusionCategory;
        public int[] i7; //len 6
        public int[] i8; //len 6
        public int i9;
        public AffixType2 affixType2;
        public int AssociatedAffix;
        public AttributeSpecifier[] attributeSpecifier; //len 4

        public void Read(MpqFileStream stream)
        {
            stream.Position += 4;
            this.Name = stream.ReadString(256,true);
            this.i0 = stream.ReadValueS32(); //260
            this.AffixLevel = stream.ReadValueS32(); //264
            this.SupMask = stream.ReadValueS32(); //268
            this.i3 = stream.ReadValueS32(); //272
            this.i4 = stream.ReadValueS32(); //276
            this.i5 = stream.ReadValueS32(); //280
            this.affixType1 = (AffixType1)stream.ReadValueS32(); //284
            this.i6 = stream.ReadValueS32(); //288
            this.snoRareNamePrefixStringList = stream.ReadValueS32(); //292
            this.snoRareNameSuffixStringList = stream.ReadValueS32(); //296
            this.AffixFamily0 = stream.ReadValueS32(); //300
            this.AffixFamily1 = stream.ReadValueS32(); //304
            this.ExclusionCategory = stream.ReadValueS32(); //308
            this.i7 = new int[6]; //312
            for (int i = 0; i < 6; i++)
                this.i7[i] = stream.ReadValueS32();
            this.i8 = new int[6]; //336
            for (int i = 0; i < 6; i++)
                this.i8[i] = stream.ReadValueS32();
            this.i9 = stream.ReadValueS32(); //360
            this.affixType2 = (AffixType2)stream.ReadValueS32(); //364
            this.AssociatedAffix = stream.ReadValueS32(); //368
            stream.Position += 4;
            this.attributeSpecifier = new AttributeSpecifier[4]; //376
            for (int i = 0; i < 4; i++)
                this.attributeSpecifier[i] = new AttributeSpecifier(stream);
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

    public class AttributeSpecifier
    {
        //Length: 24
        public int AttributeId;
        public int snoParam;
        public List<int> formula;

        public AttributeSpecifier(MpqFileStream stream)
        {
            this.AttributeId = stream.ReadValueS32();
            this.snoParam = stream.ReadValueS32();
            stream.Position += 8;
            this.formula = stream.ReadSerializedInts();
        }
    }

    public class LootDistTable : ISerializableData //0 byte file
    {
        //Total Length: 92
        public int i0;
        public int i1;
        public int i2;
        public int i3;
        public int i4;
        public int i5;
        public int i6;
        public int i7;
        public int i8;
        public int i9;
        public float f0;
        public float f1;
        public float f2;
        public float f3;
        public float f4;
        public float f5;
        public float f6;
        public float f7;
        public float f8;
        public float f9;
        public float f10;
        public int i10;
        public int i11;

        public void Read(MpqFileStream stream)
        {
            this.i0 = stream.ReadValueS32();
            this.i1 = stream.ReadValueS32();
            this.i2 = stream.ReadValueS32();
            this.i3 = stream.ReadValueS32();
            this.i4 = stream.ReadValueS32();
            this.i5 = stream.ReadValueS32();
            this.i6 = stream.ReadValueS32();
            this.i7 = stream.ReadValueS32();
            this.i8 = stream.ReadValueS32();
            this.i9 = stream.ReadValueS32();
            this.f0 = stream.ReadValueF32();
            this.f1 = stream.ReadValueF32();
            this.f2 = stream.ReadValueF32();
            this.f3 = stream.ReadValueF32();
            this.f4 = stream.ReadValueF32();
            this.f5 = stream.ReadValueF32();
            this.f6 = stream.ReadValueF32();
            this.f7 = stream.ReadValueF32();
            this.f8 = stream.ReadValueF32();
            this.f9 = stream.ReadValueF32();
            this.f10 = stream.ReadValueF32();
            this.i10 = stream.ReadValueS32();
            this.i11 = stream.ReadValueS32();
        }
    }

    public class RareItemNamesTable : ISerializableData
    {
        //Total Length: 272
        public string Name;
        public BalanceType type;
        public int RelatedAffixOrItemType;
        public AffixType2 affixType;

        public void Read(MpqFileStream stream)
        {
            stream.Position += 4;
            this.Name = stream.ReadString(256,true);
            this.type = (BalanceType)stream.ReadValueS32();
            this.RelatedAffixOrItemType = stream.ReadValueS32();
            this.affixType = (AffixType2)stream.ReadValueS32();
        }
    }

    public class MonsterAffixesTable : ISerializableData
    {
        //Total Length: 792
        public string Name;
        public int i0; //260
        public int i1; //264
        public int i2; //268
        public MonsterAffix monsterAffix;
        public Resistance resistance;
        public AffixType2 affixType;
        public int i3;
        public int i4;
        public int i5; //292
        public AttributeSpecifier[] Attributes; //Len 10
        public AttributeSpecifier[] MinionAttributes; //Len 10
        public int snoOn_Spawn_Power_Minion; //780
        public int snoOn_Spawn_Power_Champion; //784
        public int snoOn_Spawn_Power_Rare; //788

        public void Read(MpqFileStream stream)
        {
            stream.Position += 4;
            this.Name = stream.ReadString(256,true);
            this.i0 = stream.ReadValueS32();
            this.i1 = stream.ReadValueS32();
            this.i2 = stream.ReadValueS32();
            this.monsterAffix = (MonsterAffix)stream.ReadValueS32();
            this.resistance = (Resistance)stream.ReadValueS32();
            this.affixType = (AffixType2)stream.ReadValueS32();
            this.i3 = stream.ReadValueS32();
            this.i4 = stream.ReadValueS32();
            this.i5 = stream.ReadValueS32();
            this.Attributes = new AttributeSpecifier[10];
            for (int i = 0; i < 10; i++)
                this.Attributes[i] = new AttributeSpecifier(stream);
            this.MinionAttributes = new AttributeSpecifier[10];
            for (int i = 0; i < 10; i++)
                this.MinionAttributes[i] = new AttributeSpecifier(stream);
            stream.Position += 4;
            this.snoOn_Spawn_Power_Minion = stream.ReadValueS32();
            this.snoOn_Spawn_Power_Champion = stream.ReadValueS32();
            this.snoOn_Spawn_Power_Rare = stream.ReadValueS32();
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
        public string Name;
        public AffixType2 affixType;
        public string s0;

        public void Read(MpqFileStream stream)
        {
            stream.Position += 4;
            this.Name = stream.ReadString(256,true);
            this.affixType = (AffixType2)stream.ReadValueS32();
            this.s0 = stream.ReadString(128,true);
        }

    }

    public class SocketedEffectTable : ISerializableData
    {
        //Total Length: 1416
        public string Name;
        public int Item;
        public int ItemType;
        public AttributeSpecifier[] Attribute; //Len 3
        public AttributeSpecifier[] ReqAttribute; //Len 2
        public string s0;

        public void Read(MpqFileStream stream)
        {
            stream.Position += 4;
            this.Name = stream.ReadString(256,true);
            this.Item = stream.ReadValueS32(); //260
            this.ItemType = stream.ReadValueS32(); //264
            stream.Position += 4;
            this.Attribute = new AttributeSpecifier[3];
            for (int i = 0; i < 3; i++)
                this.Attribute[i] = new AttributeSpecifier(stream);
            this.ReqAttribute = new AttributeSpecifier[2];
            for (int i = 0; i < 2; i++)
                this.ReqAttribute[i] = new AttributeSpecifier(stream);
            this.s0 = stream.ReadString(1024,true);
        }
    }

    public class ItemEnhancementTable : ISerializableData
    {
        //Total Length: 696
        public string Name;
        public int i0;
        public int i1;
        public int i2;
        public int i3;
        public AttributeSpecifier[] Attribute; //len16
        public int i4;
        public RecipeIngredient[] Ingredients; //len 3


        public void Read(MpqFileStream stream)
        {
            stream.Position += 4;
            this.Name = stream.ReadString(256,true);
            this.i0 = stream.ReadValueS32(); //260
            this.i1 = stream.ReadValueS32(); //264
            this.i2 = stream.ReadValueS32(); //268
            this.i3 = stream.ReadValueS32(); //272
            stream.Position += 4;
            this.Attribute = new AttributeSpecifier[16]; //280
            for (int i = 0; i < 16; i++)
                this.Attribute[i] = new AttributeSpecifier(stream);
            this.i4 = stream.ReadValueS32(); //664
            this.Ingredients = new RecipeIngredient[3]; //668
            for (int i = 0; i < 3; i++)
                this.Ingredients[i] = new RecipeIngredient(stream);
            stream.Position += 4;
        }
    }

    public class RecipeIngredient
    {
        public int gbidItem;
        public int i0;

        public RecipeIngredient(MpqFileStream stream)
        {
            this.gbidItem = stream.ReadValueS32();
            this.i0 = stream.ReadValueS32();
        }
    }

    public class ItemDropTable : ISerializableData //0 byte file
    {
        //Total Length: 1140
        public string Name;
        public int[] i0;


        public void Read(MpqFileStream stream)
        {
            stream.Position += 4;
            this.Name = stream.ReadString(256,true);
            this.i0 = new int[219];
            for (int i = 0; i < 219; i++)
                this.i0[i] = stream.ReadValueS32();
        }

    }

    public class ItemLevelModTable : ISerializableData //0 byte file
    {
        //Total Length: 92
        public int i0;
        public float f0;
        public float f1;
        public float f2;
        public float f3;
        public float f4;
        public float f5;
        public float f6;
        public float f7;
        public float f8;
        public float f9;
        public float f10;
        public int i1;
        public int i2;
        public int i3;
        public int i4;
        public int i5;
        public int i6;
        public int i7;
        public int i8;
        public int i9;
        public int i10;
        public int i11;

        public void Read(MpqFileStream stream)
        {
            this.i0 = stream.ReadValueS32();
            this.f0 = stream.ReadValueF32();
            this.f1 = stream.ReadValueF32();
            this.f2 = stream.ReadValueF32();
            this.f3 = stream.ReadValueF32();
            this.f4 = stream.ReadValueF32();
            this.f5 = stream.ReadValueF32();
            this.f6 = stream.ReadValueF32();
            this.f7 = stream.ReadValueF32();
            this.f8 = stream.ReadValueF32();
            this.f9 = stream.ReadValueF32();
            this.f10 = stream.ReadValueF32();
            this.i1 = stream.ReadValueS32();
            this.i2 = stream.ReadValueS32();
            this.i3 = stream.ReadValueS32();
            this.i4 = stream.ReadValueS32();
            this.i5 = stream.ReadValueS32();
            this.i6 = stream.ReadValueS32();
            this.i7 = stream.ReadValueS32();
            this.i8 = stream.ReadValueS32();
            this.i9 = stream.ReadValueS32();
            this.i10 = stream.ReadValueS32();
            this.i11 = stream.ReadValueS32();
        }
    }

    public class QualityClassTable : ISerializableData //0 byte file
    {
        //Total Length: 352
        public string Name;
        public float[] f0;

        public void Read(MpqFileStream stream)
        {
            stream.Position += 4;
            this.Name = stream.ReadString(256,true);
            this.f0 = new float[22];
            for (int i = 0; i < 22; i++)
                this.f0[i] = stream.ReadValueF32();
        }

    }

    public class HirelingTable : ISerializableData
    {
        //Total Length: 824
        public string Name;
        public int snoActor;
        public int snoProxy;
        public int snoInventory;
        public float f0;
        public float f1;
        public float f2;
        public float f3;
        public float f4;
        public float f5;
        public float f6;

        public void Read(MpqFileStream stream)
        {
            stream.Position += 4;
            this.Name = stream.ReadString(256,true);
            this.snoActor = stream.ReadValueS32();
            this.snoProxy = stream.ReadValueS32();
            this.snoInventory = stream.ReadValueS32();
            stream.Position += 480;
            this.f0 = stream.ReadValueF32(); //752
            this.f1 = stream.ReadValueF32(); //756
            this.f2 = stream.ReadValueF32(); //760
            this.f3 = stream.ReadValueF32(); //764
            stream.Position += 8;
            this.f4 = stream.ReadValueF32(); //776
            this.f5 = stream.ReadValueF32(); //780
            this.f6 = stream.ReadValueF32(); //784
            stream.Position += 36;
        }

    }

    public class SetItemBonusTable : ISerializableData
    {
        //Total Length: 464
        public string Name;
        public int i0;
        public int Set;
        public AttributeSpecifier[] Attribute;

        public void Read(MpqFileStream stream)
        {
            stream.Position += 4;
            this.Name = stream.ReadString(256,true);
            this.Set = stream.ReadValueS32();
            this.i0 = stream.ReadValueS32();
            stream.Position += 4;
            Attribute = new AttributeSpecifier[8];
            for (int i = 0; i < 8; i++)
                Attribute[i] = new AttributeSpecifier(stream);
        }

    }

    public class EliteModTable : ISerializableData //0 byte file
    {
        //Total Length: 344
        public string Name;
        public float f0;
        public int Time0;
        public float f1;
        public int Time1;
        public float f2;
        public int Time2;
        public float f3;
        public int Time3;
        public float f4;
        public int Time4;
        public float f5;
        public int Time5;
        public float f6;
        public int Time6;
        public float f7;
        public float f8;
        public int Time7;
        public float f9;
        public float f10;
        public float f11;
        public float f12;

        public void Read(MpqFileStream stream)
        {
            stream.Position += 4;
            this.Name = stream.ReadString(256,true);
            this.f0 = stream.ReadValueF32();
            this.Time0 = stream.ReadValueS32();
            this.f1 = stream.ReadValueF32();
            this.Time1 = stream.ReadValueS32();
            this.f2 = stream.ReadValueF32();
            this.Time2 = stream.ReadValueS32();
            this.f3 = stream.ReadValueF32();
            this.Time3 = stream.ReadValueS32();
            this.f4 = stream.ReadValueF32();
            this.Time4 = stream.ReadValueS32();
            this.f5 = stream.ReadValueF32();
            this.Time5 = stream.ReadValueS32();
            this.f6 = stream.ReadValueF32();
            this.Time6 = stream.ReadValueS32();
            this.f7 = stream.ReadValueF32();
            this.f8 = stream.ReadValueF32();
            this.Time7 = stream.ReadValueS32();
            this.f9 = stream.ReadValueF32();
            this.f10 = stream.ReadValueF32();
            this.f11 = stream.ReadValueF32();
            this.f12 = stream.ReadValueF32();
        }

    }

    public class ItemTierTable : ISerializableData //0 byte file
    {
        //Total Length: 32
        public int Head;
        public int Torso;
        public int Feet;
        public int Hands;
        public int Shoulders;
        public int Bracers;
        public int Belt;

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
        public string s0;
        public float[] f0;

        public void Read(MpqFileStream stream)
        {
            this.s0 = stream.ReadString(1024,true);
            this.f0 = new float[61];
            for (int i = 0; i < 61; i++)
                this.f0[i] = stream.ReadValueF32();
        }
    }

    public class RecipeTable : ISerializableData
    {
        //Total Length: 332
        public string Name;
        public int snoRecipe;
        public RecipeType Type;
        public int i0;
        public int i1;
        public int i2;
        public int i3;
        public RecipeIngredient[] Ingredients;

        public void Read(MpqFileStream stream)
        {
            stream.Position += 4;
            this.Name = stream.ReadString(256,true);
            this.snoRecipe = stream.ReadValueS32();
            this.Type = (RecipeType)stream.ReadValueS32();
            this.i0 = stream.ReadValueS32();
            this.i1 = stream.ReadValueS32();
            this.i2 = stream.ReadValueS32();
            this.i3 = stream.ReadValueS32();
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
        public string Name;

        public void Read(MpqFileStream stream)
        {
            stream.Position += 4;
            this.Name = stream.ReadString(256,true);
        }
    }

}
