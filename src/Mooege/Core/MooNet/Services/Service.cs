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
using Google.ProtocolBuffers;
using Mooege.Common.Helpers.Hash;

namespace Mooege.Core.MooNet.Services
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ServiceAttribute : Attribute
    {
        public uint ServiceID { get; private set; }
        public uint Hash { get; private set; }

        public ServiceAttribute(uint serviceID, uint serviceHash)
        {
            this.ServiceID = serviceID;
            this.Hash = serviceHash;
        }

        public ServiceAttribute(uint serviceID, string serviceName)
            : this(serviceID, (uint)StringHashHelper.HashIdentity(serviceName))
        {
        }
    }

    public static class Service
    {
        private static uint _notImplementedServiceCounter = 99;
        public readonly static Dictionary<Type, ServiceAttribute> ProvidedServices = new Dictionary<Type, ServiceAttribute>();
        public readonly static Dictionary<Type, IService> Services = new Dictionary<Type, IService>();

        static Service()
        {
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes().Where(type => type.GetInterface("IServerService") != null))
            {
                object[] attributes = type.GetCustomAttributes(typeof(ServiceAttribute), true); // get the attributes of the packet.
                if (attributes.Length == 0) return;

                ProvidedServices.Add(type, (ServiceAttribute)attributes[0]);
                Services.Add(type, (IService)Activator.CreateInstance(type));
            }
        }

        public static IService GetByID(uint serviceID)
        {
            return (from pair in ProvidedServices let serviceInfo = pair.Value where serviceInfo.ServiceID == serviceID select Services[pair.Key]).FirstOrDefault();
        }

        public static uint GetByHash(uint serviceHash)
        {
            foreach (var serviceInfo in ProvidedServices.Select(pair => pair.Value).Where(serviceInfo => serviceInfo.Hash == serviceHash))
            {
                return serviceInfo.ServiceID;
            }

            return _notImplementedServiceCounter++;
        }
    }
}
