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
using Mooege.Core.GS.Powers;
using Mooege.Core.GS.Ticker;
using Mooege.Core.GS.Common.Types.TagMap;
using Mooege.Core.GS.Actors.Movement;
using Mooege.Net.GS.Message;
using Mooege.Core.GS.Common.Types.Math;
using Mooege.Net.GS.Message.Definitions.ACD;

namespace Mooege.Core.GS.Actors.Actions
{
    public class PowerAction : ActorAction
    {
        const float MaxTargetRange = 60f;
        const float PathUpdateDelay = 1f;

        private Actor _target;
        private PowerScript _power;
        private bool _powerRan;
        private TickTimer _powerFinishTimer;
        private float _baseAttackRadius;
        private ActorMover _ownerMover;
        private TickTimer _pathUpdateTimer;

        public PowerAction(Actor owner, int powerSNO, Actor target = null)
            : base(owner)
        {
            _power = PowerLoader.CreateImplementationForPowerSNO(powerSNO);
            _power.World = owner.World;
            _power.User = owner;
            _powerRan = false;
            _baseAttackRadius = this.Owner.ActorData.Cylinder.Ax2 + _power.EvalTag(PowerKeys.AttackRadius) + 1.5f;
            _ownerMover = new ActorMover(owner);
            _target = target;
        }

        public override void Start(int tickCounter)
        {
            this.Started = true;
            this.Update(tickCounter);
        }

        public override void Update(int tickCounter)
        {
            // if power executed, wait for attack/cooldown to finish.
            if (_powerRan)
            {
                if (_powerFinishTimer.TimedOut)
                    this.Done = true;

                return;
            }

            // try to get nearest target if no target yet acquired
            if (_target == null)
            {
                if (this.Owner is Minion) // assume minions are player controlled and are targeting monsters
                {
                    _target = this.Owner.GetMonstersInRange(MaxTargetRange).OrderBy(
                        (monster) => PowerMath.Distance2D(monster.Position, this.Owner.Position))
                        .FirstOrDefault();
                }
                else  // monsters targeting players
                {
                    _target = this.Owner.GetPlayersInRange(MaxTargetRange).OrderBy(
                        (player) => PowerMath.Distance2D(player.Position, this.Owner.Position))
                        .FirstOrDefault();
                }
            }

            if (_target != null)
            {
                float targetDistance = PowerMath.Distance2D(_target.Position, this.Owner.Position);

                // if target has moved out of range, deselect it as the target
                if (targetDistance > MaxTargetRange)
                {
                    _target = null;
                }
                else if (targetDistance < _baseAttackRadius + _target.ActorData.Cylinder.Ax2)  // run power if within range
                {
                    // stop any movement
                    _ownerMover.Move(this.Owner.Position, this.Owner.WalkSpeed);

                    this.Owner.World.PowerManager.RunPower(this.Owner, _power, _target, _target.Position);
                    _powerFinishTimer = new SecondsTickTimer(this.Owner.World.Game,
                        _power.EvalTag(PowerKeys.AttackSpeed) + _power.EvalTag(PowerKeys.CooldownTime));
                    _powerRan = true;
                }
                else
                {
                    // update or create path movement
                    if (_pathUpdateTimer == null || _pathUpdateTimer.TimedOut)
                    {
                        _pathUpdateTimer = new SecondsTickTimer(this.Owner.World.Game, PathUpdateDelay);

                        // move the space between each path update, or up to target.
                        float moveAmount = this.Owner.WalkSpeed *
                                           (_pathUpdateTimer.TimeoutTick - this.Owner.World.Game.TickCounter);
                        if (targetDistance < moveAmount)
                            moveAmount = targetDistance; // -(_baseAttackRadius + _target.ActorData.Cylinder.Ax2) - 0.1f;
                        Vector3D movePos = PowerMath.TranslateDirection2D(this.Owner.Position, _target.Position,
                                                                          this.Owner.Position, moveAmount);

                        this.Owner.TranslateFacing(_target.Position, false);

                        // find suitable movement animation
                        int aniTag;
                        if (this.Owner.AnimationSet == null)
                            aniTag = -1;
                        else if (this.Owner.AnimationSet.TagExists(Mooege.Common.MPQ.FileFormats.AnimationTags.Walk))
                            aniTag = this.Owner.AnimationSet.GetAnimationTag(Mooege.Common.MPQ.FileFormats.AnimationTags.Walk);
                        else if (this.Owner.AnimationSet.TagExists(Mooege.Common.MPQ.FileFormats.AnimationTags.Run))
                            aniTag = this.Owner.AnimationSet.GetAnimationTag(Mooege.Common.MPQ.FileFormats.AnimationTags.Run);
                        else
                            aniTag = -1;

                        _ownerMover.Move(movePos, this.Owner.WalkSpeed, new ACDTranslateNormalMessage
                        {
                            TurnImmediately = false,
                            AnimationTag = aniTag
                        });
                    }
                    else
                    {
                        _ownerMover.Update();
                    }
                }
            }
        }

        public override void Cancel(int tickCounter)
        {
            // TODO: make this per-power instead?
            if (_powerRan)
                this.Owner.World.PowerManager.CancelAllPowers(this.Owner);

            this.Done = true;
        }
    }
}
