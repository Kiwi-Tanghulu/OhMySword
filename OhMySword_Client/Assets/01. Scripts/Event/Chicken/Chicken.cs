using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Cinemachine;

public class Chicken : MonoBehaviour, IDamageable
{
    [SerializeField] private Vector3[] destinations;
    private static int currentDestinationIndex = 0;

    [SerializeField] private CinemachineVirtualCamera cam;

    private NavMeshAgent nav;
    private Animator anim;
    private bool isMove = false;
    public bool IsMove
    {
        get
        {
            return isMove;
        }
        set
        {
            isMove = value;
            if (nav != null)
                nav.enabled = isMove;
        }
    }

    private void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        IsMove = false;
        transform.position = destinations[currentDestinationIndex];
    }

    private void Update()
    {
        if(IsMove)
        {
            if (Vector3.Distance(transform.position, nav.destination) <= nav.stoppingDistance)
            {
                SetDestination();
            }
        }
    }

    private void SetDestination()
    {
        currentDestinationIndex = (currentDestinationIndex + 1) % destinations.Length;

        nav.SetDestination(destinations[currentDestinationIndex]);
    }

    public void StartMove()
    {
        IsMove = true;
    }

    public void StopMove()
    {
        IsMove = false;
    }

    public void OnDamage(int damage, GameObject performer, Vector3 point)
    {
        Debug.Log(123);
    }
}
