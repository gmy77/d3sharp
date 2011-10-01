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

using System;
using System.Linq;
using System.Collections.Generic;
using Mooege.Core.Common.Toons;

namespace Mooege.Core.GS.Skills
{
    public class Skills
    {
        public static int None = -1;
        public static int BasicAttack = 0x00007780;

        public static List<int> GetAllActiveSkillsByClass(ToonClass @class)
        {
            switch (@class)
            {
                case ToonClass.Barbarian:
                    return Barbarian.AllActiveSkillsList;
                case ToonClass.DemonHunter:
                    return DemonHunter.AllActiveSkillsList;
                case ToonClass.Monk:
                    return Monk.AllActiveSkillsList;
                case ToonClass.WitchDoctor:
                    return WitchDoctor.AllActiveSkillsList;
                case ToonClass.Wizard:
                    return Wizard.AllActiveSkillsList;
                default:
                    return null;
            }
        }

        public static List<int> GetPrimarySkillsByClass(ToonClass @class)
        {
            switch (@class)
            {
                case ToonClass.Barbarian:
                    return Barbarian.FuryGenerators.List;
                case ToonClass.DemonHunter:
                    return DemonHunter.HatredGenerators.List;
                case ToonClass.Monk:
                    return Monk.SpiritGenerator.List;
                case ToonClass.WitchDoctor:
                    return WitchDoctor.PhysicalRealm.List;
                case ToonClass.Wizard:
                    return Wizard.Signature.List;
                default:
                    return null;
            }
        }

        public static List<int> GetSecondarySkillsByClass(ToonClass @class)
        {
            switch (@class)
            {
                case ToonClass.Barbarian:
                    return Barbarian.FurySpenders.List;
                case ToonClass.DemonHunter:
                    return DemonHunter.HatredSpenders.List;
                case ToonClass.Monk:
                    return Monk.SpiritSpenders.List;
                case ToonClass.WitchDoctor:
                    return WitchDoctor.SpiritRealm.List;
                case ToonClass.Wizard:
                    return Wizard.Offensive.List;
                default:
                    return null;
            }
        }

        public static List<int> GetExtraSkillsByClass(ToonClass @class)
        {
            switch (@class)
            {
                case ToonClass.Barbarian:
                    return Barbarian.Situational.List;
                case ToonClass.DemonHunter:
                    return DemonHunter.Discipline.List;
                case ToonClass.Monk:
                    return Monk.Mantras.List;
                case ToonClass.WitchDoctor:
                    return WitchDoctor.Support.List;
                case ToonClass.Wizard:
                    return Wizard.Utility.List;
                default:
                    return null;
            }
        }

        public static List<int> GetPassiveSkills(ToonClass @class)
        {
            switch (@class)
            {
                case ToonClass.Barbarian:
                    return Barbarian.Passives.List;
                case ToonClass.DemonHunter:
                    return DemonHunter.Passives.List;
                case ToonClass.Monk:
                    return Monk.Passives.List;
                case ToonClass.WitchDoctor:
                    return WitchDoctor.Passives.List;
                case ToonClass.Wizard:
                    return Wizard.Passives.List;
                default:
                    return null;
            }
        }

        #region barbarian

        public class Barbarian
        {
            public static readonly List<int> AllActiveSkillsList =
                FuryGenerators.List.Concat(FurySpenders.List).Concat(Situational.List).ToList();

            public class FuryGenerators
            {
                public static readonly int Bash = 0x0001358A;
                public static readonly int Cleave = 0x00013987;
                public static readonly int LeapAttack = 0x00016CE1;
                public static readonly int GroundStomp = 0x00013656;
                public static readonly int Frenzy = 0x000132D4;
                public static readonly int WarCry = 0x00013ECC;
                public static readonly int FuriousCharge = 0x00017C9B;
                public static readonly int AncientSpear = 0x0001115B;

                public static readonly List<int> List = new List<int>
                                                            {
                                                                Bash,
                                                                Cleave,
                                                                LeapAttack,
                                                                GroundStomp,
                                                                Frenzy,
                                                                WarCry,
                                                                FuriousCharge,
                                                                AncientSpear
                                                            };
            }

            public class FurySpenders
            {
                public static readonly int HammerOfTheAncients = 0x000134E5;
                public static readonly int ThreateningShout = 0x0001389C;
                public static readonly int BattleRage = 0x000134E4;
                public static readonly int WeaponThrow = 0x00016EBD;
                public static readonly int Rend = 0x00011348;
                public static readonly int SiesmicSlam = 0x000153CD;
                public static readonly int Sprint = 0x000132D7;
                public static readonly int Whirlwind = 0x00017828;

