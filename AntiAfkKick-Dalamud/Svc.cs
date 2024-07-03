using Dalamud.Game;
using Dalamud.Game.ClientState.Objects;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;

namespace AntiAfkKick
{
    internal class Svc
    {
        [PluginService] internal static IDalamudPluginInterface PluginInterface { get; private set; }
        [PluginService] internal static IBuddyList Buddies { get; private set; }
        [PluginService] internal static IChatGui Chat { get; private set; }
        [PluginService] internal static IClientState ClientState { get; private set; }
        [PluginService] internal static ICommandManager Commands { get; private set; }
        [PluginService] internal static ICondition Condition { get; private set; }
        [PluginService] internal static IDataManager Data { get; private set; }
        [PluginService] internal static IFateTable Fates { get; private set; }
        [PluginService] internal static IFlyTextGui FlyText { get; private set; }
        [PluginService] internal static IFramework Framework { get; private set; }
        [PluginService] internal static IGameGui GameGui { get; private set; }
        [PluginService] internal static IGameNetwork GameNetwork { get; private set; }
        [PluginService] internal static IJobGauges Gauges { get; private set; }
        [PluginService] internal static IKeyState KeyState { get; private set; }
        [PluginService] internal static IObjectTable Objects { get; private set; }
        [PluginService] internal static IPartyFinderGui PfGui { get; private set; }
        [PluginService] internal static IPartyList Party { get; private set; }
        [PluginService] internal static ISigScanner SigScanner { get; private set; }
        [PluginService] internal static ITargetManager Targets { get; private set; }
        [PluginService] internal static IToastGui Toasts { get; private set; }
        [PluginService] internal static IGameInteropProvider Hook { get; private set; }
        [PluginService] internal static IPluginLog Log { get; private set; }
    }
}