using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chicken : MonoBehaviour
{
    [SerializeField] private Transform[] destinations;
    private int currentDestinationIndex;

    private NavMeshAgent nav;
    private Animator anim;

    private void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        currentDestinationIndex = UnityEngine.Random.Range(0, destinations.Length);
        transform.position = destinations[currentDestinationIndex].position;
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

        nav.SetDestination(destinations[currentDestinationIndex].position);
    }
}
