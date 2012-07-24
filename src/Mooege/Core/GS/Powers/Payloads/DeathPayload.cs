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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mooege.Common.Logging;
using Mooege.Core.GS.Actors;
using Mooege.Net.GS.Message;
using Mooege.Net.GS.Message.Definitions.Misc;
using Mooege.Net.GS.Message.Definitions.Animation;
using Mooege.Net.GS.Message.Fields;
using Mooege.Core.GS.Players;
using Mooege.Core.GS.Common.Types.TagMap;
using Mooege.Net.GS.Message.Definitions.ACD;
using Mooege.Net.GS.Message.Definitions.Player;
using Mooege.Net.GS.Message.Definitions.Trade;

namespace Mooege.Core.GS.Powers.Payloads
{
    public class DeathPayload : Payload
    {
        public DamageType DeathDamageType;
        public bool LootAndExp; //HACK: As we currently just give out random exp and loot, this is in to prevent giving this out for mobs that shouldn't give it.

        public DeathPayload(PowerContext context, DamageType deathDamageType, Actor target, bool grantsLootAndExp = true)
            : base(context, target)
        {
            this.LootAndExp = grantsLootAndExp;
            this.DeathDamageType = deathDamageType;
        }

        public void Apply()
        {
            if (this.Target.World == null) return;

            if (this.Target is Player)
            {
                _DoSimplePlayerDeath();
                return;
            }
            
            // HACK: add to hackish list thats used to defer deleting actor and filter it from powers targetting
            this.Target.World.PowerManager.AddDeletingActor(this.Target);

            // kill brain if living
            if (this.Target is Living)
            {
                Living actor = (Living)this.Target;
                if (actor.Brain != null)
                    actor.Brain.Kill();
            }

            // kill brain if monster
            if (this.Target is Monster)
            {
                Monster mon = (Monster)this.Target;
                if (mon.Brain != null)
                    mon.Brain.Kill();
            }

            // send this death payload to buffs
            this.Target.World.BuffManager.SendTargetPayload(this.Target, this);

            // wtf is this?
            this.Target.World.BroadcastIfRevealed(new Mooege.Net.GS.Message.Definitions.Effect.PlayEffectMessage()
            {
                ActorId = this.Target.DynamicID,
                Effect = Mooege.Net.GS.Message.Definitions.Effect.Effect.Unknown12,
            }, this.Target);

            this.Target.World.BroadcastIfRevealed(new ANNDataMessage(Opcodes.ANNDataMessage13)
            {
                ActorID = this.Target.DynamicID
            }, this.Target);

            // play main death animation
            this.Target.PlayAnimation(11, _FindBestDeathAnimationSNO(), 1f, 2);

            this.Target.World.BroadcastIfRevealed(new ANNDataMessage(Opcodes.ANNDataMessage24)
            {
                ActorID = this.Target.DynamicID,
            }, this.Target);

            // remove all buffs and running powers before deleting actor
            this.Target.World.BuffManager.RemoveAllBuffs(this.Target);
            this.Target.World.PowerManager.CancelAllPowers(this.Target);

            this.Target.Attributes[GameAttribute.Deleted_On_Server] = true;
            this.Target.Attributes[GameAttribute.Could_Have_Ragdolled] = true;
            this.Target.Attributes.BroadcastChangedIfRevealed();

            // Spawn Random item and give exp for each player in range
            List<Player> players = this.Target.GetPlayersInRange(26f);
            foreach (Player plr in players)
            {
                plr.UpdateExp(this.Target.Attributes[GameAttribute.Experience_Granted]);
                this.Target.World.SpawnRandomItemDrop(this.Target, plr);
            }

            if (LootAndExp)
            {
                if (this.Context.User is Player)
                {
                    Player player = (Player)this.Context.User;

                    player.ExpBonusData.Update(player.GBHandle.Type, this.Target.GBHandle.Type);
                    this.Target.World.SpawnGold(this.Target, player);
                    if (Mooege.Common.Helpers.Math.RandomHelper.Next(1, 100) < 20)
                        this.Target.World.SpawnHealthGlobe(this.Target, player, this.Target.Position);
                }
            }

            if (this.Target is Monster)
                (this.Target as Monster).PlayLore();

            // HACK: instead of deleting actor right here, its added to a list (near the top of this function)
            //this.Target.Destroy();
        }

        private void _DoSimplePlayerDeath()
        {
            // HACK: simple death implementation
            this.Target.World.BuffManager.RemoveAllBuffs(this.Target);
            this.Target.World.PowerManager.CancelAllPowers(this.Target);

            this.Target.World.BuffManager.AddBuff(this.Target, this.Target, new Implementations.ActorGhostedBuff());

            Player player = (Player)this.Target;
            player.Teleport(player.CheckPointPosition);
            player.AddPercentageHP(100);
        }

        private int _FindBestDeathAnimationSNO()
        {
            // check if power has special death animation, and roll chance to use it
            TagKeyInt specialDeathTag = _GetTagForSpecialDeath(this.Context.EvalTag(PowerKeys.SpecialDeathType));
            if (specialDeathTag != null)
            {
                float specialDeathChance = this.Context.EvalTag(PowerKeys.SpecialDeathChance);
                if (PowerContext.Rand.NextDouble() < specialDeathChance)
                {
                    int specialSNO = _GetSNOFromTag(specialDeathTag);
                    if (specialSNO != -1)
                        return specialSNO;
                }
                // decided not to use special death or actor doesn't have it, just fall back to normal death anis
            }

            int sno = _GetSNOFromTag(this.DeathDamageType.DeathAnimationTag);
            if (sno != -1)
                return sno;

            // load default ani if all else fails
            return _GetSNOFromTag(AnimationSetKeys.DeathDefault);
        }

        private int _GetSNOFromTag(TagKeyInt tag)
        {
            if (this.Target.AnimationSet != null && this.Target.AnimationSet.TagMapAnimDefault.ContainsKey(tag))
                return this.Target.AnimationSet.TagMapAnimDefault[tag];
            else
                return -1;
        }

        private static TagKeyInt _GetTagForSpecialDeath(int specialDeathType)
        {
            switch (specialDeathType)
            {
                default: return null;
                case 1: return AnimationSetKeys.DeathDisintegration;
                case 2: return AnimationSetKeys.DeathPulverise;
                case 3: return AnimationSetKeys.DeathPlague;
                case 4: return AnimationSetKeys.DeathDismember;
                case 5: return AnimationSetKeys.DeathDecap;
                case 6: return AnimationSetKeys.DeathAcid;
                case 7: return AnimationSetKeys.DeathLava;  // haven't seen lava used, but there's no other place for it
                case 8: return AnimationSetKeys.DeathSpirit;
            }
        }
    }
}
