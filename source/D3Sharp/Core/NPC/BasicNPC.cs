using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using D3Sharp.Net.Game;
using D3Sharp.Net.Game.Messages;
using D3Sharp.Net.Game.Messages.Misc;
using D3Sharp.Core.Helpers;

namespace D3Sharp.Core.NPC
{
    public class BasicNPC
    {
        public int ID;
        float HP;
        float MaxHP;

        GameClient Client;

        public void Die(int anim)
        {
            var killAni = new int[]{
                    0x2cd7,
                    0x2cd4,
                    0x01b378,
                    0x2cdc,
                    0x02f2,
                    0x2ccf,
                    0x2cd0,
                    0x2cd1,
                    0x2cd2,
                    0x2cd3,
                    0x2cd5,
                    0x01b144,
                    0x2cd6,
                    0x2cd8,
                    0x2cda,
                    0x2cd9
            };
            //Game.SendMessage(new PlayEffectMessage()
            //{
            //    Id = 0x7a,
            //    Field0 = ID,
            //    Field1 = 0x0,
            //    Field2 = 0x2,
            //});
            //Game.SendMessage(new PlayEffectMessage()
            //{
            //    Id = 0x7a,
            //    Field0 = ID,
            //    Field1 = 0xc,
            //});
            //Game.SendMessage(new PlayHitEffectMessage()
            //{
            //    Id = 0x7b,
            //    Field0 = ID,
            //    Field1 = 0x789E00E2,
            //    Field2 = 0x2,
            //    Field3 = false,
            //});

            //Game.SendMessage(new FloatingNumberMessage()
            //{
            //    Id = 0xd0,
            //    Field0 = ID,
            //    Field1 = 9001.0f,
            //    Field2 = 0,
            //});

            //Game.SendMessage(new ANNDataMessage()
            //{
            //    Id = 0x6d,
            //    Field0 = ID,
            //});

            //int ani = killAni[anim];

            //Game.SendMessage(new PlayAnimationMessage()
            //{
            //    Id = 0x6c,
            //    Field0 = ID,
            //    Field1 = 0xb,
            //    Field2 = 0,
            //    tAnim = new PlayAnimationMessageSpec[1]
            //    {
            //        new PlayAnimationMessageSpec()
            //        {
            //            Field0 = 0x2,
            //            Field1 = ani,
            //            Field2 = 0x0,
            //            Field3 = 1f
            //        }
            //    }
            //});

            //Game.packetId += 10 * 2;
            //Game.SendMessage(new DWordDataMessage()
            //{
            //    Id = 0x89,
            //    Field0 = Game.packetId,
            //});

            //Game.SendMessage(new ANNDataMessage()
            //{
            //    Id = 0xc5,
            //    Field0 = ID,
            //});

            //Game.SendMessage(new AttributeSetValueMessage
            //{
            //    Id = 0x4c,
            //    Field0 = ID,
            //    Field1 = new NetAttributeKeyValue
            //    {
            //        Attribute = GameAttribute.Attributes[0x4d],
            //        Float = 0
            //    }
            //});

            //Game.SendMessage(new AttributeSetValueMessage
            //{
            //    Id = 0x4c,
            //    Field0 = ID,
            //    Field1 = new NetAttributeKeyValue
            //    {
            //        Attribute = GameAttribute.Attributes[0x1c2],
            //        Int = 1
            //    }
            //});

            //Game.SendMessage(new AttributeSetValueMessage
            //{
            //    Id = 0x4c,
            //    Field0 = ID,
            //    Field1 = new NetAttributeKeyValue
            //    {
            //        Attribute = GameAttribute.Attributes[0x1c5],
            //        Int = 1
            //    }
            //});
            //Game.SendMessage(new PlayEffectMessage()
            //{
            //    Id = 0x7a,
            //    Field0 = ID,
            //    Field1 = 0xc,
            //});
            //Game.SendMessage(new PlayEffectMessage()
            //{
            //    Id = 0x7a,
            //    Field0 = ID,
            //    Field1 = 0x37,
            //});
            //Game.SendMessage(new PlayHitEffectMessage()
            //{
            //    Id = 0x7b,
            //    Field0 = ID,
            //    Field1 = 0x789E00E2,
            //    Field2 = 0x2,
            //    Field3 = false,
            //});

            //Game.packetId += 10 * 2;
            //Game.SendMessage(new DWordDataMessage()
            //{
            //    Id = 0x89,
            //    Field0 = Game.packetId,
            //});

            //Game.tick += 20;
            //Game.SendMessage(new EndOfTickMessage()
            //{
            //    Id = 0x008D,
            //    Field0 = Game.tick - 20,
            //    Field1 = Game.tick
            //});

            //Game.FlushOutgoingBuffer();

        }

