using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMove : MonoBehaviour
{
    public Rigidbody hip;
    public float moveSpeed = 5f;
    public Vector3 moveDir;

    [Space]
    public LayerMask groundLayer;
    public Transform leftFoot;
    public Transform rightFoot;
    public Transform hips;
    public float shouldMoveDistance = 0.5f;
    [Tooltip("shouldMoveDistance보다 작아야 함")]
    public float moveDistance = 0.5f;
    public float moveHeight = 0.3f;
    public float footMoveTime = 1f;

    //where feet should be
    private Vector3 leftFootPos;
    private Vector3 rightFootPos;
    private Vector3 prevLeftFootPos;
    private Vector3 prevRightFootPos;

    //The location where the ray was fired from the body and reached the ground.
    private Vector3 leftHitPos;
    private Vector3 rightHitPos;

    //Distance between body and feet, Only used x and y
    private Vector3 leftFootOffset;
    private Vector3 rightFootOffset;

    private void Start()
    {
        //leftFootPos = leftFoot.position;
        //rightFootPos = rightFoot.position;
        //prevLeftFootPos = leftFootPos;
        //prevRightFootPos = rightFootPos;
        //leftFootOffset = leftFoot.position - hips.position;
        //rightFootOffset = rightFoot.position - hips.position;
    }

    private void Update()
    {
        Move();
        //FootMove();
    }

    private void Move()
    {
        moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        hip.AddForce(moveDir * moveSpeed); //+= moveDir * moveSpeed * Time.deltaTime;
    }

    private void FootMove()
    {
        leftFoot.position = leftFootPos;
        rightFoot.position = rightFootPos;

        RaycastHit lefttHit = default;
        RaycastHit righttHit = default;

        if (Physics.Raycast(new Vector3(hips.position.x + leftFootOffset.x, hips.position.y, hips.position.z + leftFootOffset.z),
            Vector3.down, out lefttHit, 10, groundLayer))
        {                                 //foot toe distance
            leftHitPos = lefttHit.point + Vector3.up * 0.2f;
            //left, right distance
            if (Vector3.Distance(leftHitPos, leftFootPos) >= shouldMoveDistance * 1.7f)
            {
                Vector3 footMoveDir = (leftHitPos - leftFootPos).normalized;
                prevLeftFootPos = leftFootPos;
                leftFootPos = new Vector3(leftHitPos.x + footMoveDir.x * moveDistance, leftHitPos.y, leftHitPos.z + footMoveDir.z * moveDistance);
                StartCoroutine(FootMoveAnimation(leftFoot, leftFootPos, prevLeftFootPos));
            }
        }

        if (Physics.Raycast(new Vector3(hips.position.x + rightFootOffset.x, hips.position.y, hips.position.z + rightFootOffset.z),
            Vector3.down, out righttHit, 10, groundLayer))
        {                                   //foot toe distance
            rightHitPos = righttHit.point + Vector3.up * 0.2f;

            if (Vector3.Distance(rightHitPos, rightFootPos) >= shouldMoveDistance)
            {
                Vector3 footMoveDir = (rightHitPos - rightFootPos).normalized;
                prevRightFootPos = rightFootPos;
                rightFootPos = new Vector3(rightHitPos.x + footMoveDir.x * moveDistance, rightHitPos.y, rightHitPos.z + footMoveDir.z * moveDistance);
                StartCoroutine(FootMoveAnimation(rightFoot, rightFootPos, prevRightFootPos));
            }
        }
    }

    private IEnumerator FootMoveAnimation(Transform foot, Vector3 next, Vector3 prev)
    {
        float percent = 0;

        while (percent <= 1)
        {
            percent += Time.deltaTime / footMoveTime;

            foot.position = Vector3.Slerp(prev, next, percent);

            yield return null;
        }
    }
}