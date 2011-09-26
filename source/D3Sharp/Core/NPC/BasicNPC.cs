using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using D3Sharp.Net.Game;

namespace D3Sharp.Core.NPC
{
    public class BasicNPC
    {
        public int ID;
        float HP;
        float MaxHP;

        GameClient Game;

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

        }

        public BasicNPC(int objectId, ref GameClient g)
        {
            ID = objectId;
            Game = g;
            //Game.SendMessage(new AffixMessage()
            //{
            //    Id = 0x48,
            //    Field0 = objectId,
            //    Field1 = 0x1,
            //    aAffixGBIDs = new int[0]
            //});
            //Game.SendMessage(new AffixMessage()
            //{
            //    Id = 0x48,
            //    Field0 = objectId,
            //    Field1 = 0x2,
            //    aAffixGBIDs = new int[0]
            //});
            //Game.SendMessage(new ACDCollFlagsMessage
            //{
            //    Id = 0xa6,
            //    Field0 = objectId,
            //    Field1 = 0x1
            //});

            Game.SendMessage(new AttributesSetValuesMessage
            {
                Id = 0x4d,
                Field0 = objectId,
                atKeyVals = new NetAttributeKeyValue[15] {
                    new NetAttributeKeyValue {
                        Attribute = GameAttribute.Attributes[214],
                        Int = 0
                    },
                    new NetAttributeKeyValue {
                        Attribute = GameAttribute.Attributes[464],
                        Int = 1
                    },
                    new NetAttributeKeyValue {
                        Field0 = 1048575,
                        Attribute = GameAttribute.Attributes[441],
                        Int = 1
                    },
                    new NetAttributeKeyValue {
                        Field0 = 30582,
                        Attribute = GameAttribute.Attributes[560],
                        Int = 1
                    },
                    new NetAttributeKeyValue {
                        Field0 = 30286,
                        Attribute = GameAttribute.Attributes[560],
                        Int = 1
                    },
                    new NetAttributeKeyValue {
                        Field0 = 30285,
                        Attribute = GameAttribute.Attributes[560],
                        Int = 1
                    },
                    new NetAttributeKeyValue {
                        Field0 = 30284,
                        Attribute = GameAttribute.Attributes[560],
                        Int = 1
                    },
                    new NetAttributeKeyValue {
                        Field0 = 30283,
                        Attribute = GameAttribute.Attributes[560],
                        Int = 1
                    },
                    new NetAttributeKeyValue {
                        Field0 = 30290,
                        Attribute = GameAttribute.Attributes[560],
                        Int = 1
                    },
                    new NetAttributeKeyValue {
                        Field0 = 79486,
                        Attribute = GameAttribute.Attributes[560],
                        Int = 1
                    },
                    new NetAttributeKeyValue {
                        Field0 = 30286,
                        Attribute = GameAttribute.Attributes[460],
                        Int = 1
                    },
                    new NetAttributeKeyValue {
                        Field0 = 30285,
                        Attribute = GameAttribute.Attributes[460],
                        Int = 1
                    },
                    new NetAttributeKeyValue {
                        Field0 = 30284,
                        Attribute = GameAttribute.Attributes[460],
                        Int = 1
                    },
                    new NetAttributeKeyValue {
                        Field0 = 30283,
                        Attribute = GameAttribute.Attributes[460],
                        Int = 1
                    },
                    new NetAttributeKeyValue {
                        Field0 = 30290,
                        Attribute = GameAttribute.Attributes[460],
                        Int = 1
                    }
                }

            });

            Game.SendMessage(new AttributesSetValuesMessage
            {
                Id = 0x4d,
                Field0 = objectId,
                atKeyVals = new NetAttributeKeyValue[9] {
                    new NetAttributeKeyValue {
                        Attribute = GameAttribute.Attributes[86],
                        Float = 4.546875f
                    },
                    new NetAttributeKeyValue {
                        Field0 = 79486,
                        Attribute = GameAttribute.Attributes[460],
                        Int = 1
                    },
                    new NetAttributeKeyValue {
                        Attribute = GameAttribute.Attributes[84],
                        Float = 4.546875f
                    },
                    new NetAttributeKeyValue {
                        Attribute = GameAttribute.Attributes[81],
                        Int = 0
                    },
                    new NetAttributeKeyValue {
                        Attribute = GameAttribute.Attributes[77],
                        Float = 4.546875f
                    },
                    new NetAttributeKeyValue {
                        Attribute = GameAttribute.Attributes[69],
                        Int = 1
                    },
                    new NetAttributeKeyValue {
                        Field0 = 30582,
                        Attribute = GameAttribute.Attributes[460],
                        Int = 1
                    },
                    new NetAttributeKeyValue {
                        Attribute = GameAttribute.Attributes[67],
                        Int = 10
                    },
                    new NetAttributeKeyValue {
                        Attribute = GameAttribute.Attributes[38],
                        Int = 1
                    }
                }

            });


            //Game.SendMessage(new ACDGroupMessage
            //{
            //    Id = 0xb8,
            //    Field0 = objectId,
            //    Field1 = unchecked((int)0xb59b8de4),
            //    Field2 = unchecked((int)0xffffffff)
            //});

            //Game.SendMessage(new ANNDataMessage
            //{
            //    Id = 0x3e,
            //    Field0 = objectId
            //});

            //Game.SendMessage(new ACDTranslateFacingMessage
            //{
            //    Id = 0x70,
            //    Field0 = objectId,
            //    Field1 = (float)(rand.NextDouble() * 2.0 * Math.PI),
            //    Field2 = false
            //});

            //Game.SendMessage(new SetIdleAnimationMessage
            //{
            //    Id = 0xa5,
            //    Field0 = objectId,
            //    Field1 = 0x11150
            //});

            //Game.SendMessage(new SNONameDataMessage
            //{
            //    Id = 0xd3,
            //    Field0 = new SNOName
            //    {
            //        Field0 = 0x1,
            //        Field1 = 6652
            //    }
            //});

            //Game.packetId += 30 * 2;
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

        }

        public enum NPCList
        {
            SkeletalArcher = 5346,
            ReturnedArcher = 5347,
            SkeletonKing_Normal = 5350,
            SkeletonKing_Ghost = 5360,
            SkeletonKing_GhostAttack = 5361,
            SkeletonKing_GhostAttack2 = 5362,
            SkeletonKing_GhostDeath = 5363,
            KingLeoricsGhost = 5365,
            TombGuardian = 5387,
            Skeleton = 5393,
            Returned = 5395,
            SkeletalWarrior = 5397,
            SkeletalExecutioner = 5411,
            SerpentMagus = 5428,
            WrithingDeceiver = 5432,
            DoomViper = 5433,
            Spiderling = 5467,
        }
    }
}
