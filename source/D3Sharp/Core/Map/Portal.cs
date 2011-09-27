using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using D3Sharp.Core.Actors;
using D3Sharp.Net.Game.Message.Definitions.Misc;
using D3Sharp.Net.Game.Message.Definitions.ACD;
using D3Sharp.Net.Game.Message.Definitions.Attribute;
using D3Sharp.Net.Game.Message.Fields;
using D3Sharp.Net.Game.Messages;

using D3Sharp.Core.Toons;

namespace D3Sharp.Core.Map
{
    public class Portal
    {
        public Actor ActorRef;
        public PortalSpecifierMessage PortalMessage;

        public void Reveal(Toon t)
        {
            if (PortalMessage != null && ActorRef != null)
            {
                ActorRef.Reveal(t);

                t.Owner.LoggedInBNetClient.InGameClient.SendMessage(new AffixMessage()
                {
                    Id = 0x48,
                    Field0 = ActorRef.ID,
                    Field1 = 1,
                    aAffixGBIDs = new int[0]
                });

                t.Owner.LoggedInBNetClient.InGameClient.SendMessage(new AffixMessage()
                {
                    Id = 0x48,
                    Field0 = ActorRef.ID,
                    Field1 = 2,
                    aAffixGBIDs = new int[0]
                });

                t.Owner.LoggedInBNetClient.InGameClient.SendMessage(PortalMessage);

                t.Owner.LoggedInBNetClient.InGameClient.SendMessage(new ACDCollFlagsMessage()
                {
                    Id = 0x00A6,
                    Field0 = ActorRef.ID,
                    Field1 = 0x00000001,
                });

                t.Owner.LoggedInBNetClient.InGameClient.SendMessage(new AttributesSetValuesMessage()
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

                t.Owner.LoggedInBNetClient.InGameClient.SendMessage(new ACDGroupMessage()
                {
                    Id = 0x00B8,
                    Field0 = ActorRef.ID,
                    Field1 = -1,
                    Field2 = -1,
                });

                t.Owner.LoggedInBNetClient.InGameClient.SendMessage(new ANNDataMessage()
                {
                    Id = 0x003E,
                    Field0 = ActorRef.ID,
                });

                t.Owner.LoggedInBNetClient.InGameClient.SendMessage(new ACDTranslateFacingMessage()
                {
                    Id = 0x0070,
                    Field0 = ActorRef.ID,
                    Field1 = 0f,
                    Field2 = false,
                });

            }
            t.Owner.LoggedInBNetClient.InGameClient.FlushOutgoingBuffer();
        }

    }
}
