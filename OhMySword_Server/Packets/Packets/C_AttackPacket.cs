using H00N.Network;
using System;

namespace Packets
{
    public class C_AttackPacket : Packet
    {
        public override ushort ID => (ushort)PacketID.C_AttackPacket;

        public ushort hitObjectType;
        public ushort hitObjectID;
        public ushort attackerID;
        public ushort damage;

        public override void Deserialize(ArraySegment<byte> buffer)
        {
            ushort process = 0;

            process += sizeof(ushort);
            process += sizeof(ushort);

            process += PacketUtility.ReadUShortData(buffer, process, out hitObjectType);
            process += PacketUtility.ReadUShortData(buffer, process, out hitObjectID);
            process += PacketUtility.ReadUShortData(buffer, process, out attackerID);
            process += PacketUtility.ReadUShortData(buffer, process, out damage);
        }

        public override ArraySegment<byte> Serialize()
        {
            ArraySegment<byte> buffer = UniqueBuffer.Open(1024);
            ushort process = 0;

            process += sizeof(ushort);
            process += PacketUtility.AppendUShortData(this.ID, buffer, process);
            process += PacketUtility.AppendUShortData(this.hitObjectType, buffer, process);
            process += PacketUtility.AppendUShortData(this.hitObjectID, buffer, process);
            process += PacketUtility.AppendUShortData(this.attackerID, buffer, process);
            process += PacketUtility.AppendUShortData(this.damage, buffer, process);
            PacketUtility.AppendUShortData(process, buffer, 0);

            return UniqueBuffer.Close(process);
        }
    }
}
