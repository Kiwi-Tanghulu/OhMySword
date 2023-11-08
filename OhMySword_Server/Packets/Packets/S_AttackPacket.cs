using H00N.Network;
using System;

namespace Packets
{
    public class S_AttackPacket : Packet
    {
        public override ushort ID => (ushort)PacketID.S_AttackPacket;

        public ushort hitObjectType;
        public ushort hitObjectID;
        public ushort attackerID;

        public S_AttackPacket() { }

        public S_AttackPacket(ushort hitObjectType, ushort hitObjectID, ushort attackerID)
        {
            this.hitObjectType = hitObjectType;
            this.hitObjectID = hitObjectID;
            this.attackerID = attackerID;
        }

        public override void Deserialize(ArraySegment<byte> buffer)
        {
            ushort process = 0;

            process += sizeof(ushort);
            process += sizeof(ushort);

            process += PacketUtility.ReadUShortData(buffer, process, out hitObjectType);
            process += PacketUtility.ReadUShortData(buffer, process, out hitObjectID);
            process += PacketUtility.ReadUShortData(buffer, process, out attackerID);
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
            PacketUtility.AppendUShortData(process, buffer, 0);

            return UniqueBuffer.Close(process);
        }

        public static implicit operator S_AttackPacket(C_AttackPacket right) => new S_AttackPacket(right.hitObjectType, right.hitObjectID, right.attackerID);
    }
}
