using AntiAfkKick;
using Dalamud.Plugin;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AntiAfkKick_Dalamud
{
    class AntiAfkKick : IDalamudPlugin
    {
        public string Name => "AntiAfkKick";
        internal volatile bool running = true;
        int NextKeyPress = 0;

        public void Dispose()
        {
            running = false;
        }

        public void Initialize(DalamudPluginInterface pluginInterface)
        {
            new Thread((ThreadStart)delegate
            {
                var proc = Process.GetCurrentProcess();
                while (running)
                {
                    if (Environment.TickCount > NextKeyPress &&
                        (Native.GetForegroundWindow() != proc.MainWindowHandle || Native.IdleTimeFinder.GetIdleTime() > 60 * 1000))
                    {
                        Native.Keypress.SendKeycode(proc.MainWindowHandle, Native.Keypress.LControlKey);
                        NextKeyPress = Environment.TickCount + new Random().Next(2 * 60 * 1000, 4 * 60 * 1000);
                        PluginLog.Information("Sending anti-afk keypress");
                    }
                    Thread.Sleep(1000);
                }
            }).Start();
        }
    }
}
