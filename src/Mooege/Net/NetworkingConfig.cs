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

using Mooege.Common.Config;

namespace Mooege.Net
{
    public sealed class NetworkingConfig : Config
    {
        public bool EnableIPv6 { get { return this.GetBoolean("EnableIPv6", true); } set { this.Set("EnableIPv6", value); } }

        private static readonly NetworkingConfig _instance = new NetworkingConfig();
        public static NetworkingConfig Instance { get { return _instance; } }
        private NetworkingConfig() : base("Networking") { }
    }
}
