using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations.Rigging;

[Serializable]
public class Foot
{
    public Transform foot;
    public TwoBoneIKConstraint ik;
    public Vector3 targetPos;
    [HideInInspector] public Vector3 prevTargetPos;
    //Distance between body and feet, Only used x and z
    [HideInInspector] public Vector3 offset;

    public void SetTargetPos(Vector3 pos)
    {
        prevTargetPos = targetPos;
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
    [field: SerializeField]
    public bool footRotate { get; set; } = true;
    [field: SerializeField]
    public bool isGround { get; set; } = false;
    private bool beforeIsGround;

    [Space]
    [SerializeField] private Rigidbody hip;
    [SerializeField] private Transform hips;
    [SerializeField] private Transform hipAnchor;

    [Space]
    [SerializeField] private Foot leftFoot;
    [SerializeField] private Foot rightFoot;
    [SerializeField] private Foot beforeMoveFoot;
    [SerializeField] private Foot nextMoveFoot;

    [Space]
    [SerializeField] private Transform handTrm;
    [SerializeField] private Transform normalHandTrm;
    [SerializeField] private Transform attackHandTrm;

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

    private Vector3 hipToGroundPos;

    //The location where the ray was fired from the body and reached the ground.
    private Vector3 leftHitPos;
    private Vector3 rightHitPos;

    //between pos of foot
    private Vector3 footMiddlePos;

    private void Start()
    {
        leftFoot.targetPos = leftFoot.foot.position;
        rightFoot.targetPos = rightFoot.foot.position;

        leftFoot.prevTargetPos = leftFoot.foot.position;
        rightFoot.prevTargetPos = rightFoot.foot.position;

        leftFoot.offset = leftFoot.foot.position - hips.position;
        rightFoot.offset = rightFoot.foot.position - hips.position;
        leftFoot.offset.y = 0;
        rightFoot.offset.y = 0;

        beforeMoveFoot = leftFoot;
        nextMoveFoot = rightFoot;

        SetFootMiddlePos();

        //Time.timeScale = 0.1f;
    }

    private void Update()
    {
        CheckGround();

        HipElasticity();
        FootMove();
    }

    #region MOVE
    public void SetVelocity(Vector3 velocity)
    {
        hip.velocity = new Vector3(velocity.x, hip.velocity.y, velocity.z);
        SetHipConstraint(velocity);
    }
    private void CheckGround()
    {
        beforeIsGround = isGround;
        isGround = Physics.Raycast(hip.position, Vector3.down, out RaycastHit hipToGround, hipHeight * 2, groundLayer);
        hipToGroundPos = hipToGround.point;

        if(isGround != beforeIsGround)
        {
            if (isGround)
            {
                SetFootTargetPos(leftFoot, Vector3.zero, true);
                SetFootTargetPos(rightFoot, Vector3.zero, true);
                SetBodyAncherPos();
                leftFoot.ik.weight = 1;
                rightFoot.ik.weight = 1;
            }
            else
            {
                hip.constraints = RigidbodyConstraints.None;
                leftFoot.ik.weight = 0;
                rightFoot.ik.weight = 0;
            }
        }
    }
    #endregion

    #region FOOT
    private void FootMove()
    {
        if (!footMove && isGround)
            return;

        leftFoot.foot.position = leftFoot.targetPos;
        rightFoot.foot.position = rightFoot.targetPos;

        if (Vector3.Distance(hipToGroundPos, footMiddlePos) >= shouldMoveDistance)
        {
            Vector3 moveDir = (hipToGroundPos - footMiddlePos).normalized;

            SetFootTargetPos(nextMoveFoot, moveDir * moveDistance, true);
        }
    }
    private void SetFootTargetPos(Foot foot, Vector3 offset, bool align)
    {
        RaycastHit hit = default;
        if(Physics.Raycast(new Vector3(beforeMoveFoot.targetPos.x, hips.position.y, beforeMoveFoot.targetPos.z) 
            + offset + hips.rotation * foot.offset, Vector3.down, out hit, 10, groundLayer))
        {
            foot.SetTargetPos(hit.point + Vector3.up * footToeOffset);

            beforeMoveFoot = nextMoveFoot;
            nextMoveFoot = nextMoveFoot == rightFoot ? leftFoot : rightFoot;

            SetFootMiddlePos();
            SetBodyAncherPos();
            StartCoroutine(FootMoveAnimation(foot, footMoveTime, align));
        }
    }
    private IEnumerator FootMoveAnimation(Foot foot, float time, bool align)
    {
        float percent = 0;
        Vector3 start = foot.prevTargetPos;
        Vector3 end = foot.targetPos;
        Vector3 heightPivotVector = new Vector3((end - start).x / 2f + start.x,
            start.y + movePivotHeight, (end - start).z / 2f + start.z);
        Vector3 startToPivotLerp;
        Vector3 pivotToEndLerp;

        footRotate = false;

        while (percent <= 1)
        {
            startToPivotLerp = Vector3.Lerp(start, heightPivotVector, percent);
            pivotToEndLerp = Vector3.Lerp(heightPivotVector, end, percent);
            foot.foot.position = Vector3.Lerp(startToPivotLerp, pivotToEndLerp, percent);

            percent += Time.deltaTime / time;

            yield return null;
        }

        footRotate = true;
        SetFootMiddlePos();
        if(align)
            AlignBody();
    }
    private void SetFootMiddlePos()
    {
        footMiddlePos = (leftFoot.targetPos - rightFoot.targetPos) / 2f + rightFoot.targetPos;
    }
    #endregion

    #region HIP
    private void SetBodyAncherPos()
    {
        hipAnchor.position = new Vector3(footMiddlePos.x, (hipToGroundPos + Vector3.up * hipHeight).y, footMiddlePos.z);
    }
    private void HipElasticity()
    {
        if (!isGround || !bodyLerping)
            return;

        if (hip.velocity == Vector3.zero)
        {
            hips.transform.position = Vector3.Lerp(hips.transform.position,
            hipAnchor.position, hipElasticitySpeed * Time.deltaTime);
        }
        else
        {
            hips.transform.position = Vector3.Lerp(hips.transform.position,
            new Vector3(hips.transform.position.x, hipAnchor.position.y, hips.transform.position.z), 
            hipElasticitySpeed * Time.deltaTime);
        }
    } // body lerping
    private void SetHipConstraint(Vector3 velocity)
    {
        if (!isGround)
            return;

        if (velocity == Vector3.zero)
            hip.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        else if (velocity.x == 0)
            hip.constraints = RigidbodyConstraints.FreezePositionX;
        else if (velocity.z == 0)
            hip.constraints = RigidbodyConstraints.FreezePositionZ;
        else
            hip.constraints = RigidbodyConstraints.None;
        hip.constraints |= RigidbodyConstraints.FreezePositionY;
    }
    #endregion

    #region ALIGN
    private void AlignBody()
    {
        if (hip.velocity == Vector3.zero)
        {
            StopCoroutine(Align());
            StartCoroutine(Align());
        }
    }

    private IEnumerator Align()
    {
        float percent = 0;
        //Vector3 hipStart = hipAnchor.position;
        //Vector3 hipEnd = new Vector3(beforeMoveFoot.targetPos.x, hipAnchor.position.y, beforeMoveFoot.targetPos.z)
        //    - hips.rotation * beforeMoveFoot.offset;

        //foot align
        SetFootTargetPos(nextMoveFoot, hips.rotation * nextMoveFoot.offset, false);

        //body align
        while (percent <= 1)
        {
            percent += Time.deltaTime / footMoveTime;

            //hipAnchor.position = Vector3.Lerp(hipStart, hipEnd, percent);

            yield return null;
        }
    }
    #endregion

    #region HAND

    public IEnumerator AttackMotion(float time)
    {
        float percent = 0;

        Vector3 startPos = handTrm.position;
        Quaternion startRot = handTrm.rotation;
        Debug.Log(423);
        while (percent <= 1)
        {
            percent += Time.deltaTime / time;

            handTrm.position = Vector3.Lerp(startPos, attackHandTrm.position, percent);
            handTrm.rotation = Quaternion.Lerp(startRot, attackHandTrm.rotation, percent);

            yield return null;
        }
    }
    public IEnumerator AttacRecoverykMotion(float time)
    {
        float percent = 0;
        Debug.Log(123);
        Vector3 startPos = handTrm.position;
        Quaternion startRot = handTrm.rotation;

        while (percent <= 1)
        {
            percent += Time.deltaTime / time;

            handTrm.position = Vector3.Lerp(startPos, normalHandTrm.position, percent);
            handTrm.rotation = Quaternion.Lerp(startRot, normalHandTrm.rotation, percent);

            yield return null;
        }
    }
    #endregion


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(footMiddlePos, shouldMoveDistance);
        Gizmos.DrawCube(footMiddlePos, new Vector3(0.05f, 0.05f, 0.05f));
        Gizmos.DrawSphere(leftFoot.targetPos, 0.1f);
        Gizmos.DrawSphere(rightFoot.targetPos, 0.1f);

    }
#endif
}