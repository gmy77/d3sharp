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
using System.Reflection;
using Gibbed.IO;
using Mooege.Core.GS.Common.Types.SNO;

namespace Mooege.Common.MPQ
{
    public class Data : MPQPatchChain
    {        
        public Dictionary<SNOGroup, Dictionary<int, Asset>> Assets = new Dictionary<SNOGroup, Dictionary<int, Asset>>();
        public readonly Dictionary<SNOGroup, Type> AssetFormats = new Dictionary<SNOGroup, Type>();

        public Data()
            : base(7447, new List<string> { "CoreData.mpq", "ClientData.mpq" }, "/base/d3-update-base-(?<version>.*?).mpq")
        { }

        public void Init()
        {
            this.LoadCatalog();
        }

        private void InitCatalog()
        {
            foreach (SNOGroup group in Enum.GetValues(typeof(SNOGroup)))
            {
                this.Assets.Add(group, new Dictionary<int, Asset>());
            }

            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (!type.IsSubclassOf(typeof (FileFormat))) continue;
                var attributes = (FileFormatAttribute[])type.GetCustomAttributes(typeof(FileFormatAttribute), true);
                if (attributes.Length == 0) continue;

                AssetFormats.Add(attributes[0].Group, type);
            }
        }

        private void LoadCatalog()
        {
            this.InitCatalog();

            var tocFile = this.FileSystem.FindFile("toc.dat");
            if (tocFile == null)
            {
                Logger.Warn("Couldn't load CoreData catalog: toc.dat");
                return;
            }
            
            var stream = tocFile.Open();
            var assetsCount = stream.ReadValueS32();

            while(stream.Position<stream.Length)
            {
                var group = (SNOGroup)stream.ReadValueS32();
                var snoId = stream.ReadValueS32();
                var name = stream.ReadString(128, true);
                    
                var asset = new Asset(group, snoId, name);
                this.Assets[group].Add(snoId, asset);
            }

            stream.Close();
            
            Logger.Info("Loaded a total of {0} assets.", assetsCount);
        }
    }
}
