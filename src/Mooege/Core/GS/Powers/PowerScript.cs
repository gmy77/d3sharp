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

using System.Collections.Generic;
using System.Linq;
using Mooege.Common;
using Mooege.Core.GS.Ticker;
using Mooege.Core.GS.Common.Types.Math;
using Mooege.Net.GS.Message.Definitions.World;
using Mooege.Core.GS.Actors;

namespace Mooege.Core.GS.Powers
{
    public abstract class PowerScript : PowerContext
    {
        public Vector3D TargetPosition;
        public TargetMessage TargetMessage;

        // Called to start executing a power
        // Yields timers that signify when to continue execution.
        public abstract IEnumerable<TickTimer> Run();

        // token instance that can be yielded by Run() to indicate the power manager should stop
        // running a power implementation.
        public static readonly TickTimer StopExecution = null;


        public TargetList GetBestMeleeEnemy()
        {
            float meleeRange = 10f;  // TODO: possibly use equipped weapon range for this?

            // get all targets that could be hit by melee attack, then select the script's target if
            // it has one, otherwise use the closest target in range.
            TargetList targets = GetEnemiesInBeamDirection(User.Position, TargetPosition, meleeRange);

            Actor bestEnemy;
            if (targets.Actors.Contains(Target))
                bestEnemy = Target;
            else
                bestEnemy = targets.GetClosestTo(User.Position);

            targets.Actors.RemoveAll(actor => actor != bestEnemy);
            return targets;
        }
    }
}
