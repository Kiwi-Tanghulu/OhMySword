using Base.Network;
using DG.Tweening;
using JetBrains.Annotations;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class PlayerFeedback : MonoBehaviour
{
    private ActiveRagdoll ragdoll;
    private PlayerMove move;

    [Header("hit")]
    public float hitFeedbackPower = 15f;
    [SerializeField] private float hitEffectPlayOffset;
    [SerializeField] private Transform hitEffectPlayPos;

    [Space]
    public UnityEvent<bool> SetActiveRagdollEvent;

    private void Start()
    {
        ragdoll = GetComponent<ActiveRagdoll>();    
        move = GetComponent<PlayerMove>();
    }

    public void HitFeedback(SyncableObject attacker)
    {
        if(attacker != null)
        {
            Debug.Log("hit");
            Vector3 dir = (ragdoll.hip.transform.position - attacker.GetComponent<ActiveRagdoll>().hip.transform.position).normalized;

            PoolableMono hitEffect = PoolManager.Instance.Pop("HitEffect",
                hitEffectPlayPos.position + -dir * hitEffectPlayOffset);

            ragdoll.AddForceToSpine(dir * hitFeedbackPower);
        }
    }

    public void DieFeedback(SyncableObject attacker)
    {
        SetActiveRagdoll(false);
        Debug.Log("die");

        if(attacker != null)
        {
            Transform attackerHip = attacker.GetComponent<ActiveRagdoll>().hip.transform;

            ragdoll.hip.transform.LookAt(attackerHip.position);
        }
    }

    public void SetActiveRagdoll(bool value, float time = -1)
    {
        SetActiveRagdollEvent?.Invoke(value);

        if(time > 0)
        {
            StartCoroutine(SetActiveRagdollCo(value, time));
        }
    }

    private IEnumerator SetActiveRagdollCo(bool value, float time)
    {
        yield return new WaitForSeconds(time);

        SetActiveRagdoll(value);
    }
}
