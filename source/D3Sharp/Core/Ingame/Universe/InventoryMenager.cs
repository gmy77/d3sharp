using System.Collections.Generic;
using D3Sharp.Core.Helpers;
using D3Sharp.Core.Ingame.Map;
using D3Sharp.Net.Game.Message;
using D3Sharp.Utils;
using D3Sharp.Net.Game;
using D3Sharp.Net.Game.Message.Definitions.Misc;
using D3Sharp.Net.Game.Message.Fields;
using D3Sharp.Net.Game.Message.Definitions.Animation;
using D3Sharp.Net.Game.Message.Definitions.Player;
using D3Sharp.Net.Game.Message.Definitions.Inventory;
using D3Sharp.Net.Game.Message.Definitions.ACD;
using D3Sharp.Core.Common.Items;


namespace D3Sharp.Core.Ingame.Universe
{
    public class InventoryMenager
    {

        static readonly Logger Logger = LogManager.CreateLogger();
        public long Head;

        //
        public Dictionary<int, int> Item_Count = new Dictionary<int, int>();
        public Dictionary<int, int> Item_ID = new Dictionary<int, int>();
        //
        public InventoryMenager()
        {
            Logger.Info("InventoryMenager Initialize");
        }
        // messages 
        public void Consume(GameClient client, GameMessage message)
        {
            Logger.Info("INV {0}", message.AsText());
            if (message is InventoryRequestMoveMessage) OnInventoryRequestMoveMessage(client, (InventoryRequestMoveMessage)message);
            else if (message is InventorySplitStackMessage) OnInventorySplitStackMessage(client, (InventorySplitStackMessage)message);
            else if (message is InventoryStackTransferMessage) OnInventoryStackTransferMessage(client, (InventoryStackTransferMessage)message);
            else return;

            client.FlushOutgoingBuffer();
        }


        public void OnInventoryRequestMoveMessage(GameClient client, InventoryRequestMoveMessage msg)
        {
            Logger.Info("INV: InventoryRequestMoveMessage {0}", msg.AsText());
            client.SendMessage(new ACDInventoryPositionMessage()
            {
                Id = 0x0040,
                Field0 = msg.Field0,
                Field1 = new InventoryLocationMessageData()
                {
                    Field0 = msg.Field1.Field0,
                    Field1 = msg.Field1.Field1,
                    Field2 = new IVector2D()
                    {
                        Field0 = msg.Field1.Field2,
                        Field1 = msg.Field1.Field3,
                    },
                },
                Field2 = 0x0000001 // if is 0x1 item move ....
            });
            if (msg.Field1.Field1 > 0)
            {
                
                 Wear(client, msg);
                // change hero params
            }


            client.FlushOutgoingBuffer();
        }

        public void OnInventorySplitStackMessage(GameClient client, InventorySplitStackMessage message)
        {
            // change item params.    
        }

        public void OnInventoryStackTransferMessage(GameClient client, InventoryStackTransferMessage message)
        {
            // change item params and create new item
        }

        public void CreateItem (GameClient Client, int posX=0x0, int posY=0x0)
        {
            // randomize item id // not safe 
            var iid = RandomHelper.Next(0x78A000E4, 0x78A00F00);
            // 
            // generete item
            var itemsGenerator = new ItemTypeGenerator();
            int Gbid = itemsGenerator.generateRandomElement(ItemType.Helm).Gbid;

            Item_ID.Add(iid, Gbid);

            var item = D3.Hero.VisualItem.CreateBuilder()
                              .SetGbid(Gbid)
                              .SetDyeType(0)
                              .SetItemEffectType(0)
                              .SetEffectLevel(0)
                              .Build();

            Client.SendMessage(new ACDEnterKnownMessage()
            {
                Id = 0x003B, // stworzenie przedmiotu ....
                //Field0 = 0x78A000E4,
                Field0 = iid,               // id
                Field1 = 0x00001158,        // ?? Gfx
                Field2 = 0x00000001,        // ????
                Field3 = 0x00000001,        // ????
                Field4 = null,
                Field5 = new InventoryLocationMessageData()
                {
                    Field0 = 0x789E00E2,    // player_id should be not static ....
                    Field1 = 0x00000000,    // item place ... 0- backpack
                    Field2 = new IVector2D()
                    {
                        Field0 = posX, // pos x in backapck (0-9)
                        Field1 = posX, // pos y in backpack (0-5)
                    },
                },
                Field6 = new GBHandle()
                {
                    Field0 = 0x00000002,
                    Field1 = item.Gbid,   // item gfx id ....
                },
                Field7 = -1,                // dye
                Field8 = -1,                // efects
                Field9 = 0x00000001,        //
                Field10 = 0x00,             //
            });
        }

