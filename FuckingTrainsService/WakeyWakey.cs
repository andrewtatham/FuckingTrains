using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;

namespace FuckingTrainsService
{
    public class WakeyWakey
    {

        [DllImport("kernel32.dll")]
        public static extern SafeWaitHandle CreateWaitableTimer(IntPtr lpTimerAttributes, bool bManualReset, string lpTimerName);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWaitableTimer(SafeWaitHandle hTimer, [In] ref long pDueTime, int lPeriod, IntPtr pfnCompletionRoutine, IntPtr lpArgToCompletionRoutine, bool fResume);



        public static void WakeUpAt(DateTime utc)
        {
            long duetime = utc.ToFileTime();

            using (SafeWaitHandle handle = CreateWaitableTimer(IntPtr.Zero, true, "MyWaitabletimer"))
            {
                if (SetWaitableTimer(handle, ref duetime, 0, IntPtr.Zero, IntPtr.Zero, true))
                {
                    using (EventWaitHandle wh = new EventWaitHandle(false, EventResetMode.AutoReset))
                    {
                        wh.SafeWaitHandle = handle;
                        wh.WaitOne();
                    }
                }
                else
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
            }
        }

    }
}
