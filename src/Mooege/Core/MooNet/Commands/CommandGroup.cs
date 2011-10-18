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
using System.Reflection;
using Mooege.Common;
using Mooege.Net.MooNet;

namespace Mooege.Core.MooNet.Commands
{
    public class CommandGroup
    {
        private static readonly Logger Logger = LogManager.CreateLogger();

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

        public string Handle(string parameters, MooNetClient invokerClient)
        {
            return string.Empty;
        }

        /// <summary>
        /// Parameterless command group handler.
        /// </summary>
        /// <param name="invokerClient">The invoker client if any.</param>
        /// <returns><see cref="string"/></returns>
        public virtual string Invoke(MooNetClient invokerClient = null) { return string.Empty; }
    }
}
