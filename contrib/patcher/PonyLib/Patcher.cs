using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace PonyLib
{
    public static class Patcher
    {
        internal static Process Process;

        internal static IntPtr ProcesshWnd;

        private static Process GetClientProcess()
        {
            Process = Process.GetProcesses().FirstOrDefault(process => process.ProcessName.Trim() == "Diablo III");
            return Process;
        }

        private static ProcessModule GetBNetModule(Process process)
        {
            ProcesshWnd = IntPtr.Zero;

            if (process == null)
                throw new ArgumentException("process");

            if (process.ProcessName.Trim() != "Diablo III")
                throw new Exception("Supplied process is not a valid client!");

            ProcesshWnd = Win32.OpenProcess(0x001F0FFF, false, process.Id); // 0x001F0FFF = PROCESS_ALL_ACCESS
            if (ProcesshWnd == IntPtr.Zero)
                throw new Exception("Failed to open client process!");

            var processModules = process.Modules;

            foreach (ProcessModule module in processModules)
            {
                if (module.ModuleName != "battle.net.dll")
                    continue;

                if (module.FileVersionInfo.FileDescription != PatchInfo.RequiredBnetModuleVersion)
                    throw new Exception("battle.net.dll module version is different than expected!");

                return module;
            }

            return null;
        }

        public static bool Patch()
        {
            var process = GetClientProcess();
            var module = GetBNetModule(process);

            if (module.ModuleName != "battle.net.dll")
                throw  new Exception("Supplied module is not a valid one!");

            if (module.FileVersionInfo.FileDescription != PatchInfo.RequiredBnetModuleVersion)
                throw new Exception("battle.net.dll module version is different than expected!");

            try
            {
                var baseAddr = IntPtr.Zero;
                baseAddr = module.BaseAddress;

                if (baseAddr == IntPtr.Zero)
                    throw new Exception("Failed to locate battle.net.dll");

                var serverIPCheckAddr = baseAddr.ToInt32() + PatchInfo.ServerIPCheckOffset;
                var secondChallengeCheckAddr = baseAddr.ToInt32() + PatchInfo.SecondChallengeCheckOffset;

                var bytesWritten = IntPtr.Zero;
                var jmp = new byte[] {0xEB};

                var prevByte = ReadByte(ProcesshWnd, serverIPCheckAddr);
                if (prevByte != 0x75)
                    throw new Exception(string.Format("File already patched or unknown battle.net.dll version. 0x{0:X2} != 0x75", prevByte));

                prevByte = ReadByte(ProcesshWnd, secondChallengeCheckAddr);
                if (prevByte != 0x74)
                    throw new Exception(string.Format("File already patched or unknown battle.net.dll version. 0x{0:X2} != 0x74", prevByte));

                // patch thumbprint (game server ip check).
                Win32.WriteProcessMemory(ProcesshWnd, new IntPtr(serverIPCheckAddr), jmp, 1, out bytesWritten);
                if (bytesWritten.ToInt32() < 1)
                    throw new Exception("Failed to write to process.");

                // patch second challenge check.
                Win32.WriteProcessMemory(ProcesshWnd, new IntPtr(secondChallengeCheckAddr), jmp, 1,
                                         out bytesWritten);

                if (bytesWritten.ToInt32() < 1)
                    throw new Exception("Failed to write to process.");

                return true;
            }
            finally
            {
                if (ProcesshWnd != IntPtr.Zero)
                    Win32.CloseHandle(ProcesshWnd);                
            }
        }

        private static byte ReadByte(IntPtr handle, int offset)
        {
            byte result = 0;
            Win32.ReadProcessMemory(handle, offset, out result, 1, IntPtr.Zero);
            return result;
        }
    }
}
