using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using D3Sharp.Net;
using D3Sharp.Net.Packets;
using D3Sharp.Utils.Helpers;

namespace D3Sharp.Core.Services
{
    public class ClientService
    {
        public static ClientServiceDefinitition[] ServiceDefinitions = new[]
                  {
                      new ClientServiceDefinitition("bnet.protocol.channel.ChannelSubscriber")
                  };

        public static ClientServiceDefinitition GetDefinitionByName(string name)
        {
            return ServiceDefinitions.FirstOrDefault(service => service.Name == name);
        }
    }

    public class ClientServiceDefinitition
    {
        public string Name { get; private set; }
        public uint Hash { get; private set; }

        public ClientServiceDefinitition(string name)
        {
            this.Name = name;
            this.Hash = StringHashHelper.HashString(name);
        }
    }
}
