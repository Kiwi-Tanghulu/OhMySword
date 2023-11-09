using H00N.Network;
using System;

namespace Packets
{
    public class C_ChattingPacket : Packet
    {
        public override ushort ID => (ushort)PacketID.C_ChattingPacket;

        public string chat;

        public C_ChattingPacket() { }

        public C_ChattingPacket(string chat)
        {
            this.chat = chat;
        }

        public override void Deserialize(ArraySegment<byte> buffer)
        {
            ushort process = 0;

            process += sizeof(ushort);
            process += sizeof(ushort);

            process += PacketUtility.ReadStringData(buffer, process, out chat);
        }

        public override ArraySegment<byte> Serialize()
        {
            ArraySegment<byte> buffer = UniqueBuffer.Open(1024);
            ushort process = 0;

            process += sizeof(ushort);
            process += PacketUtility.AppendUShortData(this.ID, buffer, process);
            process += PacketUtility.AppendStringData(this.chat, buffer, process);
            PacketUtility.AppendUShortData(process, buffer, 0);

            return UniqueBuffer.Close(process);
        }
    }
}
