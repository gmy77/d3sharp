using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mooege.Core.Common.Items;
using Mooege.Net.GS.Message.Fields;

namespace Mooege.Core.GS.Player
{

    // these ids are transmitted by the client when equipping an item         
    public enum EquipmentSlotId
    {
        Helm = 1, Chest = 2, Off_Hand = 3, Main_Hand = 4, Hands = 5, Belt = 6, Feet = 7,
        Shoulders = 8, Legs = 9, Bracers = 10, Ring_right = 11, Ring_left = 12, Amulett = 13
    }

    class Equipment
    {
        public int EquipmentSlots { get { return _equipment.GetLength(0); } }
        
        private readonly Mooege.Core.GS.Player.Player _owner; // Used, because most information is not in the item class but Actors managed by the world
        private Item _goldItem;

        private uint[] _equipment;      // array of equiped items_id  (not item)

        public Equipment(Player owner){
            this._equipment = new uint[16];
            this._goldItem = null;           
            this._owner = owner;
        }
       
        /// <summary>
        /// Equips an item in an equipment slot
        /// </summary>
        public void EquipItem(Item item, int slot)
        {
            _equipment[slot] = item.DynamicID;
            item.Owner = _owner;
            item.SetInventoryLocation(slot, 0, 0);            
        }

        public void EquipItem(uint itemID, int slot)
        {
            EquipItem(_owner.World.GetItem(itemID), slot);
        }

        /// <summary>
        /// Removes an item from the equipment slot it uses
        /// returns the used equipmentSlot
        /// </summary>
        public int UnequipItem(Item item)
        {
            for (int i = 0; i < EquipmentSlots; i++)
            {
                if (_equipment[i] == item.DynamicID)
                {
                    _equipment[i] = 0;
                    item.SetInventoryLocation(-1, -1, -1);
                    item.Owner = null;
                    return i;
                }
            }

            return 0;
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

        private VisualItem GetEquipmentItem(EquipmentSlotId equipSlot)
        {
            if (_equipment[(int)equipSlot] == 0)
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
                return _owner.World.GetItem(_equipment[(int)equipSlot]).CreateVisualItem();
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

        internal Item AddGoldItem(Item collectedItem)
        {
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

            return _goldItem;
        }

        internal Item GetEquipment(int targetEquipSlot)
        {
            return _owner.World.GetItem(this._equipment[targetEquipSlot]);
        }

        internal Item GetEquipment(EquipmentSlotId targetEquipSlot)
        {
            return GetEquipment((int)targetEquipSlot);
        }
    }
}
