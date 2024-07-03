using AntiAfkKick;
using Dalamud.Game.ClientState.Keys;
using Dalamud.Hooking;
using Dalamud.Logging;
using Dalamud.Plugin;
using FFXIVClientStructs.FFXIV.Client.UI;
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
        public volatile int TimerLimit = 30;
        public volatile int ThreadSleepInterval = 10000;

        public void Dispose()
        {
            running = false;
            Svc.Commands.RemoveHandler("/aak");
        }

        public AntiAfkKick(IDalamudPluginInterface pluginInterface)
        {
            pluginInterface.Create<Svc>();
            Svc.Commands.AddHandler("/aak", new(OnCommand) { ShowInHelp = false });
            BeginWork();
        }

        private void OnCommand(string command, string arguments)
        {
            if(int.TryParse(arguments.Split(" ")[0], out var tlimit) && int.TryParse(arguments.Split(" ")[1], out var tsleep))
            {
                TimerLimit = tlimit;
                ThreadSleepInterval = tsleep;
                Svc.Chat.Print($"TimerLimit={TimerLimit}, ThreadSleepInterval={ThreadSleepInterval}");
            }
        }

        void BeginWork()
        {
            float*[] afkPtrs = [
                 &UIModule.Instance()->GetInputTimerModule()->AfkTimer,
                 &UIModule.Instance()->GetInputTimerModule()->ContentInputTimer,
                 &UIModule.Instance()->GetInputTimerModule()->InputTimer,
            ];
            float[] GetTimers()
            {
                var timers = new float[afkPtrs.Length];
                for (int i = 0; i < timers.Length; i++)
                {
                    timers[i] = *afkPtrs[i];
                }
                return timers;
            }
            new Thread((ThreadStart)delegate
            {
                while (running)
                {
                    try
                    {
                        
                        Svc.Log.Verbose($"Afk timers: {string.Join(",", GetTimers().Select(x => x.ToString()))}");
                        if (GetTimers().Max() > TimerLimit)
                        {
                            if (Native.TryFindGameWindow(out var mwh))
                            {
                                Svc.Log.Verbose($"Afk timer before: {string.Join(",", GetTimers().Select(x => x.ToString()))}");
                                Svc.Log.Verbose($"Sending anti-afk keypress: {mwh:X16}");
                                new TickScheduler(delegate
                                {
                                    SendMessage(mwh, WM_KEYDOWN, (IntPtr)LControlKey, (IntPtr)0);
                                    new TickScheduler(delegate
                                    {
                                        SendMessage(mwh, WM_KEYUP, (IntPtr)LControlKey, (IntPtr)0);
                                        Svc.Log.Verbose($"Afk timer after: {string.Join(",", GetTimers().Select(x => x.ToString()))}");
                                    }, Svc.Framework, 200);
                                }, Svc.Framework, 0);
                            }
                            else
                            {
                                Svc.Log.Error("Could not find game window");
                            }
                        }
                        Thread.Sleep(ThreadSleepInterval);
                    }
                    catch (Exception e)
                    {
                        Svc.Log.Error(e.Message + "\n" + e.StackTrace ?? "");
                    }
                }
                Svc.Log.Debug("Thread has stopped");
            }).Start();
        }

        public static float Max(params float[] values)
        {
            return values.Max();
        }
    }
}