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

    public enum SNOGroup : int
    {
        Code = -2,
        None = -1,
        Actor = 1,
        Adventure = 2,
        AiBehavior = 3,
        AiState = 4,
        AmbientSound = 5,
        Anim = 6,
        Anim2D = 7,
        AnimSet = 8,
        Appearance = 9,
        Hero = 10,
        Cloth = 11,
        Conversation = 12,
        ConversationList = 13,
        EffectGroup = 14,
        Encounter = 15,
        Explosion = 17,
        FlagSet = 18,
        Font = 19,
        GameBalance = 20,
        Globals = 21,
        LevelArea = 22,
        Light = 23,
        MarkerSet = 24,
        Monster = 25,
        Observer = 26,
        Particle = 27,
        Physics = 28,
        Power = 29,
        Quest = 31,
        Rope = 32,
        Scene = 33,
        SceneGroup = 34,
        Script = 35,
        ShaderMap = 36,
        Shaders = 37,
        Shakes = 38,
        SkillKit = 39,
        Sound = 40,
        SoundBank = 41,
        StringList = 42,
        Surface = 43,
        Textures = 44,
        Trail = 45,
        UI = 46,
        Weather = 47,
        Worlds = 48,
        Recipe = 49,
        Condition = 51,
        TreasureClass = 52,
        Account = 53,
        Conductor = 54,
        TimedEvent = 55,
        Act = 56,
        Material = 57,
        QuestRange = 58,
        Lore = 59,
        Reverb = 60,
        PhysMesh = 61,
        Music = 62,
        Tutorial = 63,
        BossEncounter = 64,
    }
}
