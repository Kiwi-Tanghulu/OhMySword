using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private ActiveRagdoll ragdoll;

    [SerializeField] private float readyTime;
    [SerializeField] private float attackTime;

    [Space]
    //[SerializeField] private Sword sword;
    [SerializeField] private Transform swrodAnchor;

    [Space]
    [SerializeField] private float attackAngle;

    public void Attack()
    {

    }
}
