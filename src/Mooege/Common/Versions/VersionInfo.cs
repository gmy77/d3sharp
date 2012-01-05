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

using System.Collections.Generic;
using Mooege.Common.Extensions;
using Mooege.Net.MooNet;

namespace Mooege.Common.Versions
{
    /// <summary>
    /// Supported Versions Info.
    /// </summary>
    /// <remarks>Put anything related to versions here.</remarks>
    public static class VersionInfo
    {
        /// <summary>
        /// Main assembly versions info.
        /// </summary>
        public static class Assembly
        {
            /// <summary>
            /// Main assemblies version.
            /// </summary>
            public const string Version = "1.8101.*";
        }

        /// <summary>
        /// MooNet versions info.
        /// </summary>
        public static class MooNet
        {
            /// <summary>
            /// Required client version.
            /// </summary>
            public const int RequiredClientVersion = 8101;

            public static Dictionary<string, int> ClientVersionMaps = new Dictionary<string, int>
            {
                {"Aurora f506438e8d_public", 8101},
                {"Aurora fbb3e7d1b4_public", 8059},
                {"Aurora 04768e5dce_public", 7931},
                {"Aurora 0ee3b2e0e2_public", 7841}, 
                {"Aurora b4367eba86_public", 7728}
            };

            /// <summary>
            /// Auth module's hash map for client platforms.
            /// </summary>
            public static Dictionary<MooNetClient.ClientPlatform, byte[]> AuthModuleHashMap = new Dictionary<MooNetClient.ClientPlatform, byte[]>()
            {
                { MooNetClient.ClientPlatform.Win,"8F52906A2C85B416A595702251570F96D3522F39237603115F2F1AB24962043C".ToByteArray() },
                { MooNetClient.ClientPlatform.Mac,"63BC118937E6EA2FAA7B7192676DAEB1B7CA87A9C24ED9F5ACD60E630B4DD7A4".ToByteArray() }
            };

            public static class Achievements
            {
                /// <summary>
                /// AchievementFile hash.
                /// </summary>
                //public static string AchievementFileHash = "ef29e59b9394e7c6f694afbb92b70a74c4fd4c96961a8ec490e770371b72e6ab";
                public static string AchievementFileHash = "0b61aeee74bba6ba02b93c9e15089404daf5d3cd1c7e631d7c108685894b3feb"; //8101

                /// <summary>
                /// AchievementFile filename.
                /// </summary>
                public static string AchievementFilename = AchievementFileHash + ".achv";

                /// <summary>
                /// AchievementFile download URL.
                /// </summary>
                public static string AchievementURL = "http://us.depot.battle.net:1119/" + AchievementFilename;

            }
        }

        /// <summary>
        /// MPQ storage versions info.
        /// </summary>
        public static class MPQ
        {
            /// <summary>
            /// Required MPQ patch version.
            /// </summary>
            public const int RequiredPatchVersion = 8101;
        }

        /// <summary>
        /// Ingame connection & client versions info.
        /// </summary>
        public static class Ingame
        {
            /// <summary>
            /// Ingame protocol hash.
            /// </summary>
            public const int ProtocolHash = unchecked((int)0xBA957E6B);

            // 0xBA957E6B                   // 8059, 8101
            // unchecked((int)0x208CA037)   // 7931
            // unchecked((int)0x208CA037)   // 7841
            // unchecked((int)0x9E121BBD)   // 7728
            // 0x21EEE08D                   // 7446

            public const string VersionString = "0.5.1.8101";

            // 0.5.1.8101 // 8101 patch 9
            // 0.5.0.8059 // 8059 patch 8
            // 0.3.1.7779 // 7728
            // 0.3.0.7484 // 7447    
            // 0.3.0.7333 // beta patch-3
        }
    }
}
