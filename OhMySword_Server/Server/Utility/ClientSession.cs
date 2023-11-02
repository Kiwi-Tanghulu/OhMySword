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
            Console.WriteLine($"{endPoint} : Ŭ���̾�Ʈ�� �����߽��ϴ�.");

            UserID = NetworkManager.Instance.PublishUserID(this);
            S_LogInPacket logInPacket = new S_LogInPacket() { userID = UserID };
            Send(logInPacket.Serialize());
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            Console.WriteLine($"{endPoint} : Ŭ���̾�Ʈ�� ������ �����߽��ϴ�.");
        }

        public override void OnPacketReceived(ArraySegment<byte> buffer)
        {

        }

        public override void OnSent(int length)
        {
            Console.WriteLine($"{length} : �����Ͱ� ���۵Ǿ����ϴ�.");
        }
    }
}