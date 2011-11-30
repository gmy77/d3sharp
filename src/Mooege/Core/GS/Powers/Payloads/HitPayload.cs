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
    public class HitPayload
    {
        public PowerContext Context;
        public Actor Target;
        public float TotalDamage;
        public DamageType DominantDamageType;
        public Dictionary<DamageType, float> ElementDamages;
        public bool IsCriticalHit;

        public bool AutomaticHitEffects = true;
        public Action<DeathPayload> OnDeath = null;

        public HitPayload(AttackPayload attackPayload, bool criticalHit, Actor target)
        {
            this.Context = attackPayload.Context;
            this.Target = target;
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
                    this.ElementDamages[entry.DamageType] += weaponMinDamage + (float)PowerContext.Rand.NextDouble() * weaponDamageDelta;
                else
                    this.ElementDamages[entry.DamageType] += entry.MinDamage + (float)PowerContext.Rand.NextDouble() * entry.DamageDelta;
            }

            // apply critical damage boost
            if (criticalHit)
            {
                // TODO: probably will calculate this off of GameAttribute.Crit_Damage_Percent, but right now that attribute is never set
                var damTypes = this.ElementDamages.Keys.ToArray();
                foreach (var type in damTypes)
                    this.ElementDamages[type] *= 1.0f + 0.25f;
            }

            // TODO: reduce element damage amounts according to target's resistances

            // TODO: reduce total damage by target's armor

            this.TotalDamage = this.ElementDamages.Sum(kv => kv.Value);
            this.DominantDamageType = this.ElementDamages.OrderByDescending(kv => kv.Value).FirstOrDefault().Key;
        }

        public void Apply()
        {
            // play damage number message
            if (this.Context.User is Player)
            {
                // TODO: handle player getting hit? maybe broadcast red message
                (this.Context.User as Player).InGameClient.SendMessage(new FloatingNumberMessage
                {
                    ActorID = this.Target.DynamicID,
                    Number = this.TotalDamage,
                    Type = this.IsCriticalHit ? FloatingNumberMessage.FloatType.WhiteCritical : FloatingNumberMessage.FloatType.White
                });
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

                // TODO: sound effects
                
                // play hit animation if the actor has one
                int hitAni = this.Target.AnimationSet.GetAniSNO(Mooege.Common.MPQ.FileFormats.AnimationTags.GetHit);
                if (hitAni != -1)
                {
                    this.Target.World.BroadcastIfRevealed(new PlayAnimationMessage()
                    {
                        ActorID = this.Target.DynamicID,
                        Field1 = 0x6,
                        Field2 = 0,
                        tAnim = new PlayAnimationMessageSpec[1]
                        {
                            new PlayAnimationMessageSpec()
                            {
                                Field0 = 40,  // HACK: harded animation length, we need to read these from .ani
                                Field1 = hitAni,
                                Field2 = 0x0,
                                Field3 = 1f  // TODO: vary this based on hit recovery
                            }
                        }
                    }, this.Target);
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

            // TODO: if target survives and it's a AI monster, give it aggro
        }
    }
}
