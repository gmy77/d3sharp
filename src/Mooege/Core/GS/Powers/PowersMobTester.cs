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
using Mooege.Net.GS.Message.Fields;
using Mooege.Core.GS.Actors;
using Mooege.Net.GS;
using Mooege.Common.Helpers;
using Mooege.Net.GS.Message.Definitions.ACD;
using Mooege.Net.GS.Message.Definitions.Misc;
using Mooege.Net.GS.Message.Definitions.Attribute;
using Mooege.Net.GS.Message;
using Mooege.Net.GS.Message.Definitions.Animation;
using Mooege.Net.GS.Message.Definitions.Effect;
using Mooege.Core.GS.Map;

namespace Mooege.Core.GS.Powers
{
    public class SimpleMob : Monster
    {
        public PowersMobTester owner;
        public float hp;
        public bool dead;

        public SimpleMob(World world, int actorSNO, Vector3D position)
            : base(world, actorSNO, position)
        {
        }

        public void ReceiveDamage(Actor from, float amount, int type)
        {
            hp -= amount;
            if (!dead && hp <= 0.0f)
                owner.KillMob(from, this);
        }
    }

    // simple hackish mob spawner, all mobs created by all instances of it are available via the AllMobs
    // property
    public class PowersMobTester
    {
        public static IEnumerable<SimpleMob> AllMobs
        {
            get
            {
                foreach (var moblist in _mobtesters)
                    foreach (var mob in moblist._mobs)
                        yield return mob;
            }
        }

        public static List<PowersMobTester> _mobtesters = new List<PowersMobTester>();

        private List<SimpleMob> _mobs = new List<SimpleMob>();

        public PowersMobTester()
        {
            _mobtesters.Add(this);
        }

        public IList<SimpleMob> SpawnMob(Actor user, int count = 10, int actorSNO = -1)
        {
            // actor sno list to select from when spawning if actorSNO == -1
            int[] mobids = { 4282, 3893, 6652, 5428, 5346, 6024, 5393, 5433, 5467 };

            IList<SimpleMob> created = new List<SimpleMob>();

            for (int n = 0; n < count; ++n)
            {
                Vector3D position = new Vector3D();
                position.X = user.Position.X;
                position.Y = user.Position.Y;
                position.Z = user.Position.Z;
                if ((n % 2) == 0)
                {
                    position.X += (float)(RandomHelper.NextDouble() * 20);
                    position.Y += (float)(RandomHelper.NextDouble() * 20);
                }
                else
                {
                    position.X -= (float)(RandomHelper.NextDouble() * 20);
                    position.Y -= (float)(RandomHelper.NextDouble() * 20);
                }

                if (actorSNO == -1)
                    actorSNO = mobids[RandomHelper.Next(mobids.Length - 1)];

                SimpleMob mob = new SimpleMob(user.World, actorSNO, position)
                {
                    hp = 50,
                    owner = this,
                    dead = false,
                };
                _mobs.Add(mob);
                created.Add(mob);
            }
            return created;
        }

        public void KillMob(Actor user, SimpleMob mob)
        {
            mob.dead = true;
            _mobs.Remove(mob);
            mob.Die((Player.Player)user);
        }
    }
}
