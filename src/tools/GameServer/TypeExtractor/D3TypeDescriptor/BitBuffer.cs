﻿/*
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
 using System.Text;

namespace D3TypeDescriptor
{
    public class ProtocolBitBufferException : Exception
    {
        public ProtocolBitBufferException(string msg) : base(msg) { }
    }

    public class BitBuffer
    {
        public byte[] Data;
        public int Length;
        public int Position;

        public BitBuffer(byte[] data, int position, int length)
        {
            Data = data;
            Position = position;
            Length = length;
        }

        public BitBuffer(byte[] data)
        {
            Data = data;
            Position = 0;
            Length = data.Length * 8;
        }

        public BitBuffer(int byteCapacity)
        {
            Data = new byte[byteCapacity];
            Position = 0;
            Length = 0;
        }

        /*
        public GameMessage ParseMessage()
        {
            return GameMessage.ParseMessage(this);
        }

        public void EncodeMessage(GameMessage msg)
        {
            WriteInt(9, msg.Id);
            msg.Encode(this);
        }
        */
        public byte[] GetPacketAndReset()
        {
            int bytes = ((Length + 7) & (~7)) >> 3;
            Position = 0;
            WriteInt(32, bytes);
            byte[] result = new byte[bytes];

            Array.Copy(Data, result, bytes);
            Length = 32;
            Position = 32;

            for (int i = 0; i < Data.Length; i++) Data[i] = 0; // FIXME: WriteInt bugged? Dont care atm

            return result;
        }

        public bool CheckAvailable(int length)
        {
            return Position + length <= Length;
        }
        const int BufferAlignment = 31;

        public void AppendData(byte[] data)
        {
            int length = Length >> 3;
            if (length + data.Length > Data.Length)
            {
                int newSize = (length + data.Length + BufferAlignment) & (~BufferAlignment);
                Array.Resize(ref Data, newSize);
            }
            Array.Copy(data, 0, Data, length, data.Length);
            Length += data.Length * 8;
        }

        public void ConsumeData()
        {
            int bytes = ((Position + 7) & (~7)) >> 3;
            Array.Copy(Data, bytes, Data, 0, (Length >> 3) - bytes);
            Length = Length - (bytes * 8);
            Position = 0;
        }


        public void MakeAvailable(int length)
        {
            if (Position + length > Data.Length * 8)
            {
                int newSize = (((Position + length + 7) / 8) + BufferAlignment) & (~BufferAlignment);
                Array.Resize(ref Data, newSize);
            }
        }

        public static int GetBitCount(int x)
        {
            int count = 0;
            while (x > 0)
            {
                x >>= 1;
                count++;
            }
            return count;
        }

        public static int GetIntegerValueBitCount(int min, int max)
        {
            int x = max - min;
            if (x <= 0)
                return 0; // D3 compat
            return GetBitCount(x);
        }

        public bool IsPacketAvailable()
        {
            if (Length - Position < 32)
                return false;
            int pos = Position;
            int packetSize = ReadInt(32);
            Position = pos;
            return CheckAvailable(packetSize * 8);
        }

        public int ReadInt(int length)
        {
            if (!CheckAvailable(length))
                throw new ProtocolBitBufferException("Not enough bits remaining.");

            int result = 0;
            while (length > 0)
            {
                int off = Position & 7;
                int count = 8 - off;
                if (count > length)
                    count = length;
                int mask = (1 << count) - 1;
                int bits = (Data[Position >> 3] >> off);
                result |= (bits & mask) << (length - count);
                length -= count;
                Position += count;
            }
            return result;
        }

        public void WriteInt(int length, int value)
        {
            MakeAvailable(length);
            while (length > 0)
            {
                int off = Position & 7;
                int count = 8 - off;
                if (count > length)
                    count = length;
                int mask = (1 << count) - 1;
                Data[Position >> 3] = (byte)((Data[Position >> 3] & (~(mask << off)))  | (((value >> (length - count)) & mask) << off));
                //Data[Position >> 3] |= (byte)(((value >> (length - count)) & mask) << off);
                length -= count;
                Position += count;
                if (Position > Length)
                    Length = Position;
            }
        }

        byte[] _floatBuffer = new byte[4];

        public float ReadFloat32()
        {
            int value = ReadInt(32);
            _floatBuffer[0] = (byte)value;
            _floatBuffer[1] = (byte)((value >> 8) & 0xFF);
            _floatBuffer[2] = (byte)((value >> 16) & 0xFF);
            _floatBuffer[3] = (byte)((value >> 24) & 0xFF);
            return BitConverter.ToSingle(_floatBuffer, 0);
        }

        public void WriteFloat32(float value)
        {
            WriteInt(32, BitConverter.ToInt32(BitConverter.GetBytes(value), 0));
        }


        public long ReadInt64(int length)
        {
            int count = length >= 32 ? 32 : length;
            long result = ReadInt(count);
            count = length - count;
            if (count > 0)
                result = (result << count) | (long)(uint)ReadInt(count);
            return result;
        }

        public void WriteInt64(int length, long value)
        {
            MakeAvailable(length);

            if (length <= 32)
            {
                WriteInt(length, (int)(uint)value);
                return;
            }

            int count = length - 32;
            WriteInt(32, (int)(uint)(value >> count));
            WriteInt(count, (int)(uint)value);
        }
        

        public string ReadCharArray(int maxLength)
        {
            int size = ReadInt(GetBitCount(maxLength));
            Position = (Position + 7) & (~7);
            if (!CheckAvailable(size * 8))
                throw new ProtocolBitBufferException("Not enough bits remaining.");
            var result = Encoding.UTF8.GetString(Data, Position >> 3, size);
            Position += size * 8;
            return result;
        }

        public void WriteCharArray(int maxLength, string value)
        {
            var result = Encoding.UTF8.GetBytes(value);
            WriteInt(GetBitCount(maxLength), result.Length);
            Position = (Position + 7) & (~7);
            MakeAvailable(result.Length * 8);
            Buffer.BlockCopy(result, 0, Data, Position >> 3, result.Length);
            Position += result.Length * 8;
            if (Position > Length)
                Length = Position;
        }

        public bool ReadBool() { return ReadInt(1) != 0; }
        public void WriteBool(bool value) { WriteInt(1, value ? 1 : 0); }

        public byte[] ReadBlob(int sizeBits)
        {
            int size = ReadInt(sizeBits);
            byte[] result = new byte[size];
            Position = (Position + 7) & (~7);
            if (!CheckAvailable(size * 8))
                throw new ProtocolBitBufferException("Not enough bits remaining.");
            Buffer.BlockCopy(Data, Position >> 3, result, 0, size);
            Position += size * 8;
            return result;
        }
        public byte[] ReadBlobNoLength(int size)
        {
            byte[] result = new byte[size];
            Position = (Position + 7) & (~7);
            if (!CheckAvailable(size * 8))
                throw new ProtocolBitBufferException("Not enough bits remaining.");
            Buffer.BlockCopy(Data, Position >> 3, result, 0, size);
            Position += size * 8;
            return result;
        }
        public void WriteBlob(int sizeBits, byte[] data)
        {
            WriteInt(sizeBits, data.Length);
            Position = (Position + 7) & (~7);
            MakeAvailable(data.Length * 8);
            Buffer.BlockCopy(data, 0, Data, Position >> 3, data.Length);
            Position += data.Length * 8;
            if (Position > Length)
                Length = Position;
        }


        public float ReadFloat16()
        {
            int bits = ReadInt(16);
            int value;
            value = (bits & 0x3FF) << 13;
            value |= (((bits >> 10) & 0x1F) + 112) << 23;
            value |= (bits & 0x8000) << 16;

            _floatBuffer[0] = (byte)value;
            _floatBuffer[1] = (byte)((value >> 8) & 0xFF);
            _floatBuffer[2] = (byte)((value >> 16) & 0xFF);
            _floatBuffer[3] = (byte)((value >> 24) & 0xFF);
            return BitConverter.ToSingle(_floatBuffer, 0);
        }

        public void WriteFloat16(float value)
        {
            int bits = BitConverter.ToInt32(BitConverter.GetBytes(value), 0);
            int x = (bits >> 13) & 0x3FF;
            int y = (((bits >> 23) & 0xFF) - 112);
            if (y > 0)
                x |= y << 10;
            x |= (bits >> 16) & 0x8000;
            WriteInt(16, x);
            if (Position > Length)
                Length = Position;
        }

    }
}
