using Mooege.Core.Common.Items;
using Mooege.Net.GS.Message.Fields;
using Mooege.Net.GS.Message;
using System.Collections.Generic;
using Mooege.Core.GS.Objects;

namespace Mooege.Core.GS.Players
{

    // these ids are transmitted by the client when equipping an item         
    public enum EquipmentSlotId
    {
        Helm = 1, Chest = 2, Off_Hand = 3, Main_Hand = 4, Hands = 5, Belt = 6, Feet = 7,
        Shoulders = 8, Legs = 9, Bracers = 10, Ring_right = 11, Ring_left = 12, Amulett = 13,
        Skills = 16, Stash = 17, Gold = 18, Vendor = 20 // To do: Should this be here? Its not really an eq. slot /fasbat
    }

    class Equipment : IRevealable
    {
        public int EquipmentSlots { get { return _equipment.GetLength(0); } }
        public Dictionary<uint, Item> Items { get; private set; }
        private readonly Player _owner; // Used, because most information is not in the item class but Actors managed by the world
        private Item _inventoryGold;

        private uint[] _equipment;      // array of equiped items_id  (not item)

        public Equipment(Player owner){
            this._equipment = new uint[17];
            this._owner = owner;
            this.Items = new Dictionary<uint, Item>();
            this._inventoryGold = ItemGenerator.CreateGold(_owner, 0);
            this._inventoryGold.Attributes[GameAttribute.ItemStackQuantityLo] = 0;
            this._inventoryGold.SetInventoryLocation(18, 0, 0);
            this._inventoryGold.Owner = _owner;
            this.Items.Add(_inventoryGold.DynamicID,_inventoryGold);
        }
       
        /// <summary>
        /// Equips an item in an equipment slot
        /// </summary>
        public void EquipItem(Item item, int slot)
        {
            _equipment[slot] = item.DynamicID;
            if (!Items.ContainsKey(item.DynamicID))
                Items.Add(item.DynamicID, item);
            item.Owner = _owner;
            item.Attributes[GameAttribute.Item_Equipped] = true; // Probaly should be handled by Equipable class /fasbat
            item.Attributes.SendChangedMessage(_owner.InGameClient, item.DynamicID);
            item.SetInventoryLocation(slot, 0, 0);            
        }

        public void EquipItem(uint itemID, int slot)
        {
            EquipItem(_owner.Inventory.GetItem(itemID), slot);
        }

        /// <summary>
        /// Removes an item from the equipment slot it uses
        /// returns the used equipmentSlot
        /// </summary>
        public int UnequipItem(Item item)
        {
            if (!Items.ContainsKey(item.DynamicID))
                return 0;
            Items.Remove(item.DynamicID);

            var slot = item.EquipmentSlot;
            if (_equipment[slot] == item.DynamicID)
            {
                _equipment[slot] = 0;
                item.Attributes[GameAttribute.Item_Equipped] = false; // Probaly should be handled by Equipable class /fasbat
                item.Attributes.SendChangedMessage(_owner.InGameClient, item.DynamicID);
                return slot;
            }

            return 0;
        }     

        /// <summary>
        /// Returns whether an item is equipped
        /// </summary>
        public bool IsItemEquipped(uint itemID)
        {
            return Items.ContainsKey(itemID);
        }

        public bool IsItemEquipped(Item item)
        {
            return IsItemEquipped(item.DynamicID);
        }

        private VisualItem GetEquipmentItem(EquipmentSlotId equipSlot)
        {
            if (_equipment[(int)equipSlot] == 0)
            {
                return new VisualItem()
                {
                    GbId = -1, // 0 causes error logs on the client  - angerwin
                    Field1 = 0,
                    Field2 = 0,
                    Field3 = 0,
                };
            }
            else
            {
                return Items[(_equipment[(int)equipSlot])].CreateVisualItem();
            }
        }

        public VisualItem[] GetVisualEquipment(){
            return new VisualItem[8]
                    {
                        GetEquipmentItem(EquipmentSlotId.Helm),
                        GetEquipmentItem(EquipmentSlotId.Chest),
                        GetEquipmentItem(EquipmentSlotId.Feet),
                        GetEquipmentItem(EquipmentSlotId.Hands),
                        GetEquipmentItem(EquipmentSlotId.Main_Hand),
                        GetEquipmentItem(EquipmentSlotId.Off_Hand),
                        GetEquipmentItem(EquipmentSlotId.Shoulders),
                        GetEquipmentItem(EquipmentSlotId.Legs),
                    };
        }

        public Item AddGoldItem(Item collectedItem)
        {
            _inventoryGold.Attributes[GameAttribute.ItemStackQuantityLo] += collectedItem.Attributes[GameAttribute.Gold];
            _inventoryGold.Attributes.SendChangedMessage(_owner.InGameClient, _inventoryGold.DynamicID);
            return _inventoryGold;
        }

        internal Item GetEquipment(int targetEquipSlot)
        {
            return GetItem(this._equipment[targetEquipSlot]);
        }

        internal Item GetEquipment(EquipmentSlotId targetEquipSlot)
        {
            return GetEquipment((int)targetEquipSlot);
        }

        public bool Reveal(Player player)
        {
            foreach (var item in Items.Values)
            {
                item.Reveal(player);
            }

            _inventoryGold.SetInventoryLocation((int)EquipmentSlotId.Gold, 0, 0);
            return true;
        }

        public bool Unreveal(Player player)
        {
            foreach (var item in Items.Values)
            {
                item.Unreveal(player);
            }

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
