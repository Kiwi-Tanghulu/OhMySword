using UnityEngine;
using System.Net;
using H00N.Network;
using System.Net.Sockets;

public class NetworkManager
{
    public static NetworkManager Instance;

    private Listener listener = null;

    public void Listen(string ip, int port)
    {
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ip), port);
        listener = new Listener(endPoint);
        if (listener.Listen(10))
            listener.StartAccept(OnAccepted);
    }

    private void OnAccepted(Socket socket)
    {
        ClientSession session = new ClientSession();
        session.Open(socket);
        session.OnConnected(socket.RemoteEndPoint);
    }
}
