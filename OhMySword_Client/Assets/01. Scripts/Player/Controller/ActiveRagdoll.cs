using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class ActiveRagdoll : MonoBehaviour
{
    public bool bodyLerping = true;
    public bool canMove = true;
    public bool canFootMove = true;

    [Space]
    public Rigidbody hip;
    public float moveSpeed = 5f;
    public Vector3 moveDir;

    [Space]
    public Transform leftFootTarget;
    public Transform rightFootTarget;
    public Transform hips;
    public Transform hipAnchor;
    [Space]
    public LayerMask groundLayer;
    public float shouldMoveDistance = 0.35f;
    [Tooltip("shouldMoveDistance보다 작아야 함")]
    public float moveDistance = 0.3f;
    public float movePivotHeight = 1f;
    public float footMoveTime = 0.2f;
    public float elasticitySpeed = 1f;

    //where feet should be
    private Vector3 leftFootTargetPos;
    private Vector3 rightFootTargetPos;
    private Vector3 prevLeftFootTargetPos;
    private Vector3 prevRightFootTargetPos;

    //The location where the ray was fired from the body and reached the ground.
    private Vector3 leftHitPos;
    private Vector3 rightHitPos;

    //Distance between body and feet, Only used x and z
    private Vector3 leftFootOffset;
    private Vector3 rightFootOffset;

    //between pos of foot
    private Vector3 footMiddlePos;

    private void Start()
    {
        leftFootTargetPos = leftFootTarget.position;
        rightFootTargetPos = rightFootTarget.position;
        prevLeftFootTargetPos = leftFootTargetPos;
        prevRightFootTargetPos = rightFootTargetPos;
        leftFootOffset = leftFootTarget.position - hips.position;
        rightFootOffset = rightFootTarget.position - hips.position;
        footMiddlePos = (leftFootTargetPos - rightFootTargetPos) / 2f + rightFootTargetPos;
    }

    private void Update()
    {
        Move();
        FootMove();
    }

    private void Move()
    {
        if (!canMove)
            return; 

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

        hip.velocity = moveDir * moveSpeed;
    }

    private void FootMove()
    {
        if (!canFootMove)
            return;

        leftFootTarget.position = leftFootTargetPos;
        rightFootTarget.position = rightFootTargetPos;

        if(Physics.Raycast(hip.position, Vector3.down, out RaycastHit hipToGround, 1, groundLayer))
        {
            if(Vector3.Distance(hipToGround.point, footMiddlePos) >= shouldMoveDistance)
            {
                Vector3 moveDir = (hipToGround.point - footMiddlePos).normalized;
                
                if(Vector3.Distance(hipToGround.point, leftFootTargetPos) > Vector3.Distance(hipToGround.point, rightFootTargetPos))
                {
                    RaycastHit lefttHit = default;
                    Physics.Raycast(new Vector3(hips.position.x + leftFootOffset.x + moveDir.x * moveDistance, 
                        hips.position.y, hips.position.z + leftFootOffset.z + moveDir.z * moveDistance), Vector3.down, out lefttHit, 10, groundLayer);

                    leftHitPos = lefttHit.point + Vector3.up * 0.2f;
                    prevLeftFootTargetPos = leftFootTargetPos;
                    leftFootTargetPos = leftHitPos;

                    StartCoroutine(FootMoveAnimation(leftFootTarget, prevLeftFootTargetPos, leftFootTargetPos));
                }
                else
                {
                    RaycastHit righttHit = default;
                    Physics.Raycast(new Vector3(hips.position.x + rightFootOffset.x + moveDir.x * moveDistance, hips.position.y, 
                        hips.position.z + rightFootOffset.z + moveDir.z * moveDistance), Vector3.down, out righttHit, 10, groundLayer);

                    rightHitPos = righttHit.point + Vector3.up * 0.2f;
                    prevRightFootTargetPos = rightFootTargetPos;
                    rightFootTargetPos = rightHitPos;

                    StartCoroutine(FootMoveAnimation(rightFootTarget, prevRightFootTargetPos, rightFootTargetPos));
                }

                footMiddlePos = (leftFootTargetPos - rightFootTargetPos) / 2f + rightFootTargetPos;
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
            foot.position = Vector3.Lerp(startToPivotLerp, pivotToEndLerp, percent);

            percent += Time.deltaTime / footMoveTime;

            yield return null;
        }
    }

    // body lerping
    private void HipElasticity()
    {
        if (!bodyLerping)
            return;
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
}