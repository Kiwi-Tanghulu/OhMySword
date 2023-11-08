using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private ActiveRagdoll ragdoll;

    [SerializeField] private Rigidbody hip;

    [SerializeField] private float moveSpeed;
    [SerializeField] private bool canMove = true;
    private Vector3 prevTargetPos;
    private Vector3 targetPos;
    private Vector3 moveDir;
    private Vector3 prevMoveDir;
    private Vector3 velocity;

    private PlayerView cam;

    private void Start()
    {
        ragdoll = GetComponent<ActiveRagdoll>();
        cam = GetComponent<PlayerView>();
        targetPos = transform.position;
        prevTargetPos = targetPos;
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        ragdoll.SetVelocity(moveDir * moveSpeed);
    }

    //플레이어용
    public void SetMoveDirection(Vector3 input)
    {
        prevMoveDir = moveDir;
        moveDir = cam.forward * input.z + cam.right * input.x;
    }

    //클라용
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
