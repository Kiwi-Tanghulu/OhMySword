using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Core;

namespace Server
{
    public class GameRoom : Room
    {
        public int maxPlayerCount;
        public ushort RoomID;

        public int Capacity => (maxPlayerCount - players.Count);
    }
}
