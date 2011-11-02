using Mooege.Core.Common.Items;
using Mooege.Net.GS.Message.Fields;
using Mooege.Net.GS.Message;

namespace Mooege.Core.GS.Players
{

    // these ids are transmitted by the client when equipping an item         
    public enum EquipmentSlotId
    {
        Helm = 1, Chest = 2, Off_Hand = 3, Main_Hand = 4, Hands = 5, Belt = 6, Feet = 7,
        Shoulders = 8, Legs = 9, Bracers = 10, Ring_right = 11, Ring_left = 12, Amulett = 13,
        Stash = 17, Vendor = 20 // To do: Should this be here? Its not really an eq. slot /fasbat
    }

    class Equipment
    {
        public int EquipmentSlots { get { return _equipment.GetLength(0); } }
        
        private readonly Player _owner; // Used, because most information is not in the item class but Actors managed by the world
        private Item _inventoryGold;

        private uint[] _equipment;      // array of equiped items_id  (not item)

        public Equipment(Player owner){
            this._equipment = new uint[16];
            this._inventoryGold = null;           
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
                    GbId = -1, // 0 causes error logs on the client  - angerwin
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

        public Item AddGoldItem(Item collectedItem)
        {
            // the logic is flawed, we shouldn't be creating new gold when it's collected! /raist.

            if (_inventoryGold == null)
            {
                _inventoryGold = ItemGenerator.CreateGold(_owner, collectedItem.Attributes[GameAttribute.Gold]);
                _inventoryGold.Attributes[GameAttribute.ItemStackQuantityLo] = collectedItem.Attributes[GameAttribute.Gold];
                _inventoryGold.Owner = _owner;
                _inventoryGold.SetInventoryLocation(18, 0, 0); // Equipment slot 18 ==> Gold
                _inventoryGold.Reveal(_owner);
            }
            else
            {
                _inventoryGold.Attributes[GameAttribute.ItemStackQuantityLo] += collectedItem.Attributes[GameAttribute.Gold];
                //_inventoryGold.Attributes.SendChangedMessage(_owner.InGameClient, _inventoryGold.DynamicID); // causes: !!!ERROR!!! Setting attribute for unknown ACD [ANN:1253] [Attribute:ItemStackQuantityLo-1048575:	8669] and client crash /raist.
            }

            return _inventoryGold;
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
