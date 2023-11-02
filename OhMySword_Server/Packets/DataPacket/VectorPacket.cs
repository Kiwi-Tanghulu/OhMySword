using H00N.Network;
using System;

namespace Packets
{
    public class VectorPacket : DataPacket
    {
        public short x;
        public short y;
        public short z;

        public VectorPacket() { }

        public VectorPacket(short x, short y, short z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public VectorPacket(float x, float y, float z)
        {
            this.x = (short)x;
            this.y = (short)y;
            this.z = (short)z;
        }

        public override ushort Deserialize(ArraySegment<byte> buffer, int offset)
        {
            ushort process = 0;
            process += PacketUtility.ReadShortData(buffer, offset + process, out this.x);
            process += PacketUtility.ReadShortData(buffer, offset + process, out this.y);
            process += PacketUtility.ReadShortData(buffer, offset + process, out this.z);

            return process;
        }

        public override ushort Serialize(ArraySegment<byte> buffer, int offset)
        {
            ushort process = 0;
            process += PacketUtility.AppendShortData(this.x, buffer, offset + process);
            process += PacketUtility.AppendShortData(this.y, buffer, offset + process);
            process += PacketUtility.AppendShortData(this.z, buffer, offset + process);

            return process;
        }
    }
}
