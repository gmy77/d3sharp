/*
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
using System.Linq;
using System.Reflection;
using Mooege.Common.Logging;
using Mooege.Net.MooNet;

namespace Mooege.Core.MooNet.Commands
{
    public static class CommandManager
    {
        private static readonly Logger Logger = LogManager.CreateLogger();
        private static readonly Dictionary<CommandGroupAttribute, CommandGroup> CommandGroups = new Dictionary<CommandGroupAttribute, CommandGroup>();

        static CommandManager()
        {
            RegisterCommandGroups();
        }

        private static void RegisterCommandGroups()
        {
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (!type.IsSubclassOf(typeof(CommandGroup))) continue;

                var attributes = (CommandGroupAttribute[])type.GetCustomAttributes(typeof(CommandGroupAttribute), true);
                if (attributes.Length == 0) continue;

                var groupAttribute = attributes[0];
                if (CommandGroups.ContainsKey(groupAttribute))
                    Logger.Warn("There exists an already registered command group named '{0}'.", groupAttribute.Name);

                var commandGroup = (CommandGroup)Activator.CreateInstance(type);
                commandGroup.Register(groupAttribute);
                CommandGroups.Add(groupAttribute, commandGroup);
            }
        }

        /// <summary>
        /// Parses a given line from console as a command if any.
        /// </summary>
        /// <param name="line">The line to be parsed.</param>
        public static void Parse(string line)
        {
            string output = string.Empty;
            string command;
            string parameters;
            var found = false;

            if (line == null) return;
            if (line.Trim() == string.Empty) return;

            if (!ExtractCommandAndParameters(line, out command, out parameters))
            {
                output = "Unknown command: " + line;
                Logger.Info(output);
                return;
            }

            foreach (var pair in CommandGroups)
            {
                if (pair.Key.Name != command) continue;
                output = pair.Value.Handle(parameters);
                found = true;
                break;
            }

            if (found == false)
                output = string.Format("Unknown command: {0} {1}", command, parameters);

            if (output != string.Empty)
                Logger.Info(output);
        }


        /// <summary>
        /// Tries to parse given line as a server command.
        /// </summary>
        /// <param name="line">The line to be parsed.</param>
        /// <param name="invokerClient">The invoker client if any.</param>
        /// <returns><see cref="bool"/></returns>
        public static bool TryParse(string line, MooNetClient invokerClient)
        {
            string output = string.Empty;
            string command;
            string parameters;
            var found = false;

            if (invokerClient == null)
                throw new ArgumentException("invokerClient");

            if (!ExtractCommandAndParameters(line, out command, out parameters))
                return false;

            foreach (var pair in CommandGroups)
            {
                if (pair.Key.Name != command) continue;
                output = pair.Value.Handle(parameters, invokerClient);
                found = true;
                break;
            }

            if (found == false)
                output = string.Format("Unknown command: {0} {1}", command, parameters);

            if (output == string.Empty)
                return true;

            output = "[mooege] " + output;

            invokerClient.SendServerWhisper(output);

            return true;
        }

        public static bool ExtractCommandAndParameters(string line, out string command, out string parameters)
        {
            line = line.Trim();
            command = string.Empty;
            parameters = string.Empty;

            if (line == string.Empty)
                return false;

            if (line[0] != Config.Instance.CommandPrefix) // if line does not start with command-prefix
                return false;

            line = line.Substring(1); // advance to actual command.
            command = line.Split(' ')[0].ToLower(); // get command
            parameters = String.Empty;
            if (line.Contains(' ')) parameters = line.Substring(line.IndexOf(' ') + 1).Trim(); // get parameters if any.

            return true;
        }

        [CommandGroup("commands", "Lists available commands for your user-level.")]
        public class CommandsCommandGroup : CommandGroup
        {
            public override string Fallback(string[] parameters = null, MooNetClient invokerClient = null)
            {
                var output = "Available commands: ";
                foreach (var pair in CommandGroups)
                {
                    if (invokerClient != null && pair.Key.MinUserLevel > invokerClient.Account.UserLevel) continue;
                    output += pair.Key.Name + ", ";
                }

                output = output.Substring(0, output.Length - 2) + ".";
                return output + "\nType 'help <command>' to get help.";
            }
        }

        [CommandGroup("help", "Oh no, we forgot to add a help to text to help command itself!")]
        public class HelpCommandGroup : CommandGroup
        {
            public override string Fallback(string[] parameters = null, MooNetClient invokerClient = null)
            {
                return "usage: help <command>";
            }

            public override string Handle(string parameters, MooNetClient invokerClient = null)
            {
                if (parameters == string.Empty)
                    return this.Fallback();

                string output = string.Empty;
                bool found = false;
                var @params = parameters.Split(' ');
                var group = @params[0];
                var command = @params.Count() > 1 ? @params[1] : string.Empty;

                foreach (var pair in CommandGroups)
                {
                    if (group != pair.Key.Name)
                        continue;

                    if (command == string.Empty)
                        return pair.Key.Help;

                    output = pair.Value.GetHelp(command);
                    found = true;
                }

                if (!found)
                    output = string.Format("Unknown command: {0} {1}", group, command);

                return output;
            }
        }
    }
}
