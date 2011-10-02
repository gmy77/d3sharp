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
using Mooege.Net.GS.Message;
using Mooege.Net.GS.Message.Definitions.Inventory;
using Mooege.Net.GS.Message.Fields;
using Mooege.Net.GS.Message.Definitions.Misc;
using Mooege.Net.GS.Message.Definitions.Combat;
using Mooege.Net.GS;
using Mooege.Net.GS.Message.Definitions.Attribute;
using Mooege.Net.GS.Message.Definitions.ACD;
using Mooege.Core.Common.Items;

namespace Mooege.Core.GS.Universe
{
    // Items are stored for this moment in GameClient,
    // this shold be esier way to generate specific or random item by any player...
    // Putting all game items outside and place in some class in future schuld make esier way to load and save to database

    // Backpack is organized by adding an item to EVERY slot it fills
    public class Inventory:IMessageConsumer
    {
        static readonly Logger Logger = LogManager.CreateLogger();

        public int Rows { get { return _backpack.GetLength(0); } }
        public int Columns { get { return _backpack.GetLength(1); } }
        public int EquipmentSlots { get { return _equipment.GetLength(0); } }

        private int[] _equipment;      // array of equiped items_id  (not item)
        private int[,] _backpack;      // backpack array

        private readonly Hero _owner; // Used, because most information is not in the item class but Actors managed by the world

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

        public Inventory(Hero owner)
        {
            this._owner = owner;
            this._backpack = new int[6, 10];
            this._equipment = new int[8];
        }

        // This should be in the database#
        // Do all items need a rectangual space in diablo 3?
        private InventorySize GetItemInventorySize(int itemID)
        {
            if (Item.IsWeapon(_owner.InGameClient.items[itemID].Type))
            {
                return new InventorySize() { Width = 1, Height = 2 };
            }
            else if (Item.IsPotion(_owner.InGameClient.items[itemID].Type))
            {
                return new InventorySize() { Width = 1, Height = 1 };
            }

            return new InventorySize() { Width = 1, Height = 2 };
        }

        private bool FreeSpace(int droppedItemID, int row, int column)
        {
            InventorySize size = GetItemInventorySize(droppedItemID);

            for (int r = row; r < Math.Min(row + size.Height, Rows); r++)
                for (int c = column; c < Math.Min(column + size.Width, Columns); c++)
                    if (_backpack[r, c] != 0)
                        return false;
            return true;
        }

        /// <summary>
        /// Collects (counts) the items overlapping with the item about to be dropped.
        /// If there are none, drop item
        /// If there is exacly one, swap it with item (TODO)
        /// If there are more, item cannot be dropped
        /// </summary>
        private int CollectOverlappingItems(int droppedItemID, int row, int column)
        {
            InventorySize dropSize = GetItemInventorySize(droppedItemID);
            var overlapping = new List<int>();

            // For every slot...
            for (int r = row; r < Rows; r++)
                for (int c = 0; c < Columns; c++)

                    // that contains an item other than the one we want to drop
                    if (_backpack[r, c] != 0 && _backpack[r, c] != droppedItemID) //TODO this would break for an item with id 0

                        // add it to the list if if dropping the item in <row, column> would need the same slot
                        if (r >= row && r <= row + dropSize.Height)
                            if (c >= column && c <= column + dropSize.Width)
                                if (!overlapping.Contains(_backpack[r, c]))
                                    overlapping.Add(_backpack[r, c]);

            return overlapping.Count;
        }

        /// <summary>
        /// Removes and item from the backpack
        /// </summary>
        private void RemoveItem(int itemID)
        {
            for (int r = 0; r < Rows; r++)
                for (int c = 0; c < Columns; c++)
                    if (_backpack[r, c] == itemID)
                        _backpack[r, c] = 0;
        }

        /// <summary>
        /// Adds an item to the backpack
        /// </summary>
        void AddItem(int itemID, int row, int column)
        {
            InventorySize size = GetItemInventorySize(itemID);

            //check backback boundaries
            if (row + size.Width > Rows || column + size.Width > Columns) return;

            for (int r = row; r < Math.Min(row + size.Height, Rows); r++)
                for (int c = column; c < Math.Min(column + size.Width, Columns); c++)
                {
                    System.Diagnostics.Debug.Assert(_backpack[r, c] == 0, "You need to remove an item from the backpack before placing another item there");
                    _backpack[r, c] = itemID;
                }
        }

