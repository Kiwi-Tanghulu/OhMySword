using H00N.Network;
using System;
using System.Net;
using UnityEngine;

public class ClientSession : Session
{
    public override void OnConnected(EndPoint endPoint)
    {
        Debug.Log($"{endPoint} : Ŭ���̾�Ʈ�� �����߽��ϴ�.");
    }

    public override void OnDisconnected(EndPoint endPoint)
    {
        Debug.Log($"{endPoint} : Ŭ���̾�Ʈ�� ������ �����߽��ϴ�.");
    }

    public override void OnPacketReceived(ArraySegment<byte> buffer)
    {

    }

    public override void OnSent(int length)
    {
        Debug.Log($"{length} : �����Ͱ� ���۵Ǿ����ϴ�.");
    }
}
