using Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public struct Vector3
    {
        public float x;
        public float y;
        public float z;

        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static Vector3 operator+(Vector3 left, Vector3 right)
        {
            left.x += right.x;
            left.y += right.y;
            left.z += right.z;

            return left;
        }

        public static implicit operator Vector3(VectorPacket right) =>  new Vector3(right.x, right.y, right.z);
        public static implicit operator VectorPacket(Vector3 right) =>  new VectorPacket(right.x, right.y, right.z);
    }
}
