using System;
using System.Collections.Generic;
using H00N.Network;
using Packets;
using UnityEngine;

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

    public Packet CreatePacket(ArraySegment<byte> buffer)
    {
        ushort packetID = PacketUtility.ReadPacketID(buffer);

        if(packetFactories.ContainsKey(packetID))
            return packetFactories[packetID]?.Invoke(buffer);
        else
            return null;
    }

    public void HandlePacket(Session session, Packet packet)
    {
        if(packet == null)
            return;

        if(packetHandlers.ContainsKey(packet.ID))
            packetHandlers[packet.ID]?.Invoke(session, packet);
    }

    private void RegisterHandler()
    {
        RegisterHandler<S_LogInPacket>(PacketID.S_LogInPacket, PacketHandler.S_LogInPacket);
        RegisterHandler<S_RoomEnterPacket>(PacketID.S_RoomEnterPacket, PacketHandler.S_RoomEnterPacket);
        RegisterHandler<S_OtherJoinPacket>(PacketID.S_OtherJoinPacket, PacketHandler.S_OtherJoinPacket);
        RegisterHandler<S_OtherExitPacket>(PacketID.S_OtherExitPacket, PacketHandler.S_OtherExitPacket);
        // RegisterHandler<S_LogInPacket>(PacketID.S_LogInPacket, PacketHandler.S_LogInPacket);
    }

    private void RegisterHandler<T>(PacketID id, Action<Session, Packet> handler) where T : Packet, new()
    {
        packetFactories.Add((ushort)id, PacketUtility.CreatePacket<T>);
        packetHandlers.Add((ushort)id, handler);
    }
}
