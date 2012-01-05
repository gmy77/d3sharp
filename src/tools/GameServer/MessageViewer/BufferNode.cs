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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Mooege.Net.GS.Message;
using Mooege.Net.GS.Message.Definitions.ACD;
using System.Drawing;
using System.IO;
using Mooege.Net.GS.Message.Definitions.Misc;
using Mooege.Net.GS.Message.Definitions.Quest;
using Mooege.Net.GS.Message.Definitions.Tick;
using Mooege.Net.GS.Message.Definitions.Attribute;
using Mooege.Net.GS.Message.Definitions.Actor;
using Mooege.Net.GS.Message.Definitions.Player;
using Mooege.Net.GS.Message.Definitions.Game;
using Mooege.Common.MPQ;
using Mooege.Core.GS.Common.Types.SNO;
using Mooege.Net.GS.Message.Definitions.Animation;
using Mooege.Net.GS.Message.Definitions.Effect;
using Mooege.Net.GS.Message.Definitions.Scene;
using Mooege.Net.GS.Message.Definitions.Map;
using Mooege.Net.GS.Message.Definitions.Combat;
using Mooege.Net.GS.Message.Definitions.World;

namespace GameMessageViewer
{
    class BufferNode : TreeNode
    {
        public Buffer Buffer;

        private Dictionary<string, TreeNode> actors;
        private Dictionary<string, TreeNode> quests;

        public List<TreeNode> allNodes = new List<TreeNode>();
        private static Dictionary<uint, TreeNode> actorMap = new Dictionary<uint, TreeNode>();
        public readonly string clientHash;

        public static void Reset()
        {
            actorMap = new Dictionary<uint, TreeNode>();
        }

        public void ApplyFilter(Dictionary<string, bool> filter)
        {
            Nodes.Clear();

            foreach (TreeNode node in allNodes)
                if (node is MessageNode)
                {
                    if (filter[(node as MessageNode).gameMessage.GetType().Name])
                        Nodes.Add(node);
                }
                else
                    Nodes.Add(node);

        }

        public BufferNode(Dictionary<string, TreeNode> actors, Dictionary<string, TreeNode> quests, string clientHash)
        {
            this.Buffer = new Buffer(new byte[0]);
            this.actors = actors;
            this.quests = quests;
            this.clientHash = clientHash;
        }



        public bool Append(byte[] data)
        {
            Buffer.AppendData(data);
            if (allNodes.Count == 1 && allNodes[0].GetType() == typeof(ErrorNode))
                allNodes.Clear();

            bool missing = Parse();


            if (allNodes.Count > 0)
                Text = allNodes.First().Text;
            else
            {
                allNodes.Add(new ErrorNode("Contents of buffer", BitConverter.ToString(Buffer.Data, 0)));
                Text = "Packet missing data";
            }
            return missing;
        }

        static Dictionary<Type, int> gg = new Dictionary<Type, int>();

