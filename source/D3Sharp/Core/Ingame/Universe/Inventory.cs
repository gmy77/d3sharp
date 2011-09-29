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

namespace D3Sharp.Core.Ingame.Universe
{
    public class Inventory:IMessageConsumer
    {
        public int Rows { get { return items.GetLength(0); } }
        public int Columns { get { return items.GetLength(1); } }
        
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
            if (item.Gbid == 0x789F00E3) return new InventorySize() { Width = 1, Height = 2 }; // Only for the testing sword
 
            Actor actor = owner.CurrentWorld.GetActor(item.Gbid);

            if (actor.SnoId == 4440) return new InventorySize() { Width = 1, Height = 1 }; // minor health potion
            if (actor.SnoId == 3245) return new InventorySize() { Width = 1, Height = 2 }; // hand axe 1


            return new InventorySize() { Width = 1, Height = 1 };
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
                    if (items[r, c] != null && items[r, c].Gbid == item.Gbid)
                        items[r, c] = null;
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
                    if (items[r,c] != null && items[r, c].Gbid == id)
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

            InventorySlot? freeSlot = FindSlotForItem(new Item(msg.Field1, ItemType.Belt));
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
                AddItem(new Item(msg.Field1, ItemType.Belt), freeSlot.Value.Row, freeSlot.Value.Column);

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
                            Field0 = freeSlot.Value.Column,
                            Field1 = freeSlot.Value.Row
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

            Item item = new Item(request.Field0, ItemType.Belt); // dummy, get actual item

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
                            Field0 = request.Field1.Field2, // Row
                            Field1 = request.Field1.Field3, // Column
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


        public void Consume(GameClient client, GameMessage message)
        {
            if (message is InventoryRequestMoveMessage) HandleRequest(message as InventoryRequestMoveMessage);
        }
    }
}
