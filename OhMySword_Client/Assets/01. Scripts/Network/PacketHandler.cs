using H00N.Network;
using Packets;
using UnityEngine;

public class PacketHandler
{
	public static void S_LogInPacket(Session session, Packet packet)
    {
        S_LogInPacket logInPacket = packet as S_LogInPacket;
        
        GameManager.Instance.UserID = logInPacket.userID;
    }
}
