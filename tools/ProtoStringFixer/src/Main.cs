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

