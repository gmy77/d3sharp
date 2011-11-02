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

using System.Collections.Generic;
using Mooege.Common.MPQ.FileFormats.Types;
using Mooege.Core.GS.Map;
using Mooege.Core.GS.Players;
using Mooege.Net.GS.Message;
using Mooege.Net.GS.Message.Definitions.Effect;
using Mooege.Net.GS.Message.Definitions.World;
using Mooege.Net.GS.Message.Definitions.NPC;
using Mooege.Net.GS.Message.Fields;

namespace Mooege.Core.GS.Actors.Implementations
{
    [HandledSNO(3533 /* Ho-ho-horadrim */)]
    public class Cain : InteractiveNPC
    {
        public Cain(World world, int snoId, Dictionary<int, TagMapEntry> tags)
            : base(world, snoId, tags)
        {
            this.Attributes[GameAttribute.MinimapActive] = true;
        }

        public override void OnTargeted(Player player, TargetMessage message)
        {
            player.InGameClient.SendMessage(new NPCInteractOptionsMessage()
            {
                Id = 0x8F,
                ActorID = (int)this.DynamicID,
                tNPCInteraction = new NPCInteraction[]
                {
                    new NPCInteraction()
                    {
                        Type = NPCInteractionType.Unknown0,
                        ConversationSNO = 198588,
                        Field2 = -1,   
                        State = NPCInteractionState.New, 
                    },
                    new NPCInteraction()
                    {
                        Type = NPCInteractionType.Unknown1,
                        ConversationSNO = 155050,
                        Field2 = -1,
                        State = NPCInteractionState.New,
                    },
                    new NPCInteraction()
                    {
                        Type = NPCInteractionType.Conversation2,
                        ConversationSNO = 72416,
                        Field2 = -1,
                        State = NPCInteractionState.New,
                    },
                    new NPCInteraction()
                    {
                        Type = NPCInteractionType.Conversation,
                        ConversationSNO = 198588,
                        Field2 = -1,
                        State = NPCInteractionState.Used,
                    },
                    new NPCInteraction()
                    {
                        Type = NPCInteractionType.Conversation,
                        ConversationSNO = 73171,
                        Field2 = -1,
                        State = NPCInteractionState.Disabled,
                    },
                    new NPCInteraction()
                    {
                        Type = NPCInteractionType.Unknown4,
                        ConversationSNO = 73171,
                        Field2 = -1,
                        State = NPCInteractionState.Used,
                    },
                    new NPCInteraction()
                    {
                        Type = NPCInteractionType.Craft,
                        ConversationSNO = -1,
                        Field2 = -1,
                        State = NPCInteractionState.New,
                    },
                    new NPCInteraction()
                    {
                        Type = NPCInteractionType.IdentifyAll,
                        ConversationSNO = -1,
                        Field2 = -1,
                        State = NPCInteractionState.New,
                    },
                    new NPCInteraction()
                    {
                        Type = NPCInteractionType.Hire,
                        ConversationSNO = -1,
                        Field2 = -1,
                        State = NPCInteractionState.New,
                    },
                    new NPCInteraction()
                    {
                        Type = NPCInteractionType.Inventory,
                        ConversationSNO = -1,
                        Field2 = -1,
                        State = NPCInteractionState.New,
                    },
                },
                Type = NPCInteractOptionsType.Normal,
            });
          
            // TODO: this has no effect, why is it sent?
            player.InGameClient.SendMessage(new PlayEffectMessage()
            {
                ActorId = this.DynamicID,
                Effect = Net.GS.Message.Definitions.Effect.Effect.Unknown36
            }); 
        }

        public override bool Reveal(Player player)
        {
            if (!base.Reveal(player))
                return false;

            return true;
        }
    }
}
