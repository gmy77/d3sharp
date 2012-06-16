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

using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace PonyLib
{
    /// <summary>
    /// Pony feeder library!
    /// </summary>
    public static class Patcher
    {
        private static Process _process; // the client process.
        private static IntPtr _processhWnd; // the hWnd for the opened process.
        private static bool _isProcessOpen = false; 
        private static bool _isAlreadyPatched = false;
        private static int _serverIpCheckAddr = 0x0;
        private static int _secondChallengeCheckAddr = 0x0;

        private static readonly byte[] JmpOpcode = new byte[] { 0xeb }; // Jmp opcode fix for ponies ;)

        /// <summary>
        /// Returns true if client is running.
        /// </summary>
        /// <returns><see cref="bool"/> value.</returns>
        public static bool IsClientRunning()
        {
            return Process.GetProcesses().FirstOrDefault(process => process.ProcessName.Trim() == "Diablo III")!=null;
        }

        /// <summary>
        /// Returns true if client is already patched, false if not.
        /// </summary>
        /// <returns><see cref="bool"/> value.</returns>
        public static bool IsAlreadyPatched()
        {
            if(!IsClientRunning()) // make sure the client is running.
                return false;

            if(!_isProcessOpen) // if we didn't open the process yet,
                FindAndOpenProcess(); // make it so.

            return _isAlreadyPatched; // FindAndOpenProcess() will check if the process and write it to _isAlreadyPatched.
        }

        /// <summary>
        /// Function that patches the client and disables server IP and second challenge checks.
        /// </summary>
        /// <returns></returns>
        public static bool Patch()
        {
            if (!IsClientRunning()) // make sure the client is running.
                return false;

            if (!_isProcessOpen) // if we didn't open the process yet,
                FindAndOpenProcess(); // make it so.

            if (IsAlreadyPatched()) // make sure process is not already patched!
                throw new Exception("Process is already patched!");

            var bytesWritten = IntPtr.Zero;

            // patch thumbprint (game server ip check).
            var retVal = Win32.WriteProcessMemory(_processhWnd, new IntPtr(_serverIpCheckAddr), JmpOpcode, 1, out bytesWritten);
            if(!retVal) // see if we hit an win32 error.
            {
                var win32ErrorCode = Marshal.GetLastWin32Error();
                throw new Exception(string.Format("Failed to write to process [win32 error code: 0x{0:X}]", win32ErrorCode));
            }
            else if (bytesWritten.ToInt32() < 1) // make sure we've actually written the byte.
            {
                throw new Exception("Failed to write to process.");
            }

            // patch second challenge check.
            retVal = Win32.WriteProcessMemory(_processhWnd, new IntPtr(_secondChallengeCheckAddr), JmpOpcode, 1, out bytesWritten);
            if (!retVal) // see if we hit an win32 error.
            {
                var win32ErrorCode = Marshal.GetLastWin32Error();
                throw new Exception(string.Format("Failed to write to process [win32 error code: {0}", win32ErrorCode));
            }
            else if (bytesWritten.ToInt32() < 1) // make sure we've actually written the byte.
            {
                throw new Exception("Failed to write to process.");
            }

            return true; // ponies should be happy now!
        }

        /// <summary>
        /// Executes any necessary cleanup operations.
        /// </summary>
        public static void Cleanup()
        {
            if (_isProcessOpen && _processhWnd != IntPtr.Zero) // if process is still open.
                Win32.CloseHandle(_processhWnd); // close it.

            _isProcessOpen = false;
            _processhWnd = IntPtr.Zero;
        }

        /// <summary>
        /// Internal function that finds the client process and opens it.
        /// </summary>
        private static void FindAndOpenProcess()
        {
            _processhWnd = IntPtr.Zero;

            _process = Process.GetProcesses().FirstOrDefault(process => process.ProcessName.Trim() == "Diablo III"); // find the process.
            if (_process == null)
            {
                _isProcessOpen = false;
                throw new Exception("Can not find client process!");
            }

            _processhWnd = Win32.OpenProcess(0x001F0FFF, false, _process.Id); // open the process with all access - 0x001F0FFF = PROCESS_ALL_ACCESS
            if (_processhWnd == IntPtr.Zero)
            {
                _isProcessOpen = false;
                throw new Exception("Failed to open client process!");
            }

            _isProcessOpen = true;

            var processModules = _process.Modules; // get process' modules.
            ProcessModule bnetModule = null;

            foreach (ProcessModule module in processModules) // enumurate all modules.
            {
                if (module.ModuleName != "battle.net.dll")
                    continue;

                // find the battle.net.dll module and check for module version.
                if (module.FileVersionInfo.FileDescription != PatchInfo.RequiredBnetModuleVersion)
                    throw new Exception("battle.net.dll module version is different than expected!");

                bnetModule = module;
                break;
            }

            var baseAddr = IntPtr.Zero;
            baseAddr = bnetModule.BaseAddress;

            if (baseAddr == IntPtr.Zero)
                throw new Exception("Failed to locate battle.net.dll");

            _serverIpCheckAddr = baseAddr.ToInt32() + PatchInfo.ServerIPCheckOffset; // calculate the address for server IP check.
            _secondChallengeCheckAddr = baseAddr.ToInt32() + PatchInfo.SecondChallengeCheckOffset; // calculate the address for second challenge check.

            var serverIPCheckByte = ReadByte(_processhWnd, _serverIpCheckAddr); // read current opcode for server IP check.
            var secondChallengeCheckByte = ReadByte(_processhWnd, _secondChallengeCheckAddr); // read current opcode for second challenge check.

            if (serverIPCheckByte != 0x75 || secondChallengeCheckByte != 0x74) // see if process is already patched.
                _isAlreadyPatched = true;
        }

        /// <summary>
        /// Internal function to read a byte from process memory.
        /// </summary>
        /// <param name="handle">The process' handle to read from.</param>
        /// <param name="offset">The offset to read from.</param>
        /// <returns></returns>
        private static byte ReadByte(IntPtr handle, int offset)
        {
            byte result = 0;
            Win32.ReadProcessMemory(handle, offset, out result, 1, IntPtr.Zero);
            return result;
        }
    }
}
