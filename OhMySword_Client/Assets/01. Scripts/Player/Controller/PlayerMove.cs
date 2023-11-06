using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private ActiveRagdoll ragdoll;

    private Vector3 prevTargetPos;
    private Vector3 targetPos;
    private Vector3 moveDir;
    private Vector3 velocity;
    [SerializeField] private float moveSpeed;

    private void Start()
    {
        ragdoll = GetComponent<ActiveRagdoll>();
        targetPos = transform.position;
        prevTargetPos = targetPos;
    }

    private void Update()
    {
        ragdoll.Move(velocity);
    }

    public void SetTargetPosition(Vector3 pos)
    {
        prevTargetPos = targetPos;
        transform.position = prevTargetPos;
        targetPos = pos;
        moveDir = (targetPos - prevTargetPos).normalized;
        velocity = moveDir * moveSpeed;
    }
}
