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

using Mooege.Common.Logging;
using Mooege.Common.Versions;

namespace Mooege.Core.MooNet.Authentication
{
    public static class VersionChecker
    {
        private static readonly Logger Logger = LogManager.CreateLogger();

        public static bool Check(bnet.protocol.authentication.LogonRequest request)
        {
            var foundVersionMatch = VersionInfo.MooNet.ClientVersionMaps.ContainsKey(request.Version) ? true: false;
            var versionMatch = foundVersionMatch ? VersionInfo.MooNet.ClientVersionMaps[request.Version] : -1;

            Logger.Trace(
                "Client Info: user: {0} program: {1}  platform: {2} locale: {3} version: {4} [{5}]  app_version: {6}.",
                request.Email, request.Program, request.Platform, request.Locale, versionMatch != -1 ? versionMatch.ToString() : "Unknown", request.Version,
                request.ApplicationVersion);

            return versionMatch == VersionInfo.MooNet.RequiredClientVersion; // see if the client fits our required version.
        }
    }
}
