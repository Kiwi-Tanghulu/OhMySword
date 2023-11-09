namespace Packets
{
    public enum PacketID
    {
        S_LogInPacket,          // 클라가 서버에 접속했을 때 아이디 발급 후 전송할 패킷
        C_RoomEnterPacket,      // 클라가 서버에게 방 참가 요청을 보내는 패킷
        C_RoomExitPacket,       // 클라가 서버에게 방을 나갈 때 보내는 패킷
        S_RoomEnterPacket,      // 서버가 클라에게 방 참가에 대한 응답을 보내는 패킷
        S_OtherJoinPacket,      // 서버가 나머지 클라에게 새로운 클라이언트가 접속했음을 알리는 패킷
        S_OtherExitPacket,      // 서버가 나머지 클라에게 누군가가 나갔다는 것을 알리는 패킷
        C_AttackPacket,         // 클라가 누군가를 공격했을 때 서버에게 전송할 패킷
        S_AttackPacket,         // 서버가 클라들에게 누군가가 공격당했다는 것을 알리는 패킷
        S_ScorePacket,          // 서버가 클라들에게 누군가가 성장했다는 것을 알리는 패킷
        C_PlayerPacket,         // 클라가 서버에게 전송하는 플레이어 정보 패킷
        S_PlayerPacket,         // 서버가 클라들에게 전송하는 플레이어 정보 패킷
        S_ScoreBoxPacket,       // 서버가 클라들에게 전송하는 점수 구조물 정보 패킷
        S_ObjectDestroyPacket,  // 서버가 클라들에게 오브젝트가 삭제되었다는 걸 알리는 패킷
        S_PlayerDiePacket,      // 서버가 클라들에게 플레이어가 죽었다는 걸 알리는 패킷
        C_ChattingPacket,       // 클라가 서버에게 보내는 채팅 패킷
        S_ChattingPacket,       // 서버가 클라에게 보내는 채팅 패킷
    }
}
