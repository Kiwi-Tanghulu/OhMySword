using H00N.Network;
using Packets;
using System;
using System.Collections.Generic;

namespace Server
{
    public class PacketManager
    {
        public static PacketManager Instance = null;

        private Dictionary<ushort, Func<ArraySegment<byte>, Packet>> packetFactories = new Dictionary<ushort, Func<ArraySegment<byte>, Packet>>();
        private Dictionary<ushort, Action<Session, Packet>> packetHandlers = new Dictionary<ushort, Action<Session, Packet>>();

        public PacketManager()
        {
            packetFactories.Clear();
            packetHandlers.Clear();

            RegisterHandler();
        }

        private void RegisterHandler()
        {
            packetFactories.Add((ushort)PacketID.C_PlayerPacket, PacketUtility.CreatePacket<C_PlayerPacket>);
            packetHandlers.Add((ushort)PacketID.C_PlayerPacket, PacketHandler.C_PlayerPacket);

            packetFactories.Add((ushort)PacketID.C_RoomEnterPacket, PacketUtility.CreatePacket<C_RoomEnterPacket>);
            packetHandlers.Add((ushort)PacketID.C_RoomEnterPacket, PacketHandler.C_RoomEnterPacket);
        }

        public Packet CreatePacket(ArraySegment<byte> buffer)
        {
            ushort packetID = PacketUtility.ReadPacketID(buffer);
            if (packetFactories.ContainsKey(packetID))
                return packetFactories[packetID]?.Invoke(buffer);
            else
                return null;
        }

        public void HandlePacket(Session session, Packet packet)
        {
            if (packet != null)
                if (packetHandlers.ContainsKey(packet.ID))
                    packetHandlers[packet.ID]?.Invoke(session, packet);
        }
    }
}
