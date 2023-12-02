using H00N.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Packets
{
    public class C_ChickenHitPacket : Packet
    {
        public override ushort ID => (ushort)PacketID.C_ChickenHitPacket;

        public VectorPacket position;

        public C_ChickenHitPacket() { }

        public C_ChickenHitPacket(ushort playerID, VectorPacket position)
        {
            this.position = position;
        }

        public override void Deserialize(ArraySegment<byte> buffer)
        {
            ushort process = 0;

            process += sizeof(ushort);
            process += sizeof(ushort);

            process += PacketUtility.ReadDataPacket<VectorPacket>(buffer, process, out position);
        }

        public override ArraySegment<byte> Serialize()
        {
            ArraySegment<byte> buffer = UniqueBuffer.Open(1024);
            ushort process = 0;

            process += sizeof(ushort);
            process += PacketUtility.AppendUShortData(this.ID, buffer, process);
            process += PacketUtility.AppendDataPacket<VectorPacket>(this.position, buffer, process);
            PacketUtility.AppendUShortData(process, buffer, 0);

            return UniqueBuffer.Close(process);
        }
    }
}
