using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Table/PositionTable")]
public class PositionTableSO : ScriptableObject
{
    [SerializeField] Vector3[] table;

    public Vector3 this[ushort index] => table[index];
}
