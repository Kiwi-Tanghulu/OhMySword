using System;
using System.Collections;
using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
	[SerializeField] string[] bgmNames;
    private AudioSource player = null;

    private void Awake()
    {
        player = GetComponent<AudioSource>();
    }

    private void Start()
    {
        PlayBGM();
    }

    private void OnDestroy()
    {
        PauseBGM();
    }

    public void PlayBGM()
    {
        AudioManager.Instance.PlayerAudio(bgmNames.PickRandom(), player);
        DelayCoroutine(player.clip.length, PlayBGM);
    }

    public void PauseBGM()
    {
        player.Pause();
        StopAllCoroutines();
    }

    private IEnumerator DelayCoroutine(float delay, Action callback)
    {
        yield return new WaitForSeconds(delay);
        callback?.Invoke();
    }
}
