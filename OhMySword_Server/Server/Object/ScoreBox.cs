using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class ScoreBox : ObjectBase
    {
        public ushort provideScore;
        public ushort hp;

        public ScoreBox(ushort id, ClientSession session, ushort score, ushort hp)
        {
            this.objectID = id;
            this.session = session;
            this.provideScore = score;
            this.hp = hp;
        }
    }
}
