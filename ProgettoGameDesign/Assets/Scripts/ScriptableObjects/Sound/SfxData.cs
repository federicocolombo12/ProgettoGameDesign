using UnityEngine;

[CreateAssetMenu(fileName = "SfxData", menuName = "Audio/SfxData")]
public class SfxData : ScriptableObject
{
    public AudioClip clip;
    public float volume = 1f;
    public float pitch = 1f;
}
