using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using D3Sharp.Utils.Extensions;

namespace D3Sharp.Net.Packets
{
    public static class Parser
    {
        public static Dictionary<Type, ServiceAttribute> Packets = new Dictionary<Type, ServiceAttribute>();
       
        static Parser()
        {
            foreach (Type t in Assembly.GetEntryAssembly().GetTypes())
            {
                if (!t.IsSubclassOf(typeof(PacketIn))) continue; // check if type is a subclass 'HardcodedPacket'.
                ReadPacketInfo(t);
            }
        }

        private static void ReadPacketInfo(Type type)
        {
            object[] attr = type.GetCustomAttributes(typeof(ServiceAttribute), true); // get the attributes of the packet.
            if (attr.Length == 0) return;

            Packets.Add(type, (ServiceAttribute)attr[0]);            
        }

        public static void Parse(ClientDataEventArgs e)
        {
            var buffer = e.Data.ToArray();

            while (buffer.Length > 0) // we may have recieved more than one packets actually, process them all..
            {
                var bytesConsumed = Identify(e.Client, buffer);
                if (bytesConsumed <= 0) return;

                var bytesLeft=buffer.Length - bytesConsumed;
                var tmp = new byte[bytesLeft];
                Array.Copy(buffer, bytesConsumed, tmp, 0, bytesLeft);
                buffer = tmp;
            }
        }

        public static int Identify(IClient client, byte[] buffer)
        {
            var header = new Header(buffer.Take(6));
            var payload = new byte[header.PayloadLength];
            if (header.PayloadLength > 0) Array.Copy(buffer, 6, payload, 0, header.PayloadLength); // if our packet contains a payload, get it.


            if(Server.Services.ContainsKey(header.ServiceID))
            {
                var serviceHash = Server.Services[header.ServiceID];
                foreach(var pair in Packets)
                {
                    if(pair.Value.ServiceHash==serviceHash && pair.Value.Method == header.Method)
                    {
                        var packet = Activator.CreateInstance(pair.Key, new object[] {header, payload});
                        client.Process((PacketIn)packet);
                        return header.Data.Length + payload.Length;
                    }
                }
            }

            Console.WriteLine("\n===========[Unknown Crap]===========\nHeader\t: {0}Payload\t: {1}", header.Data.Dump(), payload.Dump());
            return 0;
        }
    }
}
