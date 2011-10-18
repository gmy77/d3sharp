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
        private static readonly Dictionary<CommandAttribute, Command> Commands = new Dictionary<CommandAttribute, Command>();

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

                var commandAttribute = attributes[0];
                if (!Commands.ContainsKey(commandAttribute))
                    Commands.Add(commandAttribute, (Command)Activator.CreateInstance(type));
                else
                    Logger.Warn("There exists an already registered command '{0}'.", commandAttribute.Command);
            }
        }

        /// <summary>
        /// Parses a read line from console.
        /// </summary>
        /// <param name="line">The line to be parsed.</param>
        /// <param name="client">The invoker client if any.</param>
        /// <param name="responseMedium">The response medium to command</param>
        public static bool TryParse(string line, MooNetClient client=null, ResponseMediums responseMedium = ResponseMediums.Console)
        {
            var output = string.Empty;
            line = line.Trim().ToLower();

            if (line == String.Empty) // just return false, if message is empty.
                return false;

            if (line[0] != Config.Instance.CommandPrefix) // if line does not start with command-prefix
            {
                output = "Unknown command: " + line;
                if(client==null && responseMedium== ResponseMediums.Console) Logger.Info(output); // only output 'unknown command' message to console.
                    return false;
            }

            line = line.Substring(1); // advance to actual command.
            var command = line.Split(' ')[0]; // get command
            var parameters = String.Empty; 
            if (line.Contains(' ')) parameters = line.Substring(line.IndexOf(' ') + 1).ToLower().Trim(); // get parameters if any.

            foreach(var pair in Commands) 
            {
                if (pair.Key.Command != command) continue;
                InvokeCommand(pair.Key, pair.Value, parameters, client, responseMedium);
                return true;
            }

            // if execution reaches here, it means command was not found.
            output = "Unknown command: " + line;

            // if invoked from console
            if (client == null)
            {
                Logger.Info(output);
                return true;
            }

            // if invoked by a client
            if (responseMedium == ResponseMediums.Channel)
                client.SendMessage(output);
            else if (responseMedium == ResponseMediums.Whisper)
                client.SendWhisper(output);
            
            return true;
        }

        private static void InvokeCommand(CommandAttribute commandAttribute, Command command, string parameters, MooNetClient client = null, ResponseMediums responseMedium = ResponseMediums.Console)
        {
            var output = string.Empty;

            if (client != null && commandAttribute.ConsoleOnly)
                output = "You don't have enough privileges to run this command.";
            else
                output = command.Invoke(parameters, client); // invoke the command

            // if invoked from console
            if (client == null)
            {
                Logger.Info(output);
                return;
            }

            // if invoked by a client
            if (responseMedium == ResponseMediums.Channel)
                client.SendMessage(output);
            else if (responseMedium == ResponseMediums.Whisper)
                client.SendWhisper(output);
        }

        // embedded commands

        [Command("commands")]
        public class CommandsCommand : Command
        {            
            public override string Invoke(string parameters, MooNetClient invokerClient = null)
            {
                var output = "Available commands: ";
                output += Commands.Aggregate(string.Empty, (current, pair) => current + (pair.Key.Command + ", "));
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
                    if (pair.Key.Command != parameters) continue;

                    var output = pair.Value.Help();
                    return output != string.Empty ? output : "Help text not found for command: " + parameters;
                }

                return "Unknown command: " + parameters;
            }
        }

        /// <summary>
        /// Response mediums.
        /// </summary>
        public enum ResponseMediums
        {
            Console,
            Channel,
            Whisper
        }
    }
}
