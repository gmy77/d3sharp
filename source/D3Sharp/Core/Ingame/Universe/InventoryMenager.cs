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
using D3Sharp.Net.Game.Message.Definitions.Attribute;
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


        public void CreateItem (GameClient Client, int posX=0x0, int posY=0x0)
        {
            // randomize item id // not safe 
            var iid = RandomHelper.Next(0x78A000E4, 0x78A00F00);
            // 
            // generete item
            var itemsGenerator = new ItemTypeGenerator();
            int Gbid = itemsGenerator.generateRandomElement(ItemType.Helm).Gbid;

            Item_ID.Add(iid, Gbid);

       

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
                    Field1 = Gbid,   // item gfx id ....
                },
                Field7 = -1,                // dye
                Field8 = -1,                // efects
                Field9 = 0x00000001,        //
                Field10 = 0x00,             //
            });
        }

        public void AddToInventory(GameClient Client, Item item,int PlayerID=0,int posX=0,int PosY=0)
        {
            // randomize item id // not safe 
            var iid = RandomHelper.Next(0x78A000E4, 0x78A00F00);
            Item_ID.Add(iid, item.Gbid);

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
            // atributes
            Client.SendMessage(new AttributesSetValuesMessage()
            {
                Id = 0x004D,
                Field0 = iid,
                atKeyVals = new NetAttributeKeyValue[3]
                {
                    new NetAttributeKeyValue()
                    {
                        Attribute = GameAttribute.Attributes[274], 
                        Int = 0x0000005,
                        Float = 0f,
                    },
                    new NetAttributeKeyValue()
                    {
                        Attribute = GameAttribute.Attributes[275], 
                        Int = 0x0000015,
                        Float = 0f,
                    },
                    new NetAttributeKeyValue()
                    {
                        Attribute = GameAttribute.Attributes[155], 
                        Int = 0x0000000,
                        Float = 1f,
                    }
                },
            });
            //
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
           
            
            //
        }
    }


}
