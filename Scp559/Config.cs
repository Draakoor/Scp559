using System.Collections.Generic;
using System.ComponentModel;
using Exiled.API.Enums;
using Exiled.API.Interfaces;
using UnityEngine;

namespace Scp559;

public class Config : IConfig
{
    [Description("Is the Plugin Enabled?")]
    public bool IsEnabled { get; set; } = true;

    [Description("Enable this if you want to be spammed with extra logs in your console")]
    public bool Debug { get; set; } = false;
    
    [Description("SCP-559 Configuration")]
    public CakeConfig CakeConfig { get; set; } = new();
}

public class CakeConfig
{
    [Description("The Schematic to Spawn (Change this if you want to use a custom schematic instead of the default one)")]
    public string SchematicName { get; set; } = "SCP559";
    
    [Description("The Hint to show to the players near the Cake")]
    public string SlicePickupHint { get; set; } = string.Empty;
    
    [Description("The Pitch to Apply when the player is under the SCP-559 effect")]
    public float VoicePitch { get; set; } = 1.5f;
    
    [Description("The delay for the first cake spawn")]
    public float FirstCakeSpawnDelay { get; set; } = 20f;
    
    [Description("The delay for the cake to disappear")]
    public float DisappearDelay { get; set; } = 30f;
    
    [Description("The delay for the normal cake spawn")]
    public float NormalSpawnDelay { get; set; } = 20f;

    [Description("The Target Player Scale when eating the cake slice")]
    public Vector3 PlayerScaleUnderCakeEffect { get; set; } = new(0.6f, 0.6f, 0.6f);
    
    [Description("The Places and positions where the cake can spawn")]
    public Dictionary<RoomType, Vector3> SpawnPoints { get; set; } = new();
}