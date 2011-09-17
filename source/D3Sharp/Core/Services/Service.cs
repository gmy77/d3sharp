using System;
using System.Collections.Generic;
using System.Reflection;
using D3Sharp.Net;
using D3Sharp.Net.Packets;
using D3Sharp.Utils;

namespace D3Sharp.Core.Services
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ServiceAttribute : Attribute
    {
        public uint ServiceID { get; private set; }
        public uint ServerHash { get; private set; }
        public uint ClientHash { get; private set; }

        public ServiceAttribute(uint serviceID, uint serverHash, uint clientHash)
        {
            this.ServiceID = serviceID;
            this.ServerHash = serverHash;
            this.ClientHash = clientHash;
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class ServiceMethodAttribute: Attribute
    {
        public byte MethodID { get; set; }

        public ServiceMethodAttribute(byte methodID)
        {
            this.MethodID = methodID;
        }
    }

    public class Service
    {
        public static readonly Logger Logger = LogManager.CreateLogger();
        public Dictionary<uint, MethodInfo> Methods = new Dictionary<uint, MethodInfo>();

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
            //Console.WriteLine("[Client]: {0}:{1}", method.ReflectedType.FullName, method.Name);
            method.Invoke(this, new object[] {client, packet});
        }
    }
}
