using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

[Serializable]
public class Foot
{
    public Transform target;
    public TwoBoneIKConstraint ik;
    [HideInInspector] public Vector3 targetPos;
    [HideInInspector] public Vector3 prevTargetPos;
    [HideInInspector] public Vector3 offset;
    [HideInInspector] public Vector3 rayHitPos;

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



    [SerializeField] private bool isGround = false;
    private bool beforeIsGround = false;
    [SerializeField] private bool isMove = false;

    [SerializeField] private Foot leftFoot;
    [SerializeField] private Foot rightFoot;
    [SerializeField] private Foot nextMoveFoot;
    [SerializeField] private Foot lastMoveFoot;

    private Vector3 moveDir;
    

    private void Start()
    {
        leftFoot.targetPos = leftFoot.target.position;
        leftFoot.prevTargetPos = leftFoot.targetPos;
        leftFoot.offset = leftFoot.target.position - hips.position;
        leftFoot.offset.y = 0f;

        rightFoot.targetPos = rightFoot.target.position;
        rightFoot.prevTargetPos = rightFoot.target.position;
        rightFoot.offset = rightFoot.target.position - hips.position;
        rightFoot.offset.y = 0f;

        SetFootMiddlePos();
        SetHipAncherPos();

        lastMoveFoot = leftFoot;
        nextMoveFoot = rightFoot;

    }

    #region legacy

    //private void Update()
    //{
    //    FootMove();
    //    SetBodyAncherPos();
    //}

    //public void Move(Vector3 velocity)
    //{
    //    if (velocity == Vector3.zero)
    //    {
    //        hip.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
    //        HipElasticity();
    //    }
    //    else if (velocity.x == 0)
    //        hip.constraints = RigidbodyConstraints.FreezePositionX;
    //    else if (velocity.z == 0)
    //        hip.constraints = RigidbodyConstraints.FreezePositionZ;
    //    else
    //        hip.constraints = RigidbodyConstraints.None;
    //    hip.constraints |= RigidbodyConstraints.FreezePositionY;

    //    hip.velocity = velocity;
    //}

    //private void FootMove()
    //{
    //    if (!footMove)
    //        return;

    //    leftFootTarget.position = leftFootTargetPos;
    //    rightFootTarget.position = rightFootTargetPos;

    //    if (Physics.Raycast(hip.position, Vector3.down, out RaycastHit hipToGround, 1, groundLayer))
    //    {
    //        hipToGroundPos = hipToGround.point;

    //        if (Vector3.Distance(hipToGroundPos, footMiddlePos) >= shouldMoveDistance)
    //        {
    //            Vector3 moveDir = (hipToGroundPos - footMiddlePos).normalized;

    //            if (Vector3.Distance(hipToGroundPos, leftFootTargetPos) > Vector3.Distance(hipToGroundPos, rightFootTargetPos))
    //            {
    //                RaycastHit lefttHit = default;
    //                Physics.Raycast(new Vector3(hips.position.x + leftFootOffset.x + moveDir.x * moveDistance,
    //                    hips.position.y, hips.position.z + leftFootOffset.z + moveDir.z * moveDistance), Vector3.down, out lefttHit, 10, groundLayer);

    //                leftHitPos = lefttHit.point + Vector3.up * footToeOffset;
    //                prevLeftFootTargetPos = leftFootTargetPos;
    //                leftFootTargetPos = leftHitPos;

    //                StartCoroutine(FootMoveAnimation(leftFootTarget, prevLeftFootTargetPos, leftFootTargetPos));
    //            }
    //            else
    //            {
    //                RaycastHit righttHit = default;
    //                Physics.Raycast(new Vector3(hips.position.x + rightFootOffset.x + moveDir.x * moveDistance, hips.position.y,
    //                    hips.position.z + rightFootOffset.z + moveDir.z * moveDistance), Vector3.down, out righttHit, 10, groundLayer);

    //                rightHitPos = righttHit.point + Vector3.up * footToeOffset;
    //                prevRightFootTargetPos = rightFootTargetPos;
    //                rightFootTargetPos = rightHitPos;

    //                StartCoroutine(FootMoveAnimation(rightFootTarget, prevRightFootTargetPos, rightFootTargetPos));
    //            }

    //            footMiddlePos = (leftFootTargetPos - rightFootTargetPos) / 2f + rightFootTargetPos;
    //        }
    //    }
    //}

    //private void SetBodyAncherPos()
    //{
    //    hipAnchor.position = hipToGroundPos + Vector3.up * hipAnchorHeight;
    //}



    // body lerping

    #endregion


