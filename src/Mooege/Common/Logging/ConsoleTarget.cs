using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mooege.Common.Logging
{
    /// <summary>
    /// Console target for log messages.
    /// </summary>
    public class ConsoleTarget : LogTarget
    {
        /// <summary>
        /// Creates a new console target.
        /// </summary>
        /// <param name="minLevel">minimum log level.s</param>
        /// <param name="maxLevel"></param>
        /// <param name="includeTimeStamps"></param>
        public ConsoleTarget(Logger.Level minLevel, Logger.Level maxLevel, bool includeTimeStamps)
        {
            MinimumLevel = minLevel;
            MaximumLevel = maxLevel;
            this.IncludeTimeStamps = includeTimeStamps;
        }

        public override void LogMessage(Logger.Level level, string logger, string message)
        {
            var timeStamp = this.IncludeTimeStamps
                                ? "[" + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.fff") + "] "
                                : "";

            SetForeGroundColor(level);
            Console.WriteLine(string.Format("{0}[{1}] [{2}]: {3}", timeStamp, level.ToString().PadLeft(5), logger, message));
        }

        public override void LogException(Logger.Level level, string logger, string message, Exception exception)
        {
            var timeStamp = this.IncludeTimeStamps
                                ? "[" + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.fff") + "] "
                                : "";

            SetForeGroundColor(level);
            Console.WriteLine(string.Format("{0}[{1}] [{2}]: {3} - [Exception] {4}", timeStamp, level.ToString().PadLeft(5), logger, message, exception));
        }

        private static void SetForeGroundColor(Logger.Level level)
        {
            switch (level)
            {
                case Logger.Level.PacketDump: Console.ForegroundColor = ConsoleColor.DarkGray; break;
                case Logger.Level.Trace: Console.ForegroundColor = ConsoleColor.DarkGray; break;
                case Logger.Level.Debug: Console.ForegroundColor = ConsoleColor.Cyan; break;
                case Logger.Level.Info: Console.ForegroundColor = ConsoleColor.White; break;
                case Logger.Level.Warn: Console.ForegroundColor = ConsoleColor.Yellow; break;
                case Logger.Level.Error: Console.ForegroundColor = ConsoleColor.Magenta; break;
                case Logger.Level.Fatal: Console.ForegroundColor = ConsoleColor.Red; break;
                default: break;
            }
        }
    }
}
