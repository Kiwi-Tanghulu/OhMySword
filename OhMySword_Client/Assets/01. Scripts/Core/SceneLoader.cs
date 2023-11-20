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
        //asyncOper.allowSceneActivation = false;

        Scene loadingScene = SceneManager.LoadScene("LoadingScene", new LoadSceneParameters(LoadSceneMode.Additive));
        // ·Îµù ¾À ·Îµå
        yield return new WaitForSeconds(3f);
        while (true)
        {
            if (asyncOper.isDone)
            {
                asyncOper.allowSceneActivation = true;
                break;
            }
            yield return delay;
        }
        yield return new WaitForSeconds(3f);
        SceneManager.UnloadSceneAsync(loadingScene);
        onCompleted?.Invoke();
    }
}
