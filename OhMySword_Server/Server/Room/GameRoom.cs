using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class GameRoom : Room
    {
        public int maxPlayerCount = 5;
        public ushort RoomID;

        public int Capacity => (maxPlayerCount - players.Count);

        public GameRoom()
        {
            //ScoreBox scoreBox = new ScoreBox(this, 10, ObjectType.WoodenScoreBox);
            //PublishObject(scoreBox);
        }
    }
}
