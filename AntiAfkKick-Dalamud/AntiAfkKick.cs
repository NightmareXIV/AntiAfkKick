using AntiAfkKick;
using Dalamud.Game.ClientState.Keys;
using Dalamud.Logging;
using Dalamud.Plugin;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static AntiAfkKick.Native.Keypress;

namespace AntiAfkKick_Dalamud
{
    unsafe class AntiAfkKick : IDalamudPlugin
    {
        public string Name => "AntiAfkKick-Dalamud";
        internal volatile bool running = true;
        int NextKeyPress = 0;
        float* AfkTimer;

        public void Dispose()
        {
            running = false;
        }

        public AntiAfkKick(DalamudPluginInterface pluginInterface)
        {
            pluginInterface.Create<Svc>();
            AfkTimer = (float*)((IntPtr)FFXIVClientStructs.FFXIV.Client.System.Framework.Framework.Instance()->GetUiModule()->GetRaptureAtkModule() + 0x276D0);
            
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
                        PluginLog.Information("Afk timer before: " + *AfkTimer);
                        PluginLog.Information($"Sending anti-afk keypress: {proc.MainWindowHandle:X16}");
                        new TickScheduler(delegate
                        {
                            SendMessage(proc.MainWindowHandle, WM_KEYDOWN, (IntPtr)LControlKey, (IntPtr)0);
                            new TickScheduler(delegate
                            {
                                SendMessage(proc.MainWindowHandle, WM_KEYUP, (IntPtr)LControlKey, (IntPtr)0);
                                PluginLog.Information("Afk timer after: " + *AfkTimer);
                            }, Svc.Framework, 200);
                        }, Svc.Framework, 0);
                        NextKeyPress = Environment.TickCount + new Random().Next(1 * 60 * 1000, 2 * 60 * 1000);
                    }
                    if(fgWH == proc.MainWindowHandle)
                    {
                        NextKeyPress = Environment.TickCount + 2*60*1000;
                    }
                    Thread.Sleep(10000);
                }
            }).Start();
        }
    }
}
/*var convertedCode = (byte)Svc.KeyState.GetType()
        .GetMethod("ConvertVirtualKey", BindingFlags.NonPublic | BindingFlags.Instance)
        .Invoke(Svc.KeyState, new object[] { (int)VirtualKey.LCONTROL });
    PluginLog.Information("Task run: " + convertedCode);
    if (convertedCode != 0)
    {
        var bufferBase = (IntPtr)Svc.KeyState.GetType().GetField("bufferBase", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(Svc.KeyState);
        *(int*)(bufferBase + (4 * convertedCode)) = 3;
    }
*/