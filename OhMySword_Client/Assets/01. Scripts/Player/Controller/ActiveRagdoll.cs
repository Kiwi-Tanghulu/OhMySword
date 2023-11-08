using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class ActiveRagdoll : MonoBehaviour
{
    [field: SerializeField]
    public bool bodyLerping { get; set; } = true;
    [field: SerializeField]
    public bool armLerping { get; set; } = true;
    [field: SerializeField]
    public bool footMove { get; set; } = true;

    [Space]
    public Rigidbody hip;

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
    public float hipElasticitySpeed = 6f;
    public float footToeOffset = 0.1f;
    public float hipHeight = 0.55f;

    //where feet should be
    private Vector3 leftFootTargetPos;
    private Vector3 rightFootTargetPos;
    private Vector3 prevLeftFootTargetPos;
    private Vector3 prevRightFootTargetPos;
    private Vector3 hipToGroundPos;

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
        FootMove();
        SetBodyAncherPos();
    }

    public void Move(Vector3 velocity)
    {
        if (velocity == Vector3.zero)
        {
            hip.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
            HipElasticity();
        }
        else if (velocity.x == 0)
            hip.constraints = RigidbodyConstraints.FreezePositionX;
        else if (velocity.z == 0)
            hip.constraints = RigidbodyConstraints.FreezePositionZ;
        else
            hip.constraints = RigidbodyConstraints.None;
        hip.constraints |= RigidbodyConstraints.FreezePositionY;

        hip.velocity = velocity;
    }

    private void FootMove()
    {
        if (!footMove)
            return;

        leftFootTarget.position = leftFootTargetPos;
        rightFootTarget.position = rightFootTargetPos;

        if(Physics.Raycast(hip.position, Vector3.down, out RaycastHit hipToGround, 1, groundLayer))
        {
            hipToGroundPos = hipToGround.point;

            if (Vector3.Distance(hipToGroundPos, footMiddlePos) >= shouldMoveDistance)
            {
                Vector3 moveDir = (hipToGroundPos - footMiddlePos).normalized;
                
                if(Vector3.Distance(hipToGroundPos, leftFootTargetPos) > Vector3.Distance(hipToGroundPos, rightFootTargetPos))
                {
                    RaycastHit lefttHit = default;
                    Physics.Raycast(new Vector3(hips.position.x + leftFootOffset.x + moveDir.x * moveDistance, 
                        hips.position.y, hips.position.z + leftFootOffset.z + moveDir.z * moveDistance), Vector3.down, out lefttHit, 10, groundLayer);

                    leftHitPos = lefttHit.point + Vector3.up * footToeOffset;
                    prevLeftFootTargetPos = leftFootTargetPos;
                    leftFootTargetPos = leftHitPos;

                    StartCoroutine(FootMoveAnimation(leftFootTarget, prevLeftFootTargetPos, leftFootTargetPos));
                }
                else
                {
                    RaycastHit righttHit = default;
                    Physics.Raycast(new Vector3(hips.position.x + rightFootOffset.x + moveDir.x * moveDistance, hips.position.y, 
                        hips.position.z + rightFootOffset.z + moveDir.z * moveDistance), Vector3.down, out righttHit, 10, groundLayer);

                    rightHitPos = righttHit.point + Vector3.up * footToeOffset;
                    prevRightFootTargetPos = rightFootTargetPos;
                    rightFootTargetPos = rightHitPos;

                    StartCoroutine(FootMoveAnimation(rightFootTarget, prevRightFootTargetPos, rightFootTargetPos));
                }

                footMiddlePos = (leftFootTargetPos - rightFootTargetPos) / 2f + rightFootTargetPos;
            }
        }
    }

    private void SetBodyAncherPos()
    {
        hipAnchor.position = hipToGroundPos + Vector3.up * hipHeight;
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
            new Vector3(footMiddlePos.x, hipAnchor.position.y, footMiddlePos.z), hipElasticitySpeed * Time.deltaTime);
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