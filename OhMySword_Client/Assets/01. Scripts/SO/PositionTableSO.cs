using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Table/PositionTable")]
public class PositionTableSO : ScriptableObject
{
    [SerializeField] List<Vector3> table = new List<Vector3>();

    public Vector3 this[ushort index] => table[index];
}
