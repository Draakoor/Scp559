using System;
using Exiled.API.Enums;
using Exiled.API.Features;
using UnityEngine;

namespace Scp559.Utilities.RoomPoint;

[Serializable]
public class RoomPointObject
{
    public RoomPointObject() { }

    public RoomPointObject(RoomType type, Vector3 relative)
    {
        roomType = type;
        relativePosition = relative;
    }

    public RoomPointObject(Vector3 mapPosition) : this(Room.Get(mapPosition).Type, mapPosition) { }
    
    public RoomType roomType = RoomType.Unknown;
    
    public SerializedVector3 relativePosition = Vector3.zero;
}