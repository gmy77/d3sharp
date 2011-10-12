/*
 * Copyright (C) 2011 mooege project
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
using System.Linq;
using Google.ProtocolBuffers;
using Mooege.Tools.ProtoDumper.Services;

namespace Mooege.Tools.ProtoDumper.Packet
{
    public static class MooNetRouter
    {
        public static void ReadProto(PcapDotNet.Packets.Datagram payload)
        {
            Console.Write('.');
            var stream = CodedInputStream.CreateInstance(payload.ToArray());
            while (!stream.IsAtEnd)
            {
                Identify(stream);
            }
        }

        public static void Identify(CodedInputStream stream)
        {
            try
            {
                var header = new MooNetHeader(stream);
                var payload = new byte[header.PayloadLength];

                payload = stream.ReadRawBytes((int) header.PayloadLength);

                var packet = new MoonNetPacket(header, payload);
                if (header.ServiceID == 0xfe) return;

                var service = Service.GetByID(header.ServiceID);
                if (service == null) return;

                var method =
                    service.DescriptorForType.Methods.Single(
                        m => (uint) m.Options[bnet.protocol.Rpc.MethodId.Descriptor] == header.MethodID);

                var proto = service.GetRequestPrototype(method);
                var builder = proto.WeakCreateBuilderForType();

                var message =
                    builder.WeakMergeFrom(CodedInputStream.CreateInstance(packet.Payload.ToArray())).WeakBuild();
                service.CallMethod(method, null, message, (msg => { }));
            }
            catch (UninitializedMessageException e)
            {
                Console.WriteLine("Failed to parse message: {0}", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
