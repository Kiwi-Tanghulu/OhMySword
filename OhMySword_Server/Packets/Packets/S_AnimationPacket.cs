using H00N.Network;
using System;

namespace Packets
{
    public class S_AnimationPacket : Packet
    {
        public override ushort ID => (ushort)PacketID.S_AnimationPacket;

        public ushort objectID;
        public ushort objectType;
        public int animationHash;

        public S_AnimationPacket() { }

        public S_AnimationPacket(ushort objectID, ushort objectType, int animationHash)
        {
            this.objectID = objectID;
            this.objectType = objectType;
            this.animationHash = animationHash;
        }

        public override void Deserialize(ArraySegment<byte> buffer)
        {
            ushort process = 0;

            process += sizeof(ushort);
            process += sizeof(ushort);

            process += PacketUtility.ReadUShortData(buffer, process, out objectID);
            process += PacketUtility.ReadUShortData(buffer, process, out objectType);
            process += PacketUtility.ReadIntData(buffer, process, out animationHash);
        }

        public override ArraySegment<byte> Serialize()
        {
            ArraySegment<byte> buffer = UniqueBuffer.Open(1024);
            ushort process = 0;

            process += sizeof(ushort);
            process += PacketUtility.AppendUShortData(this.objectID, buffer, process);
            process += PacketUtility.AppendUShortData(this.objectType, buffer, process);
            process += PacketUtility.AppendIntData(this.animationHash, buffer, process);
            PacketUtility.AppendUShortData(process, buffer, 0);

            return UniqueBuffer.Close(process);
        }

        public static implicit operator C_AnimationPacket(S_AnimationPacket right)
            => new C_AnimationPacket(right.objectID, right.objectType, right.animationHash);
    }
}
