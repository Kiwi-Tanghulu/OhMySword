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
            room.AddJob(() => room.Broadcast(broadcastPacket, clientSession.UserID));
        }

        public static void C_RoomEnterPacket(Session session, Packet packet)
        {
            ClientSession clientSession = session as ClientSession;
            C_RoomEnterPacket enterPacket = packet as C_RoomEnterPacket;

            GameRoom room = RoomManager.Instance.GetRoom();
            Player player = new Player(session as ClientSession, enterPacket.nickname);
            
            int posIndex = Random.Range(0, DEFINE.PlayerSpawnTable.Length);
            player.position = DEFINE.PlayerSpawnTable[posIndex];

            room.AddJob(() => {
                room.PublishPlayer(player);
                clientSession.Player = player;
            });

            S_OtherJoinPacket broadcastPacket = new S_OtherJoinPacket(player.nickname, player.objectID, (ushort)posIndex);
            room.AddJob(() => room.Broadcast(broadcastPacket, clientSession.UserID));

            List<PlayerPacket> playerList = room.GetPlayerList(player.objectID);
            List<ObjectPacket> objectList = room.GetObjectList();
            S_RoomEnterPacket replyPacket = new S_RoomEnterPacket(player.objectID, (ushort)posIndex, playerList, objectList);
            clientSession.Send(replyPacket.Serialize());
        }

        public static void C_RoomExitPacket(Session session, Packet packet)
        {
            ClientSession clientSession = session as ClientSession;
            GameRoom room = clientSession.Room;

            ushort playerID = clientSession.Player.objectID;

            room.AddJob(() => room.ReleasePlayer(clientSession.Player));
            clientSession.Player = null;
            clientSession.Room = null;

            S_OtherExitPacket broadcastPackt = new S_OtherExitPacket(playerID);
            room.AddJob(() => room.Broadcast(broadcastPackt, playerID));
        }

        public static void C_AttackPacket(Session session, Packet packet)
        {
            ClientSession clientSession = session as ClientSession;
            C_AttackPacket attackPacket = packet as C_AttackPacket;
            GameRoom room = clientSession.Room;
            // 플레이어일 때 & 상자일 때 구분해야 함

            S_AttackPacket broadcastPacket = attackPacket;
            room.AddJob(() => room.Broadcast(broadcastPacket));

            if (attackPacket.hitObjectType == (ushort)ObjectType.Player)
            {
                if (room.GetPlayer(attackPacket.hitObjectID, out Player hitPlayer))
                    hitPlayer.Hit(attackPacket.damage);
            }
            else
            {
                if (room.GetObject(attackPacket.hitObjectID, out ObjectBase obj))
                    obj.Hit(attackPacket.damage);
            }
        }
    }
}