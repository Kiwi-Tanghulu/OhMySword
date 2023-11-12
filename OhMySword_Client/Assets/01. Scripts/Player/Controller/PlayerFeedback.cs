using Base.Network;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFeedback : MonoBehaviour
{
    private ActiveRagdoll ragdoll;
    private PlayerMove move;

    public float hitFeedbackPower = 100f;

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
        ragdoll.AddForceToSpine(dir * hitFeedbackPower);
    }

    public void DieFeedback()
    {
        Debug.Log("Die");
    }
}
