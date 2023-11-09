using Packets;
using static Server.DEFINE;

namespace Server
{
    public class ScoreBox : ObjectBase
    {
        public ushort hp;

        public ScoreBox(GameRoom room, ushort hp, ObjectType type)
        {
            this.room = room;
            this.hp = hp;
            this.objectType = (ushort)type;
        }

        public override void Hit(ushort damage, ushort attacker)
        {
            hp -= damage;
            if (hp > 0)
                return;

            if (room.GetPlayer(attacker, out Player player))
                player.destroyCount += 1;

            // 테이블에서 경험치 테이블 인덱스 뽑아야 함
            // 이놈 아이디 정보, 새로운 포지션 인덱스 정보 전송해야 함
            hp = 10;
            List<UShortPacket> ids = GenerateXP();
            ushort posTableIndex = ResetPosition();
            
            S_ScoreBoxPacket broadcastPacket = new S_ScoreBoxPacket(objectID, posTableIndex, ids);
            room.AddJob(() => room.Broadcast(broadcastPacket));
        }

        private ushort ResetPosition()
        {
            int posTableIndex = Random.Range(0, ScoreBoxSpawnTable.Length);
            position = ScoreBoxSpawnTable[posTableIndex];

            return (ushort)posTableIndex;
        }

        private List<UShortPacket> GenerateXP()
        {
            List<UShortPacket> ids = new List<UShortPacket>();

            int i = 0;
            int score = XPSpawnTable[objectType].score;
            int cursor = (int)MathF.Pow(10, score.ToString().Length - 1);
            while (cursor > 0)
            {
                int number = score / cursor;
                for (int j = 0; j < number; j++, i++)
                {
                    XPObject xp = new XPObject(room, (ushort)cursor);
                    xp.position = XPSpawnTable[objectType].positions[i];
                    room.PublishObject(xp);

                    ids.Add(xp.objectID);
                }

                score %= cursor;
                cursor /= 10;
            }

            return ids;
        }
    }
}
