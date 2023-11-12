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
    [SerializeField] Transform hip;

    //other client
    private float moveDistance;

    private void Start()
    {
        ragdoll = GetComponent<ActiveRagdoll>();
        targetPos = transform.position;
        prevTargetPos = targetPos;
    }

    public void Move(Vector3 input)
    {
        moveDir = input.normalized;

        if(canMove)
            ragdoll.SetVelocity(moveDir * moveSpeed);
        else
            ragdoll.SetVelocity(Vector3.zero);

    }

    #region other client
    public void SyncMove(Vector3 pos)
    {
        if (Vector3.Distance(pos, hip.position) < 0.1f)
            moveDir = Vector3.zero;
        else
        {
            prevTargetPos = targetPos;
            targetPos = pos;
            moveDir = targetPos - hip.position;
        }

        moveDistance = moveDir.magnitude;
        moveDir.Normalize();

        SetVelocity();
    }

    private void SetVelocity()
    {
        moveSpeed = moveDistance / 0.1f;
        ragdoll.SetVelocity(moveDir * moveSpeed);

        StartCoroutine(AdjustPosition());
    }

    private IEnumerator AdjustPosition()
    {
        yield return new WaitForSeconds(0.09f);


        if (Vector3.Distance(targetPos, hip.position) > 0.5f)
            hip.position = targetPos;
    }
    #endregion
}
