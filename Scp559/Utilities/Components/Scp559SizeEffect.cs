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
        if (!(_player.Scale.y > EntryPoint.Instance.Config.CakeConfig.PlayerScaleUnderCakeEffect.y)) return;
        
        _player.EnableEffect(EffectType.Ensnared, duration: 1f);
        _player.Scale -= new Vector3(0.1f, 0.1f, 0.1f) * Time.deltaTime;

        if (!(_player.Scale.y < EntryPoint.Instance.Config.CakeConfig.PlayerScaleUnderCakeEffect.y)) return;
        
        _player.Scale = EntryPoint.Instance.Config.CakeConfig.PlayerScaleUnderCakeEffect;
        Destroy(this);
    }

    private void OnDestroy() => _player = null;
}