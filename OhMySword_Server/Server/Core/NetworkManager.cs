using H00N.Network;
using System.Net.Sockets;
using System.Net;

namespace Server.Core
{
    public class NetworkManager
    {
        public static NetworkManager Instance;

        private Listener listener = null;
        private Dictionary<ushort, ClientSession> users = new Dictionary<ushort, ClientSession>();

        private ushort userIDPublisher = 0;

        private object locker = new object();

        public void Clear()
        {
            lock (locker)
            {
                listener = null;

                foreach (KeyValuePair<ushort, ClientSession> p in users)
                    p.Value.Close();

                users.Clear();
            }
        }

        public ushort PublishUserID(ClientSession session)
        {
            lock (locker)
            {
                ushort id = userIDPublisher++;
                users.Add(id, session);

                return id;
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
