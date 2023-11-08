using H00N.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Packets
{
    public class S_ScorePacket : Packet
    {
        public override ushort ID => (ushort)PacketID.S_ScorePacket;

        public ushort playerID;
        public ushort score;

        public S_ScorePacket() { }

        public S_ScorePacket(ushort playerID, ushort score)
        {
            this.playerID = playerID;
            this.score = score;
        }

        public override void Deserialize(ArraySegment<byte> buffer)
        {
            ushort process = 0;

            process += sizeof(ushort);
            process += sizeof(ushort);

            process += PacketUtility.ReadUShortData(buffer, process, out playerID);
            process += PacketUtility.ReadUShortData(buffer, process, out score);
        }

        public override ArraySegment<byte> Serialize()
        {
            ArraySegment<byte> buffer = UniqueBuffer.Open(1024);
            ushort process = 0;

            process += sizeof(ushort);
            process += PacketUtility.AppendUShortData(this.ID, buffer, process);
            process += PacketUtility.AppendUShortData(this.playerID, buffer, process);
            process += PacketUtility.AppendUShortData(this.score, buffer, process);
            PacketUtility.AppendUShortData(process, buffer, 0);

            return UniqueBuffer.Close(process);
        }
    }
}
