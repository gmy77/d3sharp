using D3Sharp.Core.Ingame.Universe;
using D3Sharp.Net.Game.Message.Definitions.ACD;
using D3Sharp.Net.Game.Message.Fields;

namespace D3Sharp.Core.Ingame.Actors
{
    public class Actor
    {
        public int ID;
        public int snoID;
        public ACDEnterKnownMessage RevealMessage;

        // new 
        public Vector3D Position = new Vector3D();
        public int Field0;
        public int Field2;
        public int Field3;
        public InventoryLocationMessageData InventoryLocationData;
        public int Field7;
        public int Field8;
        public int Field9;
        public byte Field10;

        public float Scale;
        public float RotationAmount;
        public Vector3D RotationAxis;
        public GBHandle GBHandle;
        public int WorldId;

        public void Reveal(Hero toon)
        {
            if(RevealMessage==null)
            {
                this.RevealMessage=new ACDEnterKnownMessage
                    {
                        Field0 = Field0,
                        Field1 = snoID,
                        Field2 = Field2,
                        Field3 = Field3,
                        Field4 = new WorldLocationMessageData()
                                     {
                                         Field0 = Scale,
                                         Field1 = new PRTransform()
                                                      {
                                                          Field0 = new Quaternion()
                                                                       {
                                                                           Field0 = RotationAmount,
                                                                           Field1 = RotationAxis,
                                                                       },
                                                          Field1 = Position,
                                                      },
                                         Field2 = WorldId,
                                     },
                        Field5 = InventoryLocationData,
                        Field6 = GBHandle,
                        Field7 = Field7,
                        Field8 = Field8,
                        Field9 = Field9,
                        Field10 = Field10,
                    };
            }
            
            
            toon.InGameClient.SendMessage(RevealMessage);
            toon.InGameClient.FlushOutgoingBuffer();
        }

        public void RevealNew(Player player)
        {
            player.Client.SendMessage(new ACDEnterKnownMessage
            {
                Field0 = Field0,
                Field1 = snoID, 
                Field2 = Field2,
                Field3 = Field3,
                Field4 = new WorldLocationMessageData()
                {
                    Field0 = Scale,
                    Field1 = new PRTransform()
                    {
                        Field0 = new Quaternion()
                        {
                            Field0 = RotationAmount,
                            Field1 = RotationAxis,
                        },
                        Field1 = Position,
                    },
                    Field2 = WorldId,
                },
                Field5 = null,
                Field6 = GBHandle,
                Field7 = Field7,
                Field8 = Field8,
                Field9 = Field9,
                Field10 = Field10,
            });

            player.Client.FlushOutgoingBuffer();
        }

    }
}
