/*
 * Copyright (C) 2011 D3Sharp Project
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
using System.Reflection;
using System.Threading;
using D3Sharp.Net.BNet;
using D3Sharp.Net.Game;
using D3Sharp.Utils;

namespace D3Sharp
{
    internal class Program
    {
        private static readonly Logger Logger = LogManager.CreateLogger();

        private static BnetServer _bnetServer;
        private static GameServer _gameServer;

        public static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandler; // watch for unhandled-exceptions.

            LogManager.Enabled = true; // enable the logger.
            LogManager.AttachLogTarget(new ConsoleTarget(Level.Trace)); // attach a console-target.
            LogManager.AttachLogTarget(new FileTarget(Level.Trace, "log.txt")); // attach a console-target.

            PrintLicence();

            Logger.Info("D3Sharp v{0} warming-up..", Assembly.GetExecutingAssembly().GetName().Version);
            StartupServers();
        }

        private static void StartupServers()
        {
            _bnetServer = new BnetServer();
            _gameServer = new GameServer();

            var bnetServerThread = new Thread(_bnetServer.Run) { IsBackground = true };
            bnetServerThread.Start();

            var gameServerThread = new Thread(_gameServer.Run) { IsBackground = true };
            gameServerThread.Start();

            // Read user input indefinitely.
            while (true)
            {
                var line = Console.ReadLine();
                if (!string.Equals("quit", line, StringComparison.OrdinalIgnoreCase)
                    && !string.Equals("exit", line, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                Logger.Info("Shutting down servers...");
                _bnetServer.Shutdown();
                _gameServer.Shutdown();
            }
        }

        private static void PrintLicence()
        {
            Console.WriteLine("D3Sharp, Copyright (C) 2011 D3Sharp Project\nD3Sharp comes with ABSOLUTELY NO WARRANTY.This is free software, and you are welcome to redistribute it under certain conditions; see LICENCE file for details\n");
        }

        //public void ParseArguments(string[] args)
        //{
        //    // Temp code
        //    if (args.Length > 0)
        //    {
        //        int port;
        //        if (!Int32.TryParse(args[0], out port))
        //            Logger.Warn("Invalid format for port; defaulting to {0}", _port);
        //        else
        //            _port = port;
        //    }
        //}

        private static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.IsTerminating)
                Logger.FatalException((e.ExceptionObject as Exception), "Application terminating because of unhandled exception.");
            else
                Logger.ErrorException((e.ExceptionObject as Exception), "Caught unhandled exception.");
            Console.ReadLine();
        }
    }
}
