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

            Vector3D inFrontOfUser = PowerMath.ProjectAndTranslate2D(User.Position, TargetPosition, User.Position, 7f);
            var bigtoad = SpawnEffect(109906, inFrontOfUser, TargetPosition, WaitInfinite());
            
            // HACK: holy hell there is alot of hardcoded animation timings here
            
            _PlayAni(bigtoad, 110766, 50); // spawn ani
            yield return WaitTicks(50);

            _PlayAni(bigtoad, 110520, 60); // attack ani
            TickTimer waitAttackEnd = WaitTicks(60);
            yield return WaitTicks(6 * 3); // wait for attack ani to play a bit

            var tongueTipProxy = SpawnProxy(TargetPosition, WaitInfinite());
            bigtoad.AddRopeEffect(107892, tongueTipProxy);

            // calculate time it will take for actors to reach toad
            const float moveSpeed = 3f;
            TickTimer waitMove = WaitTicks((int)(PowerMath.Distance(bigtoad.Position, TargetPosition) / moveSpeed));

            tongueTipProxy.TranslateNormal(bigtoad.Position, moveSpeed);
            if (Target != null && Target.World != null)
                Target.TranslateNormal(bigtoad.Position, moveSpeed);
            
            yield return waitMove;
            tongueTipProxy.Destroy();

            if (Target != null && Target.World != null)
            {
                _SetHiddenAttribute(Target, true);

                if (!waitAttackEnd.TimedOut)
                    yield return waitAttackEnd;

                _PlayAni(bigtoad, 110636, 60 * 5); // disgest ani, 5 seconds
                for (int n = 0; n < 5 && Target.World != null; ++n)
                {
                    WeaponDamage(Target, 0.39f, DamageType.Poison, true);
                    yield return WaitSeconds(1f);
                }

                if (Target.World != null)
                {
                    _SetHiddenAttribute(Target, false);

                    _PlayAni(bigtoad, 110637, 50); // regurgitate ani
                    Knockback(Target, 8f);
                    yield return WaitTicks(50);
                }
            }

            _PlayAni(bigtoad, 110764, 6 * 7); // despawn ani
            yield return WaitTicks(6 * 7);
            bigtoad.Destroy();
        }

        private void _SetHiddenAttribute(Actor actor, bool active)
        {
            actor.Attributes[GameAttribute.Hidden] = active;
            foreach (var msg in actor.Attributes.GetChangedMessageList(actor.DynamicID))
                World.BroadcastIfRevealed(msg, actor);
        }

        // hackish animation player until a centralized one can be made
        private void _PlayAni(Actor actor, int aniSNO, int aniTicks)
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
                        Field0 = aniTicks,
                        Field1 = aniSNO,
                        Field2 = 0x0,
                        Field3 = 1f
                    }
                }
            }, actor);
        }
    }
}
