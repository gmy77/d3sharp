/*
 * Copyright (C) 2011 D3Sharp Project
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
using D3Sharp.Core.BNet.Services;
using D3Sharp.Net.BNet.Packets;
using D3Sharp.Utils;
using Google.ProtocolBuffers;

namespace D3Sharp.Net.BNet
{
    public static class BNetRouter
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
            var header = new BNetHeader(stream);
            var payload = new byte[header.PayloadLength];
            payload = stream.ReadRawBytes((int)header.PayloadLength);

            var packet = new BNetPacket(header, payload);
            var service = Service.GetByID(header.ServiceID);

            if (service == null)
            {
                Logger.Error("No service exists with id: 0x{0}", header.ServiceID.ToString("X2"));
                return;
            }

            //var method = service.DescriptorForType.Methods[(int)header.MethodID - 1];
            //Logger.Warn("METHODID: {0}, from header: {1}", (uint)method.Options[bnet.protocol.Rpc.MethodId.Descriptor], header.MethodID);
            var method = service.DescriptorForType.Methods.Single(m => (uint)m.Options[bnet.protocol.Rpc.MethodId.Descriptor] == header.MethodID);
            var proto = service.GetRequestPrototype(method);
            var builder = proto.WeakCreateBuilderForType();

            try
            {
                var message = builder.WeakMergeFrom(CodedInputStream.CreateInstance(packet.Payload.ToArray())).WeakBuild();
                lock (service) // lock the service so that it's in-context client does not get changed..
                {
                    //Logger.Debug("service-call data:{0}", message.ToString());
                    ((IServerService) service).Client = (IBNetClient)connection.Client;
                    service.CallMethod(method, null, message, (msg => SendResponse(connection, header.RequestID, msg)));
                }
            }
            catch (NotImplementedException)
            {
                Logger.Debug("Unimplemented service method: {0} {1}", service.GetType().Name, method.Name);
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
            var packet = new BNetPacket(
                new BNetHeader(0xfe, 0x0, requestId, (uint)message.SerializedSize, 0),
                message.ToByteArray());

            client.Send(packet);
        }
    }
}
