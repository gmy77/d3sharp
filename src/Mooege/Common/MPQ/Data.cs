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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using CrystalMpq;
using Gibbed.IO;
using Mooege.Common.Logging;
using Mooege.Common.Versions;
using Mooege.Core.GS.Common.Types.SNO;
using System.Linq;
using System.Data.SQLite;
using Mooege.Common.Storage;

namespace Mooege.Common.MPQ
{
    public class Data : MPQPatchChain
    {
        public Dictionary<SNOGroup, ConcurrentDictionary<int, Asset>> Assets = new Dictionary<SNOGroup, ConcurrentDictionary<int, Asset>>();
        public readonly Dictionary<SNOGroup, Type> Parsers = new Dictionary<SNOGroup, Type>();
        private readonly List<Task> _tasks = new List<Task>();
        private static readonly SNOGroup[] PatchExceptions = new[] { SNOGroup.TimedEvent, SNOGroup.Script, SNOGroup.AiBehavior, SNOGroup.AiState, SNOGroup.Conductor, SNOGroup.FlagSet, SNOGroup.Code };

        protected static new readonly Logger Logger = LogManager.CreateLogger();

        public Data()
            : base(VersionInfo.MPQ.RequiredPatchVersion, new List<string> { "CoreData.mpq", "ClientData.mpq" }, "/base/d3-update-base-(?<version>.*?).mpq")
        { }

        public void Init()
        {
            Logger.Info("Reading assets from MPQ data..");
            this.InitCatalog(); // init asset-group dictionaries and parsers.
            this.LoadCatalogs(); // process the assets.
        }

        private void InitCatalog()
        {
            foreach (SNOGroup group in Enum.GetValues(typeof(SNOGroup)))
            {
                this.Assets.Add(group, new ConcurrentDictionary<int, Asset>());
            }

            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (!type.IsSubclassOf(typeof(FileFormat))) continue;
                var attributes = (FileFormatAttribute[])type.GetCustomAttributes(typeof(FileFormatAttribute), false);
                if (attributes.Length == 0) continue;

                Parsers.Add(attributes[0].Group, type);
            }
        }

        private void LoadCatalogs()
        {
            this.LoadCatalog("CoreTOC.dat"); // as of patch beta patch 7841, blizz renamed TOC.dat as CoreTOC.dat
            this.LoadDBCatalog();
        }

        private void LoadCatalog(string fileName, bool useBaseMPQ = false, List<SNOGroup> groupsToLoad = null)
        {
            var catalogFile = this.GetFile(fileName, useBaseMPQ);
            this._tasks.Clear();

            if (catalogFile == null)
            {
                Logger.Error("Couldn't load catalog file: {0}.", fileName);
                return;
            }

            var stream = catalogFile.Open();
            var assetsCount = stream.ReadValueS32();

            var timerStart = DateTime.Now;

            // read all assets from the catalog first and process them (ie. find the parser if any available).
            while (stream.Position < stream.Length)
            {
                var group = (SNOGroup)stream.ReadValueS32();
                var snoId = stream.ReadValueS32();
                var name = stream.ReadString(128, true);

                if (groupsToLoad != null && !groupsToLoad.Contains(group)) // if we're handled groups to load, just ignore the ones not in the list.
                    continue;

                var asset = new MPQAsset(group, snoId, name);
                asset.MpqFile = this.GetFile(asset.FileName, PatchExceptions.Contains(asset.Group)); // get the file. note: if file is in any of the groups in PatchExceptions it'll from load the original version - the reason is that assets in those groups got patched to 0 bytes. /raist.
                if (asset.MpqFile != null)
                    this.ProcessAsset(asset); // process the asset.
            }

            stream.Close();

            // Run the parsers for assets (that have a parser).

            if (this._tasks.Count > 0) // if we're running in tasked mode, run the parser tasks.
            {
                foreach (var task in this._tasks)
                {
                    task.Start();
                }

                Task.WaitAll(this._tasks.ToArray()); // Wait all tasks to finish.
            }

            GC.Collect(); // force a garbage collection.
            GC.WaitForPendingFinalizers();

            var elapsedTime = DateTime.Now - timerStart;

            if (Storage.Config.Instance.LazyLoading)
                Logger.Trace("Found a total of {0} assets from {1} catalog and postponed loading because lazy loading is activated.", assetsCount, fileName);
            else
                Logger.Trace("Found a total of {0} assets from {1} catalog and parsed {2} of them in {3:c}.", assetsCount, fileName, this._tasks.Count, elapsedTime);
        }

