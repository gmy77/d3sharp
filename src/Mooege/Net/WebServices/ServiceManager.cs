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

using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using Mooege.Common.Logging;

namespace Mooege.Net.WebServices
{
    public class ServiceManager
    {
        private static readonly Logger Logger = LogManager.CreateLogger();
        private ServiceHost _onlineServiceHost;

        public ServiceManager()
        { }

        public void Run()
        {
            var serviceUri = new Uri("http://localhost:9000/");
            this._onlineServiceHost = new ServiceHost(typeof(OnlineService), serviceUri);

            var behavior = new ServiceMetadataBehavior();
            behavior.HttpGetEnabled = true;
            this._onlineServiceHost.Description.Behaviors.Add(behavior);

            this._onlineServiceHost.AddServiceEndpoint(typeof(OnlineService), new BasicHttpBinding(), "OnlineService");
            this._onlineServiceHost.AddServiceEndpoint(typeof(IMetadataExchange), new BasicHttpBinding(), "MEX");

            _onlineServiceHost.Open();

            Logger.Info("Webservices server started...");
        }
    }
}
