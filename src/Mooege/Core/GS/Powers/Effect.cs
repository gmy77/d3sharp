using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mooege.Core.GS.Actors;
using Mooege.Net.GS.Message.Fields;

namespace Mooege.Core.GS.Powers
{
    public class Effect : Actor
    {
        public override ActorType ActorType { get { return Actors.ActorType.Monster; } }

        public Effect(Map.World world, int actorSNO, Vector3D position, float angle, int timeout)
            : base(world, world.NewActorID)
        {
            this.ActorSNO = actorSNO;
            RotationAmount = (float)Math.Cos(angle / 2f);
            RotationAxis = new Vector3D(0, 0, (float)Math.Sin(angle / 2f));

            // FIXME: This is hardcoded crap
            this.Field2 = 0x8; // monster=0x8, using effect's id results in not being able to do smooth actor movements
            this.Field3 = 0x0;
            //this.Field7 = -1;
            //this.Field8 = -1;
            this.Scale = 1.35f; // TODO: should this be 1 for effects?
            this.Position.Set(position);
            this.GBHandle.Type = -1; this.GBHandle.GBID = -1; // TODO: use proper enum value

            Timeout = DateTime.Now.AddMilliseconds(timeout);

            world.Enter(this);
        }

        public DateTime Timeout;
    }
}
