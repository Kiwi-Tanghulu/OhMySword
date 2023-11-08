using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public struct Foot
{
    public Transform foot;
    public Vector3 targetPos;
    public Vector3 prevTargetPos;
    //Distance between body and feet, Only used x and z
    public Vector3 offset;
}

public class ActiveRagdoll : MonoBehaviour
{
    [field: SerializeField]
    public bool bodyLerping { get; set; } = true;
    [field: SerializeField]
    public bool armLerping { get; set; } = true;
    [field: SerializeField]
    public bool footMove { get; set; } = true;
    [field: SerializeField]
    public bool footRotate { get; set; } = true;

    public bool isGround;

    [Space]
    public Rigidbody hip;

    [Space]
    public Transform leftFootTarget;
    public Transform rightFootTarget;
    public Transform hips;
    public Transform hipAnchor;
    private Transform beforeMoveFoot;
    [Space]
    public LayerMask groundLayer;
    public float shouldMoveDistance = 0.35f;
    [Tooltip("shouldMoveDistance보다 작아야 함")]
    public float moveDistance = 0.3f;
    public float movePivotHeight = 1f;
    public float footMoveTime = 0.2f;
    public float alignTime = 0.2f;
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

    private Vector3 prevHipVelocity;

    private void Start()
    {
        leftFootTargetPos = leftFootTarget.position;
        rightFootTargetPos = rightFootTarget.position;
        prevLeftFootTargetPos = leftFootTargetPos;
        prevRightFootTargetPos = rightFootTargetPos;
        leftFootOffset = leftFootTarget.position - hips.position;
        rightFootOffset = rightFootTarget.position - hips.position;
        leftFootOffset.y = 0f;
        rightFootOffset.y = 0f;
        footMiddlePos = (leftFootTargetPos - rightFootTargetPos) / 2f + rightFootTargetPos;
    }

    private void Update()
    {
        CheckGround();
        SetBodyAncherPos();
        FootMove();
        HipElasticity();

        prevHipVelocity = hip.velocity;
    }

    private void CheckGround()
    {
        isGround = Physics.Raycast(hip.position, Vector3.down, out RaycastHit hipToGround, hipHeight + 0.1f, groundLayer);
        
        if (isGround)
        {
            hipToGroundPos = hipToGround.point;
            hip.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | 
                RigidbodyConstraints.FreezePositionZ;
            SetBodyAncherPos();

        }
        else
        {
            hip.constraints = RigidbodyConstraints.None;
        }
    }

    private void SetFootTargetPos()
    {

    }

    private void FootMove()
    {
        if (!footMove && isGround)
            return;

        leftFootTarget.position = leftFootTargetPos;
        rightFootTarget.position = rightFootTargetPos;

        if (Vector3.Distance(hipToGroundPos, footMiddlePos) >= shouldMoveDistance)
        {
            Vector3 moveDir = (hipToGroundPos - footMiddlePos).normalized;

            if (Vector3.Distance(hipToGroundPos, leftFootTargetPos) > Vector3.Distance(hipToGroundPos, rightFootTargetPos))
            {
                RaycastHit lefttHit = default;
                Physics.Raycast(new Vector3(hips.position.x + moveDir.x * moveDistance, hips.position.y,
                    hips.position.z + moveDir.z * moveDistance) + hips.rotation * leftFootOffset,
                    Vector3.down, out lefttHit, 10, groundLayer);

                leftHitPos = lefttHit.point + Vector3.up * footToeOffset;
                prevLeftFootTargetPos = leftFootTargetPos;
                leftFootTargetPos = leftHitPos;
                beforeMoveFoot = leftFootTarget;

                StartCoroutine(FootMoveAnimation(leftFootTarget, prevLeftFootTargetPos, leftFootTargetPos,
                    footMoveTime));
            }
            else
            {
                RaycastHit righttHit = default;
                Physics.Raycast(new Vector3(hips.position.x + moveDir.x * moveDistance, hips.position.y,
                    hips.position.z + moveDir.z * moveDistance) + hips.rotation * rightFootOffset,
                    Vector3.down, out righttHit, 10, groundLayer);

                rightHitPos = righttHit.point + Vector3.up * footToeOffset;
                prevRightFootTargetPos = rightFootTargetPos;
                rightFootTargetPos = rightHitPos;
                beforeMoveFoot = rightFootTarget;

                StartCoroutine(FootMoveAnimation(rightFootTarget, prevRightFootTargetPos, rightFootTargetPos,
                    footMoveTime));
            }

            footMiddlePos = (leftFootTargetPos - rightFootTargetPos) / 2f + rightFootTargetPos;
        }
    }

