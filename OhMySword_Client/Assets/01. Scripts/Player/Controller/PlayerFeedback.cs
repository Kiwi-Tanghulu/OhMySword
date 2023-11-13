using Base.Network;
using UnityEngine;

public class PlayerFeedback : MonoBehaviour
{
    private ActiveRagdoll ragdoll;
    private PlayerMove move;

    public float hitFeedbackPower = 100f;
    [SerializeField] private ParticleSystem hitEffect;
    [SerializeField] private float hitEffectPlayOffset;
    [SerializeField] private Transform hitEffectPlayPos;
    private SyncableObject lastAttacker;

    private void Start()
    {
        ragdoll = GetComponent<ActiveRagdoll>();    
        move = GetComponent<PlayerMove>();
    }

    public void HitFeedback(SyncableObject attacker)
    {
        lastAttacker = attacker;
        Vector3 dir = (ragdoll.hip.transform.position - attacker.GetComponent<ActiveRagdoll>().hip.transform.position).normalized;
        Debug.Log("Hit");
        hitEffect.transform.position = hitEffectPlayPos.position + -dir * hitEffectPlayOffset;
        hitEffect.Play();

        ragdoll.AddForceToSpine(dir * hitFeedbackPower);
    }

    public void DieFeedback()
    {
        Debug.Log("Die");
    }
}
