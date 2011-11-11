﻿/*
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
using Mooege.Core.GS.Common.Types.Math;

namespace Mooege.Core.GS.Actors.Movement
{
    public static class ActorHelpers
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

            return (float) Math.Atan2((targetPosition.Y - lookerPosition.Y), (targetPosition.X - lookerPosition.X));
        }

        public static float GetFacingAngle(Actor looker, Actor target)
        {
            return GetFacingAngle(looker.Position, target.Position);
        }

        public static float GetFacingAngle(Actor looker, Vector2F targetPosition)
        {
            return GetFacingAngle(looker.Position, new Vector3D(targetPosition.X, targetPosition.Y, 0));
        }
    }
}
