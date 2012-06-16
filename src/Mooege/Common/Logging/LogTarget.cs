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

namespace Mooege.Common.Logging
{
    /// <summary>
    /// Log target.
    /// </summary>
    public class LogTarget
    {
        /// <summary>
        /// Minimum level of messages to emit.
        /// </summary>
        public Logger.Level MinimumLevel { get; protected set; }

        /// <summary>
        /// Maximum level of messages to emit.
        /// </summary>
        public Logger.Level MaximumLevel { get; protected set; }

        /// <summary>
        /// Include timestamps in log?
        /// </summary>
        public bool IncludeTimeStamps { get; protected set; }

        /// <summary>
        /// Logs a message to log-target.
        /// </summary>
        /// <param name="level">Log level.</param>
        /// <param name="logger">Source of the log message.</param>
        /// <param name="message">Log message.</param>
        public virtual void LogMessage(Logger.Level level, string logger, string message)
        {
            throw new NotSupportedException("Vanilla log-targets are not supported! Instead use a log-target implementation.");
        }

        /// <summary>
        /// Logs a message and an exception to log-target.
        /// </summary>
        /// <param name="level">Log level.</param>
        /// <param name="logger">Source of the log message.</param>
        /// <param name="message">Log message.</param>
        /// <param name="exception">Exception to be included with log message.</param>
        public virtual void LogException(Logger.Level level, string logger, string message, Exception exception)
        {
            throw new NotSupportedException("Vanilla log-targets are not supported! Instead use a log-target implementation.");
        }
    }
}
