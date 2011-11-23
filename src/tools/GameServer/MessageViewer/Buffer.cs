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
using Mooege.Net.GS.Message;
using System.Reflection;
using System.Runtime.Serialization;
using Mooege.Net.GS.Message.Fields;
using Mooege.Net.GS.Message.Definitions.Misc;
using Mooege.Net.GS.Message.Definitions.Skill;

namespace GameMessageViewer
{

    /// <summary>
    /// Proxy to the GameBitBuffer that does not rely on IncomingMessage - Attribute to parse messages
    /// </summary>
    class GameMessageProxy
    {
        private static readonly Dictionary<Opcodes, Type> MessageTypes = new Dictionary<Opcodes, Type>();

        // Create a dictionary of all GameMessages and what opcodes they handle
        // for all Subclasses of GameMessage defined anywhere in the AppDomain
        static GameMessageProxy()
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                foreach (Type type in assembly.GetTypes())
                    if (type.IsSubclassOf(typeof(GameMessage)) || type == typeof(HeroStateData))
                    {
                        var attributes = (MessageAttribute[])type.GetCustomAttributes(typeof(MessageAttribute), true);
                        if (attributes.Length == 0) continue;
                        foreach (MessageAttribute attribute in attributes)
                            foreach (var opcode in attribute.Opcodes)
                                MessageTypes.Add(opcode, type);
                    }

            foreach(Opcodes opcode in Enum.GetValues(typeof(Opcodes)))
                if(!MessageTypes.ContainsKey(opcode))
                    foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                        foreach (Type type in assembly.GetTypes())
                            if (type.IsSubclassOf(typeof(GameMessage)))
                                if(opcode.ToString().StartsWith(type.Name))
                                    MessageTypes.Add(opcode, type);


        }

        // Create and parse the GameMessage that handles the next opcode
        public static GameMessage ParseMessage(GameBitBuffer buffer)
        {
            GameMessage msg = null;

            Opcodes opcode = (Opcodes)buffer.ReadInt(9);
            if (MessageTypes.ContainsKey(opcode))
            {
                msg = (GameMessage)FormatterServices.GetUninitializedObject(MessageTypes[opcode]);
                typeof(GameMessage).GetProperty("Id").SetValue(msg, (int)opcode, null);
                msg.Parse(buffer);
            }

            return msg as GameMessage;
        }
    }

    /// <summary>
    /// Proxys a buffer, so messages created when parsing the buffer dont use the Incoming-attribute
    /// </summary>
    class Buffer : GameBitBuffer
    {
        public Buffer(byte[] data) : base(data) { }

        public new GameMessage ParseMessage()
        {
            return GameMessageProxy.ParseMessage(this);
        }

    }
}
