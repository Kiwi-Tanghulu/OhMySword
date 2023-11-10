using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations.Rigging;

[System.Serializable]
public class Foot
{
    public Transform target;
    public TwoBoneIKConstraint ik;
    [HideInInspector] public Vector3 targetPos;
    [HideInInspector] public Vector3 prevTargetPos;
    //Distance between body and feet, Only used x and z
    [HideInInspector] public Vector3 offset;
    //The location where the ray was fired from the body and reached the ground.
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
    public Transform hipAncher;

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
    private Vector3 hipToGroundPos;

    //between pos of foot
    private Vector3 footMiddlePos;

    [SerializeField] private Foot leftFoot;
    [SerializeField] private Foot rightFoot;

    private Vector3 moveDir;

    [SerializeField] private bool isGround;

    private void Start()
    {
        leftFoot.targetPos = leftFoot.target.position;
        leftFoot.prevTargetPos = leftFoot.targetPos;
        leftFoot.offset = leftFoot.target.position - hip.transform.position;
        leftFoot.offset.y = 0;

        rightFoot.targetPos = rightFoot.target.position;
        rightFoot.prevTargetPos = rightFoot.targetPos;
        rightFoot.offset = rightFoot.target.position - hip.transform.position;
        rightFoot.offset.y = 0;

        SetFootMiddlePos();
    }

    private void Update()
    {
        if(CheckGround())
        {
            SetHipConstraint();
            HipElasticity();
            FootMove();
        }
        else
        {

        }
    }

    private bool CheckGround()
    {
        isGround = Physics.Raycast(hip.transform.position, Vector3.down, hipHeight + 0.1f, groundLayer);

        return isGround;
    }


    public void SetVelocity(Vector3 velocity)
    {
        hip.velocity = velocity;
        moveDir = velocity.normalized;
    }

    #region FOOT
    private void FootMove()
    {
        if (!footMove)
            return;

        leftFoot.target.position = leftFoot.targetPos;
        rightFoot.target.position = rightFoot.targetPos;

        if (Physics.Raycast(hip.transform.position, Vector3.down, out RaycastHit hipToGround, 1, groundLayer))
        {
            hipToGroundPos = hipToGround.point;

            if (Vector3.Distance(hipToGroundPos, footMiddlePos) >= shouldMoveDistance)
            {
                Vector3 moveDir = (hipToGroundPos - footMiddlePos).normalized;

                if (Vector3.Distance(hipToGroundPos, leftFoot.targetPos) > Vector3.Distance(hipToGroundPos, rightFoot.targetPos))
                    SetFootTargetPos(leftFoot, moveDir * moveDistance);
                else
                    SetFootTargetPos(rightFoot, moveDir * moveDistance);
            }
        }
    }
    private void SetFootTargetPos(Foot foot, Vector3 offset)
    {
        RaycastHit hit = default;
        Physics.Raycast(hip.transform.position + foot.offset + offset,
            Vector3.down, out hit, 10, groundLayer);

        foot.rayHitPos = hit.point + Vector3.up * footToeOffset;
        foot.prevTargetPos = foot.targetPos;
        foot.targetPos = foot.rayHitPos;

        StartCoroutine(FootMoveAnimation(foot));

        SetFootMiddlePos();
        SetHipAncherPos();
    }
    private IEnumerator FootMoveAnimation(Foot foot)
    {
        float percent = 0;
        Vector3 heightPivotVector = new Vector3((foot.targetPos - foot.prevTargetPos).x / 2f + foot.prevTargetPos.x,
            foot.prevTargetPos.y + movePivotHeight, (foot.targetPos - foot.prevTargetPos).z / 2f + foot.prevTargetPos.z);
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

    private void SetFootMiddlePos()
    {
        footMiddlePos = (leftFoot.targetPos - rightFoot.targetPos) / 2f + rightFoot.targetPos;
    }
    #endregion

    #region HIP
    private void SetHipConstraint()
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

    private void SetHipAncherPos()
    {
        hipAncher.position = hipToGroundPos + Vector3.up * hipHeight;
    }

    private void HipElasticity()
    {
        if (!bodyLerping)
            return;

        if(moveDir == Vector3.zero && Vector3.Distance(hip.transform.position, hipAncher.position) > 0.01f)
        {
            hip.transform.position = Vector3.Lerp(hip.transform.position,
            new Vector3(footMiddlePos.x, hipAncher.position.y, footMiddlePos.z), hipElasticitySpeed * Time.deltaTime);
        }
    }// body lerping
    #endregion

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(footMiddlePos, shouldMoveDistance);
        Gizmos.DrawSphere(footMiddlePos, 0.1f);
        Gizmos.DrawSphere(leftFoot.rayHitPos, 0.1f);
        Gizmos.DrawSphere(rightFoot.rayHitPos, 0.1f);
    }
#endif
}