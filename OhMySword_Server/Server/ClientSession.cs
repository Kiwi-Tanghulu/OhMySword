using H00N.Network;
using System;
using System.Net;

namespace Server
{
    public class ClientSession : Session
    {
        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"{endPoint} : Ŭ���̾�Ʈ�� �����߽��ϴ�.");
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