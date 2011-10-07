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
using System.Reflection;
using System.Text;
using Mooege.Common;

namespace Mooege.Core.MooNet.Commands
{
    public static class CommandManager
    {
        private static readonly Logger Logger = LogManager.CreateLogger();
        private static readonly Dictionary<ServerCommandAttribute, MethodInfo> ServerCommands = new Dictionary<ServerCommandAttribute, MethodInfo>();

        static CommandManager()
        {
            LoadCommands();
        }

        private static void LoadCommands()
        {
            foreach(var type in Assembly.GetEntryAssembly().GetTypes())
            {
                var methods = type.GetMethods(BindingFlags.Static | BindingFlags.Public);
                foreach(var method in methods)
                {
                    object[] attributes = method.GetCustomAttributes(typeof(ServerCommandAttribute), true); // get the attributes of the packet.
                    if (attributes.Length == 0 ) continue;

                    var attribute = (ServerCommandAttribute) attributes[0];
                    ServerCommands.Add(attribute, method);
                }
            }
        }

        public static void Parse(string line)
        {
            line = line.Trim();
            if (line == String.Empty) return;
            string command = line.Split(' ')[0].ToLower();
            string parameters = String.Empty;
            
            if(line.Contains(' ')) parameters = line.Substring(line.IndexOf(' ') + 1).ToLower();

            foreach (var cmd in ServerCommands.Where(cmd => cmd.Key.Command == command))
            {
                cmd.Value.Invoke(null, new object[] {parameters});
                return;
            }

            Logger.Info("Unknown command: " + command);
        }
    }
}
