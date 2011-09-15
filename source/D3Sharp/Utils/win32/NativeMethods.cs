using System;
using System.Runtime.InteropServices;

namespace D3Sharp.Utils.win32
{
    internal static class NativeMethods
    {
        /* Win32 API entries; GetStdHandle() and AllocConsole() allows a windowed application to bind a console window */
        [DllImport("kernel32.dll", EntryPoint = "GetStdHandle", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", EntryPoint = "AllocConsole", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        internal static extern int AllocConsole();
    }
}
