using H00N.Network;
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

        //public async Task FlushRoomPackets()
        //{
        //    await Task.Run(() => {
        //        foreach(GameRoom room in gameRooms.Values)
        //            room.FlushBroadcastQueue();
        //    });
        //}

        public void FlushLoop(int delay)
        {
            int lastFlushTime = Environment.TickCount;

            while (true)
            {
                int currentTime = Environment.TickCount;
                if (currentTime - lastFlushTime > delay)
                {
                    FlushRoomPackets();
                    lastFlushTime = currentTime;
                }
            }
        }

        private void FlushRoomPackets()
        {
            foreach (GameRoom room in gameRooms.Values)
            {
                room.AddJob(() => {
                    int packetCount = room.FlushBroadcastQueue();
                    //if(packetCount > 0)
                        //Console.WriteLine($"[Room] {packetCount} Packets Flushed");
                });
            }
        }

        public void Clear()
        {
            foreach (KeyValuePair<ushort, GameRoom> p in gameRooms)
                p.Value.AddJob(p.Value.Clear);
        }

        public void PublishRoomID(GameRoom room)
        {
            lock (locker)
            {
                room.RoomID = roomIDPublisher++;
                gameRooms.Add(room.RoomID, room);
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