    private void FixedUpdate()
    {
        if(CheckGround())
        {
            OnGroundProcess();
        }
        else
        {

        }
    }

    private void OnGroundProcess()
    {
        //착지 시점 기능 구현

        FixFootPos(leftFoot);
        FixFootPos(rightFoot);


        if(isMove)
        {
            if(Vector3.Distance(footMiddlePos, hipToGroundPos) >= shouldMoveDistance)
            {
                if(Vector3.Distance(hipToGroundPos, leftFootTargetPos) > Vector3.Distance(hipToGroundPos, rightFootTargetPos))
                    SetFootTargetPos(leftFoot, hipAnchor.rotation * moveDir * moveDistance);
                else
                    SetFootTargetPos(rightFoot, hipAnchor.rotation * moveDir * moveDistance);
            }
        }
        else
        {
            HipElasticity();
        }
    }

    #region new
    private bool CheckGround()
    {
        beforeIsGround = isGround;
        RaycastHit hit;
        isGround = Physics.Raycast(hip.transform.position, Vector3.down, out hit, hipAnchorHeight + 0.1f, groundLayer);
        hipToGroundPos = hit.point;

        return isGround;
    }

    public void SetVelocity(Vector3 velocity)
    {
        hip.velocity = velocity;
        moveDir = velocity.normalized;
        isMove = velocity != Vector3.zero;
        SetHipConstraint(true);
    }

    #region HIP
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

        if(Vector3.Distance(hipAnchor.position, hip.transform.position) >= 0.1f)
        {
            hip.transform.position = Vector3.Lerp(hip.transform.position,
            hipAnchor.position, hipElasticitySpeed * Time.deltaTime);
        }
    }

    private void SetHipConstraint(bool isOn)
    {
        if(isOn)
        {
            if (moveDir == Vector3.zero)
            {
                hip.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
            }
            else if (moveDir.x == 0)
                hip.constraints = RigidbodyConstraints.FreezePositionX;
            else if (moveDir.z == 0)
                hip.constraints = RigidbodyConstraints.FreezePositionZ;
            else
                hip.constraints = RigidbodyConstraints.None;

            hip.constraints |= RigidbodyConstraints.FreezePositionY;
        }
        else
            hip.constraints = RigidbodyConstraints.None;
    }
    #endregion

    #region FOOT
    private void FixFootPos(Foot foot)
    {
        if (!footMove)
            return;
        
        foot.target.position = foot.targetPos;
    }

    private void SetFootTargetPos(Foot foot, Vector3 offset)
    {
        //Debug.Log("Set Foot Target Pos");
        //Vector3 rayStartPos = hip.transform.position + (hipAnchor.rotation * foot.offset) + offset;
        //RaycastHit hit;
        //Physics.Raycast(rayStartPos, Vector3.down, out hit, hipAnchorHeight + 0.1f, groundLayer);

        //lastMoveFoot = foot;
        //nextMoveFoot = foot == leftFoot ? rightFoot : leftFoot;

        //foot.SetTargetPos(hit.point);
        //foot.rayHitPos = hit.point;

        //StartCoroutine(FootMoveAnimation(foot));
        //SetHipAncherPos();
        //SetFootMiddlePos();

        Debug.Log("Set Foot Target Pos");
        RaycastHit hit = default;
        Physics.Raycast(new Vector3(hips.position.x + moveDir.x * moveDistance,
            hips.position.y, hips.position.z + moveDir.z * moveDistance) + foot.offset, 
            Vector3.down, out hit, hipAnchorHeight + 0.1f, groundLayer);

        foot.SetTargetPos(hit.point + Vector3.up * footToeOffset);

        StartCoroutine(FootMoveAnimation(foot.target, foot.prevTargetPos, foot.targetPos));
        SetHipAncherPos();
        SetFootMiddlePos();
    }

    private IEnumerator FootMoveAnimation(Transform foot, Vector3 start, Vector3 end)
    {
        float percent = 0;
        Vector3 heightPivotVector = new Vector3((end - start).x / 2f + start.x, 
            start.y + movePivotHeight, (end - start).z / 2f + start.z);
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

    private void SetFootMiddlePos()
    {
        footMiddlePos = (leftFoot.targetPos - rightFoot.targetPos) / 2f + rightFoot.targetPos;
    }
    #endregion
    #endregion

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(footMiddlePos, shouldMoveDistance);
        Gizmos.DrawSphere(footMiddlePos, 0.1f);
        Gizmos.DrawLine(hipAnchor.position, hipToGroundPos);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(hipToGroundPos, 0.1f);
    }
#endif
}