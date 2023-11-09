using H00N.Network;
using System;

namespace Packets
{
    public class VectorPacket : DataPacket
    {
        public float x;
        public float y;
        public float z;

        public VectorPacket() { }

        public VectorPacket(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public override ushort Deserialize(ArraySegment<byte> buffer, int offset)
        {
            ushort process = 0;
            process += PacketUtility.ReadFloatData(buffer, offset + process, out this.x);
            process += PacketUtility.ReadFloatData(buffer, offset + process, out this.y);
            process += PacketUtility.ReadFloatData(buffer, offset + process, out this.z);

            return process;
        }

        public override ushort Serialize(ArraySegment<byte> buffer, int offset)
        {
            ushort process = 0;
            process += PacketUtility.AppendFloatData(this.x, buffer, offset + process);
            process += PacketUtility.AppendFloatData(this.y, buffer, offset + process);
            process += PacketUtility.AppendFloatData(this.z, buffer, offset + process);

            return process;
        }
    }
}
