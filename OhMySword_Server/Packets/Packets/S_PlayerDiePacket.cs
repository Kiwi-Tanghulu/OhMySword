using H00N.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Packets
{
    public class S_PlayerDiePacket : Packet
    {
        public override ushort ID => (ushort)PacketID.S_PlayerDiePacket;

        public ushort attackerID;
        public ushort playerID;
        public ushort score;
        public List<ObjectPacket> objects;

        public S_PlayerDiePacket() { }

        public S_PlayerDiePacket(ushort attackerID, ushort playerID, ushort score, List<ObjectPacket> objects)
        {
            this.attackerID = attackerID;
            this.playerID = playerID;
            this.score = score;
            this.objects = objects;
        }

        public override void Deserialize(ArraySegment<byte> buffer)
        {
            ushort process = 0;

            process += sizeof(ushort);
            process += sizeof(ushort);

            process += PacketUtility.ReadUShortData(buffer, process, out attackerID);
            process += PacketUtility.ReadUShortData(buffer, process, out playerID);
            process += PacketUtility.ReadUShortData(buffer, process, out score);
            process += PacketUtility.ReadListData<ObjectPacket>(buffer, process, out objects);
        }

        public override ArraySegment<byte> Serialize()
        {
            ArraySegment<byte> buffer = UniqueBuffer.Open(1024);
            ushort process = 0;

            process += sizeof(ushort);
            process += PacketUtility.AppendUShortData(this.ID, buffer, process);
            process += PacketUtility.AppendUShortData(this.attackerID, buffer, process);
            process += PacketUtility.AppendUShortData(this.playerID, buffer, process);
            process += PacketUtility.AppendUShortData(this.score, buffer, process);
            process += PacketUtility.AppendListData<ObjectPacket>(this.objects, buffer, process);
            PacketUtility.AppendUShortData(process, buffer, 0);

            return UniqueBuffer.Close(process);
        }
    }
}
