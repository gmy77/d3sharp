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

using System;
using System.Globalization;
using System.Reflection;
using System.Threading;
using Mooege.Common;
using Mooege.Common.MPQ;
using Mooege.Core.Common.Items;
using Mooege.Core.MooNet.Commands;
using Mooege.Net.GS;
using Mooege.Net.MooNet;
using Environment = System.Environment;

namespace Mooege
{
    internal class Program
    {
        private static readonly Logger Logger = LogManager.CreateLogger();
        public static readonly DateTime StartupTime = DateTime.Now; 

        public static MooNetServer MooNetServer;
        public static GameServer GameServer;

        public static Thread MooNetServerThread;
        public static Thread GameServerThread;

        public static void Main(string[] args)
        {
            // Watch for unhandled exceptions
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandler;

            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture; // Use invariant culture - we have to set it explicitly for every thread we create.

            Console.ForegroundColor = ConsoleColor.Yellow;
            PrintBanner();
            PrintLicense();
            Console.ResetColor();

            InitLoggers(); // init logging facility.

            Logger.Info("mooege v{0} warming-up..", Assembly.GetExecutingAssembly().GetName().Version);

            if (!MPQStorage.Initialized)
            {
                Logger.Fatal("Cannot run servers as MPQStorage failed initialization.");
                Console.ReadLine();
                return;
            }

            Logger.Info("Item database loaded with a total of {0} item definitions.", ItemGenerator.TotalItems);
            StartupServers();
        }

        private static void InitLoggers()
        {
            LogManager.Enabled = true;

            foreach (var targetConfig in LogConfig.Instance.Targets)
            {
                if (!targetConfig.Enabled) continue;

                LogTarget target = null;
                switch (targetConfig.Target.ToLower())
                {
                    case "console":
                        target = new ConsoleTarget(targetConfig.MinimumLevel, targetConfig.MaximumLevel,
                                                   targetConfig.IncludeTimeStamps);
                        break;
                    case "file":
                        target = new FileTarget(targetConfig.FileName, targetConfig.MinimumLevel,
                                                targetConfig.MaximumLevel, targetConfig.IncludeTimeStamps,
                                                targetConfig.ResetOnStartup);
                        break;
                }

                if (target != null) LogManager.AttachLogTarget(target);
            }
        }

        private static void PrintBanner()
        {
            Console.WriteLine(@"  _ __ ___    ___    ___    ___   __ _   ___ ");
            Console.WriteLine(@" | '_ ` _ \  / _ \  / _ \  / _ \ / _` | / _ \");
            Console.WriteLine(@" | | | | | || (_) || (_) ||  __/| (_| ||  __/");
            Console.WriteLine(@" |_| |_| |_| \___/  \___/  \___| \__, | \___|");
            Console.WriteLine(@"                                 |___/       ");
            Console.WriteLine();
        }

        private static void PrintLicense()
        {
            Console.WriteLine("Copyright (C) 2011 mooege project");
            Console.WriteLine("mooege comes with ABSOLUTELY NO WARRANTY.");
            Console.WriteLine("This is free software, and you are welcome to redistribute it under certain conditions; see the LICENSE file for details.");
            Console.WriteLine();
        }

        private static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.IsTerminating)
                Logger.FatalException((e.ExceptionObject as Exception), "Mooege terminating because of unhandled exception.");                
            else
                Logger.ErrorException((e.ExceptionObject as Exception), "Caught unhandled exception.");
            Console.ReadLine();
        }

        #region server-control

        private static void StartupServers()
        {
            StartMooNet();
            StartGS();

            while (true)
            {
                var line = Console.ReadLine();
                CommandManager.Parse(line);
            }
        }

        public static void Shutdown()
        {
            if (MooNetServer != null)
            {
                Logger.Warn("Shutting down MooNet-Server..");
                MooNetServer.Shutdown();
            }

            if (GameServer != null)
            {
                Logger.Warn("Shutting down Game-Server..");
                GameServer.Shutdown();
            }

            Environment.Exit(0);
        }

        public static bool StartMooNet()
        {
            if (MooNetServer != null) return false;

            MooNetServer = new MooNetServer();
            MooNetServerThread = new Thread(MooNetServer.Run) {IsBackground = true, CurrentCulture = CultureInfo.InvariantCulture};
            MooNetServerThread.Start();
            return true;
        }

        public static bool StopMooNet()
        {
            if (MooNetServer == null) return false;

            Logger.Warn("Stopping MooNet-Server..");
            MooNetServer.Shutdown();
            MooNetServerThread.Abort();
            MooNetServer = null;
            return true;
        }

        public static bool StartGS()
        {
            if (GameServer != null) return false;

            GameServer = new GameServer();
            GameServerThread = new Thread(GameServer.Run) { IsBackground = true, CurrentCulture = CultureInfo.InvariantCulture };
            GameServerThread.Start();
            return true;
        }

        public static bool StopGS()
        {
            if (GameServer == null) return false;

            Logger.Warn("Stopping Game-Server..");
            GameServer.Shutdown();
            GameServerThread.Abort();
            GameServer = null;
            return true;
        }

        #endregion
    }
}
