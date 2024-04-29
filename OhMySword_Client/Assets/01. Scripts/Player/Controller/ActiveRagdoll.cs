using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations.Rigging;

[System.Serializable]
public class Foot
{
    public Transform Obj;
    public Transform Target;
    public TwoBoneIKConstraint IK;
    [HideInInspector] public Vector3 TargetPos;
    [HideInInspector] public Vector3 PrevTargetPos;
    //Distance between body and feet, Only used x and z
    [HideInInspector] public Vector3 Offset;
    //The location where the ray was fired from the body and reached the ground.
    [HideInInspector] public Vector3 RayHitPos;
    public void SetTargetPos(Vector3 pos)
    {
        PrevTargetPos = pos;
        TargetPos = pos;
    }
}

public class ActiveRagdoll : MonoBehaviour
{
    [field: SerializeField]
    public bool BodyLerping { get; set; } = true;
    [field: SerializeField]
    public bool EnableFootMove { get; set; } = true;
    [field: SerializeField]
    public bool Controlable { get; private set; } = true;
    public bool IsGround = true;
    private bool beforeIsGround = true;

    [Space]
    [SerializeField] private Animator rigAnimator;
    [SerializeField] private RigBuilder rigBuilder;

    [Space]
    [SerializeField] public Rigidbody Hip;
    [SerializeField] private ConfigurableJoint hipJoint;
    [SerializeField] private Transform hipAncher;
    public Transform Neck;

    [Space]
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
    [SerializeField] private float horizontalHipElasticitySpeed = 6f;
    [SerializeField] private float verticalHipElasticitySpeed = 6f;
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

    public AudioSource aud;

    private PlayerAnimation playerAnimation;
    private PlayerAttack playerAttack;

    private void Awake()
    {
        playerAnimation = GetComponent<PlayerAnimation>();
        playerAttack = GetComponent<PlayerAttack>();
        aud = Hip.GetComponent<AudioSource>();
    }

    private void Start()
    {
        leftFoot.TargetPos = leftFoot.Target.position;
        leftFoot.PrevTargetPos = leftFoot.TargetPos;
        leftFoot.Offset = leftFoot.Target.position - Hip.transform.position;
        leftFoot.Offset.y = 0;

        rightFoot.TargetPos = rightFoot.Target.position;
        rightFoot.PrevTargetPos = rightFoot.TargetPos;
        rightFoot.Offset = rightFoot.Target.position - Hip.transform.position;
        rightFoot.Offset.y = 0;

        SetFootMiddlePos();
    }

    private void Update()
    {
        if (!Controlable)
        {
            SetHipToGroundPos();
            SetHipAncherPos();
        }

        CheckGround();

        if (IsGround && Controlable)
        {
            SetHipConstraint(true);
            HipElasticity();
            FootMove();
        }
        else if(!IsGround)
        {
            
        }

        //trans frame
        if (beforeIsGround != IsGround)
        {
            SetConrol(IsGround);
            Hip.velocity = Vector3.zero;
        }
    }

    public void SetConrol(bool value)
    {
        Controlable = value;
        SetBodyControl(value);
        SetFootControl(value);
        //rigAnimator.enabled = value;
        //rigBuilder.enabled = value;
        playerAnimation.Animationable = value;
        playerAttack.CanAttack = value;
    }

    private bool CheckGround()
    {
        beforeIsGround = IsGround;
        IsGround = Physics.Raycast(Hip.transform.position, Vector3.down, hipHeight + 0.5f, groundLayer);

        return IsGround;
    }


    public void SetVelocity(Vector3 velocity)
    {
        Hip.velocity = new Vector3(velocity.x, Hip.velocity.y, velocity.z);
        moveDir = velocity.normalized;
    }

