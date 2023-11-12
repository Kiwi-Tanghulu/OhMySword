using H00N.Network;
using System;

namespace Packets
{
    public class ObjectPacket : DataPacket
    {
        public ushort objectID;
        public ushort objectType;
        public VectorPacket position;
        public VectorPacket rotation;

        public ObjectPacket() { }

        public ObjectPacket(ushort objectID, ushort objectType, VectorPacket position, VectorPacket rotation)
        {
            this.objectID = objectID;
            this.objectType = objectType;
            this.position = position;
            this.rotation = rotation; 
        }

        public override ushort Deserialize(ArraySegment<byte> buffer, int offset)
        {
            ushort process = 0;
            process += PacketUtility.ReadUShortData(buffer, offset + process, out this.objectID);
            process += PacketUtility.ReadUShortData(buffer, offset + process, out this.objectType);
            process += PacketUtility.ReadDataPacket<VectorPacket>(buffer, offset + process, out this.position);
            process += PacketUtility.ReadDataPacket<VectorPacket>(buffer, offset + process, out this.rotation);

            return process;
        }

        public override ushort Serialize(ArraySegment<byte> buffer, int offset)
        {
            ushort process = 0;
            process += PacketUtility.AppendUShortData(this.objectID, buffer, offset + process);
            process += PacketUtility.AppendUShortData(this.objectType, buffer, offset + process);
            process += PacketUtility.AppendDataPacket<VectorPacket>(this.position, buffer, offset + process);
            process += PacketUtility.AppendDataPacket<VectorPacket>(this.rotation, buffer, offset + process);

            return process;
        }
    }
}
