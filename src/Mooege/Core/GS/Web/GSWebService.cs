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

using System.ServiceModel;
using Mooege.Net.WebServices;

namespace Mooege.Core.GS.Web
{
    /// <summary>
    /// Web service that can return statistics on online players and so.
    /// </summary>
    [ServiceContract(Name = "GS")]
    public class GSWebService : IWebService
    {
        /// <summary>
        /// Always returns true, so that clients can see if the gs-server is online.
        /// </summary>
        /// <returns>true</returns>
        [OperationContract]
        public bool Ping()
        {
            return true;
        }
    }
}
