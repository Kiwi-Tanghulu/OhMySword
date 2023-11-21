using Packets;

namespace Server
{
    public class Player : ObjectBase
    {
        public ClientSession session;
        public string nickname;
        public ushort score;
        public ushort hp;

        public ushort destroyCount;

        public Player(ClientSession session, GameRoom room, string name)
        {
            objectType = (ushort)ObjectType.Player;
            this.room = room;

            this.session = session;
            nickname = name;
            destroyCount = 0;
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
            List<ObjectPacket> objects = new List<ObjectPacket>();
            CreateXP(objects);

            S_PlayerDiePacket broadcastPacket = new S_PlayerDiePacket(attacker, objectID, score, destroyCount, objects);
            room?.AddJob(() => room?.Broadcast(broadcastPacket));
        }

        public void AddXP(ushort amount)
        {
            score += amount;
            S_ScorePacket broadcastPackt = new S_ScorePacket(objectID, score);
            room?.AddJob(() => room.Broadcast(broadcastPackt));
        }

        private void CreateXP(List<ObjectPacket> container)
        {
            score.ForEachDigit((digit, number, index) => {
                Vector3 randInCircle = Random.InCircle(3f);
                Console.WriteLine($"x : {randInCircle.x}, y : {randInCircle.y}");
                XPObject xp = new XPObject(room, digit);
                xp.position = new Vector3(randInCircle.x, 0f, randInCircle.y);

                room.PublishObject(xp);
                container.Add(xp);
            });
        }

        public static implicit operator PlayerPacket(Player right)
        {
            return new PlayerPacket(right.objectID, right.score, right.nickname, right.position, right.rotation);
        }
    }
}
