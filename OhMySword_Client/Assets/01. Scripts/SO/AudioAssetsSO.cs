using UnityEngine;

[CreateAssetMenu(menuName = "SO/AudioClipsSO")]
public class AudioAssetsSO : ScriptableObject
{
    [SerializeField] AudioClip[] clips;

    public int Count => clips.Length;
    public AudioClip this[int index] => clips[index];
}