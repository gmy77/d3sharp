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
using Google.ProtocolBuffers;
using bnet.protocol;
using bnet.protocol.connection;

namespace Extractor.Services
{
    [Service(serviceID: 0x0, serviceHash: 0x0)]
    public class BaseService :  ConnectionService
    {
        public override void Connect(IRpcController controller, ConnectRequest request, Action<ConnectResponse> done)
        {
            ProtoOutputBuffer.Write(request.GetType(), request.ToString());
        }

        public override void Bind(IRpcController controller, BindRequest request, Action<BindResponse> done)
        {
            ProtoOutputBuffer.Write(request.GetType(), request.ToString());
        }

        public override void Echo(IRpcController controller, EchoRequest request, Action<EchoResponse> done)
        {
            ProtoOutputBuffer.Write(request.GetType(), request.ToString());
        }

        public override void ForceDisconnect(IRpcController controller, DisconnectNotification request, Action<NO_RESPONSE> done)
        {
            ProtoOutputBuffer.Write(request.GetType(), request.ToString());
        }

        public override void Null(IRpcController controller, NullRequest request, Action<NO_RESPONSE> done)
        {
            ProtoOutputBuffer.Write(request.GetType(), request.ToString());
        }

        public override void Encrypt(IRpcController controller, EncryptRequest request, Action<NoData> done)
        {
            ProtoOutputBuffer.Write(request.GetType(), request.ToString());
        }

        public override void RequestDisconnect(IRpcController controller, DisconnectRequest request, Action<NO_RESPONSE> done)
        {
            ProtoOutputBuffer.Write(request.GetType(), request.ToString());
        }
    }
}