        /// <summary>
        /// Refreshes the visual appearance of the hero
        /// TODO: this should go to hero class
        /// </summary>
        /// <param name="playerID"></param>
        void RefreshVisual(int playerID)
        {
            _owner.InGameClient.SendMessage(new VisualInventoryMessage()
            {
                Id = (int)Opcodes.VisualInventoryMessage,
                Field0 = playerID,
                EquipmentList = new VisualEquipment()
                {
                    Equipments = new VisualItem[8]
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
            // TODO find out if that is necessary
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
                return _owner.InGameClient.items[_equipment[equipSlot]].CreateVisualItem();
            }
        }

        /// <summary>
        /// Equips an item in an equipment slote
        /// </summary>
        void EquipItem(int itemID, int slot)
        {
            _equipment[slot] = itemID;
        }

        /// <summary>
        /// Removes an item from the equipment slot it uses
        /// </summary>
        void UnequipItem(int itemID)
        {
            for (int i = 0; i < EquipmentSlots; i++)
                if (_equipment[i] == itemID)
                    _equipment[i] = 0;
        }

        void AcceptMoveRequest(int itemId, InvLoc inventoryLocation)
        {
            var inventoryLocationMessage = new InventoryLocationMessageData()
                {
                    Field0 = inventoryLocation.Field0, // Inventory Owner
                    Field1 = inventoryLocation.Field1, // EquipmentSlot
                    Field2 = new IVector2D()
                    {
                        Field0 = inventoryLocation.Field2, // Row
                        Field1 = inventoryLocation.Field3, // Column
                    },
                };

            _owner.InGameClient.SendMessage(new ACDInventoryPositionMessage()
            {
                Id = (int)Opcodes.ACDInventoryPositionMessage,
                Field0 = itemId,
                Field1 = inventoryLocationMessage,
                Field2 = 1 // what does this do?  // 0- source item not disappearing from inventory, 1 - Moving, any other possibilities? its an int32
            });

            _owner.InGameClient.PacketId += 10 * 2;
            _owner.InGameClient.SendMessage(new DWordDataMessage()
            {
                Id = 0x89,
                Field0 = _owner.InGameClient.PacketId,
            });

            _owner.InGameClient.FlushOutgoingBuffer();
        }

        /// <summary>
        /// Returns whether an item is equipped
        /// </summary>
        Boolean IsItemEquipped(int itemID)
        {
            for (int i = 0; i < EquipmentSlots; i++)
                if (_equipment[i] == itemID)
                    return true;
            return false;
        }

        /// <summary>
        /// Checks whether the inventory contains an item
        /// </summary>
        public bool Contains(int itemId)
        {
            for (int r = 0; r < Rows; r++)
                for (int c = 0; c < Columns; c++)
                    if (_backpack[r, c] == itemId)
                        return true;
            return false;
        }

        /// <summary>
        /// Find an inventory slot with enough space for an item
        /// </summary>
        /// <returns>Slot or null if there is no space in the backpack</returns>
        private InventorySlot? FindSlotForItem(int itemID)
        {
            InventorySize size = GetItemInventorySize(itemID);

            for (int r = 0; r < Rows - size.Width + 1; r++)
                for (int c = 0; c < Columns - size.Height + 1; c++)
                    if (CollectOverlappingItems(itemID, r, c) == 0)
                        return new InventorySlot() { Row = r, Column = c };
            return null;
        }

        /// <summary>
        /// Picks an item up after client request
        /// </summary>
        public void PickUp(TargetMessage msg)
        {
            System.Diagnostics.Debug.Assert(!Contains(msg.Field1) && !IsItemEquipped(msg.Field1), "Item already in inventory");
            // TODO Ensure target is an item and it exists
            // TODO Autoequip when equipment slot is empty

            InventorySlot? freeSlot = FindSlotForItem(msg.Field1);
            if (freeSlot == null)
            {
                //Inventory full
                _owner.InGameClient.SendMessage(new ACDPickupFailedMessage()
                {
                    Id = (int)Opcodes.ACDPickupFailedMessage,
                    ItemId = msg.Field1,
                    Reason = ACDPickupFailedMessage.Reasons.InventoryFull
                });
            }
            else
            {
                AddItem(msg.Field1, freeSlot.Value.Row, freeSlot.Value.Column);

                _owner.InGameClient.SendMessage(new ACDInventoryPositionMessage()
                {
                    Id = (int)Opcodes.ACDInventoryPositionMessage,
                    Field0 = msg.Field1,    // ItemID
                    Field1 = new InventoryLocationMessageData()
                    {
                        Field0 = _owner.DynamicId, // Inventory Owner
                        Field1 = 0x00000000, // EquipmentSlot
                        Field2 = new IVector2D()
                        {
                            Field0 = freeSlot.Value.Column,
                            Field1 = freeSlot.Value.Row
                        },
                    },
                    Field2 = 1  // TODO, find out what this is and why it must be 1...is it an enum?
                });
            }

            // Finalize
            // TODO find out if that is necessary
            _owner.InGameClient.PacketId += 10 * 2;
            _owner.InGameClient.SendMessage(new DWordDataMessage()
            {
                Id = 0x89,
                Field0 = _owner.InGameClient.PacketId,
            });

            _owner.InGameClient.FlushOutgoingBuffer();
        }

        /// <summary>
        /// Handles a request to move an item within the inventory.
        /// This covers moving items within the backpack, from equipment
        /// slot to backpack and from backpack to equipment slot
        /// </summary>
        public void HandleInventoryRequestMoveMessage(InventoryRequestMoveMessage request)
        {
            // Request to equip item from backpack
            if (request.Field1.Field1 != 0)
            {
                System.Diagnostics.Debug.Assert(Contains(request.Field0) || IsItemEquipped(request.Field0), "Request to equip unknown item");

                // TODO find out swapping items, so no equipping when the slot is occupied
                if (request.Field1.Field1 < EquipmentSlots && this._equipment[request.Field1.Field1] == 0)
                {
                    Logger.Debug("Equip Item {0}", request.AsText());
                    RemoveItem(request.Field0);
                    EquipItem(request.Field0, request.Field1.Field1);

                    AcceptMoveRequest(request.Field0, request.Field1);
                    RefreshVisual(request.Field1.Field0);
                }
            }

            // Request to move an item (from backpack or equipmentslot)
            else
            {
                if (FreeSpace(request.Field0, request.Field1.Field3, request.Field1.Field2))
                {
                    if (IsItemEquipped(request.Field0))
                    {
                        Logger.Debug("Unequip item {0}", request.AsText());
                        UnequipItem(request.Field0);
                        RefreshVisual(request.Field1.Field0);
                    }
                    else
                    {
                        RemoveItem(request.Field0);
                    }
                    AddItem(request.Field0, request.Field1.Field3, request.Field1.Field2);
                    AcceptMoveRequest(request.Field0, request.Field1);
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
            _owner.InGameClient.items[msg.Field0].Count = (_owner.InGameClient.items[msg.Field0].Count) - ((int)msg.Field2);
            _owner.InGameClient.items[msg.Field1].Count = _owner.InGameClient.items[msg.Field1].Count + (int)msg.Field2;

            // Update source
            GameAttributeMap attributes = new GameAttributeMap();
            attributes[GameAttribute.ItemStackQuantityLo] = _owner.InGameClient.items[msg.Field0].Count;
            attributes.SendMessage(_owner.InGameClient, msg.Field0);

            // Update target
            attributes = new GameAttributeMap();
            attributes[GameAttribute.ItemStackQuantityLo] = _owner.InGameClient.items[msg.Field1].Count;
            attributes.SendMessage(_owner.InGameClient, msg.Field1);

            _owner.InGameClient.PacketId += 10 * 2;
            _owner.InGameClient.SendMessage(new DWordDataMessage()
            {
                Id = 0x89,
                Field0 = _owner.InGameClient.PacketId,
            });
        }

        private void OnInventoryDropItemMessage(InventoryDropItemMessage msg)
        {
            if (IsItemEquipped(msg.ItemId))
            {
                UnequipItem(msg.ItemId);
                RefreshVisual(_owner.DynamicId);
            }
            else
            {
                RemoveItem(msg.ItemId);
            }

            AcceptMoveRequest(msg.ItemId, new InvLoc { Field0 = _owner.DynamicId, Field1 = -1, Field2 = -1, Field3 = -1 });
            _owner.Universe.DropItem(_owner, _owner.InGameClient.items[msg.ItemId], _owner.Position);
        }

        public void Consume(GameClient client, GameMessage message)
        {
            if (message is InventoryRequestMoveMessage) HandleInventoryRequestMoveMessage(message as InventoryRequestMoveMessage);
            else if (message is InventorySplitStackMessage) OnInventorySplitStackMessage(message as InventorySplitStackMessage);
            else if (message is InventoryStackTransferMessage) OnInventoryStackTransferMessage(message as InventoryStackTransferMessage);
            else if (message is InventoryDropItemMessage) OnInventoryDropItemMessage(message as InventoryDropItemMessage);
            else return;
        }
    }
}
