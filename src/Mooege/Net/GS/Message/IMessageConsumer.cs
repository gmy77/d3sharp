﻿namespace Mooege.Net.GS.Message
 {
     public interface IMessageConsumer
     {
         void Consume(GameClient client, GameMessage message);
     }

     public enum Consumers
     {
         None,
         Universe,
         PlayerManager,
         Inventory,
         Hero
     }
 }
