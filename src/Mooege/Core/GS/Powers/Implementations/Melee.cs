using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mooege.Core.GS.Skills;

namespace Mooege.Core.GS.Powers.Implementations
{
    [PowerImplementationAttribute(0x00007780/*Skills.Skills.BasicAttack*/)]
    public class Melee : PowerImplementation
    {
        public override IEnumerable<int> Run(PowerParameters pp, PowersManager fx)
        {
            if (fx.CanHitMeleeTarget(pp.User, pp.Target))
            {
                fx.PlayHitEffect(2, pp.User, pp.Target);
                fx.DoDamage(pp.User, pp.Target, 25f, 0);
            }
            yield break;
        }
    }
}
