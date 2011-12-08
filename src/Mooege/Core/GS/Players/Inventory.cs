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

using Mooege.Common;
using Mooege.Common.Logging;
using Mooege.Core.GS.Items;
using Mooege.Net.GS;
using Mooege.Net.GS.Message;
using Mooege.Net.GS.Message.Definitions.Inventory;
using Mooege.Net.GS.Message.Fields;
using Mooege.Net.GS.Message.Definitions.ACD;
using Mooege.Core.GS.Common;
using Mooege.Common.MPQ.FileFormats;
using Mooege.Net.GS.Message.Definitions.Stash;
using Mooege.Core.GS.Objects;
using Mooege.Common.Helpers;
using Mooege.Net.GS.Message.Definitions.Misc;
using System.Collections.Generic;
using System.Linq;
using Mooege.Core.GS.Items.Implementations;

namespace Mooege.Core.GS.Players
{

    public class Inventory : IMessageConsumer, IRevealable
    {
        static readonly Logger Logger = LogManager.CreateLogger();

        // Access by ID
        private readonly Player _owner; // Used, because most information is not in the item class but Actors managed by the world

        private Equipment _equipment;
        private InventoryGrid _inventoryGrid;
        private InventoryGrid _stashGrid;
        // backpack for spellRunes, their Items are kept in equipment
        private uint[] _skillSocketRunes;

        public Inventory(Player owner)
        {
            this._owner = owner;
            this._equipment = new Equipment(owner);
            this._inventoryGrid = new InventoryGrid(owner, owner.Attributes[GameAttribute.Backpack_Slots]/10, 10);
            this._stashGrid = new InventoryGrid(owner, owner.Attributes[GameAttribute.Shared_Stash_Slots]/7, 7, (int) EquipmentSlotId.Stash);
            this._skillSocketRunes = new uint[6];
        }

        private void AcceptMoveRequest(Item item)
        {
           /* _owner.InGameClient.SendMessage(new ACDInventoryPositionMessage()
            {
                ItemId = item.DynamicID,
                InventoryLocation = item.InventoryLocationMessage,
                Field2 = 1 // what does this do?  // 0 - source item not disappearing from inventory, 1 - Moving, any other possibilities? its an int32
            }); */
        }


        /// <summary>
        /// Refreshes the visual appearance of the hero
        /// </summary>
        public void SendVisualInventory(Player player)
         {
             var message = new VisualInventoryMessage()
                               {
                                   ActorID = this._owner.DynamicID,
                                   EquipmentList = new VisualEquipment()
                                                       {
                                                           Equipment = this._equipment.GetVisualEquipment()
                                                       },
                               };

             //player.InGameClient.SendMessage(message);
             player.World.BroadcastGlobal(message);
         }


        public bool HasInventorySpace(Item item)
        {
            return _inventoryGrid.HasFreeSpace(item);
        }

        public void SpawnItem(Item item)
        {
            _inventoryGrid.AddItem(item);
        }
        
        /// <summary>
        /// Picks an item up after client request
        /// </summary>
        /// <returns>true if the item was picked up, or false if the player could not pick up the item.</returns>
        public bool PickUp(Item item)
        {
            System.Diagnostics.Debug.Assert(!_inventoryGrid.Contains(item) && !_equipment.IsItemEquipped(item), "Item already in inventory");
            // TODO: Autoequip when equipment slot is empty

            // If Item is Stackable try to add the amount
            if (item.IsStackable())
            {
                // Find items of same type (GBID) and try to add it to one of them
                List<Item> baseItems = FindSameItems(item.GBHandle.GBID);
                foreach (Item baseItem in baseItems)
                {
                    if (baseItem.Attributes[GameAttribute.ItemStackQuantityLo] + item.Attributes[GameAttribute.ItemStackQuantityLo] < baseItem.ItemDefinition.MaxStackAmount)
                    {
                        baseItem.Attributes[GameAttribute.ItemStackQuantityLo] += item.Attributes[GameAttribute.ItemStackQuantityLo];
                        baseItem.Attributes.SendChangedMessage(_owner.InGameClient);

                        // Item amount successful added. Don't place item in inventory instead destroy it.
                        item.Destroy();
                        return true;
                    }
                }
            }

            bool success = false;
            if (!_inventoryGrid.HasFreeSpace(item))
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
                item.CurrentState = ItemState.PickingUp;
                if (item.HasWorldLocation && item.World != null)
                {
                    item.Owner = _owner;
                    item.World.Leave(item);

                }

                _inventoryGrid.AddItem(item);

                if (_owner.GroundItems.ContainsKey(item.DynamicID))
                    _owner.GroundItems.Remove(item.DynamicID);
                success = true;
                item.CurrentState = ItemState.Normal;
                AcceptMoveRequest(item);
            }
          
