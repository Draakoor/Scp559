using System;
using System.Diagnostics.CodeAnalysis;
using CommandSystem;
using Exiled.API.Features;
using Scp559.Utilities.RoomPoint;
using UnityEngine;

namespace Scp559.Commands;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class RoomPoint : ICommand
{
    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, [UnscopedRef] out string response)
    {
        Player player = Player.Get(sender);
        
        Transform cameraTransform = player.CameraTransform.transform;
        
        Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit raycastHit, 100f);
        
        RoomPointObject point = new RoomPointObject(raycastHit.point + Vector3.up * 0.1f);
        
        response = $"\nThe position you are looking at as RoomPoint:" +
                   $"\n  RoomType: {point.roomType}" +
                   $"\n  X: {point.relativePosition.X}" +
                   $"\n  Y: {point.relativePosition.Y}" +
                   $"\n  Z: {point.relativePosition.Z}";
        
        return true;
    }

    public string Command { get; } = "roompoint";

    public string[] Aliases { get; } = { "rp" };
    
    public string Description { get; } = "Gets the local position you're looking with the camera";
}