using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mooege.Net.GS.Message.Fields;
using Mooege.Core.GS.Players;
using Mooege.Core.GS.Actors.Implementations.Hirelings;

namespace Mooege.Core.GS.Actors.Interactions
{
    public interface IInteraction
    {
        NPCInteraction AsNPCInteraction(InteractiveNPC npc, Player player);
    }

    public class ConversationInteraction : IInteraction
    {
        public int ConversationSNO;
        private bool read;

        public ConversationInteraction(int conversationSNO)
        {
            ConversationSNO = conversationSNO;
            read = false; // Should read from players saved data /fasbat
        }

        public NPCInteraction AsNPCInteraction(InteractiveNPC npc, Player player)
        {
            return new NPCInteraction()
            {
                Type = NPCInteractionType.Conversation,
                ConversationSNO = this.ConversationSNO,
                Field2 = -1,
                State = (read ? NPCInteractionState.Used : NPCInteractionState.New),
            };
        }

        public void MarkAsRead() // Just a hack to show functionality /fasbat
        {
            read = true;
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
                State = (npc as Hireling).HasHireling ? NPCInteractionState.New : NPCInteractionState.Disabled
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
                State = (npc as Hireling).HasProxy ? NPCInteractionState.New : NPCInteractionState.Disabled
            };
        }
    }
   
}
