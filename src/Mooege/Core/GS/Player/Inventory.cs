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

using System;
using System.Collections.Generic;
using Mooege.Common;
using Mooege.Net.GS;
using Mooege.Net.GS.Message;
using Mooege.Net.GS.Message.Definitions.Inventory;
using Mooege.Net.GS.Message.Fields;
using Mooege.Net.GS.Message.Definitions.Misc;
using Mooege.Net.GS.Message.Definitions.Combat;
using Mooege.Net.GS.Message.Definitions.Attribute;
using Mooege.Net.GS.Message.Definitions.ACD;
using Mooege.Core.Common.Items;

namespace Mooege.Core.GS.Player
{
    // Backpack is organized by adding an item to EVERY slot it fills
    public class Inventory : IMessageConsumer
    {
        static readonly Logger Logger = LogManager.CreateLogger();

        public int Rows { get { return _backpack.GetLength(0); } }
        public int Columns { get { return _backpack.GetLength(1); } }
        public int EquipmentSlots { get { return _equipment.GetLength(0); } }

        // Access by ID
        public Dictionary<uint, Item> Items { get; private set; }

        private uint[] _equipment;      // array of equiped items_id  (not item)
        private uint[,] _backpack;      // backpack array
        private Item _goldItem;

        private readonly Mooege.Core.GS.Player.Player _owner; // Used, because most information is not in the item class but Actors managed by the world

        public struct InventorySize
        {
            public int Width;
            public int Height;
        }

        private struct InventorySlot
        {
            public int Row;
            public int Column;
        }

        public Inventory(Mooege.Core.GS.Player.Player owner)
        {
            this._owner = owner;
            this.Items = new Dictionary<uint, Item>();
            this._equipment = new uint[16];
            this._backpack = new uint[6, 10];
            this._goldItem = null;
        }

        // This should be in the database#
        // Do all items need a rectangual space in diablo 3?
        private InventorySize GetItemInventorySize(Item item)
        {
            if (Item.IsWeapon(item.ItemType))
            {
                return new InventorySize() { Width = 1, Height = 2 };
            }
            else if (Item.IsPotion(item.ItemType))
            {
                return new InventorySize() { Width = 1, Height = 1 };
            }
            else if (Item.IsRing(item.ItemType))
            {
                return new InventorySize() { Width = 1, Height = 1 };
            }
            else if (Item.IsBelt(item.ItemType))
            {
                return new InventorySize() { Width = 1, Height = 1 };
            }

            return new InventorySize() { Width = 1, Height = 2 };
        }

        private InventorySize GetItemInventorySize(uint itemID)
        {
            Item item = _owner.World.GetItem(itemID);
            return GetItemInventorySize(item);
        }

        private bool FreeSpace(Item item, int row, int column)
        {
            bool result = true;
            InventorySize size = GetItemInventorySize(item);

            for (int r = row; r < Math.Min(row + size.Height, Rows); r++)
                for (int c = column; c < Math.Min(column + size.Width, Columns); c++)
                    if ((_backpack[r, c] != 0) && (_backpack[r, c] != item.DynamicID))
                        result = false;
            return result;
        }

        private bool FreeSpace(uint itemID, int row, int column)
        {
            return FreeSpace(_owner.World.GetItem(itemID), row, column);
        }

        /// <summary>
        /// Collects (counts) the items overlapping with the item about to be dropped.
        /// If there are none, drop item
        /// If there is exacly one, swap it with item (TODO)
        /// If there are more, item cannot be dropped
        /// </summary>
        private int CollectOverlappingItems(uint droppedItemID, int row, int column)
        {
            InventorySize dropSize = GetItemInventorySize(droppedItemID);
            var overlapping = new List<uint>();

            // For every slot...
            for (int r = row; r < _backpack.GetLength(0) && r < row + dropSize.Height; r++)
                for (int c = column; c < _backpack.GetLength(1) && c < column + dropSize.Width; c++)

                    // that contains an item other than the one we want to drop
                    if (_backpack[r, c] != 0 && _backpack[r, c] != droppedItemID) //TODO this would break for an item with id 0

                        // add it to the list if if dropping the item in <row, column> would need the same slot
                        //if (r >= row && r <= row + dropSize.Height)
                        //    if (c >= column && c <= column + dropSize.Width)
                                if (!overlapping.Contains(_backpack[r, c]))
                                    overlapping.Add(_backpack[r, c]);

            return overlapping.Count;
        }

