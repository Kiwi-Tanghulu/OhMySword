using H00N.Network;
using System;

namespace Packets
{
    public class C_AnimationPacket : Packet
    {
        public override ushort ID => (ushort)PacketID.C_AnimationPacket;

        public ushort objectID;
        public ushort animationType;

        public C_AnimationPacket() { }

        public C_AnimationPacket(ushort objectID, ushort animationType)
        {
            this.objectID = objectID;
            this.animationType = animationType;
        }

        public override void Deserialize(ArraySegment<byte> buffer)
        {
            ushort process = 0;

            process += sizeof(ushort);
            process += sizeof(ushort);

            process += PacketUtility.ReadUShortData(buffer, process, out objectID);
            process += PacketUtility.ReadUShortData(buffer, process, out animationType);
        }

        public override ArraySegment<byte> Serialize()
        {
            ArraySegment<byte> buffer = UniqueBuffer.Open(1024);
            ushort process = 0;

            process += sizeof(ushort);
            process += PacketUtility.AppendUShortData(this.ID, buffer, process);
            process += PacketUtility.AppendUShortData(this.objectID, buffer, process);
            process += PacketUtility.AppendUShortData(this.animationType, buffer, process);
            PacketUtility.AppendUShortData(process, buffer, 0);

            return UniqueBuffer.Close(process);
        }

        public static implicit operator S_AnimationPacket(C_AnimationPacket right)
            => new S_AnimationPacket(right.objectID, right.animationType);
    }
}