                public static readonly List<int> List = new List<int>
                                                            {
                                                                HammerOfTheAncients,
                                                                ThreateningShout,
                                                                BattleRage,
                                                                WeaponThrow,
                                                                Rend,
                                                                SiesmicSlam,
                                                                Sprint,
                                                                Whirlwind
                                                            };
            }

            public class Situational
            {
                public static readonly int IgnorePain = 0x000136A8;
                public static readonly int Revenge = 0x0001AB1E;
                public static readonly int Overpower = 0x00026DC1;
                public static readonly int Earthquake = 0x0001823E;
                public static readonly int CallOfTheAncients = 0x000138B1;
                public static readonly int WrathOfTheBerserker = 0x000136F7;

                public static readonly List<int> List = new List<int>
                                                            {
                                                                IgnorePain,
                                                                Revenge,
                                                                Overpower,
                                                                Earthquake,
                                                                CallOfTheAncients,
                                                                WrathOfTheBerserker
                                                            };
            }

            public class Passives
            {
                public static readonly int BloodThirst = 0x000321A1;
                public static readonly int PoundOfFlesh = 0x00032195;
                public static readonly int Ruthless = 0x00032177;
                public static readonly int WeaponsMaster = 0x00032543;
                public static readonly int InspiringPresence = 0x000322EA;
                public static readonly int BerserkerRage = 0x00032183;
                public static readonly int Animosity = 0x000321AC;
                public static readonly int Superstition = 0x000322B3;
                public static readonly int ToughAsNails = 0x00032418;
                public static readonly int NoEscape = 0x00031FB5;
                public static readonly int Relentless = 0x00032256;
                public static readonly int Brawler = 0x0003214D;
                public static readonly int Juggernaut = 0x0003238B;
                public static readonly int BoonOfBulKathos = 0x00031F3B;
                public static readonly int Unforgiving = 0x000321F4;

                public static readonly List<int> List = new List<int>
                                                            {
                                                                BloodThirst,
                                                                PoundOfFlesh,
                                                                Ruthless,
                                                                WeaponsMaster,
                                                                InspiringPresence,
                                                                BerserkerRage,
                                                                Animosity,
                                                                Superstition,
                                                                ToughAsNails,
                                                                NoEscape,
                                                                Relentless,
                                                                Brawler,
                                                                Juggernaut,
                                                                BoonOfBulKathos,
                                                                Unforgiving
                                                            };
            }
        }

        #endregion

        #region demon-hunter

        public class DemonHunter
        {
            public static readonly List<int> AllActiveSkillsList =
                HatredGenerators.List.Concat(HatredSpenders.List).Concat(Discipline.List).ToList();


            public class HatredGenerators
            {
                public static readonly int HungeringArrow = 0x0001F8BF;
                public static readonly int EvasiveFire = 0x00020C41;
                public static readonly int BolaShot = 0x00012EF0;
                public static readonly int EntanglingShot = 0x00012861;
                public static readonly int Grenades = 0x00015252;
                public static readonly int SpikeTrap = 0x00012625;
                public static readonly int Strafe = 0x00020B8E;

                public static readonly List<int> List = new List<int>
                                                            {
                                                                HungeringArrow,
                                                                EvasiveFire,
                                                                BolaShot,
                                                                EntanglingShot,
                                                                Grenades,
                                                                SpikeTrap,
                                                                Strafe
                                                            };
            }

            public class HatredSpenders
            {
                public static readonly int Impale = 0x00020126;
                public static readonly int RapidFire = 0x00020078;
                public static readonly int Chakram = 0x0001F8BD;
                public static readonly int ElementalArrow = 0x000200FD;
                public static readonly int FanOfKnives = 0x00012EEA;
                public static readonly int Multishot = 0x00012F51;
                public static readonly int ClusterArrow = 0x0001F8BE;
                public static readonly int RainOfVengeance = 0x001FF01;

                public static readonly List<int> List = new List<int>
                                                            {
                                                                Impale,
                                                                RapidFire,
                                                                Chakram,
                                                                ElementalArrow,
                                                                FanOfKnives,
                                                                Multishot,
                                                                ClusterArrow,
                                                                RainOfVengeance
                                                            };
            }

