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
using Mooege.Net.GS;
using Mooege.Net.GS.Message;
using Mooege.Net.GS.Message.Definitions.Inventory;
using Mooege.Net.GS.Message.Fields;
using Mooege.Net.GS.Message.Definitions.Misc;
using Mooege.Net.GS.Message.Definitions.ACD;
using Mooege.Core.Common.Items;

namespace Mooege.Core.GS.Player
{

    public class Inventory : IMessageConsumer
    {
        static readonly Logger Logger = LogManager.CreateLogger();

        // Access by ID
        public Dictionary<uint, Item> Items { get; private set; } // Not needed atm. Whats the suppose of it?
        private readonly Mooege.Core.GS.Player.Player _owner; // Used, because most information is not in the item class but Actors managed by the world

        private Equipment _equipment;
        private Stash _inventoryStash;

        public Inventory(Mooege.Core.GS.Player.Player owner)
        {
            this._owner = owner;
            this.Items = new Dictionary<uint, Item>();
            this._equipment = new Equipment(owner);
            this._inventoryStash = new Stash(owner, 6, 10);
        }

        private void AcceptMoveRequest(Item item)
        {
            /*_owner.InGameClient.SendMessage(new ACDInventoryPositionMessage()
            {
                ItemID = item.DynamicID,
                InventoryLocation = item.InventoryLocationMessage,
                Field2 = 1 // what does this do?  // 0 - source item not disappearing from inventory, 1 - Moving, any other possibilities? its an int32
            });*/
        }


         /// <summary>
        /// Refreshes the visual appearance of the hero
        /// </summary>
        public void SendVisualInvetory(Player player)
         {
             var message = new VisualInventoryMessage()
                               {
                                   ActorID = this._owner.DynamicID,
                                   EquipmentList = new VisualEquipment()
                                                       {
                                                           Equipment = this._equipment.GetVisualEquipment()
                                                       },
                               };

             player.InGameClient.SendMessage(message);             
         }
        
        /// <summary>
        /// Picks an item up after client request
        /// </summary>
        /// <returns>true if the item was picked up, or false if the player could not pick up the item.</returns>
        public bool PickUp(Item item)
        {
            System.Diagnostics.Debug.Assert(!_inventoryStash.Contains(item) && !_equipment.IsItemEquipped(item), "Item already in inventory");
            // TODO: Autoequip when equipment slot is empty

            bool success = false;
            if (!_inventoryStash.HasFreeSpace(item))
            {
                // Inventory full
                _owner.InGameClient.SendMessage(new ACDPickupFailedMessage()
                {
                    ItemID = item.DynamicID,
                    Reason = ACDPickupFailedMessage.Reasons.InventoryFull
                });
            }
            else
            {
                _inventoryStash.AddItem(item);

                if (_owner.GroundItems.ContainsKey(item.DynamicID))
                    _owner.GroundItems.Remove(item.DynamicID);
                success = true;
            }

            AcceptMoveRequest(item);
            return success;
        }

        /// <summary>
        /// Handles a request to move an item within the inventory.
        /// This covers moving items within the backpack, from equipment
        /// slot to backpack and from backpack to equipment slot
        /// </summary>
        public void HandleInventoryRequestMoveMessage(InventoryRequestMoveMessage request)
        {
            Item item = _owner.World.GetItem(request.ItemID);
            // Request to equip item from backpack
            if (request.Location.EquipmentSlot != 0)
            {
                System.Diagnostics.Debug.Assert(_inventoryStash.Contains(request.ItemID) || _equipment.IsItemEquipped(request.ItemID), "Request to equip unknown item");
                               
                int targetEquipSlot = request.Location.EquipmentSlot;
                if (IsValidEquipmentRequest(item, targetEquipSlot))
                {
                    Item oldEquipItem = _equipment.GetEquipment(targetEquipSlot);
                    
                    // check if equipment slot is empty
                    if (oldEquipItem == null)
                    {
                        _inventoryStash.RemoveItem(item);
                        _equipment.EquipItem(item, targetEquipSlot);
                        AcceptMoveRequest(item);
                    }
                    else
                    {
                        // check if item is already equipped at another equipmentslot
                        if (_equipment.IsItemEquipped(item))
                        {
                            // switch both items
                            int oldEquipmentSlot = _equipment.UnequipItem(item);
                            _equipment.EquipItem(item, targetEquipSlot);
                            _equipment.EquipItem(oldEquipItem, oldEquipmentSlot);
                        }
                        else
                        {
                            // equip item and place other item in the backpack
                            _inventoryStash.RemoveItem(item);
                            _equipment.EquipItem(item, targetEquipSlot);                            
                            _inventoryStash.AddItem(oldEquipItem);
                        }
                        AcceptMoveRequest(item);
                        AcceptMoveRequest(oldEquipItem);
                    }

                    SendVisualInvetory(this._owner);
                }
            }

            // Request to move an item (from backpack or equipmentslot)
            else
            {
                if (_inventoryStash.FreeSpace(item, request.Location.Row, request.Location.Column))
                {
                    if (_equipment.IsItemEquipped(item))
                    {
                        _equipment.UnequipItem(item); // Unequip the item
                        SendVisualInvetory(this._owner);
                    }
                    else
                    {
                        _inventoryStash.RemoveItem(item);
                    }
                    _inventoryStash.AddItem(item, request.Location.Row, request.Location.Column);
                    AcceptMoveRequest(item);                   
                }
            }
        }

