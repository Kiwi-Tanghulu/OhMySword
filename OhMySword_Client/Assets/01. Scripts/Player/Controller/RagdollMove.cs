using System.Collections;
using UnityEngine;

public class RagdollMove : MonoBehaviour
{
    //이동
    public float moveSpeed;
    public Rigidbody hipRigid;
    public Vector3 moveDir;

    //다리 움직임
    public Transform leftFootTarget;
    public Transform rightFootTarget;
    public Transform hip;
    public float shouldMoveDistance; //두 발의 중간 위치와 몸의 위치가 이 값 이상 차이나면 발 이동
    public float moveDistance; //발이 이동하는 거리
    public LayerMask groundLayer;
    public float moveHeightPivot;
    public float footMoveTime;
    private Vector3 leftFootTargetPos; // LeftFootIk의 Target의 위치
    private Vector3 rightFootTargetPos; //RightFootIk의 Target의 위치
    private Vector3 footMiddlePos; //두 발 사이의 중간 위치
    //x, z만 사용
    private Vector3 rightFootOffset; //오른쪽 발의 위치와 두 발 중간 위치의 차이
    private Vector3 leftFootOffset; //왼쪽 발의 위치와 두 발 중간 위치의 차이
    private Vector3 hipToGroundPos; // hip에서 바닥으로 레이를 쏴 감지된 위치  

    private void Start()
    {
        rightFootTargetPos = rightFootTarget.position;
        leftFootTargetPos = leftFootTarget.position;
        footMiddlePos = (leftFootTargetPos - rightFootTargetPos) / 2f + rightFootTargetPos;
        rightFootOffset = rightFootTargetPos - footMiddlePos;
        leftFootOffset = leftFootTargetPos - footMiddlePos;
    }

    private void Update()
    {
        leftFootTarget.position = leftFootTargetPos;
        rightFootTarget.position = rightFootTargetPos;

        Move();
        FootMove();
    }

    private void Move()
    {
        moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        hipRigid.velocity = moveDir * moveSpeed;
    }

    private void FootMove()
    {
        RaycastHit hipToGround = default;
        if(Physics.Raycast(hip.position, Vector3.down, out hipToGround, 10, groundLayer))
        {
            hipToGroundPos = hipToGround.point;

            if (Vector3.Distance(footMiddlePos, hipToGround.point) >= shouldMoveDistance)
            {
                Vector3 moveDir = (hipToGroundPos - footMiddlePos).normalized;

                if(Vector3.Distance(hipToGround.point, leftFootTargetPos) > Vector3.Distance(hipToGround.point, rightFootTargetPos))
                {
                    RaycastHit hit;
                    Physics.Raycast(new Vector3(hipToGroundPos.x + moveDir.x * moveDistance + leftFootOffset.x,
                        hip.position.y, hipToGroundPos.z + moveDir.z * moveDistance + leftFootOffset.z), Vector3.down, out hit, 10, groundLayer);
                    Vector3 nextFootPos = hit.point;

                    StartCoroutine(FootMoveAnimation(leftFootTarget, rightFootTargetPos, nextFootPos));
                    leftFootTargetPos = nextFootPos;
                }
                else
                {
                    RaycastHit hit;
                    Physics.Raycast(new Vector3(hipToGroundPos.x + moveDir.x * moveDistance + rightFootOffset.x,
                        hip.position.y, hipToGroundPos.z + moveDir.z * moveDistance + rightFootOffset.z), Vector3.down, out hit, 10, groundLayer);
                    Vector3 nextFootPos = hit.point;

                    StartCoroutine(FootMoveAnimation(rightFootTarget, rightFootTargetPos, nextFootPos));
                    rightFootTargetPos = nextFootPos;
                }

                footMiddlePos = (leftFootTargetPos - rightFootTargetPos) / 2f + rightFootTarget.position;
            }
        }
    }

    private IEnumerator FootMoveAnimation(Transform foot, Vector3 start, Vector3 end)
    {
        float percent = 0;
        Vector3 heightPivotVector = new Vector3((end - start).x / 2f + start.x, start.y + moveHeightPivot, (end - start).z / 2f + start.z);
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawLine(leftFootTargetPos, hipToGroundPos + Vector3.right * leftFootOffset.x);
        Gizmos.DrawLine(rightFootTargetPos, hipToGroundPos + Vector3.right * rightFootOffset.x);
        Gizmos.DrawLine(footMiddlePos, hipToGroundPos);
        

        Gizmos.DrawWireSphere(footMiddlePos, shouldMoveDistance);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(leftFootTargetPos, 0.1f);
        Gizmos.DrawSphere(rightFootTargetPos, 0.1f);
        Gizmos.DrawSphere(hipToGroundPos, 0.1f);
    }
}
