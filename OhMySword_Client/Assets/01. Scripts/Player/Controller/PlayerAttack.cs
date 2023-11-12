using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private ActiveRagdoll ragdoll;

    [SerializeField] private float attackTime;
    [SerializeField] private float recoveryTime;

    [SerializeField] private Transform nonattackTrm;
    [SerializeField] private Transform attackTrm;

    [SerializeField] private bool isAttack;

    [SerializeField] private PlayerWeapon weapon;

    private void Start()
    {
        ragdoll = GetComponent<ActiveRagdoll>();
    }

    public void Attack()
    {
        if (isAttack || !ragdoll.isGround)
            return;

        weapon.SetCollision(true);
        weapon.SetTrail(true);
        StartCoroutine(AttackCo());
    }

    private IEnumerator AttackCo()
    {
        isAttack = true;
        yield return StartCoroutine(ragdoll.SetRightArmRigTarget(attackTrm, attackTime));
        weapon.SetCollision(false);
        weapon.SetTrail(false);
        yield return StartCoroutine(ragdoll.SetRightArmRigTarget(nonattackTrm, recoveryTime));
        isAttack = false;
        weapon.ResetAttackedList();
    }
}
