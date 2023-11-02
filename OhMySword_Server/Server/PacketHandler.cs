using H00N.Network;
using Packets;

namespace Server
{
    public class PacketHandler
    {
        public static void C_PlayerPacket(Session session, Packet packet)
        {
            ClientSession clientSession = session as ClientSession;
            C_PlayerPacket playerPacket = packet as C_PlayerPacket;
            GameRoom room = clientSession.Room;

            if (room.GetPlayer(playerPacket.objectPacket.objectID, out Player player) == false)
                return;

            player.position = playerPacket.objectPacket.position;
            player.rotation = playerPacket.objectPacket.rotation;

            S_PlayerPacket broadcastPacket = playerPacket;
            room.Broadcast(broadcastPacket, clientSession.UserID);
        }
    }
}