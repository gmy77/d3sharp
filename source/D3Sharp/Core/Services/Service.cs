using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using D3Sharp.Net;
using D3Sharp.Net.Packets;

namespace D3Sharp.Core.Services
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ServiceAttribute : Attribute
    {
        public uint ServiceID { get; set; }
        public uint ServiceHash { get; set; }

        public ServiceAttribute(uint serviceID, uint serviceHash)
        {
            this.ServiceID = serviceID;
            this.ServiceHash = serviceHash;
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
            if (!this.Methods.ContainsKey(methodID)) return;

            var method = this.Methods[methodID];
            Console.WriteLine("[Client Call]: {0}:{1}\n{2}\n", method.ReflectedType.FullName, method.Name,packet);
            method.Invoke(this, new object[] {client, packet});
        }
    }
}
