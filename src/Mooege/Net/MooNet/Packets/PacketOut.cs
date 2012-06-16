/*
 * Copyright (C) 2011 - 2012 mooege project - http://www.mooege.org
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

using System.IO;
using Google.ProtocolBuffers;
using Mooege.Common.Logging;

namespace Mooege.Net.MooNet.Packets
{
    public class PacketOut
    {
        public byte[] Data { get; private set; }
        private static readonly Logger Logger = LogManager.CreateLogger();

        public PacketOut(byte serviceId, uint methodId, uint token, IMessage message, uint status)
            : this(serviceId, methodId, token, 0x0, message, status)
        {
        }

        public PacketOut(byte serviceId, uint methodId, uint token, ulong objectId, IMessage message, uint status = 0)
        {
            var builder = bnet.protocol.Header.CreateBuilder();

            if (status > 0)
            {
                builder.SetServiceId(MooNetRouter.ServiceReply)
                    .SetStatus(status);
            }
            else
                builder.SetServiceId(serviceId);

            builder.SetToken(token) // requestId.
                .SetSize((uint)message.SerializedSize);

            if (serviceId != MooNetRouter.ServiceReply)
                builder.SetMethodId(methodId);

            if (serviceId != MooNetRouter.ServiceReply && objectId != 0x0)
                builder.SetObjectId(objectId);

            var header = builder.Build();
            var headerSize = (short)(header.SerializedSize);

            using (var stream = new MemoryStream())
            {
                var output = CodedOutputStream.CreateInstance(stream);

                output.WriteRawByte((byte)(headerSize >> 8));
                output.WriteRawByte((byte)((headerSize & 0xff)));

                header.WriteTo(output);
                message.WriteTo(output);

                output.Flush();
                this.Data = stream.ToArray();
                Logger.LogOutgoingPacket(message, header);
            }
        }
    }
}
