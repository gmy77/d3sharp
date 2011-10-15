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
using Google.ProtocolBuffers.Descriptors;
using Mooege.Common;
using Mooege.Core.MooNet.Services;
using Mooege.Net.MooNet.Packets;

namespace Mooege.Net.MooNet
{
    public static class MooNetRouter
    {
        private static readonly Logger Logger = LogManager.CreateLogger();
        public const byte ServiceReply = 0xFE;

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
            var client = (MooNetClient) connection.Client;
            var packet = new PacketIn(stream);
                  
            if(packet.ServiceId==ServiceReply)
            {
                var callback = client.RPCCallbacks.Dequeue();
                
                if (callback.RequestId == packet.RequestId) callback.Action(packet.ReadMessage(callback.Builder));
                else Logger.Warn("RPC callback contains unexpected requestId: {0} where {1} was expected", callback.RequestId, packet.RequestId);
                return;
            }
            
            var service = Service.GetByID(packet.ServiceId);

            if (service == null)
            {
                Logger.Error("No service exists with id: 0x{0}", packet.ServiceId.ToString("X2"));
                return;
            }

            var method = service.DescriptorForType.Methods.Single(m => GetMethodId(m) == packet.MethodId);
            var proto = service.GetRequestPrototype(method);
            var message = packet.ReadMessage(proto.WeakToBuilder());

            try
            {
                lock (service) // lock the service so that its in-context client does not get changed..
                {
                    ((IServerService)service).Client = client;
                    service.CallMethod(method, null, message, (msg => SendRPCResponse(connection, packet.RequestId, msg)));
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

        public static uint GetMethodId(MethodDescriptor method)
        {
            return (uint)method.Options[bnet.protocol.Rpc.MethodId.Descriptor];
        }

        private static void SendRPCResponse(IConnection connection, int requestId, IMessage message)
        {
            var packet = new PacketOut(ServiceReply, 0x0, requestId, message);
            connection.Send(packet);
        }
    }
}
