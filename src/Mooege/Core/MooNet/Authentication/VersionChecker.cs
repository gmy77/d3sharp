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

using Mooege.Common.Logging;
using Mooege.Common.Versions;
using Mooege.Net.MooNet;

namespace Mooege.Core.MooNet.Authentication
{
    public static class VersionChecker
    {
        private static readonly Logger Logger = LogManager.CreateLogger();

        public static bool Check(MooNetClient client, bnet.protocol.authentication.LogonRequest request)
        {
            int versionMatch = -1;
            string clientVersionSignature = request.HasVersion ? request.Version.Substring(0, 24) : string.Empty; // get client's version string signature - the very first 24 chars like; "Aurora b4367eba86_public".

            foreach(var pair in VersionInfo.MooNet.ClientVersionMaps) // see if client's version signature matches anyone in our client versions map.
            {
                if (pair.Key != clientVersionSignature)
                    continue;

                versionMatch = pair.Value;
                break;
            }

            // set client platform.
            switch (request.Platform.ToLower())
            {
                case "win":
                    client.Platform = MooNetClient.ClientPlatform.Win;
                    break;
                case "mac":
                    client.Platform = MooNetClient.ClientPlatform.Mac;
                    break;
                default:
                    client.Platform = MooNetClient.ClientPlatform.Invalid;
                    break;
            }

            // set client locale
            switch(request.Locale)
            {
                case "enUS":
                    client.Locale = MooNetClient.ClientLocale.enUS;
                    break;
                default:
                    client.Locale = MooNetClient.ClientLocale.Invalid;
                    break;
            }

            Logger.Trace("Client Info: user: {0} program: {1}  platform: {2} locale: {3} version: {4} [{5}]  app_version: {6}.",
                request.Email, request.Program, request.Platform, request.Locale, versionMatch != -1 ? versionMatch.ToString() : "Unknown", request.Version,request.ApplicationVersion);

            return versionMatch == VersionInfo.MooNet.RequiredClientVersion; // see if the client fits our required version.
        }
    }
}
