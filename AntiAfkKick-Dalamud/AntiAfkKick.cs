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
        //long NextKeyPress = 0;
        float* AfkTimer;
        IntPtr mwh;

        public void Dispose()
        {
            running = false;
        }

        public AntiAfkKick(DalamudPluginInterface pluginInterface)
        {
            mwh = Process.GetCurrentProcess().MainWindowHandle;
            pluginInterface.Create<Svc>();
            AfkTimer = (float*)((IntPtr)FFXIVClientStructs.FFXIV.Client.System.Framework.Framework.Instance()->GetUiModule()->GetRaptureAtkModule() + 0x276D0);
            new Thread((ThreadStart)delegate
            {
                while (running)
                {
                    try
                    {
                        //var fgWH = Native.GetForegroundWindow();
                        var newHandle = Process.GetCurrentProcess().MainWindowHandle;
                        if (newHandle != IntPtr.Zero) mwh = newHandle;
                        PluginLog.Debug($"[Debug] proc.MWH:{mwh:X16}; GCP.MWH:{Process.GetCurrentProcess().MainWindowHandle:X16}");
                        //PluginLog.Debug($"[Debug] TickCount64:{Environment.TickCount64}/NextKeyPress:{NextKeyPress}");
                        PluginLog.Debug("Afk timer: " + *AfkTimer);
                        if (*AfkTimer > 2f*60f 
                            //Environment.TickCount64 > NextKeyPress &&
                            //(fgWH != mwh || Native.IdleTimeFinder.GetIdleTime() > 60 * 1000)
                            )
                        {
                            PluginLog.Debug("Afk timer before: " + *AfkTimer);
                            PluginLog.Debug($"Sending anti-afk keypress: {mwh:X16}");
                            new TickScheduler(delegate
                            {
                                SendMessage(mwh, WM_KEYDOWN, (IntPtr)LControlKey, (IntPtr)0);
                                new TickScheduler(delegate
                                {
                                    SendMessage(mwh, WM_KEYUP, (IntPtr)LControlKey, (IntPtr)0);
                                    PluginLog.Debug("Afk timer after: " + *AfkTimer);
                                }, Svc.Framework, 200);
                            }, Svc.Framework, 0);
                            //NextKeyPress = Environment.TickCount64 + new Random().Next(1 * 60 * 1000, 2 * 60 * 1000);
                        }
                        //if (fgWH == mwh)
                        //{
                        //    NextKeyPress = Environment.TickCount64 + 2 * 60 * 1000;
                        //}
                        Thread.Sleep(10000);
                    }
                    catch(Exception e)
                    {
                        PluginLog.Error(e.Message + "\n" + e.StackTrace ?? "");
                    }
                }
                PluginLog.Debug("Thread has stopped");
            }).Start();
        }
    }
}
/*var convertedCode = (byte)Svc.KeyState.GetType()
        .GetMethod("ConvertVirtualKey", BindingFlags.NonPublic | BindingFlags.Instance)
        .Invoke(Svc.KeyState, new object[] { (int)VirtualKey.LCONTROL });
    PluginLog.Debug("Task run: " + convertedCode);
    if (convertedCode != 0)
    {
        var bufferBase = (IntPtr)Svc.KeyState.GetType().GetField("bufferBase", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(Svc.KeyState);
        *(int*)(bufferBase + (4 * convertedCode)) = 3;
    }
*/