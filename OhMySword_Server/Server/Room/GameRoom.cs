using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class GameRoom : Room
    {
        public ushort RoomID;

        public GameRoom()
        {
            RoomID = NetworkManager.Instance.PublushRoomID(this);
        }
    }
}
