using UnityEngine;
using System;

[CreateAssetMenu(fileName = "SfxEventChannel", menuName = "Audio/SfxEventChannel")]
public class SfxEventChannel : ScriptableObject
{
    public Action<SfxData> OnSFXPlayRequested;

    public void RaiseEvent(SfxData sfx)
    {
        OnSFXPlayRequested?.Invoke(sfx);
    }
}
