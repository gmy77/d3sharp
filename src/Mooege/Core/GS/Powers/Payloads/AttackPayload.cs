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
using Mooege.Net.GS.Message;
using Mooege.Core.GS.Actors;

namespace Mooege.Core.GS.Powers.Payloads
{
    public class AttackPayload : Payload
    {
        // list of each amount and type of damage the attack will contain
        public class DamageEntry
        {
            public DamageType DamageType;

            public float MinDamage;
            public float DamageDelta;

            public bool IsWeaponBasedDamage;
            public float WeaponDamageMultiplier;
        }
        public List<DamageEntry> DamageEntries = new List<DamageEntry>();

        public Action<HitPayload> OnHit = null;
        public Action<DeathPayload> OnDeath = null;

        // some power's use custom hit effects, but they're not specified correctly in the tagmaps it seems
        // so this can be set to false to force off all hit effect generation from the payload
        public bool AutomaticHitEffects = true;

        private List<Actor> _targets = new List<Actor>();
        
        public AttackPayload(PowerContext context)
            : base(context, context.User)
        {
        }

        public void AddDamage(float minDamage, float damageDelta, DamageType damageType)
        {
            DamageEntries.Add(new DamageEntry
            {
                DamageType = damageType,
                MinDamage = minDamage,
                DamageDelta = damageDelta,
                IsWeaponBasedDamage = false,
            });
        }
        
        public void AddWeaponDamage(float damageMultiplier, DamageType damageType)
        {
            DamageEntries.Add(new DamageEntry
            {
                DamageType = damageType,
                IsWeaponBasedDamage = true,
                WeaponDamageMultiplier = damageMultiplier,
            });
        }

        public void AddTarget(Actor target)
        {
            _targets.Add(target);
        }

        public void AddTargets(IList<Actor> targets)
        {
            _targets.AddRange(targets);
        }

        public void ClearTargets()
        {
            _targets.Clear();
        }

        public void Apply()
        {
            this.Target.World.BuffManager.SendTargetPayload(this.Target, this);

            foreach (Actor target in _targets)
            {
                // filter null and killed targets
                if (target == null || target.World != null && target.World.PowerManager.IsDeletingActor(target))
                    continue;

                // TODO: calculate hit chance for monsters instead of always hitting

                var payload = new HitPayload(this, _DoCriticalHit(this.Context.User, target), target);
                payload.AutomaticHitEffects = this.AutomaticHitEffects;
                payload.OnDeath = OnDeath;

                if (OnHit != null)
                    OnHit(payload);

                payload.Apply();
            }
        }

        private static bool _DoCriticalHit(Actor user, Actor target)
        {
            if (target.Attributes[GameAttribute.Ignores_Critical_Hits])
                return false;

            // TODO: probably will calculate this based on GameAttribute.Crit_Percent_Base + GameAttribute.Crit_Percent_Bonus_Capped
            // right now those attributes aren't set though, so just do calc a generic 5% chance for now
            return PowerContext.Rand.NextDouble() < 0.05;
        }
    }
}
