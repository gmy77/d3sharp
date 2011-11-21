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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using CrystalMpq;
using Gibbed.IO;
using Mooege.Common.Versions;
using Mooege.Core.GS.Common.Types.SNO;
using System.Linq;

namespace Mooege.Common.MPQ
{
    public class Data : MPQPatchChain
    {
        public Dictionary<SNOGroup, ConcurrentDictionary<int, Asset>> Assets = new Dictionary<SNOGroup, ConcurrentDictionary<int, Asset>>();
        public readonly Dictionary<SNOGroup, Type> Parsers = new Dictionary<SNOGroup, Type>();
        private readonly List<Task> _tasks = new List<Task>();
        private static readonly SNOGroup[] PatchExceptions = new[] { SNOGroup.TreasureClass, SNOGroup.TimedEvent, SNOGroup.ConversationList };

        public Data()
            : base(VersionInfo.MPQ.RequiredPatchVersion, new List<string> { "CoreData.mpq", "ClientData.mpq" }, "/base/d3-update-base-(?<version>.*?).mpq")
        { }

        public void Init()
        {
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
                var attributes = (FileFormatAttribute[])type.GetCustomAttributes(typeof(FileFormatAttribute), true);
                if (attributes.Length == 0) continue;

                Parsers.Add(attributes[0].Group, type);
            }
        }

        private void LoadCatalogs()
        {
            this.LoadCatalog("CoreTOC.dat"); // as of patch beta patch 7841, blizz renamed TOC.dat as CoreTOC.dat
            this.LoadCatalog("TOC.dat", true, PatchExceptions.ToList()); // used for reading assets patched to zero bytes and removed from mainCatalog file.          
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

                var asset = this.ProcessAsset(group, snoId, name); // process the asset.
                this.Assets[group].TryAdd(snoId, asset); // add it to our assets dictionary.
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

            if(Storage.Config.Instance.LazyLoading)
                Logger.Info("Found a total of {0} assets from {1} catalog and postponed loading because lazy loading is activated.", assetsCount, fileName);
            else
                Logger.Info("Found a total of {0} assets from {1} catalog and parsed {2} of them in {3:c}.", assetsCount, fileName, this._tasks.Count, elapsedTime);
        }

        private Asset ProcessAsset(SNOGroup group, Int32 snoId, string name)
        {
            var asset = Storage.Config.Instance.LazyLoading ? new LazyAsset(group, snoId, name) : new Asset(group, snoId, name); // create the asset.
            if (!this.Parsers.ContainsKey(asset.Group)) return asset; // if we don't have a proper parser for asset, just give up.

            var parser = this.Parsers[asset.Group]; // get the type the asset's parser.
            var file = this.GetFile(asset.FileName, PatchExceptions.Contains(asset.Group)); // get the file. note: if file is in any of the groups in PatchExceptions it'll from load the original version - the reason is that assets in those groups got patched to 0 bytes. /raist.

            if (file == null || file.Size < 10) return asset; // if it's empty, give up again.

            if (Storage.Config.Instance.EnableTasks)
                this._tasks.Add(new Task(() => asset.RunParser(parser, file))); // add it to our task list, so we can parse them concurrently.        
            else
                asset.RunParser(parser, file); // run the parsers sequentally.

            return asset;
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
