using UnityEngine;
using System;

[CreateAssetMenu(fileName = "MusicEventChannel", menuName = "Scriptable Objects/MusicEventChannel")]
public class MusicEventChannel : ScriptableObject
{
    public Action<AudioClip, float> OnMusicChangeRequested;

    public void RaiseEvent(AudioClip newClip, float fadeDuration = 1f)
    {
        OnMusicChangeRequested?.Invoke(newClip, fadeDuration);
    }
}
