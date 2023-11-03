
using Packets;

namespace Server
{
    public class Player : ObjectBase
    {
        public ClientSession session;
        public string nickname;
        public ushort score;
        public ushort hp;

        public Player(ClientSession session, GameRoom room, string name)
        {
            objectType = (ushort)ObjectType.Player;
            this.room = room;

            this.session = session;
            nickname = name;
            hp = 1;
        }

        public override void Hit(ushort damage, ushort attacker)
        {
            hp -= damage;
            if (hp > 0)
                return;

            // 이거 해야됨
        }

        public void AddXP(ushort amount)
        {
            score += amount;
            S_ScorePacket broadcastPackt = new S_ScorePacket(objectID, score);
            room?.AddJob(() => room.Broadcast(broadcastPackt));
        }

        public static implicit operator PlayerPacket(Player right)
        {
            return new PlayerPacket(right.objectID, right.score, right.nickname, right.position, right.rotation);
        }
    }
}
