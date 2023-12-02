using H00N.Network;
using System;

namespace Packets
{
    public class S_OtherJoinPacket : Packet
    {
        public override ushort ID => (ushort)PacketID.S_OtherJoinPacket;

        public string nickname;
        public ushort playerID;
        public ushort skinID;
        public ushort posTableIndex;

        public S_OtherJoinPacket() { }

        public S_OtherJoinPacket(string nickname, ushort playerID, ushort skinID, ushort posTableIndex)
        {
            this.nickname = nickname;
            this.playerID = playerID;
            this.skinID = skinID;
            this.posTableIndex = posTableIndex;
        }

        public override void Deserialize(ArraySegment<byte> buffer)
        {
            ushort process = 0;

            process += sizeof(ushort);
            process += sizeof(ushort);

            process += PacketUtility.ReadStringData(buffer, process, out nickname);
            process += PacketUtility.ReadUShortData(buffer, process, out playerID);
            process += PacketUtility.ReadUShortData(buffer, process, out skinID);
            process += PacketUtility.ReadUShortData(buffer, process, out posTableIndex);
        }

        public override ArraySegment<byte> Serialize()
        {
            ArraySegment<byte> buffer = UniqueBuffer.Open(1024);
            ushort process = 0;

            process += sizeof(ushort);
            process += PacketUtility.AppendUShortData(this.ID, buffer, process);
            process += PacketUtility.AppendStringData(this.nickname, buffer, process);
            process += PacketUtility.AppendUShortData(this.playerID, buffer, process);
            process += PacketUtility.AppendUShortData(this.skinID, buffer, process);
            process += PacketUtility.AppendUShortData(this.posTableIndex, buffer, process);
            PacketUtility.AppendUShortData(process, buffer, 0);

            return UniqueBuffer.Close(process);
        }
    }
}
