using H00N.Network;
using System;

namespace Packets
{
    public class S_ScoreBoxPacket : Packet
    {
        public override ushort ID => (ushort)PacketID.S_ScoreBoxPacket;

        public ushort objectID;
        public ushort posTableIndex;

        public S_ScoreBoxPacket() { }

        public S_ScoreBoxPacket(ushort objectID, ushort posTableIndex)
        {
            this.objectID = objectID;
            this.posTableIndex = posTableIndex;
        }

        public override void Deserialize(ArraySegment<byte> buffer)
        {
            ushort process = 0;

            process += sizeof(ushort);
            process += sizeof(ushort);

            process += PacketUtility.ReadUShortData(buffer, process, out objectID);
            process += PacketUtility.ReadUShortData(buffer, process, out posTableIndex);
        }

        public override ArraySegment<byte> Serialize()
        {
            ArraySegment<byte> buffer = UniqueBuffer.Open(1024);
            ushort process = 0;

            process += sizeof(ushort);
            process += PacketUtility.AppendUShortData(this.ID, buffer, process);
            process += PacketUtility.AppendUShortData(this.objectID, buffer, process);
            process += PacketUtility.AppendUShortData(this.posTableIndex, buffer, process);
            PacketUtility.AppendUShortData(process, buffer, 0);

            return UniqueBuffer.Close(process);
        }
    }
}
