using Exiled.API.Enums;
using Exiled.API.Features;
using UnityEngine;

namespace Scp559.Utilities.Components;

public class Scp559SizeEffect : MonoBehaviour
{
    internal void InitializeComponent(Player player) => _player = player;
    
    private Player _player;
    
    private void Update()
    {
        if (_player == null || !_player.IsConnected || !_player.IsAlive) { Destroy(this); return; }
        if (!(_player.Scale.y > EntryPoint.Instance.Config.CakeConfig.PlayerScaleUnderCakeEffect.y)) return;
        
        _player.EnableEffect(EffectType.Ensnared, duration: 1f);
        _player.Scale -= new Vector3(0.1f, 0.1f, 0.1f) * Time.deltaTime;

        if (!(_player.Scale.y < EntryPoint.Instance.Config.CakeConfig.PlayerScaleUnderCakeEffect.y)) return;
        
        _player.Scale = EntryPoint.Instance.Config.CakeConfig.PlayerScaleUnderCakeEffect;
        // Keep the component while affected so SCP-500 and voice processing can identify it.
    }

    private void OnDestroy()
    {
        if (_player != null) Scp559.Utilities.Voice.VoicePitchUtilities.Forget(_player.ReferenceHub);
        _player = null;
    }
}
