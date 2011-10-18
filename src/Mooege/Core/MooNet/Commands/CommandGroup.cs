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
using System.Linq;
using System.Reflection;
using Mooege.Common;
using Mooege.Net.MooNet;

namespace Mooege.Core.MooNet.Commands
{
    public class CommandGroup
    {
        private static readonly Logger Logger = LogManager.CreateLogger();

        public CommandGroupAttribute Attributes;

        private readonly Dictionary<CommandAttribute, MethodInfo> _commands =
            new Dictionary<CommandAttribute, MethodInfo>();

        public CommandGroup()
        {            
            foreach (var method in this.GetType().GetMethods())
            {
                object[] attributes = method.GetCustomAttributes(typeof (CommandAttribute), true);
                if (attributes.Length == 0) continue;

                var attribute = (CommandAttribute) attributes[0];
                if (!this._commands.ContainsKey(attribute))
                    this._commands.Add(attribute, method);
                else
                    Logger.Warn("There exists an already registered command '{0}'.", attribute.Name);
            }
        }

        public virtual string Handle(string parameters, MooNetClient invokerClient=null)
        {
            if (parameters == string.Empty)
            {
                if (invokerClient != null && this.Attributes.MinUserLevel > invokerClient.Account.UserLevel)  // check for privileges.
                    return "You don't have enough privileges to invoke that command.";

                return this.Invoke(invokerClient);
            }

            var @params = parameters.Split(' ');

            foreach(var pair in this._commands)
            {
                if (@params[0] != pair.Key.Name) continue;

                if (invokerClient != null && this.Attributes.MinUserLevel > invokerClient.Account.UserLevel)  // check for privileges.
                    return "You don't have enough privileges to invoke that command.";

                return (string)pair.Value.Invoke(this, new object[] { @params, invokerClient });
            }

            return string.Empty;
        }

        public virtual string GetHelp(string command)
        {
            foreach (var pair in this._commands)
            {
                if (command != pair.Key.Name) continue;
                return pair.Key.Help;
            }

            return string.Empty;
        }

        /// <summary>
        /// Parameterless command group handler.
        /// </summary>
        /// <param name="invokerClient">The invoker client if any.</param>
        /// <returns><see cref="string"/></returns>
        public virtual string Invoke(MooNetClient invokerClient = null)
        {
            var output = this._commands.Aggregate("Available sub-commands: ", (current, pair) => current + (pair.Key.Name + ", "));
            return output.Substring(0, output.Length - 2) + ".";
        }
    }
}
