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
using System.Collections.Generic;
using Mooege.Common;
using Mooege.Common.Helpers;

namespace Mooege.Core.GS.Data.SNO
{
    public class SNODatabase
    {
        private static readonly Logger Logger = LogManager.CreateLogger();
        private static readonly SNODatabase _instance;
        public static SNODatabase Instance { get { return _instance; } }

        public Dictionary<string, SNOSet> Sets = new Dictionary<string, SNOSet>();
        public Dictionary<int, SNOID> Global = new Dictionary<int, SNOID>();
        public Dictionary<SNOGroup, Dictionary<int, SNOID>> Grouped = new Dictionary<SNOGroup, Dictionary<int, SNOID>>();

        static SNODatabase()
        {
            _instance = new SNODatabase();
        }

        SNODatabase()
        {
            foreach (SNOGroup grp in Enum.GetValues(typeof(SNOGroup)))
            {
                this.Grouped.Add(grp, new Dictionary<int, SNOID>());
            }
            LoadList(Config.Instance.SNOList);
        }

        public void LoadList(string listPath)
        {
            Logger.Info("Loading SNO ID sets from list {0}", listPath);
            try
            {
                var reader = File.OpenText(listPath);
                // Switch to the list file's directory so that we can use relative paths
                var owd = Directory.GetCurrentDirectory();
                Directory.SetCurrentDirectory(Path.GetDirectoryName(listPath));
                int currentLine = 1;
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine().Trim();
                    if (line.Length == 0)
                        continue;

                    string[] parts = line.Split(' ');
                    if (parts.Length != 2)
                    {
                        Logger.Warn("Malformed line at {0} in file {1}", currentLine, listPath);
                        continue;
                    }
                    SNOGroup grp = SNOGroup.Ungrouped;
                    try {
                        grp = (SNOGroup)Enum.Parse(typeof(SNOGroup), parts[0]);
                    } catch (Exception)
                    {
                        Logger.Warn("{0} is not a valid group", parts[0]);
                        continue;
                    }
                    LoadSet(grp, parts[1]);
                    ++currentLine;
                }
                // Go back to the original working directory
                Directory.SetCurrentDirectory(owd);
                reader.Close();
            }
            catch (DirectoryNotFoundException)
            {
                Logger.Fatal("Could not find directory of file path {0}", listPath);
            }
            catch (FileNotFoundException)
            {
                Logger.Fatal("Could not open file {0}", listPath);
            }
        }

        public void LoadSet(SNOGroup grp, string path)
        {
            if (this.Sets.ContainsKey(path))
            {
                Logger.Error("Path {0} was already loaded", path);
                return;
            }
            SNOSet snoset = new SNOSet(grp);
            try
            {
                Logger.Info("Loading SNO ID set from {0}", path);
                snoset.Load(path);
                MergeSet(snoset);
            }
            catch (DirectoryNotFoundException)
            {
                Logger.Warn("Could not find directory of file path {0}", path);
            }
            catch (FileNotFoundException)
            {
                Logger.Warn("Could not open file {0}", path);
            }
            catch (Exception e)
            {
                Logger.DebugException(e, "LoadSet");
            }
        }

        public void MergeSet(SNOSet snoset)
        {
            this.Sets.Add(snoset.Path, snoset);
            var groupDict = this.Grouped[snoset.Group];
            bool referencing = snoset.IsGroupReferencing();
            foreach (var pair in snoset.IDs)
            {
                if (!referencing)
                {
                    // If group is not referencing another group, we should try to add it to Global
                    if (this.Global.ContainsKey(pair.Key))
                        Logger.Warn("SNO ID {0} is already present in Global (conflictor is ignored)", pair.Key);
                    else
                        this.Global.Add(pair.Key, pair.Value);
                }
                if (groupDict.ContainsKey(pair.Key))
                    Logger.Warn("SNO ID {0} is already present in group {1} (conflictor is ignored)", pair.Key, Enum.GetName(typeof(SNOGroup), snoset.Group));
                else
                    groupDict.Add(pair.Key, pair.Value);
            }
        }

        public bool IsOfGroup(int id, SNOGroup grp)
        {
            return this.Grouped[grp].ContainsKey(id);
        }

        public int RandomID()
        {
            int id = RandomHelper.RandomValue(this.Global).ID;
            //Logger.Debug("Grabbed random global ID: {0}", id);
            return id;
        }

        public int RandomID(SNOGroup grp)
        {
            int id = RandomHelper.RandomValue(this.Grouped[grp]).ID;
            //Logger.Debug("Grabbed random ID for group {0}: {1}", Enum.GetName(typeof(SNOGroup), grp), id);
            return id;
        }
    }
}
