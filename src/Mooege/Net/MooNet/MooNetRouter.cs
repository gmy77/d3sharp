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
using Mooege.Common;
using Mooege.Core.MooNet.Services;
using Mooege.Net.MooNet.Packets;

namespace Mooege.Net.MooNet
{
    public static class MooNetRouter
    {
        private static readonly Logger Logger = LogManager.CreateLogger();

        public static void Route(ConnectionDataEventArgs e)
        {
            var stream = CodedInputStream.CreateInstance(e.Data.ToArray());
            while (!stream.IsAtEnd)
            {
                Identify(e.Connection, stream);
            }
        }

        public static void Identify(IConnection connection, CodedInputStream stream)
        {
            var header = new Header(stream);
            var payload = new byte[header.PayloadLength];
            payload = stream.ReadRawBytes((int)header.PayloadLength);

            var packet = new Packet(header, payload);
            var service = Service.GetByID(header.ServiceID);

            if (service == null)
            {
                Logger.Error("No service exists with id: 0x{0}", header.ServiceID.ToString("X2"));
                return;
            }

            var method = service.DescriptorForType.Methods.Single(m => (uint)m.Options[bnet.protocol.Rpc.MethodId.Descriptor] == header.MethodID);
            var proto = service.GetRequestPrototype(method);
            var builder = proto.WeakCreateBuilderForType();

            try
            {
                var message = builder.WeakMergeFrom(CodedInputStream.CreateInstance(packet.Payload.ToArray())).WeakBuild();
                lock (service) // lock the service so that its in-context client does not get changed..
                {
                    //Logger.Debug("service-call data:{0}", message.ToString());
                    ((IServerService) service).Client = (IMooNetClient)connection.Client;
                    service.CallMethod(method, null, message, (msg => SendResponse(connection, header.RequestID, msg)));
                }
            }
            catch (NotImplementedException)
            {
                Logger.Warn("Unimplemented service method: {0}.{1}", service.GetType().Name, method.Name);
            }
            catch (UninitializedMessageException e)
            {
                Logger.Debug("Failed to parse message: {0}", e.Message);
            }
            catch (Exception e)
            {
                Logger.DebugException(e, string.Empty);
            }
        }

        private static void SendResponse(IConnection client, int requestId, IMessage message)
        {
            var packet = new Packet(
                new Header(0xfe, 0x0, requestId, (uint)message.SerializedSize, 0),
                message.ToByteArray());

            client.Send(packet);
        }
    }
}
