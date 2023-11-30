using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chicken : MonoBehaviour
{
    [SerializeField] private Vector3[] destinations;
    private static int currentDestinationIndex = 0;

    private NavMeshAgent nav;
    private Animator anim;

    private void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        nav.enabled = false;
        transform.position = destinations[currentDestinationIndex];
        nav.enabled = true;
    }

    private void Update()
    {
        Debug.Log(transform.position);
        Debug.Log(nav.destination);
        Debug.Log(Vector3.Distance(transform.position, nav.destination));

        if(Vector3.Distance(transform.position, nav.destination) <= nav.stoppingDistance)
        {
            SetDestination();
        }
    }

    private void SetDestination()
    {
        currentDestinationIndex = (currentDestinationIndex + 1) % destinations.Length;

        nav.SetDestination(destinations[currentDestinationIndex]);
    }
}
