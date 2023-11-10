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
    private Vector3 hipToGroundPos;

    //The location where the ray was fired from the body and reached the ground.
    private Vector3 leftHitPos;
    private Vector3 rightHitPos;

    //Distance between body and feet, Only used x and z

    //between pos of foot
    private Vector3 footMiddlePos;



    [SerializeField] private Foot leftFoot;
    [SerializeField] private Foot rightFoot;

    private Vector3 moveDir;

    private void Start()
    {
        leftFoot.targetPos = leftFoot.target.position;
        leftFoot.prevTargetPos = leftFoot.targetPos;
        leftFoot.offset = leftFoot.target.position - hips.position;
        leftFoot.offset.y = 0;

        rightFoot.targetPos = rightFoot.target.position;
        rightFoot.prevTargetPos = rightFoot.targetPos;
        rightFoot.offset = rightFoot.target.position - hips.position;
        leftFoot.offset.y = 0;

        SetFootMiddlePos();
    }

    private void SetFootMiddlePos()
    {
        footMiddlePos = (leftFoot.targetPos - rightFoot.targetPos) / 2f + rightFoot.targetPos;
    }

    private void SetFootTargetPos(Foot foot, Vector3 moveDir)
    {
        RaycastHit hit = default;
        Physics.Raycast(new Vector3(hips.position.x + foot.offset.x + moveDir.x * moveDistance,
            hips.position.y, hips.position.z + foot.offset.z + moveDir.z * moveDistance), Vector3.down, out hit, 10, groundLayer);

        foot.rayHitPos = hit.point + Vector3.up * footToeOffset;
        foot.prevTargetPos = foot.targetPos;
        foot.targetPos = foot.rayHitPos;

        StartCoroutine(FootMoveAnimation(foot.target, foot.prevTargetPos, foot.targetPos));

        SetFootMiddlePos();
    }

    public void SetVelocity(Vector3 velocity)
    {
        hip.velocity = velocity;
        moveDir = velocity.normalized;

        SetHipConstraint();
    }
    private void SetHipConstraint()
    {
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
    }


    private void Update()
    {
        FootMove();
        SetBodyAncherPos();
    }

    

    private void FootMove()
    {
        if (!footMove)
            return;

        leftFoot.target.position = leftFoot.targetPos;
        rightFoot.target.position = rightFoot.targetPos;

        if(Physics.Raycast(hip.position, Vector3.down, out RaycastHit hipToGround, 1, groundLayer))
        {
            hipToGroundPos = hipToGround.point;

            if (Vector3.Distance(hipToGroundPos, footMiddlePos) >= shouldMoveDistance)
            {
                Vector3 moveDir = (hipToGroundPos - footMiddlePos).normalized;
                
                if(Vector3.Distance(hipToGroundPos, leftFoot.targetPos) > Vector3.Distance(hipToGroundPos, rightFoot.targetPos))
                    SetFootTargetPos(leftFoot, moveDir);
                else
                    SetFootTargetPos(rightFoot, moveDir);
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
        Gizmos.DrawSphere(leftFoot.rayHitPos, 0.1f);
        Gizmos.DrawSphere(rightFoot.rayHitPos, 0.1f);
    }
#endif
}