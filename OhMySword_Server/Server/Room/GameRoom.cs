using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class GameRoom : Room
    {
        public int maxPlayerCount = 20;
        public ushort RoomID;

        public int Capacity => (maxPlayerCount - players.Count);

        public GameRoom()
        {
            for(int i = 0; i < 3; i++)
            {
                ScoreBox scoreBox = new ScoreBox(this, 3, ObjectType.StoneScoreBox);
                scoreBox.ResetPosition();
                PublishObject(scoreBox);
            }

            for (int i = 0; i < 2; i++)
            {
                ScoreBox scoreBox = new ScoreBox(this, 6, ObjectType.WoodenScoreBox);
                scoreBox.ResetPosition();
                PublishObject(scoreBox);
            }

            for (int i = 0; i < 1; i++)
            {
                ScoreBox scoreBox = new ScoreBox(this, 9, ObjectType.EggScoreBox);
                scoreBox.ResetPosition();
                PublishObject(scoreBox);
            }
        }
    }
}
