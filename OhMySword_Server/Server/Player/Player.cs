
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
            this.objectType = (ushort)ObjectType.Player;
            this.room = room;

            this.session = session;
            this.nickname = name;
            this.hp = 1;
        }

        public void Hit(ushort damage)
        {
            hp -= damage;
            if (hp > 0)
                return;

            // 이거 해야됨
        }

        public static implicit operator PlayerPacket(Player right)
        {
            return new PlayerPacket(right.objectID, right.score, right.nickname, right.position, right.rotation);
        }
    }
}
