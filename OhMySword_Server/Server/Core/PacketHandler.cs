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
            Player player = new Player(session as ClientSession, room, enterPacket.nickname, enterPacket.skinID);
            
            int posIndex = Random.Range(0, DEFINE.PlayerSpawnTable.Length);
            player.position = DEFINE.PlayerSpawnTable[posIndex];

            room.AddJob(() => {
                room.PublishPlayer(player);
                clientSession.Player = player;
                clientSession.Room = room;
            });

            S_OtherJoinPacket broadcastPacket = new S_OtherJoinPacket(player.nickname, player.objectID, enterPacket.skinID, (ushort)posIndex);
            room.AddJob(() => room.Broadcast(broadcastPacket, clientSession.UserID));

            List<PlayerPacket> playerList = room.GetPlayerList(player.objectID);
            List<ObjectPacket> objectList = room.GetObjectList();
            S_RoomEnterPacket replyPacket = new S_RoomEnterPacket(player.objectID, enterPacket.skinID, (ushort)posIndex, playerList, objectList);
            clientSession.Send(replyPacket.Serialize());
        }

        public static void C_RoomExitPacket(Session session, Packet packet)
        {
            ClientSession clientSession = session as ClientSession;
            GameRoom room = clientSession.Room;

            ushort playerID = clientSession.Player.objectID;

            room.AddJob(() =>
            {
                room.ReleasePlayer(clientSession.Player);
                clientSession.Player = null;
                clientSession.Room = null;
            });
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
                    hitPlayer.Hit(attackPacket.damage, attackPacket.attackerID);
            }
            else
            {
                if (room.GetObject(attackPacket.hitObjectID, out ObjectBase obj))
                    obj.Hit(attackPacket.damage, attackPacket.attackerID);
            }
        }

        public static void C_ChattingPacket(Session session, Packet packet)
        {
            ClientSession clientSession = session as ClientSession;
            C_ChattingPacket chattingPacket = packet as C_ChattingPacket;
            GameRoom room = clientSession.Room;

            S_ChattingPacket broadcastPacket = new S_ChattingPacket(chattingPacket.chat, clientSession.Player.objectID);
            room.AddJob(() => room.Broadcast(broadcastPacket));
        }

        public static void C_AnimationPacket(Session session, Packet packet)
        {
            ClientSession clientSession = session as ClientSession;
            C_AnimationPacket animationPacket = packet as C_AnimationPacket;
            GameRoom room = clientSession.Room;

            S_AnimationPacket broadcastPacket = animationPacket;
            room.AddJob(() => room.Broadcast(broadcastPacket, clientSession.UserID)); 
        }

        public static void C_ChickenHitPacket(Session session, Packet packet)
        {
            ClientSession clientSession = session as ClientSession;
            C_ChickenHitPacket chickenPacket = packet as C_ChickenHitPacket;
            GameRoom room = clientSession.Room;

            List<ObjectPacket> objects = new List<ObjectPacket>();
            S_ChickenHitPacket broadcastPacket = new S_ChickenHitPacket(clientSession.Player.objectID, 333, chickenPacket.position, objects);

            broadcastPacket.score.ForEachDigit((digit, number, index) => {
                Vector3 randInCircle = Random.InCircle(3f);
                XPObject xp = new XPObject(room, digit);
                xp.position = new Vector3(randInCircle.x, 0.5f, randInCircle.y);
                room.PublishObject(xp);
                objects.Add(xp);

                xp.position += broadcastPacket.position;
            });

            room.AddJob(() => room.Broadcast(broadcastPacket));
        }
    }
}