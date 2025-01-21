using System;
using UnityEngine;

namespace Scp559.Utilities.RoomPoint;

[Serializable]
public class SerializedVector3
{
    public SerializedVector3(Vector3 vector)
    {
        X = vector.x;
        Y = vector.y;
        Z = vector.z;
    }

    public SerializedVector3(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public SerializedVector3() { }

    public Vector3 Parse() => new(X, Y, Z);

    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }

    public static implicit operator Vector3(SerializedVector3 vector) => vector?.Parse() ?? Vector3.zero;
    public static implicit operator SerializedVector3(Vector3 vector) => new(vector);
    public static implicit operator SerializedVector3(Quaternion rotation) => new(rotation.eulerAngles);
    public static implicit operator Quaternion(SerializedVector3 vector) => Quaternion.Euler(vector);
}