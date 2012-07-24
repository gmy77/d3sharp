﻿/*
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

namespace Mooege.Net.WebServices
{
    public sealed class Config : Common.Config.Config
    {        
        public bool Enabled { get { return this.GetBoolean("Enabled", true); } set { this.Set("Enabled", value); } }
        public string Address { get { return this.GetString("Address", ""); } set { this.Set("Address", value); } }
   
        private static readonly Config _instance = new Config();
        public static Config Instance { get { return _instance; } }
        private Config() : base("WebServices") { }
    }
}
