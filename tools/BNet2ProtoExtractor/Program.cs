using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BNet2ProtoExtractor
{
    class Program
    {
        static void Main(string[] args)
        {
            // Check command line
            if (args.Length != 1)
            {
                Console.WriteLine("usage: " + Environment.GetCommandLineArgs()[0] + " <filename>");
                return;
            }

            Console.WriteLine("Demystifying packets:");
            PcapReader.Read(args[0]);
            Console.WriteLine("[done]..");

            Console.ReadLine();
        }
    }
}
