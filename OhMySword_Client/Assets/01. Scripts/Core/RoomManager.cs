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
    }

    public void CreatePlayer(ushort id, ushort posIndex, string nickname)
    {
        PlayerController player = Instantiate(prefabTable[ObjectType.Player]) as PlayerController;
        player.Init(id, playerSpawnTable[posIndex], Vector3.zero);
        player.SetNickname(nickname);

        players.Add(id, player);
        PlayerID = id;
    }

    public void InitRoom(List<PlayerPacket> playerList, List<ObjectPacket> objectList)
    {
        playerList.ForEach(p => {
            PlayerController player = Instantiate(prefabTable[ObjectType.Player]) as PlayerController;
            player.Init(p.objectID, p.position.Vector3(), p.rotation.Vector3());
            player.SetNickname(p.nickname);
            
            players.Add(player.ObjectID, player);
        });

        objectList.ForEach(o => {
            SyncableObject obj = Instantiate(prefabTable[(ObjectType)o.objectType]);
            obj.Init(o.objectID, o.position.Vector3(), o.rotation.Vector3());
            
            objects.Add(obj.ObjectID, obj);
        });
    }
}