using System;
using System.Collections.Generic;
using H00N.Network;
using Packets;

public class PacketManager
{
    public static PacketManager Instance = null;

    private Dictionary<ushort, Func<ArraySegment<byte>, Packet>> packetFactories = new Dictionary<ushort, Func<ArraySegment<byte>, Packet>>();
    private Dictionary<ushort, Action<Session, Packet>> packetHandlers = new Dictionary<ushort, Action<Session, Packet>>();

    public PacketManager()
    {
        packetFactories.Clear();
        packetHandlers.Clear();

        RegisterHandlers();
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

    private void RegisterHandlers()
    {
        RegisterHandler<S_LogInPacket>(PacketID.S_LogInPacket, PacketHandler.S_LogInPacket);
        RegisterHandler<S_RoomEnterPacket>(PacketID.S_RoomEnterPacket, PacketHandler.S_RoomEnterPacket);
        RegisterHandler<S_OtherJoinPacket>(PacketID.S_OtherJoinPacket, PacketHandler.S_OtherJoinPacket);
        RegisterHandler<S_OtherExitPacket>(PacketID.S_OtherExitPacket, PacketHandler.S_OtherExitPacket);
        RegisterHandler<S_AttackPacket>(PacketID.S_AttackPacket, PacketHandler.S_AttackPacket);
        RegisterHandler<S_PlayerPacket>(PacketID.S_PlayerPacket, PacketHandler.S_PlayerPacket);
        RegisterHandler<S_PlayerDiePacket>(PacketID.S_PlayerDiePacket, PacketHandler.S_PlayerDiePacket);
        RegisterHandler<S_ScorePacket>(PacketID.S_ScorePacket, PacketHandler.S_ScorePacket);
        RegisterHandler<S_ObjectDestroyPacket>(PacketID.S_ObjectDestroyPacket, PacketHandler.S_ObjectDestroyPacket);
        RegisterHandler<S_ScoreBoxPacket>(PacketID.S_ScoreBoxPacket, PacketHandler.S_ScoreBoxPacket);
        RegisterHandler<S_ChattingPacket>(PacketID.S_ChattingPacket, PacketHandler.S_ChattingPacket);
        RegisterHandler<S_AnimationPacket>(PacketID.S_AnimationPacket, PacketHandler.S_AnimationPacket);
        RegisterHandler<S_ErrorPacket>(PacketID.S_ErrorPacket, PacketHandler.S_ErrorPacket);
        RegisterHandler<S_EventStartPacket>(PacketID.S_EventStartPacket, PacketHandler.S_EventStartPacket);
        RegisterHandler<S_EventEndPacket>(PacketID.S_EventEndPacket, PacketHandler.S_EventEndPacket);
    }

    private void RegisterHandler<T>(PacketID id, Action<Session, Packet> handler) where T : Packet, new()
    {
        packetFactories.Add((ushort)id, PacketUtility.CreatePacket<T>);
        packetHandlers.Add((ushort)id, handler);
    }
}

