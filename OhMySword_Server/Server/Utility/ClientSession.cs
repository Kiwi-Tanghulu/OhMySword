using H00N.Network;
using Packets;
using System;
using System.Net;
using static System.Collections.Specialized.BitVector32;

namespace Server
{
    public class ClientSession : Session
    {
        public ushort UserID;
        public GameRoom Room;
        public Player Player;

        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"[Session] Client Connected {{ Time : {DateTime.Now.ToString($"yy년 MM월 dd일 HH:mm:ss")}, Address : {endPoint} }}");

            UserID = NetworkManager.Instance.PublishUserID(this);
            S_LogInPacket logInPacket = new S_LogInPacket() { userID = UserID };
            Send(logInPacket.Serialize());
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            Console.WriteLine($"[Session] Client Disconnected {{ Time : {DateTime.Now.ToString("yy년 MM월 dd일 HH:mm:ss")}, Address : {endPoint} }}");
            NetworkManager.Instance.ReleaseUser(this);
        }

        public override void OnPacketReceived(ArraySegment<byte> buffer)
        {
            //Console.WriteLine($"[Session] Packet Received : {buffer.Count}");

            try {
                Packet packet = PacketManager.Instance.CreatePacket(buffer);
                PacketManager.Instance.HandlePacket(this, packet);
            } catch (Exception err) {
                Console.WriteLine(err.Message);
                RelayError();
            }
        }

        public override void OnSent(int length)
        {
            //Console.WriteLine($"[Session] Data Sent : {length}");
        }

        public void RelayError()
        {
            Room?.ReleasePlayer(Player);

            S_ErrorPacket errorPacket = new S_ErrorPacket();
            Send(errorPacket.Serialize());
            
            NetworkManager.Instance.ReleaseUser(this);
        }
    }
}