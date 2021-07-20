using Advanced_Combat_Tracker;
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
    class AntiAfkKick: IActPluginV1
    {
        int NextKeyPress = 0;
        volatile bool running = true;
        Process proc = null;

        public void DeInitPlugin()
        {
            running = false;
        }
        public void InitPlugin(TabPage pluginScreenSpace, Label pluginStatusText)
        {
            ((TabControl)pluginScreenSpace.Parent).TabPages.Remove(pluginScreenSpace);
            new Thread((ThreadStart)delegate
            {
                while (running)
                {
                    Thread.Sleep(1000);
                    try
                    {
                        if (proc == null || proc.HasExited)
                        {
                            var p = Process.GetProcessesByName("ffxiv_dx11");
                            if (p.Length == 0)
                            {
                                p = Process.GetProcessesByName("ffxiv");
                            }
                            if (p.Length == 0)
                            {
                                pluginStatusText.Text = ("Could not find FFXIV window");
                                continue;
                            }
                            proc = p[0];
                            pluginStatusText.Text = ("New process found, pid = " + proc.Id);
                        }
                        if (Environment.TickCount > NextKeyPress &&
                            (Native.GetForegroundWindow() != proc.MainWindowHandle || Native.IdleTimeFinder.GetIdleTime() > 60 * 1000))
                        {
                            pluginStatusText.Text = (DateTimeOffset.Now + ": Sending keypress to FFXIV window");
                            Native.Keypress.SendKeycode(proc.MainWindowHandle, Native.Keypress.LControlKey);
                            NextKeyPress = Environment.TickCount + new Random().Next(2 * 60 * 1000, 4 * 60 * 1000);
                        }
                    }
                    catch (Exception) { }
                }
            }).Start();
        }
    }
}
