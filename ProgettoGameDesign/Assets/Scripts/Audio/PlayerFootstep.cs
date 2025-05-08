using UnityEngine;

public class PlayerFootstep : MonoBehaviour
{
    [SerializeField] SfxData footstepSound;
    public void PlaySound() {         // Play the footstep sound
        AudioManager.Instance.sfxChannel.RaiseEvent(footstepSound);
    }
}
