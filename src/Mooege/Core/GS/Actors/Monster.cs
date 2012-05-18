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
using System.Linq;
using Mooege.Common.Helpers.Math;
using Mooege.Core.GS.AI.Brains;
using Mooege.Core.GS.Map;
using Mooege.Core.GS.Objects;
using Mooege.Core.GS.Players;
using Mooege.Net.GS.Message;
using Mooege.Net.GS.Message.Definitions.World;
using Mooege.Net.GS.Message.Fields;
using Mooege.Net.GS.Message.Definitions.Animation;
using Mooege.Net.GS.Message.Definitions.Effect;
using Mooege.Net.GS.Message.Definitions.Misc;
using Mooege.Core.GS.Common.Types.TagMap;
using MonsterFF = Mooege.Common.MPQ.FileFormats.Monster;
using GameBalance = Mooege.Common.MPQ.FileFormats.GameBalance;
using Mooege.Core.GS.Common.Types.SNO;

namespace Mooege.Core.GS.Actors
{
    public class Monster : Living, IUpdateable
    {
        public override ActorType ActorType { get { return ActorType.Monster; } }

        public override int Quality
        {
            get
            {
                return (int)Mooege.Common.MPQ.FileFormats.SpawnType.Normal;
            }
            set
            {
                // TODO MonsterQuality setter not implemented. Throwing a NotImplementedError is catched as message not beeing implemented and nothing works anymore...
            }
        }

        public int LoreSNOId
        {
            get
            {
                return Monster.IsValid ? (Monster.Target as MonsterFF).SNOLore : -1;
            }
        }

        /// <summary>
        /// Gets the Actors summoning fields from the mpq's and returns them in format for Monsters.
        /// Useful for the Monsters spawning/summoning skills.
        /// </summary>
        public int[] SNOSummons
        {
            get
            {
                return (Monster.Target as MonsterFF).SNOSummonActor;
            }
        }

        public Monster(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
            this.Field2 = 0x8;
            this.GBHandle.Type = (int)GBHandleType.Monster; this.GBHandle.GBID = 1;
            this.Attributes[GameAttribute.Experience_Granted] = 125;

            // lookup GameBalance MonsterLevels.gam asset
            var monsterLevels = (GameBalance)Mooege.Common.MPQ.MPQStorage.Data.Assets[SNOGroup.GameBalance][19760].Data;
            var monsterData = (Monster.Target as MonsterFF);

            // always use normal difficulty levels for now
            if (monsterData.Level.Normal >= 0 && monsterData.Level.Normal < monsterLevels.MonsterLevel.Count)
            {
                this.Brain = new MonsterBrain(this);
                this.Attributes[GameAttribute.Level] = monsterData.Level.Normal;
                this.Attributes[GameAttribute.Hitpoints_Max] = monsterLevels.MonsterLevel[monsterData.Level.Normal].F0;
                this.Attributes[GameAttribute.Hitpoints_Cur] = this.Attributes[GameAttribute.Hitpoints_Max_Total];
                this.Attributes[GameAttribute.Attacks_Per_Second] = 1.0f;
                this.Attributes[GameAttribute.Damage_Weapon_Min, 0] = 5f;
                this.Attributes[GameAttribute.Damage_Weapon_Delta, 0] = 5f;
                this.WalkSpeed = monsterData.Floats[129];  // TODO: this is probably multiplied by something
            }
        }

        public override void OnTargeted(Player player, TargetMessage message)
        {
        }

        public void Update(int tickCounter)
        {
            if (this.Brain == null)
                return;

            this.Brain.Update(tickCounter);
        }

        /// <summary>
        /// Plays lore for first death of this monster's death.
        /// </summary>
        public void PlayLore()
        {
            if (LoreSNOId != -1)
            {
                var players = this.GetPlayersInRange();
                if (players != null)
                {
                    foreach (var player in players.Where(player => !player.HasLore(LoreSNOId)))
                    {
                        player.PlayLore(LoreSNOId, false);
                    }
                }
            }
        }
    }
}
