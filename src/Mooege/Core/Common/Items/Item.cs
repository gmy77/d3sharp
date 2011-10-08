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

using System.Collections.Generic;
using Mooege.Core.GS.Game;
using Mooege.Core.GS.Actors;
using Mooege.Core.GS.Map;
using Mooege.Net.GS;
using Mooege.Net.GS.Message;
using Mooege.Net.GS.Message.Fields;
using Mooege.Net.GS.Message.Definitions.ACD;
using Mooege.Net.GS.Message.Definitions.Effect;
using Mooege.Net.GS.Message.Definitions.Misc;
using Mooege.Net.GS.Message.Definitions.Attribute;

// TODO: This entire namespace belongs in GS. Bnet only needs a certain representation of items whereas nearly everything here is GS-specific

namespace Mooege.Core.Common.Items
{
    public enum ItemType
    {
        Helm, Gloves, Boots, Belt, Shoulders, Pants, Bracers, Shield, Quiver, Orb,
        Axe_1H, Axe_2H, CombatStaff_2H, Dagger, Mace_1H, Mace_2H, Sword_1H,
        Sword_2H, Bow, Crossbow, Spear, Staff, Polearm, Wand, Ring, FistWeapon_1H,
        HealthPotion, Gold

        /* Not working at the moment:
         *  // ChestArmor                   --> does not work because there are missing itemnames for normal mode, just for nightmare and hell and some "a" and "b" variants... -> need to figure out which should be used
         *  // ThrownWeapon, ThrowingAxe    --> does not work because there are no snoId in Actors.txt
         */
    }

    public class Item : Actor
    {
        public override ActorType ActorType { get { return ActorType.Item; } }

        public Mooege.Core.GS.Player.Player Owner { get; set; } // Only set when the player has the item in its inventory. /komiga

        public ItemType ItemType { get; set; }
        public int Count { get; set; } // <- amount?

        public List<Affix> AffixList { get; set; }

        public int EquipmentSlot;
        public IVector2D InventoryLocation { get; private set; }

        public override bool HasWorldLocation
        {
            get { return this.Owner == null; }
        }

        public override InventoryLocationMessageData InventoryLocationMessage
        {
            get
            {
                return HasWorldLocation
                ? null
                : new InventoryLocationMessageData
                {
                    OwnerID = this.Owner.DynamicID,
                    EquipmentSlot = this.EquipmentSlot,
                    InventoryLocation = this.InventoryLocation
                };
            }
        }

        public InvLoc InvLoc
        {
            get
            {
                return new InvLoc
                {
                    OwnerID = (this.Owner != null) ? this.Owner.DynamicID : 0,
                    Row = this.InventoryLocation.Y,
                    Column = this.InventoryLocation.X
                };
            }
        }

        public Item(World world, int gbid, ItemType type)
            : base(world, world.NewActorID)
        {
            this.GBHandle.Type = (int)GBHandleType.Item;
            this.GBHandle.GBID = gbid;
            this.Count = 1;
            this.ItemType = type;

            this.AffixList = new List<Affix>();

            this.EquipmentSlot = 0;
            this.InventoryLocation = new IVector2D { X = 0, Y = 0 };

            this.Field2 = 0x0000001A;
            this.Field3 = 0x00000001;
            this.Field7 = -1;
            this.Field8 = -1;
            this.Field9 = 0x00000001;
            this.Field10 = 0x00;
            this.World.Enter(this); // Enter only once all fields have been initialized to prevent a run condition
        }

        // There are 2 VisualItemClasses... any way to use the builder to create a D3 Message?
        public VisualItem CreateVisualItem()
        {
            return new VisualItem()
            {
                GbId = this.GBHandle.GBID,
                Field1 = 0,
                Field2 = 0,
                Field3 = -1
            };
        }

        public static bool IsPotion(ItemType itemType)
        {
            return (itemType == ItemType.HealthPotion);
        }

        public static bool IsRing(ItemType itemType)
        {
            return (itemType == ItemType.Ring);
        }

        public static bool IsBelt(ItemType itemType)
        {
            return (itemType == ItemType.Belt);
        }

        public static bool IsWeapon(ItemType itemType)
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

        public void Drop(Mooege.Core.GS.Player.Player owner, Vector3D position)
        {
            this.Owner = owner;
            this.Position = position;
            // TODO: Notify the world so that players get the state change
        }

        public override void OnTargeted(Mooege.Core.GS.Player.Player player)
        {
            player.Inventory.PickUp(this);
        }

        // TODO: Some of this stuff should probably only be set when the item is in the inventory/on the ground
        // FIXME: Hardcoded crap
        public override void Reveal(Mooege.Core.GS.Player.Player player)
        {
            base.Reveal(player);
            GameClient client = player.InGameClient;
            var AffixGBIDs = new int[AffixList.Count];
            for (int i = 0; i < AffixList.Count; i++)
            {
                AffixGBIDs[i] = AffixList[i].AffixGbid;
            }

            client.SendMessage(new AffixMessage()
            {
                ActorID = this.DynamicID,
                Field1 = 0x00000001,
                aAffixGBIDs = AffixGBIDs,

            });

            client.SendMessage(new AffixMessage()
            {
                ActorID = this.DynamicID,
                Field1 = 0x00000002,
                aAffixGBIDs = AffixGBIDs,
            });

            client.SendMessage(new ACDCollFlagsMessage()
            {
                ActorID = this.DynamicID,
                CollFlags = 0x00000080,
            });

            if (this.ItemType == ItemType.Gold)
            {
                Attributes[GameAttribute.Gold] = this.Count;
            }
            this.Attributes.SendMessage(client, this.DynamicID);

            client.SendMessage(new ACDGroupMessage()
            {
                ActorID = this.DynamicID,
                Field1 = -1,
                Field2 = -1,
            });

            client.SendMessage(new ANNDataMessage(Opcodes.ANNDataMessage7)
            {
                ActorID = this.DynamicID,
            });

            client.SendMessage(new SNONameDataMessage()
            {
                Name = new SNOName()
                {
                    Group = 0x00000001, // Same as this.Field9?
                    Handle = this.ActorSNO,
                },
            });

            // Drop effect/sound?
            client.SendMessage(new PlayEffectMessage()
            {
                ActorID = this.DynamicID,
                Field1 = 0x00000027,
            });

            client.SendMessage(new ACDInventoryUpdateActorSNO()
            {
                ItemID = this.DynamicID,
                ItemSNO = this.ActorSNO,
            });
            client.FlushOutgoingBuffer();
        }
    }
}
