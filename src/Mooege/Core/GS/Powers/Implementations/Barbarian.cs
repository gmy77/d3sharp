using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mooege.Core.GS.Skills;
using Mooege.Net.GS.Message.Fields;

namespace Mooege.Core.GS.Powers.Implementations
{
    [PowerImplementationAttribute(0x0001358A/*Skills.Skills.Barbarian.FuryGenerators.Bash*/)]
    public class BarbarianBash : PowerImplementation
    {
        public override IEnumerable<int> Run(PowerParameters pp, PowersManager fx)
        {
            fx.SpawnEffect(pp.User, 3278, pp.User.Position, fx.AngleLookAt(pp.User.Position, pp.TargetPosition), 500);
            yield return 200;
            if (fx.WillHitMeleeTarget(pp.User, pp.Target))
            {
                fx.PlayEffectGroupActorToActor(18663, pp.User, pp.Target);
                fx.DoKnockback(pp.User, pp.Target, 4f);
                fx.DoDamage(pp.User, pp.Target, 35, 0);
            }

            yield break;
        }
    }
}
