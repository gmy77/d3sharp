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
using Mooege.Core.GS.Ticker.Helpers;
using Mooege.Net.GS.Message.Definitions.Animation;
using Mooege.Net.GS.Message.Fields;
using Mooege.Core.GS.Actors;
using Mooege.Core.GS.Common.Types.Math;
using Mooege.Net.GS.Message;

namespace Mooege.Core.GS.Powers.Implementations
{
    [ImplementsPowerSNO(Skills.Skills.WitchDoctor.PhysicalRealm.PoisonDart)]
    public class WitchDoctorPoisonDart : PowerImplementation
    {
        public override IEnumerable<TickTimer> Run()
        {
            var eff = SpawnProxy(TargetPosition);
            eff.PlayEffectGroup(106365);
            yield break;

        }
    }

    [ImplementsPowerSNO(Skills.Skills.WitchDoctor.PhysicalRealm.PlagueOfToads)]
    public class WitchDoctorPlagueOfToads : PowerImplementation
    {
        public override IEnumerable<TickTimer> Run()
        {
            // NOTE: not normal plague of toads right now but Obsidian runed "Toad of Hugeness"

            Vector3D userCastPosition = new Vector3D(User.Position);
            Vector3D inFrontOfUser = PowerMath.ProjectAndTranslate2D(User.Position, TargetPosition, User.Position, 7f);
            var bigtoad = SpawnEffect(109906, inFrontOfUser, TargetPosition, WaitInfinite());
            
            // HACK: holy hell there is alot of hardcoded animation timings here
            
            _PlayAni(bigtoad, 110766); // spawn ani
            yield return WaitSeconds(1f);

            _PlayAni(bigtoad, 110520); // attack ani
            TickTimer waitAttackEnd = WaitSeconds(1.5f);
            yield return WaitSeconds(0.3f); // wait for attack ani to play a bit

            var tongueEnd = SpawnProxy(TargetPosition, WaitInfinite());
            bigtoad.AddRopeEffect(107892, tongueEnd);

            // calculate time it will take for actors to reach toad
            const float tongueSpeed = 4f;
            int waitMoveTicks = (int)(PowerMath.Distance(bigtoad.Position, TargetPosition) / tongueSpeed);

            yield return WaitSeconds(0.3f); // have tongue hang there for a bit

            tongueEnd.TranslateNormal(bigtoad.Position, tongueSpeed);
            if (ValidTarget())
                Target.TranslateNormal(bigtoad.Position, tongueSpeed);

            yield return WaitTicks(waitMoveTicks);
            tongueEnd.Destroy();

            if (ValidTarget())
            {
                _SetHiddenAttribute(Target, true);

                if (!waitAttackEnd.TimedOut)
                    yield return waitAttackEnd;

                _PlayAni(bigtoad, 110636); // disgest ani, 5 seconds
                for (int n = 0; n < 5 && ValidTarget(); ++n)
                {
                    WeaponDamage(Target, 0.039f, DamageType.Poison, true);
                    yield return WaitSeconds(1f);
                }

                if (ValidTarget())
                {
                    _SetHiddenAttribute(Target, false);

                    _PlayAni(bigtoad, 110637); // regurgitate ani
                    Knockback(Target, userCastPosition, 6f);
                    Target.PlayEffectGroup(18281); // actual regurgitate efg isn't working so use generic acid effect
                    yield return WaitSeconds(0.9f);
                }
            }

            _PlayAni(bigtoad, 110764); // despawn ani
            yield return WaitSeconds(0.7f);
            bigtoad.Destroy();
        }

        private void _SetHiddenAttribute(Actor actor, bool active)
        {
            actor.Attributes[GameAttribute.Hidden] = active;
            foreach (var msg in actor.Attributes.GetChangedMessageList(actor.DynamicID))
                World.BroadcastIfRevealed(msg, actor);
        }

        // hackish animation player until a centralized one can be made
        private void _PlayAni(Actor actor, int aniSNO)
        {
            World.BroadcastIfRevealed(new PlayAnimationMessage()
            {
                ActorID = actor.DynamicID,
                Field1 = 0x3,
                Field2 = 0,
                tAnim = new PlayAnimationMessageSpec[1]
                {
                    new PlayAnimationMessageSpec()
                    {
                        Field0 = -2,
                        Field1 = aniSNO,
                        Field2 = 0x0,
                        Field3 = 1f
                    }
                }
            }, actor);
        }
    }
}
