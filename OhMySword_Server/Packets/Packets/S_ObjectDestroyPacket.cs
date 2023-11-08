using H00N.Network;
using System;

namespace Packets
{
    public class S_ObjectDestroyPacket : Packet
    {
        public override ushort ID => (ushort)PacketID.S_ObjectDestroyPacket;

        public ushort objectID;

        public S_ObjectDestroyPacket() { }

        public S_ObjectDestroyPacket(ushort objectID)
        {
            this.objectID = objectID;
        }

        public override void Deserialize(ArraySegment<byte> buffer)
        {
            ushort process = 0;

            process += sizeof(ushort);
            process += sizeof(ushort);

            process += PacketUtility.ReadUShortData(buffer, process, out objectID);
        }

        public override ArraySegment<byte> Serialize()
        {
            ArraySegment<byte> buffer = UniqueBuffer.Open(1024);
            ushort process = 0;

            process += sizeof(ushort);
            process += PacketUtility.AppendUShortData(this.ID, buffer, process);
            process += PacketUtility.AppendUShortData(this.objectID, buffer, process);
            PacketUtility.AppendUShortData(process, buffer, 0);

            return UniqueBuffer.Close(process);
        }
    }
}
