using H00N.Network;
using System;
using System.Net;

namespace Server
{
    public class ClientSession : Session
    {
        public ushort UserID;
        public GameRoom Room;

        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"{endPoint} : 클라이언트가 접속했습니다.");

            UserID = NetworkManager.Instance.PublishUserID(this);
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            Console.WriteLine($"{endPoint} : 클라이언트가 접속을 해제했습니다.");
        }

        public override void OnPacketReceived(ArraySegment<byte> buffer)
        {

        }

        public override void OnSent(int length)
        {
            Console.WriteLine($"{length} : 데이터가 전송되었습니다.");
        }
    }
}