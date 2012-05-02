/*
* Copyright (C) 2011 - 2012 mooege project - http://www.mooege.org
*
* This program is free software; you can redistribute it and/or modify
* it under the terms of the GNU General Public License as published by
* the Free Software Foundation; either version 2 of the License, or
* (at your option) any later version.
*
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
* GNU General Public License for more details.
*
* You should have received a copy of the GNU General Public License
* along with this program; if not, write to the Free Software
* Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mooege.Common.Helpers.Math;
using Mooege.Core.GS.Actors.Movement;
using Mooege.Core.GS.Common.Types.Math;
using Mooege.Core.GS.Ticker;
using Mooege.Net.GS.Message;
using Mooege.Core.GS.Actors;

namespace Mooege.Core.GS.Powers.Implementations
{
    public abstract class SummoningSkill : Skill
    {
        [ImplementsPowerSNO(94734)] // Summon_Zombie_Vomit.pow
        public class WretchedMotherVomit : SummoningSkill
        {
            public override IEnumerable<TickTimer> Main()
            {
                float x, y, z, castAngle = MovementHelpers.GetFacingAngle(User.Position, TargetPosition);
                x = User.Position.X + 8 * (float)Math.Cos(castAngle);
                y = User.Position.Y + 8 * (float)Math.Sin(castAngle);
                z = User.Position.Z;
                var actorSNO = (this.User as Monster).SNOSummons[0];
                yield return WaitSeconds(0.8f);
                var pos = new Vector3D(x, y, z);
                World.SpawnMonster(actorSNO, pos);
            }
        }

        [ImplementsPowerSNO(30543)] // Summon Skeleton
        public class SummonSkeleton : SummoningSkill
        {
            public override IEnumerable<TickTimer> Main()
            {
                float x, y, z, castAngle = MovementHelpers.GetFacingAngle(User.Position, TargetPosition);
                x = User.Position.X + 8 * (float)Math.Cos(castAngle);
                y = User.Position.Y + 8 * (float)Math.Sin(castAngle);
                z = User.Position.Z;
                var actorSNO = (this.User as Monster).SNOSummons[0];
                yield return WaitSeconds(0.8f);
                var pos = new Vector3D(x, y, z);
                World.SpawnMonster(actorSNO, pos);
            }
        }

        [ImplementsPowerSNO(30800)] // Summon Spores
        public class SummonSpores : SummoningSkill
        {
            public override IEnumerable<TickTimer> Main()
            {
                float x, y, z, castAngle = MovementHelpers.GetFacingAngle(User.Position, TargetPosition);
                x = User.Position.X + 8 * (float)Math.Cos(castAngle);
                y = User.Position.Y + 8 * (float)Math.Sin(castAngle);
                z = User.Position.Z;
                yield return WaitSeconds(1.0f);
                var pos = new Vector3D(x, y, z);
                World.SpawnMonster(5482, pos);//HACK, we don't have this in mpq
            }
        }
    }
}
