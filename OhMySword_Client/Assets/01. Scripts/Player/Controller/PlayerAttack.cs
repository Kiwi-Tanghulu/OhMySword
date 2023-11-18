using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAttack : MonoBehaviour
{
    [field: SerializeField]
    public bool canAttack { get; set; } = true;

    public UnityEvent OnAttackEvent;

    private ActiveRagdoll ragdoll;

    [SerializeField] private float attackTime;
    [SerializeField] private float recoveryTime;

    [SerializeField] private Transform nonattackTrm;
    [SerializeField] private Transform attackTrm;

    [SerializeField] private bool isAttack;

    [SerializeField] private PlayerWeapon weapon;

    private AudioSource audioPlayer;

    private void Start()
    {
        ragdoll = GetComponent<ActiveRagdoll>();
        audioPlayer = GetComponent<AudioSource>();
    }

    public void Attack()
    {
        if (isAttack || !ragdoll.isGround || !canAttack)
            return;

        weapon.SetCollision(true);
        weapon.SetTrail(true);
        OnAttackEvent?.Invoke();
        StartCoroutine(AttackCo());
    }

    private IEnumerator AttackCo()
    {
        isAttack = true;
        AudioManager.Instance.PlayAudio("SwordSwing", audioPlayer, true);
        yield return StartCoroutine(ragdoll.SetRightArmRigTarget(attackTrm, attackTime));
        weapon.SetTrail(false);
        weapon.SetCollision(false);
        yield return StartCoroutine(ragdoll.SetRightArmRigTarget(nonattackTrm, recoveryTime));
        isAttack = false;
    }
}
