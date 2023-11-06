using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private ActiveRagdoll rag;

    private Vector3 prevTargetPos;
    private Vector3 targetPos;
    private Vector3 moveDir;
    [SerializeField] private float moveSpeed;

    private void Start()
    {
        rag = GetComponent<ActiveRagdoll>();
        targetPos = transform.position;
        prevTargetPos = targetPos;
    }

    private void Update()
    {
        
    }

    public void SetTargetPosition(Vector3 pos)
    {
        prevTargetPos = targetPos;
        targetPos = pos;
        moveDir = (targetPos - prevTargetPos).normalized;
    }
}