        /// <summary>
        /// Load the table of contents from the database. the database toc contains the sno ids of all objects
        /// that should / can no longer be loaded from mpq because it is zeroed out or because we need to edit
        /// some of the fields
        /// </summary>
        private void LoadDBCatalog()
        {
            int assetCount = 0;
            var timerStart = DateTime.Now;

            using (var cmd = new SQLiteCommand("SELECT * FROM TOC", DBManager.MPQMirror))
            {
                var itemReader = cmd.ExecuteReader();

                if (itemReader.HasRows)
                {
                    while (itemReader.Read())
                    {
                        ProcessAsset(new DBAsset(
                            (SNOGroup)Enum.Parse(typeof(SNOGroup), itemReader["SNOGroup"].ToString()),
                            Convert.ToInt32(itemReader["SNOId"]),
                            itemReader["Name"].ToString()));
                        assetCount++;
                    }
                }
            }

            if (Storage.Config.Instance.LazyLoading)
                Logger.Trace("Found a total of {0} assets from DB catalog and postponed loading because lazy loading is activated.", assetCount);
            else
                Logger.Trace("Found a total of {0} assets from DB catalog and parsed {1} of them in {2:c}.", assetCount, this._tasks.Count, DateTime.Now - timerStart);

        }

        /// <summary>
        /// Adds the asset to the dictionary and tries to parse it if a parser
        /// is found and lazy loading is deactivated
        /// </summary>
        /// <param name="asset">New asset to be processed</param>
        private void ProcessAsset(Asset asset)
        {
            this.Assets[asset.Group].TryAdd(asset.SNOId, asset);
            if (!this.Parsers.ContainsKey(asset.Group)) return;

            asset.Parser = this.Parsers[asset.Group];

            // If lazy loading is deactivated, immediatly run parsers sequentially or threaded
            if (Storage.Config.Instance.LazyLoading == false)
            {
                if (Storage.Config.Instance.EnableTasks)
                    this._tasks.Add(new Task(() => asset.RunParser()));
                else
                {
                    try
                    {
                        asset.RunParser();
                    }
                    catch (Exception e)
                    {
                        Logger.Error("Error parsing {0}.\nMessage: {1}\n InnerException:{2}\nStack Trace:{3}", asset.FileName, e.Message, e.InnerException == null ? "(null)" : e.InnerException.Message, e.StackTrace);
                    }
                }
            }
        }

        /// <summary>
        /// Gets a file from the mpq storage.
        /// </summary>
        /// <param name="fileName">File to read.</param>
        /// <param name="startSearchingFromBaseMPQ">Use the most available patched version? If you supply false to useMostAvailablePatchedVersion, it'll be looking for file starting from the base mpq up to latest available patch.</param>
        /// <returns>The MpqFile</returns>
        private MpqFile GetFile(string fileName, bool startSearchingFromBaseMPQ = false)
        {
            MpqFile file = null;

            //Ignore loading lvl files for now.
            if (fileName.Contains(".lvl"))
                return null;

            if (!startSearchingFromBaseMPQ)
                file = this.FileSystem.FindFile(fileName);
            else
            {
                foreach (MpqArchive archive in this.FileSystem.Archives.Reverse()) //search mpqs starting from base
                {
                    file = archive.FindFile(fileName);
                    if (file != null)
                        break;
                }
            }

            return file;
        }
    }
}
