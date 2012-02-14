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
using System.Reflection;
using System.ServiceModel;
using Mooege.Net.WebServices;

namespace Mooege.Core.Web
{
    /// <summary>
    /// Web service that can return statistics from the mooege core.
    /// </summary>
    [ServiceContract(Name = "Core")]
    public class CoreWebService : IWebService
    {
        /// <summary>
        /// Returns main mooege assembly version.
        /// </summary>
        /// <returns>true</returns>
        [OperationContract]
        public string Version()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        /// <summary>
        /// Returns uptime.
        /// </summary>
        /// <returns>true</returns>
        [OperationContract]
        public string Uptime()
        {
            // TODO: return unix-time based value. /raist.

            var uptime = DateTime.Now - Program.StartupTime;
            return string.Format("{0} days, {1} hours, {2} minutes, {3} seconds.", uptime.Days, uptime.Hours, uptime.Minutes, uptime.Seconds);
        }
    }
}
