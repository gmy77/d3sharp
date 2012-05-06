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
using Mooege.Core.GS.Common.Types.TagMap;

namespace Mooege.Core.GS.Powers.Implementations
{
    public abstract class SummoningSkill : ActionTimedSkill
    {
        public void SummonMonsterInFront(int actorSNO)
        {
            float userFacing = (float)Math.Acos(this.User.RotationW) * 2f;
            Vector3D spawnPos = new Vector3D(User.Position.X + 8 * (float)Math.Cos(userFacing),
                                             User.Position.Y + 8 * (float)Math.Sin(userFacing),
                                             User.Position.Z);

            var monster = ActorFactory.Create(User.World, actorSNO, new TagMap());
            monster.Scale = 1.35f;  // TODO: look this up properly
            monster.EnterWorld(spawnPos);
            this.World.BuffManager.AddBuff(User, monster, new Implementations.SummonedBuff());
        }
        public void SummonMonsterOn(int actorSNO)
        {
            float userFacing = (float)Math.Acos(this.User.RotationW) * 2f;
            Vector3D spawnPos = new Vector3D(User.Position.X,
                                             User.Position.Y,
                                             User.Position.Z);

            var monster = ActorFactory.Create(User.World, actorSNO, new TagMap());
            monster.Scale = 1.35f;  // TODO: look this up properly
            monster.EnterWorld(spawnPos);
            this.World.BuffManager.AddBuff(User, monster, new Implementations.SummonedBuff());
        }
        public void SummonMonsterRandom(int actorSNO)
        {
            float userFacing = (float)Math.Acos(this.User.RotationW) * 2f;
            Vector3D spawnPos = new Vector3D(User.Position.X + 100 * (float)RandomHelper.NextDouble(),
                                             User.Position.Y + 100 * (float)RandomHelper.NextDouble(),
                                             User.Position.Z);

            var monster = ActorFactory.Create(User.World, actorSNO, new TagMap());
            monster.Scale = 1.35f;  // TODO: look this up properly
            monster.EnterWorld(spawnPos);
            this.World.BuffManager.AddBuff(User, monster, new Implementations.SummonedBuff());
        }
    }

    [ImplementsPowerSNO(94734)] // Summon_Zombie_Vomit.pow
    public class WretchedMotherVomit : SummoningSkill
    {
        public override IEnumerable<TickTimer> Main()
        {
            SummonMonsterInFront((this.User as Monster).SNOSummons[0]);
            yield break;
        }
    }

    [ImplementsPowerSNO(30543)] // Summon Skeleton
    public class SummonSkeleton : SummoningSkill
    {
        public override IEnumerable<TickTimer> Main()
        {
            SummonMonsterInFront((this.User as Monster).SNOSummons[0]);
            yield break;
        }
    }

    [ImplementsPowerSNO(30800)] // Summon Spores
    public class SummonSpores : SummoningSkill
    {
        public override IEnumerable<TickTimer> Main()
        {
            SummonMonsterRandom(5482);  // HACK: we don't have this in mpq
            yield break;
        }
    }
    [ImplementsPowerSNO(117580)] // Summon FleshPitFlyers
    public class SummonFleshPitFlyers : SummoningSkill
    {
        public override IEnumerable<TickTimer> Main()
        {
            SummonMonsterOn((this.User as Monster).SNOSummons[0]);
            yield break;
        }
    }
    //
}
