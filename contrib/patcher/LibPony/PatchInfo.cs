using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PonyLib
{
    public static class PatchInfo
    {
        #region current

        // Build 1.0.2.9858 
        // Build 1.0.2.9950

        /// <summary>
        /// Offset for server-ip checkç
        /// </summary>
        public const Int32 ServerIPCheckOffset = 0x000BC25C;

        /// <summary>
        /// Offset for second challenge check.
        /// </summary>
        public const Int32 SecondChallengeCheckOffset = 0x000BC219;

        /// <summary>
        /// Required bnet module version.
        /// </summary>
        public const string RequiredBnetModuleVersion = "24e2d13e54";

        #endregion

        #region old offsets

        // Build 1.0.2.9858 
        // Build 1.0.2.9950
        //public const Int32 ServerIPCheckOffset = 0x000BA8A2;
        //public const Int32 SecondChallengeCheckOffset = 0x000BA863;

        // Build 1.0.2.9749
        //static Int32 offset = 0x000BA802;
        //static string version = "8018401a9c";

        // Build 1.0.1.9558
        //static Int32 offset = 0x000B5952;
        //static string version = "31c8df955a";

        //Beta 0.11.0.9327
        //Beta 0.11.0.9359
        //static Int32 offset = 0x000B5605;
        //static string version = "8eac7d44dc";

        //Beta 0.10.0.9183
        //static Int32 offset = 0x000B5505;

        //Beta 0.9.0.8896
        //static Int32 offset = 0x000B4475;
        //static string version = "bcd3e50524"; // DLL Battle.net Aurora bcd3e50524_public/329 (Mar 14 2012 10:28:16)

        #endregion
    }
}
