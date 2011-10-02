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
using Mooege.Net.GS.Message.Fields;
using Mooege.Net.GS;
using Mooege.Net.GS.Message.Definitions.ACD;
using Mooege.Net.GS.Message.Definitions.Effect;
using Mooege.Net.GS.Message.Definitions.Misc;
using Mooege.Net.GS.Message;
using Mooege.Net.GS.Message.Definitions.Attribute;
using Mooege.Core.GS.Universe;

namespace Mooege.Core.Common.Items
{

    public enum ItemType
    {

        
        Helm, Gloves, Boots, Belt, Shoulders, Pants, Bracers, Shield, Quiver, Orb, 
        Axe_1H, Axe_2H, CombatStaff_2H, Dagger,  Mace_1H, Mace_2H, Sword_1H, 
        Sword_2H, Bow, Crossbow, Spear, Staff, Polearm, Wand, Ring, FistWeapon_1H,
        HealthPotion
        
        /*
         * Not working  at the moment
         * 
         *  // ChestArmor                   --> does not work because there are missing itemnames for normal mode, just for nightmare and hell and some "a" and "b" variants... -> need to figure out which should be used
         *  // ThrownWeapon, ThrowingAxe    --> does not work because there are no snoId in Actors.txt
         */


    }

    public class Item
    {

        public int ItemId { get; set; }
        public int Gbid { get; set; }
        public int SnoId { get; set; }
        public ItemType Type { get; set; }
        public int Count { get; set;  }             // <- amount?


        public List<Affix> AffixList { get; set; }
        public List<NetAttributeKeyValue> AttributeList { get; set; }


        public Item(int id, uint gbid, ItemType type)
        {
            ItemId = id;
            Gbid = unchecked((int)gbid);
            Count = 1;
            Type = type;

            AffixList = new List<Affix>();
            AttributeList = new List<NetAttributeKeyValue>();
        }

        // There are 2 VisualItemClasses... any way to use the builder to create a D3 Message?
        public VisualItem CreateVisualItem()
        {
            return new VisualItem()
            {
                GbId = Gbid,
                Field1 = 0,
                Field2 = 0,
                Field3 = -1
            };

        }

        public static bool isPotion(ItemType itemType)
        {
            return (itemType == ItemType.HealthPotion);
        }

        public static bool isWeapon(ItemType itemType)
        {
            return (itemType == ItemType.Axe_1H
                || itemType == ItemType.Axe_2H
                || itemType == ItemType.Bow
                || itemType == ItemType.CombatStaff_2H
                || itemType == ItemType.Crossbow
                || itemType == ItemType.Dagger
                || itemType == ItemType.FistWeapon_1H
                || itemType == ItemType.Mace_1H
                || itemType == ItemType.Mace_2H
                || itemType == ItemType.Orb
                || itemType == ItemType.Polearm
                || itemType == ItemType.Spear
                || itemType == ItemType.Staff
                || itemType == ItemType.Sword_1H
                || itemType == ItemType.Sword_2H
                //|| itemType == ItemType.ThrowingAxe
                //|| itemType == ItemType.ThrownWeapon
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


            SendAttributes(AttributeList, client);

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
             * in the original dump this was sent. But don't know for what its good for
             * 
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
            // Attributes can't be send all together
            // must be split up to part of max 15 attributes at once
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
