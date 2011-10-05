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

using Mooege.Core.GS.Game;
using Mooege.Core.GS.Map;
using Mooege.Core.GS.Actors;
using Mooege.Net.GS;
using Mooege.Net.GS.Message;
using Mooege.Net.GS.Message.Definitions.Attribute;
using Mooege.Net.GS.Message.Fields;
using Mooege.Net.GS.Message.Definitions.ACD;
using Mooege.Net.GS.Message.Definitions.Misc;

namespace Mooege.Core.GS.NPC
{
    public class Mob : Actor
    {
        public override ActorType ActorType { get { return ActorType.Monster; } }

        public Mob(World world, int actorSNO, Vector3D position)
            : base(world, world.Game.NewMonsterID)
        {
            this.AppearanceSNO = actorSNO;
            this.Position = position;
            // FIXME: This is hardcoded crap
            this.Field2 = 0x8;
            this.Field3 = 0x0;
            this.Scale = 1.35f;
            this.RotationAmount = 0.768145f;
            this.RotationAxis.X = 0f; this.RotationAxis.Y = 0f; this.RotationAxis.Z = -0.640276f;
            this.GBHandle.Type = (int)GBHandleType.Monster; this.GBHandle.GBID = 1;
            this.Field7 = 0x00000001;
            this.Field8 = AppearanceSNO;
            this.Field10 = 0x0;
            this.Field11 = 0x0;
            this.Field12 = 0x0;
            this.Field13 = 0x0;
        }

        // Ye olde, kept for proper implementation..
        //public void Die()
        //{
            //Game.SendMessage(new PlayEffectMessage()
            //{
            //    Id = 0x7a,
            //    Field0 = ID,
            //    Field1 = 0x0,
            //    Field2 = 0x2,
            //});
            //Game.SendMessage(new PlayEffectMessage()
            //{
            //    Id = 0x7a,
            //    Field0 = ID,
            //    Field1 = 0xc,
            //});
            //Game.SendMessage(new PlayHitEffectMessage()
            //{
            //    Id = 0x7b,
            //    Field0 = ID,
            //    Field1 = Client.Player.Hero.Id,
            //    Field2 = 0x2,
            //    Field3 = false,
            //});

            //Game.SendMessage(new FloatingNumberMessage()
            //{
            //    Id = 0xd0,
            //    Field0 = ID,
            //    Field1 = 9001.0f,
            //    Field2 = 0,
            //});

            //Game.SendMessage(new ANNDataMessage()
            //{
            //    Id = 0x6d,
            //    Field0 = ID,
            //});

            //int ani = killAni[anim];

            //Game.SendMessage(new PlayAnimationMessage()
            //{
            //    Id = 0x6c,
            //    Field0 = ID,
            //    Field1 = 0xb,
            //    Field2 = 0,
            //    tAnim = new PlayAnimationMessageSpec[1]
            //    {
            //        new PlayAnimationMessageSpec()
            //        {
            //            Field0 = 0x2,
            //            Field1 = ani,
            //            Field2 = 0x0,
            //            Field3 = 1f
            //        }
            //    }
            //});

            //Game.packetId += 10 * 2;
            //Game.SendMessage(new DWordDataMessage()
            //{
            //    Id = 0x89,
            //    Field0 = Game.packetId,
            //});

            //Game.SendMessage(new ANNDataMessage()
            //{
            //    Id = 0xc5,
            //    Field0 = ID,
            //});

            //Game.SendMessage(new AttributeSetValueMessage
            //{
            //    Id = 0x4c,
            //    Field0 = ID,
            //    Field1 = new NetAttributeKeyValue
            //    {
            //        Attribute = GameAttribute.Attributes[0x4d],
            //        Float = 0
            //    }
            //});

            //Game.SendMessage(new AttributeSetValueMessage
            //{
            //    Id = 0x4c,
            //    Field0 = ID,
            //    Field1 = new NetAttributeKeyValue
            //    {
            //        Attribute = GameAttribute.Attributes[0x1c2],
            //        Int = 1
            //    }
            //});

            //Game.SendMessage(new AttributeSetValueMessage
            //{
            //    Id = 0x4c,
            //    Field0 = ID,
            //    Field1 = new NetAttributeKeyValue
            //    {
            //        Attribute = GameAttribute.Attributes[0x1c5],
            //        Int = 1
            //    }
            //});
            //Game.SendMessage(new PlayEffectMessage()
            //{
            //    Id = 0x7a,
            //    Field0 = ID,
            //    Field1 = 0xc,
            //});
            //Game.SendMessage(new PlayEffectMessage()
            //{
            //    Id = 0x7a,
            //    Field0 = ID,
            //    Field1 = 0x37,
            //});
            //Game.SendMessage(new PlayHitEffectMessage()
            //{
            //    Id = 0x7b,
            //    Field0 = ID,
            //    Field1 = Client.Player.Hero.Id,
            //    Field2 = 0x2,
            //    Field3 = false,
            //});

            //Game.packetId += 10 * 2;
            //Game.SendMessage(new DWordDataMessage()
            //{
            //    Id = 0x89,
            //    Field0 = Game.packetId,
            //});

            //Game.tick += 20;
            //Game.SendMessage(new EndOfTickMessage()
            //{
            //    Id = 0x008D,
            //    Field0 = Game.tick - 20,
            //    Field1 = Game.tick
            //});
        //}
    }
}
