using System;
using Exiled.API.Features;
using Scp559.Utilities;

namespace Scp559;

// ReSharper disable ClassNeverInstantiated.Global
public class EntryPoint : Plugin<Config>
{
    public override string Author { get; } = "Draakoor";

    public override string Name { get; } = "SCP-559";

    public override string Prefix { get; } = "scp_559";

    public override Version Version { get; } = new(1, 2, 0);

    public override Version RequiredExiledVersion { get; } = new(9, 0, 0);

    internal static EntryPoint Instance;

    private Scp559Manager _scp559Manager;

    public override void OnEnabled()
    {
        if (!StartupChecks.CheckForMapEditorReborn())
        {
            Log.Error("MapEditorReborn is missing!, aborting plugin startup.");
            return;
        }

        Instance = this;

        _scp559Manager = new Scp559Manager(this);
        Exiled.Events.Handlers.Player.UsedItem += _scp559Manager.OnUsedItem;
        Exiled.Events.Handlers.Player.TogglingNoClip += _scp559Manager.OnToggleNoClip;
        Exiled.Events.Handlers.Player.VoiceChatting += _scp559Manager.OnVoiceChatting;
        Exiled.Events.Handlers.Player.Dying += _scp559Manager.OnDying;
        Exiled.Events.Handlers.Server.RoundStarted += _scp559Manager.OnRoundStart;
        Exiled.Events.Handlers.Server.EndingRound += _scp559Manager.OnEndingRound;
        
        // Internal
        Exiled.Events.Handlers.Server.WaitingForPlayers += OnWaitingForPlayers;
        
        base.OnEnabled();
    }

    public override void OnDisabled()
    {
        // Internal
        Exiled.Events.Handlers.Server.WaitingForPlayers -= OnWaitingForPlayers;
        
        Exiled.Events.Handlers.Player.UsedItem -= _scp559Manager.OnUsedItem;
        Exiled.Events.Handlers.Player.TogglingNoClip -= _scp559Manager.OnToggleNoClip;
        Exiled.Events.Handlers.Player.VoiceChatting -= _scp559Manager.OnVoiceChatting;
        Exiled.Events.Handlers.Player.Dying -= _scp559Manager.OnDying;
        Exiled.Events.Handlers.Server.RoundStarted -= _scp559Manager.OnRoundStart;
        Exiled.Events.Handlers.Server.EndingRound -= _scp559Manager.OnEndingRound;

        _scp559Manager = null;
        Instance = null;
        base.OnDisabled();
    }

    private static void OnWaitingForPlayers() => StartupChecks.UnRegisterIncompatibilities();
}