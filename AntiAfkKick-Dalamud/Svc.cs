using Dalamud.Data;
using Dalamud.Game;
using Dalamud.Game.ClientState;
using Dalamud.Game.ClientState.Buddy;
using Dalamud.Game.ClientState.Conditions;
using Dalamud.Game.ClientState.Fates;
using Dalamud.Game.ClientState.JobGauge;
using Dalamud.Game.ClientState.Keys;
using Dalamud.Game.ClientState.Objects;
using Dalamud.Game.ClientState.Party;
using Dalamud.Game.Command;
using Dalamud.Game.Gui;
using Dalamud.Game.Gui.FlyText;
using Dalamud.Game.Gui.PartyFinder;
using Dalamud.Game.Gui.Toast;
using Dalamud.Game.Libc;
using Dalamud.Game.Network;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;

namespace AntiAfkKick
{
	class Svc
	{
		[PluginService] static internal DalamudPluginInterface PluginInterface { get; private set; }
		[PluginService] static internal IBuddyList Buddies { get; private set; }
		[PluginService] static internal IChatGui Chat { get; private set; }
		[PluginService] static internal IClientState ClientState { get; private set; }
		[PluginService] static internal ICommandManager Commands { get; private set; }
		[PluginService] static internal ICondition Condition { get; private set; }
		[PluginService] static internal IDataManager Data { get; private set; }
		[PluginService] static internal IFateTable Fates { get; private set; }
		[PluginService] static internal IFlyTextGui FlyText { get; private set; }
		[PluginService] static internal IFramework Framework { get; private set; }
		[PluginService] static internal IGameGui GameGui { get; private set; }
		[PluginService] static internal IGameNetwork GameNetwork { get; private set; }
		[PluginService] static internal IJobGauges Gauges { get; private set; }
		[PluginService] static internal IKeyState KeyState { get; private set; }
		[PluginService] static internal ILibcFunction LibcFunction { get; private set; }
		[PluginService] static internal IObjectTable Objects { get; private set; }
		[PluginService] static internal IPartyFinderGui PfGui { get; private set; }
		[PluginService] static internal IPartyList Party { get; private set; }
		[PluginService] static internal ISigScanner SigScanner { get; private set; }
		[PluginService] static internal ITargetManager Targets { get; private set; }
        [PluginService] static internal IToastGui Toasts { get; private set; }
        [PluginService] static internal IGameInteropProvider Hook { get; private set; }
        [PluginService] static internal IPluginLog Log { get; private set; }
    }
}