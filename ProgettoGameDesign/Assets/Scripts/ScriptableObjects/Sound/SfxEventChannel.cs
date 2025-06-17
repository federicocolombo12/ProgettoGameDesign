using UnityEngine;
using System;

[CreateAssetMenu(fileName = "SfxEventChannel", menuName = "Audio/SfxEventChannel")]
public class SfxEventChannel : ScriptableObject
{
    public Action<SfxData, bool> OnSFXPlayRequested;

    public void RaiseEvent(SfxData sfx, bool randomizePitch)
    {
        OnSFXPlayRequested?.Invoke(sfx, randomizePitch);
    }
}
