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
        GameObject.Find("MainCanvas/BlockPanel").SetActive(false);
    }

    public static void S_RoomEnterPacket(Session session, Packet packet)
    {
        S_RoomEnterPacket enterPacket = packet as S_RoomEnterPacket;
        string nickname = UIManager.Instance.RoomPanel.Nickname;

        SceneLoader.Instance.LoadSceneAsync("InGameScene", () => {
            RoomManager.Instance.CreatePlayer(enterPacket.playerID, enterPacket.posTableIndex, nickname);
            RoomManager.Instance.InitRoom(enterPacket.players, enterPacket.objects);
        });
    }

    public static void S_OtherJoinPacket(Session session, Packet packet)
    {
        S_OtherJoinPacket joinPacket = packet as S_OtherJoinPacket;
        RoomManager.Instance.AddPlayer(joinPacket.playerID, joinPacket.posTableIndex, joinPacket.nickname);
        RoomManager.Instance.UpdateRankingBoard();
    }

    public static void S_OtherExitPacket(Session session, Packet packet)
    {
        S_OtherExitPacket exitPacket = packet as S_OtherExitPacket;
        RoomManager.Instance.DeletePlayer(exitPacket.playerID);
        RoomManager.Instance.UpdateRankingBoard();
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
        PlayerController player = RoomManager.Instance?.GetPlayer(playerPacket.objectPacket.objectID);
        
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

        player.Die(attacker, diePacket.destroyCount);

        ((ushort)(diePacket.score / 2)).ForEachDigit((digit, number, index) => {
            ObjectPacket obj = diePacket.objects[index];
            XPObject xp = RoomManager.Instance.AddObject(
                obj.objectID, 
                ObjectType.XPObject, 
                player.ragdoll.hip.transform.position,
                obj.rotation.Vector3()
            ) as XPObject;

            xp.SetXP(digit);
            xp.SetPosition(player.ragdoll.hip.transform.position + obj.position.Vector3(), false);
        });
    }

    public static void S_ScorePacket(Session session, Packet packet)
    {
        S_ScorePacket scorePacket = packet as S_ScorePacket;
        PlayerController player = RoomManager.Instance.GetPlayer(scorePacket.playerID);
        
        if(player == null)
            return;

        player.GetXP(scorePacket.score, false);
        RoomManager.Instance.UpdateRankingBoard();
    }

    public static void S_ObjectDestroyPacket(Session session, Packet packet)
    {
        S_ObjectDestroyPacket destroyPacket = packet as S_ObjectDestroyPacket;
        RoomManager.Instance.DeleteObject(destroyPacket.objectID);
    }

    public static void S_ScoreBoxPacket(Session session, Packet packet)
    {
        S_ScoreBoxPacket scoreBoxPacket = packet as S_ScoreBoxPacket;
        ScoreBox box = RoomManager.Instance.GetObject(scoreBoxPacket.objectID) as ScoreBox;

        Debug.Log($"XP Objects ID Before : {scoreBoxPacket.ids.Count}");
        box?.CreateXP(scoreBoxPacket.ids);
        Debug.Log($"XP Objects ID After : {scoreBoxPacket.ids.Count}");
        box?.SetPosition(scoreBoxPacket.posTableIndex);
    }

    public static void S_ChattingPacket(Session session, Packet packet)
    {
        S_ChattingPacket chattingPacket = packet as S_ChattingPacket;
        RoomManager.Instance.Chatting(chattingPacket.chat, chattingPacket.playerID);
    }

    public static void S_AnimationPacket(Session session, Packet packet)
    {
        S_AnimationPacket animationPacket = packet as S_AnimationPacket;
        SyncableObject animatingTarget = RoomManager.Instance.GetPlayer(animationPacket.objectID);
        animatingTarget?.PlayAnimation(animationPacket.animationType);
        
    }

    public static void S_ErrorPacket(Session session, Packet packet)
    {
        GameManager.Instance.ResetClient();
    }
}
