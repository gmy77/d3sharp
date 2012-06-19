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
            public const string Version = "1.10057.*";
        }

        /// <summary>
        /// MooNet versions info.
        /// </summary>
        public static class MooNet
        {
            /// <summary>
            /// Required client version.
            /// </summary>
            public const int RequiredClientVersion = 10057;

            public static Dictionary<string, int> ClientVersionMaps = new Dictionary<string, int>
            {
                {"Aurora ab0ebd5e2c_public", 10057},
                {"Aurora 24e2d13e54_public", 9991},
                {"Aurora 79fef7ae8e_public", 9950}, // also 9858
                {"Aurora 8018401a9c_public", 9749},
                {"Aurora 31c8df955a_public", 9558},
                {"Aurora 8eac7d44dc_public", 9359}, // also 9327
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
            /// Auth modules' hash maps for client platforms.
            /// </summary>
            //TODO: Get Hashes for Mac client.
            public static Dictionary<MooNetClient.ClientPlatform, byte[]> PasswordHashMap = new Dictionary<MooNetClient.ClientPlatform, byte[]>()
            {
                { MooNetClient.ClientPlatform.Win,"8F52906A2C85B416A595702251570F96D3522F39237603115F2F1AB24962043C".ToByteArray() },
                { MooNetClient.ClientPlatform.Mac,"63BC118937E6EA2FAA7B7192676DAEB1B7CA87A9C24ED9F5ACD60E630B4DD7A4".ToByteArray() }
            };

            public static Dictionary<MooNetClient.ClientPlatform, byte[]> ThumbprintHashMap = new Dictionary<MooNetClient.ClientPlatform, byte[]>()
            {
                { MooNetClient.ClientPlatform.Win,"36b27cd911b33c61730a8b82c8b2495fd16e8024fc3b2dde08861c77a852941c".ToByteArray() },
                { MooNetClient.ClientPlatform.Mac,"36b27cd911b33c61730a8b82c8b2495fd16e8024fc3b2dde08861c77a852941c".ToByteArray() },
            };

            public static Dictionary<MooNetClient.ClientPlatform, byte[]> TokenHashMap = new Dictionary<MooNetClient.ClientPlatform, byte[]>()
            {
                { MooNetClient.ClientPlatform.Win,"bfa574bcff509b3c92f7c4b25b2dc2d1decb962209f8c9c8582ddf4f26aac176".ToByteArray() },
                { MooNetClient.ClientPlatform.Mac,"bfa574bcff509b3c92f7c4b25b2dc2d1decb962209f8c9c8582ddf4f26aac176".ToByteArray() },
            };

            public static Dictionary<MooNetClient.ClientPlatform, byte[]> RiskFingerprintHashMap = new Dictionary<MooNetClient.ClientPlatform, byte[]>()
            {
                { MooNetClient.ClientPlatform.Win,"bcfa324ab555fc66614976011d018d2be2b9dc23d0b54d94a3bd7d12472aa107".ToByteArray() },
                { MooNetClient.ClientPlatform.Mac,"bcfa324ab555fc66614976011d018d2be2b9dc23d0b54d94a3bd7d12472aa107".ToByteArray() },
            };

            public static Dictionary<MooNetClient.ClientPlatform, byte[]> AgreementHashMap = new Dictionary<MooNetClient.ClientPlatform, byte[]>()
            {
                { MooNetClient.ClientPlatform.Win,"41686a009b345b9cbe622ded9c669373950a2969411012a12f7eaac7ea9826ed".ToByteArray() },
                { MooNetClient.ClientPlatform.Mac,"41686a009b345b9cbe622ded9c669373950a2969411012a12f7eaac7ea9826ed".ToByteArray() },
            };

            public static byte[] TOS = "00736F74006167726500005553014970E37CCD158A64A2844D6D4C05FC1697988A617E049BB2E0407D71B6C6F2".ToByteArray();
            public static byte[] EULA = "00616C75656167726500005553DDD1D77970291A4E8A64BB4FE25B2EA2D69D8915D35D53679AE9FDE5EAE47ECC".ToByteArray();
            public static byte[] RMAH = "0068616D72616772650000555398A3FC047004D6D4A0A1519A874AC9B1FC5FBD62C3EAA23188E095D6793537D7".ToByteArray();

            public static Dictionary<string, uint> Regions = new Dictionary<string, uint>()
            {
                { "US", 0x5553 },
                { "XX", 0x5858 }, //Beta Region
            };

            public static string Region = "US";

            public static class Resources
            {
                public static string ProfanityFilterHash = "de1862793fdbabb6eb1edec6ad1c95dd99e2fd3fc6ca730ab95091d694318a24"; //9558-10057
                public static string AvailableActs = "bd9e8fc323fe1dbc1ef2e0e95e46355953040488621933d0685feba5e1163a25"; //10057
                public static string AvailableQuests = "9303df8f917e2db14ec20724c04ea5d2af4e4cb6c72606b67a262178b7e18104"; //10057

                //public static string AvailableActs = "89dd44c90f3b7dca32bd7a289d5c09b253c1398b81e7dbf860cd5e635cb4a763"; //8815
                //public static string AvailableQuests = "e2aeeb41ad31eadd710f7e3729411b249195123d0a804a1b3bf18883f9011b04"; //8815
                //public static string ProfanityFilterHash = "068fec3c7426b8ba9497225a73437c6dffaa92de962c2b05589b5f46fbe5f5b0"; //8815
                //public static string ProfanityFilterHash = "1d9bdf93a409c93cd82a49670deccb36eca150c3f22ab2741666524a7368eb94"; //8896
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
                //public static string AchievementFileHash = "99b7ccad605818c95e965b21ce3bf35b8406202ea616f54705c4ebaf45c4c7f2"; //8896
                //public static string AchievementFileHash = "e3440d1a1430864371175afabb81e0b124c2824ea93def5d994cf8250cc1082b"; //9558
                public static string AchievementFileHash = "f0a945924510ece166812b241bd0724af5d0f1569e72430a67b46518fee37fb3"; //10057

                /// <summary>
                /// AchievementFile filename.
                /// </summary>
                public static string AchievementFilename = AchievementFileHash + ".achu";

                /// <summary>
                /// AchievementFile download URL.
                /// </summary>
                public static string AchievementURL = "http://" + MooNet.Region + ".depot.battle.net:1119/" + AchievementFilename;

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
            public const int RequiredPatchVersion = 10057;
        }

        /// <summary>
        /// Ingame connection & client versions info.
        /// </summary>
        public static class Ingame
        {
            /// <summary>
            /// Ingame protocol hash.
            /// </summary>
            public const int ProtocolHash = unchecked((int)0xFDD6012B); //10057

            // old hashes
            //unchecked((int)0xFDD6012B)    // 10057
            // 0x33CABB38                   // 9183, 9327, 9359, 9749, 9858, 9950, 9991
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
            public const string MajorVersion = "1.0.3";
            public const string ServerBuild = "10182";
            public const string VersionString = MajorVersion + "." + ServerBuild;

            // old version strings.
            // 1.0.2.9950   // 9950 Retail
            // 1.0.2.9858   // 9858 Retail
            // 1.0.2.9749   // 9749 Retail
            // 1.0.1.9558   // 9558 Retail
            // 0.11.0.9359  // 9359 Beta Patch 18
            // 0.11.0.9327  // 9327 Beta Patch 17
            // 0.10.0.9236  // 9183 Beta Patch 16
            // 0.9.0.8922   // 8896 Beta Patch 15
            // 0.8.0.8834   // 8815 Beta Patch 14
            // 0.7.0.8619   // 8610 Beta Patch 13
            // 0.6.2.8392   // 8392 Beta Patch 12
            // 0.6.1.8350   // 8350 Beta Patch 11
            // 0.6.0.8318   // 8296 Beta Patch 10
            // 0.5.1.8115   // 8101 Beta Patch 9
            // 0.5.0.8059   // 8059 Beta Patch 8
            // ??           // 7931 Beta Patch 7
            // ??           // 7841 Beta Patch 6
            // 0.3.1.7779   // 7728 Beta Patch 5
            // 0.3.0.7484   // 7447 Beta Patch 4
            // 0.3.0.7333   // 7733 Beta Patch 3
        }
    }
}