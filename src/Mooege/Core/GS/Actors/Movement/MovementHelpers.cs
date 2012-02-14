﻿/*
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

using System;
using Mooege.Core.GS.Common.Types.Math;

namespace Mooege.Core.GS.Actors.Movement
{
    public static class MovementHelpers
    {
        /// <summary>
        /// Returns 2D angle to face the target position.
        /// </summary>
        /// <param name="lookerPosition">The looker.</param>
        /// <param name="targetPosition">The target.</param>
        /// <returns></returns>
        public static float GetFacingAngle(Vector3D lookerPosition, Vector3D targetPosition)
        {
            if ((lookerPosition == null) || (targetPosition == null))
                return 0f;

            float a = (float)Math.Atan2((targetPosition.Y - lookerPosition.Y), (targetPosition.X - lookerPosition.X));
            if (a < 0f)
                a += (float)Math.PI * 2f;

            return a;
        }

        public static float GetFacingAngle(Actor looker, Actor target)
        {
            return GetFacingAngle(looker.Position, target.Position);
        }

        public static float GetFacingAngle(Actor looker, Vector2F targetPosition)
        {
            return GetFacingAngle(looker.Position, new Vector3D(targetPosition.X, targetPosition.Y, 0));
        }

        public static float GetFacingAngle(Actor looker, Vector3D targetPosition)
        {
            return GetFacingAngle(looker.Position, targetPosition);
        }

        public static float GetDistance(Vector3D startPosition, Vector3D targetPosition)
        {
            if ((startPosition == null) || (targetPosition == null)) return 0;
            return (float)Math.Sqrt(Math.Pow(startPosition.X - targetPosition.X, 2) + Math.Pow(startPosition.Y - targetPosition.Y, 2));
        }

        public static Vector3D GetMovementPosition(Vector3D position, float speed, float facingAngle, int ticks = 6)
        {
            var xDelta = (speed * ticks) * (float)Math.Cos(facingAngle);
            var yDelta = (speed * ticks) * (float)Math.Sin(facingAngle);

            return new Vector3D(position.X + xDelta, position.Y + yDelta, position.Z);
        }
    }
}
