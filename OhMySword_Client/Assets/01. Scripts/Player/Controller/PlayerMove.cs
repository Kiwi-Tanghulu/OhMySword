using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMove : MonoBehaviour
{
    private ActiveRagdoll ragdoll;

    [SerializeField] UnityEvent<Vector3> onMovedEvent;

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
        if((targetPos - hip.transform.position).magnitude > 0.5f)
            ragdoll.SetVelocity(moveDir * moveSpeed);
        else
            ragdoll.SetVelocity(Vector3.zero);
        onMovedEvent?.Invoke(hip.transform.position);
    }

    //�÷��̾��
    public void SetMoveDirection(Vector3 input)
    {
        prevMoveDir = moveDir;
        moveDir = cam.forward * input.z + cam.right * input.x;
    }

    //Ŭ���
    public void SetTargetPosition(Vector3 pos)
    {
        hip.transform.position = prevTargetPos;

        prevTargetPos = targetPos;
        targetPos = pos;
        moveDir = (targetPos - hip.transform.position).normalized;
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
