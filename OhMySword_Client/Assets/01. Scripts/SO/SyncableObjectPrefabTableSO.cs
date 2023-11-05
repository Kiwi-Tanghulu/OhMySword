using System.Collections.Generic;
using Base.Network;
using UnityEngine;

[CreateAssetMenu(menuName = "Table/PrefabTable")]
public class SyncableObjectPrefabTableSO : ScriptableObject
{
    [System.Serializable]
    public struct SyncableObjectPrefabTable
    {
        public ObjectType type;
        public SyncableObject prefab;
    }

    [SerializeField] List<SyncableObjectPrefabTable> table = new List<SyncableObjectPrefabTable>();
    private Dictionary<ObjectType, SyncableObject> tableMap = new Dictionary<ObjectType, SyncableObject>();

    public SyncableObject this[ObjectType type] {
        get {
            if(tableMap.ContainsKey(type) == false)
                return null;
            return tableMap[type];
        }
    }

    public void Init()
    {
        table.ForEach(i => {
            if(tableMap.ContainsKey(i.type))
                return;
            tableMap.Add(i.type, i.prefab);
        });
    }
}
