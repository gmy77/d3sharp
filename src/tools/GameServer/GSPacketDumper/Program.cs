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
using System.IO;
using Mooege.Common.Logging;

namespace GSPacketDumper
{
    internal class Program
    {
        private static readonly Logger Logger = LogManager.CreateLogger();

        private static void Main(string[] args)
        {
            PrintLicense();

            // Check command line
            if (args.Length != 1)
            {
                Console.WriteLine("usage: " + Environment.GetCommandLineArgs()[0] + " <filename>");
                return;
            }

            if (!File.Exists(args[0]))
            {
                Console.WriteLine("Input file: {0} not found", args[0]);
                Console.ReadLine();
                return;
            }

            var outputFile = Path.GetFileName(args[0]) + ".txt";
            if (File.Exists(outputFile)) File.Delete(outputFile);

            LogManager.Enabled = true;
            LogManager.AttachLogTarget(new FileTarget(outputFile, Logger.Level.PacketDump, Logger.Level.PacketDump, true, true));

            Console.WriteLine("Demystifying packets:");
            PacketReader.Read(args[0]);
            Console.WriteLine("\n\n[done]");
        }

        private static void PrintLicense()
        {
            Console.WriteLine("Copyright (C) 2011 mooege project");
            Console.WriteLine("mooege comes with ABSOLUTELY NO WARRANTY.");
            Console.WriteLine("This is free software, and you are welcome to redistribute it under certain conditions; see the LICENSE file for details.");
            Console.WriteLine();
        }
    }
}
