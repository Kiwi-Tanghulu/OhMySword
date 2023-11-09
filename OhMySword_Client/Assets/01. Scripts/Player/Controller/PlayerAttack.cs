using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private ActiveRagdoll ragdoll;

    [SerializeField] private float attackTime = 0.3f;
    [SerializeField] private bool isAttack = false;

    private void Start()
    {
        ragdoll = GetComponent<ActiveRagdoll>();
    }

    public void Attack()
    {
        if (isAttack)
            return;

        StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        isAttack = true;
        yield return StartCoroutine(ragdoll.AttackMotion(attackTime));
        yield return StartCoroutine(ragdoll.AttacRecoverykMotion(attackTime));
        isAttack = false;
    }
}
