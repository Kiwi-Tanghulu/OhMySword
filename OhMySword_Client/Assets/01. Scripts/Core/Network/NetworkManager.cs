using System.Net;
using System.Collections.Generic;
using H00N.Network;

public class NetworkManager
{
	public static NetworkManager Instance = null;

    private Connector connector = null;
    private ServerSession session = null;

    private Queue<Packet> packetQueue = new Queue<Packet>();
    private object locker = new object();

    public bool IsConnected = false;

    ~NetworkManager()
    {
        if(IsConnected)
            session.Close();
    }

    public void Connect(string ip, int port)
    {
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ip), port);
        session = new ServerSession();

        connector = new Connector(endPoint, session);
        connector.StartConnect(endPoint);
    }

    public void FlushPacketQueue()
    {
        if(IsConnected == false)
            return;

        while(true)
        {
            lock(locker)
            {
                if(packetQueue.Count <= 0)
                    break;

                Packet packet = packetQueue.Dequeue();
                PacketManager.Instance.HandlePacket(session, packet);
            }
        }
    }

    public void Send(Packet packet)
    {
        session.Send(packet.Serialize());
    }

    public void PushHandlePacket(Packet packet)
    {
        lock(locker)
            packetQueue.Enqueue(packet);
    }
}
