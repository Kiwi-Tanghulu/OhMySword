using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations.Rigging;

[System.Serializable]
public class Foot
{
    public Transform obj;
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
    public bool footMove { get; set; } = true;
    public bool isGround = true;
    private bool beforeIsGround = true;

    [Space]
    [SerializeField] public Rigidbody hip;
    [SerializeField] private Transform hipAncher;
    public Transform neck;

    [Space]
    [SerializeField] private ConfigurableJoint spine;
    [SerializeField] private Rigidbody spineRb;

    [Space]
    [SerializeField] private Foot leftFoot;
    [SerializeField] private Foot rightFoot;

    [Space]
    [SerializeField] private Transform rightArmRigTarget;

    [Space]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float shouldMoveDistance = 0.35f;
    [SerializeField] private float moveDistance = 0.3f;
    [SerializeField] private float movePivotHeight = 1f;
    [SerializeField] private float footMoveTime = 0.2f;
    [SerializeField] private float hipElasticitySpeed = 6f;
    [SerializeField] private float footToeOffset = 0.1f;
    [SerializeField] private float hipHeight = 0.55f;

    [Space]
    [SerializeField] private float footAlignTime = 0.5f;
    [SerializeField] private float footAlignDelayTime = 1f;
    [SerializeField] private float footAlignDistance = 0.3f;

    //where feet should be
    private Vector3 hipToGroundPos;

    //between pos of foot
    private Vector3 footMiddlePos;

    private Vector3 moveDir;

    private Coroutine footControlCo;
    private Coroutine footAlignCo;

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
        CheckGround();

        if (isGround)
        {
            SetHipConstraint(true);
            HipElasticity();
            FootMove();
        }
        else
        {
            
        }

