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

namespace PonyPatcher.Utilities
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
            public const string Version = "1.9950.0.0";
        }

        /// <summary>
        /// MooNet versions info.
        /// </summary>
        public static class Client
        {
            /// <summary>
            /// Required client version.
            /// </summary>
            public const int RequiredClientVersion = 9950;

        }       
    }
}