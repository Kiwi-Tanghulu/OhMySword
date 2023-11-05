using System.Collections;
using UnityEngine;

public class RagdollMove : MonoBehaviour
{
    //�̵�
    public float moveSpeed;
    public Rigidbody hipRigid;
    public Vector3 moveDir;

    //�ٸ� ������
    public Transform leftFootTarget;
    public Transform rightFootTarget;
    public Transform hip;
    public float shouldMoveDistance; //�� ���� �߰� ��ġ�� ���� ��ġ�� �� �� �̻� ���̳��� �� �̵�
    public float moveDistance; //���� �̵��ϴ� �Ÿ�
    public LayerMask groundLayer;
    public float moveHeightPivot;
    public float footMoveTime;
    private Vector3 leftFootTargetPos; // LeftFootIk�� Target�� ��ġ
    private Vector3 rightFootTargetPos; //RightFootIk�� Target�� ��ġ
    private Vector3 footMiddlePos; //�� �� ������ �߰� ��ġ
    //x, z�� ���
    private Vector3 rightFootOffset; //������ ���� ��ġ�� �� �� �߰� ��ġ�� ����
    private Vector3 leftFootOffset; //���� ���� ��ġ�� �� �� �߰� ��ġ�� ����
    private Vector3 hipToGroundPos; // hip���� �ٴ����� ���̸� �� ������ ��ġ  

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
