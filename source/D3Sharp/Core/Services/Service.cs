using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using D3Sharp.Net;
using D3Sharp.Net.Packets;
using D3Sharp.Utils;
using D3Sharp.Utils.Helpers;

namespace D3Sharp.Core.Services
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
            : this(serviceID, StringHashHelper.HashString(serviceName))
        {
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class ServiceMethodAttribute : Attribute
    {
        public byte MethodID { get; set; }

        public ServiceMethodAttribute(byte methodID)
        {
            this.MethodID = methodID;
        }
    }

    public class Service
    {
        protected static readonly Logger Logger = LogManager.CreateLogger();

        private static uint _notImplementedServiceCounter = 99;
        private readonly static Dictionary<Type, ServiceAttribute> ProvidedServices = new Dictionary<Type, ServiceAttribute>();
        private readonly static Dictionary<Type, Service> Services = new Dictionary<Type, Service>();
        
        public readonly Dictionary<uint, MethodInfo> Methods = new Dictionary<uint, MethodInfo>();

        public Service()
        {
            this.LoadMethods();
        }

        private void LoadMethods()
        {
            foreach (var methodInfo in this.GetType().GetMethods())
            {
                var attribute = Attribute.GetCustomAttribute(methodInfo, typeof(ServiceMethodAttribute));
                if (attribute == null) continue;

                this.Methods.Add(((ServiceMethodAttribute)attribute).MethodID, methodInfo);
            }
        }

        public void CallMethod(uint methodID, IClient client, Packet packet)
        {
            if (!this.Methods.ContainsKey(methodID))
            {
                Console.WriteLine("Unknown method 0x{0:x2} called on {1} ", methodID, this.GetType());
                return;
            }

            var method = this.Methods[methodID];
            method.Invoke(this, new object[] { client, packet });
        }

        static Service()
        {
            foreach (Type type in Assembly.GetEntryAssembly().GetTypes().Where(t => t.IsSubclassOf(typeof(Service))))
            {
                object[] attributes = type.GetCustomAttributes(typeof(ServiceAttribute), true); // get the attributes of the packet.
                if (attributes.Length == 0) return;

                ProvidedServices.Add(type, (ServiceAttribute)attributes[0]);
                Services.Add(type, (Service)Activator.CreateInstance(type));
            }
        }

        public static Service GetByID(uint serviceID)
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
