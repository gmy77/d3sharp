using System;
using System.Text;
using ProtoHelpers.Utils;

namespace ProtoStringFixer
{
    public class Program
    {
        public static int Main(string[] args)
        {
            bool handledargs=false, spaced=true;
            string[] given=null, stdin=null;
            if (args.Length > 0)
            {
                if (args[0]=="-nospace")
                {
                    spaced=false;
                    handledargs=true;
                }
                if (handledargs)
                    given=args;
                else {
                    given=new string[args.Length-1];
                    Array.Copy(args, 1, given, 0, args.Length-1);
                }
            }
            else if (Console.In.Peek() > -1)
            {
                string input = Console.In.ReadToEnd().Trim();
                stdin = input.Split('\n');
            }
            else if (args.Length==0 && !handledargs)
            {
                Console.WriteLine("No arguments given");
            }
            if (given!=null && given.Length > 0)
                ParseAndOutput(given, spaced);
            if (stdin!=null && stdin.Length > 0)
                ParseAndOutput(stdin, spaced);
            return 0;
        }
        
        public static void ParseAndOutput(string[] args, bool spaced)
        {
            foreach (string str in args)
            {
                Console.WriteLine("{0}", Conversion.Unescape(str).DumpHex(spaced));
                Console.WriteLine();
            }
        }
    }
}

