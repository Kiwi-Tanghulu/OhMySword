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
        if(!Moveable)
            return;

        ragdoll.SetVelocity(moveDir * moveSpeed);

        onMovedEvent?.Invoke(hip.transform.position);
    }
    #endregion

    #region OTHER MOVE
    public void SetTargetPosition(Vector3 pos)
    {
        if (!Moveable)
        {
            ragdoll.SetVelocity(Vector3.zero);
            return;
        }

        prevTargetPos = targetPos;
        targetPos = pos;

        if (Vector3.Distance(targetPos, hip.transform.position) < 0.1f)
            ragdoll.SetVelocity(Vector3.zero);
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
}
