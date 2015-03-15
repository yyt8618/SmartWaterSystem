using System;
using System.Runtime.InteropServices;

namespace Common
{
    public class Win32
    {
        public const uint INFINITE = 0xffffffffu;

        public const uint EVENT_PULSE = 1;
        public const uint EVENT_RESET = 2;
        public const uint EVENT_SET = 3;

        [DllImport("Kernel32.dll")]
        public static extern IntPtr CreateEvent(IntPtr lpEventAttributes, [MarshalAs(UnmanagedType.Bool)]bool bManualReset, [MarshalAs(UnmanagedType.Bool)]bool bInitialState, [MarshalAs(UnmanagedType.LPWStr)]string lpName);

        [DllImport("Kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseHandle(IntPtr hObject);

        [DllImport("Kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetEvent(IntPtr hEvent);

        //[DllImport("Kernel32.dll")]
        //[return: MarshalAs(UnmanagedType.Bool)]
        //public static extern bool EventModify(IntPtr hEvent, uint func);

        [DllImport("Kernel32.dll")]
        public static extern uint WaitForSingleObject(IntPtr hHandle, uint dwMilliseconds);

        [DllImport("Kernel32.dll")]
        public static extern uint WaitForMultipleObjects(uint nCount, IntPtr[] lpHandles, [MarshalAs(UnmanagedType.Bool)]bool fWaitAll, uint dwMilliseconds);

    }
}