            public class Discipline
            {
                public static readonly int Caltrops = 0x0001F8C0;
                public static readonly int Vault = 0x0001B26F;
                public static readonly int ShadowPower = 0x0001FF0E;
                public static readonly int Companion = 0x00020A3F;
                public static readonly int SmokeScreen = 0x0001FE87;
                public static readonly int Sentry = 0x0001F8C1;
                public static readonly int MarkedForDeath = 0x0001FEB2;
                public static readonly int Preparation = 0x0001F8BC;

                public static readonly List<int> List = new List<int>
                                                            {
                                                                Caltrops,
                                                                Vault,
                                                                ShadowPower,
                                                                Companion,
                                                                SmokeScreen,
                                                                Sentry,
                                                                MarkedForDeath,
                                                                Preparation
                                                            };
            }

            public class Passives
            {
                public static readonly int Brooding = 0x00033771;
                public static readonly int ThrillOfTheHunt = 0x00033919;
                public static readonly int Vengeance = 0x00026042;
                public static readonly int SteadyAim = 0x0002820B;
                public static readonly int CullTheWeak = 0x00026049;
                public static readonly int Fundamentals = 0x00026047;
                public static readonly int HotPursuit = 0x0002604D;
                public static readonly int Archery = 0x00033346;
                public static readonly int Perfectionist = 0x0002604A;
                public static readonly int CustomEngineering = 0x00032EE2;
                public static readonly int Grenadier = 0x00032F8B;
                public static readonly int Sharpshooter = 0x00026043;
                public static readonly int Ballistics = 0x0002604B;

                public static readonly List<int> List = new List<int>
                                                            {
                                                                Brooding,
                                                                ThrillOfTheHunt,
                                                                Vengeance,
                                                                SteadyAim,
                                                                CullTheWeak,
                                                                Fundamentals,
                                                                HotPursuit,
                                                                Archery,
                                                                Perfectionist,
                                                                CustomEngineering,
                                                                Grenadier,
                                                                Sharpshooter,
                                                                Ballistics
                                                            };
            }
        }

        #endregion

        #region monk

        public class Monk
        {
            public static readonly List<int> AllActiveSkillsList =
                SpiritGenerator.List.Concat(SpiritSpenders.List).Concat(Mantras.List).ToList();

            public class SpiritGenerator
            {
                public static readonly int FistsOfThunder = 0x000176C4;
                public static readonly int DeadlyReach = 0x00017713;
                public static readonly int CripplingWave = 0x00017837;
                public static readonly int ExplodingPalm = 0x00017C30;
                public static readonly int SweepingWind = 0x0001775A;
                public static readonly int WayOfTheHundredFists = 0x00017B56;

                public static readonly List<int> List = new List<int>
                                                            {
                                                                FistsOfThunder,
                                                                DeadlyReach,
                                                                CripplingWave,
                                                                ExplodingPalm,
                                                                SweepingWind,
                                                                WayOfTheHundredFists
                                                            };
            }

            public class SpiritSpenders
            {
                public static readonly int BlindingFlash = 0x000216FA;
                public static readonly int BreathOfHeaven = 0x00010E0A;
                public static readonly int LashingTailKick = 0x0001B43C;
                public static readonly int DashingStrike = 0x000177CB;
                public static readonly int LethalDecoy = 0x00011044;
                public static readonly int InnerSanctuary = 0x00017BC6;
                public static readonly int TempestRush = 0x0001DA62;
                public static readonly int Serenity = 0x000177D7;
                public static readonly int SevenSidedStrike = 0x000179B6;
                public static readonly int MysticAlly = 0x0001E148;
                public static readonly int WaveOfLight = 0x00017721;

                public static readonly List<int> List = new List<int>
                                                            {
                                                                BlindingFlash,
                                                                BreathOfHeaven,
                                                                LashingTailKick,
                                                                DashingStrike,
                                                                LethalDecoy,
                                                                InnerSanctuary,
                                                                TempestRush,
                                                                Serenity,
                                                                SevenSidedStrike,
                                                                MysticAlly,
                                                                WaveOfLight
                                                            };
            }

            public class Mantras
            {
                public static readonly int MantraOfEvasion = 0x0002EF95;
                public static readonly int MantraOfRetribution = 0x00010F6C;
                public static readonly int MantraOfHealing = 0x00010F72;
                public static readonly int MantraOfConviction = 0x00017554;

                public static readonly List<int> List = new List<int>
                                                            {
                                                                MantraOfEvasion,
                                                                MantraOfRetribution,
                                                                MantraOfHealing,
                                                                MantraOfConviction
                                                            };
            }

