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
using System.Linq;
using System.ServiceModel;
using Mooege.Core.MooNet.Online;
using Mooege.Net.WebServices;

namespace Mooege.Core.MooNet.Web
{
    /// <summary>
    /// Web service that can return statistics on online players and so.
    /// </summary>
    [ServiceContract(Name = "MooNet")]
    public class MooNetWebService : IWebService
    {
        /// <summary>
        /// Always returns true, so that clients can see if the moonet-server is online.
        /// </summary>
        /// <returns>true</returns>
        [OperationContract]
        public bool Ping()
        {
            return true; // TODO: actually fix it so that it's aware of services being stopped /raist.
        }

        /// <summary>
        /// Returns the online players count.
        /// </summary>
        /// <returns>Online player count.</returns>
        [OperationContract]
        public int OnlinePlayersCount()
        {
            return PlayerManager.OnlinePlayers.Count;
        }

        /// <summary>
        /// Returns the online players list.
        /// </summary>
        /// <returns>Online players list.</returns>
        [OperationContract]
        public List<string> OnlinePlayersList()
        {
            return (from client in PlayerManager.OnlinePlayers where client.Account.CurrentGameAccount.CurrentToon != null select string.Format("{0}", client.Account.CurrentGameAccount.CurrentToon.Name)).ToList();
        }
    }
}
