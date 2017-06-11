using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mooege.Net.MooNet
{
    public class MooNetBuffer
    {
        public int Position { get; private set; }

        public int Length
        {
            get { return _length - Position; }
        }

        private byte[] _data = new byte[0];
        private int _length = 0;

        public MooNetBuffer()
        {
            this.Position = 0;
        }

        public bool PacketAvaliable()
        {
            if (Length < 2)
                return false;

            int headerSize = (_data[Position] << 8) | _data[Position + 1]; // header size.

            if (Length < headerSize + 2)
                return false;

            var tempData = new byte[headerSize];
            Array.Copy(_data, Position + 2, tempData, 0, headerSize);

            var headerData = bnet.protocol.Header.ParseFrom(tempData); // header data.

            if (Length < headerData.Size + headerSize + 2)
                return false;

            return true;
        }

        public bnet.protocol.Header GetPacketHeader()
        {
            if (Length < 2)
                throw new Exception("GetPacketHeader when insuficient data avaliable");

            int headerSize = (_data[Position] << 8) | _data[Position + 1]; // header size.

            if (Length < headerSize + 2)
                throw new Exception("GetPacketHeader when insuficient data avaliable");

            var tempData = new byte[headerSize];
            Array.Copy(_data, Position + 2, tempData, 0, headerSize);

            var headerData = bnet.protocol.Header.ParseFrom(tempData); // header data.
            Position += 2 + headerSize;

            return headerData;
        }

        public byte[] GetPacketData(int count)
        {
            if (Length < count)
                throw new Exception("GetPacketData called when insuficient data avaliable!");

            var tempData = new byte[count];
            Array.Copy(_data, Position, tempData, 0, count);

            Position += count;
            return tempData;
        }

        public void Consume()
        {
            Array.Copy(_data, Position, _data, 0, Length);

            _length = _length - Position;
            Position = 0;
        }

        public void Append(byte[] newdata)
        {
            if (_data.Length < _length + newdata.Length)
                Array.Resize(ref _data, _length + newdata.Length);

            Array.Copy(newdata, 0, _data, _length, newdata.Length);
            _length = _length + newdata.Length;
        }
    }
}
