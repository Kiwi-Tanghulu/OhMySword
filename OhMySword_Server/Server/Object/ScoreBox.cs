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

            // 테이블에서 경험치 테이블 인덱스 뽑아야 함
            // 이놈 아이디 정보, 새로운 포지션 인덱스 정보 전송해야 함
            GenerateXP();
            ushort posTableIndex = ResetPosition();
            
            S_ScoreBoxPacket broadcastPacket = new S_ScoreBoxPacket(objectID, posTableIndex);
            room.AddJob(() => room.Broadcast(broadcastPacket));
        }

        private ushort ResetPosition()
        {
            int posTableIndex = Random.Range(0, ScoreBoxSpawnTable.Length);
            position = ScoreBoxSpawnTable[posTableIndex];

            return (ushort)posTableIndex;
        }

        private void GenerateXP()
        {
            for(int i = 0; i < 3; i++)
            {
                ushort xpAmount = (ushort)MathF.Pow(10, i);
                foreach (Vector3 pos in XPSpawnTable[objectType][i])
                {
                    XPObject xp = new XPObject(room, xpAmount);
                    xp.position = pos;
                    room.PublishObject(xp);
                }
            }
        }
    }
}
