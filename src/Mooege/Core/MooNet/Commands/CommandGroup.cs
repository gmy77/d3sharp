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

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mooege.Common.Logging;
using Mooege.Net.MooNet;

namespace Mooege.Core.MooNet.Commands
{
    public class CommandGroup
    {
        private static readonly Logger Logger = LogManager.CreateLogger();

        public CommandGroupAttribute Attributes { get; private set; }

        private readonly Dictionary<CommandAttribute, MethodInfo> _commands =
            new Dictionary<CommandAttribute, MethodInfo>();

        public void Register(CommandGroupAttribute attributes)
        {
            this.Attributes = attributes;
            this.RegisterDefaultCommand();
            this.RegisterCommands();
        }

        private void RegisterCommands()
        {
            foreach (var method in this.GetType().GetMethods())
            {
                object[] attributes = method.GetCustomAttributes(typeof(CommandAttribute), true);
                if (attributes.Length == 0) continue;

                var attribute = (CommandAttribute)attributes[0];
                if (attribute is DefaultCommand) continue;

                if (!this._commands.ContainsKey(attribute))
                    this._commands.Add(attribute, method);
                else
                    Logger.Warn("There exists an already registered command '{0}'.", attribute.Name);
            }
        }

        private void RegisterDefaultCommand()
        {
            foreach (var method in this.GetType().GetMethods())
            {
                object[] attributes = method.GetCustomAttributes(typeof(DefaultCommand), true);
                if (attributes.Length == 0) continue;
                if (method.Name.ToLower() == "fallback") continue;

                this._commands.Add(new DefaultCommand(this.Attributes.MinUserLevel), method);
                return;
            }

            // set the fallback command if we couldn't find a defined DefaultCommand.
            this._commands.Add(new DefaultCommand(this.Attributes.MinUserLevel), this.GetType().GetMethod("Fallback"));
        }

        public virtual string Handle(string parameters, MooNetClient invokerClient = null)
        {
            // check if the user has enough privileges to access command group.
            // check if the user has enough privileges to invoke the command.
            if (invokerClient != null && this.Attributes.MinUserLevel > invokerClient.Account.UserLevel)
                return "You don't have enough privileges to invoke that command.";

            string[] @params = null;
            CommandAttribute target = null;

            if (parameters == string.Empty)
                target = this.GetDefaultSubcommand();
            else
            {
                @params = parameters.Split(' ');
                target = this.GetSubcommand(@params[0]) ?? this.GetDefaultSubcommand();

                if (target != this.GetDefaultSubcommand())
                    @params = @params.Skip(1).ToArray();
            }

            // check if the user has enough privileges to invoke the command.
            if (invokerClient != null && target.MinUserLevel > invokerClient.Account.UserLevel)
                return "You don't have enough privileges to invoke that command.";

            return (string)this._commands[target].Invoke(this, new object[] { @params, invokerClient });
        }

        public string GetHelp(string command)
        {
            foreach (var pair in this._commands)
            {
                if (command != pair.Key.Name) continue;
                return pair.Key.Help;
            }

            return string.Empty;
        }

        [DefaultCommand]
        public virtual string Fallback(string[] @params = null, MooNetClient invokerClient = null)
        {
            var output = "Available subcommands: ";
            foreach (var pair in this._commands)
            {
                if (pair.Key.Name.Trim() == string.Empty) continue; // skip fallback command.
                if (invokerClient != null && pair.Key.MinUserLevel > invokerClient.Account.UserLevel) continue;
                output += pair.Key.Name + ", ";
            }

            return output.Substring(0, output.Length - 2) + ".";
        }

        protected CommandAttribute GetDefaultSubcommand()
        {
            return this._commands.Keys.First();
        }

        protected CommandAttribute GetSubcommand(string name)
        {
            return this._commands.Keys.FirstOrDefault(command => command.Name == name);
        }
    }
}