        /// <summary>
        /// Removes an item from the backpack
        /// </summary>
        private void RemoveItem(Item item)
        {
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Columns; c++)
                {
                    if (_backpack[r, c] == item.DynamicID)
                    {
                        _backpack[r, c] = 0;
                        item.SetInventoryLocation(-1, -1, -1);
                        item.Owner = null;
                    }
                }
            }
        }

        private void RemoveItem(uint itemID)
        {
            RemoveItem(_owner.World.GetItem(itemID));
        }

        /// <summary>
        /// Adds an item to the backpack
        /// </summary>
        void AddItem(Item item, int row, int column)
        {
            InventorySize size = GetItemInventorySize(item);

            //check backpack boundaries
            if (row + size.Width > Rows || column + size.Width > Columns) return;

            for (int r = row; r < Math.Min(row + size.Height, Rows); r++)
                for (int c = column; c < Math.Min(column + size.Width, Columns); c++)
                {
                    System.Diagnostics.Debug.Assert(_backpack[r, c] == 0, "You need to remove an item from the backpack before placing another item there");
                    _backpack[r, c] = item.DynamicID;
                    item.Owner = _owner;
                    item.SetInventoryLocation(0, c, r);
                }
        }

        void AddItem(uint itemID, int row, int column)
        {
            AddItem(_owner.World.GetItem(itemID), row, column);
        }

        /// <summary>
        /// Refreshes the visual appearance of the hero
        /// TODO: this should go to hero class
        /// </summary>
        /// <param name="actorID"></param>
        void RefreshVisual(uint actorID)
        {
            _owner.InGameClient.SendMessage(new VisualInventoryMessage()
            {
                ActorID = actorID,
                EquipmentList = new VisualEquipment()
                {
                    Equipment = new VisualItem[8]
                    {
                        GetEquipmentItem(0),
                        GetEquipmentItem(1),
                        GetEquipmentItem(2),
                        GetEquipmentItem(3),
                        GetEquipmentItem(4),
                        GetEquipmentItem(5),
                        GetEquipmentItem(6),
                        GetEquipmentItem(7),
                    },
                },
            });

            // Finalize
            // Hardcoded tick keeps the game updated.. /komiga
            _owner.InGameClient.PacketId += 10 * 2;
            _owner.InGameClient.SendMessage(new DWordDataMessage()
            {
                Id = 0x89,
                Field0 = _owner.InGameClient.PacketId,
            });
            _owner.InGameClient.FlushOutgoingBuffer();
        }

        public VisualItem GetEquipmentItem(int equipSlot)
        {
            if (_equipment[equipSlot] == 0)
            {
                return new VisualItem()
                {
                    GbId = 0,
                    Field1 = 0,
                    Field2 = 0,
                    Field3 = 0,
                };
            }
            else
            {
                return _owner.World.GetItem(_equipment[equipSlot]).CreateVisualItem();
            }
        }

        /// <summary>
        /// Equips an item in an equipment slote
        /// </summary>
        void EquipItem(Item item, int slot)
        {
            _equipment[slot] = item.DynamicID;
            item.Owner = _owner;
            item.SetInventoryLocation(slot, 0, 0);
        }

        void EquipItem(uint itemID, int slot)
        {
            EquipItem(_owner.World.GetItem(itemID), slot);
        }

        /// <summary>
        /// Removes an item from the equipment slot it uses
        /// </summary>
        void UnequipItem(Item item)
        {
            for (int i = 0; i < EquipmentSlots; i++)
            {
                if (_equipment[i] == item.DynamicID)
                {
                    _equipment[i] = 0;
                    item.SetInventoryLocation(-1, -1, -1);
                    item.Owner = null;
                }
            }
        }

        void UnequipItem(uint itemID)
        {
            UnequipItem(_owner.World.GetItem(itemID));
        }

        /// <summary>
        /// Returns whether an item is equipped
        /// </summary>
        public bool IsItemEquipped(uint itemID)
        {
            for (int i = 0; i < EquipmentSlots; i++)
                if (_equipment[i] == itemID)
                    return true;
            return false;
        }

        public bool IsItemEquipped(Item item)
        {
            return IsItemEquipped(item.DynamicID);
        }

        /// <summary>
        /// Checks whether the inventory contains an item
        /// </summary>
        public bool Contains(uint itemID)
        {
            for (int r = 0; r < Rows; r++)
                for (int c = 0; c < Columns; c++)
                    if (_backpack[r, c] == itemID)
                        return true;
            return false;
        }

        public bool Contains(Item item)
        {
            return Contains(item.DynamicID);
        }

        /// <summary>
        /// Find an inventory slot with enough space for an item
        /// </summary>
        /// <returns>Slot or null if there is no space in the backpack</returns>
        private InventorySlot? FindSlotForItem(Item item)
        {
            InventorySize size = GetItemInventorySize(item);
            for (int r = 0; r <= Rows - size.Height; r++)
                for (int c = 0; c <= Columns - size.Width; c++)
                    if (CollectOverlappingItems(item.DynamicID, r, c) == 0)
                        return new InventorySlot() { Row = r, Column = c };
            return null;
        }

        private void AcceptMoveRequest(Item item)
        {
            /*_owner.InGameClient.SendMessage(new ACDInventoryPositionMessage()
            {
                ItemID = item.DynamicID,
                InventoryLocation = item.InventoryLocationMessage,
                Field2 = 1 // what does this do?  // 0 - source item not disappearing from inventory, 1 - Moving, any other possibilities? its an int32
            });*/

            _owner.InGameClient.PacketId += 10 * 2;
            _owner.InGameClient.SendMessage(new DWordDataMessage()
            {
                Id = 0x89,
                Field0 = _owner.InGameClient.PacketId,
            });
            _owner.InGameClient.FlushOutgoingBuffer();
        }

        /// <summary>
        /// Picks an item up after client request
        /// </summary>
        /// <returns>true if the item was picked up, or false if the player could not pick up the item.</returns>
        public bool PickUp(Item item)
        {
            System.Diagnostics.Debug.Assert(!Contains(item) && !IsItemEquipped(item), "Item already in inventory");
            // TODO: Autoequip when equipment slot is empty

            bool success = false;
            InventorySlot? freeSlot = FindSlotForItem(item);
            if (freeSlot == null)
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
                AddItem(item, freeSlot.Value.Row, freeSlot.Value.Column);

                if (_owner.GroundItems.ContainsKey(item.DynamicID))
                    _owner.GroundItems.Remove(item.DynamicID);
                success = true;
            }

            // Finalize
            // Hardcoded tick keeps the game updated.. /komiga
            _owner.InGameClient.PacketId += 10 * 2;
            _owner.InGameClient.SendMessage(new DWordDataMessage()
            {
                Id = 0x89,
                Field0 = _owner.InGameClient.PacketId,
            });
            _owner.InGameClient.FlushOutgoingBuffer();
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
                System.Diagnostics.Debug.Assert(Contains(request.ItemID) || IsItemEquipped(request.ItemID), "Request to equip unknown item");

                // TODO find out swapping items, so no equipping when the slot is occupied
                if (request.Location.EquipmentSlot < EquipmentSlots && this._equipment[request.Location.EquipmentSlot] == 0)
                {
                    Logger.Debug("Equip Item {0}", request.AsText());
                    RemoveItem(item);
                    EquipItem(item, request.Location.EquipmentSlot);
                    AcceptMoveRequest(item);
                    RefreshVisual(_owner.DynamicID);
                }
            }

            // Request to move an item (from backpack or equipmentslot)
            else
            {
                if (FreeSpace(item, request.Location.Row, request.Location.Column))
                {
                    if (IsItemEquipped(item))
                    {
                        Logger.Debug("Unequip item {0}", request.AsText());
                        UnequipItem(item); // Unequip the item
                        RefreshVisual(_owner.DynamicID); // Refresh the visual equipment for the player
                    }
                    else
                    {
                        RemoveItem(item);
                    }
                    AddItem(item, request.Location.Row, request.Location.Column);
                    AcceptMoveRequest(item);
                }
            }
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
            Item itemFrom = this.Items[msg.FromID];
            Item itemTo = this.Items[msg.ToID];

            itemFrom.Count = (itemFrom.Count) - ((int)msg.Amount);
            itemTo.Count = itemTo.Count + (int)msg.Amount;

            // TODO: This needs to change the attribute on the item itself. /komiga
            // Update source
            GameAttributeMap attributes = new GameAttributeMap();
            attributes[GameAttribute.ItemStackQuantityLo] = itemFrom.Count;
            attributes.SendMessage(_owner.InGameClient, itemFrom.DynamicID);

            // TODO: This needs to change the attribute on the item itself. /komiga
            // Update target
            attributes = new GameAttributeMap();
            attributes[GameAttribute.ItemStackQuantityLo] = itemTo.Count;
            attributes.SendMessage(_owner.InGameClient, itemTo.DynamicID);

            _owner.InGameClient.PacketId += 10 * 2;
            _owner.InGameClient.SendMessage(new DWordDataMessage()
            {
                Id = 0x89,
                Field0 = _owner.InGameClient.PacketId,
            });
        }

        private void OnInventoryDropItemMessage(InventoryDropItemMessage msg)
        {
            Item item = _owner.World.GetItem(msg.ItemID);
            if (IsItemEquipped(item))
            {
                UnequipItem(item);
                RefreshVisual(_owner.DynamicID);
            }
            else
            {
                RemoveItem(item);
            }
            AcceptMoveRequest(item);
            item.Drop(null, _owner.Position);
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
            if (_goldItem == null)
            {
                ItemTypeGenerator itemGenerator = new ItemTypeGenerator(_owner.InGameClient);
                _goldItem = itemGenerator.CreateItem("Gold1", 0x00000178, ItemType.Gold);
                _goldItem.Count = collectedItem.Count;
                _goldItem.Owner = _owner;
                _goldItem.SetInventoryLocation(18, 0, 0); // Equipment slot 18 ==> Gold
                _goldItem.Reveal(_owner);
            }
            else
            {
                _goldItem.Count += collectedItem.Count;
            }

            GameAttributeMap attributes = new GameAttributeMap();
            attributes[GameAttribute.ItemStackQuantityLo] = _goldItem.Count;
            attributes.SendMessage(_owner.InGameClient, _goldItem.DynamicID);
        }
    }
}
