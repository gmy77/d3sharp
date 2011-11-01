using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mooege.Common;
using Mooege.Core.Common.Items;
using Mooege.Core.GS.Actors;
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
            this.EquipmentSlot = slot;
        }

        public void ResizeGrid(int rows, int columns)
        {
            var newBackpack = new uint[rows, columns];
            Array.Copy(_backpack, newBackpack, _backpack.Length);
            _backpack = newBackpack;
        }

        // This should be in the database#
        // Do all items need a rectangual space in diablo 3?
        private InventorySize GetItemInventorySize(Item item)
        {
            if(EquipmentSlot == (int) EquipmentSlotId.Vendor)
                return new InventorySize() { Width = 1, Height = 1 };
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

        /// <summary>
        /// Adds an item to the backpack
        /// </summary>
        public void AddItem(Item item, int row, int column)
        {
            InventorySize size = GetItemInventorySize(item);

            //check backpack boundaries
            if (row + size.Width > Rows || column + size.Width > Columns) return;

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
            InventorySlot? slot = FindSlotForItem(item);
            if (slot.HasValue)
            {
                AddItem(item, slot.Value.Row, slot.Value.Column);
                return true;
            }
            else
            {
                Logger.Error("Can't find slot in backpack to add item {0}", item.SNOName);
                return false;
            }
        }

        public Boolean HasFreeSpace(Item item)
        {
            return (FindSlotForItem(item) != null);
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
                    if (CollectOverlappingItems(item, r, c) == 0)
                        return new InventorySlot() { Row = r, Column = c };
            return null;
        }

        public bool Reveal(Player player)
        {
            if (_owner == null || _owner.World == null)
                return false;

            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Columns; c++)
                {
                    if (_backpack[r, c] != 0)
                    {
                        var item = _owner.World.GetItem(_backpack[r, c]);
                        if (item != null)
                            item.Reveal(player);
                    }
                }
            }

            return true;
        }

        public bool Unreveal(Player player)
        {
            if (_owner == null || _owner.World == null)
                return false;

            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Columns; c++)
                {
                    if (_backpack[r, c] != 0)
                    {
                        //_owner.World.Actors[_backpack[r, c]].Unreveal(player); // TODO: Fixme /raist
                    }
                }
            }

            return true;
        }
    }
}
