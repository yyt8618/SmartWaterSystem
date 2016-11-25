using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace SmartWaterSystem
{
    public static class MiniDump
    {
        // Taken almost verbatim from http://blog.kalmbach-software.de/2008/12/13/writing-minidumps-in-c/
        [Flags]
        public enum Option : uint

        {
            // From dbghelp.h:
            Normal = 0x00000000,

            WithDataSegs = 0x00000001,

            WithFullMemory = 0x00000002,

            WithHandleData = 0x00000004,

            FilterMemory = 0x00000008,

            ScanMemory = 0x00000010,

            WithUnloadedModules = 0x00000020,

            WithIndirectlyReferencedMemory = 0x00000040,

            FilterModulePaths = 0x00000080,

            WithProcessThreadData = 0x00000100,

            WithPrivateReadWriteMemory = 0x00000200,

            WithoutOptionalData = 0x00000400,

            WithFullMemoryInfo = 0x00000800,

            WithThreadInfo = 0x00001000,

            WithCodeSegs = 0x00002000,

            WithoutAuxiliaryState = 0x00004000,

            WithFullAuxiliaryState = 0x00008000,

            WithPrivateWriteCopyMemory = 0x00010000,

            IgnoreInaccessibleMemory = 0x00020000,

            ValidTypeFlags = 0x0003ffff,
        };

        public enum ExceptionInfo
        {
            None,
            Present
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]  // Pack=4 is important! So it works also for x64!
        public struct MiniDumpExceptionInformation
        {
            public uint ThreadId;
            public IntPtr ExceptionPointers;

            [MarshalAs(UnmanagedType.Bool)]
            public bool ClientPointers;
        }


        // Overload requiring MiniDumpExceptionInformation

        [DllImport("dbghelp.dll", EntryPoint = "MiniDumpWriteDump", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
        static extern bool MiniDumpWriteDump(IntPtr hProcess, uint processId, SafeHandle hFile, uint dumpType, ref MiniDumpExceptionInformation expParam, IntPtr userStreamParam, IntPtr callbackParam);

        // Overload supporting MiniDumpExceptionInformation == NULL

        [DllImport("dbghelp.dll", EntryPoint = "MiniDumpWriteDump", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
        static extern bool MiniDumpWriteDump(IntPtr hProcess, uint processId, SafeHandle hFile, uint dumpType, IntPtr expParam, IntPtr userStreamParam, IntPtr callbackParam);

        [DllImport("kernel32.dll", EntryPoint = "GetCurrentThreadId", ExactSpelling = true)]
        static extern uint GetCurrentThreadId();

        public static bool Write(SafeHandle fileHandle, Option options, ExceptionInfo exceptionInfo)
        {
            Process currentProcess = Process.GetCurrentProcess();
            IntPtr currentProcessHandle = currentProcess.Handle;
            uint currentProcessId = (uint)currentProcess.Id;

            MiniDumpExceptionInformation exp;
            exp.ThreadId = GetCurrentThreadId();
            exp.ClientPointers = false;
            exp.ExceptionPointers = IntPtr.Zero;

            if (exceptionInfo == ExceptionInfo.Present)
            {

                exp.ExceptionPointers = System.Runtime.InteropServices.Marshal.GetExceptionPointers();
            }

            bool bRet = false;

            if (exp.ExceptionPointers == IntPtr.Zero)
            {
                bRet = MiniDumpWriteDump(currentProcessHandle, currentProcessId, fileHandle, (uint)options, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
            }
            else
            {
                bRet = MiniDumpWriteDump(currentProcessHandle, currentProcessId, fileHandle, (uint)options, ref exp, IntPtr.Zero, IntPtr.Zero);
            }

            return bRet;
        }

        public static bool Write(SafeHandle fileHandle, Option dumpType)
        {
            return Write(fileHandle, dumpType, ExceptionInfo.None);
        }
    }
}
