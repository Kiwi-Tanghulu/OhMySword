using H00N.Network;
using System;

namespace Packets
{
    public class C_RoomEnterPacket : Packet
    {
        public override ushort ID => (ushort)PacketID.C_RoomEnterPacket;

        public string nickname;
        public ushort skinID;

        public override void Deserialize(ArraySegment<byte> buffer)
        {
            ushort process = 0;

            process += sizeof(ushort);
            process += sizeof(ushort);

            process += PacketUtility.ReadStringData(buffer, process, out nickname);
            process += PacketUtility.ReadUShortData(buffer, process, out skinID);
        }

        public override ArraySegment<byte> Serialize()
        {
            ArraySegment<byte> buffer = UniqueBuffer.Open(1024);
            ushort process = 0;

            process += sizeof(ushort);
            process += PacketUtility.AppendUShortData(this.ID, buffer, process);
            process += PacketUtility.AppendStringData(this.nickname, buffer, process);
            process += PacketUtility.AppendUShortData(this.skinID, buffer, process);
            PacketUtility.AppendUShortData(process, buffer, 0);

            return UniqueBuffer.Close(process);
        }
    }
}
