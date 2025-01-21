using Exiled.API.Enums;
using Exiled.API.Features;
using UnityEngine;

namespace Scp559.Utilities.Components;

public class Scp559RestoreEffect : MonoBehaviour
{
    internal void InitializeComponent(Player player) => _player = player;
    
    private Player _player;
    
    private void Update()
    {
        _player.EnableEffect(EffectType.Ensnared, duration: 1f);
        _player.Scale += new Vector3(0.1f, 0.1f, 0.1f) * Time.deltaTime;
            
        if (!(_player.Scale.y > 1f)) return;
        
        _player.Scale = new Vector3(1, 1, 1);
        Destroy(this);
    }

    private void OnDestroy() => _player = null;
}