    #region FOOT
    private void FootMove()
    {
        if (!EnableFootMove)
            return;

        leftFoot.Target.position = leftFoot.TargetPos;
        rightFoot.Target.position = rightFoot.TargetPos;

        if (SetHipToGroundPos())
        {
            if (Vector3.Distance(hipToGroundPos, footMiddlePos) >= shouldMoveDistance)
            {
                Vector3 moveDir = (hipToGroundPos - footMiddlePos).normalized;

                if (Vector3.Distance(hipToGroundPos, leftFoot.TargetPos) > Vector3.Distance(hipToGroundPos, rightFoot.TargetPos))
                    SetFootTargetPos(leftFoot, moveDir * moveDistance, false);
                else
                    SetFootTargetPos(rightFoot, moveDir * moveDistance, false);
            }
        }
    }
    private void SetFootTargetPos(Foot foot, Vector3 offset, bool fix)
    {
        RaycastHit hit = default;
        Physics.Raycast(Hip.transform.position +  hipAncher.rotation * foot.Offset + offset,
            Vector3.down, out hit, 10, groundLayer);

        

        foot.RayHitPos = hit.point + Vector3.up * footToeOffset;

        if(fix)
        {
            foot.TargetPos = foot.RayHitPos;
            foot.PrevTargetPos = foot.TargetPos;
        }
        else
        {
            foot.PrevTargetPos = foot.TargetPos;
            foot.TargetPos = foot.RayHitPos;
            StartCoroutine(FootMoveAnimation(foot));
        }

        SetFootMiddlePos();
        SetHipAncherPos();
    }
    private IEnumerator FootMoveAnimation(Foot foot)
    {
        float percent = 0;
        Vector3 heightPivotVector = new Vector3((foot.TargetPos - foot.PrevTargetPos).x / 2f + foot.PrevTargetPos.x,
            foot.PrevTargetPos.y + movePivotHeight, (foot.TargetPos - foot.PrevTargetPos).z / 2f + foot.PrevTargetPos.z);
        Vector3 startToPivotLerp;
        Vector3 pivotToEndLerp;

        if (footAlignCo != null)
        {
            StopCoroutine(footAlignCo);
            footAlignCo = null;
        }

        while (percent <= 1)
        {
            startToPivotLerp = Vector3.Lerp(foot.PrevTargetPos, heightPivotVector, percent);
            pivotToEndLerp = Vector3.Lerp(heightPivotVector, foot.TargetPos, percent);
            foot.Target.position = Vector3.Lerp(startToPivotLerp, pivotToEndLerp, percent);

            percent += Time.deltaTime / footMoveTime;

            yield return null;
        }

        Transform effect = PoolManager.Instance.Pop("WalkEffect", foot.Target.position + moveDir * 0.2f).transform;
        AudioManager.Instance.PlayAudio("GrassFootstep", aud, true);
        effect.LookAt(effect.position - moveDir);

        if (moveDir == Vector3.zero && footAlignCo == null)
            footAlignCo = StartCoroutine(FootAlign());
    }

    private void SetFootMiddlePos()
    {
        footMiddlePos = (leftFoot.TargetPos - rightFoot.TargetPos) / 2f + rightFoot.TargetPos;
    }

