using Packets;
using static Server.DEFINE;

namespace Server
{
    public class ScoreBox : ObjectBase
    {
        public ushort hp;
        private ushort currnetHP;

        public ScoreBox(GameRoom room, ushort hp, ObjectType type)
        {
            this.room = room;
            this.hp = hp;
            this.objectType = (ushort)type;

            currnetHP = hp;
        }

        public override void Hit(ushort damage, ushort attacker)
        {
            currnetHP -= damage;
            if (currnetHP > 0)
                return;

            if (room.GetPlayer(attacker, out Player player))
                player.destroyCount += 1;

            // 테이블에서 경험치 테이블 인덱스 뽑아야 함
            // 이놈 아이디 정보, 새로운 포지션 인덱스 정보 전송해야 함
            currnetHP = hp;
            List<UShortPacket> ids = GenerateXP();
            ushort posTableIndex = ResetPosition();
            
            S_ScoreBoxPacket broadcastPacket = new S_ScoreBoxPacket(objectID, posTableIndex, ids);
            room.AddJob(() => room.Broadcast(broadcastPacket));
        }

        public ushort ResetPosition()
        {
            int posTableIndex = Random.Range(0, ScoreBoxSpawnTables[objectType].Length);
            position = ScoreBoxSpawnTables[objectType][posTableIndex];

            return (ushort)posTableIndex;
        }

        private List<UShortPacket> GenerateXP()
        {
            List<UShortPacket> ids = new List<UShortPacket>();

            XPSpawnTable[objectType].score.ForEachDigit((digit, number, index) => {
                XPObject xp = new XPObject(room, digit);
                room.PublishObject(xp);

                xp.position = position + XPSpawnTable[objectType].positions[index];
                ids.Add(xp.objectID);
            });

            return ids;
        }
    }
}
