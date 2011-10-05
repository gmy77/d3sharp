using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mooege.Core.GS.Powers.Implementations
{
    [PowerImplementationAttribute(0x00007780/*Skills.Skills.BasicAttack*/)]
    public class Melee : PowerImplementation
    {
        public override IEnumerable<int> Run(PowerParameters pp, PowersManager fx)
        {
            if (pp.Target != null)
            {
                if (Math.Abs(pp.User.Position.X - pp.TargetPosition.X) < 15f &&
                    Math.Abs(pp.User.Position.Y - pp.TargetPosition.Y) < 15f)
                {
                    fx.PlayHitEffect(2, pp.User, pp.Target);
                    fx.DoDamage(pp.User, pp.Target, 25f, 0);
                }
            }
            yield break;
        }
    }
}
