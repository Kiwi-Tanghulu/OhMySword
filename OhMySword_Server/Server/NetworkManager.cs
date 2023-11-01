using H00N.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class NetworkManager
    {
        public static NetworkManager Instance;

        private Listener listener = null;

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
