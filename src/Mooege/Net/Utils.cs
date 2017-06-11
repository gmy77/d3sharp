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

using System.Net;
using Mooege.Net.MooNet;

namespace Mooege.Net
{
    public static class Utils
    {
        public static string GetGameServerIPForClient(MooNetClient client)
        {
            if (!NATConfig.Instance.Enabled)
            {
                // if NAT is not enabled, just return bnetclient's localendpoint address.
                // Note: D3 client doesn't seem to accept IPv6 addresses as of patch 13! 
                // read more on IPv6 in server.cs. /raist.
                return client.Connection.LocalEndPoint.Address.ToString();
            }
            else
            {
                return client.Connection.LocalEndPoint.Address.ToString() == "127.0.0.1"
                           ? client.Connection.LocalEndPoint.ToString()
                           : NATConfig.Instance.PublicIP; // if client is not connected over localhost, send him public-ip.

                // Known problems: 
                // If user enables NAT, LAN-clients (and even local-computer if d3 is configured to use lan-ip) will not able to connect in gs.
                // That needs a full implementation similar to pvpgn where we currently pretty miss the time for /raist.
            }
        }
    }
}
