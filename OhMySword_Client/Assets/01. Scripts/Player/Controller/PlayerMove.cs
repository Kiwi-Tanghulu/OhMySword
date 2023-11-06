using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private ActiveRagdoll ragdoll;

    public bool canMove = true;

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
        Move();   
    }

    private void Move()
    {
        if(canMove)
            ragdoll.Move(velocity);
        else
            ragdoll.Move(Vector3.zero);
    }

    public void SetTargetPosition(Vector3 pos)
    {
        prevTargetPos = targetPos;
        transform.position = prevTargetPos;
        targetPos = pos;
        moveDir = (targetPos - prevTargetPos).normalized;
        velocity = moveDir * moveSpeed;
    }

    public void Stun(float time)
    {
        StartCoroutine(StunCo(time));
    }

    private IEnumerator StunCo(float time)
    {
        canMove = false;

        yield return new WaitForSeconds(time);

        canMove = true;
    }
}
