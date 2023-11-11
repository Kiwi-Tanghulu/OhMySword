using System.Collections.Generic;
using UnityEngine;

public class AudioManager
{
	public static AudioManager Instance = null;

    private Dictionary<string, AudioClip> clips = new Dictionary<string, AudioClip>();

    public AudioManager(AudioClipsSO clipList)
    {
        for(int i = 0; i < clipList.Count; i++)
            RegisterAudio(clipList[i]);
    }

    public void PlayerAudio(string clipName, AudioSource player, bool oneshot = false)
    {
        if(clips.ContainsKey(clipName) == false)
            return;

        if(oneshot)
            player.PlayOneShot(player.clip = clips[clipName]);
        else
        {
            player.clip = clips[clipName];
            player.Play();
        }
    }

    private void RegisterAudio(AudioClip clip)
    {
        if(clips.ContainsKey(clip.name))
            return;

        clips.Add(clip.name, clip);
    }
}
