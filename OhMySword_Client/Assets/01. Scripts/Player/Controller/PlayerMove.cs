using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMove : MonoBehaviour
{
    private ActiveRagdoll ragdoll;

    [SerializeField] UnityEvent<Vector3> onMovedEvent;

    [SerializeField] private Rigidbody hip;
    [SerializeField] private Transform hipAnchor;

    [SerializeField] private float moveSpeed;
    [SerializeField] private bool canMove = true;
    private Vector3 prevTargetPos;
    private Vector3 targetPos;
    private Vector3 moveDir;
    private Vector3 prevMoveDir;
    private Vector3 velocity;
    private float moveDistance;

    private PlayerView cam;

    private void Start()
    {
        ragdoll = GetComponent<ActiveRagdoll>();
        cam = GetComponent<PlayerView>();
        targetPos = transform.position;
        prevTargetPos = targetPos;
    }

    #region MYMOVE
    public void SetMoveDirection(Vector3 input)
    {
        moveDir = hipAnchor.rotation * input.normalized;
    }

    public void Move()
    {
        if ((targetPos - hip.transform.position).magnitude > 0.5f)
            ragdoll.SetVelocity(moveDir * moveSpeed);
        else
            ragdoll.SetVelocity(Vector3.zero);

        onMovedEvent?.Invoke(hip.transform.position);
    }
    #endregion

    #region OTHER MOVE
    public void SetTargetPosition(Vector3 pos)
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
