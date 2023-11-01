using H00N.Network;
using System;
using System.Net;
using UnityEngine;

public class ClientSession : Session
{
    public override void OnConnected(EndPoint endPoint)
    {
        Debug.Log($"{endPoint} : 클라이언트가 접속했습니다.");
    }

    public override void OnDisconnected(EndPoint endPoint)
    {
        Debug.Log($"{endPoint} : 클라이언트가 접속을 해제했습니다.");
    }

    public override void OnPacketReceived(ArraySegment<byte> buffer)
    {

    }

    public override void OnSent(int length)
    {
        Debug.Log($"{length} : 데이터가 전송되었습니다.");
    }
}
