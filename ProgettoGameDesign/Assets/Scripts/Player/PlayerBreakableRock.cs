using System.Collections;
using UnityEngine;

public class PlayerBreakableRock : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerStateList pState;

    [SerializeField] private float chargeSpeed = 10f;
    [SerializeField] private float chargeDuration = 0.5f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pState = GetComponent<PlayerStateList>();
    }

    public void ChargeAndBreak(Transform rockTransform)
    {
        if (Player.Instance.playerTransformation.abilityType == PlayerTransformation.AbilityType.ChargingRockBreaker)
        {
            StartCoroutine(ChargeTowardsRock(rockTransform));
        }
        else
        {
            Debug.Log("Non hai la trasformazione corretta per caricare contro questa roccia!");
        }
    }

    private IEnumerator ChargeTowardsRock(Transform target)
    {
        pState.dashing = true; // se vuoi bloccare altri movimenti

        Vector2 direction = (target.position - transform.position).normalized;

        float timer = 0f;
        while (timer < chargeDuration)
        {
            rb.linearVelocity = direction * chargeSpeed;
            timer += Time.deltaTime;
            yield return null;
        }

        rb.linearVelocity = Vector2.zero;
        pState.dashing = false;

        // Dopo aver caricato, distruggi la roccia
        if (Vector2.Distance(transform.position, target.position) < 2f) // distanza tolleranza
        {
            Destroy(target.gameObject);
        }
    }
}
