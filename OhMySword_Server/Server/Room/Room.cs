using H00N.Network;
using System.Numerics;

namespace Server
{
    public class Room
    {
        protected Dictionary<ushort, Player> players = new Dictionary<ushort, Player>();
        protected Dictionary<ushort, ObjectBase> objects = new Dictionary<ushort, ObjectBase>();

        protected JobQueue jobQueue = new JobQueue();
        protected Queue<BroadcastPacket> broadcastQueue = new Queue<BroadcastPacket>();

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
                    if (player.objectID == packet.except)
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

            return (obj == null);
        }
        
        public void AddObject(ushort id, ObjectBase obj)
        {
            if (objects.ContainsKey(id))
                return;

            objects.Add(id, obj);
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

        public void AddPlayer(ushort id, Player player)
        {
            if (players.ContainsKey(id))
                return;

            players.Add(id, player);
        }
        #endregion
    }
}
