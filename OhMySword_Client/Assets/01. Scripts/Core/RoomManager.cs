using System.Collections.Generic;
using Packets;
using UnityEngine;
using OhMySword.Player;
using Base.Network;

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

    public void CreatePlayer(ushort id, ushort posIndex, string nickname)
    {
        PlayerController player = Instantiate(prefabTable[ObjectType.Player]) as PlayerController;
        player.Init(id, playerSpawnTable[posIndex], Vector3.zero);
        player.SetNickname(nickname);

        players.Add(id, player);
        PlayerID = id;
    }

    public void AddRemotePlayer(ushort objectID, Vector3 position, Vector3 rotation, string nickname)
    {
        PlayerController player = Instantiate(prefabTable[ObjectType.Player]) as PlayerController;
        player.Init(objectID, position, rotation);
        player.SetNickname(nickname);

        players.Add(objectID, player);
    }

    public void AddRemotePlayer(ushort objectID, ushort posIndex, string nickname)
        => AddRemotePlayer(objectID, playerSpawnTable[posIndex], Vector3.zero, nickname);

    public void AddRemoteObject(ushort objectID, ObjectType objectType, Vector3 position, Vector3 rotation)
    {
        SyncableObject obj = Instantiate(prefabTable[objectType]);
        obj.Init(objectID, position, rotation);

        objects.Add(objectID, obj);
    }

    public void InitRoom(List<PlayerPacket> playerList, List<ObjectPacket> objectList)
    {
        playerList.ForEach(p => AddRemotePlayer(p.objectID, p.position.Vector3(), p.rotation.Vector3(), p.nickname));
        objectList.ForEach(o => AddRemoteObject(o.objectID, (ObjectType)o.objectType, o.position.Vector3(), o.rotation.Vector3()));
    }
}