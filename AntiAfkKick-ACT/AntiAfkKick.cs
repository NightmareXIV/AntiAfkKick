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
                    Thread.Sleep(10000);
                    Console.WriteLine("Cycle begins");
                    try
                    {
                        if (Environment.TickCount > NextKeyPress)
                        {
                            List<IntPtr> handles = new List<IntPtr>();
                            foreach (var handle in Native.GetGameWindows())
                            {
                                if (Native.GetForegroundWindow() != handle || Native.IdleTimeFinder.GetIdleTime() > 60 * 1000)
                                {
                                    Native.Keypress.SendKeycode(handle, Native.Keypress.LControlKey);
                                    handles.Add(handle);
                                }
                            }
                            NextKeyPress = Environment.TickCount + 2 * 60 * 1000;
                            if (handles.Count > 0)
                            {
                                pluginStatusText.Text = (DateTimeOffset.Now.ToLocalTime() + ": Sending keypress to FFXIV windows " + String.Join(", ", handles));
                            }
                        }
                    }
                    catch (Exception) { }
                }
            }).Start();
        }
    }
}
