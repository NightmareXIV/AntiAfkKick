using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AntiAfkKick
{
    class Native
    {
        internal struct LASTINPUTINFO
        {
            public uint cbSize;

            public uint dwTime;
        }

        /// <summary>
        /// Helps to find the idle time, (in milliseconds) spent since the last user input
        /// </summary>
        public class IdleTimeFinder
        {
            [DllImport("User32.dll")]
            private static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

            [DllImport("Kernel32.dll")]
            private static extern uint GetLastError();

            public static uint GetIdleTime()
            {
                LASTINPUTINFO lastInPut = new LASTINPUTINFO();
                lastInPut.cbSize = (uint)System.Runtime.InteropServices.Marshal.SizeOf(lastInPut);
                GetLastInputInfo(ref lastInPut);

                return ((uint)Environment.TickCount - lastInPut.dwTime);
            }
            /// <summary>
            /// Get the Last input time in milliseconds
            /// </summary>
            /// <returns></returns>
            public static long GetLastInputTime()
            {
                LASTINPUTINFO lastInPut = new LASTINPUTINFO();
                lastInPut.cbSize = (uint)System.Runtime.InteropServices.Marshal.SizeOf(lastInPut);
                if (!GetLastInputInfo(ref lastInPut))
                {
                    throw new Exception(GetLastError().ToString());
                }
                return lastInPut.dwTime;
            }
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetForegroundWindow();

        public class Keypress
        {
            public const int LControlKey = 162;
            public const int Space = 32;

            const uint WM_KEYUP = 0x101;
            const uint WM_KEYDOWN = 0x100;

            [DllImport("user32.dll")]
            private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

            public static void SendKeycode(IntPtr hwnd, int keycode)
            {
                SendMessage(hwnd, WM_KEYDOWN, (IntPtr)keycode, (IntPtr)0);
                SendMessage(hwnd, WM_KEYUP, (IntPtr)keycode, (IntPtr)0);
            }
        }
    }
}
