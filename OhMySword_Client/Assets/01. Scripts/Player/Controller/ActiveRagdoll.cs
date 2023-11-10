using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Foot
{
    public Transform target;
    public Rig ik;
    [HideInInspector] public Vector3 targetPos;
    [HideInInspector] public Vector3 prevTargetPos;
    [HideInInspector] public Vector3 offset;

    public void SetTargetPos(Vector3 pos)
    {
        prevTargetPos = pos;
        targetPos = pos;
    }
}

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
    public float moveDistance = 0.35f;
    public float movePivotHeight = 0.9f;
    public float footMoveTime = 0.2f;
    public float hipElasticitySpeed = 6f;
    public float footToeOffset = 0.1f;
    public float hipAnchorHeight = 0.55f;

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


    private bool isGround = false;

    private Foot leftFoot;
    private Foot rightFoot;

    private Vector3 moveDir;
    

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

    #region legacy

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
        hipAnchor.position = hipToGroundPos + Vector3.up * hipAnchorHeight;
    }

    

    // body lerping
    
    #endregion

    #region new
    private bool CheckGround()
    {
        isGround = Physics.Raycast(hipAnchor.position, Vector3.down, hipAnchorHeight, groundLayer);

        return isGround;
    }

    public void SetVelocity(Vector3 velocity)
    {
        hip.velocity = velocity;
        moveDir = velocity.normalized;
    }
    //hip
    private void SetHipAncherPos()
    {
        RaycastHit hit;
        Physics.Raycast(hip.transform.position, Vector3.down, out hit, 1, groundLayer);
        hipAnchor.position = hit.point + Vector3.up * hipAnchorHeight;
    }

    private void HipElasticity()
    {
        if (!bodyLerping)
            return;

        hip.transform.position = Vector3.Lerp(hip.transform.position,
            hipAnchor.position, hipElasticitySpeed * Time.deltaTime);
    }
    //foot
    private void FixFootPos(Foot foot)
    {
        if (!footMove)
            return;

        foot.target.position = foot.targetPos;
    }

    private void SetFootTargetPos(Foot foot, Vector3 offset)
    {
        Vector3 rayStartPos = hipAnchor.position + (hipAnchor.rotation * foot.offset) + offset;
        RaycastHit hit;
        Physics.Raycast(rayStartPos, Vector3.down, out hit, hipAnchorHeight, groundLayer);
        
        foot.SetTargetPos(hit.point);
    }

    private IEnumerator FootMoveAnimation(Foot foot)
    {
        float percent = 0;
        Vector3 heightPivotVector = new Vector3((foot.targetPos - foot.prevTargetPos).x / 2f, 
            movePivotHeight, (foot.targetPos - foot.prevTargetPos).z / 2f) + foot.prevTargetPos;
        Vector3 startToPivotLerp;
        Vector3 pivotToEndLerp;

        while (percent <= 1)
        {
            startToPivotLerp = Vector3.Lerp(foot.prevTargetPos, heightPivotVector, percent);
            pivotToEndLerp = Vector3.Lerp(heightPivotVector, foot.targetPos, percent);
            foot.target.position = Vector3.Lerp(startToPivotLerp, pivotToEndLerp, percent);

            percent += Time.deltaTime / footMoveTime;

            yield return null;
        }
    }
    #endregion

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