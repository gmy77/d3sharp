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
using Mooege.Common;
using Mooege.Core.GS.Actors;
using Mooege.Core.GS.Common.Types.Math;
using Mooege.Core.GS.Map;
using Mooege.Core.Common.Items.ItemCreation;
using Mooege.Net.GS.Message.Definitions.World;
using Mooege.Net.GS.Message.Fields;
using Mooege.Net.GS.Message.Definitions.Effect;
using Mooege.Net.GS.Message;

// TODO: This entire namespace belongs in GS. Bnet only needs a certain representation of items whereas nearly everything here is GS-specific

namespace Mooege.Core.Common.Items
{
    public enum ItemType
    {
        Unknown, Helm, Gloves, Boots, Belt, Shoulders, Pants, Bracers, Shield, Quiver, Orb,
        Axe_1H, Axe_2H, CombatStaff_2H, Staff, Dagger, Mace_1H, Mace_2H, Sword_1H,
        Sword_2H, Crossbow, Bow, Spear, Polearm, Wand, Ring, FistWeapon_1H, ThrownWeapon, ThrowingAxe, ChestArmor, 
        HealthPotion, Gold, HealthGlobe, Dye, Elixir, Charm, Scroll, SpellRune, Rune, 
        Amethyst, Emarald, Ruby, Emerald, Topaz, Skull, Backpack, Potion, Amulet, Scepter, Rod, Journal        

        // Not working at the moment:
        // ThrownWeapon, ThrowingAxe - does not work because there are no snoId in Actors.txt. Do they actually drop in the D3 beta? /angerwin?
        // Diamond, Sapphire - I realised some days ago, that the Item type Diamond and Shappire (maybe not the only one) causes client crash and BAD GBID messages, although they actually have SNO IDs. /angerwin
    }

    public class Item : Actor
    {
        private static readonly Logger Logger = LogManager.CreateLogger();

        public override ActorType ActorType { get { return ActorType.Item; } }

        public GS.Player.Player Owner { get; set; } // Only set when the player has the item in its inventory. /komiga

        public ItemType ItemType { get; set; }


        public int EquipmentSlot { get; private set; }
        public Vector2D InventoryLocation { get; private set; } // Column, row; NOTE: Call SetInventoryLocation() instead of setting fields on this

        public override bool HasWorldLocation
        {
            get { return this.Owner == null; }
        }

        public override InventoryLocationMessageData InventoryLocationMessage
        {
            get
            {
                return new InventoryLocationMessageData
                {
                    OwnerID = (this.Owner != null) ? this.Owner.DynamicID : 0,
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
                    EquipmentSlot = this.EquipmentSlot,
                    Row = this.InventoryLocation.Y,
                    Column = this.InventoryLocation.X
                };
            }
        }

        public Item(World world, int actorSNO, int gbid, ItemType type)
            : base(world, world.NewActorID)
        {
            this.ActorSNO = actorSNO;
            this.GBHandle.Type = (int)GBHandleType.Gizmo;
            this.GBHandle.GBID = gbid;
            this.ItemType = type;
            this.EquipmentSlot = 0;
            this.InventoryLocation = new Vector2D { X = 0, Y = 0 };

            this.Field2 = 0x00000000;
            this.Field3 = 0x00000000;
            this.Field7 = 0;
            this.Field8 = 0;
            this.Field9 = 0x00000000;
            this.Field10 = 0x00;

            List<IItemAttributeCreator> attributeCreators = new AttributeCreatorFactory().Create(type);
            foreach (IItemAttributeCreator creator in attributeCreators)
            {
                creator.CreateAttributes(this);
            }

            this.World.Enter(this); // Enter only once all fields have been initialized to prevent a run condition
        }

        // There are 2 VisualItemClasses... any way to use the builder to create a D3 Message?
        public VisualItem CreateVisualItem()
        {
            return new VisualItem()
            {
                GbId = this.GBHandle.GBID,
                Field1 = Attributes[GameAttribute.DyeType],
                Field2 = 0,
                Field3 = -1
            };
        }

        public static bool IsPotion(ItemType itemType)
        {
            return (itemType == ItemType.Potion);
        }

        public static bool IsAccessory(ItemType itemType)
        {
            return (itemType == ItemType.Ring || itemType == ItemType.Belt || itemType == ItemType.Amulet);
        }

        public static bool IsRuneOrJewel(ItemType itemType)
        {
            return (itemType == ItemType.Amethyst || itemType == ItemType.Ruby || itemType == ItemType.Emerald || itemType == ItemType.Topaz || itemType == ItemType.Rune);
        }

        public static bool IsJournalOrScroll(ItemType itemType)
        {
            return (itemType == ItemType.Journal || itemType == ItemType.Scroll);
        }

        public static bool IsDye(ItemType itemType)
        {
            return (itemType == ItemType.Dye);
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

        public static bool Is2H(ItemType itemType)
        {
            return (itemType == ItemType.Sword_2H
                || itemType == ItemType.Axe_2H
                || itemType == ItemType.Mace_2H
                || itemType == ItemType.CombatStaff_2H
                || itemType == ItemType.Staff
                || itemType == ItemType.Polearm);
        }


        public void SetInventoryLocation(int equipmentSlot, int column, int row)
        {
            this.EquipmentSlot = equipmentSlot;
            this.InventoryLocation.X = column;
            this.InventoryLocation.Y = row;
            if (this.Owner != null)
                this.Owner.InGameClient.SendMessage(this.ACDInventoryPositionMessage);
        }

        public void Drop(Mooege.Core.GS.Player.Player owner, Vector3D position)
        {
            this.Owner = owner;
            this.Position = position;
            // TODO: Notify the world so that players get the state change
        }

        public override void OnTargeted(Mooege.Core.GS.Player.Player player, TargetMessage message)
        {
            //Logger.Trace("OnTargeted");
            player.Inventory.PickUp(this);
        }

        public override bool Reveal(Mooege.Core.GS.Player.Player player)
        {
            if (!base.Reveal(player))
                return false;

            // Drop effect/sound? TODO find out
            player.InGameClient.SendMessage(new PlayEffectMessage()
            {
                ActorId = this.DynamicID,
                Effect = Effect.SecondaryRessourceEffect
            });

             //Why updating with the same sno?
            /*player.InGameClient.SendMessage(new ACDInventoryUpdateActorSNO()
            {
                ItemID = this.DynamicID,
                ItemSNO = this.ActorSNO,
            });
             */

            return true;
        }
    }
}
