using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using D3Sharp.Core.Common.Items;

namespace D3Sharp.Core.Items
{
    class Weapon : Item
    {

        public float Damage { get; set; }
        public float Attackspeed { get; set; }

        public Weapon(float dmg, float attackspeed, int id, uint gbid, ItemType type)
            : base(id, gbid, type)
        {           
            Damage = dmg;
            Attackspeed = attackspeed;
        }
    }
}
