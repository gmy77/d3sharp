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
using System.Linq;
using Mooege.Core.GS.Actors;
using Mooege.Core.GS.Common.Types.Math;
using Mooege.Core.GS.Map;
using Mooege.Net.GS.Message.Definitions.ACD;
using Mooege.Core.GS.Common.Types.Misc;
using Mooege.Core.GS.Ticker.Helpers;
using Mooege.Core.GS.Objects;

namespace Mooege.Core.GS.Powers
{
    public class PowerProjectile : Actor, IUpdateable
    {
        //Define actor type
        public override ActorType ActorType { get { return ActorType.Projectile; } }

        //Gametick value at projectile creation
        public float creationTick;

        //Max time in ms the projectile is allowed to live
        public float timetolive;

        //Velocity vector of the projectile
        public Vector3D velocity;

        //Range error that will be used to calculate collision
        public float collisionError;

        //Actor hitted by the projectile
        public Actor hittedActor;

        //Actor initial position
        public Vector3D startingPosition;

        //Last known posistion of the projectile
        public Vector3D currentPosition;

        public float speed;

        //Radian angle between pos and aimpos
        public float radianAngle;

        public Action OnHit;
        public Func<bool> OnUpdate;
        public Action OnTimeout;
        public TickTimer Timeout;

        public PowerProjectile(World world, int actorSNO, Vector3D position, Vector3D aimPosition, float speed,
                               float timetolive, float scale = 1.35f, float collisionError = 3f,
                               float heightOffset = 0, float distanceOffset = 0, bool handleTranslation = false)
            : base(world, actorSNO)
        {
            this.startingPosition = new Vector3D(position);
            this.startingPosition.Z += heightOffset;
            this.speed = speed;
            this.timetolive = timetolive;
            this.collisionError = collisionError + heightOffset;

            Timeout = new TickSecondsTimer(this.World.Game, timetolive / 1000f);

            //Save projectile creation tick
            this.creationTick = this.World.Game.TickCounter;

            // FIXME: This is hardcoded crap
            this.Field2 = 0x8;
            this.Field3 = 0x0;
            this.Scale = 1f;
            this.GBHandle.Type = (int)GBHandleType.Monster; this.GBHandle.GBID = 1;
            this.Field7 = 0x00000001;
            this.Field8 = this.SNOId;
            this.Field10 = 0x1;
            this.Field11 = 0x1;
            this.Field12 = 0x1;
            this.Field13 = 0x1;
            this.CollFlags = 0x4;

            //Calculate Quaternion info
            this.radianAngle = PowerMath.AngleLookAt(startingPosition, aimPosition);

            //Assign quaternion info
            this.RotationAmount = (float)Math.Cos(this.radianAngle / 2);
            this.RotationAxis.X = 0f;
            this.RotationAxis.Y = 0f;
            this.RotationAxis.Z = (float)Math.Sin(this.radianAngle / 2);

            //Normalize position / aimPosition Vector
            Vector3D vel_normal = PowerMath.Normalize(new Vector3D(aimPosition.X - startingPosition.X,
                                                                   aimPosition.Y - startingPosition.Y,
                                                                   0f));
            this.velocity = new Vector3D(vel_normal.X * speed, vel_normal.Y * speed, vel_normal.Z * speed);                        

            //Adjust projectile distance from player
            this.startingPosition.X += this.velocity.X * distanceOffset;
            this.startingPosition.Y += this.velocity.Y * distanceOffset;

            this.Position = this.startingPosition;
            this.World.Enter(this); // Enter only once all fields have been initialized to prevent a run condition

            //If the creator dont specify he want to manipulate the projectil itself, launch it
            if (!handleTranslation)
                this.launch();
        }

        //Check if the projectile is close enough to trigger a collision with an actor
        public bool detectCollision()
        {
            //Get projectile age
            float delta_tick = this.World.Game.TickCounter - this.creationTick;

            //Is it time to dispose of the projectile ?
            if ((delta_tick / 6f > this.timetolive / 100f) && this.World != null) { this.Destroy(); return false; }

            //Get Actor
            var curPos = getCurrentPosition();
            Actor victim = this.World.QuadTree.Query<Monster>(new Circle(curPos.X, curPos.Y, this.collisionError)).FirstOrDefault();

            if (victim != null) { this.hittedActor = victim; return true; }

            return false;
        }

        //Start projectile translation
        public void launch()
        {
            this.World.BroadcastIfRevealed(new ACDTranslateFixedMessage() { Id = 113, ActorId = (int)this.DynamicID, Velocity = this.velocity, Field2 = 1, AnimationTag = 1, Field4 = 1 }, this);
        }

        //Return the current position of the projectile
        public Vector3D getCurrentPosition()
        {
            if (this.World == null) { return new Vector3D(0, 0, 0); }

            //Get projectile age
            float delta_tick = this.World.Game.TickCounter - this.creationTick;

            //Calculate current pos
            return new Vector3D(this.Position.X + (5f * this.velocity.X * (delta_tick / 6f)),
                                this.Position.Y + (5f * this.velocity.Y * (delta_tick / 6f)),
                                this.Position.Z + (5f * this.velocity.Z * (delta_tick / 6f)));
        }

        public void Update(int tickCounter)
        {
            bool doHitDetect = true;
            if (OnUpdate != null)
                doHitDetect = OnUpdate();
            
            if (doHitDetect && detectCollision())
                if (OnHit != null)
                    OnHit();

            if (this.World != null)
            {
                if (Timeout.TimedOut)
                {
                    if (OnTimeout != null)
                        OnTimeout();

                    Destroy();
                }
            }
        }
    }
}