            public class Passives
            {
                public static readonly int TheGuardiansPath = 0x00033394;
                public static readonly int SixthSense = 0x000332D6;
                public static readonly int OneWithEverything = 0x000332F8;
                public static readonly int SeizeTheInitiative = 0x000332DC;
                public static readonly int Transcendence = 0x00033162;
                public static readonly int ChantOfResonance = 0x00026333;
                public static readonly int BeaconOfYtar = 0x000330D0;
                public static readonly int FleetFooted = 0x00033085;
                public static readonly int ExaltedSoul = 0x00033083;
                public static readonly int Pacifism = 0x00033395;
                public static readonly int GuidingLight = 0x0002634C;
                public static readonly int NearDeathExperience = 0x00026344;
                public static readonly int Resolve = 0x00033A7D;

                public static readonly List<int> List = new List<int>
                                                            {
                                                                TheGuardiansPath,
                                                                SixthSense,
                                                                OneWithEverything,
                                                                SeizeTheInitiative,
                                                                Transcendence,
                                                                ChantOfResonance,
                                                                BeaconOfYtar,
                                                                FleetFooted,
                                                                ExaltedSoul,
                                                                Pacifism,
                                                                GuidingLight,
                                                                NearDeathExperience,
                                                                Resolve
                                                            };
            }
        }

        #endregion

        #region witch-hunter

        public class WitchDoctor
        {
            public static readonly List<int> AllActiveSkillsList =
                PhysicalRealm.List.Concat(SpiritRealm.List).Concat(Support.List).ToList();

            public class PhysicalRealm
            {
                public static readonly int PoisonDart = 0x0001930D;
                public static readonly int PlagueOfToads = 0x00019FE1;
                public static readonly int ZombieCharger = 0x00012113;
                public static readonly int CorpseSpiders = 0x000110EA;
                public static readonly int Firebats = 0x00019DEB;
                public static readonly int Firebomb = 0x000107EF;
                public static readonly int LocustSwarm = 0x000110EB;
                public static readonly int AcidCloud = 0x00011337;
                public static readonly int WallOfZombies = 0x00020EB5;

                public static readonly List<int> List = new List<int>
                                                            {
                                                                PoisonDart,
                                                                PlagueOfToads,
                                                                ZombieCharger,
                                                                CorpseSpiders,
                                                                Firebats,
                                                                Firebomb,
                                                                LocustSwarm,
                                                                AcidCloud,
                                                                WallOfZombies
                                                            };
            }

            public class SpiritRealm
            {
                public static readonly int Haunt = 0x00014692;
                public static readonly int Horrify = 0x00010854;
                public static readonly int SpiritWalk = 0x00019EFD;
                public static readonly int SoulHarvest = 0x00010820;
                public static readonly int SpiritBarrage = 0x0001A7DA;
                public static readonly int MassConfusion = 0x00010810;

                public static readonly List<int> List = new List<int>
                                                            {
                                                                Haunt,
                                                                Horrify,
                                                                SpiritWalk,
                                                                SoulHarvest,
                                                                SpiritBarrage,
                                                                MassConfusion
                                                            };
            }

            public class Support
            {
                public static readonly int SummonZombieDogs = 0x000190AD;
                public static readonly int GraspOfTheDead = 0x00010E3E;
                public static readonly int Hex = 0x000077A7;
                public static readonly int Sacrifice = 0x000190AC;
                public static readonly int Gargantuan = 0x000077A0;
                public static readonly int BigBadVoodoo = 0x0001CA9A;
                public static readonly int FetishArmy = 0x000011C51;

                public static readonly List<int> List = new List<int>
                                                            {
                                                                SummonZombieDogs,
                                                                GraspOfTheDead,
                                                                Hex,
                                                                Sacrifice,
                                                                Gargantuan,
                                                                BigBadVoodoo,
                                                                FetishArmy
                                                            };
            }

            public class Passives
            {
                public static readonly int CircleOfLife = 0x00032EBB;
                public static readonly int Vermin = 0x00032EB7;
                public static readonly int SpiritualAttunement = 0x00032EB9;
                public static readonly int GruesomeFeast = 0x00032ED2;
                public static readonly int BloodRitual = 0x00032EB8;
                public static readonly int ZombieHandler = 0x00032EB3;
                public static readonly int PierceTheVeil = 0x00032EF4;
                public static readonly int RushOfEssence = 0x00032EB5;
                public static readonly int VisionQuest = 0x00033091;
                public static readonly int FierceLoyalty = 0x00032EFF;
                public static readonly int DeathTrance = 0x00032EB4;
                public static readonly int TribalRites = 0x00032ED9;

