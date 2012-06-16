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
using System.Globalization;
using System.Reflection;
using System.Threading;
using Mooege.Common.Logging;
using Mooege.Common.MPQ;
using Mooege.Common.Storage;
using Mooege.Common.Storage.AccountDataBase.Entities;
using Mooege.Common.Versions;
using Mooege.Core.GS.Items;
using Mooege.Core.MooNet.Accounts;
using Mooege.Core.MooNet.Commands;
using Mooege.Net;
using Mooege.Net.GS;
using Mooege.Net.MooNet;
using Mooege.Core.MooNet.Achievement;
using Mooege.Net.WebServices;
using NHibernate.Linq;
using NHibernate.Util;
using Environment = System.Environment;

namespace Mooege
{
    /// <summary>
    /// Contains mooege's startup code.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Used for uptime calculations.
        /// </summary>
        public static readonly DateTime StartupTime = DateTime.Now; // used for uptime calculations.

        /// <summary>
        /// MooNetServer instance.
        /// </summary>
        public static MooNetServer MooNetServer;

        /// <summary>
        /// GameServer instance.
        /// </summary>
        public static GameServer GameServer;

        /// <summary>
        /// MooNetServer thread.
        /// </summary>
        public static Thread MooNetServerThread;

        /// <summary>
        /// GameServer thread.
        /// </summary>
        public static Thread GameServerThread;

        private static readonly Logger Logger = LogManager.CreateLogger(); // logger instance.

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandler; // Watch for any unhandled exceptions.
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture; // Use invariant culture - we have to set it explicitly for every thread we create to prevent any mpq-reading problems (mostly because of number formats).

            Console.ForegroundColor = ConsoleColor.Yellow;
            PrintBanner(); // print ascii banner.
            PrintLicense(); // print license text.
            Console.ResetColor(); // reset color back to default.
            
            InitLoggers(); // init logging facility.

            Logger.Info("mooege v{0} warming-up..", Assembly.GetExecutingAssembly().GetName().Version);
            Logger.Info("Required client version: {0}.", VersionInfo.MooNet.RequiredClientVersion);

            // init openssl & wrapper.
            try
            {
                Logger.Info("Found OpenSSL version {0}.", OpenSSL.Core.Version.Library.ToString());
            }
            catch (Exception e)
            {
                Logger.ErrorException(e, "OpenSSL init error.");
                Console.ReadLine();
                return;
            }

            // prefill the database.
            Common.Storage.AccountDataBase.SessionProvider.RebuildSchema();
            if (!DBSessions.AccountSession.Query<DBAccount>().Any())
            {
                Logger.Info("Initing new database, creating first owner account (test@,123456)");
                var account = AccountManager.CreateAccount("test@", "123456", "test", Account.UserLevels.Owner);
                var gameAccount = GameAccountManager.CreateGameAccount(account);
                account.DBAccount.DBGameAccounts.Add(gameAccount.DBGameAccount);
                account.SaveToDB();
            }

            // init MPQStorage.
            if (!MPQStorage.Initialized)
            {
                Logger.Fatal("Cannot run servers as MPQStorage failed initialization.");
                Console.ReadLine();
                return;
            }

            // load item database.
            Logger.Info("Loading item database..");
            Logger.Trace("Item database loaded with a total of {0} item definitions.", ItemGenerator.TotalItems);

            // load achievements database.
            Logger.Info("Loading achievements database..");
            Logger.Trace("Achievement file parsed with a total of {0} achievements and {1} criteria in {2} categories.",
                AchievementManager.TotalAchievements, AchievementManager.TotalCriteria, AchievementManager.TotalCategories);


            Logger.Info("Type '!commands' for a list of available commands.");

            StartupServers(); // startup the servers
        }

        /// <summary>
        /// Unhandled exception emitter.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;

            if (e.IsTerminating)
                Logger.FatalException(ex, "Mooege terminating because of unhandled exception.");                
            else
                Logger.ErrorException(ex, "Caught unhandled exception.");

            Console.ReadLine();
        }

        #region server startup managment

        private static void StartupServers()
        {
            if(NetworkingConfig.Instance.EnableIPv6)
                Logger.Info("IPv6 enabled!");

            StartMooNet(); // start moonet.
            StartGS(); // start game-server.

            if(Net.WebServices.Config.Instance.Enabled) // if webservices are enabled,
                StartWebServices(); // start them.

            while (true) // idle loop & command parser
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

            // todo: stop webservices.

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

        public static bool StartWebServices()
        {
            Environment.SetEnvironmentVariable("MONO_STRICT_MS_COMPLIANT", "yes"); // we need this here to make sure web-services also work under mono too. /raist.
			
            var webservices = new ServiceManager();
            webservices.Run();

            return true;
        }

        #endregion

        #region logging facility 

        /// <summary>
        /// Inits logging facility and loggers.
        /// </summary>
        private static void InitLoggers()
        {
            LogManager.Enabled = true; // enable logger by default.

            foreach (var targetConfig in LogConfig.Instance.Targets)
            {
                if (!targetConfig.Enabled)
                    continue;

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

                if (target != null)
                    LogManager.AttachLogTarget(target);
            }
        }

        #endregion

        #region console banners

        /// <summary>
        /// Prints an info banner.
        /// </summary>
        private static void PrintBanner()
        {
            Console.WriteLine(@"  _ __ ___    ___    ___    ___   __ _   ___ ");
            Console.WriteLine(@" | '_ ` _ \  / _ \  / _ \  / _ \ / _` | / _ \");
            Console.WriteLine(@" | | | | | || (_) || (_) ||  __/| (_| ||  __/");
            Console.WriteLine(@" |_| |_| |_| \___/  \___/  \___| \__, | \___|");
            Console.WriteLine(@"                                 |___/       ");
            Console.WriteLine();
        }

        /// <summary>
        /// Prints a copyright banner.
        /// </summary>
        private static void PrintLicense()
        {
            Console.WriteLine("Copyright (C) 2011 - 2012, mooege project");
            Console.WriteLine("mooege comes with ABSOLUTELY NO WARRANTY.");
            Console.WriteLine("This is free software, and you are welcome to redistribute it under certain conditions; see the LICENSE file for details.");
            Console.WriteLine();
        }

        #endregion
    }
}
