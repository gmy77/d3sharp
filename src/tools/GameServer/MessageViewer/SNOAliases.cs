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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GameMessageViewer
{
    /// <summary>
    /// Creates a dictionary with names for snos
    /// </summary>
    class SNOAliases
    {
        public static Dictionary<string, string> Aliases;
        public static Dictionary<string, string> AnimationGroups;


        static SNOAliases()
        {
            Aliases = new Dictionary<string, string>();
            AnimationGroups = new Dictionary<string, string>();

            try
            {
                foreach (string filename in new string[] { "snos.txt"})
                    foreach (string entry in File.ReadAllLines(filename))
                        if(Aliases.ContainsKey(entry.Split(' ')[0]) == false)
                            Aliases.Add(entry.Split(' ')[0], String.Join(" ", entry.Split(' ').Skip(1).ToArray()));

                foreach (string filename in new string[] { "AnimationGroups.txt" })
                    foreach (string entry in File.ReadAllLines(filename))
                        if (AnimationGroups.ContainsKey(entry.Split(' ')[0]) == false)
                            AnimationGroups.Add(entry.Split(' ')[0], entry.Split(' ')[1]);
            }
            catch (Exception) { Console.WriteLine("Error creating sno list"); }
        }
    }
}
