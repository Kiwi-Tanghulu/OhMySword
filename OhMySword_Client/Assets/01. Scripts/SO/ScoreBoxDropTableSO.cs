using UnityEngine;

[CreateAssetMenu(menuName = "Table/ScoreBoxDropTable")]
public class ScoreBoxDropTableSO : ScriptableObject
{
	public ushort score;

    [SerializeField] Vector3[] table;
    public Vector3 this[int index] => table[index];
}
