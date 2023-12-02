using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMove : MonoBehaviour
{
    [field: SerializeField]
    public bool Moveable { get; set; } = true;

    private ActiveRagdoll ragdoll;

    [SerializeField] UnityEvent<Vector3> onMovedEvent;

    [SerializeField] private Rigidbody hip;
    [SerializeField] private Transform hipAnchor;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpPower = 10f;
    
    private Vector3 prevTargetPos;
    private Vector3 targetPos;
    private Vector3 moveDir;
    private Vector3 verticalVelocity;
    private Vector3 prevMoveDir;
    private Vector3 velocity;
    private float moveDistance;

    private void Start()
    {
        ragdoll = GetComponent<ActiveRagdoll>();
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
        if (!Moveable)
        {
            Stop();
        }
        else
        {
            ragdoll.SetVelocity(moveDir * moveSpeed);
        }


        onMovedEvent?.Invoke(hip.transform.position);
    }

    public void Jump()
    {
        ragdoll.SetConrol(false);
        ragdoll.hip.AddForce(Vector3.up *  jumpPower, ForceMode.Impulse);
    }
    #endregion

    #region OTHER MOVE
    public void SetTargetPosition(Vector3 pos)
    {
        if (!Moveable)
        {
            Stop();
            return;
        }

        prevTargetPos = targetPos;
        targetPos = pos;

        if (Vector3.Distance(targetPos, hip.transform.position) < 0.2f)
            Stop();
        else
        {
            if (Vector3.Distance(prevTargetPos, hip.transform.position) > 0.5f)
                hip.transform.position = prevTargetPos;

            moveDir = targetPos - prevTargetPos;
            moveDistance = moveDir.magnitude;
            moveDir.Normalize();

            SetVelocity();
        }
    }

    private void SetVelocity()
    {
        moveSpeed = moveDistance / 0.1f;
        ragdoll.SetVelocity(moveDir * moveSpeed);

        //StartCoroutine(AdjustPosition());
    }

    private IEnumerator AdjustPosition()
    {
        yield return new WaitForSeconds(0.09f);

        if (Vector3.Distance(targetPos, hip.transform.position) > 0.5f)
            hip.transform.position = targetPos;
    }
    #endregion

    public void Stop()
    {
        Debug.Log("sd");
        ragdoll.SetHipConstraint(true);
        ragdoll.SetVelocity(Vector3.zero);
    }
}
