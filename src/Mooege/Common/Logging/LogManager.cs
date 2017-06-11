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
using System.Diagnostics;
using System.Linq;

namespace Mooege.Common.Logging
{
    /// <summary>
    /// Log manager class.
    /// </summary>
    public static class LogManager
    {
        /// <summary>
        /// Is logging enabled?
        /// </summary>
        public static bool Enabled { get; set; }

        /// <summary>
        /// Available & configured log targets.
        /// </summary>
        internal readonly static List<LogTarget> Targets = new List<LogTarget>();

        /// <summary>
        /// Available loggers.
        /// </summary>
        internal static readonly Dictionary<string, Logger> Loggers = new Dictionary<string, Logger>();

        /// <summary>
        /// Creates and returns a logger named with declaring type.
        /// </summary>
        /// <returns>A <see cref="Logger"/> instance.</returns>
        public static Logger CreateLogger()
        {
            var frame = new StackFrame(1, false); // read stack frame.
            var name = frame.GetMethod().DeclaringType.Name; // get declaring type's name.

            if (name == null) // see if we got a name.
                throw new Exception("Error getting full name for declaring type.");

            if (!Loggers.ContainsKey(name))  // see if we already have instance for the given name.
                Loggers.Add(name, new Logger(name)); // add it to dictionary of loggers.

            return Loggers[name]; // return the newly created logger.
        }

        /// <summary>
        /// Creates and returns a logger with given name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>A <see cref="Logger"/> instance.</returns>
        public static Logger CreateLogger(string name)
        {
            if (!Loggers.ContainsKey(name)) // see if we already have instance for the given name.
                Loggers.Add(name, new Logger(name)); // add it to dictionary of loggers.

            return Loggers[name]; // return the newly created logger.
        }

        /// <summary>
        /// Attachs a new log target.
        /// </summary>
        /// <param name="target"></param>
        public static void AttachLogTarget(LogTarget target)
        {
            Targets.Add(target);
        }
    }
}
