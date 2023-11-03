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

            // 경험치가 생성되고
            // 나머지 클라한테 죽었음을 알리고
            // 나는 퇴장되고
            List<VectorPacket> positions = new List<VectorPacket>();
            CreateXP(positions);

            S_PlayerDiePacket broadcastPacket = new S_PlayerDiePacket(attacker, objectID, score, positions);
            room?.AddJob(() => room?.Broadcast(broadcastPacket));
        }

        public void AddXP(ushort amount)
        {
            score += amount;
            S_ScorePacket broadcastPackt = new S_ScorePacket(objectID, score);
            room?.AddJob(() => room.Broadcast(broadcastPackt));
        }

        private void CreateXP(List<VectorPacket> container)
        {
            int score = this.score;
            int cursor = (int)MathF.Pow(10, score.ToString().Length - 1);
            while (cursor > 0)
            {
                int number = score / cursor;
                for(int i = 0; i < number; i++)
                {
                    Vector3 randInCircle = Random.InCircle(10f);
                    container.Add(new VectorPacket(randInCircle.x, 2f, randInCircle.z));
                }

                score %= cursor;
                cursor /= 10;
            }
        }

        public static implicit operator PlayerPacket(Player right)
        {
            return new PlayerPacket(right.objectID, right.score, right.nickname, right.position, right.rotation);
        }
    }
}
