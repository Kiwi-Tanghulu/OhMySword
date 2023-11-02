using H00N.Network;
using Packets;

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
        public void Broadcast(Packet packet, ushort except = ushort.MaxValue) => broadcastQueue.Enqueue(new BroadcastPacket(packet, except));

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

        public List<PlayerPacket> GetPlayerList(ushort except)
        {
            List<PlayerPacket> playerList = new List<PlayerPacket>();

            foreach (KeyValuePair<ushort, Player> p in players)
            {
                Player player = p.Value;
                if (player.objectID != except)
                    playerList.Add(player);
            }

            return playerList;
        }

        public List<ObjectPacket> GetObjectList() => objects.Select(i => (ObjectPacket)i.Value).ToList();

        #region Managing Obejcts
        public bool GetObject<T>(ushort id, out T obj) where T : ObjectBase
        {
            obj = null;

            if (objects.ContainsKey(id))
                obj = objects[id] as T;

            return (obj != null);
        }

        public void PublishObject(ObjectBase obj)
        {
            obj.objectID = objectIDPublisher++;
            objects.Add(obj.objectID, obj);
        }

        public void ReleaseObject(ObjectBase obj)
        {
            objects.Remove(obj.objectID);
            obj.objectID = 0;
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

        public void PublishPlayer(Player player)
        {
            player.objectID = playerIDPublisher++;
            players.Add(player.objectID, player);
        }

        public void ReleasePlayer(Player player)
        {
            players.Remove(player.objectID);
            player.session = null;
            player.objectID = 0;
        }
        #endregion
    }
}
