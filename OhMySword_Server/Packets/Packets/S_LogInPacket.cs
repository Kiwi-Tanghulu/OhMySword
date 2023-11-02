using H00N.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Packets
{
    public class S_LogInPacket : Packet
    {
        public override ushort ID => (ushort)PacketID.S_LogInPacket;

        public ushort userID;

        public override void Deserialize(ArraySegment<byte> buffer)
        {
            ushort process = 0;

            process += sizeof(ushort);
            process += sizeof(ushort);

            process += PacketUtility.ReadUShortData(buffer, process, out userID);
        }

        public override ArraySegment<byte> Serialize()
        {
            ArraySegment<byte> buffer = UniqueBuffer.Open(1024);
            ushort process = 0;

            process += sizeof(ushort);
            process += PacketUtility.AppendUShortData(this.ID, buffer, process);
            process += PacketUtility.AppendUShortData(this.userID, buffer, process);
            PacketUtility.AppendUShortData(process, buffer, 0);

            return UniqueBuffer.Close(process);
        }
    }
}
