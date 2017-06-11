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

namespace Mooege.Common.Logging
{
    /// <summary>
    /// Holds configuration info for log manager.
    /// </summary>
    public sealed class LogConfig : Config.Config
    {
        /// <summary>
        /// Gets or sets the logging root.
        /// </summary>
        public string LoggingRoot
        {
            get { return this.GetString("Root", @"logs"); }
            set { this.Set("Root", value); }
        }

        /// <summary>
        /// Available log target configs.
        /// </summary>
        public LogTargetConfig[] Targets = new[]
        {
            new LogTargetConfig("ConsoleLog"), 
            new LogTargetConfig("ServerLog"), 
            new LogTargetConfig("PacketLog")
        };

        /// <summary>
        /// Creates a new log config.
        /// </summary>
        private LogConfig() :
            base("Logging") // Call the base ctor with section name 'Logging'.
        { }

        /// <summary>
        /// Singleton instance.
        /// </summary>
        public static LogConfig Instance { get { return _instance; } }

        /// <summary>
        /// The internal instance pointer.
        /// </summary>
        private static readonly LogConfig _instance = new LogConfig();
    }

    /// <summary>
    /// Holds configuration of a log target.
    /// </summary>
    public class LogTargetConfig : Config.Config
    {
        /// <summary>
        /// Is enabled?
        /// </summary>
        public bool Enabled
        {
            get { return this.GetBoolean("Enabled", true); }
            set { this.Set("Enabled", value); }
        }

        /// <summary>
        /// Target type. Valid values are file and console.
        /// </summary>
        public string Target
        {
            get { return this.GetString("Target", "Console"); }
            set { this.GetString("Target", value); }
        }

        /// <summary>
        /// Include timestamps in logs?
        /// </summary>
        public bool IncludeTimeStamps
        {
            get { return this.GetBoolean("IncludeTimeStamps", false); }
            set { this.Set("IncludeTimeStamps", value); }
        }

        /// <summary>
        /// Filename if logtarget is a file based one.
        /// </summary>
        public string FileName
        {
            get { return this.GetString("FileName", ""); }
            set { this.GetString("FileName", value); }
        }

        /// <summary>
        /// Minimum level of messages to emit.
        /// </summary>
        public Logger.Level MinimumLevel
        {
            get { return (Logger.Level)(this.GetInt("MinimumLevel", (int)Logger.Level.Info, true)); }
            set { this.Set("MinimumLevel", (int)value); }
        }

        /// <summary>
        /// Maximum level of messages to emit
        /// </summary>
        public Logger.Level MaximumLevel
        {
            get { return (Logger.Level)(this.GetInt("MaximumLevel", (int)Logger.Level.Fatal, true)); }
            set { this.Set("MaximumLevel", (int)value); }
        }

        /// <summary>
        /// Reset log file on startup?
        /// </summary>
        public bool ResetOnStartup
        {
            get { return this.GetBoolean("ResetOnStartup", false); }
            set { this.Set("ResetOnStartup", value); }
        }

        public LogTargetConfig(string loggerName)
            : base(loggerName) { }
    }
}
