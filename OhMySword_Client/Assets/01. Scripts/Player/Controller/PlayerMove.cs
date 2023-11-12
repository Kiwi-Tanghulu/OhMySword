using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMove : MonoBehaviour
{
    private ActiveRagdoll ragdoll;

    public bool canMove = true;

    public UnityEvent<Vector3> positionSync;

    private Vector3 prevTargetPos;
    private Vector3 targetPos;
    private Vector3 moveDir;
    private Vector3 velocity;
    [SerializeField] private float moveSpeed;
    [SerializeField] Rigidbody hip;

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

        positionSync.Invoke(hip.transform.position);
    }

    #region other client
    public void SyncMove(Vector3 pos)
    {
        if (Vector3.Distance(pos, hip.transform.position) < 0.1f)
            return;

        prevTargetPos = targetPos;
        targetPos = pos;
        moveDir = targetPos - hip.transform.position;
        moveDistance = moveDir.magnitude;
        moveDir.Normalize();

        //if (Vector3.Distance(pos, hip.transform.position) < 0.1f)
        //    return;
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


        if (Vector3.Distance(targetPos, hip.transform.position) > 0.5f)
            hip.transform.position = targetPos;
    }
    #endregion
}
