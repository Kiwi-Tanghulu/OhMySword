using Packets;
using UnityEngine;

public static class VectorExtensions
{
    public static Vector3 Vector3(this VectorPacket left) => new Vector3(left.x, left.y, left.z); 
}
