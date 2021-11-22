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
        float* AfkTimer2;
        float* AfkTimer3;

        public void Dispose()
        {
            running = false;
        }

        public AntiAfkKick(DalamudPluginInterface pluginInterface)
        {
            pluginInterface.Create<Svc>();
            AfkTimer = (float*)((IntPtr)FFXIVClientStructs.FFXIV.Client.System.Framework.Framework.Instance()->GetUiModule()->GetRaptureAtkModule() + 161480);
            AfkTimer2 = (float*)((IntPtr)FFXIVClientStructs.FFXIV.Client.System.Framework.Framework.Instance()->GetUiModule()->GetRaptureAtkModule() + 161508);
            AfkTimer3 = (float*)((IntPtr)FFXIVClientStructs.FFXIV.Client.System.Framework.Framework.Instance()->GetUiModule()->GetRaptureAtkModule() + 161488);
            new Thread((ThreadStart)delegate
            {
                while (running)
                {
                    try
                    {
                        PluginLog.Debug($"Afk timers: {*AfkTimer}/{*AfkTimer2}/{*AfkTimer3}");
                        if (Max(*AfkTimer, *AfkTimer2, *AfkTimer3) > 2f * 60f)
                        {
                            if (Native.TryFindGameWindow(out var mwh))
                            {
                                PluginLog.Debug($"Afk timer before: {*AfkTimer}/{*AfkTimer2}/{*AfkTimer3}");
                                PluginLog.Debug($"Sending anti-afk keypress: {mwh:X16}");
                                new TickScheduler(delegate
                                {
                                    SendMessage(mwh, WM_KEYDOWN, (IntPtr)LControlKey, (IntPtr)0);
                                    new TickScheduler(delegate
                                    {
                                        SendMessage(mwh, WM_KEYUP, (IntPtr)LControlKey, (IntPtr)0);
                                        PluginLog.Debug($"Afk timer after: {*AfkTimer}/{*AfkTimer2}/{*AfkTimer3}");
                                    }, Svc.Framework, 200);
                                }, Svc.Framework, 0);
                            }
                            else
                            {
                                PluginLog.Error("Could not find game window");
                            }
                        }
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
        public static float Max(params float[] values)
        {
            return values.Max();
        }
    }
}