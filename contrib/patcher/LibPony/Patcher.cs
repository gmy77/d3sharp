using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace PonyLib
{
    public static class Patcher
    {
        private static Process _process;

        private static IntPtr _processhWnd;

        private static bool _isProcessOpen = false;
        private static bool _isAlreadyPatched;

        private static int _serverIpCheckAddr = 0x0;
        private static int _secondChallengeCheckAddr = 0x0;

        private static readonly byte[] JmpOpcode = new byte[] { 0xEB };


        public static bool IsClientRunning()
        {
            return Process.GetProcesses().FirstOrDefault(process => process.ProcessName.Trim() == "Diablo III")!=null;
        }

        public static bool IsAlreadyPatched()
        {
            if(!IsClientRunning())
                return false;

            if(!_isProcessOpen)
                FindAndOpenProcess();

            return _isAlreadyPatched;
        }

        private static void FindAndOpenProcess()
        {
            _processhWnd = IntPtr.Zero;

            try
            {
                _process = Process.GetProcesses().FirstOrDefault(process => process.ProcessName.Trim() == "Diablo III");

                if(_process==null)
                {
                    _isProcessOpen = false;
                    throw  new Exception("Can not find client process!");
                }

                _processhWnd = Win32.OpenProcess(0x001F0FFF, false, _process.Id); // 0x001F0FFF = PROCESS_ALL_ACCESS

                if (_processhWnd == IntPtr.Zero)
                {
                    _isProcessOpen = false;
                    throw new Exception("Failed to open client process!");
                }

                _isProcessOpen = true;

                var processModules = _process.Modules;
                ProcessModule bnetModule = null;

                foreach (ProcessModule module in processModules)
                {
                    if (module.ModuleName != "battle.net.dll")
                        continue;

                    if (module.FileVersionInfo.FileDescription != PatchInfo.RequiredBnetModuleVersion)
                        throw new Exception("battle.net.dll module version is different than expected!");

                    bnetModule = module;
                    break;
                }

                var baseAddr = IntPtr.Zero;
                baseAddr = bnetModule.BaseAddress;

                if (baseAddr == IntPtr.Zero)
                    throw new Exception("Failed to locate battle.net.dll");

                _serverIpCheckAddr = baseAddr.ToInt32() + PatchInfo.ServerIPCheckOffset;
                _secondChallengeCheckAddr = baseAddr.ToInt32() + PatchInfo.SecondChallengeCheckOffset;

                var serverIPCheckByte = ReadByte(_processhWnd, _serverIpCheckAddr);
                var secondChallengeCheckByte = ReadByte(_processhWnd, _secondChallengeCheckAddr);

                if (serverIPCheckByte != 0x75 || secondChallengeCheckByte != 0x74)
                    _isAlreadyPatched = true;
            }
            finally
            {
                Cleanup();
            }
        }

        public static bool Patch()
        {
            if (!_isProcessOpen)
                FindAndOpenProcess();

            if (IsAlreadyPatched())
                throw new Exception("Process is already patched or an unknown bnet module is hit!");

            var bytesWritten = IntPtr.Zero;

            // patch thumbprint (game server ip check).
            Win32.WriteProcessMemory(_processhWnd, new IntPtr(_serverIpCheckAddr), JmpOpcode, 1, out bytesWritten);

            if (bytesWritten.ToInt32() < 1)
                throw new Exception("Failed to write to process.");

            // patch second challenge check.
            Win32.WriteProcessMemory(_processhWnd, new IntPtr(_secondChallengeCheckAddr), JmpOpcode, 1, out bytesWritten);

            if (bytesWritten.ToInt32() < 1)
                throw new Exception("Failed to write to process.");

            return true;
        }

        public static void Cleanup()
        {
            if (_isProcessOpen && _processhWnd != IntPtr.Zero)
                Win32.CloseHandle(_processhWnd);
        }

        private static byte ReadByte(IntPtr handle, int offset)
        {
            byte result = 0;
            Win32.ReadProcessMemory(handle, offset, out result, 1, IntPtr.Zero);
            return result;
        }
    }
}
