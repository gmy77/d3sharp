using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace patcher
{
    class Program
    {
        #region Imports
        [DllImport("kernel32.dll", ExactSpelling = true)]
        static extern IntPtr OpenProcess(uint dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out IntPtr lpNumberOfBytesWritten);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool ReadProcessMemory(IntPtr hProcess, int lpBaseAddress, out byte lpBuffer, int dwSize, IntPtr lpNumberOfBytesRead);
        #endregion

        #region BETA
        #region Build 0.9.0.8896.BETA
        //static Int32 offset = 0x000B4475;
        //static string version = "bcd3e50524"; //DLL Battle.net Aurora bcd3e50524_public/329 (Mar 14 2012 10:28:16)
        #endregion

        #region Build 0.10.0.9183.BETA
        //static Int32 offset = 0x000B5505;
        #endregion

        #region Build 0.11.0.9327.BETA
        //Build 0.11.0.9359.BETA
        //static Int32 offset = 0x000B5605;
        //static string version = "8eac7d44dc";
        #endregion
        #endregion

        #region Build 1.0.1.9558
        //static Int32 offset = 0x000B5952;
        //static string version = "31c8df955a";
        #endregion

        #region Build 1.0.2.9749
        //static Int32 offset = 0x000BA802;
        //static string version = "8018401a9c";
        #endregion

        #region Build 1.0.2.9858 & 1.0.2.9950
        //static Int32 serverOffset = 0x000BA8A2;
        //static Int32 challengeOffset = 0x000BA863;
        //static string version = "79fef7ae8e";
        #endregion

        #region Build 1.0.2.9991
        static Int32 serverOffset = 0x000BC25C;
        static Int32 challengeOffset = 0x000BC219;
        static string version = "24e2d13e54";
        #endregion

        static void Main(string[] args)
        {
            var foundD3 = false;
            var hWnd = IntPtr.Zero;
            try
            {
                foreach (var p in Process.GetProcesses())
                {
                    if (p.ProcessName == "Diablo III")
                    {
                        foundD3 = true;
                        hWnd = OpenProcess(0x001F0FFF, false, p.Id);
                        if (hWnd == IntPtr.Zero)
                            throw new Exception("Failed to open process.");

                        var modules = p.Modules;
                        IntPtr baseAddr = IntPtr.Zero;

                        foreach (ProcessModule module in modules)
                        {
                            if (module.ModuleName == "battle.net.dll")
                            {
                                if (module.FileVersionInfo.FileDescription == version)
                                {
                                    baseAddr = module.BaseAddress;
                                    break;
                                }
                                else
                                    throw new Exception("battle.net.dll version different than expected.");
                            }
                        }

                        if (baseAddr == IntPtr.Zero)
                            throw new Exception("Failed to locate battle.net.dll");

                        var serverAddr = baseAddr.ToInt32() + serverOffset;
                        var challengeAddr = baseAddr.ToInt32() + challengeOffset;
                        var BytesWritten = IntPtr.Zero;
                        byte[] JMP = new byte[] { 0xEB };
                        Console.WriteLine("battle.net.dll address: 0x{0:X8}", baseAddr.ToInt32());
                        var prevByte = ReadByte(hWnd, serverAddr);
                        if (prevByte != 0x75)
                            throw new Exception(string.Format("File already patched or unknown battle.net.dll version. 0x{0:X2} != 0x75",prevByte));
                        prevByte = ReadByte(hWnd, challengeAddr);
                        if (prevByte != 0x74)
                            throw new Exception(string.Format("File already patched or unknown battle.net.dll version. 0x{0:X2} != 0x74", prevByte));

                        // patch thumbprint.
                        WriteProcessMemory(hWnd, new IntPtr(serverAddr), JMP, 1, out BytesWritten);
                        if (BytesWritten.ToInt32() < 1)
                            throw new Exception("Failed to write to process.");

                        //disable second challenge checks
                        WriteProcessMemory(hWnd, new IntPtr(challengeAddr), JMP, 1, out BytesWritten);

                        //Console.WriteLine("After write: 0x{0:X2}", ReadByte(hWnd, JMPAddr));

                        if (BytesWritten.ToInt32() < 1)
                            throw new Exception("Failed to write to process.");
                        else
                            Console.WriteLine("Program should now be patched.");
                    }
                }
                if (!foundD3)
                    throw new Exception("Failed to located D3 process. Is D3 running?");
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
            finally
            {
                if (hWnd != IntPtr.Zero)
                    CloseHandle(hWnd);
            }
            Console.ReadLine();
        }

        static byte ReadByte(IntPtr _handle, int offset)
        {
            byte result = 0;
            ReadProcessMemory(_handle, offset, out result, 1, IntPtr.Zero);
            return result;
        }
    }
}