        /// <summary>
        /// Checks if Item can be equipped at that slot. Handels equipment for Two-Handed-Weapons.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="equipmentSlot"></param>
        /// <returns></returns>
        private bool IsValidEquipmentRequest(Item item, int equipmentSlot)
        {

            ItemType type = item.ItemType;
                
            if (equipmentSlot == (int)EquipmentSlotId.Main_Hand)
            {
                if (Item.Is2H(type))
                {
                    Item itemOffHand = _equipment.GetEquipment(EquipmentSlotId.Off_Hand);                    
                    if (itemOffHand != null)
                    {                       
                        _equipment.UnequipItem(itemOffHand);                      
                        _inventoryStash.AddItem(itemOffHand);
                        AcceptMoveRequest(itemOffHand);
                    }
                }
            }
            else if (equipmentSlot == (int)EquipmentSlotId.Off_Hand)
            {
                Item itemMainHand = _equipment.GetEquipment(EquipmentSlotId.Main_Hand);                
                if (Item.Is2H(type))
                {   
                    if(itemMainHand != null)
                    {
                        _equipment.UnequipItem(itemMainHand);
                        _inventoryStash.AddItem(itemMainHand);
                        AcceptMoveRequest(itemMainHand);
                    }
                    _inventoryStash.RemoveItem(item);
                    _equipment.EquipItem(item, (int)EquipmentSlotId.Main_Hand);
                    AcceptMoveRequest(item); 
                   
                    SendVisualInvetory(this._owner);
                    // All equipment commands are executed. the original EquipmentRequest is invalid at this moment
                    return false;
                }
                             
                if (itemMainHand != null)
                {
                    if (Item.Is2H(itemMainHand.ItemType))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public void OnInventorySplitStackMessage(InventorySplitStackMessage msg)
        {
            // TODO need to create and introduce a new item that is of the same type as the source
        }

        /// <summary>
        /// Transfers an amount from one stack to another
        /// </summary>
        public void OnInventoryStackTransferMessage(InventoryStackTransferMessage msg)
        {
            Item itemFrom = _owner.World.GetItem(msg.FromID);
            Item itemTo = _owner.World.GetItem(msg.ToID);

            itemFrom.Attributes[GameAttribute.ItemStackQuantityLo] -= (int)msg.Amount;
            itemTo.Attributes[GameAttribute.ItemStackQuantityLo] -= (int)msg.Amount;
            

            // TODO: This needs to change the attribute on the item itself. /komiga
            // Update source
            GameAttributeMap attributes = new GameAttributeMap();
            attributes[GameAttribute.ItemStackQuantityLo] = itemFrom.Attributes[GameAttribute.ItemStackQuantityLo];
            attributes.SendMessage(_owner.InGameClient, itemFrom.DynamicID);

            // TODO: This needs to change the attribute on the item itself. /komiga
            // Update target
            attributes = new GameAttributeMap();
            attributes[GameAttribute.ItemStackQuantityLo] = itemTo.Attributes[GameAttribute.ItemStackQuantityLo];
            attributes.SendMessage(_owner.InGameClient, itemTo.DynamicID);
        }

        private void OnInventoryDropItemMessage(InventoryDropItemMessage msg)
        {
            Item item = _owner.World.GetItem(msg.ItemID);
            if (_equipment.IsItemEquipped(item))
            {
                _equipment.UnequipItem(item);
                SendVisualInvetory(this._owner);
            }
            else
            {
                _inventoryStash.RemoveItem(item);
            }            
            item.Drop(null, _owner.Position);
            AcceptMoveRequest(item);
        }

        public void Consume(GameClient client, GameMessage message)
        {
            if (message is InventoryRequestMoveMessage) HandleInventoryRequestMoveMessage(message as InventoryRequestMoveMessage);
            else if (message is InventorySplitStackMessage) OnInventorySplitStackMessage(message as InventorySplitStackMessage);
            else if (message is InventoryStackTransferMessage) OnInventoryStackTransferMessage(message as InventoryStackTransferMessage);
            else if (message is InventoryDropItemMessage) OnInventoryDropItemMessage(message as InventoryDropItemMessage);
            else return;
        }

        // TODO: The inventory's gold item should not be created here. /komiga
        public void PickUpGold(uint itemID)
        {
            Item collectedItem = _owner.GroundItems[itemID];
            Item sumGoldItem = _equipment.AddGoldItem(collectedItem);

            GameAttributeMap attributes = new GameAttributeMap();
            attributes[GameAttribute.ItemStackQuantityLo] = sumGoldItem.Attributes[GameAttribute.ItemStackQuantityLo];
            attributes.SendMessage(_owner.InGameClient, sumGoldItem.DynamicID);
        }
    }
}