        public BasicNPC(int objectId, ref GameClient g)
        {
            ID = objectId;
            Client = g;
            //Game.SendMessage(new AffixMessage()
            //{
            //    Id = 0x48,
            //    Field0 = objectId,
            //    Field1 = 0x1,
            //    aAffixGBIDs = new int[0]
            //});
            //Game.SendMessage(new AffixMessage()
            //{
            //    Id = 0x48,
            //    Field0 = objectId,
            //    Field1 = 0x2,
            //    aAffixGBIDs = new int[0]
            //});
            //Game.SendMessage(new ACDCollFlagsMessage
            //{
            //    Id = 0xa6,
            //    Field0 = objectId,
            //    Field1 = 0x1
            //});

            Client.SendMessage(new AttributesSetValuesMessage
            {
                Id = 0x4d,
                Field0 = objectId,
                atKeyVals = new NetAttributeKeyValue[15] {
                    new NetAttributeKeyValue {
                        Attribute = GameAttribute.Attributes[214],
                        Int = 0
                    },
                    new NetAttributeKeyValue {
                        Attribute = GameAttribute.Attributes[464],
                        Int = 1
                    },
                    new NetAttributeKeyValue {
                        Field0 = 1048575,
                        Attribute = GameAttribute.Attributes[441],
                        Int = 1
                    },
                    new NetAttributeKeyValue {
                        Field0 = 30582,
                        Attribute = GameAttribute.Attributes[560],
                        Int = 1
                    },
                    new NetAttributeKeyValue {
                        Field0 = 30286,
                        Attribute = GameAttribute.Attributes[560],
                        Int = 1
                    },
                    new NetAttributeKeyValue {
                        Field0 = 30285,
                        Attribute = GameAttribute.Attributes[560],
                        Int = 1
                    },
                    new NetAttributeKeyValue {
                        Field0 = 30284,
                        Attribute = GameAttribute.Attributes[560],
                        Int = 1
                    },
                    new NetAttributeKeyValue {
                        Field0 = 30283,
                        Attribute = GameAttribute.Attributes[560],
                        Int = 1
                    },
                    new NetAttributeKeyValue {
                        Field0 = 30290,
                        Attribute = GameAttribute.Attributes[560],
                        Int = 1
                    },
                    new NetAttributeKeyValue {
                        Field0 = 79486,
                        Attribute = GameAttribute.Attributes[560],
                        Int = 1
                    },
                    new NetAttributeKeyValue {
                        Field0 = 30286,
                        Attribute = GameAttribute.Attributes[460],
                        Int = 1
                    },
                    new NetAttributeKeyValue {
                        Field0 = 30285,
                        Attribute = GameAttribute.Attributes[460],
                        Int = 1
                    },
                    new NetAttributeKeyValue {
                        Field0 = 30284,
                        Attribute = GameAttribute.Attributes[460],
                        Int = 1
                    },
                    new NetAttributeKeyValue {
                        Field0 = 30283,
                        Attribute = GameAttribute.Attributes[460],
                        Int = 1
                    },
                    new NetAttributeKeyValue {
                        Field0 = 30290,
                        Attribute = GameAttribute.Attributes[460],
                        Int = 1
                    }
                }

            });

            Client.SendMessage(new AttributesSetValuesMessage
            {
                Id = 0x4d,
                Field0 = objectId,
                atKeyVals = new NetAttributeKeyValue[9] {
                    new NetAttributeKeyValue {
                        Attribute = GameAttribute.Attributes[86],
                        Float = 4.546875f
                    },
                    new NetAttributeKeyValue {
                        Field0 = 79486,
                        Attribute = GameAttribute.Attributes[460],
                        Int = 1
                    },
                    new NetAttributeKeyValue {
                        Attribute = GameAttribute.Attributes[84],
                        Float = 4.546875f
                    },
                    new NetAttributeKeyValue {
                        Attribute = GameAttribute.Attributes[81],
                        Int = 0
                    },
                    new NetAttributeKeyValue {
                        Attribute = GameAttribute.Attributes[77],
                        Float = 4.546875f
                    },
                    new NetAttributeKeyValue {
                        Attribute = GameAttribute.Attributes[69],
                        Int = 1
                    },
                    new NetAttributeKeyValue {
                        Field0 = 30582,
                        Attribute = GameAttribute.Attributes[460],
                        Int = 1
                    },
                    new NetAttributeKeyValue {
                        Attribute = GameAttribute.Attributes[67],
                        Int = 10
                    },
                    new NetAttributeKeyValue {
                        Attribute = GameAttribute.Attributes[38],
                        Int = 1
                    }
                }

            });


            //Game.SendMessage(new ACDGroupMessage
            //{
            //    Id = 0xb8,
            //    Field0 = objectId,
            //    Field1 = unchecked((int)0xb59b8de4),
            //    Field2 = unchecked((int)0xffffffff)
            //});

            //Game.SendMessage(new ANNDataMessage
            //{
            //    Id = 0x3e,
            //    Field0 = objectId
            //});

            //Game.SendMessage(new ACDTranslateFacingMessage
            //{
            //    Id = 0x70,
            //    Field0 = objectId,
            //    Field1 = (float)(RandomHelper.NextDouble() * 2.0 * Math.PI),
            //    Field2 = false
            //});

            //Game.SendMessage(new SetIdleAnimationMessage
            //{
            //    Id = 0xa5,
            //    Field0 = objectId,
            //    Field1 = 0x11150
            //});

            //Game.SendMessage(new SNONameDataMessage
            //{
            //    Id = 0xd3,
            //    Field0 = new SNOName
            //    {
            //        Field0 = 0x1,
            //        Field1 = 6652
            //    }
            //});

            //Game.packetId += 30 * 2;
            //Game.SendMessage(new DWordDataMessage()
            //{
            //    Id = 0x89,
            //    Field0 = Game.packetId,
            //});

            //Game.tick += 20;
            //Game.SendMessage(new EndOfTickMessage()
            //{
            //    Id = 0x008D,
            //    Field0 = Game.tick - 20,
            //    Field1 = Game.tick
            //});

        }

