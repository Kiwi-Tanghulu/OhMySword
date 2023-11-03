using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public Transform hipAnchor;
    public float shouldMoveDistance = 0.35f;
    [Tooltip("shouldMoveDistance보다 작아야 함")]
    public float moveDistance = 0.3f;
    public float movePivotHeight = 1f;
    public float footMoveTime = 0.2f;
    public float elasticitySpeed = 1f;

    //where feet should be
    private Vector3 leftFootPos;
    private Vector3 rightFootPos;
    private Vector3 prevLeftFootPos;
    private Vector3 prevRightFootPos;

    //The location where the ray was fired from the body and reached the ground.
    private Vector3 leftHitPos;
    private Vector3 rightHitPos;

    //Distance between body and feet, Only used x and z
    private Vector3 leftFootOffset;
    private Vector3 rightFootOffset;

    private Vector3 footMiddlePos;

    private void Start()
    {
        leftFootPos = leftFootTarget.position;
        rightFootPos = rightFootTarget.position;
        prevLeftFootPos = leftFootPos;
        prevRightFootPos = rightFootPos;
        leftFootOffset = leftFootTarget.position - hips.position;
        rightFootOffset = rightFootTarget.position - hips.position;
        footMiddlePos = (leftFootPos - rightFootPos) / 2f + rightFootPos;
    }

    private void Update()
    {
        Move();
        FootMove();
    }

    private void Move()
    {
        moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        if (moveDir == Vector3.zero)
        {
            hip.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
            HipElasticity();
        }
        else if (moveDir.x == 0)
            hip.constraints = RigidbodyConstraints.FreezePositionX;
        else if (moveDir.z == 0)
            hip.constraints = RigidbodyConstraints.FreezePositionZ;
        else
            hip.constraints = RigidbodyConstraints.None;
        hip.constraints |= RigidbodyConstraints.FreezePositionY;

        //hip.AddForce(moveDir * moveSpeed);
        //hip.velocity = new Vector3(hip.velocity.x, 0, hip.velocity.z);

        hip.velocity = moveDir * moveSpeed;
    }

    private void FootMove()
    {
        leftFootTarget.position = leftFootPos;
        rightFootTarget.position = rightFootPos;

        if(Physics.Raycast(hip.position, Vector3.down, out RaycastHit hipToGround, 1, groundLayer))
        {
            if(Vector3.Distance(hipToGround.point, footMiddlePos) >= shouldMoveDistance)
            {
                Vector3 moveDir = (hipToGround.point - footMiddlePos).normalized;
                
                if(Vector3.Distance(hipToGround.point, leftFootPos) > Vector3.Distance(hipToGround.point, rightFootPos))
                {
                    RaycastHit lefttHit = default;
                    Physics.Raycast(new Vector3(hips.position.x + leftFootOffset.x + moveDir.x * moveDistance, 
                        hips.position.y, hips.position.z + leftFootOffset.z + moveDir.z * moveDistance), Vector3.down, out lefttHit, 10, groundLayer);

                    leftHitPos = lefttHit.point + Vector3.up * 0.2f;
                    prevLeftFootPos = leftFootPos;
                    leftFootPos = leftHitPos;
                    //leftFootPos = new Vector3(hip.position.x + leftFootOffset.x + moveDir.x * moveDistance, leftFootPos.y,
                    //    hip.position.z + leftFootOffset.z + moveDir.z * moveDistance);
                    StartCoroutine(FootMoveAnimation(leftFootTarget, prevLeftFootPos, leftFootPos));
                }
                else
                {
                    RaycastHit righttHit = default;
                    Physics.Raycast(new Vector3(hips.position.x + rightFootOffset.x + moveDir.x * moveDistance, hips.position.y, 
                        hips.position.z + rightFootOffset.z + moveDir.z * moveDistance), Vector3.down, out righttHit, 10, groundLayer);

                    rightHitPos = righttHit.point + Vector3.up * 0.2f;
                    prevRightFootPos = rightFootPos;
                    rightFootPos = rightHitPos;
                    //rightFootPos = new Vector3(hip.position.x + rightFootOffset.x + moveDir.x * moveDistance, rightFootPos.y,
                    //    hip.position.z + rightFootOffset.z + moveDir.z * moveDistance);
                    StartCoroutine(FootMoveAnimation(rightFootTarget, prevRightFootPos, rightFootPos));
                }

                footMiddlePos = (leftFootPos - rightFootPos) / 2f + rightFootPos;
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

    private void HipElasticity()
    {
        Vector3 dir = ((footMiddlePos + Vector3.up * hip.position.y) - hip.position).normalized;
        //Debug.Log(123);
        Debug.Log(Vector3.Lerp(hip.position, footMiddlePos, elasticitySpeed * Time.deltaTime));
        hip.transform.position = Vector3.Lerp(hip.transform.position, 
            new Vector3(footMiddlePos.x, hipAnchor.position.y, footMiddlePos.z), elasticitySpeed * Time.deltaTime);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(footMiddlePos, shouldMoveDistance);
        Gizmos.DrawSphere(footMiddlePos, 0.1f);
        Gizmos.DrawSphere(leftHitPos, 0.1f);
        Gizmos.DrawSphere(rightHitPos, 0.1f);
    }
#endif

    #region dispose
    //private void FootMove()
    //{
    //    leftFootTarget.position = leftFootPos;
    //    rightFootTarget.position = rightFootPos;

    //    RaycastHit lefttHit = default;
    //    RaycastHit righttHit = default;

    //    if (Physics.Raycast(new Vector3(hips.position.x + leftFootOffset.x, hips.position.y, hips.position.z + leftFootOffset.z),
    //        Vector3.down, out lefttHit, 10, groundLayer))
    //    {                                 //foot toe distance
    //        leftHitPos = lefttHit.point + Vector3.up * 0.2f;

    //        //left, right distance
    //        if (Vector3.Distance(leftHitPos, leftFootPos) >= shouldMoveDistance * 1.7f)
    //        {
    //            Vector3 footMoveDir = (leftHitPos - leftFootPos).normalized;
    //            prevLeftFootPos = leftFootPos;
    //            leftFootPos = new Vector3(leftHitPos.x + footMoveDir.x * moveDistance, leftHitPos.y, leftHitPos.z + footMoveDir.z * moveDistance);
    //            StartCoroutine(FootMoveAnimation(leftFootTarget, prevLeftFootPos, leftFootPos));
    //        }
    //    }

    //    if (Physics.Raycast(new Vector3(hips.position.x + rightFootOffset.x, hips.position.y, hips.position.z + rightFootOffset.z),
    //        Vector3.down, out righttHit, 10, groundLayer))
    //    {                                   //foot toe distance
    //        rightHitPos = righttHit.point + Vector3.up * 0.2f;

    //        if (Vector3.Distance(rightHitPos, rightFootPos) >= shouldMoveDistance)
    //        {
    //            Vector3 footMoveDir = (rightHitPos - rightFootPos).normalized;
    //            prevRightFootPos = rightFootPos;
    //            rightFootPos = new Vector3(rightHitPos.x + footMoveDir.x * moveDistance, rightHitPos.y, rightHitPos.z + footMoveDir.z * moveDistance);
    //            StartCoroutine(FootMoveAnimation(rightFootTarget, prevRightFootPos, rightFootPos));
    //        }
    //    }
    //}
    #endregion
}