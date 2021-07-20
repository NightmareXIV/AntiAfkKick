using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AntiAfkKick
{
    class AntiAfkKick
    {
        static int NextKeyPress = 0;
        static NotifyIcon n = null;
        private static string appGuid = "92f42221-51a5-4753-9e91-84aeea157d17";

        static void Main(string[] args)
        {
            using (Mutex mutex = new Mutex(false, "Global\\" + appGuid))
            {
                if (!mutex.WaitOne(0, false))
                {
                    MessageBox.Show("Application already running");
                    return;
                }
                n = new NotifyIcon
                {
                    Icon = SystemIcons.Application,
                    Visible = true,
                    ContextMenu = new ContextMenu(new MenuItem[] { new MenuItem("Exit", delegate { n.Dispose(); Environment.Exit(0); }) })
                };
                new Thread((ThreadStart)delegate
                {
                   while (true)
                   {
                       var proc = Process.GetProcessesByName("ffxiv_dx11");
                       if (proc.Length == 0)
                       {
                           proc = Process.GetProcessesByName("ffxiv");
                       }
                       if (proc.Length == 0)
                       {
                           Console.WriteLine("Could not find FFXIV window");
                       }
                       else
                       {
                           if (Environment.TickCount > NextKeyPress &&
                               (Native.GetForegroundWindow() != proc[0].MainWindowHandle || Native.IdleTimeFinder.GetIdleTime() > 60 * 1000))
                           {
                               Console.WriteLine("Sending keypress to FFXIV window");
                               Native.Keypress.SendKeycode(proc[0].MainWindowHandle, Native.Keypress.LControlKey);
                               NextKeyPress = Environment.TickCount + new Random().Next(2 * 60 * 1000, 4 * 60 * 1000);
                           }
                       }
                       Thread.Sleep(1000);
                   }
               }).Start();
                Application.Run();
            }
        }
    }
}
