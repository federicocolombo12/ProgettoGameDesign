using System.Collections;
using System.Linq;
using UnityEngine;

public class PlayerBreakableRock : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerStateList pState;

    [SerializeField] private float chargeSpeed = 10f;
    [SerializeField] private float chargeDuration = 0.5f;
    [SerializeField] private SfxData breakRockSfx;
    [SerializeField] ParticleSystem chargeEffect;
    [SerializeField] ParticleSystem smokeEffect;

    private bool isCharging = false;
    private Transform targetRock;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pState = GetComponent<PlayerStateList>();
    }

    public void ChargeAndBreak(Transform rockTransform)
    {
        if (Player.Instance.playerTransformation.abilities.Contains(PlayerTransformation.AbilityType.ChargingRockBreaker))
        {
            targetRock = rockTransform;
            StartCoroutine(ChargeTowardsRock());
        }
        else
        {
            Debug.Log("Non hai la trasformazione corretta per caricare contro questa roccia!");
        }
    }

    private IEnumerator ChargeTowardsRock()
    {
        pState.dashing = true;
        isCharging = true;

        
            Player.Instance.animator.SetBool("SpAbility", true);
        

        AudioManager.Instance.sfxChannel.RaiseEvent(breakRockSfx, true);
        CameraManager.Instance.ShakeCamera(0.3f);

        Vector2 direction = (targetRock.position - transform.position).normalized;

        float timer = 0f;
        while (timer < chargeDuration)
        {
            rb.linearVelocity = direction * chargeSpeed;
            timer += Time.deltaTime;
            yield return null;
        }

        rb.linearVelocity = Vector2.zero;
        pState.dashing = false;
        isCharging = false;

            Player.Instance.animator.SetBool("SpAbility", false);
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isCharging && collision.transform == targetRock)
        {
            
            EffectManager.Instance.PlayOneShot(chargeEffect, targetRock.transform.position);
            EffectManager.Instance.PlayOneShot(smokeEffect, targetRock.transform.position);
            Destroy(targetRock.gameObject);
            isCharging = false;
        }
    }
}
