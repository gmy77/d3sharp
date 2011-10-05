using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mooege.Core.GS.Actors;
using Mooege.Net.GS;
using Mooege.Net.GS.Message.Fields;

namespace Mooege.Core.GS.Powers.Implementations
{
    [PowerImplementationAttribute(0x00010E46/*Skills.Skills.Wizard.Offensive.Meteor*/)]
    public class WizardMeteor : PowerImplementation
    {
        public override IEnumerable<int> Run(PowerParameters pp, PowersManager fx)
        {
            fx.SpawnEffect(pp.User, 86790, pp.TargetPosition);
            yield return 2000;
            fx.SpawnEffect(pp.User, 86769, pp.TargetPosition);
            fx.SpawnEffect(pp.User, 90364, pp.TargetPosition, -1, 4000);

            IList<Actor> hits = fx.FindActorsInRadius(pp.TargetPosition, 13f);
            fx.DoDamage(pp.User, hits, 150f, 0);
        }
    }

    [PowerImplementationAttribute(0x000006E5/*Skills.Skills.Wizard.Signature.Electrocute*/)]
    public class WizardElectrocute : PowerImplementation
    {
        public override IEnumerable<int> Run(PowerParameters pp, PowersManager fx)
        {
            // channeled power
            fx.AddChannelingActor(pp.User);

            fx.LookAt(pp.User, pp.TargetPosition);

            IList<Actor> targets;
            if (pp.Target == null)
            {
                targets = new List<Actor>();
                fx.PlayRopeEffectActorToActor(0x78c0, pp.User, fx.GetProxyEffectFor(pp.User, pp.TargetPosition));
                fx.SendDWordTickFor(pp.User);
            }
            else
            {
                targets = fx.FindActorsInRadius(pp.TargetPosition, 15f, 1);
                targets.Insert(0, pp.Target);
                Actor effect_source = pp.User;
                foreach (Actor actor in targets)
                {
                    fx.PlayHitEffect(2, effect_source, actor);
                    fx.PlayRopeEffectActorToActor(0x78c0, effect_source, actor);
                    fx.SendDWordTickFor(actor);

                    effect_source = actor;
                }
            }

            fx.DoDamage(pp.User, targets, 12, 0);
            yield break;
        }
    }

    [PowerImplementationAttribute(0x00007818/*Skills.Skills.Wizard.Signature.MagicMissile*/)]
    public class WizardMagicMissile : PowerImplementation
    {
        public override IEnumerable<int> Run(PowerParameters pp, PowersManager fx)
        {
            // HACK: made up spell, not real magic missile
            for (int step = 1; step < 10; ++step)
            {
                var spos = new Vector3D();
                spos.X = pp.User.Position.X + ((pp.TargetPosition.X - pp.User.Position.X) * (step * 0.10f));
                spos.Y = pp.User.Position.Y + ((pp.TargetPosition.Y - pp.User.Position.Y) * (step * 0.10f));
                spos.Z = pp.User.Position.Z + ((pp.TargetPosition.Z - pp.User.Position.Z) * (step * 0.10f));

                fx.SpawnEffect(pp.User, 61419, spos);

                IList<Actor> hits = fx.FindActorsInRadius(spos, 6f);
                fx.DoDamage(pp.User, hits, 60f, 0);
                yield return 100;
            }
        }
    }

    [PowerImplementationAttribute(0x00007805/*Skills.Skills.Wizard.Offensive.Hydra*/)]
    public class WizardHydra : PowerImplementation
    {
        public override IEnumerable<int> Run(PowerParameters pp, PowersManager fx)
        {
            // HACK: made up demonic meteor spell, not real hydra
            fx.SpawnEffect(pp.User, 185366, pp.TargetPosition);
            yield return 400;

            IList<Actor> hits = fx.FindActorsInRadius(pp.TargetPosition, 10f);
            fx.DoDamage(pp.User, hits, 100f, 0);
        }
    }

    [PowerImplementationAttribute(0x0001659D/*Skills.Skills.Wizard.Offensive.Disintegrate*/)]
    public class WizardDisintegrate : PowerImplementation
    {
        public override IEnumerable<int> Run(PowerParameters pp, PowersManager fx)
        {
            // TODO: damage and hit effects
            //Effect pid = fx.GetProxyEffectFor(fx.pp.User, pp.TargetPosition);
            //if (!_channelingActors.Contains(pp.User))
            //{
            //    _channelingActors.Add(pp.User);
            //    PlayRopeEffectActorToActor(30888, pp.User, pid);
            //}

            //DoDamage(user, target, 12, 0);
            yield break;
        }
    }
}
