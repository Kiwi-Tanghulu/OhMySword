using Cinemachine;
using UnityEngine;

public class TPriority : MonoBehaviour
{
	private void Start()
    {
        GetComponent<CinemachineVirtualCamera>().Priority = 15;
    }
}
