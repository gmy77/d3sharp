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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Description;
using Mooege.Common.Logging;

namespace Mooege.Net.WebServices
{
    public class ServiceManager
    {
        private static readonly Logger Logger = LogManager.CreateLogger();

        private readonly List<ServiceHost> _serviceHosts = new List<ServiceHost>();
        private readonly Dictionary<Type, ServiceContractAttribute> _webServices = new Dictionary<Type, ServiceContractAttribute>();

        public ServiceManager()
        {
            this.LoadServices();
        }

        private void LoadServices()
        {
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes().Where(type => type.GetInterface("IWebService") != null))
            {
                object[] attributes = type.GetCustomAttributes(typeof(ServiceContractAttribute), true); // get the attributes of the packet.
                if (attributes.Length == 0) return;

                _webServices.Add(type, (ServiceContractAttribute)attributes[0]);
            }
        }

        public void Run()
        {
            foreach (var pair in this._webServices)
            {
                var uri = new Uri(string.Format("{0}/{1}", Config.Instance.Address, pair.Value.Name));
                var serviceHost = new ServiceHost(pair.Key, uri);

                serviceHost.Description.Behaviors.Add(new ServiceMetadataBehavior { HttpGetEnabled = true });
                var debugBehavior = (ServiceDebugBehavior)serviceHost.Description.Behaviors[typeof(ServiceDebugBehavior)];
                debugBehavior.IncludeExceptionDetailInFaults = true;

                serviceHost.AddServiceEndpoint(typeof(IMetadataExchange), new BasicHttpBinding(), "Mex");
                serviceHost.AddServiceEndpoint(pair.Key, new BasicHttpBinding(), "");

                serviceHost.Open();
                this._serviceHosts.Add(serviceHost);
            }

            Logger.Info("Loaded web-services manager with {0} services..", this._webServices.Count);
        }
    }
}
