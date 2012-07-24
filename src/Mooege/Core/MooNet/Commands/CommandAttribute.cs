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
using Mooege.Core.MooNet.Accounts;

namespace Mooege.Core.MooNet.Commands
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CommandGroupAttribute : Attribute
    {
        /// <summary>
        /// Command group's name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Help text for command group.
        /// </summary>
        public string Help { get; private set; }

        /// <summary>
        /// Minimum user level required to invoke the command.
        /// </summary>
        public Account.UserLevels MinUserLevel { get; private set; }

        public CommandGroupAttribute(string name, string help, Account.UserLevels minUserLevel = Account.UserLevels.User)
        {
            this.Name = name.ToLower();
            this.Help = help;
            this.MinUserLevel = minUserLevel;
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class CommandAttribute : Attribute
    {
        /// <summary>
        /// Command's name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Help text for command.
        /// </summary>
        public string Help { get; private set; }

        /// <summary>
        /// Minimum user level required to invoke the command.
        /// </summary>
        public Account.UserLevels MinUserLevel { get; private set; }

        public CommandAttribute(string command, string help, Account.UserLevels minUserLevel = Account.UserLevels.User)
        {
            this.Name = command.ToLower();
            this.Help = help;
            this.MinUserLevel = minUserLevel;
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class DefaultCommand : CommandAttribute
    {
        public DefaultCommand(Account.UserLevels minUserLevel = Account.UserLevels.User)
            : base("", "", minUserLevel)
        {
        }
    }
}
