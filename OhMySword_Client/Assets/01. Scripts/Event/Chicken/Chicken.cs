using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Cinemachine;
using Base.Network;
using Packets;

public class Chicken : MonoBehaviour, IDamageable
{
    [SerializeField] private Vector3[] destinations;
    private int currentDestinationIndex = 0;

    public CinemachineVirtualCamera cam;
    public float takeCameraTime = 2f;

    private AudioSource aud;
    private NavMeshAgent nav;

    private bool isInit = false;

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

    public void Init(int param)
    {
        nav = GetComponent<NavMeshAgent>();
        aud = GetComponent<AudioSource>();

        IsMove = false;
        currentDestinationIndex = param;
        transform.position = destinations[currentDestinationIndex];
        CameraManager.Instance.SetActiveCamTemporarily(cam, takeCameraTime);
        AudioManager.Instance.PlayAudio("Chicken", aud, true);
        isInit = true;
    }

    private void Update()
    {
        if (!isInit)
            return;

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
        SyncableObject attacker = performer.GetComponent<SyncableObject>();
        VectorPacket position = new VectorPacket(transform.position.x, transform.position.y, transform.position.z);
        C_ChickenHitPacket attackPacket = new C_ChickenHitPacket(attacker.ObjectID, position);

        NetworkManager.Instance.Send(attackPacket);
    }
}
