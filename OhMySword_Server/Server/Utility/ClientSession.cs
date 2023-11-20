using H00N.Network;
using Packets;
using System;
using System.Net;

namespace Server
{
    public class ClientSession : Session
    {
        public ushort UserID;
        public GameRoom Room;
        public Player Player;

        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"[Session] Client Connected : {endPoint}.");

            UserID = NetworkManager.Instance.PublishUserID(this);
            S_LogInPacket logInPacket = new S_LogInPacket() { userID = UserID };
            Send(logInPacket.Serialize());
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            Console.WriteLine($"[Session] Client Disconnected : {endPoint}.");
            NetworkManager.Instance.ReleaseUser(this);
        }

        public override void OnPacketReceived(ArraySegment<byte> buffer)
        {
            //Console.WriteLine($"[Session] Packet Received : {buffer.Count}");
            Packet packet = PacketManager.Instance.CreatePacket(buffer);
            PacketManager.Instance.HandlePacket(this, packet);
        }

        public override void OnSent(int length)
        {
            //Console.WriteLine($"[Session] Data Sent : {length}");
        }
    }
}