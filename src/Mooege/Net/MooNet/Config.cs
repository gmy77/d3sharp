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

namespace Mooege.Net.MooNet
{
    public sealed class Config : Common.Config.Config
    {
        public string BindIP { get { return this.GetString("BindIP", "0.0.0.0"); } set { this.Set("BindIP", value); } }
        public string BindIPv6 { get { return this.GetString("BindIPv6", "::1"); } set { this.Set("BindIPv6", value); } }
        public int Port { get { return this.GetInt("Port", 1345); } set { this.Set("Port", value); } }
        public string MOTD { get { return this.GetString("MOTD", ""); } set { this.Set("MOTD", value); } }

        private static readonly Config _instance = new Config();
        public static Config Instance { get { return _instance; } }
        private Config() : base("MooNet-Server") { }
    }
}
