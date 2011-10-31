using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mooege.Net.GS.Message.Fields;
using Mooege.Core.GS.Players;

namespace Mooege.Core.GS.Actors.Interactions
{
    public interface IInteraction
    {
        NPCInteraction AsNPCInteraction(InteractiveNPC npc, Player player);
    }

    public class ConversationInteraction : IInteraction
    {
        public int ConversationSNO;

        public ConversationInteraction(int conversationSNO)
        {
            ConversationSNO = conversationSNO;
        }

        public NPCInteraction AsNPCInteraction(InteractiveNPC npc, Player player)
        {
            return new NPCInteraction()
            {
                Type = NPCInteractionType.Conversation,
                ConversationSNO = this.ConversationSNO,
                Field2 = -1,
                State = NPCInteractionState.New,
            };
        }
    }

    public class HireInteraction : IInteraction
    {
        public NPCInteraction AsNPCInteraction(InteractiveNPC npc, Player player)
        {
            return new NPCInteraction()
            {
                Type = NPCInteractionType.Hire,
                ConversationSNO = -1,
                Field2 = -1,
                State = NPCInteractionState.New,
            };
        }
    }

    public class IdentifyAllInteraction : IInteraction
    {
        public NPCInteraction AsNPCInteraction(InteractiveNPC npc, Player player)
        {
            return new NPCInteraction()
            {
                Type = NPCInteractionType.IdentifyAll,
                ConversationSNO = -1,
                Field2 = -1,
                State = NPCInteractionState.New // Has items to identify? If no disable,
            };
        }
    }

    public class CraftInteraction : IInteraction
    {
        public NPCInteraction AsNPCInteraction(InteractiveNPC npc, Player player)
        {
            return new NPCInteraction()
            {
                Type = NPCInteractionType.Craft,
                ConversationSNO = -1,
                Field2 = -1,
                State = NPCInteractionState.New,
            };
        }
    }

    public class InventoryInteraction : IInteraction
    {
        public NPCInteraction AsNPCInteraction(InteractiveNPC npc, Player player)
        {
            return new NPCInteraction()
            {
                Type = NPCInteractionType.Inventory,
                ConversationSNO = -1,
                Field2 = -1,
                State = NPCInteractionState.New,
            };
        }
    }
   
}
