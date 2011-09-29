using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using D3Sharp.Net.Game.Message.Definitions;
using D3Sharp.Net.Game.Message.Fields;
using D3Sharp.Net.Game;
using D3Sharp.Net.Game.Message.Definitions.ACD;
using D3Sharp.Net.Game.Message.Definitions.Misc;
using D3Sharp.Net.Game.Message.Definitions.Attribute;
using D3Sharp.Net.Game.Message.Definitions.Inventory;
using D3Sharp.Net.Game.Message.Definitions.Combat;
using D3Sharp.Core.Common.Items;
using D3Sharp.Core.Ingame.Actors;
using D3Sharp.Net.Game.Message;
using D3Sharp.Utils;

namespace D3Sharp.Core.Ingame.Universe
{
    public class Inventory:IMessageConsumer
    {
        private static readonly Logger Logger = LogManager.CreateLogger();

        public int Rows { get { return items.GetLength(0); } }
        public int Columns { get { return items.GetLength(1); } }
       //public Dictionary<int, Item> inventory = new Dictionary<ihn, Item>();

        Item[,] items;
        private Hero owner; // Used, because most information is not in the item class but Actors managed by the world

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

        // This should be in the database
        private InventorySize GetItemInventorySize(Item item)
        {
            if (Item.isWeapon(item.Type)) // Or Shield or ....
            {
                return new InventorySize() { Width = 1, Height = 2 };
            }
            else
            {
                return new InventorySize() { Width = 1, Height = 1 };
            }           
        }

        private List<Item> collectOverlappingItems(Item droppedItem, int row, int column)
        {
            InventorySize dropSize = GetItemInventorySize(droppedItem);
            List<Item> overlapping = new List<Item>();

            for (int r = 0; r < Rows; r++)
                for (int c = 0; c < Columns; c++)
                    if (items[r,c] != null && items[r, c].Gbid != droppedItem.Gbid)
                        if (r >= row && r <= row + dropSize.Height)
                            if (c >= column && c <= column + dropSize.Width)        // sorry for the stack
                                if (!overlapping.Contains(items[r, c]))
                                    overlapping.Add(items[r, c]);

            return overlapping;
        }


        private void RemoveItem(Item item)
        {
            for (int r = 0; r < Rows; r++)
                for (int c = 0; c < Columns; c++)
                    if (items[r, c] != null && items[r, c].ItemId == item.ItemId)
                        items[r, c] = null;
        }

        private Item GetItem(int id)
        {
            for (int r = 0; r < Rows; r++)
                for (int c = 0; c < Columns; c++)
                    if (items[r, c] != null && items[r, c].ItemId == id)
                        return items[r, c];

            return null;
        }

        void AddItem(Item item, int row, int column)
        {
            // TODO does not check if item is move beyond boundaries
            InventorySize size = GetItemInventorySize(item);            
            for (int r = row; r < Math.Min(row + size.Height, Rows); r++)
                for (int c = column; c < Math.Min(column + size.Width, Columns); c++)
                    // TODO Assert field is empty
                    items[r, c] = item;
        }

        void MoveItem(Item item, int row, int column)
        {
            RemoveItem(item);
            AddItem(item, row, column);
        }


        public Inventory(Hero owner)
        {
            this.owner = owner;
            items = new Item[6, 10];
        }


        /// <summary>
        /// Checks wheter the inventory contains an item
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Contains(int id)
        {
            for (int r = 0; r < Rows; r++)
                for (int c = 0; c < Columns; c++)
                    if (items[r,c] != null && items[r, c].ItemId == id)
                        return true;
            return false;
        }


        private InventorySlot? FindSlotForItem(Item item)
        {
            InventorySize size = GetItemInventorySize(item);

            for (int r = 0; r < Rows - size.Width + 1; r++)
                for (int c = 0; c < Columns - size.Height + 1; c++)
                    if (collectOverlappingItems(item, r, c).Count == 0)
                        return new InventorySlot() { Row = r, Column = c };

            return null;
        }


        public void PickUp(TargetMessage msg)
        {
            // TODO Ensure item not in inventory
            // TODO Ensure target is an item and it exists

            //if(msg.Field1 != 0x7B1E008F) return null;

            Item item = new Item(msg.Field1, 0, ItemType.Mace_1H); // Pickup mean: this item exists already and musst be retrieved from somewhere 
            InventorySlot? freeSlot = FindSlotForItem(item);
            if (freeSlot == null)
            {
                //Inventory full
                owner.InGameClient.SendMessage(new ACDPickupFailedMessage()
                {
                    Id = (int)Opcodes.ACDPickupFailedMessage,
                    ItemId = msg.Field1,
                    Reason = ACDPickupFailedMessage.Reasons.InventoryFull
                });
            }
            else
            {
                AddItem(item, freeSlot.Value.Row, freeSlot.Value.Column);

                owner.InGameClient.SendMessage(new ACDInventoryPositionMessage()
                {
                    Id = 64,                // OPCODE
                    Field0 = msg.Field1,    // ItemID
                    Field1 = new InventoryLocationMessageData()
                    {
                        Field0 = 0x789E00E2, // Inventory Owner
                        Field1 = 0x00000000, // EquipmentSlot
                        Field2 = new IVector2D()
                        {
                            x = freeSlot.Value.Column,
                            y = freeSlot.Value.Row
                        },
                    },
                    Field2 = 1  //?
                });
            }

            // Finalize....necessary?
            owner.InGameClient.PacketId += 10 * 2;
            owner.InGameClient.SendMessage(new DWordDataMessage()
            {
                Id = 0x89,
                Field0 = owner.InGameClient.PacketId,
            });

            owner.InGameClient.FlushOutgoingBuffer();
        }



        public void HandleRequest(InventoryRequestMoveMessage request)
        {
            System.Diagnostics.Debug.Assert(Contains(request.Field0));


            if(!Contains(request.Field0)){
                Logger.Warn("Item {0} is not present in Inventory! ", request.Field0 );
                return;
            }else{
                Item item = GetItem(request.Field0);

                if (collectOverlappingItems(item, request.Field1.Field3, request.Field1.Field2).Count == 0)
                {
                    MoveItem(item, request.Field1.Field3, request.Field1.Field2);

                    owner.InGameClient.SendMessage(new ACDInventoryPositionMessage()
                    {
                        Id = 64,                // OPCODE
                        Field0 = request.Field0,    // ItemID
                        Field1 = new InventoryLocationMessageData()
                        {
                            Field0 = 0x789E00E2, // Inventory Owner
                            Field1 = 0x00000000, // EquipmentSlot
                            Field2 = new IVector2D()
                            {
                                x = request.Field1.Field2, // Row
                                y = request.Field1.Field3, // Column
                            },
                        },
                        Field2 = 1 // what does this do?
                    });

                    // Finalize....necessary?
                    owner.InGameClient.PacketId += 10 * 2;
                    owner.InGameClient.SendMessage(new DWordDataMessage()
                    {
                        Id = 0x89,
                        Field0 = owner.InGameClient.PacketId,
                    });

                    owner.InGameClient.FlushOutgoingBuffer();

                }
            }
        }


        public void Consume(GameClient client, GameMessage message)
        {
            if (message is InventoryRequestMoveMessage) HandleRequest(message as InventoryRequestMoveMessage);
        }
    }
}
