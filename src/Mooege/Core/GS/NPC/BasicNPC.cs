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

using Mooege.Net.GS;
using Mooege.Net.GS.Message;
using Mooege.Net.GS.Message.Definitions.Attribute;
using Mooege.Net.GS.Message.Fields;
using Mooege.Net.GS.Message.Definitions.ACD;
using Mooege.Net.GS.Message.Definitions.Misc;

namespace Mooege.Core.GS.NPC
{
    public class BasicNPC
    {
        public int ID;
        private float HP;
        private float MaxHP;
        private int snoId;
        public WorldPlace Location;

        public void Die(int anim)
        {
            var killAni = new int[]{
                    0x2cd7,
                    0x2cd4,
                    0x01b378,
                    0x2cdc,
                    0x02f2,
                    0x2ccf,
                    0x2cd0,
                    0x2cd1,
                    0x2cd2,
                    0x2cd3,
                    0x2cd5,
                    0x01b144,
                    0x2cd6,
                    0x2cd8,
                    0x2cda,
                    0x2cd9
            };

            
            #region old
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
            //    Field1 = 0x789E00E2,
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
            //    Field1 = 0x789E00E2,
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

            //Game.FlushOutgoingBuffer();
            #endregion
        }

        public BasicNPC(int objectId, int snoId, WorldPlace location)
        {
            this.ID = objectId;
            this.snoId = snoId;
           
            this.Location = location;


        }

        public void Reveal(GameClient client){
            client.SendMessage(new ACDEnterKnownMessage()
            {
                Id = 0x003B,
                Field0 = client.ObjectId,
                Field1 = snoId,
                Field2 = 0x8,
                Field3 = 0x0,
                Field4 = new WorldLocationMessageData()
                {
                    Field0 = 1.35f,
                    Field1 = new PRTransform()
                    {
                        Field0 = new Quaternion()
                        {
                            Amount = 0.768145f,
                            Axis = new Vector3D()
                            {
                                X = 0f,
                                Y = 0f,
                                Z = -0.640276f,
                            },
                        },
                        ReferencePoint = Location.Field0,
                    },
                    Field2 = 0x772E0000,
                },
                Field5 = null,
                Field6 = new GBHandle()
                {
                    Field0 = 1,
                    Field1 = 1,
                },
                Field7 = 0x00000001,
                Field8 = snoId,
                Field9 = 0x0,
                Field10 = 0x0,
                Field11 = 0x0,
                Field12 = 0x0,
                Field13 = 0x0
            });
        }
    }
}
