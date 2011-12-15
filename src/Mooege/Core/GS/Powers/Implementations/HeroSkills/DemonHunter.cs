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
using Mooege.Core.GS.Ticker;
using Mooege.Net.GS.Message.Definitions.Effect;
using Mooege.Core.GS.Common.Types.Math;
using Mooege.Core.GS.Powers.Payloads;

namespace Mooege.Core.GS.Powers.Implementations
{
    [ImplementsPowerSNO(Skills.Skills.DemonHunter.HatredGenerators.Grenades)]
    public class DemonHunterGrenades : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            GeneratePrimaryResource(ScriptFormula(25));

            float targetDistance = PowerMath.Distance2D(User.Position, TargetPosition);
            
            // create grenade projectiles with shared detonation timer
            TickTimer timeout = WaitSeconds(ScriptFormula(2));
            Projectile[] grenades = new Projectile[Rune_C > 0 ? 1 : 3];
            for (int i = 0; i < grenades.Length; ++i)
            {
                var projectile = new Projectile(this, Rune_C > 0 ? 212547 : 88244, User.Position);
                projectile.Timeout = timeout;
                grenades[i] = projectile;
            }

            // generate spread positions with distance-scaled spread amount.
            float scaledSpreadOffset = Math.Max(targetDistance - ScriptFormula(14), 0f);
            Vector3D[] projDestinations = PowerMath.GenerateSpreadPositions(User.Position, TargetPosition,
                ScriptFormula(11) - scaledSpreadOffset, grenades.Length);

            // launch and bounce grenades
            yield return WaitTicks(1);  // helps make bounce timings more consistent

            float bounceOffset = 1f;
            float minHeight = ScriptFormula(21);
            float height = minHeight + ScriptFormula(22);
            float bouncePercent = 0.7f; // ScriptFormula(23);
            while (!timeout.TimedOut)
            {
                for (int i = 0; i < grenades.Length; ++i)
                {
                    grenades[i].LaunchArc(PowerMath.ProjectAndTranslate2D(projDestinations[i], User.Position, projDestinations[i],
                                                                          targetDistance * 0.3f * bounceOffset),
                                          height, ScriptFormula(20));
                }

                height *= bouncePercent;
                bounceOffset *= 0.3f;

                yield return grenades[0].ArrivalTime;
                // play "dink dink" grenade bounce sound
                grenades[0].PlayEffect(Effect.Unknown69);
            }

            // damage effects
            for (int i = 0; i < grenades.Length; ++i)
            {
                SpawnEffect(RuneSelect(154027, 154045, 154028, 154044, 154046, 154043), grenades[i].Position);
                if (Rune_A > 0)
                    SpawnEffect(154076, grenades[i].Position, 0, WaitSeconds(ScriptFormula(7)));

                AttackPayload attack = new AttackPayload(this);
                attack.Targets = GetEnemiesInRadius(grenades[i].Position, ScriptFormula(4));
                attack.AddWeaponDamage(ScriptFormula(0), Rune_A > 0 ? DamageType.Poison : DamageType.Fire);
                attack.OnHit = (hitPayload) =>
                {
                    if (Rune_E > 0)
                    {
                        if (Rand.NextDouble() < ScriptFormula(9))
                            AddBuff(hitPayload.Target, new DebuffStunned(WaitSeconds(ScriptFormula(10))));
                    }
                    if (Rune_C > 0)
                        Knockback(grenades[i].Position, hitPayload.Target, ScriptFormula(8));
                };
                attack.Apply();
            }

            // TODO replace with better pool effect system
            // poison pool effect
            if (Rune_A > 0)
            {
                TickTimer cloudTimeout = WaitSeconds(ScriptFormula(7));
                while (!cloudTimeout.TimedOut)
                {
                    for (int i = 0; i < grenades.Length; ++i)
                    {
                        WeaponDamage(GetEnemiesInRadius(grenades[i].Position, ScriptFormula(5)), ScriptFormula(6), DamageType.Poison);
                    }
                    yield return WaitSeconds(1f);
                }
            }

            // clusterbomb hits
            if (Rune_B > 0)
            {
                int damagePulses = (int)ScriptFormula(28);
                for (int pulse = 0; pulse < damagePulses; ++pulse)
                {
                    yield return WaitSeconds(ScriptFormula(12) / damagePulses);

                    for (int i = 0; i < grenades.Length; ++i)
                    {
                        WeaponDamage(GetEnemiesInRadius(grenades[i].Position, ScriptFormula(4)), ScriptFormula(0), DamageType.Fire);
                    }
                }
            }
        }
    }

    [ImplementsPowerSNO(Skills.Skills.DemonHunter.HatredSpenders.RainOfVengeance)]
    public class DemonHunterRainOfVengeance : Skill
    {
        public override IEnumerable<TickTimer> Main()
        {
            gok();
            yield break;
        }

        private void gok()
        {
            SpawnEffect(149949, TargetPosition, -1);
        }
    }
}
