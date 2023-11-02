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

        public static void C_RoomEnterPacket(Session session, Packet packet)
        {
            C_RoomEnterPacket enterPacket = packet as C_RoomEnterPacket;

            GameRoom room = RoomManager.Instance.GetRoom();
            Player player = new Player(session as ClientSession, enterPacket.nickname);

            room.AddJob(() => room.PublishPlayer(player));

            // ¿©±â ÇØ¾ßµÊ
        }
    }
}