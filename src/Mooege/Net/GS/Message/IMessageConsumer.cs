﻿﻿namespace Mooege.Net.GS.Message
 {
     public interface IMessageConsumer
     {
         void Consume(GameClient client, GameMessage message);
     }

     public enum Consumers
     {
         None,
         ClientManager,
         Game,
         Inventory,
         Conversations,
         Player,
         SelectedNPC
     }
 }
