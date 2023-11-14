using System;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
	public static SceneLoader Instance = null;
    
    private float syncDelayTick = 0.5f;

    public void LoadSceneAsync(string sceneName, Action onCompleted = null)
    {
        AsyncOperation asyncOper = SceneManager.LoadSceneAsync(sceneName);
        StartCoroutine(AsyncLoadCoroutine(asyncOper, onCompleted));
    }

    private IEnumerator AsyncLoadCoroutine(AsyncOperation asyncOper, Action onCompleted)
    {
        YieldInstruction delay = new WaitForSeconds(syncDelayTick);

        while(true)
        {
            if(asyncOper.isDone)
                break;

            yield return delay;
        }

        yield return null;

        onCompleted?.Invoke();
    }
}
