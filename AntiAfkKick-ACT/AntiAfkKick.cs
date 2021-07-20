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
                    var proc = Process.GetProcessesByName("ffxiv_dx11");
                    if (proc.Length == 0)
                    {
                        proc = Process.GetProcessesByName("ffxiv");
                    }
                    if (proc.Length == 0)
                    {
                        pluginStatusText.Text = Environment.TickCount + ": Could not find FFXIV window";
                    }
                    else
                    {
                        if (Environment.TickCount > NextKeyPress &&
                            (Native.GetForegroundWindow() != proc[0].MainWindowHandle || Native.IdleTimeFinder.GetIdleTime() > 60 * 1000))
                        {
                            pluginStatusText.Text = Environment.TickCount + ": Sending keypress to FFXIV window";
                            Native.Keypress.SendKeycode(proc[0].MainWindowHandle, Native.Keypress.LControlKey);
                            NextKeyPress = Environment.TickCount + new Random().Next(2 * 60 * 1000, 4 * 60 * 1000);
                        }
                    }
                    Thread.Sleep(1000);
                }
            }).Start();
        }
    }
}
