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
using System.Reflection;
using System.Text;
using Mooege.Common.MPQ.FileFormats;
using Mooege.Common.Extensions;

namespace Mooege.Common.MPQ
{
    public class CoreData : MPQPatchChain
    {        
        public Dictionary<SNOGroup, Dictionary<int, Asset>> Assets = new Dictionary<SNOGroup, Dictionary<int, Asset>>();
        public readonly Dictionary<AssetFormatAttribute, Type> AssetFormats = new Dictionary<AssetFormatAttribute, Type>();

        public CoreData()
            : base("CoreData.mpq", "/base/d3-update-base-(?<version>.*?).MPQ")
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
                if (!type.IsSubclassOf(typeof (AssetFormat))) continue;
                var attributes = (AssetFormatAttribute[])type.GetCustomAttributes(typeof(AssetFormatAttribute), true);
                if (attributes.Length == 0) continue;

                AssetFormats.Add(attributes[0], type);
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

            Logger.Info("Loading CoreData assets catalog..");
            var stream = tocFile.Open();
            var assetsCount = stream.ReadInt32();

            while(stream.Position<stream.Length)
            {
                var group = (SNOGroup)stream.ReadInt32();
                var snoId = stream.ReadInt32();
                var name = new byte[128];
                stream.Read(name, 0, 128);
                
                var asset = new Asset(group, snoId, name);
                this.Assets[group].Add(snoId, asset);
            }

            stream.Close();
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

    //static string[] snoExtensions = new string[65]
    //{
    //    "", ".acr", ".adv", ".aib", ".ais", ".ams", ".ani", ".an2",
    //    ".ans", ".app", ".hro", ".clt", ".cnv", ".cnl", ".efg", ".enc", 
    //    "", ".xpl", ".flg",  ".fnt", ".gam", ".glo", ".lvl", ".lit", 
    //    ".mrk", ".mon", ".obs", ".prt", ".phy", ".pow", "", ".qst", 
    //    ".rop", ".scn", ".scg", ".scr", ".shm", ".shd", ".shk", ".skl",
    //    ".snd", ".sbk", ".stl", ".srf", ".tex", ".trl", ".ui", ".wth",
    //    ".wrl", ".rcp", "", ".cnd", ".trs", ".acc", ".con", ".tme",
    //    ".act", ".mat", ".qsr", ".lor", ".rev", ".phm", ".mus", ".tut", ".bos",
    //};
}
