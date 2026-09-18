using System.Collections.Generic;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using ProjectMER.Features;
using ProjectMER.Features.Objects;
using MEC;
using Scp559.Utilities.Components;
using Scp559.Utilities.Voice;
using UnityEngine;
using Random = System.Random;

namespace Scp559;

public class Scp559Manager
{
    private readonly EntryPoint _entryPoint;
    
    public Scp559Manager(EntryPoint entryPoint) => _entryPoint = entryPoint;

    private SchematicObject _cakeModel;
    private CoroutineHandle _spawnRoutine, _hintRoutine;

    internal void OnUsedItem(UsedItemEventArgs args)
    {
        if (args.Item?.Type is not ItemType.SCP500)
            return;

        if (args.Player.Scale == Vector3.one)
            return;

        if (!args.Player.GameObject.TryGetComponent(out Scp559SizeEffect scp559Effect))
            return;
        
        Object.Destroy(scp559Effect);
        args.Player.GameObject.AddComponent<Scp559RestoreEffect>().InitializeComponent(args.Player);
    }

    internal void OnToggleNoClip(TogglingNoClipEventArgs args)
    {
        if (args.Player.IsNoclipPermitted || args.Player.IsScp)
            return;
        
        if (_cakeModel is null)
            return;
        
        args.IsAllowed = false;

        if (Vector3.Distance(args.Player.Position, _cakeModel.Position) >= 2.5f)
            return;
        
        if (!args.Player.GameObject.TryGetComponent(out Scp559SizeEffect _))
            args.Player.GameObject.AddComponent<Scp559SizeEffect>().InitializeComponent(args.Player);
    }

    internal void OnVoiceChatting(VoiceChattingEventArgs args)
    {
        if (!args.Player.GameObject.TryGetComponent(out Scp559SizeEffect _))
            return;

        args.VoiceMessage = VoicePitchUtilities.SetVoicePitch(args.Player.ReferenceHub, args.VoiceMessage);
    }

    internal void OnDying(DyingEventArgs args)
    {
        if (!args.Player.GameObject.TryGetComponent(out Scp559SizeEffect scp559Effect))
            return;
        
        Object.Destroy(scp559Effect);
        args.Player.Scale = Vector3.one;
    }

    internal void OnRoundStart()
    {
        Debug.Log("roundstart for scp559 detected");
        Cleanup();
        _spawnRoutine = Timing.RunCoroutine(CakeSpawnHandler());
        _hintRoutine = Timing.RunCoroutine(SlicePickupIndicator());
    }

    internal void OnEndingRound(RoundEndedEventArgs ev) { Cleanup(); }

    internal void OnChangingRole(ChangingRoleEventArgs ev)
    {
        if (!ev.IsAllowed) return;
        var effect = ev.Player.GameObject.GetComponent<Scp559SizeEffect>();
        var restore = ev.Player.GameObject.GetComponent<Scp559RestoreEffect>();
        if (effect == null && restore == null) return;
        if (effect != null) Object.Destroy(effect);
        if (restore != null) Object.Destroy(restore);
        ev.Player.Scale = Vector3.one;
    }

    internal void Cleanup()
    {
        Timing.KillCoroutines(_spawnRoutine, _hintRoutine);
        if (_cakeModel != null) _cakeModel.Destroy();
        _cakeModel = null;
        foreach (var player in Player.List)
        {
            var effect = player.GameObject.GetComponent<Scp559SizeEffect>();
            var restore = player.GameObject.GetComponent<Scp559RestoreEffect>();
            if (effect == null && restore == null) continue;
            if (effect != null) Object.Destroy(effect);
            if (restore != null) Object.Destroy(restore);
            player.Scale = Vector3.one;
        }
    }

    private IEnumerator<float> CakeSpawnHandler()
    {
        yield return Timing.WaitForSeconds(_entryPoint.Config.CakeConfig.FirstCakeSpawnDelay);
        Debug.Log("spawning cake in "+ _entryPoint.Config.CakeConfig.FirstCakeSpawnDelay+" seconds");
        while (true)
        {
            if (Round.IsEnded)
                yield break;

            var rooms = Room.List.Where(r => _entryPoint.Config.CakeConfig.SpawnPoints.ContainsKey(r.Type)).ToArray();
            if (rooms.Length == 0) { Log.Warn("SCP-559: no configured room exists in this map."); yield break; }
            Room room = rooms[UnityEngine.Random.Range(0, rooms.Length)];
            Vector3 spawnPoint = room.WorldPosition(_entryPoint.Config.CakeConfig.SpawnPoints[room.Type]);
            if (Physics.Raycast(spawnPoint + Vector3.up * 2, Vector3.down, out var ground, 6, PlayerRoles.FirstPersonControl.FpcStateProcessor.Mask))
                spawnPoint.y = ground.point.y;
            if (!ObjectSpawner.TrySpawnSchematic(_entryPoint.Config.CakeConfig.SchematicName, spawnPoint, out _cakeModel))
            { Log.Error("SCP-559: ProjectMER schematic missing: " + _entryPoint.Config.CakeConfig.SchematicName); yield break; }
            Log.Info("SCP-559 spawned in " + room.Type + " at " + _cakeModel.Position);

            yield return Timing.WaitForSeconds(_entryPoint.Config.CakeConfig.DisappearDelay);
            
            if (_cakeModel != null) _cakeModel.Destroy();
            _cakeModel = null;

            ServerConsole.AddLog("cake despawned");

            yield return Timing.WaitForSeconds(_entryPoint.Config.CakeConfig.NormalSpawnDelay);
            Debug.Log("cake respawning");
        }
    }
    
    private IEnumerator<float> SlicePickupIndicator()
    {
        while (true)
        {
            if (Round.IsEnded)
                yield break;

            foreach (Player player in Player.List)
            {
                if (_cakeModel is not null && player.IsHuman && !player.IsNPC && !string.IsNullOrWhiteSpace(_entryPoint.Config.CakeConfig.SlicePickupHint) && Vector3.Distance(player.Position, _cakeModel.Position) <= 2.5f)
                {
                    player.ShowHint(_entryPoint.Config.CakeConfig.SlicePickupHint, 1.1f);
                }
            }

            yield return Timing.WaitForSeconds(1f);
        }
    }

    private RoomType GetRandomRoom()
    {
        Random random = new Random();

        List<RoomType> roomNames = _entryPoint.Config.CakeConfig.SpawnPoints.Keys.ToList();

        int index = random.Next(roomNames.Count);

        return roomNames[index];
    }
}
