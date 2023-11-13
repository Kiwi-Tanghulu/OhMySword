using UnityEngine;

public class PlayAudioCallback : MonoBehaviour
{
    [SerializeField] string clipName;

	public void PlayAudio(string clipName)
    {
        AudioSource player = UIManager.Instance.UIAudioPlayer;
        if(player == null)
            return;
        
        AudioManager.Instance.PlayerAudio(clipName, player, true);
    }

    public void PlayOneShot(string clipName)
    {
        AudioSource player = UIManager.Instance.UIAudioPlayer;
        if(player == null)
            return;

        AudioManager.Instance.PlayerAudio(clipName, player, true);
    }

    public void PlayAudio()
    {
        AudioSource player = UIManager.Instance.UIAudioPlayer;
        if(player == null)
            return;

        PlayAudio(clipName);
    }

    public void PlayOneShot()
    {
        AudioSource player = UIManager.Instance.UIAudioPlayer;
        if(player == null)
            return;

        PlayOneShot(clipName);
    }
}
