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
    #region TreasureGoblin
    //Unknown: Wrong Way! you aren't supposed to attack!
    [HandledSNO(5984, 5985, 5987, 5988)] //54862 is goblin portal
    public class TreasureGoblin : Monster
    {
        public TreasureGoblin(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
            this.Brain = new MonsterBrain(this);
            //this.Attributes[GameAttribute.Hitpoints_Max_Total] = 5f;
            this.Attributes[GameAttribute.Hitpoints_Max] = 5f;
            //this.Attributes[GameAttribute.Hitpoints_Total_From_Level] = 0f;
            this.Attributes[GameAttribute.Hitpoints_Cur] = 5f;
            this.Attributes[GameAttribute.Attacks_Per_Second] = 1.0f;
            this.Attributes[GameAttribute.Damage_Weapon_Min, 0] = 5f;
            this.Attributes[GameAttribute.Damage_Weapon_Delta, 0] = 7f;

        }
    }
    #endregion
    #region Spore
    //Unknown: These should spawn when Woodwraiths use that spell
    [HandledSNO(5482)]
    public class Spore : Monster
    {
        public Spore(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
            this.Brain = new MonsterBrain(this);
            //this.Attributes[GameAttribute.Hitpoints_Max_Total] = 5f;
            this.Attributes[GameAttribute.Hitpoints_Max] = 5f;
            //this.Attributes[GameAttribute.Hitpoints_Total_From_Level] = 0f;
            this.Attributes[GameAttribute.Hitpoints_Cur] = 5f;
            this.Attributes[GameAttribute.Attacks_Per_Second] = 1.0f;
            this.Attributes[GameAttribute.Damage_Weapon_Min, 0] = 5f;
            this.Attributes[GameAttribute.Damage_Weapon_Delta, 0] = 7f;

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
            this.Brain = new MonsterBrain(this);
            //this.Attributes[GameAttribute.Hitpoints_Max_Total] = 5f;
            this.Attributes[GameAttribute.Hitpoints_Max] = 5f;
            //this.Attributes[GameAttribute.Hitpoints_Total_From_Level] = 0f;
            this.Attributes[GameAttribute.Hitpoints_Cur] = 5f;
            this.Attributes[GameAttribute.Attacks_Per_Second] = 1.0f;
            this.Attributes[GameAttribute.Damage_Weapon_Min, 0] = 5f;
            this.Attributes[GameAttribute.Damage_Weapon_Delta, 0] = 7f;

        }
    }
    #endregion
    #region Ghost
    [HandledSNO(370, 136943)]
    public class Ghost : Monster
    {
        public Ghost(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
            this.Brain = new MonsterBrain(this);
            //this.Attributes[GameAttribute.Hitpoints_Max_Total] = 15f;
            this.Attributes[GameAttribute.Hitpoints_Max] = 15f;
            //this.Attributes[GameAttribute.Hitpoints_Total_From_Level] = 0f;
            this.Attributes[GameAttribute.Hitpoints_Cur] = 15f;
            this.Attributes[GameAttribute.Attacks_Per_Second] = 1.0f;
            this.Attributes[GameAttribute.Damage_Weapon_Min, 0] = 15f;
            this.Attributes[GameAttribute.Damage_Weapon_Delta, 0] = 7f;
        }
    }
    #endregion
    #region Unburied
    //Unknown: These guys dont want to move :)
    [HandledSNO(6356, 6359)]
    public class Unburied : Monster
    {
        public Unburied(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
            this.Brain = new MonsterBrain(this);
            //this.Attributes[GameAttribute.Hitpoints_Max_Total] = 5f;
            this.Attributes[GameAttribute.Hitpoints_Max] = 5f;
            //this.Attributes[GameAttribute.Hitpoints_Total_From_Level] = 0f;
            this.Attributes[GameAttribute.Hitpoints_Cur] = 5f;
            this.Attributes[GameAttribute.Attacks_Per_Second] = 1.0f;
            this.Attributes[GameAttribute.Damage_Weapon_Min, 0] = 5f;
            this.Attributes[GameAttribute.Damage_Weapon_Delta, 0] = 7f;

        }
    }
    #endregion
    #region WoodWraith
    //Unknown: Doesn't spawn them?
    [HandledSNO(6572, 139454, 139456, 170324, 170325, 495)]
    public class WoodWraith : Monster
    {
        public WoodWraith(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
            this.Brain = new MonsterBrain(this);
            //this.Attributes[GameAttribute.Hitpoints_Max_Total] = 5f;
            this.Attributes[GameAttribute.Hitpoints_Max] = 5f;
            //this.Attributes[GameAttribute.Hitpoints_Total_From_Level] = 0f;
            this.Attributes[GameAttribute.Hitpoints_Cur] = 5f;
            this.Attributes[GameAttribute.Attacks_Per_Second] = 1.0f;
            this.Attributes[GameAttribute.Damage_Weapon_Min, 0] = 5f;
            this.Attributes[GameAttribute.Damage_Weapon_Delta, 0] = 7f;

        }
    }
    #endregion
    #region Zombie
    //No Uniques Added
    [HandledSNO(6652, 6653, 6654, 204256, //Zombies
        6644, 6646, 6647, 6651)] //ZombieSkinny
    public class Zombie : Monster
    {
        public Zombie(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
            
            this.Brain = new MonsterBrain(this);
            //this.Attributes[GameAttribute.Hitpoints_Max_Total] = 5f;
            this.Attributes[GameAttribute.Hitpoints_Max] = 5f;
            //this.Attributes[GameAttribute.Hitpoints_Total_From_Level] = 0f;
            this.Attributes[GameAttribute.Hitpoints_Cur] = 5f;
            this.Attributes[GameAttribute.Attacks_Per_Second] = 1.0f;
            this.Attributes[GameAttribute.Damage_Weapon_Min, 0] = 5f;
            this.Attributes[GameAttribute.Damage_Weapon_Delta, 0] = 7f;
        }
    }
     #endregion
    #region Zombie_LeahInn
    [HandledSNO(203121)] //ZombieSkinny_A_LeahInn.acr
    public class InnZombie : Monster
    {
        public InnZombie(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
            this.Brain = new MonsterBrain(this);
            //this.Attributes[GameAttribute.Hitpoints_Max_Total] = 4.132813f;
            this.Attributes[GameAttribute.Hitpoints_Max] = 4.132813f;
            //this.Attributes[GameAttribute.Hitpoints_Total_From_Level] = 0f;
            this.Attributes[GameAttribute.Hitpoints_Cur] = 4.132813f;
            this.Attributes[GameAttribute.Attacks_Per_Second] = 1.0f;
            this.Attributes[GameAttribute.Damage_Weapon_Min, 0] = 4f; 
            this.Attributes[GameAttribute.Damage_Weapon_Delta, 0] = 4f; 
        }
    }
     #endregion
    #region ZombieCrawler
    [HandledSNO(218367)] //ZombieCrawler_Barricade_A.acr
    public class ZombieCrawler : Monster
    {
        public ZombieCrawler(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
            this.Brain = new MonsterBrain(this);
            //this.Attributes[GameAttribute.Hitpoints_Max_Total] = 5f;
            this.Attributes[GameAttribute.Hitpoints_Max] = 1.602539f;
            //this.Attributes[GameAttribute.Hitpoints_Total_From_Level] = 0f;
            this.Attributes[GameAttribute.Hitpoints_Cur] = 1.602539f;
            this.Attributes[GameAttribute.Attacks_Per_Second] = 1.0f;
            this.Attributes[GameAttribute.Damage_Weapon_Min, 0] = 4f; 
            this.Attributes[GameAttribute.Damage_Weapon_Delta, 0] = 4f; 
        }
    }
    #endregion
    #region ZombieCustom
    [HandledSNO(218339)] //ZombieSkinny_Custom_A.acr (2036596938)
    public class ZombieSkinny : Monster
    {
        public ZombieSkinny(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
            this.Brain = new MonsterBrain(this);
            //this.Attributes[GameAttribute.Hitpoints_Max_Total] = 4.132813f;
            this.Attributes[GameAttribute.Hitpoints_Max] = 4.132813f;
            //this.Attributes[GameAttribute.Hitpoints_Total_From_Level] = 0f;
            this.Attributes[GameAttribute.Hitpoints_Cur] = 4.132813f;
            this.Attributes[GameAttribute.Attacks_Per_Second] = 1.0f;
            this.Attributes[GameAttribute.Damage_Weapon_Min, 0] = 4f; 
            this.Attributes[GameAttribute.Damage_Weapon_Delta, 0] = 4f; 
        }
    }
    #endregion
    #region Skeleton
    //No Uniques Added
    [HandledSNO(5393, 87012, 5395, 5397, 80652, 5407, 5408, 5411, 434)]
    public class Skeleton : Monster
    {
        public Skeleton(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
            this.Brain = new MonsterBrain(this);
            //this.Attributes[GameAttribute.Hitpoints_Max_Total] = 5f;
            this.Attributes[GameAttribute.Hitpoints_Max] = 5f;
            //this.Attributes[GameAttribute.Hitpoints_Total_From_Level] = 0f;
            this.Attributes[GameAttribute.Hitpoints_Cur] = 5f;
            this.Attributes[GameAttribute.Attacks_Per_Second] = 1.0f;
            this.Attributes[GameAttribute.Damage_Weapon_Min, 0] = 5f;
            this.Attributes[GameAttribute.Damage_Weapon_Delta, 0] = 7f;
        }
    }
    #endregion
    #region Skeleton_TemplerIntro_NoWander
    [HandledSNO(105863)]
    public class Skeleton_TemplerIntro_NoWander : Monster
    {
        public Skeleton_TemplerIntro_NoWander(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
            this.Brain = new MonsterBrain(this);
            //this.Attributes[GameAttribute.Hitpoints_Max_Total] = 5f;
            this.Attributes[GameAttribute.Hitpoints_Max] = 5f;
            //this.Attributes[GameAttribute.Hitpoints_Total_From_Level] = 0f;
            this.Attributes[GameAttribute.Hitpoints_Cur] = 5f;
            this.Attributes[GameAttribute.Attacks_Per_Second] = 1.0f;
            this.Attributes[GameAttribute.Damage_Weapon_Min, 0] = 5f;
            this.Attributes[GameAttribute.Damage_Weapon_Delta, 0] = 7f;
        }
    }
    #endregion
    #region SkeletonSummoner
    //No Uniques Added
    [HandledSNO(5387)]
    public class SkeletonSummoner : Monster
    {
        public SkeletonSummoner(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
            this.Brain = new MonsterBrain(this);
            //this.Attributes[GameAttribute.Hitpoints_Max_Total] = 5f;
            this.Attributes[GameAttribute.Hitpoints_Max] = 5f;
            //this.Attributes[GameAttribute.Hitpoints_Total_From_Level] = 0f;
            this.Attributes[GameAttribute.Hitpoints_Cur] = 5f;
            this.Attributes[GameAttribute.Attacks_Per_Second] = 1.0f;
            this.Attributes[GameAttribute.Damage_Weapon_Min, 0] = 5f;
            this.Attributes[GameAttribute.Damage_Weapon_Delta, 0] = 7f;
        }
    }
    #endregion
    #region SkeletonArcher
    [HandledSNO(5346, 218400, 5347)]
    public class SkeletonArcher : Monster
    {
        public SkeletonArcher(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
            this.Brain = new MonsterBrain(this);
            //this.Attributes[GameAttribute.Hitpoints_Max_Total] = 5f;
            this.Attributes[GameAttribute.Hitpoints_Max] = 5f;
            //this.Attributes[GameAttribute.Hitpoints_Total_From_Level] = 0f;
            this.Attributes[GameAttribute.Hitpoints_Cur] = 5f;
            this.Attributes[GameAttribute.Attacks_Per_Second] = 1.0f;
            this.Attributes[GameAttribute.Damage_Weapon_Min, 0] = 5f;
            this.Attributes[GameAttribute.Damage_Weapon_Delta, 0] = 7f;
        }
    }
    #endregion
    #region ShieldSkeleton
    //No Uniques Added
    [HandledSNO(5275, 5276, 5277)]
    public class ShieldSkeletonSkeleton : Monster
    {
        public ShieldSkeletonSkeleton(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
            this.Brain = new MonsterBrain(this);
            //this.Attributes[GameAttribute.Hitpoints_Max_Total] = 5f;
            this.Attributes[GameAttribute.Hitpoints_Max] = 5f;
            //this.Attributes[GameAttribute.Hitpoints_Total_From_Level] = 0f;
            this.Attributes[GameAttribute.Hitpoints_Cur] = 5f;
            this.Attributes[GameAttribute.Attacks_Per_Second] = 1.0f;
            this.Attributes[GameAttribute.Damage_Weapon_Min, 0] = 5f;
            this.Attributes[GameAttribute.Damage_Weapon_Delta, 0] = 7f;
        }
    }
    #endregion
    #region Grotesque
    //No Uniques Added
    [HandledSNO(3847, 3848, 218307, 218308, 218405, 4564)]
    public class Grotesque : Monster
    {
        //3851 suicide blood, 220536 suicide imps = these happen on different SNOs and happen as they are dying.

        public Grotesque(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
            this.Brain = new MonsterBrain(this);
            //this.Attributes[GameAttribute.Hitpoints_Max_Total] = 5f;
            this.Attributes[GameAttribute.Hitpoints_Max] = 5f;
            //this.Attributes[GameAttribute.Hitpoints_Total_From_Level] = 0f;
            this.Attributes[GameAttribute.Hitpoints_Cur] = 5f;
            this.Attributes[GameAttribute.Attacks_Per_Second] = 1.0f;
            this.Attributes[GameAttribute.Damage_Weapon_Min, 0] = 5f;
            this.Attributes[GameAttribute.Damage_Weapon_Delta, 0] = 7f;
        }
    }
    #endregion
    #region FleshPitFlyers
    //No Uniques Added
    [HandledSNO(4156, 218314, 218362, 4157, 81954, 368, 195747)]
    public class FleshPitFlyers : Monster
    {
        public FleshPitFlyers(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
            this.Brain = new MonsterBrain(this);
            //this.Attributes[GameAttribute.Hitpoints_Max_Total] = 5f;
            this.Attributes[GameAttribute.Hitpoints_Max] = 5f;
            //this.Attributes[GameAttribute.Hitpoints_Total_From_Level] = 0f;
            this.Attributes[GameAttribute.Hitpoints_Cur] = 5f;
            this.Attributes[GameAttribute.Attacks_Per_Second] = 1.0f;
            this.Attributes[GameAttribute.Damage_Weapon_Min, 0] = 5f;
            this.Attributes[GameAttribute.Damage_Weapon_Delta, 0] = 7f;
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
            this.Brain = new MonsterBrain(this);
            //this.Attributes[GameAttribute.Hitpoints_Max_Total] = 13.38281f;
            this.Attributes[GameAttribute.Hitpoints_Max] = 13.38281f;
            //this.Attributes[GameAttribute.Hitpoints_Total_From_Level] = 0f;
            this.Attributes[GameAttribute.Hitpoints_Cur] = 13.38281f;
            this.Attributes[GameAttribute.Attacks_Per_Second] = 1.0f;
            this.Attributes[GameAttribute.Damage_Weapon_Min, 0] = 4f;
            this.Attributes[GameAttribute.Damage_Weapon_Delta, 0] = 6f;
            this.WalkSpeed = 0f; //We hardcode this so RumFord doesnt kill her before u even grab the quest...
        }
    }
    #endregion
}