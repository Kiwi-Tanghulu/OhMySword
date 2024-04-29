using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAttack : MonoBehaviour
{
    [field: SerializeField]
    public bool CanAttack { get; set; } = true;

    public UnityEvent OnAttackEvent;

    private ActiveRagdoll ragdoll;

    [SerializeField] private float defaultAttackDelay = 1f;
    [SerializeField] private float attackDelay = 0f;
    [SerializeField] private float attackTime;
    [SerializeField] private float attackReadyTime;
    [SerializeField] private float recoveryTime;

    [SerializeField] private Transform nonattackTrm;
    [SerializeField] private Transform attackReadyTrm;
    [SerializeField] private Transform attackTrm;

    [SerializeField] private bool isAttack;

    [SerializeField] private PlayerWeapon weapon;

    private void Start()
    {
        ragdoll = GetComponent<ActiveRagdoll>();
        attackDelay = defaultAttackDelay;
    }

    public void Attack()
    {
        if (isAttack || !ragdoll.IsGround || !CanAttack)
            return;

        OnAttackEvent?.Invoke();
        StartCoroutine(AttackCo());
    }

    private IEnumerator AttackCo()
    {
        isAttack = true;
        yield return StartCoroutine(ragdoll.SetRightArmRigTarget(attackReadyTrm, attackReadyTime));
        weapon.SetCollision(true);
        weapon.SetTrail(true);
        AudioManager.Instance.PlayAudio("SwordSwing", ragdoll.aud, true);
        yield return StartCoroutine(ragdoll.SetRightArmRigTarget(attackTrm, attackTime));
        weapon.SetTrail(false);
        weapon.SetCollision(false);
        yield return StartCoroutine(ragdoll.SetRightArmRigTarget(nonattackTrm, recoveryTime));
        yield return new WaitForSeconds(attackDelay);
        isAttack = false;
        weapon.attacked.Clear();
    }

    public void SetAttackDelay(float value)
    {
        attackDelay = value + defaultAttackDelay;
    }
}
