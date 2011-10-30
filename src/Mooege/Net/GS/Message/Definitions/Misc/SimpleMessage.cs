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
using System.Text;
using Mooege.Core.GS.Common.Types.Math;
using Mooege.Net.GS.Message.Definitions.ACD;
using Mooege.Net.GS.Message.Definitions.Hireling;
using Mooege.Net.GS.Message.Fields;

namespace Mooege.Net.GS.Message.Definitions.Misc
{
    [Message(new[]{
        Opcodes.SimpleMessage1, Opcodes.SimpleMessage2, Opcodes.SimpleMessage3, Opcodes.SimpleMessage5, Opcodes.SimpleMessage6, Opcodes.SimpleMessage7, Opcodes.SimpleMessage8,
        Opcodes.SimpleMessage9, Opcodes.SimpleMessage10, Opcodes.SimpleMessage11, Opcodes.SimpleMessage13, Opcodes.SimpleMessage14, Opcodes.SimpleMessage15, Opcodes.SimpleMessage16, 
        Opcodes.SimpleMessage18, Opcodes.SimpleMessage19, Opcodes.SimpleMessage20, Opcodes.SimpleMessage21, Opcodes.SimpleMessage22, Opcodes.SimpleMessage23, Opcodes.SimpleMessage24, 
        Opcodes.SimpleMessage25, Opcodes.SimpleMessage26, Opcodes.SimpleMessage27, Opcodes.SimpleMessage28, Opcodes.SimpleMessage29, Opcodes.SimpleMessage30, Opcodes.SimpleMessage31,
        Opcodes.SimpleMessage32, Opcodes.SimpleMessage33, Opcodes.SimpleMessage34, Opcodes.SimpleMessage35, Opcodes.SimpleMessage36, Opcodes.SimpleMessage37, Opcodes.SimpleMessage38, 
        Opcodes.SimpleMessage39, Opcodes.SimpleMessage40, Opcodes.SimpleMessage41, Opcodes.SimpleMessage42, Opcodes.SimpleMessage43, Opcodes.SimpleMessage44, Opcodes.SimpleMessage45, 
        Opcodes.SimpleMessage46})]
    public class SimpleMessage : GameMessage, ISelfHandler
    {
        public void Handle(GameClient client)
        {
            var player = client.Player;
            switch (this.Id)
            {
                case 0x0030: // Sent with DwordDataMessage(0x0125, Value:0) and SimpleMessage(0x0125)
                    {
                        // What the dickens is this stuff
                        #region hardcoded1
                        #region HirelingInfo
                        client.SendMessage(new HirelingInfoUpdateMessage()
                        {
                            Id = 0x009D,
                            Field0 = 0x00000001,
                            Field1 = false,
                            Field2 = -1,
                            Field3 = 0x00000000,
                        });

                        client.SendMessage(new HirelingInfoUpdateMessage()
                        {
                            Id = 0x009D,
                            Field0 = 0x00000002,
                            Field1 = false,
                            Field2 = -1,
                            Field3 = 0x00000000,
                        });

                        client.SendMessage(new HirelingInfoUpdateMessage()
                        {
                            Id = 0x009D,
                            Field0 = 0x00000003,
                            Field1 = false,
                            Field2 = -1,
                            Field3 = 0x00000000,
                        });
                        #endregion
                        #region Player Attribute Values

                        GameAttributeMap attributes = new GameAttributeMap();
                        attributes[GameAttribute.Banter_Cooldown, 0xFFFFF] = 0x000007C9;
                        attributes[GameAttribute.Buff_Active, 0x20CBE] = true;
                        attributes[GameAttribute.Buff_Active, 0x33C40] = false;
                        attributes[GameAttribute.Immobolize] = false;
                        attributes[GameAttribute.Untargetable] = false;
                        attributes[GameAttribute.CantStartDisplayedPowers] = false;
                        attributes[GameAttribute.Buff_Icon_Start_Tick0, 0x20CBE] = 0xC1;
                        attributes[GameAttribute.Disabled] = false;
                        attributes[GameAttribute.Hidden] = false;
                        attributes[GameAttribute.Buff_Icon_Count0, 0x33C40] = 0;
                        attributes[GameAttribute.Buff_Icon_End_Tick0, 0x20CBE] = 0x7C9;
                        attributes[GameAttribute.Loading] = false;
                        attributes[GameAttribute.Buff_Icon_End_Tick0, 0x33C40] = 0;
                        attributes[GameAttribute.Invulnerable] = false;
                        attributes[GameAttribute.Buff_Icon_Count0, 0x20CBE] = 1;
                        attributes[GameAttribute.Buff_Icon_Start_Tick0, 0x33C40] = 0;
                        attributes.SendMessage(client, player.DynamicID);

                        #endregion

                        client.SendMessage(new ACDCollFlagsMessage()
                        {
                            ActorID = player.DynamicID,
                            CollFlags = 0x00000008,
                        });

                        client.SendMessage(new DWordDataMessage()
                        {
                            Id = 0x0089,
                            Field0 = 0x000000C1,
                        });
                        #endregion
                        
                        #region hardcoded2
                        // NOTE: This is very similar to ACDEnterKnown fields
                        // TODO: Map proper values from the actor..
                        client.SendMessage(new TrickleMessage()
                        {
                            ActorId = player.DynamicID,
                            ActorSNO = client.Player.ClassSNO,
                            WorldLocation = new WorldPlace()
                            {
                                Position = new Vector3D()
                                {
                                    X = 3143.75f,
                                    Y = 2828.75f,
                                    Z = 59.07559f,
                                },
                                WorldID = client.Player.World.DynamicID,
                            },
                            PlayerIndex = 0x00000000,
                            LevelAreaSNO = 0x00026186,
                            Field5 = 1f,
                            Field6 = 0x00000001,
                            Field7 = 0x00000024,
                            Field10 = unchecked((int)0x8DFA5D13),
                            StringListSNO = 0x0000F063,
                        });

                        client.SendMessage(new DWordDataMessage()
                        {
                            Id = 0x0089,
                            Field0 = 0x000000D1,
                        });
                        #endregion
                        
                    }
                    break;
                case 0x0028: // Logout complete (sent when delay timer expires on client side)
                    //if (client.IsLoggingOut)
                    //{
                    //    client.SendMessageNow(new QuitGameMessage()
                    //    {
                    //        Id = 0x0003,
                    //        // Field0 - quit reason?
                    //        // 0 - logout
                    //        // 1 - kicked by party leader
                    //        // 2 - disconnected due to client-server (version?) missmatch
                    //        PlayerIndex = 0,
                    //    });
                    //}
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public override void Parse(GameBitBuffer buffer)
        {
        }

        public override void Encode(GameBitBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("SimpleMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }
    }
}
