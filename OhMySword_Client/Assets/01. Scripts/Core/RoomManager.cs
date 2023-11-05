using System.Collections.Generic;
using Packets;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance = null;

    public ushort PlayerID { get; private set; }
    
    private Dictionary<ushort, PlayerControl> players = new Dictionary<ushort, PlayerControl>();

	private void Awake()
    {
        if(Instance != null)
            DestroyImmediate(Instance.gameObject);

        Instance = this;
    }

    public void CreatePlayer(ushort id, ushort posIndex, string nickname)
    {
        PlayerID = id;
        // 풀링으로 생성해야 함
        // players.Add()
    }

    public void InitRoom(List<PlayerPacket> playerList, List<ObjectPacket> objectList)
    {
        playerList.ForEach(p => {
            // 생성 & 추가
        });

        objectList.ForEach(o => {
            // 생성 & 추가
        });
    }
}