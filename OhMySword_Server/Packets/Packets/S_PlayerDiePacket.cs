using H00N.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Packets
{
    public class S_PlayerDiePacket : Packet
    {
        public override ushort ID => (ushort)PacketID.S_PlayerDiePacket;

        public ushort attakerID;
        public ushort playerID;
        public ushort score;
        public List<VectorPacket> positions;

        public S_PlayerDiePacket() { }

        public S_PlayerDiePacket(ushort attakerID, ushort playerID, ushort score, List<VectorPacket> positions)
        {
            this.attakerID = attakerID;
            this.playerID = playerID;
            this.score = score;
            this.positions = positions;
        }

        public override void Deserialize(ArraySegment<byte> buffer)
        {
            ushort process = 0;

            process += sizeof(ushort);
            process += sizeof(ushort);

            process += PacketUtility.ReadUShortData(buffer, process, out attakerID);
            process += PacketUtility.ReadUShortData(buffer, process, out playerID);
            process += PacketUtility.ReadUShortData(buffer, process, out score);
            process += PacketUtility.ReadListData<VectorPacket>(buffer, process, out positions);
        }

        public override ArraySegment<byte> Serialize()
        {
            ArraySegment<byte> buffer = UniqueBuffer.Open(1024);
            ushort process = 0;

            process += sizeof(ushort);
            process += PacketUtility.AppendUShortData(this.ID, buffer, process);
            process += PacketUtility.AppendUShortData(this.attakerID, buffer, process);
            process += PacketUtility.AppendUShortData(this.playerID, buffer, process);
            process += PacketUtility.AppendUShortData(this.score, buffer, process);
            process += PacketUtility.AppendListData<VectorPacket>(this.positions, buffer, process);
            PacketUtility.AppendUShortData(process, buffer, 0);

            return UniqueBuffer.Close(process);
        }
    }
}
