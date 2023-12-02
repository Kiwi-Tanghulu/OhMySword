using H00N.Network;
using System;

namespace Packets
{
    public class S_EventStartPacket : Packet
    {
        public override ushort ID => (ushort)PacketID.S_EventStartPacket;

        public ushort eventType;
        public ushort param;

        public S_EventStartPacket() { }
        public S_EventStartPacket(ushort eventType, ushort param) 
        {
            this.eventType = eventType;
            this.param = param;
        }

        public override void Deserialize(ArraySegment<byte> buffer)
        {
            ushort process = 0;

            process += sizeof(ushort);
            process += sizeof(ushort);

            process += PacketUtility.ReadUShortData(buffer, process, out eventType);
            process += PacketUtility.ReadUShortData(buffer, process, out param);
        }

        public override ArraySegment<byte> Serialize()
        {
            ArraySegment<byte> buffer = UniqueBuffer.Open(1024);
            ushort process = 0;

            process += sizeof(ushort);
            process += PacketUtility.AppendUShortData(this.ID, buffer, process);
            process += PacketUtility.AppendUShortData(this.eventType, buffer, process);
            process += PacketUtility.AppendUShortData(this.param, buffer, process);
            PacketUtility.AppendUShortData(process, buffer, 0);

            return UniqueBuffer.Close(process);
        }
    }
}
