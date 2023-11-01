using H00N.Network;
using System.Net.Sockets;
using System.Net;

namespace Server
{
    public class NetworkManager
    {
        public static NetworkManager Instance;

        private Listener listener = null;
        private Dictionary<ushort, GameRoom> gameRooms = new Dictionary<ushort, GameRoom>();
        private Dictionary<ushort, ClientSession> users = new Dictionary<ushort, ClientSession>();

        private object locker = new object();

        public void Clear()
        {
            lock (locker)
            {
                listener = null;

                foreach (KeyValuePair<ushort, GameRoom> p in gameRooms)
                    p.Value.AddJob(p.Value.Clear);

                foreach (KeyValuePair<ushort, ClientSession> p in users)
                    p.Value.Close();

                users.Clear();
            }
        }

        public bool Listen(string ip, int port)
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            listener = new Listener(endPoint);

            bool success = listener.Listen(10);
            if (success)
            {
                listener.StartAccept(OnAccepted);
            }

            return success;
        }

        private void OnAccepted(Socket socket)
        {
            ClientSession session = new ClientSession();
            session.Open(socket);
            session.OnConnected(socket.RemoteEndPoint);
        }
    }
}