        //trans frame
        if (beforeIsGround != isGround)
        {
            SetBodyControl(isGround);
            SetFootControl(isGround);
            hip.velocity = Vector3.zero;
        }
    }

    private bool CheckGround()
    {
        beforeIsGround = isGround;
        isGround = Physics.Raycast(hip.transform.position, Vector3.down, hipHeight + 0.5f, groundLayer);

        return isGround;
    }


    public void SetVelocity(Vector3 velocity)
    {
        hip.velocity = new Vector3(velocity.x, hip.velocity.y, velocity.z);
        moveDir = velocity.normalized;
    }

    #region FOOT
    private void FootMove()
    {
        if (!footMove)
            return;

        leftFoot.target.position = leftFoot.targetPos;
        rightFoot.target.position = rightFoot.targetPos;

        if (SetHipToGroundPos())
        {
            if (Vector3.Distance(hipToGroundPos, footMiddlePos) >= shouldMoveDistance)
            {
                Vector3 moveDir = (hipToGroundPos - footMiddlePos).normalized;

                if (Vector3.Distance(hipToGroundPos, leftFoot.targetPos) > Vector3.Distance(hipToGroundPos, rightFoot.targetPos))
                    SetFootTargetPos(leftFoot, moveDir * moveDistance, false);
                else
                    SetFootTargetPos(rightFoot, moveDir * moveDistance, false);
            }
        }
    }
    private void SetFootTargetPos(Foot foot, Vector3 offset, bool fix)
    {
        RaycastHit hit = default;
        Physics.Raycast(hip.transform.position +  hipAncher.rotation * foot.offset + offset,
            Vector3.down, out hit, 10, groundLayer);

        

        foot.rayHitPos = hit.point + Vector3.up * footToeOffset;

        if(fix)
        {
            foot.targetPos = foot.rayHitPos;
            foot.prevTargetPos = foot.targetPos;
        }
        else
        {
            foot.prevTargetPos = foot.targetPos;
            foot.targetPos = foot.rayHitPos;
            StartCoroutine(FootMoveAnimation(foot));
        }

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

        if (footAlignCo != null)
        {
            StopCoroutine(footAlignCo);
            footAlignCo = null;
        }

        while (percent <= 1)
        {
            startToPivotLerp = Vector3.Lerp(foot.prevTargetPos, heightPivotVector, percent);
            pivotToEndLerp = Vector3.Lerp(heightPivotVector, foot.targetPos, percent);
            foot.target.position = Vector3.Lerp(startToPivotLerp, pivotToEndLerp, percent);

            percent += Time.deltaTime / footMoveTime;

            yield return null;
        }

        Transform effect = PoolManager.Instance.Pop("WalkEffect", foot.target.position + moveDir * 0.2f).transform;
        effect.LookAt(effect.position - moveDir);

        if (moveDir == Vector3.zero && footAlignCo == null)
            footAlignCo = StartCoroutine(FootAlign());
    }

    private void SetFootMiddlePos()
    {
        footMiddlePos = (leftFoot.targetPos - rightFoot.targetPos) / 2f + rightFoot.targetPos;
    }

    private void SetFootControl(bool value)
    {
        footMove = value;
        SetFootRig(value);

        if(value)
        {
            SetFootTargetPos(leftFoot, Vector3.zero, true);
            SetFootTargetPos(rightFoot, Vector3.zero, true);
        }
    }
    private void SetFootRig(bool value)
    {
        if (footControlCo != null)
            StopCoroutine(footControlCo);

        footControlCo = StartCoroutine(SetFootRigCo(value));
    }
    private IEnumerator SetFootRigCo(bool value)
    {
        float percent = 0;

        float start = value ? 0 : 1;
        float end = value ? 1 : 0;

        while (percent <= 1)
        {
            leftFoot.ik.weight = Mathf.Lerp(start, end, percent);
            rightFoot.ik.weight = Mathf.Lerp(start, end, percent);

            percent += Time.deltaTime / 0.3f;

            yield return null;
        }
    }

    private IEnumerator FootAlign()
    {
        if (Mathf.Abs(leftFoot.obj.position.y - rightFoot.obj.position.y) >= footAlignDistance)
        {
            footAlignCo = null;
            yield break;
        }
        
        yield return new WaitForSeconds(footAlignDelayTime);

        Vector3 lStart = leftFoot.targetPos;
        Vector3 rStart = rightFoot.targetPos;
        float y = leftFoot.targetPos.y > rightFoot.targetPos.y ? leftFoot.targetPos.y : rightFoot.targetPos.y;
        Vector3 lEnd = new Vector3(hipAncher.position.x, y, hipAncher.position.z)
            + hipAncher.rotation * leftFoot.offset;
        Vector3 rEnd = new Vector3(hipAncher.position.x, y, hipAncher.position.z)
            + hipAncher.rotation * rightFoot.offset;
        float percent = 0;

        while(percent <= 1)
        {
            leftFoot.targetPos = Vector3.Lerp(lStart, lEnd, percent);
            rightFoot.targetPos = Vector3.Lerp(rStart, rEnd, percent);

            percent += Time.deltaTime / footAlignTime;

            SetFootMiddlePos();

            yield return null;
        }

        footAlignCo = null;
    }
    #endregion

    #region HIP
    public bool SetHipToGroundPos()
    {
        if (Physics.Raycast(hip.transform.position, Vector3.down, out RaycastHit hipToGround, hipHeight + 0.6f, groundLayer))
        {
            hipToGroundPos = hipToGround.point;

            return true;
        }

        return false;
    }

    private void SetHipConstraint(bool turnOn)
    {
        if (turnOn)
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

    private void SetHipAncherPos()
    {
        hipAncher.position = hipToGroundPos + Vector3.up * hipHeight;
    }

    private void HipElasticity()
    {
        if (!bodyLerping)
            return;

        Vector3 targetPos = hip.transform.position;

        if(Vector3.Distance(hip.transform.position, hipAncher.position) > 0.01f)
        {
            targetPos.y = hipAncher.position.y;

            if(moveDir == Vector3.zero)
            {
                targetPos.x = hipAncher.position.x;
                targetPos.z = hipAncher.position.z;
            }
        }

        hip.transform.position = Vector3.Lerp(hip.transform.position,
                targetPos, hipElasticitySpeed * Time.deltaTime);
    }// body lerping

    private void SetBodyControl(bool value)
    {
        JointDrive xDrive = spine.angularXDrive;
        JointDrive yzDrive = spine.angularYZDrive;

        if(value)
        {
            xDrive.positionSpring = 1000f;
            yzDrive.positionSpring = 1000f;
            SetHipToGroundPos();
        }
        //else
        //{
        //    xDrive.positionSpring = 0f;
        //    yzDrive.positionSpring = 0f;
        //}

        spine.angularXDrive = xDrive;
        spine.angularYZDrive = yzDrive;
        bodyLerping = value;
        hip.useGravity = !value;
        SetHipConstraint(value);
    }
    #endregion

    #region ARM
    public IEnumerator SetRightArmRigTarget(Transform targetTrm, float time)
    {
        float percent = 0f;

        while(percent <= 1f)
        {
            rightArmRigTarget.position = Vector3.Lerp(rightArmRigTarget.position, targetTrm.position, percent);
            rightArmRigTarget.rotation = Quaternion.Lerp(rightArmRigTarget.rotation, targetTrm.rotation, percent);

            percent += Time.deltaTime / time;

            yield return null;
        }
    }
    #endregion

    #region SPINE
    public void AddForceToSpine(Vector3 power)
    {
        spineRb.AddForce(power, ForceMode.Impulse);
    }
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