using System;
using System.Reflection;
using D3Sharp.Net;
using D3Sharp.Net.Packets;
using D3Sharp.Utils;

namespace D3Sharp
{
    internal class Program
    {
        private static readonly Logger Logger = LogManager.CreateLogger();

        private int _port;
        private Server _server;

        public static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandler; // watch for unhandled-exceptions.

            LogManager.Enabled = true; // enable the logger.
            LogManager.AttachLogTarget(new ConsoleTarget(Level.Trace)); // attach a console-target.

            Logger.Info("d3sharp v{0} warming-up..", Assembly.GetExecutingAssembly().GetName().Version);

            var main = new Program(); // startup.
            main.ParseArguments(args);
            main.Run();
        }
        
        Program() {
            this._port=1345;
        }
        
        public void ParseArguments(string[] args) {
            // Temp code
            if (args.Length>0) {
                int port;
                if (!Int32.TryParse(args[0], out port))
                    Logger.Warn("Invalid format for port; defaulting to {0}", _port);
                else
                    _port=port;
            }
        }
        
        public void Run()
        {
            using (_server = new Server()) // Create new test server.
            {
                InitializeServerEvents(); // Initializes server events for debug output.

                // we can't listen for port 1119 because D3 and the launcher (agent) communicates on that port through loopback.
                // so we change our default port and start D3 with a shortcut like so:
                //   "F:\Diablo III Beta\Diablo III.exe" -launch -auroraaddress 127.0.0.1:1345
                _server.Listen(_port);
                Logger.Info("Server is listening on port {0}...", _server.Port.ToString());

                // Read user input indefinitely.
                while (_server.IsListening)
                {
                    var line = Console.ReadLine();
                    if (!string.Equals("quit", line, StringComparison.OrdinalIgnoreCase)
                     && !string.Equals("exit", line, StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }
                    Logger.Info("Shutting down server...");
                    _server.Shutdown();
                }
            }
        }

        private void InitializeServerEvents()
        {
            _server.ClientConnected += (sender, e) => Logger.Trace("Client connected: {0}", e.Client.ToString());
            _server.ClientDisconnected += (sender, e) => Logger.Trace("Client disconnected: {0}", e.Client.ToString());
            _server.DataReceived += (sender, e) => PacketRouter.Route(e);
            _server.DataSent += (sender, e) => { };
        }

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
