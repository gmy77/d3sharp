using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mooege.Core.GS.Powers
{
    public class DamageType
    {
        public enum HitEffectType : int
        {
            Blood = 0,
            Fire = 1,
            Lightning = 2,
            Cold = 3,
            Poison = 4,
            Arcane = 5,
            Holy = 6,
            UnknownFlicker = 7
        }

        public HitEffectType HitEffect;

        public static readonly DamageType Physical = new DamageType { HitEffect = HitEffectType.Blood };
        public static readonly DamageType Arcane = new DamageType { HitEffect = HitEffectType.Arcane };
        public static readonly DamageType Cold = new DamageType { HitEffect = HitEffectType.Cold };
        public static readonly DamageType Fire = new DamageType { HitEffect = HitEffectType.Fire };
        public static readonly DamageType Lightning = new DamageType { HitEffect = HitEffectType.Lightning };
        public static readonly DamageType Disease = new DamageType { HitEffect = HitEffectType.Poison }; // disease and poison share hit effect?
        public static readonly DamageType Poison = new DamageType { HitEffect = HitEffectType.Poison };
        public static readonly DamageType Holy = new DamageType { HitEffect = HitEffectType.Holy };
    }
}
