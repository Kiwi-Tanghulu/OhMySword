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

            packetFactories.Add((ushort)PacketID.C_RoomExitPacket, PacketUtility.CreatePacket<C_RoomExitPacket>);
            packetHandlers.Add((ushort)PacketID.C_RoomExitPacket, PacketHandler.C_RoomExitPacket);

            packetFactories.Add((ushort)PacketID.C_AttackPacket, PacketUtility.CreatePacket<C_AttackPacket>);
            packetHandlers.Add((ushort)PacketID.C_AttackPacket, PacketHandler.C_AttackPacket);

            packetFactories.Add((ushort)PacketID.C_ChattingPacket, PacketUtility.CreatePacket<C_ChattingPacket>);
            packetHandlers.Add((ushort)PacketID.C_ChattingPacket, PacketHandler.C_ChattingPacket);

            packetFactories.Add((ushort)PacketID.C_AnimationPacket, PacketUtility.CreatePacket<C_AnimationPacket>);
            packetHandlers.Add((ushort)PacketID.C_AnimationPacket, PacketHandler.C_AnimationPacket);
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
