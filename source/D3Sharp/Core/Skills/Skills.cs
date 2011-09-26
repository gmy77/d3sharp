namespace D3Sharp.Core.Skills
{
    public class Skills
    {
        public int None = -1;
        public int BasicAttack = 0x00007780;

        public enum Barbarian
        {
            Bash = 0x0001358A,
            Cleave = 0x00013987,
            LeapAttack = 0x00016CE1,
            GroundStomp = 0x00013656,
            Frenzy = 0x000132D4,
            WarCry = 0x00013ECC,
            FuriousCharge = 0x00017C9B,
            AncientSpear = 0x0001115B,
            HammerOfTheAncients = 0x000134E5,
            ThreateningShout = 0x0001389C,
            BattleRage = 0x000134E4,
            WeaponThrow = 0x00016EBD,
            Rend = 0x00011348,
            SiesmicSlam = 0x000153CD,
            Sprint = 0x000132D7,
            Whirlwind = 0x00017828,
            IgnorePain = 0x000136A8,
            Revenge = 0x0001AB1E,
            Overpower = 0x00026DC1,
            Earthquake = 0x0001823E,
            CallOfTheAncients = 0x000138B1,
            WrathOfTheBerserker = 0x000136F7
        }

        public enum DemonHunter
        {
            HungeringArrow = 0x0001F8BF,
            EvasiveFire = 0x00020C41,
            BolaShot = 0x00012EF0,
            EntanglingShot = 0x00012861,
            Grenades = 0x00015252,
            SpikeTrap = 0x00012625,
            Strafe = 0x00020B8E,
            Impale = 0x00020126,
            RapidFire = 0x00020078,
            Chakram = 0x0001F8BD,
            ElementalArrow = 0x000200FD,
            FanOfKnives = 0x00012EEA,
            Multishot = 0x00012F51,
            ClusterArrow = 0x0001F8BE,
            Caltrops = 0x0001F8C0,
            Vault = 0x0001B26F,
            ShadowPower = 0x0001FF0E,
            Companion = 0x00020A3F,
            SmokeScreen = 0x0001FE87,
            Sentry = 0x0001F8C1,
            MarkedForDeath = 0x0001FEB2,
            Preparation = 0x0001F8BC
            //not in old build - Rain of Vengeance
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
            WaveOfLight= 0x00017721,

            //
            // Mantras
            //
            MantraOfEvasion = 0x0002EF95,
            MantraOfRetribution = 0x00010F6C,
            MantraOfHealing = 0x00010F72,
            MantraOfConviction = 0x00017554
        }

        public enum WitchDoctor
        {
            SummonZombieDogs = 0x000190AD,
            PoisonDart = 0x0001930D,
            PlagueOfToads = 0x00019FE1,
            GraspOfTheDead = 0x00010E3E,
            Haunt = 0x00014692,
            ZombieCharger = 0x00012113,
            Hex = 0x000077A7,
            CorpseSpiders = 0x000110EA,
            Horrify = 0x00010854,
            Firebats = 0x00019DEB,
            Firebomb = 0x000107EF,
            SpiritWalk = 0x00019EFD,
            SoulHarvest = 0x00010820,
            Sacrifice = 0x000190AC,
            Gargantuan = 0x000077A0,
            LocustSwarm = 0x000110EB,
            SpiritBarrage = 0x0001A7DA,
            AcidCloud = 0x00011337,
            MassConfusion = 0x00010810,
            BigBadVoodoo = 0x0001CA9A,
            WallOfZombies = 0x00020EB5,
            FetishArmy = 0x000011C51
        }

        public enum Wizard
        {
            MagicMissile = 0x00007818,
            ShockPulse = 0x0000783F,
            SpectralBlade = 0x0001177C,
            Electrocute = 0x000006E5,
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
            Archon = 0x00020ED8
        }
    }
}
