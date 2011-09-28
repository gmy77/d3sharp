using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using D3Sharp.Utils;
using D3Sharp.Core.Ingame.Actors;
using D3Sharp.Core.Ingame.Universe;
using D3Sharp.Net.Game.Message.Definitions.Misc;
using D3Sharp.Net.Game.Message.Definitions.ACD;
using D3Sharp.Net.Game.Message.Definitions.Attribute;
using D3Sharp.Net.Game.Message.Fields;
using D3Sharp.Net.Game.Message;

namespace D3Sharp.Core.Ingame.Map
{
    public class Portal
    {
        static readonly Logger Logger = LogManager.CreateLogger();
        
        public Actor ActorRef;
        public PortalSpecifierMessage PortalMessage;

        public int TargetWorldID;
        public Vector3D TargetPos;

        //uncomment this constructor to show all portals in the world, even those that will crash the client on entry
        //useful for determining which actor the portal is.
        //public Portal()
        //{
        //    TargetPos = new Vector3D();
        //}

        public void Reveal(Hero hero)
        {
            if (PortalMessage != null && ActorRef != null && TargetPos!=null)
                //targetpos!=null in this case is used to detect if the portal has been completely initialized to have a target
                //if it doesn't have one, it won't be displayed - otherwise the client would crash from this.
            {
                //Logger.Info("Revealing portal: " + PortalMessage.AsText());

                ActorRef.Reveal(hero);

                hero.InGameClient.SendMessage(new AffixMessage()
                {
                    Id = 0x48,
                    Field0 = ActorRef.Id,
                    Field1 = 1,
                    aAffixGBIDs = new int[0]
                });

                hero.InGameClient.SendMessage(new AffixMessage()
                {
                    Id = 0x48,
                    Field0 = ActorRef.Id,
                    Field1 = 2,
                    aAffixGBIDs = new int[0]
                });

                hero.InGameClient.SendMessage(PortalMessage);

                hero.InGameClient.SendMessage(new ACDCollFlagsMessage()
                {
                    Id = 0x00A6,
                    Field0 = ActorRef.Id,
                    Field1 = 0x00000001,
                });

                hero.InGameClient.SendMessage(new AttributesSetValuesMessage()
                {
                    Id = 0x004D,
                    Field0 = ActorRef.Id,
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

                hero.InGameClient.SendMessage(new ACDGroupMessage()
                {
                    Id = 0x00B8,
                    Field0 = ActorRef.Id,
                    Field1 = -1,
                    Field2 = -1,
                });

                hero.InGameClient.SendMessage(new ANNDataMessage()
                {
                    Id = 0x003E,
                    Field0 = ActorRef.Id,
                });

                hero.InGameClient.SendMessage(new ACDTranslateFacingMessage()
                {
                    Id = 0x0070,
                    Field0 = ActorRef.Id,
                    Field1 = 0f,
                    Field2 = false,
                });

            }
            hero.InGameClient.FlushOutgoingBuffer();
        }

    }
}
