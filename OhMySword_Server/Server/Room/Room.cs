using H00N.Network;

namespace Server
{
    public class Room
    {
        protected Dictionary<ushort, Player> players = new Dictionary<ushort, Player>();
        protected Dictionary<ushort, ObjectBase> objects = new Dictionary<ushort, ObjectBase>();

        protected JobQueue jobQueue = new JobQueue();
        protected Queue<BroadcastPacket> broadcastQueue = new Queue<BroadcastPacket>();

        public ushort playerIDPublisher = 0;
        public ushort objectIDPublisher = 0;

        public virtual void Clear()
        {
            players.Clear();
            broadcastQueue.Clear();
        }

        public void AddJob(Action job) => jobQueue.Push(job);
        public void Broadcast(Packet packet, ushort except) => broadcastQueue.Enqueue(new BroadcastPacket(packet, except));

        public void FlushBroadcastQueue()
        {
            while(broadcastQueue.Count > 0)
            {
                BroadcastPacket packet = broadcastQueue.Dequeue();
                ArraySegment<byte> buffer = packet.packet.Serialize();

                foreach (KeyValuePair<ushort, Player> p in players)
                {
                    Player player = p.Value;
                    if (player.session.UserID == packet.except)
                        continue;

                    player.session.Send(buffer);
                }
            }
        }

        #region Managing Obejcts
        public bool GetObject<T>(ushort id, out T obj) where T : ObjectBase
        {
            obj = null;

            if (objects.ContainsKey(id))
                obj = objects[id] as T;

            return (obj != null);
        }

        public ushort PublishObject(ObjectBase obj)
        {
            ushort id = objectIDPublisher++;
            objects.Add(id, obj);

            return id;
        }

        public bool GetPlayer(ushort id, out Player player)
        {
            player = null;

            if (players.ContainsKey(id))
            {
                player = players[id];
                return true;
            }
            else
                return false;
        }

        public ushort PublishPlayer(Player player)
        {
            ushort id = playerIDPublisher++;
            players.Add(id, player);

            return id;
        }

        public void AddPlayer(ushort id, Player player)
        {
            if (players.ContainsKey(id))
                return;

            players.Add(id, player);
        }
        #endregion
    }
}