    private void SetFootControl(bool value)
    {
        EnableFootMove = value;
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
            leftFoot.IK.weight = Mathf.Lerp(start, end, percent);
            rightFoot.IK.weight = Mathf.Lerp(start, end, percent);

            percent += Time.deltaTime / 0.3f;

            yield return null;
        }
    }

    private IEnumerator FootAlign()
    {
        if (Mathf.Abs(leftFoot.Obj.position.y - rightFoot.Obj.position.y) >= footAlignDistance)
        {
            footAlignCo = null;
            yield break;
        }
        
        yield return new WaitForSeconds(footAlignDelayTime);

        Vector3 lStart = leftFoot.TargetPos;
        Vector3 rStart = rightFoot.TargetPos;
        float y = leftFoot.TargetPos.y > rightFoot.TargetPos.y ? leftFoot.TargetPos.y : rightFoot.TargetPos.y;
        Vector3 lEnd = new Vector3(hipAncher.position.x, y, hipAncher.position.z)
            + hipAncher.rotation * leftFoot.Offset;
        Vector3 rEnd = new Vector3(hipAncher.position.x, y, hipAncher.position.z)
            + hipAncher.rotation * rightFoot.Offset;
        float percent = 0;

        while(percent <= 1)
        {
            leftFoot.TargetPos = Vector3.Lerp(lStart, lEnd, percent);
            rightFoot.TargetPos = Vector3.Lerp(rStart, rEnd, percent);

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
        if (Physics.Raycast(Hip.transform.position, Vector3.down, out RaycastHit hipToGround, hipHeight + 0.6f, groundLayer))
        {
            hipToGroundPos = hipToGround.point;

            return true;
        }

        return false;
    }

    public void SetHipConstraint(bool turnOn)
    {
        if (turnOn)
        {
            if (moveDir == Vector3.zero)
            {
                Hip.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
            }
            else if (moveDir.x == 0)
                Hip.constraints = RigidbodyConstraints.FreezePositionX;
            else if (moveDir.z == 0)
                Hip.constraints = RigidbodyConstraints.FreezePositionZ;
            else
                Hip.constraints = RigidbodyConstraints.None;

            Hip.constraints |= RigidbodyConstraints.FreezePositionY;

            Hip.constraints |= RigidbodyConstraints.FreezeRotationX
            | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

            //hip.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            Hip.constraints = RigidbodyConstraints.None;
        }
    }

    private void SetHipAncherPos()
    {
        hipAncher.position = hipToGroundPos + Vector3.up * hipHeight;
    }

    private void HipElasticity()
    {
        if (!BodyLerping)
            return;

        Vector3 targetPos = Hip.transform.position;

        if(Vector3.Distance(Hip.transform.position, hipAncher.position) > 0.01f)
        {
            targetPos.y = Mathf.Lerp(targetPos.y, hipAncher.position.y, verticalHipElasticitySpeed * Time.deltaTime);

            if(moveDir == Vector3.zero)
            {
                targetPos.x = Mathf.Lerp(targetPos.x, hipAncher.position.x, horizontalHipElasticitySpeed * Time.deltaTime);
                targetPos.z = Mathf.Lerp(targetPos.z, hipAncher.position.z, horizontalHipElasticitySpeed * Time.deltaTime);

                //targetPos.x = hipAncher.position.x;
                //targetPos.z = hipAncher.position.z;
            }
        }

        Hip.transform.position = targetPos;
    }// body lerping

    private void SetBodyControl(bool value)
    {
        JointDrive xDrive = hipJoint.angularXDrive;
        JointDrive yzDrive = hipJoint.angularYZDrive;

        if(value)
        {
            xDrive.positionSpring = 1000f;
            yzDrive.positionSpring = 1000f;
            SetHipToGroundPos();
        }
        else
        {
            xDrive.positionSpring = 0f;
            yzDrive.positionSpring = 0f;
        }

        hipJoint.angularXDrive = xDrive;
        hipJoint.angularYZDrive = yzDrive;
        BodyLerping = value;
        SetHipConstraint(value);
        SetSpineConstraint(value);
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

    private void SetSpineConstraint(bool value)
    {
        if(value)
        {
            //spineRb.constraints = RigidbodyConstraints.FreezeRotation;
        }
        else
        {
            spineRb.constraints = RigidbodyConstraints.None;
        }
    }
    #endregion

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(footMiddlePos, shouldMoveDistance);
        Gizmos.DrawSphere(footMiddlePos, 0.1f);
        Gizmos.DrawSphere(leftFoot.RayHitPos, 0.1f);
        Gizmos.DrawSphere(rightFoot.RayHitPos, 0.1f);
    }
#endif
}