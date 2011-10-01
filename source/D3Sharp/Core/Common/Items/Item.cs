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
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using D3Sharp.Net.Game;
using D3Sharp.Net.Game.Message.Fields;
using D3Sharp.Net.Game.Message.Definitions.ACD;
using D3Sharp.Net.Game.Message.Definitions.Attribute;
using D3Sharp.Core.Items;
using D3Sharp.Net.Game.Message.Definitions.Misc;
using D3Sharp.Net.Game.Message.Definitions.Effect;
using D3Sharp.Net.Game.Message;
using D3Sharp.Core.Ingame.Universe;
using D3Sharp.Core.Ingame.Actors;

namespace D3Sharp.Core.Common.Items
{

    public enum ItemType
    {

        Helm, ChestArmor, Gloves, Boots, Shoulders, Belt, Pants, Bracers, Shield, Quiver, Orb, 
        Axe_1H, Axe_2H, CombatStaff_2H, Dagger, FistWeapon, Mace_1H, Mace_2H, Sword_1H, 
        Sword_2H, Bow, Crossbow, Spear, Staff, Polearm, ThrownWeapon, ThrowingAxe, Wand, Ring
    }

    public class Item
    {

        public int ItemId { get; set; }
        public int Gbid { get; set; }
        public int SnoId { get; set; }
        public ItemType Type { get; set; }
        public int Count { get; set;  }             // <- amount?


        public List<Affix> AffixList { get; set; }
        public List<NetAttribute> AttributeList { get; set; }


        public Item(int id, uint gbid, ItemType type)
        {
            ItemId = id;
            Gbid = unchecked((int)gbid);
            Count = 1;
            Type = type;

            AffixList = new List<Affix>();
            AttributeList = new List<NetAttribute>();
        }

        // There are 2 VisualItemClasses... any way to use the builder to create a D3 Message?
        public VisualItem CreateVisualItem()
        {
            return new VisualItem()
            {
                Field0 = Gbid,
                Field1 = 0,
                Field2 = 0,
                Field3 = -1
            };

        }

        public static bool isPotion(ItemType itemType)
        {
            // TODO: implement me
            return false;
        }

        public static bool isWeapon(ItemType itemType)
        {
            return (itemType == ItemType.Axe_1H
                || itemType == ItemType.Axe_2H
                || itemType == ItemType.Bow
                || itemType == ItemType.CombatStaff_2H
                || itemType == ItemType.Crossbow
                || itemType == ItemType.Dagger
                || itemType == ItemType.FistWeapon
                || itemType == ItemType.Mace_1H
                || itemType == ItemType.Mace_2H
                || itemType == ItemType.Orb
                || itemType == ItemType.Polearm
                || itemType == ItemType.Spear
                || itemType == ItemType.Staff
                || itemType == ItemType.Sword_1H
                || itemType == ItemType.Sword_2H
                || itemType == ItemType.ThrowingAxe
                || itemType == ItemType.ThrownWeapon
                || itemType == ItemType.Wand
                );
        }



        public void RevealInInventory(Hero hero, int row, int column, int equipmentSlot)
        {

            InventoryLocationMessageData inventorylocation = new InventoryLocationMessageData()
                    {
                        Field0 = hero.Id,
                        Field1 = equipmentSlot,
                        Field2 = new IVector2D()
                        {
                            Field0 = row,
                            Field1 = column,
                        },
                    };

            ACDEnterKnownMessage msg = new ACDEnterKnownMessage()
            {
                Id = 0x003B,
                Field0 = ItemId,
                Field1 = SnoId,
                Field2 = 0x0000001A,
                Field3 = 0x00000001,
                Field4 = null,
                Field5 = inventorylocation,
                Field6 = new GBHandle()
                {
                    Field0 = 0x00000002,
                    Field1 = Gbid,
                },
                Field7 = -1,
                Field8 = -1,
                Field9 = 0x00000001,
                Field10 = 0x00,
            };

            hero.InGameClient.SendMessage(msg);

            Reveal(hero);
        }

        public void Reveal(Hero hero)
        {

            GameClient client = hero.InGameClient;

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


            client.SendMessage(new ACDCollFlagsMessage()
            {
                Id = 0x00A6,
                Field0 = ItemId,
                Field1 = 0x00000080,
            });


            List<NetAttributeKeyValue> netAttributesList = new List<NetAttributeKeyValue>();


            foreach (NetAttribute attr in AttributeList)
            {
                NetAttributeKeyValue netAttr = null;
                if (attr.MessageType != -1)
                {
                    netAttr = new NetAttributeKeyValue()
                    {
                        Field0 = attr.MessageType,
                        Attribute = GameAttribute.Attributes[attr.Key],
                        Int = attr.IntValue,
                        Float = attr.FloatValue,
                    };
                }
                else
                {
                    netAttr = new NetAttributeKeyValue()
                    {
                        Attribute = GameAttribute.Attributes[attr.Key],
                        Int = unchecked((int)attr.IntValue),
                        Float = attr.FloatValue,
                    };
                }
                netAttributesList.Add(netAttr);
            }


            SendAttributes(netAttributesList, client);

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
                    Field1 = SnoId,
                },
            });

            /*
            foreach (NetAttributeKeyValue attr in netAttributesList){
                client.SendMessage(new AttributeSetValueMessage()
                {                   
                    Id = 0x004C,
                    Field0 = ItemId,
                    Field1 = attr,
                });
            };  */

            client.SendMessage(new PlayEffectMessage()
            {
                Id = 0x007A,
                Field0 = ItemId,
                Field1 = 0x00000027,
            });
           

            client.SendMessage(new ACDInventoryUpdateActorSNO()
            {
                Id = 0x0041,
                Field0 = ItemId,
                Field1 = SnoId,
            });

            client.FlushOutgoingBuffer();

        }


        private void SendAttributes(List<NetAttributeKeyValue> netAttributesList, GameClient client)
        {

            List<NetAttributeKeyValue> tmpList = new List<NetAttributeKeyValue>(netAttributesList);

            while (tmpList.Count > 0)
            {
                int selectCount = (tmpList.Count > 15) ? 15 : tmpList.Count;
                client.SendMessage(new AttributesSetValuesMessage()
                {
                    Id = 0x004D,
                    Field0 = ItemId,
                    atKeyVals = tmpList.GetRange(0, selectCount).ToArray(),
                });
                tmpList.RemoveRange(0, selectCount);


            }
        }


    }
}
