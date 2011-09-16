using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using D3Sharp.Core.Services;
using D3Sharp.Utils.Extensions;

namespace D3Sharp.Net.Packets
{
    public static class PacketRouter
    {
        public static void Route(ClientDataEventArgs e)
        {            
            var buffer = e.Data.ToArray();
            // handle data as a stream -- a single packet can contain multiple messages
            while (buffer.Length > 0)
            {
                var bytesConsumed = Identify(e.Client, buffer);
                if (bytesConsumed <= 0)
                    return;

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
            // if our packet contains a payload, get it.
            if (header.PayloadLength > 0)
                Array.Copy(buffer, 6, payload, 0, header.PayloadLength);


            var packet = new Packet(header, payload);
            var service = ServiceManager.GetServerServiceByID(header.ServiceID);

            if(service!=null)
            {
                service.CallMethod(header.MethodID, client, packet);
                return packet.Lenght;
            }

            Console.WriteLine("\n===========[Unknown Crap]===========\nHeader\t: {0}Payload\t: {1}", header.Data.Dump(), payload.Dump());
            return 0;
        }
    }
}
