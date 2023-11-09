using System.Collections.Generic;
using Packets;
using UnityEngine;
using OhMySword.Player;
using Base.Network;
using System;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance = null;

    [SerializeField] SyncableObjectPrefabTableSO prefabTable;
    [SerializeField] PositionTableSO playerSpawnTable;

    public ushort PlayerID { get; private set; }
    
    private Dictionary<ushort, PlayerController> players = new Dictionary<ushort, PlayerController>();
    private Dictionary<ushort, SyncableObject> objects = new Dictionary<ushort, SyncableObject>();

	private void Awake()
    {
        if(Instance != null)
            DestroyImmediate(Instance.gameObject);

        Instance = this;
        prefabTable.Init();
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
        PlayerController player = AddPlayer(id, posIndex, nickname);
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

        // $"{sender.nickname} : {chat}"으로 채팅 띄우기
    }
}