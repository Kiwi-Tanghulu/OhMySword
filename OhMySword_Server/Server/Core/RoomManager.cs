using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class RoomManager
    {
        public static RoomManager Instance;

        private Dictionary<ushort, GameRoom> gameRooms = new Dictionary<ushort, GameRoom>();
        private ushort roomIDPublisher = 0;

        private object locker = new object();

        public void Clear()
        {
            foreach (KeyValuePair<ushort, GameRoom> p in gameRooms)
                p.Value.AddJob(p.Value.Clear);
        }

        public ushort PublishRoomID(GameRoom room)
        {
            lock (locker)
            {
                ushort id = roomIDPublisher++;
                gameRooms.Add(id, room);

                return id;
            }
        }

        public GameRoom GetRoom()
        {
            foreach (KeyValuePair<ushort, GameRoom> p in gameRooms)
            {
                if (p.Value.Capacity > 0)
                    return p.Value;
            }

            GameRoom room = new GameRoom();
            PublishRoomID(room);

            return room;
        }
    }
}
