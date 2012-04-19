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
            public const string Version = "1.9359.*";
        }

        /// <summary>
        /// MooNet versions info.
        /// </summary>
        public static class MooNet
        {
            /// <summary>
            /// Required client version.
            /// </summary>
            public const int RequiredClientVersion = 9359;

            public static Dictionary<string, int> ClientVersionMaps = new Dictionary<string, int>
            {
                {"Aurora 8eac7d44dc_public", 9359}, //9327
                {"Aurora _public", 9183},
                {"Aurora bcd3e50524_public", 8896},
                {"Aurora 4a39a60e1b_public", 8815},
                {"Aurora 7f06f1aabd_public", 8610},
                {"Aurora 9e9ccb8fdf_public", 8392},
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
                //{ MooNetClient.ClientPlatform.Win,"bfa574bcff509b3c92f7c4b25b2dc2d1decb962209f8c9c8582ddf4f26aac176".ToByteArray() },
                //{ MooNetClient.ClientPlatform.Win,"72dd40a65ccadc04fe4ece1323effd3177f4afb9f88a96905a7a30db42c0ae0f".ToByteArray() },
                { MooNetClient.ClientPlatform.Win,"8F52906A2C85B416A595702251570F96D3522F39237603115F2F1AB24962043C".ToByteArray() }, //2nd
                { MooNetClient.ClientPlatform.Mac,"63BC118937E6EA2FAA7B7192676DAEB1B7CA87A9C24ED9F5ACD60E630B4DD7A4".ToByteArray() }
            };

            public static Dictionary<MooNetClient.ClientPlatform, byte[]> ThumbprintHashMap = new Dictionary<MooNetClient.ClientPlatform, byte[]>()
            {
                { MooNetClient.ClientPlatform.Win,"36b27cd911b33c61730a8b82c8b2495fd16e8024fc3b2dde08861c77a852941c".ToByteArray() },
                { MooNetClient.ClientPlatform.Mac,"36b27cd911b33c61730a8b82c8b2495fd16e8024fc3b2dde08861c77a852941c".ToByteArray() },
            };

            public static uint Region = 0x5858; //XX
                //0x5553 //US

            public static class Resources
            {
                public static string ProfanityFilterHash = "1d9bdf93a409c93cd82a49670deccb36eca150c3f22ab2741666524a7368eb94"; //8896
                public static string AvailableActs = "89dd44c90f3b7dca32bd7a289d5c09b253c1398b81e7dbf860cd5e635cb4a763"; //8815
                public static string AvailableQuests = "e2aeeb41ad31eadd710f7e3729411b249195123d0a804a1b3bf18883f9011b04"; //8815

                //public static string ProfanityFilterHash = "068fec3c7426b8ba9497225a73437c6dffaa92de962c2b05589b5f46fbe5f5b0"; //8815
            }

            public static class Achievements
            {
                /// <summary>
                /// AchievementFile hash.
                /// </summary>
                //public static string AchievementFileHash = "9c1b0943a8e68352bb60ec872f35c645036feaabaac92ea13bee8b2f1dc9c5b9"; //8610
                //public static string AchievementFileHash = "ef29e59b9394e7c6f694afbb92b70a74c4fd4c96961a8ec490e770371b72e6ab"; // ??
                //public static string AchievementFileHash = "0b61aeee74bba6ba02b93c9e15089404daf5d3cd1c7e631d7c108685894b3feb"; //8101
                //public static string AchievementFileHash = "c06c3a43f760b9ef2c7965ac229531d17e93279cd2666bf1b9f130b8db5cb2f9"; //8296,8815
                public static string AchievementFileHash = "99b7ccad605818c95e965b21ce3bf35b8406202ea616f54705c4ebaf45c4c7f2"; //8896

                /// <summary>
                /// AchievementFile filename.
                /// </summary>
                public static string AchievementFilename = AchievementFileHash + ".achu";

                /// <summary>
                /// AchievementFile download URL.
                /// </summary>
                public static string AchievementURL = "http://XX.depot.battle.net:1119/" + AchievementFilename;

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
            public const int RequiredPatchVersion = 9359;
        }

        /// <summary>
        /// Ingame connection & client versions info.
        /// </summary>
        public static class Ingame
        {
            /// <summary>
            /// Ingame protocol hash.
            /// </summary>
            public const int ProtocolHash = 0x33CABB38;

            // old hashes
            // 0x33CABB38                   // 9183, 9327, 9359
            // unchecked((int)0x9726E2E3)   // 8896
            // 0x375AE194                   // 8815
            // unchecked((int)0xA8F17EC5)   // 8610
            // 0x01A64B41                   // 8296, 8350, 8392
            // 0xBA957E6B                   // 8059, 8101
            // unchecked((int)0x208CA037)   // 7931
            // unchecked((int)0x208CA037)   // 7841
            // unchecked((int)0x9E121BBD)   // 7728
            // 0x21EEE08D                   // 7446

            //This is the server version sent in VersionsMessage
            public const string MajorVersion = "0.11.0";
            public const string ServerBuild = "9359";
            public const string VersionString = MajorVersion + ServerBuild;

            // old version strings.
            // 0.11.0.9359  // 9359 patch 18
            // 0.11.0.9327  // 9327 patch 17
            // 0.10.0.9236  // 9183 patch 16
            // 0.9.0.8922   // 8896 patch 15
            // 0.8.0.8834   // 8815 patch 14
            // 0.7.0.8619   // 8610 patch 13
            // 0.6.2.8392   // 8392 patch 12
            // 0.6.1.8350   // 8350 patch 11
            // 0.6.0.8318   // 8296 patch 10
            // 0.5.1.8115   // 8101 patch 9
            // 0.5.0.8059   // 8059 patch 8
            // ??           // 7931 patch 7
            // ??           // 7841 patch 6
            // 0.3.1.7779   // 7728 patch 5
            // 0.3.0.7484   // 7447 patch 4
            // 0.3.0.7333   // 7733 patch 3
        }
    }
}