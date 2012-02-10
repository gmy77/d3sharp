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

using Google.ProtocolBuffers;

namespace Mooege.Net.MooNet.Packets
{
    public class PacketIn
    {
        public MooNetClient Client {get; private set;}
        public CodedInputStream Stream {get; private set;}

        public bnet.protocol.Header Header {get; private set;}

        public PacketIn(MooNetClient client, CodedInputStream stream)
        {
            this.Client = client;
            this.Stream = stream;

            this.Read();
        }
       
        private void Read()
        {
            var size = (this.Stream.ReadRawByte() << 8) | this.Stream.ReadRawByte(); // header size.
            var headerData = this.Stream.ReadRawBytes(size); // header data.
            this.Header = bnet.protocol.Header.ParseFrom(headerData);  // parse header. 
        }

        public IMessage ReadMessage(IBuilder builder)
        {
            return builder.WeakMergeFrom(CodedInputStream.CreateInstance(this.GetPayload(Stream))).WeakBuild();
            
            // this._stream.ReadMessage(builder, ExtensionRegistry.Empty); // this method doesn't seem to work with 7728. /raist.
            // return builder.WeakBuild();
        }

        public byte[] GetPayload(CodedInputStream stream)
        {
            return stream.ReadRawBytes((int)this.Header.Size);
        }

        public override string ToString()
        {
            return this.Header.ToString();
            //return string.Format("[S]: 0x{0}, [M]: 0x{1}, [R]: 0x{2}, [O]: 0x{3}", this.ServiceId.ToString("X2"), this.MethodId.ToString("X2"), this.RequestId.ToString("X2"), this.ObjectId.ToString("X2"));
        }
    }
}
