﻿/*
 * Copyright (C) 2011 - 2012 mooege project - http://www.mooege.org
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
using System.Reflection;
using System.Text;
using Mooege.Common.Logging;

namespace Mooege.Net.GS.Message
{
    public abstract class GameMessage
    {
        protected static readonly Logger Logger = LogManager.CreateLogger();

        private static readonly Dictionary<Opcodes, Type> MessageTypes = new Dictionary<Opcodes, Type>();
        private static readonly Dictionary<Opcodes, Consumers> MessageConsumers = new Dictionary<Opcodes, Consumers>();

        static GameMessage()
        {
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (!type.IsSubclassOf(typeof(GameMessage))) continue;

                var attributes = (MessageAttribute[])type.GetCustomAttributes(typeof(MessageAttribute), true);
                if (attributes.Length == 0) continue;
                foreach (MessageAttribute attribute in attributes)
                {
                    foreach (var opcode in attribute.Opcodes)
                    {
                        if (MessageTypes.ContainsKey(opcode))
                        {
                            Logger.Fatal("Duplicate opcode detected: {0}", opcode.ToString());
                        }
                        else
                        {
                            MessageTypes.Add(opcode, type);
                            MessageConsumers.Add(opcode, attribute.Consumer);
                        }
                    }
                }
            }
        }

        public static T Allocate<T>(Opcodes opcode) where T : GameMessage
        {
            if (!MessageTypes.ContainsKey(opcode))
            {
                Logger.Debug("Unimplemented message: " + opcode.ToString());
                return null;
            }

            var ctorWithParameterExists = MessageTypes[opcode].GetConstructor(new[] { typeof(Opcodes) }) != null;
            var msg = (T)Activator.CreateInstance(MessageTypes[opcode], ctorWithParameterExists ? new object[] { opcode } : null);

            msg.Id = (int)opcode;
            msg.Consumer = MessageConsumers[opcode];
            return msg;
        }

        public static GameMessage ParseMessage(GameBitBuffer buffer)
        {
            int id = buffer.ReadInt(9);
            var msg = Allocate<GameMessage>((Opcodes)id);
            if (msg == null) return null;

            msg.Id = id;
            msg.Parse(buffer);
            return msg;
        }

        protected GameMessage() { }

        protected GameMessage(int id)
        {
            this.Id = id;
        }

        protected GameMessage(Opcodes opcode)
        {
            this.Id = (int)opcode;
        }

        public int Id { get; set; } // this is needed for farmys dumper /fasbat
        public Consumers Consumer { get; set; }

        public abstract void Parse(GameBitBuffer buffer);
        public abstract void Encode(GameBitBuffer buffer);
        public abstract void AsText(StringBuilder b, int pad);

        public string AsText()
        {
            var builder = new StringBuilder();
            builder.AppendLine("GameMessage(0x" + Id.ToString("X4") + ")");
            AsText(builder, 0);
            return builder.ToString();
        }
    }
}