    private void SetBodyAncherPos()
    {
        hipAnchor.position = hipToGroundPos + Vector3.up * hipHeight;
    }
    // body lerping
    private void HipElasticity()
    {
        if (!bodyLerping || hip.velocity != Vector3.zero)
            return;

        if (!isGround)
            return;

        hip.transform.position = Vector3.Lerp(hip.transform.position,
            new Vector3(footMiddlePos.x, hipAnchor.position.y, footMiddlePos.z), hipElasticitySpeed * Time.deltaTime);
    }

    private IEnumerator FootMoveAnimation(Transform foot, Vector3 start, Vector3 end, float time)
    {
        float percent = 0;
        Vector3 heightPivotVector = new Vector3((end - start).x / 2f + start.x, start.y + movePivotHeight, (end - start).z / 2f + start.z);
        Vector3 startToPivotLerp;
        Vector3 pivotToEndLerp;

        footRotate = false;

        while (percent <= 1)
        {
            startToPivotLerp = Vector3.Lerp(start, heightPivotVector, percent);
            pivotToEndLerp = Vector3.Lerp(heightPivotVector, end, percent);
            foot.position = Vector3.Lerp(startToPivotLerp, pivotToEndLerp, percent);

            percent += Time.deltaTime / time;

            yield return null;
        }

        footRotate = true;
        AlignFoot();
    }

    private void AlignFoot()
    {
        Debug.Log(1);
        if (hip.velocity == Vector3.zero && beforeMoveFoot != null)
        {
            StopCoroutine(Align());
            StartCoroutine(Align());
        }
    }

    private IEnumerator Align()
    {
        float percent = 0;
        Vector3 hipStart = hipAnchor.position;
        Vector3 hipEnd = new Vector3(beforeMoveFoot.position.x, hipAnchor.position.y, beforeMoveFoot.position.z);

        hipEnd -= beforeMoveFoot == rightFootTarget ? rightFootOffset : leftFootOffset;

        //foot align
        //if (beforeMoveFoot == rightFootTarget)
        //{
        //    prevLeftFootTargetPos = leftFootTargetPos;
        //    leftFootTargetPos = beforeMoveFoot.position +
        //        hips.rotation * new Vector3(leftFootOffset.x * 2, 0, leftFootOffset.z);

        //    FootMoveAnimation(leftFootTarget, prevLeftFootTargetPos, leftFootTargetPos, alignTime);
        //}
        //else
        //{
        //    prevLeftFootTargetPos = rightFootTargetPos;
        //    rightFootTargetPos = beforeMoveFoot.position +
        //        hips.rotation * new Vector3(rightFootOffset.x * 2, 0, rightFootOffset.z);
        //    FootMoveAnimation(rightFootTarget, prevRightFootTargetPos, rightFootTargetPos, alignTime);
        //}

        ////set foot middle pos
        //footMiddlePos = (leftFootTargetPos - rightFootTargetPos) / 2f + rightFootTargetPos;

        //body dlign
        while (percent <= 1)
        {
            Debug.Log(132);
            percent += Time.deltaTime / alignTime;

            hipAnchor.position = Vector3.Lerp(hipStart, hipEnd, percent);

            yield return null;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(footMiddlePos, shouldMoveDistance);
        Gizmos.DrawCube(footMiddlePos, new Vector3(0.05f, 0.05f, 0.05f));
        Gizmos.DrawSphere(leftHitPos, 0.1f);
        Gizmos.DrawSphere(rightHitPos, 0.1f);

    }
#endif
}