using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class GameRoom : Room
    {

        public override void Init()
        {
            base.Init();
            objects.Clear();
        }

        public void AddObject(ushort id, ObjectBase obj)
        {
        }
    }
}
