using System.Collections.Generic;
using Packets;
using UnityEngine;
using OhMySword.Player;
using Base.Network;
using System;
using System.Linq;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance = null;

    [SerializeField] SyncableObjectPrefabTableSO prefabTable;
    [SerializeField] PositionTableSO playerSpawnTable;
    [SerializeField] PlayerController playerPrefab;

    public ushort PlayerID { get; private set; }
    
    private Dictionary<ushort, PlayerController> players = new Dictionary<ushort, PlayerController>();
    private Dictionary<ushort, SyncableObject> objects = new Dictionary<ushort, SyncableObject>();

    [Space(10f), SerializeField] int boardCount = 5;
    public event Action<List<PlayerController>> OnRankingChangedEvent = null;
    [SerializeField] private List<PlayerController> rankBoard = null;

	private void Awake()
    {
        if(Instance != null)
            DestroyImmediate(Instance.gameObject);

        Instance = this;
        prefabTable.Init();
    }

    public void UpdateRankingBoard()
    {
        rankBoard = players.Values.ToList();
        rankBoard.Sort();
        OnRankingChangedEvent?.Invoke(rankBoard.GetRange(0, boardCount));
    }

    public PlayerController GetPlayer(ushort id)
    {
        if(players.ContainsKey(id) == false)
            return null;

        return players[id];
    }

    public SyncableObject GetObject(ushort id)
    {
        if(objects.ContainsKey(id) == false)
            return null;

        return objects[id];
    }

    public void CreatePlayer(ushort id, ushort posIndex, string nickname)
    {
        PlayerController player = Instantiate(playerPrefab) as PlayerController;
        player.Init(id, playerSpawnTable[posIndex], Vector3.zero);
        player.SetNickname(nickname);
        player.OnCreated();

        players.Add(id, player);
        PlayerID = id;
    }

    public PlayerController AddPlayer(ushort objectID, Vector3 position, Vector3 rotation, string nickname)
    {
        PlayerController player = Instantiate(prefabTable[ObjectType.Player]) as PlayerController;
        player.Init(objectID, position, rotation);
        player.SetNickname(nickname);
        player.OnCreated();

        players.Add(objectID, player);
        return player;
    }

    public PlayerController AddPlayer(ushort objectID, ushort posIndex, string nickname) 
        => AddPlayer(objectID, playerSpawnTable[posIndex], Vector3.zero, nickname);

    public SyncableObject AddObject(ushort objectID, ObjectType objectType, Vector3 position, Vector3 rotation)
    {
        SyncableObject obj = Instantiate(prefabTable[objectType]);
        obj.Init(objectID, position, rotation);
        obj.OnCreated();

        objects.Add(objectID, obj);
        return obj;
    }

    public void DeletePlayer(ushort id)
    {
        if(players.ContainsKey(id) == false)
            return;

        players[id].OnDeleted();
        Destroy(players[id].gameObject);
    }

    public void DeleteObject(ushort id)
    {
        if(objects.ContainsKey(id) == false)
            return;

        objects[id].OnDeleted();
        Destroy(objects[id].gameObject);
    }

    public void InitRoom(List<PlayerPacket> playerList, List<ObjectPacket> objectList)
    {
        playerList.ForEach(p => AddPlayer(p.objectID, p.position.Vector3(), p.rotation.Vector3(), p.nickname));
        objectList.ForEach(o => AddObject(o.objectID, (ObjectType)o.objectType, o.position.Vector3(), o.rotation.Vector3()));
    }

    public void Chatting(string chat, ushort id)
    {
        if(players.ContainsKey(id) == false)
            return;

        PlayerController sender = players[id];
        sender.DoChat(chat);

        UIManager.Instance.ChattingPanel?.DoChat(sender.nickname, chat);
    }
}