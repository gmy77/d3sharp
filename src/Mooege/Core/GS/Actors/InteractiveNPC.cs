/*
 * Copyright (C) 2011 mooege project
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */

using System;
using System.Linq;
using System.Collections.Generic;
using Mooege.Common.MPQ.FileFormats.Types;
using Mooege.Core.GS.Map;
using Mooege.Core.GS.Players;
using Mooege.Net.GS.Message;
using Mooege.Net.GS.Message.Definitions.World;
using Mooege.Core.GS.Actors.Interactions;
using Mooege.Net.GS.Message.Fields;
using Mooege.Net.GS.Message.Definitions.NPC;
using Mooege.Net.GS;
using Mooege.Net.GS.Message.Definitions.Hireling;

namespace Mooege.Core.GS.Actors
{
    public class InteractiveNPC : NPC, IMessageConsumer
    {
        public List<IInteraction> Interactions { get; private set; }
        public List<ConversationInteraction> Conversations { get; private set; }
        
        public InteractiveNPC(World world, int snoId, Dictionary<int, TagMapEntry> tags)
            : base(world, snoId, tags)
        {
            this.Attributes[GameAttribute.NPC_Has_Interact_Options, 0] = true;
            this.Attributes[GameAttribute.NPC_Is_Operatable] = true;
            this.Attributes[GameAttribute.Buff_Visual_Effect, 0x00FFFFF] = true;
            Interactions = new List<IInteraction>();
            Conversations = new List<ConversationInteraction>();
        }

        public override void Update(int tickCounter)
        {
            if (tickCounter < 500) return;
            base.Update(tickCounter);

            if (conversationList != null)
            {
                Conversations = new List<ConversationInteraction>();
                foreach (var entry in conversationList.ConversationListEntries)
                {
                //   if (Mooege.Common.MPQ.MPQStorage.Data.Assets[Common.Types.SNO.SNOGroup.QuestRange].ContainsKey(entry.SNOQuestRange))
                //        if (World.Game.Quests.IsInQuestRange(Mooege.Common.MPQ.MPQStorage.Data.Assets[Common.Types.SNO.SNOGroup.QuestRange][entry.SNOQuestRange].Data as Mooege.Common.MPQ.FileFormats.QuestRange))
                          
                    
                    Conversations.Add(new ConversationInteraction(entry.SNOConv));
                    if(Mooege.Common.MPQ.MPQStorage.Data.Assets[Common.Types.SNO.SNOGroup.Conversation].ContainsKey(entry.SNOConv))
                            if ((Mooege.Common.MPQ.MPQStorage.Data.Assets[Common.Types.SNO.SNOGroup.Conversation][entry.SNOConv].Data as Mooege.Common.MPQ.FileFormats.Conversation).I0 == 1)
                            {
                                Attributes[GameAttribute.Conversation_Icon, 0] = 1;
                                //Attributes[GameAttribute.Buff_Visual_Effect, 0x000FFFFF] = true;
                                foreach (var message in Attributes.GetChangedMessageList(this.DynamicID))
                                    World.BroadcastIfRevealed(message, this);
                                //World.BroadcastIfRevealed(new Mooege.Net.GS.Message.Definitions.Misc.ANNDataMessage(Opcodes.ANNDataMessage10) { Id = 0x6d, ActorID = this.DynamicID }, this);
                                Attributes.ClearChanged();
                            }

                //    if (World.Game.Quests.IsInQuestRange(entry.SNOQuestCurrent, entry.I3))
                //        Conversations.Add(new ConversationInteraction(entry.SNOConv));
                }
            }


        }

        public override void OnTargeted(Player player, TargetMessage message)
        {
            player.SelectedNPC = this;

            var count = Interactions.Count + Conversations.Count;
            if (count == 0)
                return;

            NPCInteraction[] npcInters = new NPCInteraction[count];

            var it = 0;
            foreach(var conv in Conversations)
            {
                npcInters[it] = conv.AsNPCInteraction(this, player);
                it++;
            }

            foreach(var inter in Interactions)
            {
                npcInters[it] = inter.AsNPCInteraction(this, player);
                it++;
            }

            player.InGameClient.SendMessage(new NPCInteractOptionsMessage()
            {
                ActorID = this.DynamicID,
                tNPCInteraction = npcInters,
                Type = NPCInteractOptionsType.Normal             
            });

            // TODO: this has no effect, why is it sent?
            player.InGameClient.SendMessage(new Mooege.Net.GS.Message.Definitions.Effect.PlayEffectMessage()
            {
                ActorId = this.DynamicID,
                Effect = Net.GS.Message.Definitions.Effect.Effect.Unknown36
            }); 
        }

        public void Consume(GameClient client, GameMessage message)
        {
            if (message is NPCSelectConversationMessage) OnSelectConversation(client.Player, message as NPCSelectConversationMessage);
            if (message is HirelingHireMessage) OnHire(client.Player);
            else return;
        }

        public virtual void OnHire(Player player)
        {
            throw new NotImplementedException();
        }

        private void OnSelectConversation(Player player, NPCSelectConversationMessage message)
        {
            var conversation = Conversations.FirstOrDefault(conv => conv.ConversationSNO == message.ConversationSNO);
            if (conversation == null) 
                return;

            player.Conversations.StartConversation(conversation.ConversationSNO);
            conversation.MarkAsRead();
        }
    }
}