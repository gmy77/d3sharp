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
    public sealed class NATConfig : Config
    {
        public bool Enabled { get { return this.GetBoolean("Enabled", true); } set { this.Set("Enabled", value); } }
        public string PublicIP { get { return this.GetString("PublicIP", "0.0.0.0"); } set { this.Set("PublicIP", value); } }

        private static readonly NATConfig _instance = new NATConfig();
        public static NATConfig Instance { get { return _instance; } }
        private NATConfig() : base("NAT") { }
    }
}
