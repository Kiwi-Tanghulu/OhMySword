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
            ClientSession clientSession = session as ClientSession;
            C_RoomEnterPacket enterPacket = packet as C_RoomEnterPacket;

            GameRoom room = RoomManager.Instance.GetRoom();
            Player player = new Player(session as ClientSession, enterPacket.nickname);
            
            int posIndex = Random.Range(0, DEFINE.PlayerSpawnTable.Length);
            player.position = DEFINE.PlayerSpawnTable[posIndex];

            room.AddJob(() => room.PublishPlayer(player));

            S_OtherJoinPacket broadcastPacket = new S_OtherJoinPacket(player.nickname, player.objectID, (ushort)posIndex);
            room.Broadcast(broadcastPacket, clientSession.UserID);

            S_RoomEnterPacket replyPacket = new S_RoomEnterPacket(player.objectID, (ushort)posIndex, room.GetPlayerList(player.objectID), room.GetObjectList());
            clientSession.Send(replyPacket.Serialize());
        }
    }
}