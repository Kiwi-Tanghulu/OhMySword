using H00N.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Packets
{
    public class S_ChickenHitPacket : Packet
    {
        public override ushort ID => (ushort)PacketID.S_ChickenHitPacket;

        public ushort playerID;
        public ushort score;
        public VectorPacket position;
        public List<ObjectPacket> objects;

        public S_ChickenHitPacket() { }

        public S_ChickenHitPacket(ushort playerID, ushort score, VectorPacket position, List<ObjectPacket> objects)
        {
            this.playerID = playerID;
            this.score = score;
            this.position = position;
            this.objects = objects;
        }

        public override void Deserialize(ArraySegment<byte> buffer)
        {
            ushort process = 0;

            process += sizeof(ushort);
            process += sizeof(ushort);

            process += PacketUtility.ReadUShortData(buffer, process, out playerID);
            process += PacketUtility.ReadUShortData(buffer, process, out score);
            process += PacketUtility.ReadDataPacket<VectorPacket>(buffer, process, out position);
            process += PacketUtility.ReadListData<ObjectPacket>(buffer, process, out objects);
        }

        public override ArraySegment<byte> Serialize()
        {
            ArraySegment<byte> buffer = UniqueBuffer.Open(1024);
            ushort process = 0;

            process += sizeof(ushort);
            process += PacketUtility.AppendUShortData(this.ID, buffer, process);
            process += PacketUtility.AppendUShortData(this.playerID, buffer, process);
            process += PacketUtility.AppendUShortData(this.score, buffer, process);
            process += PacketUtility.AppendDataPacket<VectorPacket>(this.position, buffer, process);
            process += PacketUtility.AppendListData<ObjectPacket>(this.objects, buffer, process);
            PacketUtility.AppendUShortData(process, buffer, 0);

            return UniqueBuffer.Close(process);
        }
    }
}
