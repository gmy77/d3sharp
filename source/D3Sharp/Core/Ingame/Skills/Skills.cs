/*
 * Copyright (C) 2011 D3Sharp Project
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

namespace D3Sharp.Core.Ingame.Skills
{
    public class Skills
    {
        public static int None = -1;
        public static int BasicAttack = 0x00007780;

        public enum Barbarian
        {
            //
            // Fury Generator
            //
            Bash = 0x0001358A,
            Cleave = 0x00013987,
            LeapAttack = 0x00016CE1,
            GroundStomp = 0x00013656,
            Frenzy = 0x000132D4,
            WarCry = 0x00013ECC,
            FuriousCharge = 0x00017C9B,
            AncientSpear = 0x0001115B,

            //
            // Fury Spender
            //
            HammerOfTheAncients = 0x000134E5,
            ThreateningShout = 0x0001389C,
            BattleRage = 0x000134E4,
            WeaponThrow = 0x00016EBD,
            Rend = 0x00011348,
            SiesmicSlam = 0x000153CD,
            Sprint = 0x000132D7,
            Whirlwind = 0x00017828,

            //
            // Situational
            //
            IgnorePain = 0x000136A8,
            Revenge = 0x0001AB1E,
            Overpower = 0x00026DC1,
            Earthquake = 0x0001823E,
            CallOfTheAncients = 0x000138B1,
            WrathOfTheBerserker = 0x000136F7,

            //
            // Barbarian Passives
            //
            BloodThirst = 0x000321A1,
            PoundOfFlesh = 0x00032195,
            Ruthless = 0x00032177,
            WeaponsMaster = 0x00032543,
            InspiringPresence = 0x000322EA,
            BerserkerRage = 0x00032183,
            Animosity = 0x000321AC,
            Superstition = 0x000322B3,
            ToughAsNails = 0x00032418,
            NoEscape = 0x00031FB5,
            Relentless = 0x00032256,
            Brawler = 0x0003214D,
            Juggernaut = 0x0003238B,
            BoonOfBulKathos = 0x00031F3B,
            Unforgiving = 0x000321F4
        }

        public enum DemonHunter
        {
            //
            // Hatred Generator
            //
            HungeringArrow = 0x0001F8BF,
            EvasiveFire = 0x00020C41,
            BolaShot = 0x00012EF0,
            EntanglingShot = 0x00012861,
            Grenades = 0x00015252,
            SpikeTrap = 0x00012625,
            Strafe = 0x00020B8E,

            //
            // Hatred Spender
            //
            Impale = 0x00020126,
            RapidFire = 0x00020078,
            Chakram = 0x0001F8BD,
            ElementalArrow = 0x000200FD,
            FanOfKnives = 0x00012EEA,
            Multishot = 0x00012F51,
            ClusterArrow = 0x0001F8BE,
            RainOfVengeance = 0x001FF01,

            //
            // Discipline
            //
            Caltrops = 0x0001F8C0,
            Vault = 0x0001B26F,
            ShadowPower = 0x0001FF0E,
            Companion = 0x00020A3F,
            SmokeScreen = 0x0001FE87,
            Sentry = 0x0001F8C1,
            MarkedForDeath = 0x0001FEB2,
            Preparation = 0x0001F8BC,

            //
            // Demon Hunter Passives
            //
            Brooding = 0x00033771,
            ThrillOfTheHunt = 0x00033919,
            Vengeance = 0x00026042,
            SteadyAim = 0x0002820B,
            CullTheWeak = 0x00026049,
            Fundamentals = 0x00026047,
            HotPursuit = 0x0002604D,
            Archery = 0x00033346,
            Perfectionist = 0x0002604A,
            CustomEngineering = 0x00032EE2,
            Grenadier = 0x00032F8B,
            Sharpshooter = 0x00026043,
            Ballistics = 0x0002604B,
        }

        public enum Monk
        {
            //
            // Spirit Generator
            //
            FistsOfThunder = 0x000176C4,
            DeadlyReach = 0x00017713,
            CripplingWave = 0x00017837,
            ExplodingPalm = 0x00017C30,
            SweepingWind = 0x0001775A,
            WayOfTheHundredFists = 0x00017B56,

            //
            // Spirit Spender
            //
            BlindingFlash = 0x000216FA,
            BreathOfHeaven = 0x00010E0A,
            LashingTailKick = 0x0001B43C,
            DashingStrike = 0x000177CB,
            LethalDecoy = 0x00011044,
            InnerSanctuary = 0x00017BC6,
            TempestRush = 0x0001DA62,
            Serenity = 0x000177D7,
            SevenSidedStrike = 0x000179B6,
            MysticAlly = 0x0001E148,
            WaveOfLight = 0x00017721,

            //
            // Mantras
            //
            MantraOfEvasion = 0x0002EF95,
            MantraOfRetribution = 0x00010F6C,
            MantraOfHealing = 0x00010F72,
            MantraOfConviction = 0x00017554,

            //
            // Monk Passives
            //
            TheGuardiansPath = 0x00033394,
            SixthSense = 0x000332D6,
            OneWithEverything = 0x000332F8,
            SeizeTheInitiative = 0x000332DC,
            Transcendence = 0x00033162,
            ChantOfResonance = 0x00026333,
            BeaconOfYtar = 0x000330D0,
            FleetFooted = 0x00033085,
            ExaltedSoul = 0x00033083,
            Pacifism = 0x00033395,
            GuidingLight = 0x0002634C,
            NearDeathExperience = 0x00026344,
            Resolve = 0x00033A7D
        }

        public enum WitchDoctor
        {
            //
            // Physical Realm
            //
            PoisonDart = 0x0001930D,
            PlagueOfToads = 0x00019FE1,
            ZombieCharger = 0x00012113,
            CorpseSpiders = 0x000110EA,
            Firebats = 0x00019DEB,
            Firebomb = 0x000107EF,
            LocustSwarm = 0x000110EB,
            AcidCloud = 0x00011337,
            WallOfZombies = 0x00020EB5,

            //
            // Spirit Realm
            //
            Haunt = 0x00014692,
            Horrify = 0x00010854,
            SpiritWalk = 0x00019EFD,
            SoulHarvest = 0x00010820,
            SpiritBarrage = 0x0001A7DA,
            MassConfusion = 0x00010810,

            //
            // Support
            //
            SummonZombieDogs = 0x000190AD,
            GraspOfTheDead = 0x00010E3E,
            Hex = 0x000077A7,
            Sacrifice = 0x000190AC,
            Gargantuan = 0x000077A0,
            BigBadVoodoo = 0x0001CA9A,
            FetishArmy = 0x000011C51,

            //
            // Witch Doctor Passives
            //
            CircleOfLife = 0x00032EBB,
            Vermin = 0x00032EB7,
            SpiritualAttunement = 0x00032EB9,
            GruesomeFeast = 0x00032ED2,
            BloodRitual = 0x00032EB8,
            ZombieHandler = 0x00032EB3,
            PierceTheVeil = 0x00032EF4,
            RushOfEssence = 0x00032EB5,
            VisionQuest = 0x00033091,
            FierceLoyalty = 0x00032EFF,
            DeathTrance = 0x00032EB4,
            TribalRites = 0x00032ED9
        }

        public enum Wizard
        {
            //
            // Signature
            //
            MagicMissile = 0x00007818,
            ShockPulse = 0x0000783F,
            SpectralBlade = 0x0001177C,
            Electrocute = 0x000006E5,

            //
            // Offensive
            //
            WaveOfForce = 0x0000784C,
            ArcaneOrb = 0x000077CC,
            EnergyTwister = 0x00012D39,
            Disintegrate = 0x0001659D,
            ExplosiveBlast = 0x000155E5,
            Hydra = 0x00007805,
            RayOfFrost = 0x00016CD3,
            ArcaneTorrent = 0x00020D38,
            Meteor = 0x00010E46,
            Blizzard = 0x000077D8,

            //
            // Utility
            //
            FrostNova = 0x000077FE,
            IceArmor = 0x00011E07,
            MagicWeapon = 0x0001294C,
            DiamondSkin = 0x0001274F,
            StormArmor = 0x00012303,
            MirrorImage = 0x00017EEB,
            SlowTime = 0x000006E9,
            Teleport = 0x00029198,
            EnergyArmor = 0x000153CF,
            Familiar = 0x00018330,
            Archon = 0x00020ED8,

            //
            // Wizard Passives
            //
            PowerHungry = 0x00032E5E,
            TemporalFlux = 0x00032E5D,
            GlassCannon = 0x00032E57,
            Prodigy = 0x00032E6D,
            Virtuoso = 0x00032E65,
            AstralPresence = 0x00032E58,
            Illusionist = 0x00032EA3,
            GalvanizingWard = 0x00032E9D,
            Blur = 0x00032E54,
            Evocation = 0x00032E59,
            ArcaneDynamo = 0x00032FB7,
            UnstableAnomaly = 0x00032E5A
        }

        public enum Enchantress
        {
            Disorient = 0x00018C31,
            ReflectMissiles = 0x00018EF5,
            FocusedMind = 0x00018E66,
            ForcefulPush = 0x00018E51,
            ScorchedEarth = 0x00018E6B,
            PoweredArmor = 0x00018C55,
            Charm = 0x00018EA9,
            MassCharm = 0x00031334
        }

        public enum Scoundrel
        {
            RapidFire = 0x000175CA,
            PoisonBolts = 0x000076FC,
            PowerShot = 0x00030DE9,
            Anatomy = 0x000076F6,
            Multishot = 0x000076FA,
            DirtyFighting = 0x00017C9C,
            CripplingShot = 0x000175BB
        }

        public enum Templar
        {
            Intimidate = 0x00016ECD,
            Inspire = 0x00007694,
            Guardian = 0x00007697,
            Intervene = 0x00007698,
            Onslaught = 0x00016EC0,
            Charge = 0x00016EF2,
            Heal = 0x000014DE
        }
    }
}
