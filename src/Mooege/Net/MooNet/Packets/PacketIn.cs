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

using Google.ProtocolBuffers;

namespace Mooege.Net.MooNet.Packets
{
    public class PacketIn
    {
        private readonly CodedInputStream _stream = null;

        public byte ServiceId { get; private set; }
        public uint MethodId { get; private set; }
        public int RequestId { get; private set; }
        public ulong ObjectId { get; private set; }

        public PacketIn(CodedInputStream stream)
        {
            this._stream = stream;

            this.ServiceId = stream.ReadRawByte();
            this.MethodId = stream.ReadRawVarint32();
            this.RequestId = stream.ReadRawByte() | (stream.ReadRawByte() << 8);

            this.ObjectId = 0UL;
            if (this.ServiceId != 0xfe) this.ObjectId = stream.ReadRawVarint64();
        }

        public IMessage ReadMessage(IBuilder builder)
        {
            this._stream.ReadMessage(builder, ExtensionRegistry.Empty);
            return builder.WeakBuild();
        }

        public byte[] GetPayload()
        {
            var payloadLength = this._stream.ReadRawVarint32();
            return this._stream.ReadRawBytes((int)payloadLength);
        }

        public override string ToString()
        {
            return string.Format("[S]: 0x{0}, [M]: 0x{1}, [R]: 0x{2}, [O]: 0x{3}", this.ServiceId.ToString("X2"), this.MethodId.ToString("X2"), this.RequestId.ToString("X2"), this.ObjectId.ToString("X2"));
        }
    }
}
