using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UIElements;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    [SerializeField] private int activeCamPriority;
    [SerializeField] private int deactiveCamPriority;

    [SerializeField] private CinemachineVirtualCamera activeCam;
    [SerializeField] private CinemachineVirtualCamera beforeCam;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void SetActiveCam(CinemachineVirtualCamera cam)
    {
        if(activeCam != null)
        {
            activeCam.Priority = deactiveCamPriority;
            beforeCam = activeCam;
        }
        activeCam = cam;
        activeCam.Priority = activeCamPriority;
    }

    public void SetActiveCamTemporarily(CinemachineVirtualCamera cam, float time)
    {
        StopAllCoroutines();
        StartCoroutine(SetActiveCamTemporarilyCo(cam, time));
    }

    private IEnumerator SetActiveCamTemporarilyCo(CinemachineVirtualCamera cam, float time)
    {
        SetActiveCam(cam);
        Debug.Log(time);
        yield return new WaitForSeconds(time);


        if (beforeCam == null)
            yield break;

        SetActiveCam(beforeCam);
    }
}
