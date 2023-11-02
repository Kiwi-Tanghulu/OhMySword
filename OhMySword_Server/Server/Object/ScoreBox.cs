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

        public ScoreBox(ushort score, ushort hp, ObjectType type)
        {
            this.provideScore = score;
            this.hp = hp;
            this.objectType = (ushort)type;
        }
    }
}