        public void Wear(GameClient Client, InventoryRequestMoveMessage msg)
        {
            //Logger.Info("Equipe item id {0}", msg.Field0);
            //Logger.Info("D3 item id {0}", Client.Player.Hero.Properties.Equipment.VisualItemList[1].Gbid); 
            //
            //
            
            Client.SendMessage(new VisualInventoryMessage()
            {
                Id = 0x004E,
                Field0 = msg.Field1.Field0, // player_id/owner_id
                Field1 = new VisualEquipment()
                {
                    Field0 = new VisualItem[8]
                    {
                            /*
                             *  Field0 = Client.Player.Hero.Properties.Equipment.VisualItemList[0].Gbid,
                                Field1 = Client.Player.Hero.Properties.Equipment.VisualItemList[0].DyeType,
                                Field2 = Client.Player.Hero.Properties.Equipment.VisualItemList[0].ItemEffectType,
                                Field3 = Client.Player.Hero.Properties.Equipment.VisualItemList[0].EffectLevel,
                             * */
                        new VisualItem() //Head
                            {
                                Field0 = Item_ID[msg.Field0],
                                Field1 = 0x00000000,
                                Field2 = 0x00000000,
                                Field3 = -1,
                            },
                        new VisualItem() //Chest
                            {
                                Field0 = Client.Player.Hero.Properties.Equipment.VisualItemList[1].Gbid,
                                Field1 = 0x00000000,
                                Field2 = 0x00000000,
                                Field3 = -1,
                            },
                        new VisualItem() //Feet
                            {
                                Field0 = Client.Player.Hero.Properties.Equipment.VisualItemList[2].Gbid,
                                Field1 = 0x00000000,
                                Field2 = 0x00000000,
                                Field3 = -1,
                            },
                        new VisualItem() //Hands
                            {
                                Field0 = Client.Player.Hero.Properties.Equipment.VisualItemList[3].Gbid,
                                Field1 = 0x00000000,
                                Field2 = 0x00000000,
                                Field3 = -1,
                            },
                        new VisualItem() //Main hand
                            {
                                Field0 = Client.Player.Hero.Properties.Equipment.VisualItemList[4].Gbid,
                                Field1 = 0x00000000,
                                Field2 = 0x00000000,
                                Field3 = -1,
                            },
                        new VisualItem() //Offhand
                            {
                                Field0 = Client.Player.Hero.Properties.Equipment.VisualItemList[5].Gbid,
                                Field1 = 0x00000000,
                                Field2 = 0x00000000,
                                Field3 = -1,
                            },
                        new VisualItem() //Shoulders
                            {
                                Field0 = Client.Player.Hero.Properties.Equipment.VisualItemList[6].Gbid,
                                Field1 = 0x00000000,
                                Field2 = 0x00000000,
                                Field3 = -1,
                            },
                        new VisualItem() //Legs
                            {
                                Field0 =Client.Player.Hero.Properties. Equipment.VisualItemList[7].Gbid,
                                Field1 = 0x00000000,
                                Field2 = 0x00000000,
                                Field3 = -1,
                            },
                    },
                },
            });
            //
            Client.SendMessage(new PlayerActorSetInitialMessage()
            {
                Id = 0x0039,
                Field0 = 0x789E00E2,
                Field1 = 0x00000000,
            });
            
            //
        }
    }


}
