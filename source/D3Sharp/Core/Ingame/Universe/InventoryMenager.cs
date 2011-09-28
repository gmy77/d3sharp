using System.Collections.Generic;
using D3Sharp.Core.Helpers;
using D3Sharp.Core.Ingame.Map;
using D3Sharp.Net.Game.Message;
using D3Sharp.Utils;
using D3Sharp.Net.Game;
using D3Sharp.Net.Game.Message.Definitions.Misc;
using D3Sharp.Net.Game.Message.Fields;
using D3Sharp.Net.Game.Message.Definitions.Inventory;
using D3Sharp.Net.Game.Message.Definitions.ACD;
using D3Sharp.Core.Common.Items;


namespace D3Sharp.Core.Ingame.Universe
{
    public class InventoryMenager
    {

        static readonly Logger Logger = LogManager.CreateLogger();
        public long Head;
        public int PacketID = 0x0000077;

        //
        public Dictionary<ulong, int> Items = new Dictionary<ulong, int>();
        //
        public InventoryMenager()
        {
            //var itemsGenerator = new ItemTypeGenerator();
            //Head = itemsGenerator.generateRandomElement(ItemType.Helm).Gbid;
            Logger.Info("InventoryMenager Initialize");
            //



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
                // this.Wear(client, msg);
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

        public void Wear(GameClient client, InventoryRequestMoveMessage message)
        {
            client.SendMessage(new VisualInventoryMessage()
            {
                Id = 0x004E,
                Field0 = message.Field0,
                Field1 = new VisualEquipment()
                {
                    Field0 = new VisualItem[1]
                    {
                        new VisualItem() 
                        {
                            Field0 = message.Field1.Field0,
                            Field1 = 0x00000000,
                            Field2 = 0x00000000,
                            Field3 = -1,
                        },
                    },
                },
            });
        }
    }


}
