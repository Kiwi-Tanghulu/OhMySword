using Base.Network;
using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerFeedback : MonoBehaviour
{
    private ActiveRagdoll ragdoll;
    private PlayerMove move;

    [Header("hit")]
    public float hitFeedbackPower = 15f;
    [SerializeField] private float hitEffectPlayOffset;
    [SerializeField] private Transform hitEffectPlayPos;

    [Space]
    [Header("die")]
    [SerializeField] private Transform onDieLeftLegTrm;
    [SerializeField] private Transform onDieRightLegTrm;
    [SerializeField] private Transform onDieRightArmTrm;
    [SerializeField] private Transform RightLegTarget;
    [SerializeField] private Transform LeftLegTarget;
    [SerializeField] private Transform RightArmTarget;
    [SerializeField] private float onDieTransTime = 0.1f;
    public UnityEvent onDie;

    private void Start()
    {
        ragdoll = GetComponent<ActiveRagdoll>();    
        move = GetComponent<PlayerMove>();
    }

    public void HitFeedback(SyncableObject attacker)
    {
        if(attacker == null)
            return;

        Vector3 dir = (ragdoll.hip.transform.position - attacker.GetComponent<ActiveRagdoll>().hip.transform.position).normalized;
        
        PoolableMono hitEffect = PoolManager.Instance.Pop("HitEffect", 
            hitEffectPlayPos.position + -dir * hitEffectPlayOffset);

        ragdoll.AddForceToSpine(dir * hitFeedbackPower);
    }

    public void DieFeedback(SyncableObject attacker)
    {
        onDie?.Invoke();
        Debug.Log("die");
        Transform attackerHip = attacker.GetComponent<ActiveRagdoll>().hip.transform;

        ragdoll.hip.transform.LookAt(attackerHip.position);

        //die event
        //onDie?.Invoke();    
        StartCoroutine(OnDieCo());
    }

    private IEnumerator OnDieCo()
    {
        float percent = 0;

        while(percent <= 1)
        {
            RightArmTarget.position = Vector3.Lerp(RightArmTarget.position, onDieRightArmTrm.position, percent);
            LeftLegTarget.position = Vector3.Lerp(LeftLegTarget.position, onDieLeftLegTrm.position, percent);
            RightLegTarget.position = Vector3.Lerp(RightLegTarget.position, onDieRightArmTrm.position, percent);

            RightArmTarget.rotation = Quaternion.Lerp(RightArmTarget.rotation, onDieRightArmTrm.rotation, percent);
            LeftLegTarget.rotation = Quaternion.Lerp(LeftLegTarget.rotation, onDieLeftLegTrm.rotation, percent);
            RightLegTarget.rotation = Quaternion.Lerp(RightLegTarget.rotation, onDieRightArmTrm.rotation, percent);

            percent += Time.deltaTime / onDieTransTime;

            yield return null;
        }
    }
}
