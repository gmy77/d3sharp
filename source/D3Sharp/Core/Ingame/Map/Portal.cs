using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using D3Sharp.Core.Ingame.Actors;
using D3Sharp.Core.Ingame.Universe;
using D3Sharp.Net.Game.Message.Definitions.Misc;
using D3Sharp.Net.Game.Message.Definitions.ACD;
using D3Sharp.Net.Game.Message.Definitions.Attribute;
using D3Sharp.Net.Game.Message.Fields;
using D3Sharp.Net.Game.Message;

using D3Sharp.Core.Ingame.Universe;

namespace D3Sharp.Core.Ingame.Map
{
    public class Portal
    {
        public Actor ActorRef;
        public PortalSpecifierMessage PortalMessage;

        public int TargetWorldID;
        public Vector3D TargetPos;

        public void Reveal(IngameToon t)
        {
            if (PortalMessage != null && ActorRef != null && TargetPos!=null)
                //targetpos!=null in this case is used to detect if the portal has been completely initialized to have a target
                //if it doesn't have one, it won't be displayed - otherwise the client would crash from this.
            {
                ActorRef.Reveal(t);

                t.InGameClient.SendMessage(new AffixMessage()
                {
                    Id = 0x48,
                    Field0 = ActorRef.ID,
                    Field1 = 1,
                    aAffixGBIDs = new int[0]
                });

                t.InGameClient.SendMessage(new AffixMessage()
                {
                    Id = 0x48,
                    Field0 = ActorRef.ID,
                    Field1 = 2,
                    aAffixGBIDs = new int[0]
                });

                t.InGameClient.SendMessage(PortalMessage);

                t.InGameClient.SendMessage(new ACDCollFlagsMessage()
                {
                    Id = 0x00A6,
                    Field0 = ActorRef.ID,
                    Field1 = 0x00000001,
                });

                t.InGameClient.SendMessage(new AttributesSetValuesMessage()
                {
                    Id = 0x004D,
                    Field0 = ActorRef.ID,
                    atKeyVals = new NetAttributeKeyValue[7]
                    {
                         new NetAttributeKeyValue()
                         {
                            Attribute = GameAttribute.Attributes[0x02BC], // MinimapActive 
                            Int = 0x00000001,
                            Float = 0f,
                         },
                         new NetAttributeKeyValue()
                         {
                            Attribute = GameAttribute.Attributes[0x0056], // Hitpoints_Max_Total 
                            Int = 0x00000000,
                            Float = 1f,
                         },
                         new NetAttributeKeyValue()
                         {
                            Attribute = GameAttribute.Attributes[0x0054], // Hitpoints_Max 
                            Int = 0x00000000,
                            Float = 0.0009994507f,
                         },
                         new NetAttributeKeyValue()
                         {
                            Attribute = GameAttribute.Attributes[0x0051], // Hitpoints_Total_From_Level 
                            Int = 0x00000000,
                            Float = 3.051758E-05f,
                         },
                         new NetAttributeKeyValue()
                         {
                            Attribute = GameAttribute.Attributes[0x004D], // Hitpoints_Cur 
                            Int = 0x00000000,
                            Float = 0.0009994507f,
                         },
                         new NetAttributeKeyValue()
                         {
                            Attribute = GameAttribute.Attributes[0x0043], // TeamID 
                            Int = 0x00000001,
                            Float = 0f,
                         },
                         new NetAttributeKeyValue()
                         {
                            Attribute = GameAttribute.Attributes[0x0026], // Level 
                            Int = 0x00000001,
                            Float = 0f,
                         },
                    },
                });

                t.InGameClient.SendMessage(new ACDGroupMessage()
                {
                    Id = 0x00B8,
                    Field0 = ActorRef.ID,
                    Field1 = -1,
                    Field2 = -1,
                });

                t.InGameClient.SendMessage(new ANNDataMessage()
                {
                    Id = 0x003E,
                    Field0 = ActorRef.ID,
                });

                t.InGameClient.SendMessage(new ACDTranslateFacingMessage()
                {
                    Id = 0x0070,
                    Field0 = ActorRef.ID,
                    Field1 = 0f,
                    Field2 = false,
                });

            }
            t.InGameClient.FlushOutgoingBuffer();
        }

    }
}