            return success;
        }

        private List<Item> FindSameItems(int gbid)
        {
            return _inventoryGrid.Items.Values.Where(i => i.GBHandle.GBID == gbid).ToList();
        }
       
        public void BuyItem(Item originalItem)
        {
            // TODO: Create a copy instead of random.
            var newItem = ItemGenerator.CreateItem(_owner, originalItem.ItemDefinition);
            _inventoryGrid.AddItem(newItem);
        }

        /// <summary>
        /// Handles a request to move an item within the inventory.
        /// This covers moving items within the backpack, from equipment
        /// slot to backpack and from backpack to equipment slot
        /// </summary>
        public void HandleInventoryRequestMoveMessage(InventoryRequestMoveMessage request)
        {
            // TODO Normal inventory movement does not require setting of inv loc msg! Just Tick. /fasbat
            Item item = GetItem(request.ItemID);
            if (item == null)
                return;
            // Request to equip item from backpack
            if (request.Location.EquipmentSlot != 0 && request.Location.EquipmentSlot != (int) EquipmentSlotId.Stash)
            {
                var sourceGrid = (item.InvLoc.EquipmentSlot == 0 ? _inventoryGrid :
                    item.InvLoc.EquipmentSlot == (int)EquipmentSlotId.Stash ? _stashGrid : null);

                System.Diagnostics.Debug.Assert((sourceGrid != null && sourceGrid.Contains(request.ItemID)) || _equipment.IsItemEquipped(request.ItemID), "Request to equip unknown item");

                int targetEquipSlot = request.Location.EquipmentSlot;

                if (IsValidEquipmentRequest(item, targetEquipSlot))
                {
                    Item oldEquipItem = _equipment.GetEquipment(targetEquipSlot);

                    // check if equipment slot is empty
                    if (oldEquipItem == null)
                    {
                        // determine if item is in backpack or switching item from position with target originally empty
                        if (sourceGrid != null)
                            sourceGrid.RemoveItem(item);
                        else
                            _equipment.UnequipItem(item);

                        _equipment.EquipItem(item, targetEquipSlot);
                        AcceptMoveRequest(item);
                    }
                    else
                    {
                        // check if item is already equipped at another equipmentslot
                        if (_equipment.IsItemEquipped(item))
                        {
                            // switch both items
                            if (!IsValidEquipmentRequest(oldEquipItem, item.EquipmentSlot))
                                return;

                            int oldEquipmentSlot = _equipment.UnequipItem(item);
                            _equipment.EquipItem(item, targetEquipSlot);
                            _equipment.EquipItem(oldEquipItem, oldEquipmentSlot);

                        }
                        else
                        {
                            // equip item and place other item in the backpack
                            sourceGrid.RemoveItem(item);
                            _equipment.EquipItem(item, targetEquipSlot);
                            sourceGrid.AddItem(oldEquipItem);
                        }
                        AcceptMoveRequest(item);
                        AcceptMoveRequest(oldEquipItem);
                    }

                    SendVisualInventory(this._owner);
                }
            }

            // Request to move an item (from backpack or equipmentslot)
            else
            {
                if (request.Location.EquipmentSlot == 0)
                {
                    // check if not unsocketting rune
                    for (int i = 0; i < _skillSocketRunes.Length; i++)
                    {
                        if (_skillSocketRunes[i] == request.ItemID)
                        {
                            if (_inventoryGrid.FreeSpace(item, request.Location.Row, request.Location.Column))
                            {
                                RemoveRune(i);
                                _inventoryGrid.AddItem(item, request.Location.Row, request.Location.Column);
                                if (item.InvLoc.EquipmentSlot != request.Location.EquipmentSlot)
                                    AcceptMoveRequest(item);
                            }
                            return;
                        }
                    }
                }

                var destGrid = (request.Location.EquipmentSlot == 0 ? _inventoryGrid : _stashGrid);

                if (destGrid.FreeSpace(item, request.Location.Row, request.Location.Column))
                {
                    if (_equipment.IsItemEquipped(item))
                    {
                        _equipment.UnequipItem(item); // Unequip the item
                        SendVisualInventory(this._owner);
                    }
                    else
                    {
                        var sourceGrid = (item.InvLoc.EquipmentSlot == 0 ? _inventoryGrid : _stashGrid);
                        sourceGrid.RemoveItem(item);
                    }
                    destGrid.AddItem(item, request.Location.Row, request.Location.Column);
                    if (item.InvLoc.EquipmentSlot != request.Location.EquipmentSlot) 
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

            ItemTypeTable type = item.ItemType;

            if (equipmentSlot == (int)EquipmentSlotId.Main_Hand)
            {
                // useful for 1hand + shield switching, this is to avoid shield to be go to main hand
                if (!Item.IsWeapon(type))
                    return false;

                if (Item.Is2H(type))
                {
                    Item itemOffHand = _equipment.GetEquipment(EquipmentSlotId.Off_Hand);
                    if (itemOffHand != null)
                    {
                        _equipment.UnequipItem(itemOffHand);
                        if (!_inventoryGrid.AddItem(itemOffHand))
                        {
                            _equipment.EquipItem(itemOffHand, (int)EquipmentSlotId.Off_Hand);
                            return false;
                        }
                        AcceptMoveRequest(itemOffHand);
                    }
                }
            }
            else if (equipmentSlot == (int)EquipmentSlotId.Off_Hand)
            {
                Item itemMainHand = _equipment.GetEquipment(EquipmentSlotId.Main_Hand);
                if (Item.Is2H(type))
                {
                    //remove object first to make room for possible unequiped item
                    _inventoryGrid.RemoveItem(item);

                    if(itemMainHand != null)
                    {
                        _equipment.UnequipItem(itemMainHand);
                        _inventoryGrid.AddItem(itemMainHand);
                        AcceptMoveRequest(itemMainHand);
                    }

                    _equipment.EquipItem(item, (int)EquipmentSlotId.Main_Hand);
                    AcceptMoveRequest(item);

                    SendVisualInventory(this._owner);
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

        /// <summary>
        /// Transfers an amount from one stack to a free space
        /// </summary>
        public void OnInventorySplitStackMessage(InventorySplitStackMessage msg)
        {
            Item itemFrom = GetItem((uint)msg.FromID);
            itemFrom.Attributes[GameAttribute.ItemStackQuantityLo] -= (int)msg.Amount;
            itemFrom.Attributes.SendChangedMessage(_owner.InGameClient);

            Item item = ItemGenerator.CreateItem(_owner, itemFrom.ItemDefinition);
            item.Attributes[GameAttribute.ItemStackQuantityLo] = (int)msg.Amount;

            InventoryGrid targetGrid = (msg.InvLoc.EquipmentSlot == (int)EquipmentSlotId.Stash) ? _stashGrid : _inventoryGrid;
            targetGrid.AddItem(item, msg.InvLoc.Row, msg.InvLoc.Column);
        }

        /// <summary>
        /// Transfers an amount from one stack to another
        /// </summary>
        public void OnInventoryStackTransferMessage(InventoryStackTransferMessage msg)
        {
            Item itemFrom = GetItem(msg.FromID);
            Item itemTo = GetItem(msg.ToID);

            itemFrom.Attributes[GameAttribute.ItemStackQuantityLo] -= (int)msg.Amount;
            itemTo.Attributes[GameAttribute.ItemStackQuantityLo] += (int)msg.Amount;

            itemFrom.Attributes.SendChangedMessage(_owner.InGameClient);
            itemTo.Attributes.SendChangedMessage(_owner.InGameClient);
        }

        private void OnInventoryDropItemMessage(InventoryDropItemMessage msg)
        {
            var item = GetItem(msg.ItemID);
            if (item == null)
                return; // TODO: Throw smthg? /fasbat

            if (_equipment.IsItemEquipped(item))
            {
                _equipment.UnequipItem(item);
                SendVisualInventory(this._owner);
            }
            else
            {
                var sourceGrid = (item.InvLoc.EquipmentSlot == 0 ? _inventoryGrid : _stashGrid);
                sourceGrid.RemoveItem(item);
            }

            item.CurrentState = ItemState.Dropping;
            item.Unreveal(_owner);
            item.SetNewWorld(_owner.World);
            item.Drop(null, _owner.Position);
            item.CurrentState = ItemState.Normal;
            AcceptMoveRequest(item);
        }

        public void Consume(GameClient client, GameMessage message)
        {
            if (message is InventoryRequestMoveMessage) HandleInventoryRequestMoveMessage(message as InventoryRequestMoveMessage);
            else if (message is InventorySplitStackMessage) OnInventorySplitStackMessage(message as InventorySplitStackMessage);
            else if (message is InventoryStackTransferMessage) OnInventoryStackTransferMessage(message as InventoryStackTransferMessage);
            else if (message is InventoryDropItemMessage) OnInventoryDropItemMessage(message as InventoryDropItemMessage);
            else if (message is InventoryRequestUseMessage) OnInventoryRequestUseMessage(message as InventoryRequestUseMessage);
            else if (message is RequestBuySharedStashSlotsMessage) OnBuySharedStashSlots(message as RequestBuySharedStashSlotsMessage);
            else if (message is InventoryRequestUseMessage) OnInventoryRequestUseMessage(message as InventoryRequestUseMessage);
            else if (message is RequestUseCauldronOfJordanMessage) OnUseCauldronOfJordanMessage(message as RequestUseCauldronOfJordanMessage);
            else if (message is RequestUseNephalemCubeMessage) OnUseNephalmCubeMessage(message as RequestUseNephalemCubeMessage);
            else return;
        }

        private void OnBuySharedStashSlots(RequestBuySharedStashSlotsMessage requestBuySharedStashSlotsMessage)
        {
            // TODO: Take that money away ;)
            _owner.Attributes[GameAttribute.Shared_Stash_Slots] += 14;
            _owner.Attributes.BroadcastChangedIfRevealed();
            _stashGrid.ResizeGrid(_owner.Attributes[GameAttribute.Shared_Stash_Slots] / 7, 7);
        }

        // TODO: The inventory's gold item should not be created here. /komiga
        public void PickUpGold(uint itemID)
        {
            Item collectedItem = _owner.World.GetItem(itemID);
            Item sumGoldItem = _equipment.AddGoldItem(collectedItem);
        }

        private void OnInventoryRequestUseMessage(InventoryRequestUseMessage inventoryRequestUseMessage)
        {
            uint targetItemId = inventoryRequestUseMessage.UsedOnItem;
            uint usedItemId = inventoryRequestUseMessage.UsedItem;
            int actionId = inventoryRequestUseMessage.Field1;
            Item usedItem = GetItem(usedItemId);
            Item targetItem = GetItem(targetItemId);

            usedItem.OnRequestUse(_owner, targetItem, actionId, inventoryRequestUseMessage.Location);
        }

        public void DestroyInventoryItem(Item item)
        {
            if (_equipment.IsItemEquipped(item))
            {
                _equipment.UnequipItem(item);
                SendVisualInventory(_owner);
            }
            else
            {
                _inventoryGrid.RemoveItem(item);
            }

            item.Destroy();
        }

        public bool Reveal(Player player)
        {
            _equipment.Reveal(player);
            if (player == _owner)
            {
                _inventoryGrid.Reveal(player);
                _stashGrid.Reveal(player);
            }
            return true;
        }

        public bool Unreveal(Player player)
        {
            _equipment.Unreveal(player);
            if (player == _owner)
            {
                _inventoryGrid.Unreveal(player);
                _stashGrid.Unreveal(player);
            }

            return true;
        }

        public Item GetItem(uint itemId)
        {
            Item result;
            if (!_inventoryGrid.Items.TryGetValue(itemId, out result) &&
                !_stashGrid.Items.TryGetValue(itemId, out result) &&
                !_equipment.Items.TryGetValue(itemId, out result))
            {
                return null;
            }
            return result;
        }

        /// <summary>
        /// Returns rune in skill's socket
        /// </summary>
        /// <param name="skillIndex"></param>
        /// <returns></returns>
        public Item GetRune(int skillIndex)
        {
            if ((skillIndex < 0) || (skillIndex > 5))
            {
                return null;
            }
            if (_skillSocketRunes[skillIndex] == 0)
            {
                return null;
            }
            return _equipment.GetItem(_skillSocketRunes[skillIndex]);
        }

        /// <summary>
        /// Visually adds rune to skill (move from backpack to runes' slot)
        /// </summary>
        /// <param name="rune"></param>
        /// <param name="powerSNOId"></param>
        /// <param name="skillIndex"></param>
        public void SetRune(Item rune, int powerSNOId, int skillIndex)
        {
            if ((skillIndex < 0) || (skillIndex > 5))
            {
                return;
            }
            if (rune == null)
            {
                _skillSocketRunes[skillIndex] = 0;
                return;
            }
            if (_inventoryGrid.Items.ContainsKey(rune.DynamicID))
            {
                _inventoryGrid.RemoveItem(rune);
            }
            else
            {
                // unattuned rune changes to attuned w/o getting into inventory
                rune.World.Leave(rune);
                rune.Reveal(_owner);
            }
            _equipment.Items.Add(rune.DynamicID, rune);
            _skillSocketRunes[skillIndex] = rune.DynamicID;
            // will set only one of these to rank
            _owner.Attributes[GameAttribute.Rune_A, powerSNOId] = rune.Attributes[GameAttribute.Rune_A];
            _owner.Attributes[GameAttribute.Rune_B, powerSNOId] = rune.Attributes[GameAttribute.Rune_B];
            _owner.Attributes[GameAttribute.Rune_C, powerSNOId] = rune.Attributes[GameAttribute.Rune_C];
            _owner.Attributes[GameAttribute.Rune_D, powerSNOId] = rune.Attributes[GameAttribute.Rune_D];
            _owner.Attributes[GameAttribute.Rune_E, powerSNOId] = rune.Attributes[GameAttribute.Rune_E];
            // position of rune is read from mpq as INDEX of skill in skill kit - loaded in helper /xsochor
            rune.SetInventoryLocation(16, RuneHelper.GetRuneIndexForPower(powerSNOId), 0);
        }

        /// <summary>
        /// Visually removes rune from skill. Also removes effect of that rune
        /// </summary>
        /// <param name="skillIndex"></param>
        /// <returns></returns>
        public Item RemoveRune(int skillIndex)
        {
            if ((skillIndex < 0) || (skillIndex > 5))
            {
                return null;
            }
            Item rune = GetRune(skillIndex);
            if (rune != null)
            {
                _equipment.Items.Remove(rune.DynamicID);
            }
            int powerSNOId = _owner.SkillSet.ActiveSkills[skillIndex];
            _skillSocketRunes[skillIndex] = 0;
            _owner.Attributes[GameAttribute.Rune_A, powerSNOId] = 0;
            _owner.Attributes[GameAttribute.Rune_B, powerSNOId] = 0;
            _owner.Attributes[GameAttribute.Rune_C, powerSNOId] = 0;
            _owner.Attributes[GameAttribute.Rune_D, powerSNOId] = 0;
            _owner.Attributes[GameAttribute.Rune_E, powerSNOId] = 0;
            return rune;
        }


		private void OnUseNephalmCubeMessage(RequestUseNephalemCubeMessage requestUseNephalemCubeMessage)
        {
            Item salvageItem = GetItem(requestUseNephalemCubeMessage.ActorID);
            NephalemCube.OnUse(_owner, salvageItem);
        }

        private void OnUseCauldronOfJordanMessage(RequestUseCauldronOfJordanMessage requestUseCauldronOfJordanMessage)
        {
            Item sellItem = GetItem(requestUseCauldronOfJordanMessage.ActorID);
            CauldronOfJordan.OnUse(_owner, sellItem);
        }


        public void AddGoldAmount(int amount)
        {
            _equipment.AddGoldAmount(amount);
        }
    }
}