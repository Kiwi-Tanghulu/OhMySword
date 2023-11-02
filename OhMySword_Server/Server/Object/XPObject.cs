using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class XPObject : ObjectBase
    {
        public ushort xp;

        public XPObject(GameRoom room, ushort xp)
        {
            this.room = room;
            this.xp = xp;
        }

        public override void Hit(ushort damage)
        {
            // 이거 채워야 함
        }
    }
}
