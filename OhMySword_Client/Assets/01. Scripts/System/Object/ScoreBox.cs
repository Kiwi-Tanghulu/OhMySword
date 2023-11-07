using System.Collections.Generic;
using Base.Network;
using Packets;
using UnityEngine;

public class ScoreBox : SyncableObject, IDamageable, IHitable
{
    [SerializeField] ScoreBoxDropTableSO dropTable = null;
    [SerializeField] PositionTableSO positionTable = null;

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

        C_AttackPacket attackPacket = new C_AttackPacket((ushort)ObjectType.WoodenScoreBox, ObjectID, attacker.ObjectID, (ushort)damage);
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
                transform.position + dropTable[index], 
                Vector3.zero
            ) as XPObject;

            xp.SetXP(digit);
        });
    }

    public void SetPosition(ushort posIndex)
    {
        SetPosition(positionTable[posIndex], true);
    }
}
