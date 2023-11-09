using H00N.Network;
using System;

namespace Packets
{
    public class S_PlayerPacket : Packet
    {
        public override ushort ID => (ushort)PacketID.S_PlayerPacket;

        public ObjectPacket objectPacket;

        public override void Deserialize(ArraySegment<byte> buffer)
        {
            ushort process = 0;

            process += sizeof(ushort);
            process += sizeof(ushort);

            process += PacketUtility.ReadDataPacket<ObjectPacket>(buffer, process, out objectPacket);
        }

        public override ArraySegment<byte> Serialize()
        {
            ArraySegment<byte> buffer = UniqueBuffer.Open(1024);
            ushort process = 0;

            process += sizeof(ushort);
            process += PacketUtility.AppendUShortData(this.ID, buffer, process);
            process += PacketUtility.AppendDataPacket<ObjectPacket>(this.objectPacket, buffer, process);
            PacketUtility.AppendUShortData(process, buffer, 0);

            return UniqueBuffer.Close(process);
        }

        public static implicit operator S_PlayerPacket(C_PlayerPacket right) 
            => new S_PlayerPacket() { objectPacket = right.objectPacket };
    }
}
