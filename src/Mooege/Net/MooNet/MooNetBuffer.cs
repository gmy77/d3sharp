using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mooege.Net.MooNet
{
    public class MooNetBuffer
    {
        private byte[] Data = new byte[0];
        public int Position
        {
            get;
            private set;
        }

        private int _length = 0;

        public int Length { 
         get {
           return _length - Position;
         } 
        }

        public MooNetBuffer()
        {
            this.Position = 0;
        }

        public bool PacketAvaliable()
        {
            if (Length < 2)
                return false;
            int headerSize = (Data[Position] << 8) | Data[Position+1]; // header size.
            if (Length < headerSize + 2)
                return false;

            byte[] tempData = new byte[headerSize];
            Array.Copy(Data, Position+2, tempData, 0, headerSize);

            var headerData = bnet.protocol.Header.ParseFrom(tempData); // header data.

            if (Length < headerData.Size + headerSize + 2)
                return false;
            
            return true;
        }

        public bnet.protocol.Header GetPacketHeader()
        {
            if (Length < 2)
                throw new Exception("GetPacketHeader when insuficient data avaliable");

            int headerSize = (Data[Position] << 8) | Data[Position + 1]; // header size.
            if (Length < headerSize + 2)
                throw new Exception("GetPacketHeader when insuficient data avaliable");

            byte[] tempData = new byte[headerSize];
            Array.Copy(Data, Position+2, tempData, 0, headerSize);

            var headerData = bnet.protocol.Header.ParseFrom(tempData); // header data.

            Position += 2 + headerSize;

            return headerData;
        }

        public byte[] GetPacketData(int count)
        {
            if (Length < count)
            {
                throw new Exception("GetPacketData when insuficient data avaliable");
            }

            byte[] tempData = new byte[count];
            Array.Copy(Data, Position, tempData, 0, count);

            Position += count;
            return tempData;
        }

        public void Consume()
        {
            Array.Copy(Data, Position, Data, 0, Length);

            _length = _length - Position;
            Position = 0;
        }

        public void Append(byte[] newdata)
        {
            if (Data.Length < _length + newdata.Length)
                Array.Resize(ref Data, _length + newdata.Length);

            Array.Copy(newdata, 0, Data, _length, newdata.Length);
            _length = _length + newdata.Length;
        }
    }
}
