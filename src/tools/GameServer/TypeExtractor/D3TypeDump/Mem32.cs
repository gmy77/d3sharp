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
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace D3TypeDump
{
    class MemAddress32
    {
        public Mem32 Memory;
        public int Offset;

        public MemAddress32(Mem32 mem, int offset) { Memory = mem; Offset = offset; }

        public MemAddress32 this[int offset] { get { return new MemAddress32(Memory, Offset + offset); } }

        public byte Byte { get { return Memory.ReadByte(Offset); } }
        public ushort UInt16 { get { return Memory.ReadUInt16(Offset); } }
        public float Float { get { return Memory.ReadFloat(Offset); } }
        public int Int32 { get { return Memory.ReadInt32(Offset); } }
        public uint UInt32 { get { return (uint)Memory.ReadInt32(Offset); } }
        public MemAddress32 Ptr { get { return new MemAddress32(Memory, Memory.ReadInt32(Offset)); } }
        public string CStringPtr { get { return Memory.ReadCString(Memory.ReadInt32(Offset)); } }
        public string CString { get { return Memory.ReadCString(Offset); } }
    }

    class Mem32 : IDisposable
    {
        [DllImport("kernel32.dll", ExactSpelling = true)]
        static extern IntPtr OpenProcess(int dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, IntPtr lpNumberOfBytesWritten);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [Out] byte[] lpBuffer, int dwSize, IntPtr lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool ReadProcessMemory(IntPtr hProcess, int lpBaseAddress, out int lpBuffer, int dwSize, IntPtr lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool ReadProcessMemory(IntPtr hProcess, int lpBaseAddress, out float lpBuffer, int dwSize, IntPtr lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool ReadProcessMemory(IntPtr hProcess, int lpBaseAddress, out ushort lpBuffer, int dwSize, IntPtr lpNumberOfBytesRead);
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool ReadProcessMemory(IntPtr hProcess, int lpBaseAddress, out byte lpBuffer, int dwSize, IntPtr lpNumberOfBytesRead);

        const int ProcessFlags_All = 0x001F0FFF;

        IntPtr _handle;

        public Mem32(Process proc)
        {
            _handle = OpenProcess(ProcessFlags_All, false, proc.Id);
            if (_handle == IntPtr.Zero)
                throw new Exception("Failed to open process.");
        }

        public MemAddress32 this[int offset] { get { return new MemAddress32(this, offset); } }

        public byte ReadByte(int offset)
        {
            byte result = 0;
            ReadProcessMemory(_handle, offset, out result, 1, IntPtr.Zero);
            return result;
        }
        public ushort ReadUInt16(int offset)
        {
            ushort result = 0;
            ReadProcessMemory(_handle, offset, out result, 2, IntPtr.Zero);
            return result;
        }
        public int ReadInt32(int offset)
        {
            int result = 0;
            ReadProcessMemory(_handle, offset, out result, 4, IntPtr.Zero);
            return result;
        }

        public float ReadFloat(int offset)
        {
            float result = 0;
            ReadProcessMemory(_handle, offset, out result, 4, IntPtr.Zero);
            return result;
        }

        static Encoding IBM437 = Encoding.GetEncoding(437);
        public string ReadCString(int offset)
        {
            byte result = 0;
            byte[] buffer = new byte[256];
            int count = 0;

            for (; ; )
            {
                ReadProcessMemory(_handle, offset++, out result, 1, IntPtr.Zero);
                if (result == 0)
                    break;
                if (count == buffer.Length)
                    Array.Resize<byte>(ref buffer, buffer.Length + 256);
                buffer[count++] = result;
            }
            return IBM437.GetString(buffer, 0, count);
        }

        ~Mem32()
        {
            if (_handle != IntPtr.Zero)
                CloseHandle(_handle);
        }

        public void Dispose()
        {
            if (_handle != IntPtr.Zero)
            {
                CloseHandle(_handle);
                System.GC.SuppressFinalize(this);
            }
        }
    }
}
