using System;
using H00N.Network;
using Packets;
using UnityEngine;

public class PacketHandler
{
	public static void S_LogInPacket(Session session, Packet packet)
    {
        S_LogInPacket logInPacket = packet as S_LogInPacket;
        
        GameManager.Instance.UserID = logInPacket.userID;
        GameObject.Find("Canvas/BlockPanel").SetActive(false);
    }

    public static void S_RoomEnterPacket(Session session, Packet packet)
    {
        S_RoomEnterPacket enterPacket = packet as S_RoomEnterPacket;
        // string nickname = UIManager.Instnace.RoomPanel.Nickname;
        string nickname = "This is Nickname";

        SceneLoader.Instance.LoadSceneAsync("InGameScene", () => {
            RoomManager.Instance.CreatePlayer(enterPacket.playerID, enterPacket.posTableIndex, nickname);
            RoomManager.Instance.InitRoom(enterPacket.players, enterPacket.objects);
        });
    }
}