        public bool Parse()
        {
            //allNodes.Clear();

            while (Buffer.IsPacketAvailable())
            {
                int end = Buffer.Position;
                end += Buffer.ReadInt(32) * 8;
                if (end < Buffer.Position) break;

                while ((end - Buffer.Position) >= 9)
                {
                    int start = Buffer.Position;

                    #region Try parsing a message
                    try
                    {
                        GameMessage message = Buffer.ParseMessage();
                        if (message != null)
                        {
            
                            if (message is ACDEnterKnownMessage)
                            {
                                String hex = (message as ACDEnterKnownMessage).ActorID.ToString("X8");
                                //string name;
                                //SNOAliases.Aliases.TryGetValue((message as ACDEnterKnownMessage).ActorSNOId.ToString(), out name);
                                string name = SNOAliases.GetAlias((message as ACDEnterKnownMessage).ActorSNOId);
                                if (!actors.ContainsKey(hex))
                                {
                                    TreeNode actorNode = new TreeNode(hex + "  " + name);
                                    actors.Add(hex, actorNode);
                                    actorMap.Add((message as ACDEnterKnownMessage).ActorID, actorNode);
                                    actorNode.Tag = hex;
                                }
                            }

                            if (message is QuestUpdateMessage)
                            {
                                string name = SNOAliases.GetAlias((message as QuestUpdateMessage).snoQuest);
                                //SNOAliases.Aliases.TryGetValue(.ToString(), out name);
                                if (!quests.ContainsKey((message as QuestUpdateMessage).snoQuest.ToString("X8")))
                                {
                                    TreeNode questNode = new TreeNode(name);
                                    questNode.Tag = (message as QuestUpdateMessage).snoQuest.ToString("X8");
                                    quests.Add(questNode.Tag.ToString(), questNode);
                                }
                            }

                            MessageNode node = new MessageNode(message);
                            allNodes.Add(node);

                            /// THIS IS FOR QUICKER PARSING BUT IM TOO LAZY TO DO THAT FOR ALL MESSAGES
                            #region quickparse

                            // skip messages without actor or quest id
                            if (message is GameTickMessage ||
                                message is SNODataMessage || 
                                message is SNONameDataMessage || 
                                message is SimpleMessage ||
                                message is RevealSceneMessage ||
                                message is DestroySceneMessage ||
                                message is MapRevealSceneMessage 
                                )
                                continue;

                            try
                            {
                                if (gg.ContainsKey(message.GetType()))
                                    gg[message.GetType()]++;
                                else
                                    gg.Add(message.GetType(), 1);

                                if (message is ANNDataMessage)
                                {
                                    if (actorMap.ContainsKey((uint)(message as ANNDataMessage).ActorID))
                                        actorMap[(uint)(message as ANNDataMessage).ActorID].Nodes.Add(node.Clone());
                                    continue;
                                }
                                if (message is ACDTranslateFacingMessage)
                                {
                                    actorMap[(uint)(message as ACDTranslateFacingMessage).ActorId].Nodes.Add(node.Clone());
                                    continue;
                                }

                                if (message is AttributeSetValueMessage)
                                {
                                    var msg = message as AttributeSetValueMessage;
                                    actorMap[(uint)(message as AttributeSetValueMessage).ActorID].Nodes.Add(node.Clone());

                                    if (msg.Field1.Attribute != GameAttribute.Attached_To_ACD &&
                                        msg.Field1.Attribute != GameAttribute.Attachment_ACD &&
                                        msg.Field1.Attribute != GameAttribute.Banner_ACDID &&
                                        msg.Field1.Attribute != GameAttribute.Follow_Target_ACDID &&
                                        msg.Field1.Attribute != GameAttribute.Forced_Enemy_ACDID &&
                                        msg.Field1.Attribute != GameAttribute.Gizmo_Operator_ACDID &&
                                        msg.Field1.Attribute != GameAttribute.Guard_Object_ACDID &&
                                        msg.Field1.Attribute != GameAttribute.Item_Bound_To_ACD &&
                                        msg.Field1.Attribute != GameAttribute.Last_Damage_ACD &&
                                        msg.Field1.Attribute != GameAttribute.Last_ACD_Attacked &&
                                        msg.Field1.Attribute != GameAttribute.Last_ACD_Attacked_By &&
                                        msg.Field1.Attribute != GameAttribute.Attached_To_ACD &&
                                        msg.Field1.Attribute != GameAttribute.Last_Blocked_ACD &&
                                        msg.Field1.Attribute != GameAttribute.Loading_Player_ACD &&
                                        msg.Field1.Attribute != GameAttribute.RootTargetACD &&
                                        msg.Field1.Attribute != GameAttribute.Script_Target_ACDID &&
                                        msg.Field1.Attribute != GameAttribute.Spawned_by_ACDID &&
                                        msg.Field1.Attribute != GameAttribute.Summoned_By_ACDID &&
                                        msg.Field1.Attribute != GameAttribute.Taunt_Target_ACD &&
                                        msg.Field1.Attribute != GameAttribute.Wizard_Slowtime_Proxy_ACD)
                                        //actorMap[(uint)(message as AttributeSetValueMessage).Field1.Int].Nodes.Add(node.Clone());
                                        continue;
                                }

                                if (message is ACDTranslateNormalMessage)
                                {
                                    actorMap[(uint)(message as ACDTranslateNormalMessage).ActorId].Nodes.Add(node.Clone());
                                    continue;
                                }
                                if (message is ACDClientTranslateMessage)
                                {
                                    // If the client sends a PlayerMoveMessage, but the actor is not yet in the
                                    // actor list, its due to a broken cap that starts too late...add the node for speed (and grouping)
                                    // ACDClientTranslateMessage does not contain ActorId, we'll just make one up for now
                                    if (!actorMap.ContainsKey(0x1F1F1F1F))
                                    {
                                        String hex = 0x1F1F1F1F.ToString("X8");

                                        if (!actors.ContainsKey(hex))
                                        {
                                            TreeNode actorNode = new TreeNode(hex + " Capper");
                                            actorMap.Add(0x1F1F1F1F, actorNode);
                                            actorNode.Tag = hex;
                                            actors.Add(hex, actorNode);
                                        }
                                    }

                                    actorMap[0x1F1F1F1F].Nodes.Add(node.Clone());
                                    continue;
                                }

                                if (message is ACDGroupMessage)
                                {
                                    actorMap[(uint)(message as ACDGroupMessage).ActorID].Nodes.Add(node.Clone());
                                    continue;
                                }
                                if (message is ACDCollFlagsMessage)
                                {
                                    actorMap[(uint)(message as ACDCollFlagsMessage).ActorID].Nodes.Add(node.Clone());
                                    continue;
                                }
                                if (message is AffixMessage)
                                {
                                    actorMap[(uint)(message as AffixMessage).ActorID].Nodes.Add(node.Clone());
                                    continue;
                                }
                                if (message is TrickleMessage)
                                {
                                    if (actorMap.ContainsKey((uint)(message as TrickleMessage).ActorId))
                                        actorMap[(uint)(message as TrickleMessage).ActorId].Nodes.Add(node.Clone());
                                    continue;
                                }
                                if (message is ACDEnterKnownMessage)
                                {
                                    actorMap[(uint)(message as ACDEnterKnownMessage).ActorID].Nodes.Add(node.Clone());
                                    continue;
                                }
                                if (message is ACDCreateActorMessage)
                                {
                                    actorMap[(uint)(message as ACDCreateActorMessage).ActorId].Nodes.Add(node.Clone());
                                    continue;
                                }
                                if (message is ACDDestroyActorMessage)
                                {
                                    actorMap[(uint)(message as ACDDestroyActorMessage).ActorId].Nodes.Add(node.Clone());
                                    continue;
                                }
                                if (message is ACDTranslateFixedMessage)
                                {
                                    actorMap[(uint)(message as ACDTranslateFixedMessage).ActorId].Nodes.Add(node.Clone());
                                    continue;
                                }
                                if (message is AttributesSetValuesMessage)
                                {
                                    actorMap[(uint)(message as AttributesSetValuesMessage).ActorID].Nodes.Add(node.Clone());
                                    continue;
                                }
                                if (message is SetIdleAnimationMessage)
                                {
                                    actorMap[(uint)(message as SetIdleAnimationMessage).ActorID].Nodes.Add(node.Clone());
                                    continue;
                                }
                                if (message is ACDWorldPositionMessage)
                                {
                                    actorMap[(uint)(message as ACDWorldPositionMessage).ActorID].Nodes.Add(node.Clone());
                                    continue;
                                }
                                if (message is PlayEffectMessage)
                                {
                                    actorMap[(uint)(message as PlayEffectMessage).ActorId].Nodes.Add(node.Clone());
                                    continue;
                                }
                                if (message is PlayAnimationMessage)
                                {
                                    actorMap[(uint)(message as PlayAnimationMessage).ActorID].Nodes.Add(node.Clone());
                                    continue;
                                }
                                if (message is FloatingNumberMessage)
                                {
                                    actorMap[(uint)(message as FloatingNumberMessage).ActorID].Nodes.Add(node.Clone());
                                    continue;
                                }
                                if (message is ACDTranslateSyncMessage)
                                {
                                    actorMap[(uint)(message as ACDTranslateSyncMessage).Field0].Nodes.Add(node.Clone());
                                    continue;
                                }
                                if (message is ACDTranslateDetPathMessage)
                                {
                                    actorMap[(uint)(message as ACDTranslateDetPathMessage).Field0].Nodes.Add(node.Clone());
                                    continue;
                                }
                                if (message is PlayHitEffectMessage)
                                {
                                    actorMap[(uint)(message as PlayHitEffectMessage).ActorID].Nodes.Add(node.Clone());
                                    continue;
                                }
                                if (message is AimTargetMessage)
                                {
                                    actorMap[(uint)(message as AimTargetMessage).Field0].Nodes.Add(node.Clone());
                                    if((message as AimTargetMessage).Field2 != -1)
                                        actorMap[(uint)(message as AimTargetMessage).Field2].Nodes.Add(node.Clone()); 
                                    continue;
                                }
                                if (message is TargetMessage)
                                {
                                    if((message as TargetMessage).TargetID != 0xFFFFFFFF)
                                        actorMap[(uint)(message as TargetMessage).TargetID].Nodes.Add(node.Clone());
                                    continue;
                                }

                            }
                            catch (Exception) {}

                            #endregion

                            // Bruteforce find actor ID and add to actor tree
                            string text = message.AsText();
                            foreach (TreeNode an in actors.Values)
                                if (text.Contains((string)an.Tag))
                                {
                                    MessageNode nodeb = node.Clone();
                                    nodeb.BackColor = this.BackColor;
                                    an.Nodes.Add(nodeb);
                                }

                            // Bruteforce find quest SNO and add to quest tree
                            foreach (TreeNode qn in quests.Values)
                                if (text.Contains(qn.Tag.ToString()))
                                {
                                    MessageNode nodeb = node.Clone();
                                    nodeb.BackColor = this.BackColor;
                                    qn.Nodes.Add(nodeb);
                                }
                        }
                        else
                        {
                            int pos = Buffer.Position;
                            Buffer.Position = start;
                            ErrorNode errorNode = new ErrorNode(String.Format("No message handler found for message id {0}", (Opcodes)Buffer.ReadInt(9)), "");
                            errorNode.BackColor = Color.Pink;
                            allNodes.Add(errorNode);
                            Buffer.Position = pos;
                        }
                    }

                    catch (Exception e)
                    {
                        int pos = Buffer.Position;
                        Buffer.Position = start;
                        ErrorNode errorNode = new ErrorNode(String.Format("Error parsing messsage {0}", (Opcodes)Buffer.ReadInt(9)), e.Message);
                        errorNode.BackColor = Color.Pink;
                        allNodes.Add(errorNode);
                        Buffer.Position = pos;
                    }
                    #endregion
                }

                Buffer.Position = end;
            }

            return Buffer.Position != Buffer.Length;
        }
    }
}
