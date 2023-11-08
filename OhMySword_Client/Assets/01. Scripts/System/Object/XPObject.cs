using Base.Network;
using DG.Tweening;
using UnityEngine;

public class XPObject : SyncableObject, IDamageable
{
    [SerializeField] float rotateSpeed = 30f;
    
    [Space(10f)]
    [SerializeField] float jumpPower = 1.5f;
    [SerializeField] float jumpDuration = 1f;
    
    private ushort xpAmount = 0;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            Vector3 err = new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));
            err.y = 0;
            SetPosition(transform.position + err);
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        transform.Rotate(new Vector3(0, rotateSpeed * Time.fixedDeltaTime, 0), Space.World);
    }

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

    public override void SetPosition(Vector3 position, bool immediately = false)
    {
        base.SetPosition(position, false);

        float factor = 1f + xpAmount.ToString().Length * 0.5f;
        transform.DOJump(position, jumpPower * factor, 1, jumpDuration);
    }
}
