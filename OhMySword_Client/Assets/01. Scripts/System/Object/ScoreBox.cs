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
    [SerializeField] UnityEvent OnHitEvent;
    [SerializeField] AudioSource audioPlayer;

    [Space(10f)]
    [SerializeField] Color gizmoColor;

    private Vector3 hitpos;

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
        hitpos = point;
    }

    public void Hit(SyncableObject attacker)
    {
        OnHitEvent?.Invoke();
        AudioManager.Instance.PlayAudio("Hit", audioPlayer, true);
        PoolManager.Instance.Pop("HitEffect", hitpos);
    }

    public void CreateXP(List<UShortPacket> ids)
    {
        AudioManager.Instance.PlayAudio("CreateXP", audioPlayer, true);
        dropTable.score.ForEachDigit((digit, number, index) => {
            XPObject xp = RoomManager.Instance.AddObject(
                ids[index], 
                ObjectType.XPObject, 
                transform.position, 
                Vector3.zero
            ) as XPObject;

            xp.SetXP(digit);
            xp.SetPosition(transform.position + dropTable[index], false);
        });
    }

    public void SetPosition(ushort posIndex)
    {
        AudioManager.Instance.PlayAudio("ScoreBoxBreak", audioPlayer, true);
        SetPosition(positionTable[posIndex], true);
        AudioManager.Instance.PlayAudio("ScoreBoxCreated", audioPlayer, true);
        OnMovedEvent?.Invoke();
    }

    #if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        try
        {
            Gizmos.color = gizmoColor;
            MeshFilter[] filter = GetComponentsInChildren<MeshFilter>();
            if(filter.Length > 0)
            {
                if(filter[0].sharedMesh != null)
                    Gizmos.DrawWireMesh(filter[0].sharedMesh, 0, filter[0].transform.position, filter[0].transform.rotation, filter[0].transform.localScale);
            }
        }catch {}
    }

    #endif
}
