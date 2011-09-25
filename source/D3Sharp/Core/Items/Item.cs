/*
 * Copyright (C) 2011 D3Sharp Project
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
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using D3Sharp.Net.Game;

namespace D3Sharp.Core.Items
{

    public enum ItemType
    {

        Helm, ChestArmor, Gloves, Boots, Shoulders, Belt, Pants, Bracers, Shield, Quiver, Orb, 
        Axe_1H, Axe_2H, CombatStaff_2H, Dagger, FistWeapon, Mace_1H, Mace_2H, Sword_1H, 
        Sword_2H, Bow, Crossbow, Spear, Staff, Polearm, ThrownWeapon, ThrowingAxe, Wand, Ring
    }

    class Item
    {

        public int ItemId { get; set; }
        public int Gbid { get; set; }
        public int PlayerId { get; set; }

        public IVector2D InvLoc { get; set; }
        public List<Affix> AffixList { get; set; }
        public List<NetAttribute> AttributeList { get; set; }


        public Item(int id, int gbid)
        {
            ItemId = id;
            Gbid = gbid;
            InvLoc = new IVector2D();
            InvLoc.Field0 = 0x00000000 -1;
            InvLoc.Field1 = 0x00000000 -1;

            AffixList = new List<Affix>();
            AttributeList = new List<NetAttribute>();
        }

        public void SendTo(GameClient client)
        {
            client.SendMessage(new ACDEnterKnownMessage()
            {
                Id = 0x003B,
                Field0 = ItemId,
                Field1 = 0x00001158,
                Field2 = 0x0000001A,
                Field3 = 0x00000001,
                Field4 = null,
                Field5 = new InventoryLocationMessageData()
                {
                    Field0 = PlayerId,
                    Field1 = 0x00000000,
                    Field2 = InvLoc,
                },
                Field6 = new GBHandle()
                {
                    Field0 = 0x00000002,
                    Field1 = Gbid,
                },
                Field7 = -1,
                Field8 = -1,
                Field9 = 0x00000001,
                Field10 = 0x00,
            });



            int[] affixGbis = new int[AffixList.Count];
            for (int i = 0; i < AffixList.Count; i++)
            {
                affixGbis[i] = AffixList[i].AffixGbid;
            }

            client.SendMessage(new AffixMessage()
            {
                Id = 0x0048,
                Field0 = ItemId,
                Field1 = 0x00000001,
                aAffixGBIDs = affixGbis,

            });

            client.SendMessage(new AffixMessage()
            {
                Id = 0x0048,
                Field0 = ItemId,
                Field1 = 0x00000002,
                aAffixGBIDs = affixGbis,
            });

            /*
            
            client.SendMessage(new AffixMessage()
            {
                Id = 0x0048,
                Field0 = Id,
                Field1 = 0x00000001,
                aAffixGBIDs = new int[0]{},
                            
            });

            client.SendMessage(new AffixMessage()
                {
                            Id = 0x0048,
                            Field0 = Id,
                            Field1 = 0x00000002,
                            aAffixGBIDs = new int[0]
                {
                },
            });


            */
            client.SendMessage(new ACDCollFlagsMessage()
            {
                Id = 0x00A6,
                Field0 = ItemId,
                Field1 = 0x00000080,
            });


            List<NetAttributeKeyValue> netAttributesList = new List<NetAttributeKeyValue>();            
            foreach (NetAttribute attr in AttributeList){
                NetAttributeKeyValue netAttr = new NetAttributeKeyValue()
                {
                    Attribute = GameAttribute.Attributes[attr.Key], // Hitpoints_Granted 
                    Int = unchecked((int)attr.IntValue),
                    Float = attr.FloatValue,
                };
                netAttributesList.Add(netAttr);
            }

            client.SendMessage(new AttributesSetValuesMessage()
            {
                Id = 0x004D,
                Field0 = ItemId,
                atKeyVals = netAttributesList.ToArray(),
            });





            /*
            client.SendMessage(new AttributesSetValuesMessage()
            {
                Id = 0x004D,
                Field0 = Id,
                atKeyVals = new NetAttributeKeyValue[4]
                    {
                         new NetAttributeKeyValue()
                         {
                            Attribute = GameAttribute.Attributes[0x0052], // Hitpoints_Granted 
                            Int = 0x00000000,
                            Float = 100f,
                         },
                         new NetAttributeKeyValue()
                         {
                            Attribute = GameAttribute.Attributes[0x0125], // Seed 
                            Int = unchecked((int)0x884DCD35),
                            Float = 0f,
                         },
                         new NetAttributeKeyValue()
                         {
                            Attribute = GameAttribute.Attributes[0x0121], // ItemStackQuantityLo 
                            Int = 0x00000001,
                            Float = 0f,
                         },
                         new NetAttributeKeyValue()
                         {
                            Attribute = GameAttribute.Attributes[0x0115], // Item_Quality_Level 
                            Int = 0x00000001,
                            Float = 0f,
                         },
                    },
            });
            */
            client.SendMessage(new ACDGroupMessage()
            {
                Id = 0x00B8,
                Field0 = ItemId,
                Field1 = -1,
                Field2 = -1,
            });

            client.SendMessage(new ANNDataMessage()
            {
                Id = 0x003E,
                Field0 = ItemId,
            });

            client.SendMessage(new SNONameDataMessage()
            {
                Id = 0x00D3,
                Field0 = new SNOName()
                {
                    Field0 = 0x00000001,
                    Field1 = 0x00001158,
                },
            });


            client.SendMessage(new AttributeSetValueMessage()
            {
                Id = 0x004C,
                Field0 = ItemId,
                Field1 = new NetAttributeKeyValue()
                {
                    Attribute = GameAttribute.Attributes[0x0115], // Item_Quality_Level 
                    Int = 0x00000001,
                    Float = 0f,
                },
            });

            client.SendMessage(new AttributeSetValueMessage()
            {
                Id = 0x004C,
                Field0 = ItemId,
                Field1 = new NetAttributeKeyValue()
                {
                    Attribute = GameAttribute.Attributes[0x0052], // Hitpoints_Granted 
                    Int = 0x00000000,
                    Float = 100f,
                },
            });

            client.SendMessage(new AttributeSetValueMessage()
            {
                Id = 0x004C,
                Field0 = ItemId,
                Field1 = new NetAttributeKeyValue()
                {
                    Attribute = GameAttribute.Attributes[0x0125], // Seed 
                    Int = unchecked((int)0x884DCD35),
                    Float = 0f,
                },
            });

            client.SendMessage(new AttributeSetValueMessage()
            {
                Id = 0x004C,
                Field0 = ItemId,
                Field1 = new NetAttributeKeyValue()
                {
                    Attribute = GameAttribute.Attributes[0x0121], // ItemStackQuantityLo 
                    Int = 0x00000001,
                    Float = 0f,
                },
            });

            client.SendMessage(new PlayEffectMessage()
            {
                Id = 0x007A,
                Field0 = ItemId,
                Field1 = 0x00000027,
            });

            client.SendMessage(new ACDInventoryPositionMessage()
            {
                Id = 0x0040,
                Field0 = ItemId,
                Field1 = new InventoryLocationMessageData()
                {
                    Field0 = PlayerId,
                    Field1 = 0x00000000,
                    Field2 = InvLoc,
                },
                Field2 = 0x00000001,
            });

            client.SendMessage(new ACDInventoryUpdateActorSNO()
            {
                Id = 0x0041,
                Field0 = ItemId,
                Field1 = 0x00001158,
            });

          
        }
    }
}
