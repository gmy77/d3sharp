using D3Sharp.Core.Ingame.Universe;
using D3Sharp.Net.Game.Message.Definitions.ACD;
using D3Sharp.Net.Game.Message.Fields;

namespace D3Sharp.Core.Ingame.Actors
{
    public class Actor
    {
        public int Id;
        public int snoID;
        public ACDEnterKnownMessage RevealMessage;

        // new 
        public Vector3D Position = new Vector3D();
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
                        Field0 = Id,
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
   
        public void ParseFrom(int worldId, string[] data)
        {
            this.WorldId = worldId;
            this.Id = int.Parse(data[4]);
            this.snoID = int.Parse(data[5]);
            this.Field2 = int.Parse(data[6]);
            this.Field3 = int.Parse(data[7]);

            if (int.Parse(data[2]) > 0)
            {
                this.Scale = float.Parse(data[8], System.Globalization.CultureInfo.InvariantCulture);
                this.RotationAmount = float.Parse(data[12], System.Globalization.CultureInfo.InvariantCulture);
                this.RotationAxis = new Vector3D()
                                        {
                                            X = float.Parse(data[9], System.Globalization.CultureInfo.InvariantCulture),
                                            Y = float.Parse(data[10], System.Globalization.CultureInfo.InvariantCulture),
                                            Z = float.Parse(data[11], System.Globalization.CultureInfo.InvariantCulture),
                                        };

                this.Position = new Vector3D()
                                    {
                                        X = float.Parse(data[13], System.Globalization.CultureInfo.InvariantCulture),
                                        Y = float.Parse(data[14], System.Globalization.CultureInfo.InvariantCulture),
                                        Z = float.Parse(data[15], System.Globalization.CultureInfo.InvariantCulture),
                                    };
            }

            // data[14] = world_id

            if (int.Parse(data[3]) > 0)
            {
                this.InventoryLocationData = new InventoryLocationMessageData()
                {
                    Field0 = int.Parse(data[17]),
                    Field1 = int.Parse(data[18]),
                    Field2 = new IVector2D()
                    {
                        Field0 = int.Parse(data[19]),
                        Field1 = int.Parse(data[20]),
                    }
                };
            }

            this.GBHandle = new GBHandle()
            {
                Field0 = int.Parse(data[21]),
                Field1 = int.Parse(data[22]),
            };

            this.Field7 = int.Parse(data[23]);
            this.Field8 = int.Parse(data[24]);
            this.Field9 = int.Parse(data[25]);
            this.Field10 = byte.Parse(data[26]); 
        }
    }
}
