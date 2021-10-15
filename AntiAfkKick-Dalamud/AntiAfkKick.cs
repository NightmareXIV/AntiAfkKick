using AntiAfkKick;
using Dalamud.Logging;
using Dalamud.Plugin;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static AntiAfkKick.Native.Keypress;

namespace AntiAfkKick_Dalamud
{
    class AntiAfkKick : IDalamudPlugin
    {
        public string Name => "AntiAfkKick-Dalamud";
        internal volatile bool running = true;
        int NextKeyPress = 0;

        public void Dispose()
        {
            running = false;
        }

        public AntiAfkKick(DalamudPluginInterface pluginInterface)
        {
            new Thread((ThreadStart)delegate
            {
                var proc = Process.GetCurrentProcess();
                while (running)
                {
                    var fgWH = Native.GetForegroundWindow();
                    //PluginLog.Information($"[Debug] fgWH:{fgWH:X16}; proc.MWH:{proc.MainWindowHandle:X16}; GCP.MWH:{Process.GetCurrentProcess().MainWindowHandle:X16}");
                    if (Environment.TickCount > NextKeyPress &&
                        (fgWH != proc.MainWindowHandle || Native.IdleTimeFinder.GetIdleTime() > 60 * 1000))
                    {
                        PluginLog.Information($"Sending anti-afk keypress: {proc.MainWindowHandle:X16}");
                        Task.Run(delegate 
                        {
                            SendMessage(proc.MainWindowHandle, WM_KEYDOWN, (IntPtr)LControlKey, (IntPtr)0);
                            Thread.Sleep(100);
                            SendMessage(proc.MainWindowHandle, WM_KEYUP, (IntPtr)LControlKey, (IntPtr)0);
                        });
                        NextKeyPress = Environment.TickCount + new Random().Next(2 * 60 * 1000, 4 * 60 * 1000);
                    }
                    if(fgWH == proc.MainWindowHandle)
                    {
                        NextKeyPress = Environment.TickCount + 5*60*1000;
                    }
                    Thread.Sleep(10000);
                }
            }).Start();
        }
    }
}
