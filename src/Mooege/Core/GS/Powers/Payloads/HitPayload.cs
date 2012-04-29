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
using Mooege.Core.GS.Actors;
using Mooege.Net.GS.Message;
using Mooege.Core.GS.Players;
using Mooege.Net.GS.Message.Definitions.Misc;
using Mooege.Core.GS.Common.Types.TagMap;
using Mooege.Net.GS.Message.Definitions.Animation;
using Mooege.Net.GS.Message.Fields;

namespace Mooege.Core.GS.Powers.Payloads
{
    public class HitPayload : Payload
    {
        public float TotalDamage;
        public DamageType DominantDamageType;
        public Dictionary<DamageType, float> ElementDamages;
        public bool IsCriticalHit;

        public bool AutomaticHitEffects = true;
        public Action<DeathPayload> OnDeath = null;

        public HitPayload(AttackPayload attackPayload, bool criticalHit, Actor target)
            : base(attackPayload.Context, target)
        {
            this.IsCriticalHit = criticalHit;
            
            // TODO: select these values based on element type?
            float weaponMinDamage = this.Context.User.Attributes[GameAttribute.Damage_Weapon_Min_Total, 0];
            float weaponDamageDelta = this.Context.User.Attributes[GameAttribute.Damage_Weapon_Delta_Total, 0];

            // calculate and add up damage amount for each element type
            this.ElementDamages = new Dictionary<DamageType, float>();

            foreach (var entry in attackPayload.DamageEntries)
            {
                if (!this.ElementDamages.ContainsKey(entry.DamageType))
                    this.ElementDamages[entry.DamageType] = 0f;

                if (entry.IsWeaponBasedDamage)
                    this.ElementDamages[entry.DamageType] += entry.WeaponDamageMultiplier *
                        (weaponMinDamage + (float)PowerContext.Rand.NextDouble() * weaponDamageDelta);
                else
                    this.ElementDamages[entry.DamageType] += entry.MinDamage + (float)PowerContext.Rand.NextDouble() * entry.DamageDelta;

                this.ElementDamages[entry.DamageType] *= 1.0f + this.Target.Attributes[GameAttribute.Amplify_Damage_Percent];
            }

            // apply critical damage boost
            if (criticalHit)
            {
                // TODO: probably will calculate this off of GameAttribute.Crit_Damage_Percent, but right now that attribute is never set
                var damTypes = this.ElementDamages.Keys.ToArray();
                foreach (var type in damTypes)
                    this.ElementDamages[type] *= 1.5f + this.Target.Attributes[GameAttribute.Crit_Percent_Bonus_Capped];
            }



            // TODO: reduce element damage amounts according to target's resistances

            // TODO: reduce total damage by target's armor
            // ~weltmeyer Using WOW Calculation till we find the correct formula :)


            this.TotalDamage = this.ElementDamages.Sum(kv => kv.Value) ;
            var targetArmor = target.Attributes[GameAttribute.Armor_Total];
            var attackerLevel = attackPayload.Context.User.Attributes[GameAttribute.Level];
            var reduction = TotalDamage * (0.1f * targetArmor) /
                               ((8.5f * attackerLevel) + 40);

            reduction /= 1+reduction;
            reduction = Math.Min(0.75f, reduction);
            this.TotalDamage = TotalDamage*(1 - reduction);

            this.DominantDamageType = this.ElementDamages.OrderByDescending(kv => kv.Value).FirstOrDefault().Key;
            if (this.DominantDamageType == null)
                this.DominantDamageType = DamageType.Physical; // default to physical if no other damage type calced
        }

        public void Apply()
        {
            if (this.Target.World != null)
                this.Target.World.BuffManager.SendTargetPayload(this.Target, this);

            // floating damage number
            if (this.Target.World != null)
            {
                this.Target.World.BroadcastIfRevealed(new FloatingNumberMessage
                {
                    ActorID = this.Target.DynamicID,
                    Number = this.TotalDamage,
                    // make player damage red, all other damage white
                    Type = this.IsCriticalHit ? 
                        (this.Target is Player) ? FloatingNumberMessage.FloatType.RedCritical : FloatingNumberMessage.FloatType.Golden
                                              :
                        (this.Target is Player) ? FloatingNumberMessage.FloatType.Red : FloatingNumberMessage.FloatType.White
                }, this.Target);
            }

            if (this.AutomaticHitEffects)
            {
                // play override hit effect it power context has one
                if (this.Context.EvalTag(PowerKeys.OverrideHitEffects) > 0)
                {
                    int efg = this.Context.EvalTag(PowerKeys.HitEffect);
                    if (efg != -1)
                        this.Target.PlayEffectGroup(efg);
                }
                else
                {
                    this.Target.PlayHitEffect((int)this.DominantDamageType.HitEffect, this.Context.User);
                }

                if (this.TotalDamage > 0f)
                {
                    // play override hitsound if any, otherwise just default to playing metal weapon hit for now
                    int overridenSound = this.Context.EvalTag(PowerKeys.HitsoundOverride);
                    int hitsound = overridenSound != -1 ? overridenSound : 1;
                    if (hitsound > 0)
                        this.Target.PlayEffect(Net.GS.Message.Definitions.Effect.Effect.Hit, hitsound);
                }
            }

            // TODO: critical hit special element buff/effects

            // update hp
            float new_hp = Math.Max(this.Target.Attributes[GameAttribute.Hitpoints_Cur] - this.TotalDamage, 0f);
            this.Target.Attributes[GameAttribute.Hitpoints_Cur] = new_hp;
            this.Target.Attributes.BroadcastChangedIfRevealed();

            // if hp=0 do death
            if (new_hp == 0f)
            {
                var deathload = new DeathPayload(this.Context, this.DominantDamageType, this.Target);
                if (OnDeath != null)
                    OnDeath(deathload);

                deathload.Apply();
            }
            else if (this.AutomaticHitEffects && this.Target.World != null)
            {
                // HACK: reduce hit effect rate for Player to 10% so they don't get perma hit-sun locked.
                if (this.Target is Player && PowerContext.Rand.NextDouble() < 0.9)
                    return;

                // target didn't die, so play hit animation if the actor has one
                if (this.Target.World.BuffManager.GetFirstBuff<Implementations.KnockbackBuff>(this.Target) == null &&
                    this.Target.AnimationSet != null)
                {
                    if (this.Target.AnimationSet.TagMapAnimDefault.ContainsKey(AnimationSetKeys.GetHit))
                    {
                        int hitAni = this.Target.AnimationSet.TagMapAnimDefault[AnimationSetKeys.GetHit];
                        if (hitAni != -1)
                        {
                            // HACK: hardcoded animation speed/ticks, need to base those off hit recovery speed
                            this.Target.PlayAnimation(6, hitAni, 1.0f, 40);
                        }
                    }
                }
            }

            // TODO: if target survives and it's a AI monster, give it aggro
        }
    }
}