                public static readonly List<int> List = new List<int>
                                                            {
                                                                CircleOfLife,
                                                                Vermin,
                                                                SpiritualAttunement,
                                                                GruesomeFeast,
                                                                BloodRitual,
                                                                ZombieHandler,
                                                                PierceTheVeil,
                                                                RushOfEssence,
                                                                VisionQuest,
                                                                FierceLoyalty,
                                                                DeathTrance,
                                                                TribalRites,
                                                            };
            }
        }

        #endregion

        #region wizard

        public class Wizard
        {
            public static readonly List<int> AllActiveSkillsList =
                Signature.List.Concat(Offensive.List).Concat(Utility.List).ToList();

            public class Signature
            {
                public static readonly int MagicMissile = 0x00007818;
                public static readonly int ShockPulse = 0x0000783F;
                public static readonly int SpectralBlade = 0x0001177C;
                public static readonly int Electrocute = 0x000006E5;

                public static readonly List<int> List = new List<int>
                                                            {
                                                                MagicMissile,
                                                                ShockPulse,
                                                                SpectralBlade,
                                                                Electrocute
                                                            };
            }

            public class Offensive
            {
                public static readonly int WaveOfForce = 0x0000784C;
                public static readonly int ArcaneOrb = 0x000077CC;
                public static readonly int EnergyTwister = 0x00012D39;
                public static readonly int Disintegrate = 0x0001659D;
                public static readonly int ExplosiveBlast = 0x000155E5;
                public static readonly int Hydra = 0x00007805;
                public static readonly int RayOfFrost = 0x00016CD3;
                public static readonly int ArcaneTorrent = 0x00020D38;
                public static readonly int Meteor = 0x00010E46;
                public static readonly int Blizzard = 0x000077D8;

                public static readonly List<int> List = new List<int>
                                                            {
                                                                WaveOfForce,
                                                                ArcaneOrb,
                                                                EnergyTwister,
                                                                Disintegrate,
                                                                ExplosiveBlast,
                                                                Hydra,
                                                                RayOfFrost,
                                                                ArcaneTorrent,
                                                                Meteor,
                                                                Blizzard
                                                            };
            }

            public class Utility
            {
                public static readonly int FrostNova = 0x000077FE;
                public static readonly int IceArmor = 0x00011E07;
                public static readonly int MagicWeapon = 0x0001294C;
                public static readonly int DiamondSkin = 0x0001274F;
                public static readonly int StormArmor = 0x00012303;
                public static readonly int MirrorImage = 0x00017EEB;
                public static readonly int SlowTime = 0x000006E9;
                public static readonly int Teleport = 0x00029198;
                public static readonly int EnergyArmor = 0x000153CF;
                public static readonly int Familiar = 0x00018330;
                public static readonly int Archon = 0x00020ED8;

                public static readonly List<int> List = new List<int>
                                                            {
                                                                FrostNova,
                                                                IceArmor,
                                                                MagicWeapon,
                                                                DiamondSkin,
                                                                StormArmor,
                                                                MirrorImage,
                                                                SlowTime,
                                                                Teleport,
                                                                EnergyArmor,
                                                                Familiar,
                                                                Archon
                                                            };
            }

            public class Passives
            {
                public static readonly int PowerHungry = 0x00032E5E;
                public static readonly int TemporalFlux = 0x00032E5D;
                public static readonly int GlassCannon = 0x00032E57;
                public static readonly int Prodigy = 0x00032E6D;
                public static readonly int Virtuoso = 0x00032E65;
                public static readonly int AstralPresence = 0x00032E58;
                public static readonly int Illusionist = 0x00032EA3;
                public static readonly int GalvanizingWard = 0x00032E9D;
                public static readonly int Blur = 0x00032E54;
                public static readonly int Evocation = 0x00032E59;
                public static readonly int ArcaneDynamo = 0x00032FB7;
                public static readonly int UnstableAnomaly = 0x00032E5A;

                public static readonly List<int> List = new List<int>
                                                            {
                                                                PowerHungry,
                                                                TemporalFlux,
                                                                GlassCannon,
                                                                Prodigy,
                                                                Virtuoso,
                                                                AstralPresence,
                                                                Illusionist,
                                                                GalvanizingWard,
                                                                Blur,
                                                                Evocation,
                                                                ArcaneDynamo,
                                                                UnstableAnomaly
                                                            };
            }
        }

        #endregion
    }
}
