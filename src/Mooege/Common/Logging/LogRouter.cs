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
using System.Linq;

namespace Mooege.Common.Logging
{
    /// <summary>
    /// LogRouter class that routes messages to appropriate log-targets.
    /// </summary>
    internal static class LogRouter
    {
        /// <summary>
        /// Routes a message to appropriate log-targets.
        /// </summary>
        /// <param name="level">Log level.</param>
        /// <param name="logger">Source of the log message.</param>
        /// <param name="message">Log message.</param>
        public static void RouteMessage(Logger.Level level, string logger, string message)
        {
            if (!LogManager.Enabled) // if we logging is not enabled,
                return; // just skip.

            if (LogManager.Targets.Count == 0) // if we don't have any active log-targets,
                return; // just skip

            // loop through all available logs targets and route the messages that meet the filters.
            foreach (var target in LogManager.Targets.Where(target => level >= target.MinimumLevel && level <= target.MaximumLevel))
            {
                target.LogMessage(level, logger, message);
            }
        }

        /// <summary>
        /// Routes a message to appropriate log-targets.
        /// </summary>
        /// <param name="level">Log level.</param>
        /// <param name="logger">Source of the log message.</param>
        /// <param name="message">Log message.</param>
        /// <param name="exception">Exception to be included with log message.</param>
        public static void RouteException(Logger.Level level, string logger, string message, Exception exception)
        {
            if (!LogManager.Enabled) // if we logging is not enabled,
                return; // just skip.

            if (LogManager.Targets.Count == 0) // if we don't have any active log-targets,
                return; // just skip

            // loop through all available logs targets and route the messages that meet the filters.
            foreach (var target in LogManager.Targets.Where(target => level >= target.MinimumLevel && level <= target.MaximumLevel))
            {
                target.LogException(level, logger, message, exception);
            }
        }
    }
}
