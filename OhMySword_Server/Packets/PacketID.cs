namespace Packets
{
    public enum PacketID
    {
        S_LogInPacket,      // 클라가 서버에 접속했을 때 아이디 발급 후 전송할 패킷
        C_RoomEnterPacket,  // 클라가 서버에게 방 참가 요청을 보내는 패킷
        C_RoomExitPacket,   // 클라가 서버에게 방을 나갈 때 보내는 패킷
        S_RoomEnterPacket,  // 서버가 클라에게 방 참가에 대한 응답을 보내는 패킷
        S_OtherJoinPacket,  // 서버가 나머지 클라에게 새로운 클라이언트가 접속했음을 알리는 패킷
        S_OtherExitPacket,  // 서버가 나머지 클라에게 누군가가 나갔다는 것을 알리는 패킷
        C_AttackPacket,     // 클라가 누군가를 공격했을 때 서버에게 전송할 패킷
        S_AttackPacket,     // 서버가 클라들에게 누군가가 공격당했다는 것을 알리는 패킷
        C_ScorePacket,      // 클라가 점수를 획득했을 때 서버에게 전송할 패킷
        S_ScorePacket,      // 서버가 클라들에게 누군가가 성장했다는 것을 알리는 패킷
        C_MovePacket,       // 클라가 움직였을 때 서버에게 전송할 패킷
        S_MovePacket,       // 서버가 클라들에게 누군가가 움직였다는 것을 알릴 패킷
        C_RotatePacket,     // 클라가 회전했을 때 서버에게 전송할 패킷
        S_RotatePacket,     // 서버가 클라들에게 누군가가 회전했다는 것을 알릴 패킷
    }
}
