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
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using Mooege.Common;
using Mooege.Core.Common.Items;
using Mooege.Core.MooNet.Accounts;
using Mooege.Core.MooNet.Commands;
using Mooege.Core.MooNet.Online;
using Mooege.Net.GS;
using Mooege.Net.MooNet;

namespace Mooege
{
    internal class Program
    {
        private static readonly Logger Logger = LogManager.CreateLogger();

        public static MooNetServer MooNetServer;
        public static GameServer GameServer;

        public static Thread MooNetServerThread;
        public static Thread GameServerThread;

        public static void Main(string[] args)
        {
            // Watch for unhandled exceptions
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandler;

            Console.ForegroundColor = ConsoleColor.Yellow;
            PrintBanner();
            PrintLicense();
            Console.ResetColor();

            InitLoggers(); // init logging facility.

            Logger.Info("mooege v{0} warming-up..", Assembly.GetExecutingAssembly().GetName().Version);
            Logger.Info("Item database loaded with a total of {0} item definitions", ItemGenerator.TotalItems);

            StartupServers();
        }

        private static void StartupServers()
        {
            MooNetServer = new MooNetServer();
            MooNetServerThread = new Thread(MooNetServer.Run) {IsBackground = true};
            MooNetServerThread.Start();

            GameServer = new GameServer();
            GameServerThread = new Thread(GameServer.Run) { IsBackground = true };
            GameServerThread.Start();

            while (true)
            {
                var line = Console.ReadLine();
                CommandManager.Parse(line);                
            }
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

        #region general commands 

        [ServerCommand("stats")]
        public static void Stats(string parameters)
        {
            // warning: only use mono-enabled counters here - http://www.mono-project.com/Mono_Performance_Counters

            if(parameters.ToLower()=="help")
            {
                Console.WriteLine("stats [detailed]");
                return;
            }

            var output = new StringBuilder();
            output.AppendFormat("Total Accounts: {0}, Online Players: {1}", AccountManager.TotalAccounts, PlayerManager.OnlinePlayers.Count);

            if (parameters.ToLower() != "detailed")
            {
                Console.WriteLine(output.ToString());
                return;
            }

            output.AppendFormat("\nGC Allocated Memory: {0}KB ", GC.GetTotalMemory(true)/1024);

            if (PerformanceCounterCategory.Exists("Processor") && PerformanceCounterCategory.CounterExists("% Processor Time", "Processor"))
            {
                var processorTimeCounter = new PerformanceCounter { CategoryName = "Processor", CounterName = "% Processor Time", InstanceName = "_Total" };
                output.AppendFormat("Processor Time: {0}%", processorTimeCounter.NextValue());
            }

            if (PerformanceCounterCategory.Exists(".NET CLR LocksAndThreads"))
            {
                if(PerformanceCounterCategory.CounterExists("# of current physical Threads", ".NET CLR LocksAndThreads"))
                {
                    var physicalThreadsCounter = new PerformanceCounter { CategoryName = ".NET CLR LocksAndThreads", CounterName = "# of current physical Threads", InstanceName = Process.GetCurrentProcess().ProcessName };
                    output.AppendFormat("\nPhysical Threads: {0} ", physicalThreadsCounter.NextValue());
                }

                if(PerformanceCounterCategory.CounterExists("# of current logical Threads", ".NET CLR LocksAndThreads"))
                {
                    var logicalThreadsCounter = new PerformanceCounter { CategoryName = ".NET CLR LocksAndThreads", CounterName = "# of current logical Threads", InstanceName = Process.GetCurrentProcess().ProcessName };
                    output.AppendFormat("Logical Threads: {0} ", logicalThreadsCounter.NextValue());
                }

                if (PerformanceCounterCategory.CounterExists("Contention Rate / sec", ".NET CLR LocksAndThreads"))
                {
                    var contentionRateCounter = new PerformanceCounter { CategoryName = ".NET CLR LocksAndThreads", CounterName = "Contention Rate / sec", InstanceName = Process.GetCurrentProcess().ProcessName };
                    output.AppendFormat("Contention Rate: {0}/sec", contentionRateCounter.NextValue());
                }
            }

            if (PerformanceCounterCategory.Exists(".NET CLR Exceptions") && PerformanceCounterCategory.CounterExists("# of Exceps Thrown", ".NET CLR Exceptions"))
            {
                var exceptionsThrownCounter = new PerformanceCounter { CategoryName = ".NET CLR Exceptions", CounterName = "# of Exceps Thrown", InstanceName = Process.GetCurrentProcess().ProcessName };
                output.AppendFormat("\nExceptions Thrown: {0}", exceptionsThrownCounter.NextValue());
            }

            Console.WriteLine(output.ToString());
        }

        [ServerCommand("version")]
        public static void Version(string parameters)
        {
            Console.WriteLine("v{0}", Assembly.GetExecutingAssembly().GetName().Version);
        }

        private static DateTime _startupTime = DateTime.Now; 

        [ServerCommand("uptime")]
        public static void Uptime(string parameters)
        {
            var uptime = DateTime.Now - _startupTime;
            Console.WriteLine("Uptime: {0} days, {1} hours, {2} minutes, {3} seconds.", uptime.Days, uptime.Hours, uptime.Minutes, uptime.Seconds);
        }

        [ServerCommand("shutdown")]
        public static void Shutdown(string parameters)
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

        [ServerCommand("stop")]
        public static void Stop(string parameters) // actually server.cs should be providing us start/stop/restart methods /raist.
        {
            var stopMooNetServer = false;
            var stopGameServer = false;

            if (parameters.ToLower() == "help")
            {
                Console.WriteLine("stop [all|moonet|gs]");
                return;
            }

            if(parameters==String.Empty || parameters=="all")
            {
                stopMooNetServer = true;
                stopGameServer = true;
            }

            if (parameters == "mnet") stopMooNetServer = true;
            if (parameters == "gs") stopGameServer = true;

            if (stopMooNetServer)
            {
                if (MooNetServer != null)
                {
                    Logger.Warn("Stopping MooNet-Server..");
                    MooNetServer.Shutdown();
                    MooNetServerThread.Abort();
                    MooNetServer = null;
                }
                else Console.WriteLine("MooNet-Server is already stopped");
            }

            if (stopGameServer)
            {
                if (GameServer != null)
                {
                    Logger.Warn("Stopping Game-Server..");
                    GameServer.Shutdown();
                    GameServerThread.Abort();
                    GameServer = null;
                }
                else Console.WriteLine("Game-Server is already stopped");
            }            
        }

        [ServerCommand("start")]
        public static void Start(string parameters) // actually server.cs should be providing us start/stop/restart methods /raist.
        {
            var startMooNetServer = false;
            var startGameServer = false;

            if (parameters.ToLower() == "help")
            {
                Console.WriteLine("start [all|mnet|gs]");
                return;
            }

            if (parameters == String.Empty || parameters == "all")
            {
                startMooNetServer = true;
                startGameServer = true;
            }

            if (parameters == "mnet") startMooNetServer = true;
            if (parameters == "gs") startGameServer = true;

            if (startMooNetServer)
            {
                if (MooNetServer == null)
                {
                    MooNetServer = new MooNetServer();
                    MooNetServerThread = new Thread(MooNetServer.Run) { IsBackground = true };
                    MooNetServerThread.Start();
                }
                else Console.WriteLine("MooNet-Server is already running");
            }

            if (startGameServer)
            {
                if (GameServer == null)
                {
                    GameServer = new GameServer();
                    GameServerThread = new Thread(GameServer.Run) { IsBackground = true };
                    GameServerThread.Start();
                }
                else Console.WriteLine("Game-Server is already running");
            }
        }

        #endregion
    }
}
