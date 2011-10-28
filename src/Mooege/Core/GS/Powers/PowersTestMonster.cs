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
using Mooege.Core.GS.Common.Types.Math;

namespace Mooege.Core.GS.Powers
{
    // test monster that can take some damage
    public class PowersTestMonster : Monster
    {
        public float HP;
        public bool IsDead;

        public PowersTestMonster(World world, int actorSNO, Vector3D position)
            : base(world, actorSNO, position)
        {
            this.World.Enter(this);
        }

        public void ReceiveDamage(Actor from, float amount, int type)
        {
            HP -= amount;
            if (!IsDead && HP <= 0.0f)
            {
                IsDead = true;
                Die((Player.Player)from);
            }
        }

        public static void CreateTestMonsters(World world, Vector3D spawnPoint, int count)
        {
            // list of actorSNO values to pick from when spawning
            int[] actorSNO_values = { 4282, 3893, 6652, 5428, 5346, 6024, 5393, 5433, 5467 };

            for (int n = 0; n < count; ++n)
            {
                Vector3D position = new Vector3D(spawnPoint);
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

                int actorSNO = actorSNO_values[RandomHelper.Next(actorSNO_values.Length - 1)];

                PowersTestMonster mob = new PowersTestMonster(world, actorSNO, position)
                {
                    HP = 50,
                    IsDead = false,
                };
            }
        }
    }
}