        public enum NPCList
        {
            A1C2R1_AdventurerLeader = 3098, //Desperate Villager
            A1C4CultistMelee = 51281, //Dark Ritualist
            A1C4VillagerScreamer = 5246, //NONE
            A1C4ZombieSkinny = 51287, //Risen
            A1C5RFarmer = 51288, //Terrified Farmer
            A1_GenericVendor_Tinker = 81610, //Wandering Tinker
            A1_UniqueVendor_Armorer = 81609, //Vendel the Armorsmith
            A1_UniqueVendor_Collector = 107535, //Arghus the Collector
            A1_UniqueVendor_Collector_InTown_01 = 107535, //Arghus the Collector
            A1_UniqueVendor_Collector_InTown_02 = 107535, //Arghus the Collector
            A1_UniqueVendor_Collector_InTown_03 = 107535, //Arghus the Collector
            A1_UniqueVendor_Curios = 107419, //Adenah the Curio Vendor
            A1_UniqueVendor_Fence = 104569, //Radek the Fence
            A1_UniqueVendor_Fence_InTown = 104569, //Radek the Fence
            A1_UniqueVendor_Fence_InTown_01 = 104569, //Radek the Fence
            A1_UniqueVendor_Fence_InTown_02 = 104569, //Radek the Fence
            A1_UniqueVendor_Fence_InTown_03 = 104569, //Radek the Fence
            A1_UniqueVendor_Miner = 107076, //Tashun the Miner
            A1_UniqueVendor_Miner_InTown = 107076, //Tashun the Miner
            A1_UniqueVendor_Miner_InTown_01 = 107076, //Tashun the Miner
            A1_UniqueVendor_Miner_InTown_02 = 107076, //Tashun the Miner
            A1_UniqueVendor_Miner_InTown_03 = 107076, //Tashun the Miner
            A2C2AbdAlHazir = 4526, //Abd al-Hazir
            acidSlime = 120950, //Acid Slime
            Adria = 3095, //Adria
            Adventurer_C_TemplarIntro = 3098, //Death Initiate
            Adventurer_D_TemplarIntroUnique = 86624, //Jondar
            Axe_Bad_Data = 320, //Axe Bad Data
            Barbarian_CallOfTheAncients_1 = 90443, //Talic
            Barbarian_CallOfTheAncients_2 = 90535, //Korlic
            Barbarian_CallOfTheAncients_3 = 90536, //Mawdawc
            Beast_A = 3337, //Beast
            Beast_B = 3338, //Hulker
            BlacksmithWife = 98888, //Mira Eamon
            Blizzcon_Raven_Pecking_A = 5013, //Blizzcon_Raven_Pecking_A
            Blizzcon_Raven_Perched_A = 5014, //Blizzcon_Raven_Perched_A
            Cain = 3533, //Deckard Cain
            Cain_Intro = 102386, //Deckard Cain
            caldeumPoor_Male = 3582, //Refugee
            CaptainRumfoord = 3739, //Captain Rumford
            CircleOfProtection = 4701, //CircleOfProtection
            Companion = 131999, //Companion
            CorpseSpider = 6443, //Corpse Spider
            Corpulent_A = 3847, //Grotesque
            Corpulent_B = 3848, //Harvester
            CrownCultist_Leader = 6025, //Cult Leader
            CryingGhostMom = 3892, //Ghostly Woman
            CryptChild_A = 3893, //Imp
            CryptChild_A_FamilyTree_Son = 77090, //Little Jebby Rathe
            CryptChild_A_FastEvent = 81283, //Crazed Imp
            CryptChild_B = 3894, //Fiend
            Crypt_Endless_Spawner = 57736, //Endless Spawner
            DebugPlainDog = 186035, //DebugPlainDog
            demonFlyer_A = 62736, //Demonic Hellflyer
            DemonHunter_ShockSpike_Spike = 132732, //DemonHunter_ShockSpike_Spike
            DemonHunter_SpikeTrap_Proxy = 111330, //DemonHunter_SpikeTrap_Proxy
            Despina = 3947, //Despina
            DH_caltrops_inactive_proxyActor = 196030, //Caltrops - Inactive
            DH_caltrops_runeA_damage = 154811, //Caltrops - Rune A
            DH_caltrops_runeB_slower = 155734, //Caltrops - Rune B
            DH_caltrops_runeC_weakenMonsters = 155159, //Caltrops - Rune C
            DH_caltrops_runeD_reduceDiscipline = 155848, //Caltrops - Rune D
            DH_caltrops_runeE_empower = 155376, //Caltrops - Rune E
            DH_caltrops_unruned = 129784, //Caltrops
            DH_Companion = 133741, //Bitey
            DH_Companion_RuneD = 159102, //DH_Companion_RuneD
            DH_Companion_RuneE = 159144, //DH_Companion_RuneE
            DH_rainofArrows_shadowBeast = 149949, //Atramental Creation
            DH_sentry = 141402, //DH_sentry
            Disintegrate_Target = 52687, //Disintegrate_Target
            Enchantress = 4062, //Enchantress
            EntanglingRoots = 362, //Entangling Roots
            Event_Highlands_VendorRescue_Vendor = 129782, //Rike the Apothecary
            FallenGrunt_A = 4080, //Fallen
            FamilyTree_Daughter = 76907, //Willa Rathe
            Farmer_Event_Roadside_Assistance = 51288, //Hobbled Farmer
            Ferryman = 153019, //Ferryman
            Fetish_Melee_A = 81283, //Voodoo Soldier
            Fetish_Ranged_A = 5346, //Voodoo Shooter
            Fetish_Shaman_A = 90320, //Voodoo Caster
            FleshPitFlyerSpawner_A = 4152, //Carrion Nest
            FleshPitFlyerSpawner_B = 4153, //Plague Nest
            FleshPitFlyerSpawner_B_Event_FarmAmbush = 4153, //Plague Nest
            FleshPitFlyerSpawner_C = 4153, //Cursed Nest
            FleshPitFlyer_A = 4156, //Carrion Bat
            FleshPitFlyer_B = 4157, //Plague Carrier
            FleshPitFlyer_B_Event_Ambusher = 81954, //Plague Carrier
            FleshPitFlyer_C = 368, //Winged Molok
            FleshPitFlyer_E = 195747, //Weirdling
            Generic_Proxy = 4176, //Generic_Proxy
            Gharbad_The_Weak_Ghost = 81068, //Gharbad's Ghost
            GhostKnight1 = 4181, //Alaric
            GhostKnight1_Festering = 4181, //Alaric
            GhostKnight1_FWAmbush = 4181, //Apparition
            GhostKnight2 = 4182, //Eldwin
            GhostKnight3 = 4183, //Lachdanan's Ghost
            Ghost_A = 370, //Enraged Phantom
            Ghost_A_NoRun = 136943, //Ghost
            Ghost_A_Unique_Chancellor = 156353, //Chancellor Eamon
            Ghost_A_Unique_House1000Undead = 85971, //Tormented Spirit
            Ghoul_A = 4201, //Ghoul
            Goatman_Moonclan_Melee_A = 4282, //Moon Clan Warrior
            Goatman_Moonclan_Melee_B = 4283, //Dark Moon Clan Warrior
            Goatman_Moonclan_Melee_B_Event_Ghost = 4283, //Goatman Ghost
            Goatman_Moonclan_Melee_C = 4284, //Blood Moon Clan Warrior
            Goatman_Moonclan_Ranged_A = 4286, //Moon Clan Impaler
            Goatman_Moonclan_Ranged_B = 4287, //Dark Moon Clan Impaler
            Goatman_Moonclan_Ranged_B_Event_Ghost = 4287, //Goatman Ghost
            Goatman_Moonclan_Shaman_A = 4290, //Moon Clan Shaman
            Goatman_Moonclan_Shaman_A_Event_Ghost = 4290, //Goatman Ghost
            Goatman_Moonclan_Shaman_A_Event_Graveyard = 81533, //Chupa Khazra
            Goatman_Moonclan_Shaman_B = 375, //Dark Moon Clan Shaman
            Goatman_Moonclan_Shaman_B_Unique = 76676, //Rambolt the Lunatic
            Goatman_Moonclan_Shaman_B_Unique_MysticWagon = 169533, //Nalghban the Foul
            GoatMutant_Melee_A = 4295, //Blood Clan Warrior
            GoatMutant_Melee_A_Unique_Gharbad = 81342, //Gharbad the Strong
            GrabbingHands = 4327, //Zombie Prisoner
            graveDigger_A = 4345, //Gravedigger
            graveDigger_B = 4340, //Ghastly Gravedigger
            graveRobber_A = 4372, //Brigand
            graveRobber_B = 4373, //Brigand
            graveRobber_C = 4376, //Brigand
            graveRobber_C_Nigel = 174013, //Nigel Cutthroat
            graveRobber_D = 4377, //Brigand
            graveRobber_ghost_A = 4372, //Ghostly Murderer
            HelperSoldier = 4480, //Soldier
            Hireling_Enchantress = 4482, //Enchantress
            Hireling_Scoundrel = 52694, //Scoundrel
            Hireling_Templar = 52693, //Templar
            Hub_CaravanLeader = 177544, //Caravan Leader
            Human_NPC_Male_Event_FarmAmbush = 51288, //Beleaguered Farmer
            IceRay_Target = 6535, //IceRay_Target
            Karyna = 56948, //Karyna
            Lamprey_A = 4564, //Corpse Worm
            Leah = 4580, //Leah
            LeoricGhost = 5365, //King Leoric's Ghost
            Monk_LethalDecoy = 98940, //Lethal Decoy
            Monk_LethalDecoy_RuneC = 142478, //Monk_LethalDecoy_RuneC
            Monk_LethalDecoy_RuneE = 147208, //Monk_LethalDecoy_RuneE
            Monk_mysticAlly_NoRune = 169904, //Mystic Ally
            Monk_mysticAlly_RuneA = 169906, //Mystic Ally
            Monk_mysticAlly_RuneB = 169907, //Mystic Ally
            Monk_mysticAlly_RuneC = 169909, //Mystic Ally
            Monk_mysticAlly_RuneD = 169908, //Mystic Ally
            Monk_mysticAlly_RuneE = 169905, //Mystic Ally
            OmniNPC_Female = 4800, //Villager
            OmniNPC_Male = 56948, // Villager
            OmniNPC_Male_Skeleton_A_Alaric = 142590, //Alaric
            OmniNPC_Tristram_Male_A = 84529, //Villager
            OmniNPC_Tristram_Male_A_Dying = 84529, //Dying Villager
            OmniNPC_Tristram_Male_A_NewTristram = 84529, //Traveling Scholar
            OmniNPC_Tristram_Male_B = 84531, //Villager
            OmniNPC_Tristram_Male_B_NoLook_2 = 142107, //Tired Patron
            OmniNPC_Tristram_Male_B_NoLook = 84531, //Villager
            OmniNPC_Tristram_Male_D = 84540, //Villager
            OmniNPC_Tristram_Male_E = 84542, //Villager
            OmniNPC_Tristram_Male_F = 84544, //Villager
            Power_Proxy_Seeker = 4889, //Power_Proxy_Seeker
            Priest_Male_B_NoLook = 141246, //Brother Malachi the Healer
            PT_Blacksmith = 56947, //Haedrig Eamon
            PT_Blacksmith_ForgeArmorShortcut = 56947, //Forge Armor
            PT_Blacksmith_ForgeWeaponShortcut = 56947, //Forge Weapons
            PT_Blacksmith_NonVendor = 56947, //Haedrig
            PT_Blacksmith_RepairShortcut = 56947, //Repair
            PT_Jeweler = 56949, //Covetous Shen
            PT_Jeweler_Apprentice = 56949, //Jeweler's Goon
            PT_Mystic = 56948, //Myriam
            QuillDemon_A = 4982, //Quill Fiend
            QuillDemon_A_Baby_Event = 128781, //Young Quill Fiend
            QuillDemon_A_LootHoarder = 187664, //Quill Fiend
            QuillDemon_A_Unique_LootHoarderLeader = 201878, //Sarkoth
            Scavenger_A = 5235, //Scavenger
            Scavenger_B = 5236, //Burrowing Leaper
            Scavenger_B_Armorer = 5236, //Armourer's Bane
            Scavenger_B_PuppyLove = 81738, //Leaper
            Scavenger_B_Unique_ScavengerFarm = 167205, //Burrow Bile
            Scoundrel = 4644, //Scoundrel
            ScoundrelNPC = 80812, //Scoundrel
            Scoundrel_Farmers_Daughter = 3582, //Sasha
            shadowVermin_A = 60049, //Shadow Vermin
            Shield_Skeleton_A = 5275, //Skeletal Shieldbearer
            Shield_Skeleton_A_NephChamp = 112134, //Ancient Hero
            Shield_Skeleton_B = 5276, //Returned Shieldman
            Shield_Skeleton_C = 5277, //Skeletal Sentry
            SkeletonArcher_A = 5346, //Skeletal Archer
            SkeletonArcher_B = 5347, //Returned Archer
            SkeletonKing = 5350, //Skeleton King
            SkeletonKing_Ghost = 5360, //Skeleton King
            SkeletonKing_Shield_Skeleton = 84919, //Forgotten Soldier
            SkeletonKing_Skeleton = 51340, //Forgotten Warrior
            SkeletonKing_SkeletonArcher = 2851, //Forgotten Archer
            SkeletonKing_Target_Proxy = 80140, //SkeletonKing_Target_Proxy
            SkeletonSummoner_A = 5387, //Tomb Guardian
            SkeletonSummoner_A_TemplarIntro = 104728, //Servant of Jondar
            Skeleton_A = 5393, //Skeleton
            Skeleton_A_Knee = 87012, //Royal Henchman
            Skeleton_A_Knee_Unique = 115403, //Headcleaver
            Skeleton_A_TemplarIntro = 5393, //Necromantic Minion
            Skeleton_A_TemplarIntro_NoWander = 105863, //Necromantic Minion
            Skeleton_B = 5395, //Returned
            Skeleton_D = 5397, //Skeletal Warrior
            Skeleton_Knee = 80652, //Skeleton_Knee
            Skeleton_twohander_A = 5411, //Skeletal Executioner
            Skeleton_twohander_B = 434, //Returned Executioner
            SlowTime_Proxy = 52695, //SlowTime_Proxy
            SnakeMan_Caster_A = 5428, //Serpent Magus
            SnakeMan_Melee_A = 5432, //Writhing Deceiver
            SnakeMan_Melee_B = 5433, //Doom Viper
            SnakeMan_Melee_C = 5434, //Writhing Deceiver
            SnakeMan_Melee_SaddleGuard = 60816, //Veiled Sentinel
            Spawner_Leor_Iron_Maiden = 100956, //Iron Maiden
            Spiderling_A = 5467, //Spiderling
            SpiderQueen = 51341, //Queen Araneae
            SporePod = 5482, //SporePod
            TargetDummy_Level40 = 51342, //High HP Test Dummy
            Teleport_Proxy = 5542, //Teleport_Proxy
            Templar = 4538, //Templar
            TemplarNPC = 4538, //Templar
            TemplarNPC_Imprisoned = 104813, //Warrior
            Tower_of_Power_Cultist_Voiceover = 153770, //Dark Cultist
            Tower_of_Power_Unique_Voiceover = 153773, //Tortured Soul
            trDun_Cath_Orb = 177365, //Gatekeeper
            trDun_Cath_SkeletonTotem = 177365, //Gatekeeper
            trDun_Cath_Skeleton_SummoningMachine = 176907, //Skeletal Guardian
            trDun_Cath_WallCollapse_01 = 5786, //NONE
            trDun_Cath_WoodDoor_A_Barricaded = 5792, //Barricaded Door
            trDun_Cath_WoodDoor_B_Barricaded = 5823, //NONE
            trDun_Crypt_Pillar_Spawner = 5840, //Activated Pillar
            trDun_Crypt_Urn_Group_A_01 = 5852, //NONE
            trDun_Crypt_Urn_Group_A_02 = 5853, //NONE
            trDun_Crypt_Urn_Group_A_03 = 5854, //NONE
            trDun_Magic_Painting = 5895, //Painting
            trDun_Magic_Painting_B = 5896, //Painting
            trDun_Magic_Painting_C = 5897, //Painting
            trDun_Magic_Painting_D = 5898, //Painting
            trDun_Wall_Colapse_A = 51345, //Falling Wall
            Treasure_Goblin_A = 5984, //Treasure Goblin
            Treasure_Goblin_B = 5985, //Treasure Seeker
            Treasure_Goblin_C = 5987, //Treasure Bandit
            Treasure_Goblin_D = 5988, //Treasure Pygmy
            Treasure_Goblin_Portal = 54862, //Portal
            TristramFemale = 51346, //Villager
            TristramFloatGuard = 5997, //Tristram Militia
            TristramGateGuardR = 5998, //Tristram Militia
            TristramGateGuardR_Event = 207483, //Tristram Militia
            TristramGuard = 51347, //Tristram Militia
            TristramGuard_A = 5999, //Tristram Militia
            TristramGuard_A_Ghost = 158122, //Enraged Spirit
            TristramGuard_B = 465, //Tristram Militia Recruit
            TristramGuard_C = 6002, //Tristram Militia
            TristramMale = 51348, //Villager
            Tristram_Mayor = 141508, //Mayor Holus
            Tristram_MilitiaGossip_Eran = 141650, //Eran
            Tristram_MilitiaGossip_Joshua = 141652, //Joshua
            trist_Urn_Tall = 6023, //NONE
            TriuneCultist_A = 6024, //Dark Cultist
            TriuneCultist_A_Templar = 145745, //Dark Cultist
            TriuneCultist_B = 6025, //Dark Zealot
            TriuneCultist_C = 6027, //Dark Cultist
            TriuneCultist_C_TortureLeader = 6027, //Cultist Grand Inquisitor
            TriuneSummoner_A = 6035, //Dark Summoner
            TriuneSummoner_B_RabbitHoleEvent = 111580, //Cadhul the Deathcaller
            TriuneVesselActivated_A = 6042, //The Subjugated
            TriuneVesselActivated_A_Unique_Tower_Of_Power = 189906, //Tortured Soul
            TriuneVessel_A = 6046, //Dark Vessel
            Triune_Berserker_A = 6052, //Dark Berserker
            Triune_Summonable_A = 6059, //Dark Hellion
            Triune_Summonable_B = 468, //Vicious Hellion
            trOut_Barkeep = 109467, //Bron the Barkeep
            trOut_Highlands_Goatmen_SummoningMachine_A_Node = 166452, //Khazra Totem
            Unburied_A = 6356, //Unburied
            Unburied_A_Unique = 6356, //Manglemaw
            Unburied_C = 6359, //Disentombed Hulk
            Unique_CaptainDaltyn = 156801, //Captain Daltyn
            Unique_WretchedQueen = 108444, //Wretched Queen
            vizjereiMale_A_CathAdventures = 162544, //Lloigor the Crazed
            Voiceover_Act1_Tinker = 128895, //Wandering Tinker
            Voiceover_FreedPrisoner = 111456, //Prisoner's Remains
            Warriv = 167955, //Warriv
            WD_fetish_A = 90321, //Witch Doctor's Fetish A
            WD_fetish_D = 107752, //Witch Doctor's Fetish D
            WD_Gargantuan = 122305, //Gargantuan
            WD_massConfusion_inkyBro = 184445, //Mass Confusion - Spirit
            WD_plagueOfToads_huge_toad = 107899, //Binkles the Frog
            WD_wallOfZombiesRune_spawn = 146534, //Wall Of Zombies Crawlers
            WD_ZombieDog = 51353, //Zombie Dog
            witchdoctor_bigbadvoodoo_fetish = 71643, //The Dancing Shaman
            witchDoctor_CorpseSpider_crimsonRune = 6443, //witchDoctor_CorpseSpider_crimsonRune
            witchDoctor_CorpseSpider_indigoRune = 6443, //Mommy Spider
            witchDoctor_CorpseSpider_obsidianRune = 107067, //Leaping Spider
            Witchdoctor_PitOfFire_fetish = 71643, //Witchdoctor_PitOfFire_fetish
            Witchdoctor_SpiritBarrage_RuneC = 181880, //Spirit Barrage - Rune C
            witchDoctor_spiritWalk_Dummy = 106584, //Wickerman
            WitherMoth_A = 6500, //Withermoth
            WitherMoth_A_Hidden = 6500, //Withermoth
            Wizard_ArcaneTorrent_RuneC = 166130, //Wizard_ArcaneTorrent_RuneC
            Wizard_HydraHead_Acid = 82111, //Acid Hydra
            Wizard_HydraHead_Arcane = 81515, //Arcane Hydra
            Wizard_HydraHead_Big = 83959, //Fire Hydra
            Wizard_HydraHead_Default = 80745, //Fire Hydra
            Wizard_HydraHead_Frost = 82972, //Frost Hydra
            Wizard_HydraHead_Lightning = 82109, //Lightning Hydra
            Wizard_MirrorImage_Female = 98010, //Wizard_MirrorImage_Female
            Wizard_MirrorImage_Male = 107916, //Wizard_MirrorImage_Male
            Wizard_Tornado = 6560, //Wizard_Tornado
            WoodWraith_A = 6572, //Gnarled Walker
            WoodWraith_A_HighlandsVer = 6572, //Highland Walker
            WoodWraith_Static_pose_01_A = 6583, //
            WoodWraith_Static_pose_01_B = 6584, //
            WoodWraith_Static_pose_02_A = 6586, //
            WoodWraith_Static_pose_02_B = 6587, //
            WoodWraith_Static_pose_03_A = 6589, //
            WoodWraith_Static_pose_03_B = 6590, //
            WoodWraith_Unique_A = 496, //The Old Man
            ZombieCrawler_A = 6632, //Crawling Torso
            ZombieCrawler_B = 6633, //Hungry Torso
            ZombieCrawler_C = 6634, //Voracious Torso
            ZombieCrawler_Custom_A = 6632, //Crawling Torso
            ZombieCrawler_Custom_B = 123160, //Hungry Torso
            zombieCrawler_Spawner_B = 176054, //Rotting Corpses
            ZombieFemale_A = 6638, //Deathspitter
            ZombieFemale_A_Blacksmith_A = 85900, //Mira Eamon
            ZombieFemale_A_FamilyTree_Mother = 77087, //Mother Rathe
            ZombieFemale_A_TristramQuest = 108444, //Wretched Mother
            ZombieFemale_B = 6639, //Retching Cadaver
            ZombieSkinny_A = 6644, //Risen
            ZombieSkinny_A_LeahInn = 203121, //Risen
            ZombieSkinny_B = 6646, //Ravenous Dead
            ZombieSkinny_C = 6647, //Voracious Zombie
            ZombieSkinny_D = 6651, //Decayer
            Zombie_A = 6652, //Walking Corpse
            Zombie_A_FamilyTree_Father = 77085, //Father Rathe
            Zombie_B = 6653, //Hungry Corpse
            Zombie_C = 6654, //Bloated Corpse
            Zombie_E = 204256, //Rancid Stumbler
        }

        public static int RandomNPC()
        {
            NPCList[] values = (NPCList[])Enum.GetValues(typeof(NPCList));
            int toReturn = (int)values[RandomHelper.Next(0, values.Length)];
            return toReturn;
        }
    }
}
