using H00N.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Packets
{
    public class PlayerPacket : DataPacket
    {
        public ushort objectID;
        public ushort score;
        public string nickname;
        public VectorPacket position;
        public VectorPacket rotation;

        public PlayerPacket() { }

        public PlayerPacket(ushort objectID, ushort score, string nickname, VectorPacket position, VectorPacket rotation)
        {
            this.objectID = objectID;
            this.score = score;
            this.nickname = nickname;
            this.position = position;
            this.rotation = rotation;
        }

        public override ushort Deserialize(ArraySegment<byte> buffer, int offset)
        {
            ushort process = 0;
            process += PacketUtility.ReadUShortData(buffer, offset + process, out this.objectID);
            process += PacketUtility.ReadUShortData(buffer, offset + process, out this.score);
            process += PacketUtility.ReadStringData(buffer, offset + process, out this.nickname);
            process += PacketUtility.ReadDataPacket<VectorPacket>(buffer, offset + process, out this.position);
            process += PacketUtility.ReadDataPacket<VectorPacket>(buffer, offset + process, out this.rotation);

            return process;
        }

        public override ushort Serialize(ArraySegment<byte> buffer, int offset)
        {
            ushort process = 0;
            process += PacketUtility.AppendUShortData(this.objectID, buffer, offset + process);
            process += PacketUtility.AppendUShortData(this.score, buffer, offset + process);
            process += PacketUtility.AppendStringData(this.nickname, buffer, offset + process);
            process += PacketUtility.AppendDataPacket<VectorPacket>(this.position, buffer, offset + process);
            process += PacketUtility.AppendDataPacket<VectorPacket>(this.rotation, buffer, offset + process);

            return process;
        }
    }
}
