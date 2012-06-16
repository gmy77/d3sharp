/*
 * Copyright (C) 2011 - 2012 mooege project - http://www.mooege.org
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
using Mooege.Common.Logging;
using Mooege.Core.GS.Actors;
using Mooege.Core.GS.Items;
using Mooege.Core.GS.Objects;
using Mooege.Core.GS.Players;

namespace Mooege.Core.GS.Common
{

    /// <summary>
    /// This class handels the gridlayout of an stash. Possible usecases are the inventory backpack, shared stash, traders stash,...
    /// Stash is organized by adding an item to EVERY slot it fills
    /// </summary>
    public class InventoryGrid : IRevealable
    {
        static readonly Logger Logger = LogManager.CreateLogger();

        public int EquipmentSlot { get; private set; }
        public int Rows { get { return _backpack.GetLength(0); } }
        public int Columns { get { return _backpack.GetLength(1); } }
        public Dictionary<uint, Item> Items { get; private set; }
        private uint[,] _backpack;

        private readonly Actor _owner; // Used, because most information is not in the item class but Actors managed by the world

        private struct InventorySize
        {
            public int Width;
            public int Height;
        }

        private struct InventorySlot
        {
            public int Row;
            public int Column;
        }

        public InventoryGrid(Actor owner, int rows, int columns, int slot = 0)
        {
            this._backpack = new uint[rows, columns];
            this._owner = owner;
            this.Items = new Dictionary<uint, Item>();
            this.EquipmentSlot = slot;
        }

        public void ResizeGrid(int rows, int columns)
        {
            var newBackpack = new uint[rows, columns];
            Array.Copy(_backpack, newBackpack, _backpack.Length);
            _backpack = newBackpack;
        }

        public void Clear()
        {
            Items.Clear();
            int r = Rows;
            int c = Columns;
            this._backpack = new uint[r, c];
        }

        // This should be in the database#
        // Do all items need a rectangual space in diablo 3?
        private InventorySize GetItemInventorySize(Item item)
        {
            if (EquipmentSlot == (int)EquipmentSlotId.Vendor)
                return new InventorySize() { Width = 1, Height = 1 };
            // TODO: identify a belt as 1x1, not as generic armour 1x2
            if (Item.IsWeapon(item.ItemType) || Item.IsArmor(item.ItemType) || Item.IsOffhand(item.ItemType))
            {
                return new InventorySize() { Width = 1, Height = 2 };
            }
            return new InventorySize() { Width = 1, Height = 1 };

        }

        public bool FreeSpace(Item item, int row, int column)
        {
            bool result = true;
            InventorySize size = GetItemInventorySize(item);

            for (int r = row; r < Math.Min(row + size.Height, Rows); r++)
                for (int c = column; c < Math.Min(column + size.Width, Columns); c++)
                    if ((_backpack[r, c] != 0) && (_backpack[r, c] != item.DynamicID))
                        result = false;
            return result;
        }

        /// <summary>
        /// Collects (counts) the items overlapping with the item about to be dropped.
        /// If there are none, drop item
        /// If there is exacly one, swap it with item (TODO)
        /// If there are more, item cannot be dropped
        /// </summary>
        private int CollectOverlappingItems(Item item, int row, int column)
        {
            InventorySize dropSize = GetItemInventorySize(item);
            var overlapping = new List<uint>();

            // For every slot...
            for (int r = row; r < _backpack.GetLength(0) && r < row + dropSize.Height; r++)
                for (int c = column; c < _backpack.GetLength(1) && c < column + dropSize.Width; c++)

                    // that contains an item other than the one we want to drop
                    if (_backpack[r, c] != 0 && _backpack[r, c] != item.DynamicID) //TODO this would break for an item with id 0

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
        public void RemoveItem(Item item)
        {
            if (!Items.ContainsKey(item.DynamicID))
                return;

            Items.Remove(item.DynamicID);

            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Columns; c++)
                {
                    if (_backpack[r, c] == item.DynamicID)
                    {
                        _backpack[r, c] = 0;
                    }
                }
            }
        }

        /// <summary>
        /// Adds an item to the backpack
        /// </summary>
        public void AddItem(Item item, int row, int column)
        {
            InventorySize size = GetItemInventorySize(item);

            //check backpack boundaries
            if (row + size.Width > Rows || column + size.Width > Columns) return;

            Items.Add(item.DynamicID, item);

            for (int r = row; r < Math.Min(row + size.Height, Rows); r++)
                for (int c = column; c < Math.Min(column + size.Width, Columns); c++)
                {
                    System.Diagnostics.Debug.Assert(_backpack[r, c] == 0, "You need to remove an item from the backpack before placing another item there");
                    _backpack[r, c] = item.DynamicID;
                }

            item.Owner = _owner;
            item.SetInventoryLocation(EquipmentSlot, column, row);
        }

        /// <summary>
        /// Adds an Item at a free spot to the backpack
        /// </summary>
        /// <param name="item"></param>
        public bool AddItem(Item item)
        {
            return AddItem(-1, -1, item);
        }
        /// <summary>
        /// Adds an Item at a free spot to the backpack
        /// </summary>
        /// <param name="minRow"></param>
        /// <param name="maxRow"></param>
        /// <param name="item"></param>
        public bool AddItem(int minRow, int maxRow, Item item)
        {
            InventorySlot? slot = FindSlotForItem(minRow, maxRow, item);
            if (slot.HasValue)
            {
                AddItem(item, slot.Value.Row, slot.Value.Column);
                return true;
            }
            else
            {
                Logger.Error("Can't find slot in backpack to add item {0}", item.ActorSNO);
                return false;
            }
        }

        public Boolean HasFreeSpace(Item item)
        {
            return (FindSlotForItem(-1, -1, item) != null);
        }

        public Boolean HasFreeSpace(int minRow, int maxRow, Item item)
        {
            return (FindSlotForItem(minRow, maxRow, item) != null);
        }

        /// <summary>
        /// Checks whether the inventory contains an item
        /// </summary>
        public bool Contains(uint itemID)
        {
            return Items.ContainsKey(itemID);
        }

        public bool Contains(Item item)
        {
            return Contains(item.DynamicID);
        }

        /// <summary>
        /// Find an inventory slot with enough space for an item
        /// </summary>
        /// <returns>Slot or null if there is no space in the backpack</returns>
        private InventorySlot? FindSlotForItem(int minRow, int maxRow, Item item)
        {
            InventorySize size = GetItemInventorySize(item);
            // If we target a specific tab in stash, we need to specify min and max row to fill
            int nStartRow = minRow == -1 ? 0 : Math.Min(minRow, Rows); // maybe not needed, because Rows always > minRow
            int nEndRow = minRow == -1 ? Rows : Math.Min(maxRow, Rows);
            for (int r = nStartRow; r <= nEndRow - size.Height; r++)
                for (int c = 0; c <= Columns - size.Width; c++)
                    if (CollectOverlappingItems(item, r, c) == 0)
                        return new InventorySlot() { Row = r, Column = c };
            return null;
        }

        public bool Reveal(Player player)
        {
            if (_owner == null || _owner.World == null)
                return false;

            foreach (var item in Items.Values)
                item.Reveal(player);

            return true;
        }

        public bool Unreveal(Player player)
        {
            if (_owner == null || _owner.World == null)
                return false;

            foreach (var item in Items.Values)
                item.Unreveal(player);

            return true;
        }

        public Item GetItem(uint itemId)
        {
            Item item;
            if (!Items.TryGetValue(itemId, out item))
                return null;
            return item;
        }
    }
}
