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
    private PlayerMove playerMove;
    private PlayerInput playerInput;
    private BoxCollider hitboxCollider;

    [Header("hit")]
    public float HitFeedbackPower = 15f;
    [SerializeField] private float hitEffectPlayOffset;
    [SerializeField] private Transform hitEffectPlayPos;

    [Space]
    public UnityEvent<SyncableObject> OnDieEvent;

    private void Start()
    {
        ragdoll = GetComponent<ActiveRagdoll>();    
        playerMove = GetComponent<PlayerMove>();
        TryGetComponent<PlayerInput>(out playerInput);
        hitboxCollider = transform.Find("Hips/Hitbox").GetComponent<BoxCollider>();
    }

    public void HitFeedback(SyncableObject attacker)
    {
        if(attacker != null)
        {
            Debug.Log("hit");
            Vector3 dir = (ragdoll.Hip.transform.position - attacker.GetComponent<ActiveRagdoll>().Hip.transform.position).normalized;

            PoolableMono hitEffect = PoolManager.Instance.Pop("HitEffect",
                hitEffectPlayPos.position + -dir * hitEffectPlayOffset);

            ragdoll.AddForceToSpine(dir * HitFeedbackPower);
        }
    }

    public void DieFeedback(SyncableObject attacker)
    {
        OnDie();
        OnDieEvent?.Invoke(attacker);
        Debug.Log("die");
    }

    //public void SetActiveRagdoll(bool value, float time = -1)
    //{
    //    OnDieEvent?.Invoke(value);

    //    if(time > 0)
    //    {
    //        StartCoroutine(SetActiveRagdollCo(value, time));
    //    }
    //}

    //private IEnumerator SetActiveRagdollCo(bool value, float time)
    //{
    //    yield return new WaitForSeconds(time);

    //    SetActiveRagdoll(value);
    //}

    private void OnDie()
    {
        ragdoll.SetConrol(false);
        playerMove.enabled = false;
        if (playerInput != null) playerInput.enabled = false;
        hitboxCollider.enabled = false;
    }
}
