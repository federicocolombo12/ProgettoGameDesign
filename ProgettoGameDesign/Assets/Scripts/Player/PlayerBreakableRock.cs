using System.Collections;
using UnityEngine;

public class PlayerBreakableRock : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerStateList pState;

    [SerializeField] private float chargeSpeed = 10f;
    [SerializeField] private float chargeDuration = 0.5f;

    private bool isCharging = false;
    private Transform targetRock;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pState = GetComponent<PlayerStateList>();
    }

    public void ChargeAndBreak(Transform rockTransform)
    {
        if (Player.Instance.playerTransformation.abilityType == PlayerTransformation.AbilityType.ChargingRockBreaker)
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
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isCharging && collision.transform == targetRock)
        {
            Destroy(targetRock.gameObject);
            isCharging = false;
        }
    }
}
