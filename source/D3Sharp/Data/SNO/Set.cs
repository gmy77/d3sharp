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
using System.IO;
using System.Text;
using System.Collections.Generic;
using D3Sharp.Utils;

namespace D3Sharp.Data.SNO
{
    // Need moar, cap'n!
    public enum SNOType
    {
        Unknown,
        Actor,          // .acr
        Power           // .pow
    }

    public enum SNOGroup
    {
        Ungrouped,
        Actors,
        NPCs,
        Mobs,
        Powers,
        Blacklist
    }

    public struct SNOID
    {
        public SNOType Type;
        public int ID;
        public string Name;

        public SNOID(SNOType type, int id, string name)
        {
            this.Type = type;
            this.ID = id;
            this.Name = name;
        }
        
        public override string ToString()
        {
            return String.Format("Type: {0:10} ID: {1:10}  Name: {2}", this.Type, this.ID, this.Name);
        }
    }

    public class SNOSet
    {
        private static readonly Logger Logger = LogManager.CreateLogger();
        public Dictionary<int, SNOID> IDs = new Dictionary<int, SNOID>();
        public SNOGroup Group { get; private set; }
        public string Path { get; private set; }

        public SNOSet(SNOGroup grp)
        {
            this.Group = grp;
        }

        public void Load(string path)
        {
            this.Path = path;
            var reader = File.OpenText(path);
            int currentLine = 1;
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine().Trim();
                if (line.Length == 0)
                    continue;

                int spaceIndex = line.IndexOf(' ');
                int id = 0;
                try
                {
                    id = Int32.Parse(line.Substring(0, spaceIndex > -1 ? spaceIndex : line.Length).Trim(), System.Globalization.CultureInfo.InvariantCulture);
                }
                catch (Exception)
                {
                    Logger.Warn("Failed to parse ID on line {0}", currentLine);
                    continue;
                }
                string snoFilename = String.Empty;
                if (spaceIndex > -1)
                {
                    try
                    {
                        snoFilename = line.Substring(spaceIndex+1, line.Length-spaceIndex-1).Trim();
                    }
                    catch (Exception)
                    {
                        Logger.Warn("Failed to parse filename on line {0}", currentLine);
                        continue;
                    }
                }
                SNOType type = GetTypeFromExtension(System.IO.Path.GetExtension(snoFilename));
                string name = System.IO.Path.GetFileName(snoFilename);
                SNOID snoid = new SNOID(type, id, name);
                this.IDs.Add(id, snoid);
                ++currentLine;
            }
            reader.Close();
        }

        public bool IsGroupReferencing()
        {
            return IsGroupReferencing(this.Group);
        }

        public static SNOType GetTypeFromExtension(string ext)
        {
            switch (ext.ToLower())
            {
                case "acr":
                    return SNOType.Actor;
                case "pow":
                    return SNOType.Power;
            }
            return SNOType.Unknown;
        }

        // Check whether a group is referencing or defining IDs
        public static bool IsGroupReferencing(SNOGroup grp)
        {
            switch (grp)
            {
                case SNOGroup.Ungrouped:
                case SNOGroup.Actors:
                    return false;
            }
            return true;
        }
    }
}

