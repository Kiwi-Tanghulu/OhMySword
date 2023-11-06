using System;
using Base.Network;
using H00N.Network;
using OhMySword.Player;
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

    public static void S_OtherJoinPacket(Session session, Packet packet)
    {
        S_OtherJoinPacket joinPacket = packet as S_OtherJoinPacket;
        RoomManager.Instance.AddPlayer(joinPacket.playerID, joinPacket.posTableIndex, joinPacket.nickname);
    }

    public static void S_OtherExitPacket(Session session, Packet packet)
    {
        S_OtherExitPacket exitPacket = packet as S_OtherExitPacket;
        RoomManager.Instance.DeletePlayer(exitPacket.playerID);
    }

    public static void S_AttackPacket(Session session, Packet packet)
    {
        S_AttackPacket attackPacket = packet as S_AttackPacket;
        PlayerController attacker = RoomManager.Instance.GetPlayer(attackPacket.attackerID);

        if(attackPacket.hitObjectType == (ushort)ObjectType.Player)
        {
            if(attackPacket.hitObjectID == RoomManager.Instance.PlayerID)
            {
                // 내가 맞았을 때 뭐 특별한 처리 해주면 됨
            }

            RoomManager.Instance.GetPlayer(attackPacket.hitObjectID)?.Hit(attacker);
        }
        else
        {
            IHitable hitObject = RoomManager.Instance.GetObject(attackPacket.hitObjectID) as IHitable;
            hitObject?.Hit(attacker);
        }
    }

    public static void S_PlayerPacket(Session session, Packet packet)
    {
        S_PlayerPacket playerPacket = packet as S_PlayerPacket;
        PlayerController player = RoomManager.Instance.GetPlayer(playerPacket.objectPacket.objectID);
        
        if(player == null)
            return;

        player.SetPosition(playerPacket.objectPacket.position.Vector3());
        player.SetRotation(playerPacket.objectPacket.rotation.Vector3());
    }

    public static void S_PlayerDiePacket(Session session, Packet packet)
    {
        S_PlayerDiePacket diePacket = packet as S_PlayerDiePacket;
        PlayerController player = RoomManager.Instance.GetPlayer(diePacket.playerID);
        PlayerController attacker = RoomManager.Instance.GetPlayer(diePacket.attackerID);

        player.Die();

        int i = 0;
        int score = diePacket.score;
        int cursor = (int)MathF.Pow(10, score.ToString().Length - 1);
        while (cursor > 0)
        {
            int number = score / cursor;
            for (int j = 0; j < number; j++, i++)
            {
                ObjectPacket obj = diePacket.objects[i];
                XPObject xp = RoomManager.Instance.AddObject(
                    obj.objectID, 
                    ObjectType.XPObject, 
                    obj.position.Vector3(), 
                    obj.rotation.Vector3()
                ) as XPObject;

                xp.SetXP((ushort)cursor);
            }

            score %= cursor;
            cursor /= 10;
        }
    }
}
