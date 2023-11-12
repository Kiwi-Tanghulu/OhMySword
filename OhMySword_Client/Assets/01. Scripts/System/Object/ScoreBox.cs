using System.Collections.Generic;
using Base.Network;
using Packets;
using UnityEngine;
using UnityEngine.Events;

public class ScoreBox : SyncableObject, IDamageable, IHitable
{
    [SerializeField] ScoreBoxDropTableSO dropTable = null;
    [SerializeField] PositionTableSO positionTable = null;
    [SerializeField] ObjectType type = ObjectType.None;

    [Space(10f)]
    [SerializeField] UnityEvent OnMovedEvent;

    [Space(10f)]
    [SerializeField] Color gizmoColor;

    public override void OnCreated()
    {
        
    }

    public override void OnDeleted()
    {

    }

    public void OnDamage(int damage, GameObject performer, Vector3 point)
    {
        SyncableObject attacker = performer.GetComponent<SyncableObject>();
        if (attacker == null)
            return;

        C_AttackPacket attackPacket = new C_AttackPacket((ushort)type, ObjectID, attacker.ObjectID, (ushort)damage);
        NetworkManager.Instance.Send(attackPacket);
    }

    public void Hit(SyncableObject attacker)
    {

    }

    public void CreateXP(List<UShortPacket> ids)
    {
        dropTable.score.ForEachDigit((digit, number, index) => {
            XPObject xp = RoomManager.Instance.AddObject(
                ids[index], 
                ObjectType.XPObject, 
                transform.position, 
                Vector3.zero
            ) as XPObject;

            xp.SetXP(digit);
            xp.SetPosition(transform.position, true);
            xp.SetPosition(transform.position + dropTable[index], false);
        });
    }

    public void SetPosition(ushort posIndex)
    {
        SetPosition(positionTable[posIndex], true);

        OnMovedEvent?.Invoke();
    }

    #if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        try
        {
            Gizmos.color = gizmoColor;
            if(TryGetComponent<MeshFilter>(out MeshFilter filter))
            {
                if(filter.sharedMesh != null)
                    Gizmos.DrawWireMesh(filter.sharedMesh, 0, transform.position, transform.rotation, transform.localScale);
            }
        }catch {}
    }

    #endif
}
