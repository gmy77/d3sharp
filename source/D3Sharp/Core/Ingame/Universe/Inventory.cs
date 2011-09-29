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
    // Items are stored for this moment in GameClient, 
    // this shold be esier way to generate specific or random item by any player...
    // Putting all game items outside and place in some class in future schuld make esier way to load and save to database
    
    public class Inventory:IMessageConsumer
    {
        static readonly Logger Logger = LogManager.CreateLogger();

        public int Rows { get { return backpack.GetLength(0); } }
        public int Columns { get { return backpack.GetLength(1); } }
        
        private int[] equipment;      // array of equiped items_id  (not item)
        private int[,] backpack;      // backpack array

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
        private InventorySize GetItemInventorySize(int itemID)
        {
            
            //if (owner.InGameClient.items[itemID].Gbid == 0x789F00E3) return new InventorySize() { Width = 1, Height = 2 }; // Only for the testing sword
          
            //Actor actor = owner.CurrentWorld.GetActor(owner.InGameClient.items[itemID].Gbid);
            
            //if (actor.SnoId == 4440) return new InventorySize() { Width = 1, Height = 1 }; // minor health potion
            //if (actor.SnoId == 3245) return new InventorySize() { Width = 1, Height = 2 }; // hand axe 1
            

            return new InventorySize() { Width = 1, Height = 2 };
        }

        private bool FreeSpace(int droppedItemID, int row, int column)
        {
             InventorySize size = GetItemInventorySize(droppedItemID);

             for (int r = row; r < Math.Min(row + size.Height, Rows); r++)
                 for (int c = column; c < Math.Min(column + size.Width, Columns); c++)
                  if(backpack[r,c]!=0)
                     return false;
             return true;
        }

        private int collectOverlappingItems(int droppedItemID, int row, int column)
        {
            InventorySize dropSize = GetItemInventorySize(droppedItemID);
            List<int> overlapping = new List<int>();

            for (int r = row; r < Rows; r++)
                for (int c = 0; c < Columns; c++)
                    if (backpack[r, c] != droppedItemID)
                        if (r >= row && r <= row + dropSize.Height)
                            if (c >= column && c <= column + dropSize.Width)        // sorry for the stack
                                if (!overlapping.Contains(backpack[r, c]))
                                    overlapping.Add(backpack[r, c]);                // dont understand ....

            return overlapping.Count;
        }


        private void RemoveItem(int itemID)
        {
            for (int r = 0; r < Rows; r++)
                for (int c = 0; c < Columns; c++)
                    if (backpack[r, c] == itemID)
                        backpack[r, c] = 0;
        }

        void AddItem(int itemID, int row, int column)
        {
            // TODO does not check if item is move beyond boundaries
            InventorySize size = GetItemInventorySize(itemID);

            for (int r = row; r < Math.Min(row + size.Height, Rows); r++)
                for (int c = column; c < Math.Min(column + size.Width, Columns); c++)
                    // TODO Assert field is empty
                    backpack[r, c] = itemID;
        }


        void RepaintHero(int PlayerID)
        {
            owner.InGameClient.SendMessage(new VisualInventoryMessage()
            {
                Id = 0x004E,
                Field0 = PlayerID, // player_id/owner_id
                Field1 = new VisualEquipment()
                {
                    Field0 = new VisualItem[8]
                    {
                        new VisualItem() //Head
                            {
                               Field0 = owner.InGameClient.items[equipment[0]].Gbid,
                                Field1 = 0x00000000,
                                Field2 = 0x00000000,
                                Field3 = -1,
                            },
                        new VisualItem() //Chest
                            {
                                Field0 = owner.InGameClient.items[equipment[1]].Gbid,
                                Field1 = 0x00000000,
                                Field2 = 0x00000000,
                                Field3 = -1,
                            },
                        new VisualItem() //Feet
                            {
                                Field0 = owner.InGameClient.items[equipment[2]].Gbid,
                                Field1 = 0x00000000,
                                Field2 = 0x00000000,
                                Field3 = -1,
                            },
                        new VisualItem() //Hands
                            {
                                Field0 = owner.InGameClient.items[equipment[3]].Gbid,
                                Field1 = 0x00000000,
                                Field2 = 0x00000000,
                                Field3 = -1,
                            },
                        new VisualItem() //Main hand
                            {
                                Field0 = owner.InGameClient.items[equipment[4]].Gbid,
                                Field1 = 0x00000000,
                                Field2 = 0x00000000,
                                Field3 = -1,
                            },
                        new VisualItem() //Offhand
                            {
                                Field0 = owner.InGameClient.items[equipment[5]].Gbid,
                                Field1 = 0x00000000,
                                Field2 = 0x00000000,
                                Field3 = -1,
                            },
                        new VisualItem() //Shoulders
                            {
                                Field0 = owner.InGameClient.items[equipment[6]].Gbid,
                                Field1 = 0x00000000,
                                Field2 = 0x00000000,
                                Field3 = -1,
                            },
                        new VisualItem() //Legs
                            {
                                Field0 = owner.InGameClient.items[equipment[7]].Gbid,
                                Field1 = 0x00000000,
                                Field2 = 0x00000000,
                                Field3 = -1,
                            },
                    },
                },
            });
        }

        void EquipItem(int itemID, int Slot)
        {
            equipment[Slot] = itemID;  
        }

        void UnEquipItem(int itemID)
        {
            for (int i = 0; i < 8; i++)
                if (equipment[i] == itemID)
                    equipment[i] = 0;
        }

        void MoveItem(InventoryRequestMoveMessage request)
        {
            owner.InGameClient.SendMessage(new ACDInventoryPositionMessage()
            {
                Id = 64,                // OPCODE
                Field0 = request.Field0,    // ItemID
                Field1 = new InventoryLocationMessageData()
                {
                    Field0 = request.Field1.Field0, // Inventory Owner
                    Field1 = request.Field1.Field1, // EquipmentSlot
                    Field2 = new IVector2D()
                    {
                        Field0 = request.Field1.Field2, // Row
                        Field1 = request.Field1.Field3, // Column
                    },
                },
                Field2 = 1 // what does this do?  // 0- source item not disappearing from inventory, 1 - Moving
            }); 
        }

        public Inventory(Hero owner)
        {
            this.owner = owner;
            backpack = new int[6, 10];
            equipment = new int[8];
            for (int r = 0; r < Rows; r++)
                for (int c = 0; c < Columns; c++)
                    backpack[r, c] = 0;
            for (int i = 0; i < 8; i++)
               equipment[i] = 0;
        }
        Boolean isItemEquiped(int ItemID)
        {
           for (int i = 0; i < 8; i++)
               if(equipment[i]==ItemID)
                   return true;
            return false;     
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
                    if (backpack[r, c] == id)
                        return true;
            return false;
        }


        private InventorySlot? FindSlotForItem(int itemID)
        {
            InventorySize size = GetItemInventorySize(itemID);

            for (int r = 0; r < Rows - size.Width + 1; r++)
                for (int c = 0; c < Columns - size.Height + 1; c++)
                    if (collectOverlappingItems(itemID, r, c) == 0)
                        return new InventorySlot() { Row = r, Column = c };

            return null;
        }


        public void PickUp(TargetMessage msg)
        {
            // TODO Ensure item not in inventory
            // TODO Ensure target is an item and it exists

            //if(msg.Field1 != 0x7B1E008F) return null;

            InventorySlot? freeSlot = FindSlotForItem(msg.Field1);
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
                AddItem(msg.Field1, freeSlot.Value.Row, freeSlot.Value.Column);

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
            // equip    
            if (request.Field1.Field1 != 0)
            {
                Logger.Debug("Move Item {0}", request.AsText());    
                EquipItem(request.Field0,request.Field1.Field1);
                MoveItem(request);
                RepaintHero(request.Field1.Field0);
            }
            else // to backpack
            {
                if (FreeSpace(request.Field0, request.Field1.Field3, request.Field1.Field2))
                {    
                    if(isItemEquiped(request.Field0)) // form equipment
                    {
                        Logger.Debug("uNeQUIPEiTE,");             
                        UnEquipItem(request.Field0);
                        RepaintHero(request.Field1.Field0);
                    }
                    else
                    {
                        AddItem(request.Field0, request.Field1.Field2, request.Field1.Field3);
                    }
                    MoveItem(request); 
                }
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

        public void OnInventorySplitStackMessage(InventorySplitStackMessage msg)
        {
            // change item params.    
        }

        public void OnInventoryStackTransferMessage(InventoryStackTransferMessage msg)
        {
            owner.InGameClient.items[msg.Field0].Count = owner.InGameClient.items[msg.Field0].Count - (int)msg.Field2;
            owner.InGameClient.items[msg.Field1].Count = owner.InGameClient.items[msg.Field1].Count + (int)msg.Field2;
            //Logger.Trace("transfer stack", msg.AsText());

            //Items[msg.Field0] = Items[msg.Field0] - (int)msg.Field2;
            
            owner.InGameClient.SendMessage(new AttributeSetValueMessage   //transfer from
            {
                Id = 0x4c,
                Field0 = msg.Field0,
                Field1 = new NetAttributeKeyValue
                {
                    Attribute = GameAttribute.Attributes[0x0121], // ItemStackQuantityLo 
                    Int = owner.InGameClient.items[msg.Field0].Count, // quality
                    Float = 0f,
                }
            });
            // transfer to
            owner.InGameClient.SendMessage(new AttributeSetValueMessage
            {
                Id = 0x4c,
                Field0 = msg.Field1,
                Field1 = new NetAttributeKeyValue
                {
                    Attribute = GameAttribute.Attributes[0x0121], // ItemStackQuantityLo 
                    Int = owner.InGameClient.items[msg.Field1].Count, // count
                    Float = 0f,
                }
            });

            owner.InGameClient.PacketId += 10 * 2;
            owner.InGameClient.SendMessage(new DWordDataMessage()
            {
                Id = 0x89,
                Field0 = owner.InGameClient.PacketId,
            }); 
        }

        public void Consume(GameClient client, GameMessage message)
        {
            if (message is InventoryRequestMoveMessage) HandleRequest(message as InventoryRequestMoveMessage);
            else if (message is InventorySplitStackMessage) OnInventorySplitStackMessage(message as InventorySplitStackMessage);
            else if (message is InventoryStackTransferMessage) OnInventoryStackTransferMessage(message as InventoryStackTransferMessage);
            else return;
        }
    }
}
