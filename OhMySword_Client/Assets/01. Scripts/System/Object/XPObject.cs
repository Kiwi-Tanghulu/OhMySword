using Base.Network;
using UnityEngine;

public class XPObject : SyncableObject, IDamageable
{
    private ushort xpAmount = 0;

    public void SetXP(ushort amount)
    {
        xpAmount = amount;
        // 뭐 대충 이펙트 하면 되겠지
    }

    public override void OnCreated()
    {
        
    }

    public override void OnDeleted()
    {
        
    }

    public void OnDamage(int damage, GameObject performer, Vector3 point)
    {
        
    }
}
