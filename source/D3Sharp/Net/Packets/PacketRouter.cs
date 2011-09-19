using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using D3Sharp.Core.Services;
using D3Sharp.Utils;
using D3Sharp.Utils.Extensions;
using Google.ProtocolBuffers;

namespace D3Sharp.Net.Packets
{
    public static class PacketRouter
    {
        private static readonly Logger Logger = LogManager.CreateLogger();

        public static void Route(ClientDataEventArgs e)
        {
            var stream = CodedInputStream.CreateInstance(e.Data.ToArray());
            while (!stream.IsAtEnd)
            {
                Identify(e.Client, stream);
            }
        }

        public static int Identify(IClient client, CodedInputStream stream)
        {
            var header = new Header(stream);
            var payload = new byte[header.PayloadLength];
            payload = stream.ReadRawBytes((int)header.PayloadLength);

            var packet = new Packet(header, payload);
            var service = Service.GetByID(header.ServiceID);                

            if (service != null)
            {
                service.CallMethod(header.MethodID, client, packet);
                return packet.Length;
            }

            Console.WriteLine("\n===========[Unknown Crap]===========\nHeader\t: {0}Payload\t: {1}", header.Data.Dump(), payload.Dump());
            return 0;
        }
    }
}
