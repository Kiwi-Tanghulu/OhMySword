using System;
using System.Net;
using H00N.Network;
using UnityEngine;

public class ServerSession : Session
{
    public override void OnConnected(EndPoint endPoint)
    {
        Debug.Log($"{endPoint} : 서버 접속!");
        NetworkManager.Instance.IsConnected = true;
    }

    public override void OnDisconnected(EndPoint endPoint)
    {
        Debug.Log($"{endPoint} : 접속 해제!");
        NetworkManager.Instance.IsConnected = false;
    }

    public override void OnPacketReceived(ArraySegment<byte> buffer)
    {
        Debug.Log($"{buffer.Count} : 데이터 받음!");
        Packet packet = PacketManager.Instance.CreatePacket(buffer);
        Debug.Log(packet.ID);
        NetworkManager.Instance.PushHandlePacket(packet);
    }

    public override void OnSent(int length)
    {
        Debug.Log($"{length} : 데이터 보냄!");
    }
}
