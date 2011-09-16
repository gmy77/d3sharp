using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace D3Sharp.Core.Services
{
    public static class ServiceManager
    {
        public static Dictionary<Type, ServiceAttribute> ProvidedServices = new Dictionary<Type, ServiceAttribute>();
        public static Dictionary<Type, Service> Services = new Dictionary<Type, Service>();

        static ServiceManager()
        {
            foreach (Type t in Assembly.GetEntryAssembly().GetTypes())
            {
                if (!t.IsSubclassOf(typeof(Service)))
                    continue;
                ReadServiceInfo(t);
            }
        }

        public static Service GetServerServiceByID(uint serviceID)
        {
            return (from pair in ProvidedServices let serviceInfo = pair.Value where serviceInfo.ServiceID == serviceID select Services[pair.Key]).FirstOrDefault();
        }

        private static uint _notImplementedServiceCounter = 99;

        public static uint GetServerServiceIDByHash(uint serviceHash)
        {
            foreach (var serviceInfo in ProvidedServices.Select(pair => pair.Value).Where(serviceInfo => serviceInfo.ServerHash == serviceHash))
            {
                return serviceInfo.ServiceID;
            }

            return _notImplementedServiceCounter++;
        }

        private static void ReadServiceInfo(Type type)
        {
            object[] attributes = type.GetCustomAttributes(typeof(ServiceAttribute), true); // get the attributes of the packet.
            if (attributes.Length == 0) return;

            ProvidedServices.Add(type, (ServiceAttribute)attributes[0]);
            Services.Add(type, (Service)Activator.CreateInstance(type));
        }
    }
}
