using TMPro.Examples;
using UnityEngine;

public class AttackAnimatorEvents : MonoBehaviour
{
    [SerializeField] Collider2D attackCollider;
    [SerializeField] ParticleSystem attackParticleSystem;
    [SerializeField] Transform impactTransform;
    [SerializeField] float cameraShakeIntensity = 0.2f;
    private void OnAttackStart()
    {

       attackCollider.enabled = true;
        EffectManager.Instance.PlayOneShot(attackParticleSystem, impactTransform.position);
        CameraManager.Instance.ShakeCamera(cameraShakeIntensity);
    }

    // Update is called once per frame
    private void OnAttackEnd()
    {
        attackCollider.enabled = false;
    }
}
