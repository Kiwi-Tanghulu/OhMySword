using H00N.Network;
using System;

namespace Packets
{
    public class S_ChattingPacket : Packet
    {
        public override ushort ID => (ushort)PacketID.S_ChattingPacket;

        public string chat;
        public ushort playerID;

        public S_ChattingPacket() { }

        public S_ChattingPacket(string chat, ushort playerID)
        {
            this.chat = chat;
            this.playerID = playerID;
        }

        public override void Deserialize(ArraySegment<byte> buffer)
        {
            ushort process = 0;

            process += sizeof(ushort);
            process += sizeof(ushort);

            process += PacketUtility.ReadStringData(buffer, process, out chat);
            process += PacketUtility.ReadUShortData(buffer, process, out playerID);
        }

        public override ArraySegment<byte> Serialize()
        {
            ArraySegment<byte> buffer = UniqueBuffer.Open(1024);
            ushort process = 0;

            process += sizeof(ushort);
            process += PacketUtility.AppendUShortData(this.ID, buffer, process);
            process += PacketUtility.AppendStringData(this.chat, buffer, process);
            process += PacketUtility.AppendUShortData(this.playerID, buffer, process);
            PacketUtility.AppendUShortData(process, buffer, 0);

            return UniqueBuffer.Close(process);
        }
    }
}
