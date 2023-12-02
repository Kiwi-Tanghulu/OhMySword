using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager
{
	public static AudioManager Instance = null;
    private const float MAX_VOLUME = 0;
    private const float MIN_VOLUME = -40;
    private const float MUTE_VOLUME = -80;

    private Dictionary<string, AudioClip> clips = new Dictionary<string, AudioClip>();
    private AudioMixer audioMixer = null;

    public AudioManager(AudioAssetsSO clipList, AudioMixer audioMixer)
    {
        this.audioMixer = audioMixer;

        for(int i = 0; i < clipList.Count; i++)
            RegisterAudio(clipList[i]);
    }

    public void PlayAudio(string clipName, AudioSource player, bool oneshot = false)
    {
        if(clips.ContainsKey(clipName) == false)
            return;

        if(oneshot)
        {
            player.PlayOneShot(clips[clipName]);
        }
        else
        {
            player.clip = clips[clipName];
            player.Play();
        }
    }

    public void SetVolume(AudioType type, float percentage)
    {
        if(percentage == 0f)
            audioMixer.SetFloat(type.ToString(), MUTE_VOLUME);
        else
            audioMixer.SetFloat(type.ToString(), Mathf.Lerp(MIN_VOLUME, MAX_VOLUME, percentage));
    }

    private void RegisterAudio(AudioClip clip)
    {
        if(clips.ContainsKey(clip.name))
            return;

        clips.Add(clip.name, clip);
        //Debug.Log(clip.name);
        Debug.Log(clips[clip.name]);
    }
}
