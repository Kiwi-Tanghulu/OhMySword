using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMove : MonoBehaviour
{
    public Rigidbody hip;
    public float moveSpeed = 5f;
    public Vector3 moveDir;

    [Space]
    public LayerMask groundLayer;
    public Transform leftFootTarget;
    public Transform rightFootTarget;
    public Transform hips;
    public float shouldMoveDistance = 0.35f;
    [Tooltip("shouldMoveDistance보다 작아야 함")]
    public float moveDistance = 0.3f;
    public float movePivotHeight = 1f;
    public float footMoveTime = 0.2f;

    //where feet should be
    private Vector3 leftFootPos;
    private Vector3 rightFootPos;
    private Vector3 prevLeftFootPos;
    private Vector3 prevRightFootPos;

    //The location where the ray was fired from the body and reached the ground.
    private Vector3 leftHitPos;
    private Vector3 rightHitPos;

    //Distance between body and feet, Only used x and y
    private Vector3 leftFootOffset;
    private Vector3 rightFootOffset;

    private void Start()
    {
        leftFootPos = leftFootTarget.position;
        rightFootPos = rightFootTarget.position;
        prevLeftFootPos = leftFootPos;
        prevRightFootPos = rightFootPos;
        leftFootOffset = leftFootTarget.position - hips.position;
        rightFootOffset = rightFootTarget.position - hips.position;
    }

    private void Update()
    {
        Move();
        FootMove();
    }

    private void Move()
    {
        moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        hip.constraints = moveDir.x == 0 ? RigidbodyConstraints.FreezePositionX : RigidbodyConstraints.None;
        hip.constraints = moveDir.z == 0 ? RigidbodyConstraints.FreezePositionZ : RigidbodyConstraints.None;

        hip.AddForce(moveDir * moveSpeed); //+= moveDir * moveSpeed * Time.deltaTime;
    }

    private void FootMove()
    {
        leftFootTarget.position = leftFootPos;
        rightFootTarget.position = rightFootPos;

        RaycastHit lefttHit = default;
        RaycastHit righttHit = default;

        if (Physics.Raycast(new Vector3(hips.position.x + leftFootOffset.x, hips.position.y, hips.position.z + leftFootOffset.z),
            Vector3.down, out lefttHit, 10, groundLayer))
        {                                 //foot toe distance
                leftHitPos = lefttHit.point + Vector3.up * 0.2f;
            
            //left, right distance
            if (Vector3.Distance(leftHitPos, leftFootPos) >= shouldMoveDistance * 1.7f)
            {
                Vector3 footMoveDir = (leftHitPos - leftFootPos).normalized;
                prevLeftFootPos = leftFootPos;
                leftFootPos = new Vector3(leftHitPos.x + footMoveDir.x * moveDistance, leftHitPos.y, leftHitPos.z + footMoveDir.z * moveDistance);
                StartCoroutine(FootMoveAnimation(leftFootTarget, prevLeftFootPos, leftFootPos));
            }
        }

        if (Physics.Raycast(new Vector3(hips.position.x + rightFootOffset.x, hips.position.y, hips.position.z + rightFootOffset.z),
            Vector3.down, out righttHit, 10, groundLayer))
        {                                   //foot toe distance
                rightHitPos = righttHit.point + Vector3.up * 0.2f;

            if (Vector3.Distance(rightHitPos, rightFootPos) >= shouldMoveDistance)
            {
                Vector3 footMoveDir = (rightHitPos - rightFootPos).normalized;
                prevRightFootPos = rightFootPos;
                rightFootPos = new Vector3(rightHitPos.x + footMoveDir.x * moveDistance, rightHitPos.y, rightHitPos.z + footMoveDir.z * moveDistance);
                StartCoroutine(FootMoveAnimation(rightFootTarget, prevRightFootPos, rightFootPos));
            }
        }
    }

    private IEnumerator FootMoveAnimation(Transform foot, Vector3 start, Vector3 end)
    {
        float percent = 0;
        Vector3 heightPivotVector = new Vector3((end - start).x / 2f + start.x, start.y + movePivotHeight, (end - start).z / 2f + start.z);
        Vector3 startToPivotLerp;
        Vector3 pivotToEndLerp;

        while (percent <= 1)
        {
            startToPivotLerp = Vector3.Lerp(start, heightPivotVector, percent);
            pivotToEndLerp = Vector3.Lerp(heightPivotVector, end, percent);
            foot.position = Vector3.Slerp(startToPivotLerp, pivotToEndLerp, percent);

            percent += Time.deltaTime / footMoveTime;

            yield return null;
        }
    }
}