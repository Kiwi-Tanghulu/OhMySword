using H00N.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Packets
{
    public class S_OtherExitPacket : Packet
    {
        public override ushort ID => (ushort)PacketID.S_OtherExitPacket;

        public ushort playerID;

        public S_OtherExitPacket() { }

        public S_OtherExitPacket(ushort playerID)
        {
            this.playerID = playerID;
        }

        public override void Deserialize(ArraySegment<byte> buffer)
        {
            ushort process = 0;

            process += sizeof(ushort);
            process += sizeof(ushort);

            process += PacketUtility.ReadUShortData(buffer, process, out playerID);
        }

        public override ArraySegment<byte> Serialize()
        {
            ArraySegment<byte> buffer = UniqueBuffer.Open(1024);
            ushort process = 0;

            process += sizeof(ushort);
            process += PacketUtility.AppendUShortData(this.ID, buffer, process);
            process += PacketUtility.AppendUShortData(this.playerID, buffer, process);
            PacketUtility.AppendUShortData(process, buffer, 0);

            return UniqueBuffer.Close(process);
        }
    }
}
