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
using Mooege.Common;
using Mooege.Net.MooNet;

namespace Mooege.Core.MooNet.Commands
{
    public static class CommandManager
    {
        private static readonly Logger Logger = LogManager.CreateLogger();
        private static readonly Dictionary<string, Command> Commands = new Dictionary<string, Command>();

        static CommandManager()
        {
            RegisterCommands();
        }

        private static void RegisterCommands()
        {
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (!type.IsSubclassOf(typeof(Command))) continue;

                var attributes = (CommandAttribute[])type.GetCustomAttributes(typeof(CommandAttribute), true);
                if (attributes.Length == 0) continue;

                var command = attributes[0].Command;
                if (!Commands.ContainsKey(command))
                    Commands.Add(command, (Command)Activator.CreateInstance(type));
                else
                    Logger.Warn("There exists an already registered command '{0}'.", command);
            }
        }

        /// <summary>
        /// Parses a read line from console.
        /// </summary>
        /// <param name="line"></param>
        public static bool TryParse(string line, MooNetClient client=null)
        {
            var output = string.Empty;
            line = line.Trim().ToLower();

            if (line == String.Empty) // just return false, if message is empty.
                return false;

            if (line[0] != '!') // if line does not start with '!'
            {
                output = "Unknown command: " + line;
                if(client==null) Logger.Info(output); // only output 'unknown command' message to console.
                    return false;
            }

            line = line.Substring(1); // advance to actual command.
            var command = line.Split(' ')[0]; // get command
            var parameters = String.Empty; 
            if (line.Contains(' ')) parameters = line.Substring(line.IndexOf(' ') + 1).ToLower().Trim(); // get parameters if any.

            foreach(var pair in Commands) 
            {
                if (pair.Key != command) continue;

                output = pair.Value.Invoke(parameters); // invoke the command
                if (output.Trim() == string.Empty) return false; // if command outputs an empty message, just ignore it.

                // send the output to invoker
                if (client == null) // if it's invoked from console
                    Logger.Info(output); // output to console
                else // if invoked by a client
                    { } // send to client.

                return true;
            }

            // if execution reaches here, it means command was not found.
            output = "Unknown command: " + line;
            if (client == null)
                Logger.Info(output);
            else
                { }

            return false;
        }

        // embedded commands

        [Command("commands")]
        public class CommandsCommand : Command
        {            
            public override string Invoke(string parameters, MooNetClient invokerClient = null)
            {
                var output = "Available commands: ";
                output += Commands.Aggregate(string.Empty, (current, pair) => current + (pair.Key + ", "));
                output += "\nType 'help <command>' to get help.";
                return output;
            }
        }

        [Command("help")]
        public class HelpCommand : Command
        {
            public override string Invoke(string parameters, MooNetClient invokerClient = null)
            {
                if(parameters == string.Empty)
                {
                    return "usage: help <command>";
                }

                foreach (var pair in CommandManager.Commands)
                {
                    if (pair.Key != parameters) continue;

                    var output = pair.Value.Help();
                    return output != string.Empty ? output : "Help text not found for command: " + parameters;
                }

                return "Unknown command: " + parameters;
            }
        }
    }
}
