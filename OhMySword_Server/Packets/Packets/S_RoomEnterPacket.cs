using H00N.Network;
using System;
using System.Collections.Generic;

namespace Packets
{
    public class S_RoomEnterPacket : Packet
    {
        public override ushort ID => (ushort)PacketID.S_RoomEnterPacket;

        public ushort playerID;
        public ushort posTableIndex;
        public List<PlayerPacket> players;
        public List<ObjectPacket> objects;

        public S_RoomEnterPacket() { }

        public S_RoomEnterPacket(ushort playerID, ushort posTableIndex, List<PlayerPacket> players, List<ObjectPacket> objects)
        {
            this.playerID = playerID;
            this.posTableIndex = posTableIndex;
            this.players = players;
            this.objects = objects;
        }

        public override void Deserialize(ArraySegment<byte> buffer)
        {
            ushort process = 0;

            process += sizeof(ushort);
            process += sizeof(ushort);

            process += PacketUtility.ReadUShortData(buffer, process, out playerID);
            process += PacketUtility.ReadUShortData(buffer, process, out posTableIndex);
            process += PacketUtility.ReadListData<PlayerPacket>(buffer, process, out players);
            process += PacketUtility.ReadListData<ObjectPacket>(buffer, process, out objects);
        }

        public override ArraySegment<byte> Serialize()
        {
            ArraySegment<byte> buffer = UniqueBuffer.Open(1024);
            ushort process = 0;

            process += sizeof(ushort);
            process += PacketUtility.AppendUShortData(this.ID, buffer, process);
            process += PacketUtility.AppendUShortData(this.playerID, buffer, process);
            process += PacketUtility.AppendUShortData(this.posTableIndex, buffer, process);
            process += PacketUtility.AppendListData<PlayerPacket>(this.players, buffer, process);
            process += PacketUtility.AppendListData<ObjectPacket>(this.objects, buffer, process);
            PacketUtility.AppendUShortData(process, buffer, 0);

            return UniqueBuffer.Close(process);
        }
    }
}
