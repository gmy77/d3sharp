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

namespace PonyLib
{
    public static class PatchInfo
    {
        #region current

        // Build 1.0.3.10057

        /// <summary>
        /// Offset for server-ip check.
        /// </summary>
        public const Int32 ServerIPCheckOffset = 0x000BC2CC;

        /// <summary>
        /// Offset for second challenge check.
        /// </summary>
        public const Int32 SecondChallengeCheckOffset = 0x000BC289;

        /// <summary>
        /// Required bnet module version.
        /// </summary>
        public const string RequiredBnetModuleVersion = "ab0ebd5e2c";

        #endregion

        #region old offsets

        // add previous offset information to here once you update current region!

        //1.2.9991
        //public const Int32 ServerIPCheckOffset = 0x000BC25C;
        //public const Int32 SecondChallengeCheckOffset = 0x000BC219;
        //public const string RequiredBnetModuleVersion = "24e2d13e54";

        // 1.0.2.9858, 1.0.2.9950
        //public const Int32 ServerIPCheckOffset = 0x000BA8A2;
        //public const Int32 SecondChallengeCheckOffset = 0x000BA863;

        // 1.0.2.9749
        //static Int32 offset = 0x000BA802;
        //static string version = "8018401a9c";

        // 1.0.1.9558
        //static Int32 offset = 0x000B5952;
        //static string version = "31c8df955a";

        // Beta 0.11.0.9327, Beta 0.11.0.9359
        //static Int32 offset = 0x000B5605;
        //static string version = "8eac7d44dc";

        // Beta 0.10.0.9183
        //static Int32 offset = 0x000B5505;

        // Beta 0.9.0.8896
        //static Int32 offset = 0x000B4475;
        //static string version = "bcd3e50524"; // DLL Battle.net Aurora bcd3e50524_public/329 (Mar 14 2012 10:28:16)

        #endregion
    }
}
