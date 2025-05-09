using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    Collider2D attackCollider;
    [SerializeField] ParticleSystem attackParticleSystem;
    Transform impactTransform;
    [SerializeField] float cameraShakeIntensity = 0.2f;

    void Start()
    {
        attackCollider = GetComponent<Collider2D>();
        impactTransform = transform;
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CameraManager.Instance.ShakeCamera(cameraShakeIntensity);
            EffectManager.Instance.PlayOneShot(attackParticleSystem, impactTransform.position);
            Player.Instance.playerHealth.TakeDamage(1);
        }
    }
}
