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
        static ulong NextKeyPress = 0;
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
                Icon icon;
                try
                {
                    icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
                }
                catch (Exception)
                {
                    icon = SystemIcons.Application;
                }
                n = new NotifyIcon
                {
                    Icon = icon,
                    Visible = true,
                    ContextMenu = new ContextMenu(new MenuItem[] {
                        new MenuItem("AntiAfkKick standalone") { Enabled = false },
                        new MenuItem("-"),
                        new MenuItem("Report issue", delegate { Process.Start(new ProcessStartInfo() { UseShellExecute=true, FileName="https://github.com/Eternita-S/AntiAfkKick/issues" }); }),
                        new MenuItem("Exit", delegate { n.Dispose(); Environment.Exit(0); })
                    }),
                    Text = "AntiAfkKick"
                };
                new Thread((ThreadStart)delegate
                {
                    while (true)
                    {
                        Thread.Sleep(10000);
                        Console.WriteLine($"Cycle begins {Native.GetTickCount64()}");
                        try
                        {
                            if (Native.GetTickCount64() > NextKeyPress) {
                                foreach (var handle in Native.GetGameWindows())
                                {
                                    if(Native.GetForegroundWindow() != handle || Native.IdleTimeFinder.GetIdleTime() > 60 * 1000)
                                    {
                                        Console.WriteLine(Native.GetTickCount64() + ": Sending keypress to FFXIV window " + handle.ToString());
                                        Native.Keypress.SendKeycode(handle, Native.Keypress.LControlKey);
                                    }
                                }
                                //NextKeyPress = Native.GetTickCount64() + 2 * 60 * 1000;
                            }
                        }
                        catch (Exception) { }
                    }
                }).Start();
                Application.Run();
            }
        }
    }
}
