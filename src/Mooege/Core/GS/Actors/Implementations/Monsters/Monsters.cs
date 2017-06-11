﻿/*
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mooege.Core.GS.Map;
using Mooege.Core.GS.Common.Types.TagMap;
using Mooege.Core.GS.AI.Brains;
using Mooege.Net.GS.Message;


namespace Mooege.Core.GS.Actors.Implementations.Monsters
{
    #region Goblins
    //
    //54862 - spawn goblin portal
    //
    [HandledSNO(5984)]
    public class TreasureGoblin : Monster
    {
        public TreasureGoblin(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
        }
    }

    [HandledSNO(5985)]
    public class TreasureSeeker : Monster
    {
        public TreasureSeeker(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
        }
    }

    [HandledSNO(5987)]
    public class TreasureBandit : Monster
    {
        public TreasureBandit(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
        }
    }

    [HandledSNO(5988)]
    public class TreasurePygmy : Monster
    {
        public TreasurePygmy(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
        }
    }
    #endregion
    #region Spore
    [HandledSNO(5482)]
    public class Spore : Monster
    {
        public Spore(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
            (Brain as MonsterBrain).AddPresetPower(30525);//SporeCloud.pow
        }
    }
    #endregion
    #region QuillDemon
    [HandledSNO(4982)]
    public class QuillDemon : Monster
    {
        public QuillDemon(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
            (Brain as MonsterBrain).AddPresetPower(107729);
            (Brain as MonsterBrain).AddPresetPower(30592);
        }
    }
    #endregion
    #region DarkCultists
    [HandledSNO(6024)]
    public class DarkCultists : Monster
    {
        public DarkCultists(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
        }
    }
    [HandledSNO(6035)]
    public class DarkSummoner : Monster
    {
        public DarkSummoner(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
            (Brain as MonsterBrain).AddPresetPower(30592);
        }
    }
    #endregion
    #region Ghost
    //Enraged phantom 370, 136943
    [HandledSNO(370, 136943)]
    public class EnragedPhantom : Monster
    {
        public EnragedPhantom(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
            this.Attributes[GameAttribute.Damage_Weapon_Min, 0] = 15f;
        }
    }
    #endregion
    #region Unburieds
    [HandledSNO(6356)]
    public class Unburied : Monster
    {
        public Unburied(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
            (Brain as MonsterBrain).AddPresetPower(30592);
        }
    }

    [HandledSNO(6359)]
    public class DisentombHulk : Monster
    {
        public DisentombHulk(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
            (Brain as MonsterBrain).AddPresetPower(30592);
        }
    }
    #endregion
    #region WoodWraiths
    [HandledSNO(6572, 139454, 139456)]
    public class WoodWraith : Monster
    {
        public WoodWraith(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
            (Brain as MonsterBrain).AddPresetPower(30592);
        }
    }
    [HandledSNO(170324, 170325, 495)]
    public class HighLandWalker : Monster
    {
        public HighLandWalker(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
            (Brain as MonsterBrain).AddPresetPower(30592);
        }
    }
    [HandledSNO(496)]
    public class TheOldMan : Monster
    {
        public TheOldMan(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
            (Brain as MonsterBrain).AddPresetPower(30592);
            // Summon Spores
        }
    }
    #endregion
    #region Zombies
    [HandledSNO(6652)]
    public class WalkingCorpse : Monster
    {
        public WalkingCorpse(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
        }
    }
    [HandledSNO(6653)]
    public class HungryCorpse : Monster
    {
        public HungryCorpse(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
        }
    }
    [HandledSNO(6654)]
    public class BloatedCorpse : Monster
    {
        public BloatedCorpse(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
        }
    }
    [HandledSNO(204256)]
    public class RancidStumbler : Monster
    {
        public RancidStumbler(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
        }
    }
    [HandledSNO(6644)] //ZombieSkinny
    public class Risen : Monster
    {
        public Risen(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
        }
    }
    [HandledSNO(6646)] //ZombieSkinny
    public class RavenousDead : Monster
    {
        public RavenousDead(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
        }
    }
    [HandledSNO(6647)] //ZombieSkinny
    public class VoraciousZombie : Monster
    {
        public VoraciousZombie(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
        }
    }
    [HandledSNO(6651)] //ZombieSkinny
    public class Decayer : Monster
    {
        public Decayer(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
        }
    }
    //Risen
    [HandledSNO(218339)] //ZombieSkinny_Custom_A.acr
    public class ZombieSkinny : Monster
    {
        public ZombieSkinny(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
        }
    }
    [HandledSNO(218367)] //ZombieCrawler_Barricade_A.acr
    public class CrowlingTorso : Monster
    {
        public CrowlingTorso(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
        }
    }
    [HandledSNO(203121)] //ZombieSkinny_A_LeahInn.acr
    public class LeahInnZombie : Monster
    {
        public LeahInnZombie(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
        }
    }
    #endregion
    #region Skeleton
    [HandledSNO(539)]
    public class Skeleton : Monster
    {
        public Skeleton(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
        }
    }
    [HandledSNO(87012)]
    public class RoyalHanchman : Monster
    {
        public RoyalHanchman(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
        }
    }
    [HandledSNO(5395)]
    public class Returned : Monster
    {
        public Returned(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
        }
    }
    [HandledSNO(5397)]
    public class SkeletalWarrior : Monster
    {
        public SkeletalWarrior(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
        }
    }
    [HandledSNO(80652)]
    public class SkeletonKnee : Monster
    {
        public SkeletonKnee(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
            //30474
        }
    }
    [HandledSNO(5411)]
    public class SkeletalExecutioner : Monster
    {
        public SkeletalExecutioner(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
        }
    }
    [HandledSNO(434)]
    public class ReturnedExecutioner : Monster
    {
        public ReturnedExecutioner(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
        }
    }
    #endregion
    #region Skeleton_Necromantic_Minion
    //Necromantic Minion
    [HandledSNO(105863)]
    public class NecromanticMinion : Monster
    {
        public NecromanticMinion(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
        }
    }
    #endregion
    #region Skeleton_Summoner
    //No Uniques Added
    // Tomb Guardian -> All
    [HandledSNO(5387)]
    public class TombGuardian : Monster
    {
        public TombGuardian(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
            (Brain as MonsterBrain).AddPresetPower(30592);
            (Brain as MonsterBrain).AddPresetPower(30503);
            (Brain as MonsterBrain).AddPresetPower(30543); //Summon Skeletons
        }
    }
    #endregion
    #region Skeleton_Archer
    [HandledSNO(5346)]
    public class SkeletalArcher : Monster
    {
        public SkeletalArcher(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
        }
    }
    [HandledSNO(218400)]
    public class KillianDamort : Monster
    {
        public KillianDamort(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
            (Brain as MonsterBrain).AddPresetPower(30592);
        }
    }
    [HandledSNO(5347)]
    public class ReturnedArcher : Monster
    {
        public ReturnedArcher(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
            (Brain as MonsterBrain).AddPresetPower(30592);
        }
    }
    #endregion
    #region Shield_Skeleton
    [HandledSNO(5275)]
    public class SkeletalShieldBearer : Monster
    {
        public SkeletalShieldBearer(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
            (Brain as MonsterBrain).AddPresetPower(30592);
        }
    }
    [HandledSNO(5276)]
    public class ReturnedShieldMan : Monster
    {
        public ReturnedShieldMan(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
            (Brain as MonsterBrain).AddPresetPower(30592);
        }
    }
    [HandledSNO(5277)]
    public class SkeletalSentry : Monster
    {
        public SkeletalSentry(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
            (Brain as MonsterBrain).AddPresetPower(30592);
        }
    }
    #endregion
    #region Grotesque
    [HandledSNO(3847)]
    public class Grotesque : Monster
    {
        //3851 suicide blood, 220536 suicide imps = these happen on different SNOs and happen as they are dying.

        public Grotesque(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
            (Brain as MonsterBrain).AddPresetPower(30531);
            (Brain as MonsterBrain).AddPresetPower(30530);
            (Brain as MonsterBrain).AddPresetPower(30529); //Explode
        }
    }
    [HandledSNO(38484)]
    public class Harvester : Monster
    {
        //3851 suicide blood, 220536 suicide imps = these happen on different SNOs and happen as they are dying.

        public Harvester(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
        }
    }
    [HandledSNO(218307)]
    public class RagusGrimlow : Monster
    {
        //3851 suicide blood, 220536 suicide imps = these happen on different SNOs and happen as they are dying.

        public RagusGrimlow(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
        }
    }
    [HandledSNO(218308)]
    public class BralukGrimlow : Monster
    {
        //3851 suicide blood, 220536 suicide imps = these happen on different SNOs and happen as they are dying.

        public BralukGrimlow(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
        }
    }
    [HandledSNO(218405)]
    public class BellyBloatTheScarred : Monster
    {
        //3851 suicide blood, 220536 suicide imps = these happen on different SNOs and happen as they are dying.

        public BellyBloatTheScarred(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
        }
    }
    [HandledSNO(4564)]
    public class CorpseWorm : Monster
    {

        public CorpseWorm(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
        }
    }

    #endregion
    #region FleshPitFlyers
    [HandledSNO(4156)]
    public class CarrionBat : Monster
    {
        public CarrionBat(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
        }
    }
    [HandledSNO(4157, 81954)]
    public class PlagueCarrier : Monster
    {
        public PlagueCarrier(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
        }
    }
    [HandledSNO(218314)]
    public class Glidewing : Monster
    {
        public Glidewing(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
        }
    }
    [HandledSNO(218362)]
    public class Firestarter : Monster
    {
        public Firestarter(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
        }
    }
    [HandledSNO(195747)]
    public class VileHellbat : Monster
    {
        public VileHellbat(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
        }
    }
    #endregion
    #region CarrionNest
    [HandledSNO(4152)]
    public class CarrionNest : Monster
    {
        public CarrionNest(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
            (Brain as MonsterBrain).RemovePresetPower(30592);
            (Brain as MonsterBrain).AddPresetPower(117580);
        }

    }
    #endregion
    #region Wretched Mothers
    [HandledSNO(219725, 108444)] // ZombieFemale_A_TristramQuest_Unique.acr
    public class WretchedMother : Monster
    {
        public WretchedMother(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
            (Brain as MonsterBrain).RemovePresetPower(30592); // Only distance attack
            (Brain as MonsterBrain).AddPresetPower(94734);
            (Brain as MonsterBrain).AddPresetPower(110518);
            //this.Attributes[GameAttribute.Damage_Weapon_Min, 0] = 4f;
            //this.Attributes[GameAttribute.Damage_Weapon_Delta, 0] = 4f;
        }
    }
    #endregion
}